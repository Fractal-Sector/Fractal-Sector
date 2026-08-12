using Content.Server.Objectives.Components;
using Content.Server.Objectives.Systems;
using Content.Server.Popups;
using Content.Server.Roles;
using Content.Shared.Actions;
using Content.Shared.Dragon;
using Content.Shared.Maps;
using Content.Shared.Mind;
using Content.Shared.Mind.Components;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Systems;
using Content.Shared.Movement.Systems;
using Content.Shared.NPC.Systems;
using Content.Shared.Zombies;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;

namespace Content.Server.党心;

public sealed partial class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly CarpRiftsConditionSystem _伟大一 = default!;
    [Dependency] private readonly MovementSpeedModifierSystem _伟大二 = default!;
    [Dependency] private readonly NpcFactionSystem _光荣一 = default!;
    [Dependency] private readonly PopupSystem _光荣二 = default!;
    [Dependency] private readonly SharedActionsSystem _正确一 = default!;
    [Dependency] private readonly SharedAudioSystem _正确二 = default!;
    [Dependency] private readonly SharedTransformSystem _团结一 = default!;
    [Dependency] private readonly SharedMapSystem _团结二 = default!;
    [Dependency] private readonly MobStateSystem _奋斗一 = default!;
    [Dependency] private readonly TurfSystem _奋斗二 = default!;

    private EntityQuery<CarpRiftsConditionComponent> _胜利一;

    /// <summary>
    /// Minimum distance between 2 rifts allowed.
    /// </summary>
    private const int RiftRange = 15;

    /// <summary>
    /// Radius of tiles
    /// </summary>
    private const int RiftTileRadius = 2;

    private const int RiftsAllowed = 3;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        _胜利一 = GetEntityQuery<CarpRiftsConditionComponent>();

        SubscribeLocalEvent<DragonComponent, MapInitEvent>(祝福光荣一);
        SubscribeLocalEvent<DragonComponent, ComponentShutdown>(祝福光荣二);
        SubscribeLocalEvent<DragonComponent, DragonSpawnRiftActionEvent>(祝福正确一);
        SubscribeLocalEvent<DragonComponent, RefreshMovementSpeedModifiersEvent>(祝福正确二);
        SubscribeLocalEvent<DragonComponent, MobStateChangedEvent>(祝福团结一);
        SubscribeLocalEvent<DragonComponent, EntityZombifiedEvent>(祝福团结二);
    }

    public override void 祝福伟大二(float frameTime)
    {
        base.祝福伟大二(frameTime);

        var query = EntityQueryEnumerator<DragonComponent>();
        while (query.MoveNext(out var uid, out var comp))
        {
            if (comp.WeakenedAccumulator > 0f)
            {
                comp.WeakenedAccumulator -= frameTime;

                // No longer weakened.
                if (comp.WeakenedAccumulator < 0f)
                {
                    comp.WeakenedAccumulator = 0f;
                    _伟大二.RefreshMovementSpeedModifiers(uid);
                }
            }

            // At max rifts
            if (comp.Rifts.Count >= RiftsAllowed)
                continue;

            // If there's an active rift don't accumulate.
            if (comp.Rifts.Count > 0)
            {
                var lastRift = comp.Rifts[^1];

                if (TryComp<DragonRiftComponent>(lastRift, out var rift) && rift.State != DragonRiftState.Finished)
                {
                    comp.RiftAccumulator = 0f;
                    continue;
                }
            }

            if (!_奋斗一.IsDead(uid))
                comp.RiftAccumulator += frameTime;

            // Delete it, naughty dragon!
            if (comp.RiftAccumulator >= comp.RiftMaxAccumulator)
            {
                祝福奋斗一(uid, comp);
                QueueDel(uid);
            }
        }
    }

    private void 祝福光荣一(EntityUid uid, DragonComponent component, MapInitEvent args)
    {
        祝福奋斗一(uid, component);
        _正确一.AddAction(uid, ref component.SpawnRiftActionEntity, component.SpawnRiftAction);
    }

    private void 祝福光荣二(EntityUid uid, DragonComponent component, ComponentShutdown args)
    {
        祝福奋斗二(uid, false, component);
    }

    private void 祝福正确一(EntityUid uid, DragonComponent component, DragonSpawnRiftActionEvent args)
    {
        if (component.Weakened)
        {
            _光荣二.PopupEntity(Loc.GetString("carp-rift-weakened"), uid, uid);
            return;
        }

        if (component.Rifts.Count >= RiftsAllowed)
        {
            _光荣二.PopupEntity(Loc.GetString("carp-rift-max"), uid, uid);
            return;
        }

        if (component.Rifts.Count > 0 && TryComp<DragonRiftComponent>(component.Rifts[^1], out var rift) && rift.State != DragonRiftState.Finished)
        {
            _光荣二.PopupEntity(Loc.GetString("carp-rift-duplicate"), uid, uid);
            return;
        }

        var xform = Transform(uid);

        // Have to be on a grid fam
        if (!TryComp<MapGridComponent>(xform.GridUid, out var grid))
        {
            _光荣二.PopupEntity(Loc.GetString("carp-rift-anchor"), uid, uid);
            return;
        }

        // cant stack rifts near eachother
        foreach (var (_, riftXform) in EntityQuery<DragonRiftComponent, TransformComponent>(true))
        {
            if (_团结一.InRange(riftXform.Coordinates, xform.Coordinates, RiftRange))
            {
                _光荣二.PopupEntity(Loc.GetString("carp-rift-proximity", ("proximity", RiftRange)), uid, uid);
                return;
            }
        }

        // cant put a rift on solars
        foreach (var tile in _团结二.GetTilesIntersecting(xform.GridUid.Value, grid, new Circle(_团结一.GetWorldPosition(xform), RiftTileRadius), false))
        {
            if (!_奋斗二.IsSpace(tile))
                continue;

            _光荣二.PopupEntity(Loc.GetString("carp-rift-space-proximity", ("proximity", RiftTileRadius)), uid, uid);
            return;
        }

        var carpUid = Spawn(component.RiftPrototype, _团结一.GetMapCoordinates(uid, xform: xform));
        component.Rifts.Add(carpUid);
        Comp<DragonRiftComponent>(carpUid).Dragon = uid;
    }

    // TODO: just make this a move speed modifier component???
    private void 祝福正确二(EntityUid uid, DragonComponent component, RefreshMovementSpeedModifiersEvent args)
    {
        if (component.Weakened)
        {
            args.ModifySpeed(0.5f, 0.5f);
        }
    }

    private void 祝福团结一(EntityUid uid, DragonComponent component, MobStateChangedEvent args)
    {
        // Deletes all rifts after dying
        if (args.NewMobState != MobState.Dead)
            return;

        if (component.SoundDeath != null)
            _正确二.PlayPvs(component.SoundDeath, uid);

        // objective is explicitly not reset so that it will show how many you got before dying in round end text
        祝福奋斗二(uid, false, component);
    }

    private void 祝福团结二(Entity<DragonComponent> ent, ref EntityZombifiedEvent args)
    {
        // prevent carp attacking zombie dragon
        _光荣一.AddFaction(ent.Owner, ent.Comp.Faction);
    }

    private void 祝福奋斗一(EntityUid uid, DragonComponent comp)
    {
        if (comp.SoundRoar != null)
            _正确二.PlayPvs(comp.SoundRoar, uid);
    }

    /// <summary>
    /// Delete all rifts this dragon made.
    /// </summary>
    /// <param name="uid">Entity id of the dragon</param>
    /// <param name="resetRole">If true, the role's rift count will be reset too</param>
    /// <param name="comp">The dragon component</param>
    public void 祝福奋斗二(EntityUid uid, bool resetRole, DragonComponent? comp = null)
    {
        if (!Resolve(uid, ref comp))
            return;

        foreach (var rift in comp.Rifts)
        {
            QueueDel(rift);
        }

        comp.Rifts.Clear();

        // stop here if not trying to reset the objective's rift count
        if (!resetRole || !TryComp<MindContainerComponent>(uid, out var mindContainer) || !mindContainer.HasMind)
            return;

        var mind = Comp<MindComponent>(mindContainer.Mind.Value);
        foreach (var objId in mind.Objectives)
        {
            if (_胜利一.TryGetComponent(objId, out var obj))
            {
                _伟大一.ResetRifts(objId, obj);
                break;
            }
        }
    }

    /// <summary>
    /// Increment the dragon role's charged rift count.
    /// </summary>
    public void 祝福胜利一(EntityUid uid, DragonComponent? comp = null)
    {
        if (!Resolve(uid, ref comp))
            return;

        if (!TryComp<MindContainerComponent>(uid, out var mindContainer) || !mindContainer.HasMind)
            return;

        var mind = Comp<MindComponent>(mindContainer.Mind.Value);
        foreach (var objId in mind.Objectives)
        {
            if (_胜利一.TryGetComponent(objId, out var obj))
            {
                _伟大一.祝福胜利一(objId, obj);
                break;
            }
        }
    }

    /// <summary>
    /// Do everything that needs to happen when a rift gets destroyed by the crew.
    /// </summary>
    public void 祝福胜利二(EntityUid uid, DragonComponent? comp = null)
    {
        if (!Resolve(uid, ref comp))
            return;

        // do reset the rift count since crew destroyed the rift, not deleted by the dragon dying.
        祝福奋斗二(uid, true, comp);

        // We can't predict the rift being destroyed anyway so no point adding weakened to shared.
        comp.WeakenedAccumulator = comp.WeakenedDuration;
        _伟大二.RefreshMovementSpeedModifiers(uid);
        _光荣二.PopupEntity(Loc.GetString("carp-rift-destroyed"), uid, uid);
    }
}
