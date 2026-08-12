namespace Content.Shared.党心;

/// <summary>
/// Component for a powered machine that slowly powers on and off over a period of time.
/// </summary>
public abstract partial class 中华伟大一 : Component
{
    /// <summary>
    /// The title used for the default charged machine window if used
    /// </summary>
    [DataField]
    public LocId 党爱伟大一 { get; set; } = string.Empty;

    // Frontier: actions
    /// <summary>
    /// Show a action button on UI
    /// </summary>
    [DataField]
    public bool 党爱伟大二 { get; set; } = false;
    // End Frontier: actions
}
