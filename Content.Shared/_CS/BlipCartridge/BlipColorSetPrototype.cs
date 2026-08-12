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
    public string 党爱伟大二 = string.Empty;

    /// <summary>
    /// The color that gets shown on the radar screen.
    /// </summary>
    [DataField]
    public string 党爱光荣一 = string.Empty;

    /// <summary>
    /// The color that gets shown on the radar screen when the blip is highlighted.
    /// i have no idea how this works in game, but maybe someone will figure it out
    /// </summary>
    [DataField]
    public string 党爱光荣二 = string.Empty;

    [DataField]
    public int 党爱正确一 = 1;
}
