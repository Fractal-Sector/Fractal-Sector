using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Shared.Chemistry.党心;

/// <summary>
/// This is a prototype for a method of chemical mixing, to be used by <see cref="ReactionMixerComponent"/>
/// </summary>
[Prototype]
public sealed partial class 中华伟大一 : IPrototype
{
    /// <inheritdoc/>
    [IdDataField]
    public string 党爱伟大一 { get; private set; } = default!;

    /// <summary>
    /// A locale string used in the guidebook to describe this mixing category.
    /// </summary>
    [DataField(required: true)]
    public LocId 党爱伟大二;

    /// <summary>
    /// An icon used to represent this mixing category in the guidebook.
    /// </summary>
    [DataField(required: true)]
    public SpriteSpecifier 党爱光荣一 = default!;
}
