using Content.Server.Atmos.EntitySystems;
using Content.Server.Atmos.Piping.Components;
using Content.Server.Chemistry.Containers.EntitySystems;
using Content.Server.NodeContainer.EntitySystems;
using Content.Server.NodeContainer.Nodes;
using Content.Server.Power.EntitySystems;
using Content.Shared._FS.Petroleum;
using Content.Shared._Starlight.Plumbing;
using Content.Shared.Atmos;
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

public sealed class OilRefinerySystem : EntitySystem
{
    [Dependency] private readonly SolutionContainerSystem _solution = default!;
    [Dependency] private readonly PowerReceiverSystem _power = default!;
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] private readonly SharedMapSystem _map = default!;
    [Dependency] private readonly AtmosphereSystem _atmos = default!;
    [Dependency] private readonly NodeContainerSystem _nodes = default!;

    private static readonly ReagentId CrudeOil = new("CrudeOil", null);

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<OilRefineryComponent, AnchorStateChangedEvent>(OnMasterAnchor);
        SubscribeLocalEvent<OilRefineryComponent, ComponentShutdown>(OnMasterShutdown);
        SubscribeLocalEvent<OilRefineryComponent, GetVerbsEvent<AlternativeVerb>>(OnMasterVerbs);

        SubscribeLocalEvent<OilRefineryPartComponent, AnchorStateChangedEvent>(OnPartAnchor);
        SubscribeLocalEvent<OilRefineryPartComponent, ComponentShutdown>(OnPartShutdown);
        SubscribeLocalEvent<OilRefineryPartComponent, GetVerbsEvent<AlternativeVerb>>(OnPartVerbs);

        SubscribeLocalEvent<OilRefineryGasOutletComponent, AtmosDeviceUpdateEvent>(OnGasAtmosTick);
    }

    private void OnMasterAnchor(EntityUid uid, OilRefineryComponent comp, ref AnchorStateChangedEvent args)
    {
        if (!args.Anchored) { UnlinkAll(uid, comp); return; }
        if (!TryGetTile(uid, out var gUid, out var grid, out var masterTile)) return;

        foreach (OilRefineryPartType type in Enum.GetValues<OilRefineryPartType>())
        {
            if (GetCachedRef(comp, type) != null) continue;

            var targetTile = masterTile + GetOffset(comp, type);
            foreach (var candidate in _map.GetAnchoredEntities(gUid, grid, targetTile))
            {
                if (!TryComp<OilRefineryPartComponent>(candidate, out var part)) continue;
                if (part.PartType != type || part.Master != null) continue;
                Link(uid, comp, candidate, part);
                break;
            }
        }
    }

    private void OnPartAnchor(EntityUid uid, OilRefineryPartComponent part, ref AnchorStateChangedEvent args)
    {
        if (!args.Anchored)
        {
            if (part.Master is { } m && TryComp<OilRefineryComponent>(m, out var mc))
            {
                SetCachedRef(mc, part.PartType, null);
                Dirty(m, mc);
            }
            part.Master = null;
            Dirty(uid, part);
            return;
        }

        if (!TryGetTile(uid, out var gUid, out var grid, out var partTile)) return;

        // Ищем мастера в 3x3 вокруг этого модуля, потом проверяем что он
        // действительно ждёт наш тип именно на нашем тайле.
        for (var dx = -2; dx <= 2; dx++)
        for (var dy = -2; dy <= 2; dy++)
        {
            var checkTile = partTile + new Vector2i(dx, dy);
            foreach (var candidate in _map.GetAnchoredEntities(gUid, grid, checkTile))
            {
                if (!TryComp<OilRefineryComponent>(candidate, out var mc)) continue;
                if (GetCachedRef(mc, part.PartType) != null) continue;

                var mTile = _map.TileIndicesFor(gUid, grid, Transform(candidate).Coordinates);
                if (mTile + GetOffset(mc, part.PartType) != partTile) continue;

                Link(candidate, mc, uid, part);
                return;
            }
        }
    }

    private void OnMasterShutdown(EntityUid uid, OilRefineryComponent comp, ComponentShutdown args)
        => UnlinkAll(uid, comp);

    private void OnPartShutdown(EntityUid uid, OilRefineryPartComponent part, ComponentShutdown args)
    {
        if (part.Master is { } m && TryComp<OilRefineryComponent>(m, out var mc))
        {
            SetCachedRef(mc, part.PartType, null);
            Dirty(m, mc);
        }
    }

    private void Link(EntityUid mUid, OilRefineryComponent mc, EntityUid pUid, OilRefineryPartComponent part)
    {
        SetCachedRef(mc, part.PartType, pUid);
        Dirty(mUid, mc);
        part.Master = mUid;
        Dirty(pUid, part);
    }

    private void UnlinkAll(EntityUid mUid, OilRefineryComponent mc)
    {
        foreach (OilRefineryPartType type in Enum.GetValues<OilRefineryPartType>())
        {
            if (GetCachedRef(mc, type) is not { } pUid) continue;
            if (TryComp<OilRefineryPartComponent>(pUid, out var part))
            {
                part.Master = null;
                Dirty(pUid, part);
            }
            SetCachedRef(mc, type, null);
        }
        Dirty(mUid, mc);
    }

    private static Vector2i GetOffset(OilRefineryComponent c, OilRefineryPartType t) => t switch
    {
        OilRefineryPartType.Input => c.InputOffset,
        OilRefineryPartType.Naphtha => c.NaphthaOffset,
        OilRefineryPartType.Light => c.LightOffset,
        OilRefineryPartType.Heavy => c.HeavyOffset,
        OilRefineryPartType.Gas => c.GasOffset,
        _ => Vector2i.Zero,
    };

    private static EntityUid? GetCachedRef(OilRefineryComponent c, OilRefineryPartType t) => t switch
    {
        OilRefineryPartType.Input => c.InputPart,
        OilRefineryPartType.Naphtha => c.NaphthaPart,
        OilRefineryPartType.Light => c.LightPart,
        OilRefineryPartType.Heavy => c.HeavyPart,
        OilRefineryPartType.Gas => c.GasPart,
        _ => null,
    };

    private static void SetCachedRef(OilRefineryComponent c, OilRefineryPartType t, EntityUid? v)
    {
        switch (t)
        {
            case OilRefineryPartType.Input:
                c.InputPart = v;
                break;
            case OilRefineryPartType.Naphtha:
                c.NaphthaPart = v;
                break;
            case OilRefineryPartType.Light:
                c.LightPart = v;
                break;
            case OilRefineryPartType.Heavy:
                c.HeavyPart = v;
                break;
            case OilRefineryPartType.Gas:
                c.GasPart = v;
                break;
        }
    }

    private bool TryGetTile(EntityUid uid, out EntityUid gridUid, out MapGridComponent grid, out Vector2i tile)
    {
        gridUid = default;
        grid = default!;
        tile = default;

        var xform = Transform(uid);

        if (xform.GridUid is not { } g) return false;
        if (!TryComp(g, out MapGridComponent? gc)) return false;

        gridUid = g;
        grid = gc;
        tile = _map.TileIndicesFor(g, gc, xform.Coordinates);

        return true;
    }

    private void OnMasterVerbs(EntityUid uid, OilRefineryComponent comp, GetVerbsEvent<AlternativeVerb> args)
        => AddCleanVerb(uid, comp, args);

    private void OnPartVerbs(EntityUid uid, OilRefineryPartComponent part, GetVerbsEvent<AlternativeVerb> args)
    {
        if (part.Master is { } m && TryComp<OilRefineryComponent>(m, out var mc))
            AddCleanVerb(m, mc, args);
    }

    private void AddCleanVerb(EntityUid masterUid, OilRefineryComponent comp, GetVerbsEvent<AlternativeVerb> args)
    {
        if (!args.CanAccess || !args.CanInteract) return;

        var gunk = (int) MathF.Floor(comp.SulfurGunk);

        args.Verbs.Add(new AlternativeVerb
        {
            Text = $"Очистить серные фильтры ({gunk}/{(int) comp.MaxSulfurGunk})",
            Act = () =>
            {
                var stacks = (int) MathF.Floor(comp.SulfurGunk / 20f);
                var coords = _transform.GetMoverCoordinates(masterUid);

                for (var i = 0; i < stacks; i++)
                    Spawn("SheetSulfur1", coords);

                comp.SulfurGunk = 0f;
                Dirty(masterUid, comp);
                _popup.PopupEntity("Вы вычистили серный шлам из фильтров НПЗ.", masterUid, args.User);
            },
        });
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<OilRefineryComponent>();
        while (query.MoveNext(out var uid, out var comp))
        {
            if (comp.InputPart == null)
            {
                SetRunning(uid, false);
                continue;
            }

            if (comp.SulfurGunk >= comp.MaxSulfurGunk)
            {
                SetRunning(uid, false);
                continue;
            }

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

    private bool TryProcess(EntityUid masterUid, OilRefineryComponent comp, float frameTime)
    {
        if (comp.InputPart is not { } inputUid) return false;

        if (!_solution.TryGetSolution(inputUid, "crude", out var inputHolder, out var inputSol))
            return false;

        if (!inputSol.ContainsReagent(CrudeOil)) return false;

        if (inputSol.Temperature < comp.MinProcessTemp) return false;

        var available = (float) inputSol.GetReagentQuantity(CrudeOil);
        if (available <= 0f) return false;

        var toProcess = MathF.Min(comp.ProcessRate * frameTime, available);

        var naphthaAmt = toProcess * 0.40f;
        var lightAmt = toProcess * 0.30f;
        var heavyAmt = toProcess * 0.20f;
        var gasAmt = toProcess * 0.10f;

        Entity<SolutionComponent>? naphthaHolder = null;
        Entity<SolutionComponent>? lightHolder   = null;
        Entity<SolutionComponent>? heavyHolder   = null;

        var scale = 1f;

        if (comp.NaphthaPart is { } naphthaUid &&
            _solution.TryGetSolution(naphthaUid, "output", out var nh, out var nSol))
        {
            naphthaHolder = nh;
            scale = MathF.Min(scale, FitScale(nSol.AvailableVolume, naphthaAmt));
        }

        if (comp.LightPart is { } lightUid &&
            _solution.TryGetSolution(lightUid, "output", out var lh, out var lSol))
        {
            lightHolder = lh;
            scale = MathF.Min(scale, FitScale(lSol.AvailableVolume, lightAmt));
        }

        if (comp.HeavyPart is { } heavyUid &&
            _solution.TryGetSolution(heavyUid, "output", out var hh, out var hSol))
        {
            heavyHolder = hh;
            scale = MathF.Min(scale, FitScale(hSol.AvailableVolume, heavyAmt));
        }

        if (scale <= 0f)
            return false;

        toProcess *= scale;
        naphthaAmt *= scale;
        lightAmt *= scale;
        heavyAmt *= scale;
        gasAmt *= scale;

        _solution.RemoveReagent(inputHolder.Value, CrudeOil, FixedPoint2.New(toProcess));

        if (naphthaHolder is { } nHolder)
            _solution.TryAddReagent(nHolder, "Naphtha", FixedPoint2.New(naphthaAmt), out _, null, null);

        if (lightHolder is { } lHolder)
            _solution.TryAddReagent(lHolder, "LightOil", FixedPoint2.New(lightAmt), out _, null, null);

        if (heavyHolder is { } hHolder)
            _solution.TryAddReagent(hHolder, "HeavyOil", FixedPoint2.New(heavyAmt), out _, null, null);

        if (comp.GasPart is { } gasUid &&
            TryComp<OilRefineryGasOutletComponent>(gasUid, out var gasOutlet))
        {
            gasOutlet.PendingMoles += gasAmt * comp.GasMolesPerUnit;
        }

        comp.SulfurGunk = MathF.Min(comp.MaxSulfurGunk, comp.SulfurGunk + toProcess * 0.05f);
        Dirty(masterUid, comp);

        return true;
    }

    /// <summary>
    /// Вызывается AtmosDeviceUpdateEvent (не каждый игровой тик, а по расписанию атмоса).
    /// Сливает накопленные моли из PendingMoles прямо в GasMixture подключённой трубы.
    /// </summary>
    private void OnGasAtmosTick(EntityUid uid, OilRefineryGasOutletComponent outlet, ref AtmosDeviceUpdateEvent args)
    {
        if (outlet.PendingMoles <= 0f)
            return;

        if (!_nodes.TryGetNode<PipeNode>(uid, outlet.PipeNodeName, out var pipe))
            return;

        var toRelease = new GasMixture(volume: 1f) { Temperature = outlet.GasReleaseTemp };
        toRelease.AdjustMoles(outlet.GasType, outlet.PendingMoles);
        _atmos.Merge(pipe.Air, toRelease);

        outlet.PendingMoles = 0f;
    }

    /// <summary>
    /// Возвращает, какую долю от wanted мы можем положить при наличии available места.
    /// 1.0 если всё влезает, 0..1 если места мало, 0 если места нет совсем.
    /// </summary>
    private static float FitScale(FixedPoint2 available, float wanted)
    {
        if (wanted <= 0f) return 1f;
        var a = available.Float();
        return a >= wanted ? 1f : MathF.Max(0f, a / wanted);
    }
}
