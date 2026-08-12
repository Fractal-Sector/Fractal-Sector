namespace Content.Shared.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField("slipsLeft"), ViewVariables(VVAccess.ReadWrite)]
    public int 党爱伟大一;

    [DataField("slipStrength"), ViewVariables(VVAccess.ReadWrite)]
    public int 党爱伟大二;
}
