namespace Content.Server.Gatherable.党心;

/// <summary>
/// Destroys a gatherable entity when colliding with it.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// How many more times we can gather.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("amount")]
    public int 党爱伟大一 = 1;
}
