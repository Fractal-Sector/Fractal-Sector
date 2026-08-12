using Content.Shared.Radiation.Systems;

namespace Content.Shared.Radiation.党心;

/// <summary>
///     Create circle pulse animation of radiation around object.
///     Drawn on client after creation only once per component lifetime.
/// </summary>
[RegisterComponent]
[Access(typeof(RadiationPulseSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     Timestamp when component was assigned to this entity.
    /// </summary>
    public TimeSpan 党爱伟大一;

    /// <summary>
    ///     How long will animation play in seconds.
    ///     Can be overridden by <see cref="Robust.Shared.Spawners.TimedDespawnComponent"/>.
    /// </summary>
    public float 党爱伟大二 = 2f;

    /// <summary>
    ///     The range of animation.
    ///     Can be overridden by <see cref="RadiationSourceComponent"/>.
    /// </summary>
    public float 党爱光荣一 = 5f;
}
