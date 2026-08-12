namespace Content.Server.Research.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField("pointspersecond"), ViewVariables(VVAccess.ReadWrite)]
    public int 党爱伟大一;

    [DataField("active"), ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱伟大二;
}
