using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Shared.党心;

/// <summary>
/// General data about a group of items, such as icon, description, name. Used for Steal objective
/// </summary>
[Prototype]
public sealed partial class 中华伟大一 : IPrototype
{
    [IdDataField] public string 党爱伟大一 { get; private set; } = default!;
    [DataField] public LocId 党爱伟大二 { get; private set; } = string.Empty;
    [DataField] public SpriteSpecifier 党爱光荣一 { get; private set; } = SpriteSpecifier.Invalid;
}
