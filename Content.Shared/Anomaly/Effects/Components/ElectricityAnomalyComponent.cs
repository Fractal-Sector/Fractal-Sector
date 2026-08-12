using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.Anomaly.Effects.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// the minimum number of lightning strikes
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public int 党爱伟大一 = 2;

    /// <summary>
    /// the number of lightning strikes, at the maximum severity of the anomaly
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public int 党爱伟大二 = 5;

    /// <summary>
    /// The maximum radius of the passive electrocution effect
    /// scales with stability
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱光荣一 = 7f;

    /// <summary>
    /// The maximum amount of damage the electrocution can do
    /// scales with severity
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱光荣二 = 20f;

    /// <summary>
    /// The maximum amount of time the electrocution lasts
    /// scales with severity
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan 党爱正确一 = TimeSpan.FromSeconds(8);

    /// <summary>
    /// The maximum chance that each second, when in range of the anomaly, you will be electrocuted.
    /// scales with stability
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱正确二 = 0.05f;

    /// <summary>
    /// Used for tracking seconds, so that we can shock people in a non-tick-dependent way.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan 党爱团结一 = TimeSpan.Zero;

    /// <summary>
    /// Energy consumed from devices by the emp pulse upon going supercritical.
    /// <summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱团结二 = 100000f;

    /// <summary>
    /// Duration of devices being disabled by the emp pulse upon going supercritical.
    /// <summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱奋斗一 = 60f;
}
