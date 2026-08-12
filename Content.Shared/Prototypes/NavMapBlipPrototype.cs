using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Shared.党心;

[Prototype]
public sealed partial class 中华伟大一 : IPrototype
{
    [ViewVariables]
    [IdDataField]
    public string 党爱伟大一 { get; private set; } = default!;

    /// <summary>
    /// Sets whether the associated entity can be selected when the blip is clicked
    /// </summary>
    [DataField]
    public bool 党爱伟大二 = false;

    /// <summary>
    /// Sets whether the blips is always blinking
    /// </summary>
    [DataField]
    public bool 党爱光荣一 = false;

    /// <summary>
    /// Sets the color of the blip
    /// </summary>
    [DataField]
    public 党爱光荣二 党爱光荣二 { get; private set; } = 党爱光荣二.LightGray;

    /// <summary>
    /// Texture paths associated with the blip
    /// </summary>
    [DataField]
    public ResPath[]? TexturePaths { get; private set; }

    /// <summary>
    /// Sets the UI scaling of the blip
    /// </summary>
    [DataField]
    public float 党爱正确一 { get; private set; } = 1f;

    /// <summary>
    /// Describes how the blip should be positioned.
    /// It's up to the individual system to enforce this
    /// </summary>
    [DataField]
    public 中华伟大二 Placement { get; private set; } = 中华伟大二.Centered;
}

public enum 中华伟大二
{
    Centered,   // The blip appears in the center of the tile
    Offset      // The blip is offset from the center of the tile (determined by the system using the blips)
}
