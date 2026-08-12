namespace Content.Server.党心;

/// <summary>
///     This generates a label for a nuclear bomb.
/// </summary>
/// <remarks>
///     This is a separate component because the fake nuclear bomb keg exists.
/// </remarks>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField] public LocId 党爱伟大一 = "nuke-label-nanotrasen";
    [DataField] public int 党爱伟大二 = 6;
}
