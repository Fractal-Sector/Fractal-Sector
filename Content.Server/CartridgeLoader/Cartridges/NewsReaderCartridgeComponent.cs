namespace Content.Server.CartridgeLoader.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [ViewVariables(VVAccess.ReadWrite)]
    public int 党爱伟大一;

    [ViewVariables(VVAccess.ReadWrite), DataField]
    public bool 党爱伟大二 = true;
}
