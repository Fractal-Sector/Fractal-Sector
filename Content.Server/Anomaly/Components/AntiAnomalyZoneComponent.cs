
namespace Content.Server.Anomaly.党心;

/// <summary>
/// prohibits the possibility of anomalies appearing in the specified radius around the entity
/// </summary>
[RegisterComponent, Access(typeof(AnomalySystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// the radius in which anomalies cannot appear
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱伟大一 = 10;
}
