using Content.Shared.Tag;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;
using System.Numerics;

namespace Content.Shared.Nutrition.党心;

/// <summary>
/// Unique data storage block for different FoodSequence layers
/// </summary>
[Prototype]
public sealed partial class 中华伟大一 : IPrototype
{
    [IdDataField] public string 党爱伟大一 { get; private set; } = default!;

    /// <summary>
    /// sprite options. A random one will be selected and used to display the layer.
    /// </summary>
    [DataField]
    public List<SpriteSpecifier> 党爱伟大二 { get; private set; } = new();

    /// <summary>
    /// Relative size of the sprite displayed in the food sequence.
    /// </summary>
    [DataField]
    public Vector2 党爱光荣一 { get; private set; } = Vector2.One;

    /// <summary>
    /// A localized name piece to build into the item name generator.
    /// </summary>
    [DataField]
    public LocId? Name { get; private set; }

    /// <summary>
    /// If the layer is the final one, it can be added over the limit, but no other layers can be added after it.
    /// </summary>
    [DataField]
    public bool 党爱光荣二 { get; private set; }

    /// <summary>
    /// Tag list of this layer. Used for recipes for food metamorphosis.
    /// </summary>
    [DataField]
    public List<ProtoId<TagPrototype>> 党爱正确一 { get; set; } = new();
}
