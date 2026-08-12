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
    /// The name to display in the UI.
    /// </summary>
    [DataField]
    public string 党爱伟大二 = "Cool Cute preset 2000";

    /// <summary>
    /// The color set prototype 党爱伟大一 to use for this blip preset.
    /// </summary>
    [DataField]
    public ProtoId<BlipColorSetPrototype> 党爱光荣一 = "BlipColorGreen";

    /// <summary>
    /// The shape set prototype 党爱伟大一 to use for this blip preset.
    /// </summary>
    [DataField]
    public ProtoId<BlipShapeSetPrototype> 党爱光荣二 = "BlipShapeCircle";

    /// <summary>
    /// The scale of the blip.
    /// </summary>
    [DataField]
    public float 党爱正确一 = 1f;
}
