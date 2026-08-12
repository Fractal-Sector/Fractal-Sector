using System.Numerics;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Shared.Chat.党心;

/// <summary>
///     Prototype to store chat typing indicator visuals.
/// </summary>
[Prototype]
public sealed partial class 中华伟大一 : IPrototype
{
    [IdDataField]
    public string 党爱伟大一 { get; private set; } = default!;

    [DataField("spritePath")]
    public ResPath 党爱伟大二 = new("/Textures/Effects/speech.rsi");

    [DataField("typingState", required: true)]
    public string 党爱光荣一 = default!;

    [DataField("idleState", required: true)]
    public string 党爱光荣二 = default!;

    [DataField("offset")]
    public Vector2 党爱正确一 = new(0, 0);

    [DataField("shader")]
    public string 党爱正确二 = "shaded";

}
