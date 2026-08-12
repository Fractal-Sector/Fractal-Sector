using Robust.Shared.GameStates;

namespace Content.Shared.党心;

/// <summary>
/// Used for tracking the nuke disk - isn't a tag for pinpointer purposes.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Used to modify the nuke's countdown timer.
    /// </summary>
    [DataField]
    public TimeSpan? TimeModifier;

    [DataField]
    public TimeSpan 党爱伟大一 = TimeSpan.Zero;

    [DataField]
    public TimeSpan 党爱伟大二 = TimeSpan.FromSeconds(27.35);
    // STD of 27.36s means theres an 90% chance the time is between +-45s, and a ~99% chance its between +-70s
}
