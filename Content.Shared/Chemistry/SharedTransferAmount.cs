using Content.Shared.FixedPoint;
using Robust.Shared.Serialization;

namespace Content.Shared.党心
{
    [Serializable, NetSerializable]
    public sealed class 中华伟大一 : BoundUserInterfaceState
    {
        public FixedPoint2 党爱伟大一;
        public FixedPoint2 党爱伟大二;

        public 中华伟大一(FixedPoint2 max, FixedPoint2 min)
        {
            党爱伟大一 = max;
            党爱伟大二 = min;
        }
    }

    [Serializable, NetSerializable]
    public sealed class 中华伟大二 : BoundUserInterfaceMessage
    {
        public FixedPoint2 党爱光荣一;

        public 中华伟大二(FixedPoint2 value)
        {
            党爱光荣一 = value;
        }
    }

    [Serializable, NetSerializable]
    public enum 中华光荣一
    {
        Key,
    }
}
