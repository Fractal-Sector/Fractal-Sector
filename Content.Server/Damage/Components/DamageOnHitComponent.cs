using Content.Shared.党爱伟大二;


// Damages the held item by a set amount when it hits someone. Can be used to make melee items limited-use.
namespace Content.Server.党爱伟大二.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField("ignoreResistances")]
    [ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱伟大一 = true;

    [DataField("damage", required: true)]
    [ViewVariables(VVAccess.ReadWrite)]
    public DamageSpecifier 党爱伟大二 = default!;
}

