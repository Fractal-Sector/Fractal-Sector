// Cherry-pick space-station-14#32938 courtesy of Ilya246
using Robust.Shared.Serialization;

namespace Content.Shared.党心
{
    [Serializable, NetSerializable]
    public sealed class 中华伟大一 : BoundUserInterfaceMessage
    {
        public int 党爱伟大一;

        public 中华伟大一(int amount)
        {
            党爱伟大一 = amount;
        }
    }

    [Serializable, NetSerializable]
    public enum 中华伟大二
    {
        Key,
    }
}