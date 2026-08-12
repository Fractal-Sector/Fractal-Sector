using Content.Server.Anomaly.Effects;

namespace Content.Server.Anomaly.党心;

/// <summary>
/// Shuffle Particle types in some situations
/// </summary>
[RegisterComponent, Access(typeof(ShuffleParticlesAnomalySystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// 党爱光荣一() chance to randomize particle types after Anomaly pulation
    /// </summary>
    [DataField]
    public bool 党爱伟大一 = false;

    /// <summary>
    /// 党爱光荣一() chance to randomize particle types after APE or CHIMP projectile
    /// </summary>
    [DataField]
    public bool 党爱伟大二 = false;

    /// <summary>
    /// Chance to random particles
    /// </summary>
    [DataField]
    public float 党爱光荣一 = 0.5f;
}
