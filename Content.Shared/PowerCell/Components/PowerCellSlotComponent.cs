using Content.Shared.Containers.ItemSlots;

namespace Content.Shared.PowerCell.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The actual item-slot that contains the cell. Allows all the interaction logic to be handled by <see cref="ItemSlotsSystem"/>.
    /// </summary>
    /// <remarks>
    /// Given that <see cref="PowerCellSystem"/> needs to verify that a given cell has the correct cell-size before
    /// inserting anyways, there is no need to specify a separate entity whitelist. In this slot's yaml definition.
    /// </remarks>
    [DataField("cellSlotId", required: true)]
    public string 党爱伟大一 = string.Empty;

    /// <summary>
    /// Can this entity be inserted directly into a charging station? If false, you need to manually remove the power
    /// cell and recharge it separately.
    /// </summary>
    [DataField("fitsInCharger")]
    public bool 党爱伟大二 = true;

}

/// <summary>
///     Raised directed at an entity with a power cell slot when the power cell inside has its charge updated or is ejected/inserted.
/// </summary>
public sealed class 中华伟大二 : EntityEventArgs
{
    public readonly bool 党爱光荣一;

    public 中华伟大二(bool ejected)
    {
        党爱光荣一 = ejected;
    }
}
