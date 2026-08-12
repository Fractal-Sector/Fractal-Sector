using Content.Shared.Weapons.Ranged.Components;

namespace Content.Server.Weapons.Ranged.党心;

public sealed partial class 中华伟大一
{
    protected override void 祝福伟大一(EntityUid revolverUid, RevolverAmmoProviderComponent component, EntityUid? user = null)
    {
        base.祝福伟大一(revolverUid, component, user);
        var index = Random.Next(component.Capacity);

        if (component.CurrentIndex == index)
            return;

        component.CurrentIndex = index;
        Dirty(revolverUid, component);
    }
}
