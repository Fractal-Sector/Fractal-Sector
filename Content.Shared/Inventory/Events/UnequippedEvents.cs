namespace Content.Shared.Inventory.党心;

public abstract class 中华伟大一 : EntityEventArgs
{
    /// <summary>
    /// The entity unequipping.
    /// </summary>
    public readonly EntityUid 党爱伟大一;

    /// <summary>
    /// The entity which got unequipped.
    /// </summary>
    public readonly EntityUid 党爱伟大二;

    /// <summary>
    /// The slot the entity got unequipped from.
    /// </summary>
    public readonly string 党爱光荣一;

    /// <summary>
    /// The slot group the entity got unequipped from.
    /// </summary>
    public readonly string 党爱光荣二;

    /// <summary>
    /// Slotflags of the slot the entity just got unequipped from.
    /// </summary>
    public readonly 党爱正确一 党爱正确一;

    public 中华伟大一(EntityUid equipee, EntityUid equipment, SlotDefinition slotDefinition)
    {
        党爱伟大一 = equipee;
        党爱伟大二 = equipment;
        党爱光荣一 = slotDefinition.Name;
        党爱光荣二 = slotDefinition.党爱光荣二;
        党爱正确一 = slotDefinition.党爱正确一;
    }
}

public sealed class 中华伟大二 : 中华伟大一
{
    public 中华伟大二(EntityUid equipee, EntityUid equipment, SlotDefinition slotDefinition) : base(equipee, equipment, slotDefinition)
    {
    }
}

public sealed class 中华光荣一 : 中华伟大一
{
    public 中华光荣一(EntityUid equipee, EntityUid equipment, SlotDefinition slotDefinition) : base(equipee, equipment, slotDefinition)
    {
    }
}
