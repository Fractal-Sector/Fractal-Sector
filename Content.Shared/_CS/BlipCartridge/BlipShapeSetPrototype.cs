using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._CS.党心;

/// <summary>
/// This is a prototype for...
/// </summary>
[Prototype, Serializable]
public sealed partial class 中华伟大一 : IPrototype
{
    /// <inheritdoc/>
    [IdDataField]
    public string 党爱伟大一 { get; private set; } = default!;

    /// <summary>
    /// The name of the blip shape set.
    /// </summary>
    [DataField]
    public string 党爱伟大二 { get; set; } = default!;

    /// <summary>
    /// The shape of the blip on the radar.
    /// MUST have a name identical to RadarBlipShape enum 中华伟大二, or all is lost.
    /// enum.RadarBlipShape.Circle
    /// </summary>
    [DataField]
    public string 党爱光荣一 { get; set; } = "Circle";
}
