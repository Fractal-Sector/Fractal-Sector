using Content.Server.Chemistry.Containers.EntitySystems;
using Content.Server.Power.EntitySystems;
using Content.Shared._FS.Petroleum;
using Content.Shared._Starlight.Plumbing;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.FixedPoint;
using Content.Shared.Popups;
using Content.Shared.Verbs;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using System;

namespace Content.Server._FS.Petroleum;

public sealed class ReactiveColumnSystem : EntitySystem
{
    [Dependency] private readonly SolutionContainerSystem _solution = default!;
    [Dependency] private readonly PowerReceiverSystem _power = default!;
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] private readonly SharedMapSystem _map = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ReactiveColumnComponent, AnchorStateChangedEvent>(OnMasterAnchor);
        SubscribeLocalEvent<ReactiveColumnComponent, ComponentShutdown>(OnMasterShutdown);
        SubscribeLocalEvent<ReactiveColumnPortComponent, AnchorStateChangedEvent>(OnPortAnchor);
        SubscribeLocalEvent<ReactiveColumnPortComponent, ComponentShutdown>(OnPortShutdown);
    }

    private void OnMasterAnchor(EntityUid uid, ReactiveColumnComponent comp, ref AnchorStateChangedEvent args)
    {
        if (!args.Anchored)
        {
            Unlink(uid, comp);
            return;
        }

        if (!TryGetTile(uid, out var gUid, out var grid, out var masterTile))
            return;

        var portTile = masterTile + comp.PortOffset;
        foreach (var candidate in _map.GetAnchoredEntities(gUid, grid, portTile))
        {
            if (!TryComp<ReactiveColumnPortComponent>(candidate, out var port))
                continue;
            if (port.Master != null)
                continue;

            DoLink(uid, comp, candidate, port);
            break;
        }
    }

    private void OnPortAnchor(EntityUid uid, ReactiveColumnPortComponent port, ref AnchorStateChangedEvent args)
    {
        if (!args.Anchored)
        {
            if (port.Master is { } m && TryComp<ReactiveColumnComponent>(m, out var mc))
            {
                mc.Port = null;
                Dirty(m, mc);
            }
            port.Master = null;

            Dirty(uid, port);
            return;
        }

        if (!TryGetTile(uid, out var gUid, out var grid, out var portTile))
            return;

        for (var dx = -2; dx <= 2; dx++)
        for (var dy = -2; dy <= 2; dy++)
        {
            foreach (var candidate in _map.GetAnchoredEntities(gUid, grid, portTile + new Vector2i(dx, dy)))
            {
                if (!TryComp<ReactiveColumnComponent>(candidate, out var mc))
                    continue;

                if (mc.Port != null)
                    continue;

                var masterTile = _map.TileIndicesFor(gUid, grid, Transform(candidate).Coordinates);
                if (masterTile + mc.PortOffset != portTile)
                    continue;

                DoLink(candidate, mc, uid, port);
                return;
            }
        }
    }

    private void OnMasterShutdown(EntityUid uid, ReactiveColumnComponent comp, ComponentShutdown args)
        => Unlink(uid, comp);

    private void OnPortShutdown(EntityUid uid, ReactiveColumnPortComponent port, ComponentShutdown args)
    {
        if (port.Master is { } m && TryComp<ReactiveColumnComponent>(m, out var mc))
        {
            mc.Port = null;
            Dirty(m, mc);
        }
    }

    private void DoLink(EntityUid mUid, ReactiveColumnComponent mc, EntityUid pUid, ReactiveColumnPortComponent port)
    {
        mc.Port = pUid;
        Dirty(mUid, mc);

        port.Master = mUid;
        Dirty(pUid, port);
    }

    private void Unlink(EntityUid mUid, ReactiveColumnComponent mc)
    {
        if (mc.Port is { } pUid && TryComp<ReactiveColumnPortComponent>(pUid, out var port))
        {
            port.Master = null;
            Dirty(pUid, port);
        }

        mc.Port = null;
        Dirty(mUid, mc);
    }

    private bool TryGetTile(EntityUid uid, out EntityUid gridUid, out MapGridComponent grid, out Vector2i tile)
    {
        gridUid = default;
        grid = default!;
        tile = default;

        var xform = Transform(uid);

        if (xform.GridUid is not { } g)
            return false;

        if (!TryComp(g, out MapGridComponent? gc))
            return false;

        gridUid = g;
        grid = gc;

        tile = _map.TileIndicesFor(g, gc, xform.Coordinates);
        return true;
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<ReactiveColumnComponent>();
        while (query.MoveNext(out var uid, out var comp))
        {
            if (!_power.IsPowered(uid))
            {
                SetRunning(uid, false);
                continue;
            }
            SetRunning(uid, TryProcess(uid, comp, frameTime));
        }
    }

    private void SetRunning(EntityUid uid, bool running)
        => _appearance.SetData(uid, PlumbingVisuals.Running, running);

    private bool TryProcess(EntityUid uid, ReactiveColumnComponent comp, float frameTime)
    {
        if (!_solution.TryGetSolution(uid, comp.InputSolution, out var inputHolder, out var inputSol))
            return false;

        if (inputSol.Volume <= FixedPoint2.Zero)
            return false;

        ReactiveColumnRecipe? recipe = null;
        ReagentId inputReagent = default;

        foreach (var r in comp.Recipes)
        {
            var rid = new ReagentId(r.Input, null);
            if (!inputSol.ContainsReagent(rid))
                continue;

            if (inputSol.Temperature < r.MinTemp)
                continue;

            recipe = r;
            inputReagent = rid;
            break;
        }

        if (recipe == null)
            return false;

        var available = (float) inputSol.GetReagentQuantity(inputReagent);
        if (available <= 0f)
            return false;

        if (!_solution.TryGetSolution(uid, comp.Output1Solution, out var out1Holder, out var out1Sol))
            return false;

        Entity<SolutionComponent>? out2Holder = null;
        FixedPoint2? out2Room = null;

        if (comp.Port is { } portUid &&
            TryComp<ReactiveColumnPortComponent>(portUid, out var portComp) &&
            _solution.TryGetSolution(portUid, portComp.Output2Solution, out var oh, out var out2Sol))
        {
            out2Holder = oh;
            out2Room   = out2Sol.AvailableVolume;
        }

        var toProcess = MathF.Min(comp.ProcessRate * frameTime, available);
        var amt1 = toProcess * recipe.Output1Fraction;
        var amt2 = toProcess * recipe.Output2Fraction;

        var scale = FitScale(out1Sol.AvailableVolume, amt1);
        if (out2Room is { } r2 && out2Holder != null)
            scale = MathF.Min(scale, FitScale(r2, amt2));

        if (scale <= 0f)
            return false;

        toProcess *= scale;
        amt1 *= scale;
        amt2 *= scale;

        _solution.RemoveReagent(inputHolder.Value, inputReagent, FixedPoint2.New(toProcess));
        _solution.TryAddReagent(out1Holder.Value, recipe.Output1, FixedPoint2.New(amt1), out _, null, null);

        if (out2Holder is { } validOut2)
            _solution.TryAddReagent(validOut2, recipe.Output2, FixedPoint2.New(amt2), out _, null, null);

        return true;
    }

    private static float FitScale(FixedPoint2 available, float wanted)
    {
        if (wanted <= 0f)
            return 1f;

        var a = available.Float();
        return a >= wanted ? 1f : MathF.Max(0f, a / wanted);
    }
}
