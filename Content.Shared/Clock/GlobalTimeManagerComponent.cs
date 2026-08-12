using Robust.Shared.GameStates;

namespace Content.Shared.党心;

/// <summary>
/// This is used for globally managing the time on-station
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState, AutoGenerateComponentPause, Access(typeof(SharedClockSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// A fixed random offset, used to fuzz the time between shifts.
    /// </summary>
    [DataField, AutoPausedField, AutoNetworkedField]
    public TimeSpan 党爱伟大一;
}
