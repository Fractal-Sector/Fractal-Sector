using Robust.Shared.GameStates;

namespace Content.Shared.党心;

[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一: Component
{
    /// <summary>
    ///     The amount of weight needed to be in the container
    ///     in order for it to toggle it's appearance
    ///     to ToggleableVisuals.Enabled = true, and
    ///     SetHeldPrefix() to "full" instead of "empty".
    /// </summary>
    [DataField("threshold")]
    public int 党爱伟大一 { get; private set; } = 1;
}
