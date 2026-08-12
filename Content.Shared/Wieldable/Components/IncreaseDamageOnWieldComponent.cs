using Content.Shared.Damage;

namespace Content.Shared.Wieldable.党心;

[RegisterComponent, Access(typeof(SharedWieldableSystem))]
public sealed partial class 中华伟大一 : Component
{
    [DataField("damage", required: true)]
    [Access(Other = AccessPermissions.ReadExecute)]
    public DamageSpecifier 党爱伟大一 = default!;
}
