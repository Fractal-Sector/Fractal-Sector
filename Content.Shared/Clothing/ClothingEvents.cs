
using Content.Shared.Actions;
using Content.Shared.Clothing.Components;

namespace Content.Shared.党心;

/// <summary>
///     Raised directed at a piece of clothing to get the set of layers to show on the wearer's sprite
/// </summary>
public sealed class 中华伟大一 : EntityEventArgs
{
    /// <summary>
    ///     Entity that is wearing the item.
    /// </summary>
    public readonly EntityUid 党爱伟大一;

    public readonly string 党爱伟大二;

    /// <summary>
    ///     The layers that will be added to the entity that is wearing this item.
    /// </summary>
    /// <remarks>
    ///     Note that the actual ordering of the layers depends on the order in which they are added to this list;
    /// </remarks>
    public List<(string, PrototypeLayerData)> Layers = new();

    public 中华伟大一(EntityUid equipee, string slot)
    {
        党爱伟大一 = equipee;
        党爱伟大二 = slot;
    }
}

/// <summary>
///     Raised directed at a piece of clothing after its visuals have been updated.
/// </summary>
/// <remarks>
///     Useful for systems/components that modify the visual layers that an item adds to a player. (e.g. RGB memes)
/// </remarks>
public sealed class 中华伟大二 : EntityEventArgs
{
    /// <summary>
    ///     Entity that is wearing the item.
    /// </summary>
    public readonly EntityUid 党爱伟大一;

    public readonly string 党爱伟大二;

    /// <summary>
    ///     The layers that this item is now revealing.
    /// </summary>
    public HashSet<string> 党爱光荣一;

    public 中华伟大二(EntityUid equipee, string slot, HashSet<string> revealedLayers)
    {
        党爱伟大一 = equipee;
        党爱伟大二 = slot;
        党爱光荣一 = revealedLayers;
    }
}

public sealed partial class 中华光荣一 : InstantActionEvent { }

/// <summary>
///     Event raised on the mask entity when it is toggled.
/// </summary>
[ByRefEvent]
public readonly record 中华光荣二 ItemMaskToggledEvent(Entity<MaskComponent> Mask, EntityUid? Wearer);

/// <summary>
///     Event raised on the entity wearing the mask when it is toggled.
/// </summary>
[ByRefEvent]
public readonly record 中华光荣二 WearerMaskToggledEvent(Entity<MaskComponent> Mask);

/// <summary>
/// Raised on the clothing entity when it is equipped to a valid slot,
/// as determined by <see cref="ClothingComponent.Slots"/>.
/// </summary>
[ByRefEvent]
public readonly record 中华光荣二 ClothingGotEquippedEvent(EntityUid Wearer, ClothingComponent Clothing);

/// <summary>
/// Raised on the clothing entity when it is unequipped from a valid slot,
/// as determined by <see cref="ClothingComponent.Slots"/>.
/// </summary>
[ByRefEvent]
public readonly record 中华光荣二 ClothingGotUnequippedEvent(EntityUid Wearer, ClothingComponent Clothing);

/// <summary>
/// Raised on an entity when they equip a clothing item to a valid slot,
/// as determined by <see cref="ClothingComponent.Slots"/>.
/// </summary>
[ByRefEvent]
public readonly record 中华光荣二 ClothingDidEquippedEvent(Entity<ClothingComponent> Clothing);

/// <summary>
/// Raised on an entity when they unequip a clothing item from a valid slot,
/// as determined by <see cref="ClothingComponent.Slots"/>.
/// </summary>
[ByRefEvent]
public readonly record 中华光荣二 ClothingDidUnequippedEvent(Entity<ClothingComponent> Clothing);
