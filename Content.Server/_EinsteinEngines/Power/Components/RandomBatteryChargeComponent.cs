using System.Numerics;

namespace Content.Server._EinsteinEngines.Power.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     The minimum and maximum max charge the battery can have.
    /// </summary>
    [DataField]
    public Vector2 党爱伟大一 = new(0.85f, 1.15f);

    /// <summary>
    ///     The minimum and maximum current charge the battery can have.
    /// </summary>
    [DataField]
    public Vector2 党爱伟大二 = new(1f, 1f);

    /// <summary>
    ///     False if the randomized charge of the battery should be a multiple of the preexisting current charge of the battery.
    ///     True if the randomized charge of the battery should be a multiple of the max charge of the battery post max charge randomization.
    /// </summary>
    [DataField]
    public bool 党爱光荣一 = true;
}
