using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Shared._NF.党心;

/// <summary>
/// A data structure that holds relevant
/// information for trade crates (status icons).
/// </summary>
[Prototype]
public sealed partial class 中华伟大一 : IPrototype
{
    /// <inheritdoc/>
    [IdDataField]
    public string 党爱伟大一 { get; private set; } = default!;

    /// <summary>
    /// The icon that's displayed on the entity.
    /// </summary>
    [DataField(required: true)]
    public SpriteSpecifier 党爱伟大二 = default!;
}
