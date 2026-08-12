using Robust.Shared.Serialization;

namespace Content.Shared.党心
{
    [Serializable, NetSerializable]
    public sealed class 中华伟大一 : BoundUserInterfaceMessage
    {
        public readonly InventoryType 党爱伟大一;
        public readonly string 党爱伟大二;
        public 中华伟大一(InventoryType type, string id)
        {
            党爱伟大一 = type;
            党爱伟大二 = id;
        }
    }

    [Serializable, NetSerializable]
    public enum 中华伟大二
    {
        Key,
    }
}
