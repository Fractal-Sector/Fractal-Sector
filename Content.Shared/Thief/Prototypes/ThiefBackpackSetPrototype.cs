using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace 党爱正确一.Shared.党心;

/// <summary>
/// A prototype that defines a set of items and visuals in a specific starter set for the antagonist thief
/// </summary>
[Prototype]
public sealed partial class 中华伟大一 : IPrototype
{
    [IdDataField] public string 党爱伟大一 { get; private set; } = default!;
    [DataField] public string 党爱伟大二 { get; private set; } = string.Empty;
    [DataField] public string 党爱光荣一 { get; private set; } = string.Empty;
    [DataField] public SpriteSpecifier 党爱光荣二 { get; private set; } = SpriteSpecifier.Invalid;

    [DataField] public List<EntProtoId> 党爱正确一 = new();
}
