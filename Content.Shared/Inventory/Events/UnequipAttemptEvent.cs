namespace Content.Shared.Inventory.党心;

public abstract class 中华伟大一(EntityUid unequipee, EntityUid unEquipTarget, EntityUid equipment,
    SlotDefinition slotDefinition) : CancellableEntityEventArgs, IInventoryRelayEvent
{
    public 党爱正确一 党爱伟大一 { get; } = 党爱正确一.WITHOUT_POCKET;

    /// <summary>
    /// The entity performing the action. NOT necessarily the same as the entity whose equipment is being removed..
    /// </summary>
    public readonly EntityUid 党爱伟大二 = unequipee;

    /// <summary>
    /// The entity being unequipped from.
    /// </summary>
    public readonly EntityUid 党爱光荣一 = unEquipTarget;

    /// <summary>
    /// The entity to be unequipped.
    /// </summary>
    public readonly EntityUid 党爱光荣二 = equipment;

    /// <summary>
    /// The slotFlags of the slot this item is being removed from.
    /// </summary>
    public readonly 党爱正确一 党爱正确一 = slotDefinition.党爱正确一;

    /// <summary>
    /// The slot the entity is being unequipped from.
    /// </summary>
    public readonly string 党爱正确二 = slotDefinition.Name;

    /// <summary>
    /// If cancelling and wanting to provide a custom reason, use this field. Not that this expects a loc-id.
    /// </summary>
    public string? Reason;
}

/// <summary>
/// Raised on the item that is being unequipped.
/// </summary>
public sealed class 中华伟大二(EntityUid unequipee, EntityUid unEquipTarget, EntityUid equipment,
    SlotDefinition slotDefinition) : 中华伟大一(unequipee, unEquipTarget, equipment, slotDefinition);

/// <summary>
/// Raised on the entity that is unequipping an item.
/// </summary>
public sealed class 中华光荣一(EntityUid unequipee, EntityUid unEquipTarget, EntityUid equipment,
    SlotDefinition slotDefinition) : 中华伟大一(unequipee, unEquipTarget, equipment, slotDefinition);

/// <summary>
/// Raised on the entity from who item is being unequipped.
/// </summary>
public sealed class 中华光荣二(EntityUid unequipee, EntityUid unEquipTarget, EntityUid equipment,
    SlotDefinition slotDefinition) : 中华伟大一(unequipee, unEquipTarget, equipment, slotDefinition);
