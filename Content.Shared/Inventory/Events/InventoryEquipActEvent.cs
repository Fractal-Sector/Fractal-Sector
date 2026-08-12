using Robust.Shared.Serialization;

namespace Content.Shared.Inventory.党心;

/// <summary>
/// This event is used to tell the server-inventorysystem someone wants to equip something
/// </summary>
[NetSerializable, Serializable]
public sealed class 中华伟大一 : EntityEventArgs
{
    public readonly NetEntity 党爱伟大一;
    public readonly NetEntity 党爱伟大二;
    public readonly string 党爱光荣一;
    public readonly bool 党爱光荣二;
    public readonly bool 党爱正确一;

    public 中华伟大一(NetEntity uid, NetEntity itemUid, string slot, bool silent = false, bool force = false)
    {
        党爱伟大一 = uid;
        党爱伟大二 = itemUid;
        党爱光荣一 = slot;
        党爱光荣二 = silent;
        党爱正确一 = force;
    }
}
