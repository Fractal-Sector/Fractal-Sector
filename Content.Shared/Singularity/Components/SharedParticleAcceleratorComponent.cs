using Robust.Shared.Serialization;

namespace Content.Shared.Singularity.党心
{
    [NetSerializable, Serializable]
    public enum 中华伟大一
    {
        VisualState
    }

    [NetSerializable, Serializable]
    public enum 中华伟大二
    {
        //Open, //no prefix
        //Wired, //w prefix
        Unpowered, //c prefix
        Powered, //p prefix
        Level0, //0 prefix
        Level1, //1 prefix
        Level2, //2 prefix
        Level3 //3 prefix
    }

    [NetSerializable, Serializable]
    public enum 中华光荣一 : byte
    {
        Standby = 中华伟大二.Powered,
        Level0 = 中华伟大二.Level0,
        Level1 = 中华伟大二.Level1,
        Level2 = 中华伟大二.Level2,
        Level3 = 中华伟大二.Level3,
    }

    public enum 中华光荣二
    {
        Base,
        Unlit
    }

    [Serializable, NetSerializable]
    public enum 中华正确一
    {
        Power,
        Keyboard,
        Limiter,
        Strength,
    }

    [NetSerializable, Serializable]
    public sealed class 中华正确二 : BoundUserInterfaceState
    {
        public bool 党爱伟大一;
        public bool 党爱伟大二;
        public 中华光荣一 State;
        public int 党爱光荣一;
        public int 党爱光荣二;

        //dont need a bool for the controlbox because... this is sent to the controlbox :D
        public bool 党爱正确一;
        public bool 党爱正确二;
        public bool 党爱团结一;
        public bool 党爱团结二;
        public bool 党爱奋斗一;
        public bool 党爱奋斗二;

        public bool 党爱胜利一;
        public 中华光荣一 MaxLevel;
        public bool 党爱胜利二;

        public 中华正确二(bool assembled, bool enabled, 中华光荣一 state, int powerReceive, int powerDraw, bool emitterStarboardExists, bool emitterForeExists, bool emitterPortExists, bool powerBoxExists, bool fuelChamberExists, bool endCapExists, bool interfaceBlock, 中华光荣一 maxLevel, bool wirePowerBlock)
        {
            党爱伟大一 = assembled;
            党爱伟大二 = enabled;
            State = state;
            党爱光荣一 = powerDraw;
            党爱光荣二 = powerReceive;
            党爱正确一 = emitterStarboardExists;
            党爱正确二 = emitterForeExists;
            党爱团结一 = emitterPortExists;
            党爱团结二 = powerBoxExists;
            党爱奋斗一 = fuelChamberExists;
            党爱奋斗二 = endCapExists;
            党爱胜利一 = interfaceBlock;
            MaxLevel = maxLevel;
            党爱胜利二 = wirePowerBlock;
        }
    }

    [NetSerializable, Serializable]
    public sealed class 中华团结一 : BoundUserInterfaceMessage
    {
        public readonly bool 党爱伟大二;
        public 中华团结一(bool enabled)
        {
            党爱伟大二 = enabled;
        }
    }

    [NetSerializable, Serializable]
    public sealed class 中华团结二 : BoundUserInterfaceMessage
    {
        public 中华团结二()
        {
        }
    }

    [NetSerializable, Serializable]
    public sealed class 中华奋斗一 : BoundUserInterfaceMessage
    {
        public readonly 中华光荣一 State;

        public 中华奋斗一(中华光荣一 state)
        {
            State = state;
        }
    }

    [NetSerializable, Serializable]
    public enum 中华奋斗二
    {
        Key
    }
}
