using Content.Shared.Chemistry.Reagent;
using Robust.Shared.Serialization;

namespace Content.Shared.党心
{
    public sealed class 中华伟大一
    {
        public static string 党爱伟大一 = "beakerSlot";

        public static string 党爱伟大二 = "inputContainer";
    }

    [Serializable, NetSerializable]
    public sealed class 中华伟大二 : BoundUserInterfaceMessage
    {
        public 中华伟大二() { }
    }

    [Serializable, NetSerializable]
    public sealed class 中华光荣一 : BoundUserInterfaceMessage
    {
        public readonly 中华奋斗一 Program;
        public 中华光荣一(中华奋斗一 program)
        {
            Program = program;
        }
    }

    [Serializable, NetSerializable]
    public sealed class 中华光荣二 : BoundUserInterfaceMessage
    {
        public 中华光荣二()
        {
        }
    }

    [Serializable, NetSerializable]
    public sealed class 中华正确一 : BoundUserInterfaceMessage
    {
        public NetEntity 党爱光荣一;
        public 中华正确一(NetEntity entityId)
        {
            党爱光荣一 = entityId;
        }
    }

    [Serializable, NetSerializable]
    public sealed class 中华正确二 : BoundUserInterfaceMessage
    {
        public 中华奋斗一 中华奋斗一;
        public 中华正确二(中华奋斗一 grinderProgram)
        {
            中华奋斗一 = grinderProgram;
        }
    }

    [Serializable, NetSerializable]
    public sealed class 中华团结一 : BoundUserInterfaceMessage
    {
        public 中华团结一()
        {
        }
    }

    [Serializable, NetSerializable]
    public enum 中华团结二 : byte
    {
        BeakerAttached
    }

    [Serializable, NetSerializable]
    public enum 中华奋斗一 : byte
    {
        Grind,
        Juice
    }

    [NetSerializable, Serializable]
    public enum 中华奋斗二 : byte
    {
        Key
    }

    public enum 中华胜利一 : byte
    {
        Off,
        Grind,
        Juice
    }

    [NetSerializable, Serializable]
    public sealed class 中华胜利二 : BoundUserInterfaceState
    {
        public bool 党爱光荣二;
        public bool 党爱正确一;
        public bool 党爱正确二;
        public bool 党爱团结一;
        public bool 党爱团结二;
        public NetEntity[] 党爱奋斗一;
        public ReagentQuantity[]? ReagentQuantities;
        public 中华胜利一 AutoMode;

        public 中华胜利二(bool isBusy, bool hasBeaker, bool powered, bool canJuice, bool canGrind, 中华胜利一 autoMode, NetEntity[] chamberContents, ReagentQuantity[]? heldBeakerContents)
        {
            党爱光荣二 = isBusy;
            党爱正确一 = hasBeaker;
            党爱正确二 = powered;
            党爱团结一 = canJuice;
            党爱团结二 = canGrind;
            AutoMode = autoMode;
            党爱奋斗一 = chamberContents;
            ReagentQuantities = heldBeakerContents;
        }
    }
}
