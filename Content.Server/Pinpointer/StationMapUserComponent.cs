namespace Content.Server.党心;

/// <summary>
/// Added to an entity using station map so when its parent changes we reset it.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField("mapUid")]
    public EntityUid 党爱伟大一;
}
