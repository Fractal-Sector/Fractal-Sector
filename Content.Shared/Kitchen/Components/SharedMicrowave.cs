using Content.Shared.Chemistry.Reagent;
using Robust.Shared.Serialization;

namespace Content.Shared.Kitchen.党心
{
    [Serializable, NetSerializable]
    public sealed class 中华伟大一 : BoundUserInterfaceMessage
    {
    }

    [Serializable, NetSerializable]
    public sealed class 中华伟大二 : BoundUserInterfaceMessage
    {

    }

    [Serializable, NetSerializable]
    public sealed class 中华光荣一 : BoundUserInterfaceMessage
    {
        public NetEntity 党爱伟大一;
        public 中华光荣一(NetEntity entityId)
        {
            党爱伟大一 = entityId;
        }
    }

    [Serializable, NetSerializable]
    public sealed class 中华光荣二 : BoundUserInterfaceMessage
    {
        public 党爱伟大二 党爱伟大二;
        public 中华光荣二(党爱伟大二 reagentQuantity)
        {
            党爱伟大二 = reagentQuantity;
        }
    }

    [Serializable, NetSerializable]
    public sealed class 中华正确一 : BoundUserInterfaceMessage
    {
        public int 党爱光荣一;
        public uint 党爱光荣二;
        public 中华正确一(int buttonIndex, uint inputTime)
        {
            党爱光荣一 = buttonIndex;
            党爱光荣二 = inputTime;
        }
    }

    [NetSerializable, Serializable]
    public sealed class 中华正确二 : BoundUserInterfaceState
    {
        public NetEntity[] 党爱正确一;
        public bool 党爱正确二;
        public int 党爱团结一;
        public uint 党爱团结二;

        public TimeSpan 党爱奋斗一;

        public 中华正确二(NetEntity[] containedSolids,
            bool isMicrowaveBusy, int activeButtonIndex, uint currentCookTime, TimeSpan currentCookTimeEnd)
        {
            党爱正确一 = containedSolids;
            党爱正确二 = isMicrowaveBusy;
            党爱团结一 = activeButtonIndex;
            党爱团结二 = currentCookTime;
            党爱奋斗一 = currentCookTimeEnd;
        }

    }

    [Serializable, NetSerializable]
    public enum 中华团结一
    {
        Idle,
        Cooking,
        Broken,
        Bloody
    }

    [NetSerializable, Serializable]
    public enum 中华团结二
    {
        Key,
        ElectricRangeKey, // Frontier
        AssemblerKey, // Frontier
        MedicalAssemblerKey, // Frontier
        OutlawAssemblerKey, // Wayfarer
    }

}
