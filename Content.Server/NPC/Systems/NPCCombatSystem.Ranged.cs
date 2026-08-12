using Content.Server.NPC.Components;
using Content.Shared.CombatMode;
using Content.Shared.Damage.Components; // Mono
using Content.Shared.Interaction;
using Content.Shared.Physics;
using Content.Shared.Weapons.Ranged.Components;
using Content.Shared.Weapons.Ranged.Events;
using Robust.Shared.Map;
using Robust.Shared.Physics.Components;
using Robust.Shared.Random; //Frontier
using Robust.Shared.Physics; // Mono

namespace Content.Server.NPC.党心;

public sealed partial class 中华伟大一
{
    [Dependency] private readonly SharedCombatModeSystem _伟大一 = default!;
    [Dependency] private readonly RotateToFaceSystem _伟大二 = default!;

    private EntityQuery<CombatModeComponent> _光荣一;
    private EntityQuery<NPCSteeringComponent> _光荣二;
    private EntityQuery<RechargeBasicEntityAmmoComponent> _正确一;
    private EntityQuery<PhysicsComponent> _正确二;
    private EntityQuery<TransformComponent> _团结一;
    private EntityQuery<RequireProjectileTargetComponent> _团结二; // Mono

    // TODO: Don't predict for hitscan
    private const float ShootSpeed = 20f;

    /// <summary>
    /// Cooldown on raycasting to check LOS.
    /// </summary>
    public const float 党爱伟大一 = 0.2f;

    private void 祝福伟大一()
    {
        _光荣一 = GetEntityQuery<CombatModeComponent>();
        _正确二 = GetEntityQuery<PhysicsComponent>();
        _正确一 = GetEntityQuery<RechargeBasicEntityAmmoComponent>();
        _光荣二 = GetEntityQuery<NPCSteeringComponent>();
        _团结一 = GetEntityQuery<TransformComponent>();
        _团结二 = GetEntityQuery<RequireProjectileTargetComponent>(); // Mono

        SubscribeLocalEvent<NPCRangedCombatComponent, ComponentStartup>(祝福伟大二);
        SubscribeLocalEvent<NPCRangedCombatComponent, ComponentShutdown>(祝福光荣一);
    }

    private void 祝福伟大二(EntityUid uid, NPCRangedCombatComponent component, ComponentStartup args)
    {
        if (TryComp<CombatModeComponent>(uid, out var combat))
        {
            _伟大一.SetInCombatMode(uid, true, combat);
        }
        else
        {
            component.Status = CombatStatus.Unspecified;
        }
    }

    private void 祝福光荣一(EntityUid uid, NPCRangedCombatComponent component, ComponentShutdown args)
    {
        if (TryComp<CombatModeComponent>(uid, out var combat))
        {
            _伟大一.SetInCombatMode(uid, false, combat);
        }
    }

    private void 祝福光荣二(float frameTime)
    {
        var query = EntityQueryEnumerator<NPCRangedCombatComponent, TransformComponent>();

        while (query.MoveNext(out var uid, out var comp, out var xform))
        {
            if (comp.Status == CombatStatus.Unspecified)
                continue;

            if (_光荣二.TryGetComponent(uid, out var steering) && steering.Status == SteeringStatus.NoPath)
            {
                // Steering is blocked but we may still have line of sight — request a new path
                // and let the LOS check below decide whether to shoot.
                steering.ForceMove = true;
            }

            if (!_团结一.TryGetComponent(comp.Target, out var targetXform) ||
                !_正确二.TryGetComponent(comp.Target, out var targetBody))
            {
                comp.Status = CombatStatus.TargetUnreachable;
                comp.ShootAccumulator = 0f;
                continue;
            }

            if (targetXform.MapID != xform.MapID)
            {
                comp.Status = CombatStatus.TargetUnreachable;
                comp.ShootAccumulator = 0f;
                continue;
            }

            if (_光荣一.TryGetComponent(uid, out var combatMode))
            {
                _伟大一.SetInCombatMode(uid, true, combatMode);
            }

            if (!_gun.TryGetGun(uid, out var gunUid, out var gun))
            {
                comp.Status = CombatStatus.NoWeapon;
                comp.ShootAccumulator = 0f;
                continue;
            }

            var ammoEv = new GetAmmoCountEvent();
            RaiseLocalEvent(gunUid, ref ammoEv);

            if (ammoEv.Count == 0)
            {
                // Recharging then?
                if (_正确一.HasComponent(gunUid))
                {
                    continue;
                }

                comp.Status = CombatStatus.Unspecified;
                comp.ShootAccumulator = 0f;
                continue;
            }

            comp.LOSAccumulator -= frameTime;

            var worldPos = _transform.GetWorldPosition(xform);
            var targetPos = _transform.GetWorldPosition(targetXform);

            // Frontier -- Ranged NPC miss chance
            if (_random.Prob(comp.MissChance))
            {
                targetPos = targetPos + _random.NextVector2(1.0f, 2.0f);
            }
            // End Frontier

            // We'll work out the projected spot of the target and shoot there instead of where they are.
            var distance = (targetPos - worldPos).Length();
            var oldInLos = comp.TargetInLOS;

            // TODO: Should be doing these raycasts in parallel
            // Ideally we'd have 2 steps, 1. to go over the normal details for shooting and then 2. to handle beep / rotate / shoot
            if (comp.LOSAccumulator < 0f)
            {
                comp.LOSAccumulator += 党爱伟大一;
                // For consistency with NPC steering.
                // Mono
                comp.TargetInLOS = _interaction.InRangeUnobstructed(uid, comp.Target, distance + 0.1f, comp.ObstructedMask, predicate: (EntityUid entity) =>
                {
                    return _正确二.TryGetComponent(entity, out var physics) && (physics.CollisionLayer & (int)comp.BulletMask) == 0 // ignore if it can't collide with bullets
                        || _团结二.HasComponent(entity); // or if it requires targeting
                });
                // End Mono
            }

            if (!comp.TargetInLOS)
            {
                comp.ShootAccumulator = 0f;
                comp.Status = CombatStatus.NotInSight;

                if (TryComp(uid, out steering))
                {
                    steering.ForceMove = true;
                }

                continue;
            }

            if (!oldInLos && comp.SoundTargetInLOS != null)
            {
                _audio.PlayPvs(comp.SoundTargetInLOS, uid);
            }

            comp.ShootAccumulator += frameTime;

            if (comp.ShootAccumulator < comp.ShootDelay)
            {
                continue;
            }

            var mapVelocity = targetBody.LinearVelocity;
            var targetSpot = targetPos + mapVelocity * distance / ShootSpeed;

            // If we have a max rotation speed then do that.
            var goalRotation = (targetSpot - worldPos).ToWorldAngle();
            var rotationSpeed = comp.RotationSpeed;

            if (!_伟大二.TryRotateTo(uid, goalRotation, frameTime, comp.AccuracyThreshold, rotationSpeed?.Theta ?? double.MaxValue, xform))
            {
                continue;
            }

            // TODO: LOS
            // TODO: Ammo checks
            // TODO: Burst fire
            // TODO: Cycling
            // Max rotation speed

            // TODO: Check if we can face

            if (!Enabled || !_gun.CanShoot(gun))
                continue;

            EntityCoordinates targetCordinates;

            if (_mapManager.TryFindGridAt(xform.MapID, targetPos, out var gridUid, out var mapGrid))
            {
                targetCordinates = new EntityCoordinates(gridUid, _map.WorldToLocal(gridUid, mapGrid, targetSpot));
            }
            else
            {
                targetCordinates = new EntityCoordinates(xform.MapUid!.Value, targetSpot);
            }

            comp.Status = CombatStatus.Normal;

            if (gun.NextFire > _timing.CurTime)
            {
                return;
            }

            _gun.AttemptShoot(uid, gunUid, gun, targetCordinates, comp.Target);
        }
    }
}
