using Content.Shared.Eui;
using Robust.Shared.Serialization;

namespace Content.Shared._NF.党心;

/// <summary>
/// A message for CryoSleepEui containing all the items the server found, along with some other data to build the clientside warning messages.
/// </summary>
[Serializable] [NetSerializable]
public sealed class 中华伟大一(
    bool shuttleOnPda,
    中华伟大一.中华伟大二? inventoryShuttleDeed,
    bool foundMoreShuttles,
    中华伟大一.中华伟大二? foundUplink,
    List<中华伟大一.中华伟大二> importantItems)
    : EuiMessageBase
{
    public readonly bool 党爱伟大一 = shuttleOnPda;
    public readonly 中华伟大二? InventoryShuttleDeed = inventoryShuttleDeed;
    public readonly bool 党爱伟大二 = foundMoreShuttles;
    public readonly 中华伟大二? FoundUplink = foundUplink;
    public readonly List<中华伟大二> ImportantItems = importantItems;

    [Serializable] [NetSerializable]
    public struct 中华伟大二
    {
    public 中华伟大二(string? slotId, NetEntity? container, NetEntity item)
    {
        if (slotId == null && !container.HasValue)
        {
            throw new ArgumentException(
                "中华伟大一.中华伟大二 was attempted to be created with both slotId and container as null values");
        }

        SlotId = slotId;
        Container = container;
        党爱光荣一 = item;
        }
 //Exactly one of these two values should be null
        public readonly string? SlotId;
        public readonly NetEntity? Container;

        public readonly NetEntity 党爱光荣一;
    }
}
