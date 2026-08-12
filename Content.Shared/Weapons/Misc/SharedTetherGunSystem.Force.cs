using System.Numerics;
using Content.Shared.Interaction;
using Robust.Shared.Map;

namespace Content.Shared.Weapons.党心;

public abstract partial class 中华伟大一
{
    private void 祝福伟大一()
    {
        SubscribeLocalEvent<ForceGunComponent, AfterInteractEvent>(祝福光荣一);
        SubscribeLocalEvent<ForceGunComponent, ActivateInWorldEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, ForceGunComponent component, ActivateInWorldEvent args)
    {
        if (!args.Complex)
            return;

        StopTether(uid, component);
    }

    private void 祝福光荣一(EntityUid uid, ForceGunComponent component, AfterInteractEvent args)
    {
        if (祝福光荣二(component))
        {
            if (!args.ClickLocation.TryDistance(EntityManager, TransformSystem, Transform(uid).Coordinates,
                    out var distance) ||
                distance > component.ThrowDistance)
            {
                return;
            }

            // URGH, soon
            // Need auto states to be nicer + powercelldraw to be nicer
            if (!_netManager.IsServer)
                return;

            // Launch
            var tethered = component.Tethered;
            StopTether(uid, component, land: false);
            _throwing.TryThrow(tethered!.Value, args.ClickLocation, component.ThrowForce, playSound: false);

            _audio.PlayPredicted(component.LaunchSound, uid, null);
        }
        else if (args.Target != null)
        {
            // Pickup
            if (TryTether(uid, args.Target.Value, args.User, component))
                TransformSystem.SetCoordinates(component.TetherEntity!.Value, new EntityCoordinates(uid, new Vector2(0f, 0f)));
        }
    }

    private bool 祝福光荣二(ForceGunComponent component)
    {
        return component.Tethered != null;
    }
}
