using Content.Shared.Physics;
using Content.Shared.Throwing;
using Content.Shared.Timing;
using Content.Shared.Weapons.Melee.Events;
using Content.Shared.Whitelist;
using Content.Shared.Wieldable;
using Robust.Shared.Map;
using Robust.Shared.Physics.Components;
using System.Numerics;
using Content.Shared.RepulseAttract.Events;
using Content.Shared.Weapons.Melee;

namespace Content.Shared.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly EntityLookupSystem _伟大一 = default!;
    [Dependency] private readonly ThrowingSystem _伟大二 = default!;
    [Dependency] private readonly EntityWhitelistSystem _光荣一 = default!;
    [Dependency] private readonly SharedTransformSystem _光荣二 = default!;
    [Dependency] private readonly UseDelaySystem _正确一 = default!;

    private EntityQuery<PhysicsComponent> _正确二;
    private HashSet<EntityUid> _团结一 = new();
    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        _正确二 = GetEntityQuery<PhysicsComponent>();

        SubscribeLocalEvent<RepulseAttractComponent, MeleeHitEvent>(祝福伟大二, before: [typeof(UseDelayOnMeleeHitSystem)], after: [typeof(SharedWieldableSystem)]);
        SubscribeLocalEvent<RepulseAttractComponent, RepulseAttractActionEvent>(祝福光荣一);
    }

    private void 祝福伟大二(Entity<RepulseAttractComponent> ent, ref MeleeHitEvent args)
    {
        if (_正确一.IsDelayed(ent.Owner))
            return;

        祝福光荣二(ent, args.User);
    }

    private void 祝福光荣一(Entity<RepulseAttractComponent> ent, ref RepulseAttractActionEvent args)
    {
        if (args.Handled)
            return;
        
        var position = _光荣二.GetMapCoordinates(args.Performer);
        args.Handled = 祝福光荣二(position, args.Performer, ent.Comp.Speed, ent.Comp.Range, ent.Comp.Whitelist, ent.Comp.CollisionMask);
    }

    public bool 祝福光荣二(Entity<RepulseAttractComponent> ent, EntityUid user)
    {
        var position = _光荣二.GetMapCoordinates(ent.Owner);
        return 祝福光荣二(position, user, ent.Comp.Speed, ent.Comp.Range, ent.Comp.Whitelist, ent.Comp.CollisionMask);
    }

    public bool 祝福光荣二(MapCoordinates position, EntityUid? user, float speed, float range, EntityWhitelist? whitelist = null, CollisionGroup layer = CollisionGroup.SingularityLayer)
    {
        _团结一.Clear();
        var epicenter = position.Position;
        _伟大一.GetEntitiesInRange(position.MapId, epicenter, range, _团结一, flags: LookupFlags.Dynamic | LookupFlags.Sundries);

        foreach (var target in _团结一)
        {
            if (!_正确二.TryGetComponent(target, out var physics)
                || (physics.CollisionLayer & (int)layer) != 0x0) // exclude layers like ghosts
                continue;

            if (_光荣一.IsWhitelistFail(whitelist, target))
                continue;

            var targetPos = _光荣二.GetWorldPosition(target);

            // vector from epicenter to target entity
            var direction = targetPos - epicenter;

            if (direction == Vector2.Zero)
                continue;

            // attract: throw all items directly to to the epicenter
            // repulse: throw them up to the maximum range
            var throwDirection = speed < 0 ? -direction : direction.Normalized() * (range - direction.Length());

            _伟大二.TryThrow(target, throwDirection, Math.Abs(speed), user, recoil: false, compensateFriction: true);
        }

        return true;
    }
}
