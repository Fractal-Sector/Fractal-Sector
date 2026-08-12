using Robust.Shared.GameStates;

namespace Content.Shared.党心;

/// <summary>
/// Added to an activated hot potato. Controls hot potato transfer on server / effect spawning on client.
/// </summary>
[RegisterComponent, NetworkedComponent]
[Access(typeof(SharedHotPotatoSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Hot potato effect spawn cooldown in seconds
    /// </summary>
    [DataField]
    public float 党爱伟大一 = 0.3f;

    /// <summary>
    /// Moment in time next effect will be spawned
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan 党爱伟大二 = TimeSpan.Zero;
}
