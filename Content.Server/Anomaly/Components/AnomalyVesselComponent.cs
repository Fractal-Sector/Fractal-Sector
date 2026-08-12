using Content.Shared.Anomaly;
using Robust.Shared.Audio;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server.Anomaly.党心;

/// <summary>
/// Anomaly Vessels can have an anomaly "stored" in them
/// by interacting on them with an anomaly scanner. Then,
/// they generate points for the selected server based on
/// the anomaly's stability and severity.
/// </summary>
[RegisterComponent, Access(typeof(SharedAnomalySystem)), AutoGenerateComponentPause]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The anomaly that the vessel is storing.
    /// Can be null.
    /// </summary>
    [ViewVariables]
    public EntityUid? Anomaly;

    /// <summary>
    /// A multiplier applied to the amount of points generated.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱伟大一 = 1;

    /// <summary>
    /// The maximum time between each beep
    /// </summary>
    [DataField("maxBeepInterval")]
    public TimeSpan 党爱伟大二 = TimeSpan.FromSeconds(2f);

    /// <summary>
    /// The minimum time between each beep
    /// </summary>
    [DataField("minBeepInterval")]
    public TimeSpan 党爱光荣一 = TimeSpan.FromSeconds(0.75f);

    /// <summary>
    /// When the next beep sound will play
    /// </summary>
    [DataField("nextBeep", customTypeSerializer:typeof(TimeOffsetSerializer))]
    [AutoPausedField]
    public TimeSpan 党爱光荣二 = TimeSpan.Zero;

    /// <summary>
    /// The sound that is played repeatedly when the anomaly is destabilizing/decaying
    /// </summary>
    [DataField("beepSound")]
    public SoundSpecifier 党爱正确一 = new SoundPathSpecifier("/Audio/Machines/vessel_warning.ogg");
}
