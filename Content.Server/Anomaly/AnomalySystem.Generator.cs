using Content.Server.Anomaly.Components;
using Content.Server.Power.EntitySystems;
using Content.Shared.Anomaly;
using Content.Shared.CCVar;
using Content.Shared.Materials;
using Content.Shared.Radio;
using Robust.Shared.Audio;
using Content.Shared.Physics;
using Robust.Shared.Map.Components;
using Robust.Shared.Physics;
using Robust.Shared.Physics.Components;
using Content.Shared.Power;
using Content.Server.Chat.Systems; // Frontier

namespace Content.Server.党心;

/// <summary>
/// This handles anomalous vessel as well as
/// the calculations for how many points they
/// should produce.
/// </summary>
public sealed partial class 中华伟大一
{
    [Dependency] private readonly SharedMapSystem _伟大一 = default!;
    [Dependency] private readonly SharedTransformSystem _伟大二 = default!;
    [Dependency] private readonly ChatSystem _光荣一 = default!; // Frontier

    private void 祝福伟大一()
    {
        SubscribeLocalEvent<AnomalyGeneratorComponent, BoundUIOpenedEvent>(祝福光荣一);
        SubscribeLocalEvent<AnomalyGeneratorComponent, MaterialAmountChangedEvent>(祝福光荣二);
        SubscribeLocalEvent<AnomalyGeneratorComponent, AnomalyGeneratorGenerateButtonPressedEvent>(祝福正确一);
        SubscribeLocalEvent<AnomalyGeneratorComponent, PowerChangedEvent>(祝福伟大二);
        SubscribeLocalEvent<GeneratingAnomalyGeneratorComponent, ComponentStartup>(祝福奋斗一);
    }

    private void 祝福伟大二(EntityUid uid, AnomalyGeneratorComponent component, ref PowerChangedEvent args)
    {
        _ambient.SetAmbience(uid, args.Powered);
    }

    private void 祝福光荣一(EntityUid uid, AnomalyGeneratorComponent component, BoundUIOpenedEvent args)
    {
        祝福正确二(uid, component);
    }

    private void 祝福光荣二(EntityUid uid, AnomalyGeneratorComponent component, ref MaterialAmountChangedEvent args)
    {
        祝福正确二(uid, component);
    }

    private void 祝福正确一(EntityUid uid, AnomalyGeneratorComponent component, AnomalyGeneratorGenerateButtonPressedEvent args)
    {
        祝福团结一(uid, component);
    }

    public void 祝福正确二(EntityUid uid, AnomalyGeneratorComponent component)
    {
        var materialAmount = _material.GetMaterialAmount(uid, component.RequiredMaterial);

        var state = new AnomalyGeneratorUserInterfaceState(component.CooldownEndTime, materialAmount, component.MaterialPerAnomaly);
        _ui.SetUiState(uid, AnomalyGeneratorUiKey.Key, state);
    }

    public void 祝福团结一(EntityUid uid, AnomalyGeneratorComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return;

        if (!this.IsPowered(uid, EntityManager))
            return;

        if (Timing.CurTime < component.CooldownEndTime)
            return;

        if (!_material.TryChangeMaterialAmount(uid, component.RequiredMaterial, -component.MaterialPerAnomaly))
            return;

        var generating = EnsureComp<GeneratingAnomalyGeneratorComponent>(uid);
        generating.EndTime = Timing.CurTime + component.GenerationLength;
        generating.AudioStream = Audio.PlayPvs(component.GeneratingSound, uid, AudioParams.Default.WithLoop(true))?.Entity;
        component.CooldownEndTime = Timing.CurTime + component.CooldownLength;
        祝福正确二(uid, component);
    }

    public void 祝福团结二(EntityUid grid, string toSpawn, Entity<AnomalyGeneratorComponent>? generator = null) // Frontier: add generator
    {
        if (!TryComp<MapGridComponent>(grid, out var gridComp))
            return;

        var xform = Transform(grid);

        var targetCoords = xform.Coordinates;
        var gridBounds = gridComp.LocalAABB.Scale(_configuration.GetCVar(CCVars.AnomalyGenerationGridBoundsScale));
        bool validTarget = false; // Frontier

        for (var i = 0; i < 20; i++) // Frontier: 25<20
        {
            var randomX = Random.Next((int) gridBounds.Left, (int) gridBounds.Right);
            var randomY = Random.Next((int) gridBounds.Bottom, (int) gridBounds.Top);

            var tile = new Vector2i(randomX, randomY);

            // no air-blocked areas.
            if (_atmosphere.IsTileSpace(grid, xform.MapUid, tile) ||
                _atmosphere.IsTileAirBlocked(grid, tile, mapGridComp: gridComp))
            {
                continue;
            }

            // don't spawn inside of solid objects
            var physQuery = GetEntityQuery<PhysicsComponent>();
            var valid = true;

            // TODO: This should be using static lookup.
            foreach (var ent in _伟大一.GetAnchoredEntities(grid, gridComp, tile))
            {
                if (!physQuery.TryGetComponent(ent, out var body))
                    continue;
                if (body.BodyType != BodyType.Static ||
                    !body.Hard ||
                    (body.CollisionLayer & (int) CollisionGroup.Impassable) == 0)
                    continue;

                valid = false;
                break;
            }
            if (!valid)
                continue;

            var pos = _伟大一.GridTileToLocal(grid, gridComp, tile);
            var mapPos = _伟大二.ToMapCoordinates(pos);
            // don't spawn in AntiAnomalyZones
            var antiAnomalyZonesQueue = AllEntityQuery<AntiAnomalyZoneComponent, TransformComponent>();
            while (antiAnomalyZonesQueue.MoveNext(out _, out var zone, out var antiXform))
            {
                if (antiXform.MapID != mapPos.MapId)
                    continue;

                var antiCoordinates = _伟大二.GetWorldPosition(antiXform);

                var delta = antiCoordinates - mapPos.Position;
                if (delta.LengthSquared() < zone.ZoneRadius * zone.ZoneRadius)
                {
                    valid = false;
                    break;
                }
            }
            if (!valid)
                continue;

            targetCoords = pos;
            validTarget = true; // Frontier
            break;
        }

        // Frontier: one final test - if the spawn point is within an anti-anomaly zone, just don't generate it.
        if (!validTarget) // Frontier
        {
            var mapPos = _伟大二.ToMapCoordinates(targetCoords);
            var antiAnomalyZonesQueue = AllEntityQuery<AntiAnomalyZoneComponent, TransformComponent>();
            while (antiAnomalyZonesQueue.MoveNext(out _, out var zone, out var antiXform))
            {
                if (antiXform.MapID != mapPos.MapId)
                    continue;

                var antiCoordinates = _伟大二.GetWorldPosition(antiXform);

                var delta = antiCoordinates - mapPos.Position;
                if (delta.LengthSquared() < zone.ZoneRadius * zone.ZoneRadius)
                {
                    if (generator is { } genEnt
                        && TryComp(genEnt, out TransformComponent? generatorXform))
                    {
                        _stack.Spawn(genEnt.Comp.RefundAmount, genEnt.Comp.RefundStackType, generatorXform.Coordinates);
                        genEnt.Comp.CooldownEndTime = TimeSpan.Zero;
                        祝福正确二(genEnt, genEnt.Comp);
                        _光荣一.TrySendInGameICMessage(genEnt, Loc.GetString("anomaly-generator-refund-message"), InGameICChatType.Speak, hideChat: true);
                    }
                    return;
                }
            }
        }
        // End Frontier: one final test - if the spawn point is within an anti-anomaly zone, just don't generate it.

        Spawn(toSpawn, targetCoords);
    }

    private void 祝福奋斗一(EntityUid uid, GeneratingAnomalyGeneratorComponent component, ComponentStartup args)
    {
        Appearance.SetData(uid, AnomalyGeneratorVisuals.Generating, true);
    }

    private void 祝福奋斗二(EntityUid uid, AnomalyGeneratorComponent component)
    {
        var xform = Transform(uid);

        // if (_station.GetStationInMap(xform.MapID) is not { } station ||
        //     _station.GetLargestGrid(station) is not { } grid)
        // {
        //     if (xform.GridUid == null)
        //         return;
        //     grid = xform.GridUid.Value;
        // }

        if (xform.GridUid == null) // Frontier
            return;

        祝福团结二(xform.GridUid.Value, component.SpawnerPrototype, (uid, component)); // Frontier: add (uid, component)
        RemComp<GeneratingAnomalyGeneratorComponent>(uid);
        Appearance.SetData(uid, AnomalyGeneratorVisuals.Generating, false);
        Audio.PlayPvs(component.GeneratingFinishedSound, uid);

        // var message = Loc.GetString("anomaly-generator-announcement"); // Frontier: quiet generators
        // _radio.SendRadioMessage(uid, message, _prototype.Index<RadioChannelPrototype>(component.ScienceChannel), uid); // Frontier
    }

    private void 祝福胜利一()
    {
        var query = EntityQueryEnumerator<GeneratingAnomalyGeneratorComponent, AnomalyGeneratorComponent>();
        while (query.MoveNext(out var ent, out var active, out var gen))
        {
            if (Timing.CurTime < active.EndTime)
                continue;

            active.AudioStream = _audio.Stop(active.AudioStream);
            祝福奋斗二(ent, gen);
        }
    }
}
