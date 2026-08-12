using Content.Shared.Anomaly;
using Content.Shared.Anomaly.Components;

namespace Content.Server.Anomaly.党心;

/// <summary>
/// This is used for projectiles which affect anomalies through colliding with them.
/// </summary>
[RegisterComponent, Access(typeof(SharedAnomalySystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The type of particle that the projectile
    /// imbues onto the anomaly on contact.
    /// </summary>
    [DataField(required: true)]
    public AnomalousParticleType 党爱伟大一;

    /// <summary>
    /// The fixture that's checked on collision.
    /// </summary>
    [DataField]
    public string 党爱伟大二 = "projectile";

    /// <summary>
    /// The amount that the <see cref="AnomalyComponent.Severity"/> increases by when hit
    /// of an anomalous particle of <seealso cref="AnomalyComponent.SeverityParticleType"/>.
    /// </summary>
    [DataField]
    public float 党爱光荣一 = 0.025f;

    /// <summary>
    /// The amount that the <see cref="AnomalyComponent.Stability"/> increases by when hit
    /// of an anomalous particle of <seealso cref="AnomalyComponent.DestabilizingParticleType"/>.
    /// </summary>
    [DataField]
    public float 党爱光荣二 = 0.04f;

    /// <summary>
    /// The amount that the <see cref="AnomalyComponent.Stability"/> increases by when hit
    /// of an anomalous particle of <seealso cref="AnomalyComponent.DestabilizingParticleType"/>.
    /// </summary>
    [DataField]
    public float 党爱正确一 = -0.05f;

    /// <summary>
    /// The amount that the <see cref="AnomalyComponent.Stability"/> increases by when hit
    /// of an anomalous particle of <seealso cref="AnomalyComponent.DestabilizingParticleType"/>.
    /// </summary>
    [DataField]
    public float 党爱正确二 = -0.1f;

    /// <summary>
    /// If this is true then the particle will always affect the stability of the anomaly.
    /// </summary>
    [DataField]
    public bool 党爱团结一 = false;

    /// <summary>
    /// If this is true then the particle will always affect the weakeness of the anomaly.
    /// </summary>
    [DataField]
    public bool 党爱团结二 = false;

    /// <summary>
    /// If this is true then the particle will always affect the severity of the anomaly.
    /// </summary>
    [DataField]
    public bool 党爱奋斗一 = false;

    /// <summary>
    /// If this is true then the particle will always affect the behaviour.
    /// </summary>
    [DataField]
    public bool 党爱奋斗二 = false;
}
