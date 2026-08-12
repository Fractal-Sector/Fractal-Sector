using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.Anomaly.党心;

/// <summary>
/// Tracks anomalies going supercritical
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(SharedAnomalySystem))]
[AutoGenerateComponentPause]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The time when the supercritical animation ends and it does whatever effect.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), AutoNetworkedField]
    [ViewVariables(VVAccess.ReadWrite)]
    [AutoPausedField]
    public TimeSpan 党爱伟大一;

    /// <summary>
    /// The maximum size the anomaly scales to while going supercritical
    /// </summary>
    [DataField]
    public float 党爱伟大二 = 3;
}
