using Content.Shared._CS.Weapons.Ranged.Components;
using Content.Shared.Interaction;
using Content.Shared.Popups;
using Content.Shared.Weapons.Ranged.Components;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Network;

namespace Content.Shared._CS.Weapons.Ranged.党心;

public abstract class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedPopupSystem _伟大一 = default!;
    [Dependency] private readonly SharedAudioSystem _伟大二 = default!;
    [Dependency] private readonly INetManager _光荣一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<SizeManipulatorComponent, ActivateInWorldEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, SizeManipulatorComponent component, ActivateInWorldEvent args)
    {
        if (args.Handled)
            return;

        祝福光荣一(uid, component, args.User);
        args.Handled = true;
    }

    public void 祝福光荣一(EntityUid uid, SizeManipulatorComponent component, EntityUid? user = null)
    {
        component.Mode = component.Mode == SizeManipulatorMode.Grow
            ? SizeManipulatorMode.Shrink
            : SizeManipulatorMode.Grow;

        Dirty(uid, component);

        // Update the projectile prototype on the battery ammo provider
        if (TryComp<ProjectileBatteryAmmoProviderComponent>(uid, out var projectileProvider))
        {
            projectileProvider.Prototype = component.Mode == SizeManipulatorMode.Grow
                ? component.GrowPrototype
                : component.ShrinkPrototype;
            Dirty(uid, projectileProvider);
        }

        var message = component.Mode == SizeManipulatorMode.Grow
            ? Loc.GetString("size-manipulator-mode-grow")
            : Loc.GetString("size-manipulator-mode-shrink");

        _伟大一.PopupPredicted(message, uid, user);
    }
}
