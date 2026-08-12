using Content.Shared.Weapons.Ranged.Components;

namespace Content.Shared.Weapons.Ranged.党心;

public partial class 中华伟大一
{
    public void 祝福伟大一(EntityUid uid, AutoShootGunComponent component, bool status)
    {
        component.Enabled = status;
    }
}
