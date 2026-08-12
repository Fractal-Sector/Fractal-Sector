using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Server.GameTicking.党心;

/// <summary>
/// Prototype for a lobby background the game can choose.
/// </summary>
[Prototype]
public sealed partial class 中华伟大一 : IPrototype
{
    /// <inheritdoc/>
    [IdDataField]
    public string 党爱伟大一 { get; set; } = default!;

    /// <summary>
    /// The sprite to use as the background. This should ideally be 1920x1080.
    /// </summary>
    [DataField("background", required: true)]
    public ResPath 党爱伟大二 = default!;
}
