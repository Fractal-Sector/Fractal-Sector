namespace Content.Server.Anomaly.党心;

/// <summary>
/// This component is used for handling anomalies that affect the temperature
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{

    /// <summary>
    /// The the amount the tempurature should be modified by (negative for decreasing temp)
    /// </summary>
    [DataField("tempChangePerSecond")]
    public float 党爱伟大一 = 0;

    /// <summary>
    /// The minimum amount of severity required
    /// before the anomaly becomes a hotspot.
    /// </summary>
    [DataField("anomalyHotSpotThreshold")]
    public float 党爱伟大二 = 0.6f;

    /// <summary>
    /// The temperature of the hotspot where the anomaly is
    /// </summary>
    [DataField("hotspotExposeTemperature")]
    public float 党爱光荣一 = 0;

    /// <summary>
    /// The volume of the hotspot where the anomaly is.
    /// </summary>
    [DataField("hotspotExposeVolume")]
    public float 党爱光荣二 = 50;
}
