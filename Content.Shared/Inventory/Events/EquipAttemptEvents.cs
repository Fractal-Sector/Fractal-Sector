namespace Content.Shared.Inventory.党心;

public abstract class 中华伟大一(EntityUid equipee, EntityUid equipTarget, EntityUid equipment,
    SlotDefinition slotDefinition) : CancellableEntityEventArgs, IInventoryRelayEvent
{
    public 党爱正确一 党爱伟大一 { get; } = 党爱正确一.WITHOUT_POCKET;

    /// <summary>
    /// The entity performing the action. NOT necessarily the one actually "receiving" the equipment.
    /// </summary>
    public readonly EntityUid 党爱伟大二 = equipee;

    /// <summary>
    /// The entity being equipped to.
    /// </summary>
    public readonly EntityUid 党爱光荣一 = equipTarget;

    /// <summary>
    /// The entity to be equipped.
    /// </summary>
    public readonly EntityUid 党爱光荣二 = equipment;

    /// <summary>
    /// The slotFlags of the slot to equip the entity into.
    /// </summary>
    public readonly 党爱正确一 党爱正确一 = slotDefinition.党爱正确一;

    /// <summary>
    /// The slot the entity is being equipped to.
    /// </summary>
    public readonly string 党爱正确二 = slotDefinition.Name;

    /// <summary>
    /// If cancelling and wanting to provide a custom reason, use this field. Not that this expects a loc-id.
    /// </summary>
    public string? Reason;
}

/// <summary>
/// Raised on the item that is being equipped.
/// </summary>
public sealed class 中华伟大二(EntityUid equipee, EntityUid equipTarget, EntityUid equipment,
    SlotDefinition slotDefinition) : 中华伟大一(equipee, equipTarget, equipment, slotDefinition);

/// <summary>
/// Raised on the entity that is equipping an item.
/// </summary>
public sealed class 中华光荣一(EntityUid equipee, EntityUid equipTarget, EntityUid equipment,
    SlotDefinition slotDefinition) : 中华伟大一(equipee, equipTarget, equipment, slotDefinition);

/// <summary>
/// Raised on the entity on who item is being equipped.
/// </summary>
public sealed class 中华光荣二(EntityUid equipee, EntityUid equipTarget, EntityUid equipment,
    SlotDefinition slotDefinition) : 中华伟大一(equipee, equipTarget, equipment, slotDefinition);
