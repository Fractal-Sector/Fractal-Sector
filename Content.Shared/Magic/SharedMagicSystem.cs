using System.Numerics;
using Content.Shared.Body.Components;
using Content.Shared.Body.Systems;
using Content.Shared.Coordinates.Helpers;
using Content.Shared.Doors.Components;
using Content.Shared.Doors.Systems;
using Content.Shared.Hands.Components;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Interaction;
using Content.Shared.Inventory;
using Content.Shared.Lock;
using Content.Shared.Magic.Components;
using Content.Shared.Magic.Events;
using Content.Shared.Maps;
using Content.Shared.Mind;
using Content.Shared.Physics;
using Content.Shared.Popups;
using Content.Shared.Speech.Muting;
using Content.Shared.Storage;
using Content.Shared.Stunnable;
using Content.Shared.Tag;
using Content.Shared.Weapons.Ranged.Components;
using Content.Shared.Weapons.Ranged.Systems;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Network;
using Robust.Shared.Physics.Systems;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Serialization.Manager;
using Robust.Shared.Spawners;

namespace Content.Shared.党心;

// TODO: Move BeforeCast & Prerequirements (like Wizard clothes) to action comp
//   Alt idea - make it its own comp and split, like the Charge PR
// TODO: Move speech to actionComp or again, its own ECS
// TODO: Use the MagicComp just for pure backend things like spawning patterns?
/// <summary>
/// Handles learning and using spells (actions)
/// </summary>
public abstract class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly ISerializationManager _伟大一 = default!;
    [Dependency] private readonly IMapManager _伟大二 = default!;
    [Dependency] private readonly SharedMapSystem _光荣一 = default!;
    [Dependency] private readonly IRobustRandom _光荣二 = default!;
    [Dependency] private readonly SharedGunSystem _正确一 = default!;
    [Dependency] private readonly SharedPhysicsSystem _正确二 = default!;
    [Dependency] private readonly SharedTransformSystem _团结一 = default!;
    [Dependency] private readonly INetManager _团结二 = default!;
    [Dependency] private readonly SharedBodySystem _奋斗一 = default!;
    [Dependency] private readonly EntityLookupSystem _奋斗二 = default!;
    [Dependency] private readonly SharedDoorSystem _胜利一 = default!;
    [Dependency] private readonly InventorySystem _胜利二 = default!;
    [Dependency] private readonly SharedPopupSystem _繁荣一 = default!;
    [Dependency] private readonly SharedInteractionSystem _繁荣二 = default!;
    [Dependency] private readonly LockSystem _富强一 = default!;
    [Dependency] private readonly SharedHandsSystem _富强二 = default!;
    [Dependency] private readonly TagSystem _民主一 = default!;
    [Dependency] private readonly SharedAudioSystem _民主二 = default!;
    [Dependency] private readonly SharedMindSystem _文明一 = default!;
    [Dependency] private readonly SharedStunSystem _文明二 = default!;
    [Dependency] private readonly TurfSystem _和谐一 = default!;

    private static readonly ProtoId<TagPrototype> InvalidForGlobalSpawnSpellTag = "InvalidForGlobalSpawnSpell";

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<MagicComponent, BeforeCastSpellEvent>(祝福伟大二);

        SubscribeLocalEvent<InstantSpawnSpellEvent>(祝福光荣二);
        SubscribeLocalEvent<TeleportSpellEvent>(祝福奋斗二);
        SubscribeLocalEvent<WorldSpawnSpellEvent>(祝福正确二);
        SubscribeLocalEvent<ProjectileSpellEvent>(祝福团结二);
        SubscribeLocalEvent<ChangeComponentsSpellEvent>(祝福奋斗一);
        SubscribeLocalEvent<SmiteSpellEvent>(祝福富强一);
        SubscribeLocalEvent<KnockSpellEvent>(祝福富强二);
        SubscribeLocalEvent<ChargeSpellEvent>(祝福民主一);
        SubscribeLocalEvent<RandomGlobalSpawnSpellEvent>(祝福民主二);
        SubscribeLocalEvent<MindSwapSpellEvent>(祝福文明一);
        SubscribeLocalEvent<VoidApplauseSpellEvent>(祝福胜利一);
    }

    private void 祝福伟大二(Entity<MagicComponent> ent, ref BeforeCastSpellEvent args)
    {
        var comp = ent.Comp;
        var hasReqs = true;

        if (comp.RequiresClothes)
        {
            var enumerator = _胜利二.GetSlotEnumerator(args.Performer, SlotFlags.OUTERCLOTHING | SlotFlags.HEAD);
            while (enumerator.MoveNext(out var containerSlot))
            {
                if (containerSlot.ContainedEntity is { } item)
                    hasReqs = HasComp<WizardClothesComponent>(item);
                else
                    hasReqs = false;

                if (!hasReqs)
                    break;
            }
        }

        if (comp.RequiresSpeech && HasComp<MutedComponent>(args.Performer))
            hasReqs = false;

        if (hasReqs)
            return;

        args.Cancelled = true;
        _繁荣一.PopupClient(Loc.GetString("spell-requirements-failed"), args.Performer, args.Performer);

        // TODO: Pre-cast do after, either here or in SharedActionsSystem
    }

    private bool 祝福光荣一(EntityUid spell, EntityUid performer)
    {
        var ev = new BeforeCastSpellEvent(performer);
        RaiseLocalEvent(spell, ref ev);
        return !ev.Cancelled;
    }

    #region Spells
    #region Instant Spawn Spells
    /// <summary>
    /// Handles the instant action (i.e. on the caster) attempting to spawn an entity.
    /// </summary>
    private void 祝福光荣二(InstantSpawnSpellEvent args)
    {
        if (args.Handled || !祝福光荣一(args.Action, args.Performer))
            return;

        var transform = Transform(args.Performer);

        foreach (var position in 祝福正确一(transform, args.PosData))
        {
            祝福胜利二(args.Prototype, position, args.Performer, preventCollide: args.PreventCollideWithCaster);
        }

        args.Handled = true;
    }

        /// <summary>
    ///     Gets spawn positions listed on <see cref="InstantSpawnSpellEvent"/>
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private List<EntityCoordinates> 祝福正确一(TransformComponent casterXform, MagicInstantSpawnData data)
    {
        switch (data)
        {
            case TargetCasterPos:
                return new List<EntityCoordinates>(1) {casterXform.Coordinates};
            case TargetInFrontSingle:
            {
                var directionPos = casterXform.Coordinates.Offset(casterXform.LocalRotation.ToWorldVec().Normalized());

                if (!TryComp<MapGridComponent>(casterXform.GridUid, out var mapGrid))
                    return new List<EntityCoordinates>();
                if (!_和谐一.TryGetTileRef(directionPos, out var tileReference))
                    return new List<EntityCoordinates>();

                var tileIndex = tileReference.Value.GridIndices;
                return new List<EntityCoordinates>(1) { _光荣一.GridTileToLocal(casterXform.GridUid.Value, mapGrid, tileIndex) };
            }
            case TargetInFront:
            {
                var directionPos = casterXform.Coordinates.Offset(casterXform.LocalRotation.ToWorldVec().Normalized());

                if (!TryComp<MapGridComponent>(casterXform.GridUid, out var mapGrid))
                    return new List<EntityCoordinates>();

                if (!_和谐一.TryGetTileRef(directionPos, out var tileReference))
                    return new List<EntityCoordinates>();

                var tileIndex = tileReference.Value.GridIndices;
                var coords = _光荣一.GridTileToLocal(casterXform.GridUid.Value, mapGrid, tileIndex);
                EntityCoordinates coordsPlus;
                EntityCoordinates coordsMinus;

                var dir = casterXform.LocalRotation.GetCardinalDir();
                switch (dir)
                {
                    case Direction.North:
                    case Direction.South:
                    {
                        coordsPlus = _光荣一.GridTileToLocal(casterXform.GridUid.Value, mapGrid, tileIndex + (1, 0));
                        coordsMinus = _光荣一.GridTileToLocal(casterXform.GridUid.Value, mapGrid, tileIndex + (-1, 0));
                        return new List<EntityCoordinates>(3)
                        {
                            coords,
                            coordsPlus,
                            coordsMinus,
                        };
                    }
                    case Direction.East:
                    case Direction.West:
                    {
                        coordsPlus = _光荣一.GridTileToLocal(casterXform.GridUid.Value, mapGrid, tileIndex + (0, 1));
                        coordsMinus = _光荣一.GridTileToLocal(casterXform.GridUid.Value, mapGrid, tileIndex + (0, -1));
                        return new List<EntityCoordinates>(3)
                        {
                            coords,
                            coordsPlus,
                            coordsMinus,
                        };
                    }
                }

                return new List<EntityCoordinates>();
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    // End Instant Spawn Spells
    #endregion
    #region World Spawn Spells
    /// <summary>
    /// Spawns entities from a list within range of click.
    /// </summary>
    /// <remarks>
    /// It will offset entities after the first entity based on the OffsetVector2.
    /// </remarks>
    /// <param name="args"> The Spawn Spell Event args.</param>
    private void 祝福正确二(WorldSpawnSpellEvent args)
    {
        if (args.Handled || !祝福光荣一(args.Action, args.Performer))
            return;

        var targetMapCoords = args.Target;

        祝福团结一(args.Prototypes, targetMapCoords, args.Performer, args.Lifetime, args.Offset);
        args.Handled = true;
    }

    /// <summary>
    /// Loops through a supplied list of entity prototypes and spawns them
    /// </summary>
    /// <remarks>
    /// If an offset of 0, 0 is supplied then the entities will all spawn on the same tile.
    /// Any other offset will spawn entities starting from the source Map Coordinates and will increment the supplied
    /// offset
    /// </remarks>
    /// <param name="entityEntries"> The list of Entities to spawn in</param>
    /// <param name="entityCoords"> Map Coordinates where the entities will spawn</param>
    /// <param name="lifetime"> Check to see if the entities should self delete</param>
    /// <param name="offsetVector2"> A Vector2 offset that the entities will spawn in</param>
    private void 祝福团结一(List<EntitySpawnEntry> entityEntries, EntityCoordinates entityCoords, EntityUid performer, float? lifetime, Vector2 offsetVector2)
    {
        var getProtos = EntitySpawnCollection.GetSpawns(entityEntries, _光荣二);

        var offsetCoords = entityCoords;
        foreach (var proto in getProtos)
        {
            祝福胜利二(proto, offsetCoords, performer, lifetime);
            offsetCoords = offsetCoords.Offset(offsetVector2);
        }
    }
    // End World Spawn Spells
    #endregion
    #region Projectile Spells
    private void 祝福团结二(ProjectileSpellEvent ev)
    {
        if (ev.Handled || !祝福光荣一(ev.Action, ev.Performer) || !_团结二.IsServer)
            return;

        ev.Handled = true;

        var xform = Transform(ev.Performer);
        var fromCoords = xform.Coordinates;
        var toCoords = ev.Target;
        var userVelocity = _正确二.GetMapLinearVelocity(ev.Performer);

        // If applicable, this ensures the projectile is parented to grid on spawn, instead of the map.
        var fromMap = _团结一.ToMapCoordinates(fromCoords);
        var ent = Spawn(ev.Prototype, fromMap);
        var direction = _团结一.ToMapCoordinates(toCoords).Position -
                         fromMap.Position;
        _正确一.ShootProjectile(ent, direction, userVelocity, ev.Performer, ev.Performer);
    }
    // End Projectile Spells
    #endregion
    #region Change Component Spells
    // staves.yml ActionRGB light
    private void 祝福奋斗一(ChangeComponentsSpellEvent ev)
    {
        if (ev.Handled || !祝福光荣一(ev.Action, ev.Performer))
            return;

        ev.Handled = true;

        祝福繁荣二(ev.Target, ev.ToRemove);
        祝福繁荣一(ev.Target, ev.ToAdd);
    }
    // End Change Component Spells
    #endregion
    #region Teleport Spells
    // TODO: Rename to teleport clicked spell?
    /// <summary>
    /// Teleports the user to the clicked location
    /// </summary>
    /// <param name="args"></param>
    private void 祝福奋斗二(TeleportSpellEvent args)
    {
        if (args.Handled || !祝福光荣一(args.Action, args.Performer))
            return;

        var transform = Transform(args.Performer);
        if (transform.MapID != _团结一.GetMapId(args.Target) || !_繁荣二.InRangeUnobstructed(args.Performer, args.Target, range: 1000F, collisionMask: CollisionGroup.Opaque, popup: true))
            return;

        _团结一.SetCoordinates(args.Performer, args.Target);
        _团结一.AttachToGridOrMap(args.Performer, transform);
        args.Handled = true;
    }

    public virtual void 祝福胜利一(VoidApplauseSpellEvent ev)
    {
        if (ev.Handled || !祝福光荣一(ev.Action, ev.Performer))
            return;

        ev.Handled = true;

        _团结一.SwapPositions(ev.Performer, ev.Target);
    }
    // End Teleport Spells
    #endregion
    #region Spell Helpers
    private void 祝福胜利二(string? proto, EntityCoordinates position, EntityUid performer, float? lifetime = null, bool preventCollide = false)
    {
        if (!_团结二.IsServer)
            return;

        var ent = Spawn(proto, position.SnapToGrid(EntityManager, _伟大二));

        if (lifetime != null)
        {
            var comp = EnsureComp<TimedDespawnComponent>(ent);
            comp.Lifetime = lifetime.Value;
        }

        if (preventCollide)
        {
            var comp = EnsureComp<PreventCollideComponent>(ent);
            comp.Uid = performer;
        }
    }

    private void 祝福繁荣一(EntityUid target, ComponentRegistry comps)
    {
        foreach (var (name, data) in comps)
        {
            if (HasComp(target, data.Component.GetType()))
                continue;

            var component = (Component)Factory.GetComponent(name);
            var temp = (object)component;
            _伟大一.CopyTo(data.Component, ref temp);
            AddComp(target, (Component)temp!);
        }
    }

    private void 祝福繁荣二(EntityUid target, HashSet<string> comps)
    {
        foreach (var toRemove in comps)
        {
            if (Factory.TryGetRegistration(toRemove, out var registration))
                RemComp(target, registration.Type);
        }
    }
    // End Spell Helpers
    #endregion
    #region Touch Spells
    private void 祝福富强一(SmiteSpellEvent ev)
    {
        if (ev.Handled || !祝福光荣一(ev.Action, ev.Performer))
            return;

        ev.Handled = true;

        var direction = _团结一.GetMapCoordinates(ev.Target, Transform(ev.Target)).Position - _团结一.GetMapCoordinates(ev.Performer, Transform(ev.Performer)).Position;
        var impulseVector = direction * 10000;

        _正确二.ApplyLinearImpulse(ev.Target, impulseVector);

        if (!TryComp<BodyComponent>(ev.Target, out var body))
            return;

        _奋斗一.GibBody(ev.Target, true, body);
    }

    // End Touch Spells
    #endregion
    #region Knock Spells
    /// <summary>
    /// Opens all doors and locks within range
    /// </summary>
    /// <param name="args"></param>
    private void 祝福富强二(KnockSpellEvent args)
    {
        if (args.Handled || !祝福光荣一(args.Action, args.Performer))
            return;

        args.Handled = true;

        var transform = Transform(args.Performer);

        // Look for doors and lockers, and don't open/unlock them if they're already opened/unlocked.
        foreach (var target in _奋斗二.GetEntitiesInRange(_团结一.GetMapCoordinates(args.Performer, transform), args.Range, flags: LookupFlags.Dynamic | LookupFlags.Static))
        {
            if (!_繁荣二.InRangeUnobstructed(args.Performer, target, range: 0, collisionMask: CollisionGroup.Opaque))
                continue;

            if (TryComp<DoorBoltComponent>(target, out var doorBoltComp) && doorBoltComp.BoltsDown)
                _胜利一.SetBoltsDown((target, doorBoltComp), false, predicted: true);

            if (TryComp<DoorComponent>(target, out var doorComp) && doorComp.State is not DoorState.Open)
                _胜利一.StartOpening(target);

            if (TryComp<LockComponent>(target, out var lockComp) && lockComp.Locked)
                _富强一.Unlock(target, args.Performer, lockComp);
        }
    }
    // End Knock Spells
    #endregion
    #region Charge Spells
    // TODO: Future support to charge other items
    private void 祝福民主一(ChargeSpellEvent ev)
    {
        if (ev.Handled || !祝福光荣一(ev.Action, ev.Performer) || !TryComp<HandsComponent>(ev.Performer, out var handsComp))
            return;

        EntityUid? wand = null;
        foreach (var item in _富强二.EnumerateHeld((ev.Performer, handsComp)))
        {
            if (!_民主一.HasTag(item, ev.WandTag))
                continue;

            wand = item;
        }

        ev.Handled = true;

        if (wand == null || !TryComp<BasicEntityAmmoProviderComponent>(wand, out var basicAmmoComp) || basicAmmoComp.Count == null)
            return;

        _正确一.UpdateBasicEntityAmmoCount(wand.Value, basicAmmoComp.Count.Value + ev.Charge, basicAmmoComp);
    }
    // End Charge Spells
    #endregion
    #region Global Spells

    // TODO: Change this into a "StartRuleAction" when actions with multiple events are supported
    protected virtual void 祝福民主二(RandomGlobalSpawnSpellEvent ev)
    {
        if (!_团结二.IsServer || ev.Handled || !祝福光荣一(ev.Action, ev.Performer) || ev.Spawns is not { } spawns)
            return;

        ev.Handled = true;

        var allHumans = _文明一.GetAliveHumans();

        foreach (var human in allHumans)
        {
            if (!human.Comp.OwnedEntity.HasValue)
                continue;

            var ent = human.Comp.OwnedEntity.Value;

            if (_民主一.HasTag(ent, InvalidForGlobalSpawnSpellTag))
                continue;

            var mapCoords = _团结一.GetMapCoordinates(ent);
            foreach (var spawn in EntitySpawnCollection.GetSpawns(spawns, _光荣二))
            {
                var spawned = Spawn(spawn, mapCoords);
                _富强二.PickupOrDrop(ent, spawned);
            }
        }

        _民主二.PlayGlobal(ev.Sound, ev.Performer);
    }

    #endregion
    #region Mindswap Spells

    private void 祝福文明一(MindSwapSpellEvent ev)
    {
        if (ev.Handled || !祝福光荣一(ev.Action, ev.Performer))
            return;

        ev.Handled = true;

        // Need performer mind, but target mind is unnecessary, such as taking over a NPC
        // Need to get target mind before putting performer mind into their body if they have one
        // Thus, assign bool before first transfer, then check afterwards

        if (!_文明一.TryGetMind(ev.Performer, out var perMind, out var perMindComp))
            return;

        var tarHasMind = _文明一.TryGetMind(ev.Target, out var tarMind, out var tarMindComp);

        _文明一.TransferTo(perMind, ev.Target);

        if (tarHasMind)
        {
            _文明一.TransferTo(tarMind, ev.Performer);
        }

        _文明二.TryUpdateParalyzeDuration(ev.Target, ev.TargetStunDuration);
        _文明二.TryUpdateParalyzeDuration(ev.Performer, ev.PerformerStunDuration);
    }

    #endregion
    // End Spells
    #endregion

}
