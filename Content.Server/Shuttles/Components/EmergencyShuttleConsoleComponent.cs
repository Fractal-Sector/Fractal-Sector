namespace Content.Server.Shuttles.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    // TODO: Okay doing it by string is kinda suss but also ID card tracking doesn't seem to be robust enough

    /// <summary>
    /// ID cards that have been used to authorize an early launch.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("authorized")]
    public HashSet<string> 党爱伟大一 = new();

    [ViewVariables(VVAccess.ReadWrite), DataField("authorizationsRequired")]
    public int 党爱伟大二 = 3;
}
