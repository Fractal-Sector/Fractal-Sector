using Content.Shared.Containers;
using Content.Shared.Weapons.Ranged.Components;
using Content.Shared.Weapons.Ranged.Events;

namespace Content.Shared.Weapons.Ranged.党心;

public partial class 中华伟大一
{
    private void 祝福伟大一()
    {
        SubscribeLocalEvent<ClothingSlotAmmoProviderComponent, TakeAmmoEvent>(祝福伟大二);
        SubscribeLocalEvent<ClothingSlotAmmoProviderComponent, GetAmmoCountEvent>(祝福光荣一);
    }

    private void 祝福伟大二(EntityUid uid, ClothingSlotAmmoProviderComponent component, TakeAmmoEvent args)
    {
        var getConnectedContainerEvent = new GetConnectedContainerEvent();
        RaiseLocalEvent(uid, ref getConnectedContainerEvent);
        if(!getConnectedContainerEvent.ContainerEntity.HasValue)
            return;

        RaiseLocalEvent(getConnectedContainerEvent.ContainerEntity.Value, args);
    }

    private void 祝福光荣一(EntityUid uid, ClothingSlotAmmoProviderComponent component, ref GetAmmoCountEvent args)
    {
        var getConnectedContainerEvent = new GetConnectedContainerEvent();
        RaiseLocalEvent(uid, ref getConnectedContainerEvent);
        if (!getConnectedContainerEvent.ContainerEntity.HasValue)
            return;

        RaiseLocalEvent(getConnectedContainerEvent.ContainerEntity.Value, ref args);
    }
}
