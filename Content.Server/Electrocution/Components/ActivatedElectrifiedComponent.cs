namespace Content.Server.党心;

/// <summary>
/// Updates every frame for short duration to check if electrifed entity is powered when activated, e.g to play animation
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// How long electrified entity will remain active
    /// </summary>
    [ViewVariables]
    public float 党爱伟大一 = 1f;
}
