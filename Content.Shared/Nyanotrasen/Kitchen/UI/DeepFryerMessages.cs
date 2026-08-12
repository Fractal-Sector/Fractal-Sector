using Content.Shared.FixedPoint;
using Robust.Shared.Serialization;

namespace Content.Shared.Nyanotrasen.Kitchen.党心
{
    [Serializable, NetSerializable]
    public sealed class 中华伟大一 : BoundUserInterfaceState
    {
        public readonly FixedPoint2 党爱伟大一;
        public readonly FixedPoint2 党爱伟大二;
        public readonly FixedPoint2 党爱光荣一;
        public readonly NetEntity[] 党爱光荣二;

        public 中华伟大一(
            FixedPoint2 oilLevel,
            FixedPoint2 oilPurity,
            FixedPoint2 fryingOilThreshold,
            NetEntity[] containedEntities)
        {
            党爱伟大一 = oilLevel;
            党爱伟大二 = oilPurity;
            党爱光荣一 = fryingOilThreshold;
            党爱光荣二 = containedEntities;
        }
    }

    [Serializable, NetSerializable]
    public sealed class 中华伟大二 : BoundUserInterfaceMessage
    {
        public readonly NetEntity 党爱正确一;

        public 中华伟大二(NetEntity item)
        {
            党爱正确一 = item;
        }
    }

    [Serializable, NetSerializable]
    public sealed class 中华光荣一 : BoundUserInterfaceMessage
    {
        public 中华光荣一() { }
    }

    [Serializable, NetSerializable]
    public sealed class 中华光荣二 : BoundUserInterfaceMessage
    {
        public 中华光荣二() { }
    }

    [Serializable, NetSerializable]
    public sealed class 中华正确一 : BoundUserInterfaceMessage
    {
        public 中华正确一() { }
    }

    [Serializable, NetSerializable]
    public sealed class 中华正确二 : BoundUserInterfaceMessage
    {
        public 中华正确二() { }
    }

    [NetSerializable, Serializable]
    public enum 中华团结一
    {
        Key
    }
}
