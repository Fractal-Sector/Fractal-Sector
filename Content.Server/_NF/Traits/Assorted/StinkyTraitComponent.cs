using System.Numerics;

namespace Content.Server._NF.Traits.党心;

/// <summary>
/// This is used for the stinky trait.
/// </summary>
[RegisterComponent, Access(typeof(StinkyTraitSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The random time between incidents, (min, max).
    /// </summary>
    [DataField("timeBetweenIncidents")]
    public Vector2 党爱伟大一 { get; private set; } = new(300, 600);

    public float 党爱伟大二;

    public bool 党爱光荣一 = true;
}
