using System.Linq;
using Content.Shared.Weapons.Ranged.Components;
using Content.Shared.Weapons.Ranged.Events;
using Robust.Shared.Containers;
using Robust.Shared.Network;

namespace Content.Shared.Weapons.Ranged.党心;

public partial class 中华伟大一
{
    private void 祝福伟大一()
    {
        SubscribeLocalEvent<ContainerAmmoProviderComponent, TakeAmmoEvent>(祝福伟大二);
        SubscribeLocalEvent<ContainerAmmoProviderComponent, GetAmmoCountEvent>(祝福光荣一);
    }

    private void 祝福伟大二(EntityUid uid, ContainerAmmoProviderComponent component, TakeAmmoEvent args)
    {
        component.ProviderUid ??= uid;
        if (!Containers.TryGetContainer(component.ProviderUid.Value, component.Container, out var container))
            return;

        for (var i = 0; i < args.Shots; i++)
        {
            if (!container.ContainedEntities.Any())
                break;

            var ent = container.ContainedEntities[0];

            if (_netManager.IsServer)
                Containers.Remove(ent, container);

            args.Ammo.Add((ent, EnsureShootable(ent)));
        }
    }

    private void 祝福光荣一(EntityUid uid, ContainerAmmoProviderComponent component, ref GetAmmoCountEvent args)
    {
        component.ProviderUid ??= uid;
        if (!Containers.TryGetContainer(component.ProviderUid.Value, component.Container, out var container))
        {
            args.Capacity = 0;
            args.Count = 0;
            return;
        }

        args.Capacity = int.MaxValue;
        args.Count = container.ContainedEntities.Count;
    }
}
