using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.FixedPoint;
using Content.Shared._FS.Petroleum;
using Content.Server.Chemistry.Containers.EntitySystems;
using Content.Server.Power.EntitySystems;
using Content.Shared._Starlight.Plumbing;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.Verbs;
using Content.Shared.Popups;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using System;

namespace Content.Server._FS.Petroleum;

public sealed class OilRefinerySystem : EntitySystem
{
    [Dependency] private readonly SolutionContainerSystem _solutionContainer = default!;
    [Dependency] private readonly PowerReceiverSystem _powerReceiver = default!;
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] private readonly SharedMapSystem _mapSystem = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<OilRefineryComponent, GetVerbsEvent<AlternativeVerb>>(AddRefineryVerbs);
        SubscribeLocalEvent<OilRefineryComponent, AnchorStateChangedEvent>(OnMasterAnchorChanged);
        SubscribeLocalEvent<OilRefineryComponent, ComponentRemove>(OnMasterRemoved);

        SubscribeLocalEvent<OilRefineryPartComponent, GetVerbsEvent<AlternativeVerb>>(AddPartVerbs);
        SubscribeLocalEvent<OilRefineryPartComponent, AnchorStateChangedEvent>(OnPartAnchorChanged);
        SubscribeLocalEvent<OilRefineryPartComponent, ComponentRemove>(OnPartRemoved);
    }

    // -------------------------------------------------------------------
    // Linking: run ONCE when a master or a part anchors, by exact tile
    // offset. No range lookups, no chance of grabbing the wrong refinery's
    // parts if two are built next to each other.
    // -------------------------------------------------------------------

    private void OnMasterAnchorChanged(EntityUid uid, OilRefineryComponent refinery, ref AnchorStateChangedEvent args)
    {
        if (!args.Anchored)
        {
            UnlinkAll(uid, refinery);
            return;
        }

        TryLinkMaster(uid, refinery);
    }

    private void OnPartAnchorChanged(EntityUid uid, OilRefineryPartComponent part, ref AnchorStateChangedEvent args)
    {
        if (!args.Anchored)
        {
            if (part.Master is { } oldMaster && TryComp<OilRefineryComponent>(oldMaster, out var oldRefinery))
                UnlinkPart(oldMaster, oldRefinery, part.PartType);
            part.Master = null;
            Dirty(uid, part);
            return;
        }

        // A part anchoring after its master was already placed - check the tile
        // directly opposite this part's expected offset for a master.
        if (!TryComp<TransformComponent>(uid, out var xform) || xform.GridUid is not { } gridUid)
            return;

        if (!TryComp<MapGridComponent>(gridUid, out var grid))
            return;

        var partTile = _mapSystem.TileIndicesFor(gridUid, grid, xform.Coordinates);

        foreach (var candidate in _mapSystem.GetAnchoredEntities(gridUid, grid, partTile))
        {
            if (!TryComp<OilRefineryComponent>(candidate, out var refinery))
                continue;

            if (!TryComp<TransformComponent>(candidate, out var masterXform))
                continue;

            var masterTile = _mapSystem.TileIndicesFor(gridUid, grid, masterXform.Coordinates);
            var offset = partTile - masterTile;

            if (OffsetMatches(refinery, part.PartType, offset))
            {
                LinkPart(candidate, refinery, uid, part);
                return;
            }
        }
    }

    private void TryLinkMaster(EntityUid uid, OilRefineryComponent refinery)
    {
        if (!TryComp<TransformComponent>(uid, out var xform) || xform.GridUid is not { } gridUid)
            return;

        if (!TryComp<MapGridComponent>(gridUid, out var grid))
            return;

        var masterTile = _mapSystem.TileIndicesFor(gridUid, grid, xform.Coordinates);

        LinkAtOffset(gridUid, grid, masterTile, refinery.NorthOffset, "north", uid, refinery);
        LinkAtOffset(gridUid, grid, masterTile, refinery.EastOffset, "east", uid, refinery);
        LinkAtOffset(gridUid, grid, masterTile, refinery.GasOffset, "gas", uid, refinery);

        UpdateAssembledState(uid, refinery);
    }

    private void LinkAtOffset(EntityUid gridUid, MapGridComponent grid, Vector2i masterTile, Vector2i offset, string partType,
        EntityUid masterUid, OilRefineryComponent refinery)
    {
        foreach (var candidate in _mapSystem.GetAnchoredEntities(gridUid, grid, masterTile + offset))
        {
            if (TryComp<OilRefineryPartComponent>(candidate, out var part) && part.PartType == partType)
            {
                LinkPart(masterUid, refinery, candidate, part);
                return;
            }
        }
    }

    private void LinkPart(EntityUid masterUid, OilRefineryComponent refinery, EntityUid partUid, OilRefineryPartComponent part)
    {
        part.Master = masterUid;
        Dirty(partUid, part);

        switch (part.PartType)
        {
            case "north": refinery.NorthPart = partUid; break;
            case "east": refinery.EastPart = partUid; break;
            case "gas": refinery.GasPart = partUid; break;
        }
        Dirty(masterUid, refinery);
        UpdateAssembledState(masterUid, refinery);
    }

    private void UnlinkPart(EntityUid masterUid, OilRefineryComponent refinery, string partType)
    {
        switch (partType)
        {
            case "north": refinery.NorthPart = null; break;
            case "east": refinery.EastPart = null; break;
            case "gas": refinery.GasPart = null; break;
        }
        Dirty(masterUid, refinery);
        UpdateAssembledState(masterUid, refinery);
    }

    private void UnlinkAll(EntityUid masterUid, OilRefineryComponent refinery)
    {
        foreach (var partUid in new[] { refinery.NorthPart, refinery.EastPart, refinery.GasPart })
        {
            if (partUid is { } u && TryComp<OilRefineryPartComponent>(u, out var part))
            {
                part.Master = null;
                Dirty(u, part);
            }
        }

        refinery.NorthPart = null;
        refinery.EastPart = null;
        refinery.GasPart = null;
        Dirty(masterUid, refinery);
        UpdateAssembledState(masterUid, refinery);
    }

    private void OnMasterRemoved(EntityUid uid, OilRefineryComponent refinery, ComponentRemove args)
        => UnlinkAll(uid, refinery);

    private void OnPartRemoved(EntityUid uid, OilRefineryPartComponent part, ComponentRemove args)
    {
        if (part.Master is { } masterUid && TryComp<OilRefineryComponent>(masterUid, out var refinery))
            UnlinkPart(masterUid, refinery, part.PartType);
    }

    private static bool OffsetMatches(OilRefineryComponent refinery, string partType, Vector2i offset) => partType switch
    {
        "north" => offset == refinery.NorthOffset,
        "east" => offset == refinery.EastOffset,
        "gas" => offset == refinery.GasOffset,
        _ => false,
    };

    private void UpdateAssembledState(EntityUid uid, OilRefineryComponent refinery)
    {
        var assembled = refinery.NorthPart != null && refinery.EastPart != null && refinery.GasPart != null;
        if (assembled == refinery.IsAssembled)
            return;

        refinery.IsAssembled = assembled;
        Dirty(uid, refinery);
        _appearance.SetData(uid, PlumbingVisuals.Running, false);
    }

    // -------------------------------------------------------------------
    // Verbs - available on ANY tile of the refinery, always acting on the
    // master's sulfur buffer.
    // -------------------------------------------------------------------

    private void AddRefineryVerbs(EntityUid uid, OilRefineryComponent component, GetVerbsEvent<AlternativeVerb> args)
        => AddCleanVerb(uid, component, args.User, args);

    private void AddPartVerbs(EntityUid uid, OilRefineryPartComponent part, GetVerbsEvent<AlternativeVerb> args)
    {
        if (part.Master is not { } masterUid || !TryComp<OilRefineryComponent>(masterUid, out var refinery))
            return;

        AddCleanVerb(masterUid, refinery, args.User, args);
    }

    private void AddCleanVerb(EntityUid masterUid, OilRefineryComponent component, EntityUid user, GetVerbsEvent<AlternativeVerb> args)
    {
        if (!args.CanAccess || !args.CanInteract)
            return;

        VerbCategory actionCategory = new("Обслуживание НПЗ", "/Textures/Interface/VerbIcons/gear.png");

        AlternativeVerb cleanFilter = new()
        {
            Text = $"Очистить серные фильтры ({Math.Round(component.SulfurGunk)}/{component.MaxSulfurGunk})",
            Category = actionCategory,
            Disabled = component.SulfurGunk < 20f,
            Act = () =>
            {
                int stacksToSpawn = (int) Math.Floor(component.SulfurGunk / 20f);
                var masterCoordinates = _transform.GetMoverCoordinates(masterUid);

                for (int i = 0; i < stacksToSpawn; i++)
                    EntityManager.SpawnEntity("MaterialSulfur", masterCoordinates);

                component.SulfurGunk = 0f;
                Dirty(masterUid, component);
                _popup.PopupEntity("Вы выгребли твёрдый серный шлам из карманов завода!", masterUid, user);
            },
        };
        args.Verbs.Add(cleanFilter);
    }

    // -------------------------------------------------------------------
    // Processing loop - operates purely on cached part references, never
    // does a spatial lookup. Skips unassembled refineries outright.
    // -------------------------------------------------------------------

    private static readonly ReagentId CrudeOilId = new("CrudeOil", null);

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<OilRefineryComponent>();
        while (query.MoveNext(out var uid, out var refinery))
        {
            if (!refinery.IsAssembled)
            {
                _appearance.SetData(uid, PlumbingVisuals.Running, false);
                continue;
            }

            bool isPowered = _powerReceiver.IsPowered(uid);

            if (refinery.SulfurGunk >= refinery.MaxSulfurGunk || !isPowered)
            {
                _appearance.SetData(uid, PlumbingVisuals.Running, false);
                continue;
            }

            PullFromNorth(uid, refinery, frameTime);

            bool isProcessing = TryProcess(uid, refinery, frameTime);

            _appearance.SetData(uid, PlumbingVisuals.Running, isProcessing);
        }
    }

    /// <summary>
    /// Pulls crude from the north intake's buffer into the master's input buffer,
    /// rate-limited and capped to the master buffer's remaining capacity so excess
    /// crude simply waits at the intake instead of being lost.
    /// </summary>
    private void PullFromNorth(EntityUid uid, OilRefineryComponent refinery, float frameTime)
    {
        if (refinery.NorthPart is not { } northUid)
            return;

        if (!_solutionContainer.TryGetSolution(northUid, "buffer", out var northHolder, out var northSol))
            return;

        if (northSol.Volume <= FixedPoint2.Zero)
            return;

        if (!_solutionContainer.TryGetSolution(uid, refinery.InputSolutionId, out var masterInputHolder, out var masterInput))
            return;

        var room = masterInput.AvailableVolume;
        if (room <= FixedPoint2.Zero)
            return;

        var transferRate = FixedPoint2.New(refinery.ProcessRate * 2f * frameTime); // intake can outpace processing a bit
        var toPull = FixedPoint2.Min(FixedPoint2.Min(northSol.Volume, room), transferRate);
        if (toPull <= FixedPoint2.Zero)
            return;

        var oilToTransfer = _solutionContainer.SplitSolution(northHolder.Value, toPull);
        Entity<SolutionComponent> validMasterInput = (masterInputHolder.Value.Owner, masterInputHolder.Value.Comp);
        _solutionContainer.TryAddSolution(validMasterInput, oilToTransfer);
    }

    /// <summary>
    /// Runs the actual refining step. Returns true if any crude was processed this
    /// tick. Refuses to overflow ANY output buffer - instead scales the whole batch
    /// down to whatever the tightest output allows, so reagents are never silently
    /// destroyed by a full tank.
    /// </summary>
    private bool TryProcess(EntityUid uid, OilRefineryComponent refinery, float frameTime)
    {
        if (!_solutionContainer.TryGetSolution(uid, refinery.InputSolutionId, out var inputHolder, out var inputSolution))
            return false;

        if (!inputSolution.ContainsReagent(CrudeOilId) || inputSolution.Temperature < refinery.MinProcessTemp)
            return false;

        var crudeAmount = (float) inputSolution.GetReagentQuantity(CrudeOilId);
        if (crudeAmount <= 0)
            return false;

        if (!_solutionContainer.TryGetSolution(uid, "naphtha_out", out var naphthaHolder, out var naphthaSol))
            return false;

        if (refinery.EastPart is not { } eastUid ||
            !_solutionContainer.TryGetSolution(eastUid, "buffer", out var eastHolder, out var eastSol))
            return false;

        if (refinery.GasPart is not { } gasUid ||
            !_solutionContainer.TryGetSolution(gasUid, "buffer", out var gasHolder, out var gasSol))
            return false;

        float toProcess = Math.Min(refinery.ProcessRate * frameTime, crudeAmount);

        float naphthaAmount = toProcess * 0.4f;
        float lightAmount = toProcess * 0.3f;
        float heavyAmount = toProcess * 0.2f;
        float gasAmount = toProcess * 0.1f;
        float sulfurAmount = toProcess * 0.1f;

        float scale = 1f;
        scale = Math.Min(scale, RoomScale(naphthaSol.AvailableVolume, naphthaAmount));
        scale = Math.Min(scale, RoomScale(eastSol.AvailableVolume, lightAmount));
        scale = Math.Min(scale, RoomScale(gasSol.AvailableVolume, gasAmount));

        if (scale <= 0f)
            return false;

        toProcess *= scale;
        naphthaAmount *= scale;
        lightAmount *= scale;
        heavyAmount *= scale;
        gasAmount *= scale;
        sulfurAmount *= scale;

        Entity<SolutionComponent> validInputHolder = (inputHolder.Value.Owner, inputHolder.Value.Comp);
        Entity<SolutionComponent> validNaphthaHolder = (naphthaHolder.Value.Owner, naphthaHolder.Value.Comp);
        Entity<SolutionComponent> validEast = (eastHolder.Value.Owner, eastHolder.Value.Comp);
        Entity<SolutionComponent> validGas = (gasHolder.Value.Owner, gasHolder.Value.Comp);

        _solutionContainer.RemoveReagent(validInputHolder, CrudeOilId, FixedPoint2.New(toProcess));

        _solutionContainer.TryAddReagent(validNaphthaHolder, "Naphtha", FixedPoint2.New(naphthaAmount), out _, null, null);
        _solutionContainer.TryAddReagent(validInputHolder, "HeavyOil", FixedPoint2.New(heavyAmount), out _, null, null);
        _solutionContainer.TryAddReagent(validGas, "PetroleumGas", FixedPoint2.New(gasAmount), out _, null, null);
        _solutionContainer.TryAddReagent(validEast, "LightOil", FixedPoint2.New(lightAmount), out _, null, null);

        refinery.SulfurGunk = Math.Min(refinery.MaxSulfurGunk, refinery.SulfurGunk + sulfurAmount);
        Dirty(uid, refinery);

        return true;
    }

    private static float RoomScale(FixedPoint2 available, float wanted)
    {
        if (wanted <= 0f)
            return 1f;
        var availableF = available.Float();
        return availableF >= wanted ? 1f : Math.Max(0f, availableF / wanted);
    }
}
