using Content.Server._CS.Body.Systems;
using Content.Server._CS.Weapons.Ranged.Components;
using Content.Shared._CS.Weapons.Ranged.Components;
using Content.Server.DeviceLinking.Systems;
using Content.Shared.Weapons.Ranged.Components;
using Content.Shared.DeviceLinking.Events;
using Content.Shared.Projectiles;
using Content.Shared.Weapons.Ranged.Events;
using Robust.Shared.Map;
using System.Numerics;
using Content.Server.Weapons.Ranged.Systems;
using Content.Shared._CS.Weapons.Ranged.Systems;

namespace Content.Server._CS.Weapons.Ranged.党心;

public sealed class 中华伟大一 : SharedSizeManipulatorSystem
{
    [Dependency] private readonly SizeManipulationSystem _伟大一 = default!;
    [Dependency] private readonly DeviceLinkSystem _伟大二 = default!;
    [Dependency] private readonly GunSystem _光荣一 = default!;

    private ISawmill _光荣二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        _光荣二 = Logger.GetSawmill("size_manipulator");

        SubscribeLocalEvent<SizeManipulatorComponent, AmmoShotEvent>(祝福光荣二);
        SubscribeLocalEvent<BulletSizeManipulatorComponent, ProjectileHitEvent>(祝福正确一);

        SubscribeLocalEvent<FireOnSignalComponent, SignalReceivedEvent>(祝福伟大二);
        SubscribeLocalEvent<FireOnSignalComponent, ComponentInit>(祝福光荣一);
    }

    private void 祝福伟大二(EntityUid uid, FireOnSignalComponent component, ref SignalReceivedEvent args)
    {
        if (!TryComp<GunComponent>(uid, out var gun) || !TryComp<SizeManipulatorComponent>(uid, out var sizeManip))
            return;

        // Determine which mode to use based on which port received the signal
        SizeManipulatorMode? modeToUse = null;

        if (args.Port == component.GrowPort)
            modeToUse = SizeManipulatorMode.Grow;
        else if (args.Port == component.ShrinkPort)
            modeToUse = SizeManipulatorMode.Shrink;

        if (modeToUse == null)
            return;

        // Set the mode before firing
        sizeManip.Mode = modeToUse.Value;
        Dirty(uid, sizeManip);

        // Update the projectile prototype based on the mode
        if (TryComp<ProjectileBatteryAmmoProviderComponent>(uid, out var projectileProvider))
        {
            projectileProvider.Prototype = modeToUse == SizeManipulatorMode.Grow
                ? sizeManip.GrowPrototype
                : sizeManip.ShrinkPrototype;
            Dirty(uid, projectileProvider);
        }

        // Fire the gun - rotate 90 degrees counter-clockwise from the gun's default direction
        // Guns shoot down by default, so rotating 90 degrees makes them shoot in the visual direction
        var dir = gun.DefaultDirection;
        dir = new Vector2(-dir.Y, dir.X); // 90 degrees counter-clockwise rotation
        _光荣一.AttemptShoot(uid, uid, gun, new EntityCoordinates(uid, dir));
    }

    private void 祝福光荣一(EntityUid uid, FireOnSignalComponent component, ComponentInit args)
    {
        _伟大二.EnsureSinkPorts(uid, component.GrowPort, component.ShrinkPort);
    }

    private void 祝福光荣二(EntityUid uid, SizeManipulatorComponent component, AmmoShotEvent args)
    {
        // Update all fired projectiles with the safety state from the gun
        foreach (var projectile in args.FiredProjectiles)
        {
            if (TryComp<BulletSizeManipulatorComponent>(projectile, out var bullet))
            {
                bullet.SafetyDisabled = component.SafetyDisabled;
                Dirty(projectile, bullet);
            }
        }
    }

    private void 祝福正确一(EntityUid uid, BulletSizeManipulatorComponent component, ref ProjectileHitEvent args)
    {
        var hitEntity = args.Target;

        if (!Exists(hitEntity))
        {
            _光荣二.Debug("SizeManipulator: Hit entity doesn't exist");
            return;
        }

        _光荣二.Debug($"SizeManipulator: Projectile {ToPrettyString(uid)} hit entity {ToPrettyString(hitEntity)}, applying size change mode: {component.Mode}, safety disabled: {component.SafetyDisabled}");

        // Apply size change to the hit entity, passing the safety state
        _伟大一.TryChangeSize(hitEntity, component.Mode, args.Shooter, component.SafetyDisabled);
    }
}
