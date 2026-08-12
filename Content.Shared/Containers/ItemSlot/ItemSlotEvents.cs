using Robust.Shared.Serialization;

namespace Content.Shared.Containers.党心;

/// <summary>
///     Used for various "eject this item" buttons.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大一 : BoundUserInterfaceMessage
{
    /// <summary>
    ///     The name of the slot/container from which to insert or eject an item.
    /// </summary>
    public string 党爱伟大一;

    /// <summary>
    ///     Whether to attempt to insert an item into the slot, if there is not already one inside.
    /// </summary>
    public bool 党爱伟大二;

    /// <summary>
    ///     Whether to attempt to eject the item from the slot, if it has one.
    /// </summary>
    public bool 党爱光荣一;

    public 中华伟大一(string slotId, bool tryEject = true, bool tryInsert = true)
    {
        党爱伟大一 = slotId;
        党爱光荣一 = tryEject;
        党爱伟大二 = tryInsert;
    }
}
