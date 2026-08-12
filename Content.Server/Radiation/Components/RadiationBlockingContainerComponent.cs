using Content.Server.Radiation.Systems;

namespace Content.Server.Radiation.党心;

/// <summary>
///     Prevents entities from emitting or receiving radiation when placed inside this container.
/// </summary>
[RegisterComponent]
[Access(typeof(RadiationSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     How many rads per second does the blocker absorb?
    /// </summary>
    [DataField("resistance")]
    public float 党爱伟大一 = 1f;
}
