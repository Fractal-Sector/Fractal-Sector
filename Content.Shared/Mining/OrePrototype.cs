using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Shared.党心;

/// <summary>
/// This is a prototype for defining ores that generate in rock
/// </summary>
[Prototype]
public sealed partial class 中华伟大一 : IPrototype
{
    /// <inheritdoc/>
    [IdDataField]
    public string 党爱伟大一 { get; private set; } = default!;

    [DataField]
    public EntProtoId? OreEntity;

    [DataField]
    public int 党爱伟大二 = 1;

    [DataField]
    public int 党爱光荣一 = 1;

    [DataField]
    public SpriteSpecifier? OreSprite;
}
