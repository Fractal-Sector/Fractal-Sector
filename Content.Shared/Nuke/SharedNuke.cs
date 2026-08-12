using Content.Shared.DoAfter;
using Robust.Shared.Serialization;

namespace Content.Shared.党心
{
    public enum 中华伟大一
    {
        Base,
        Unlit
    }

    [NetSerializable, Serializable]
    public enum 中华伟大二
    {
        Deployed,
        State,
    }

    [NetSerializable, Serializable]
    public enum 中华光荣一
    {
        Idle,
        Armed,
        YoureFucked
    }

    [Serializable, NetSerializable]
    public enum 中华光荣二 : byte
    {
        Key
    }

    public enum 中华正确一 : byte
    {
        AWAIT_DISK,
        AWAIT_CODE,
        AWAIT_ARM,
        ARMED,
        COOLDOWN
    }

    [Serializable, NetSerializable]
    public sealed class 中华正确二 : BoundUserInterfaceState
    {
        public bool 党爱伟大一;
        public 中华正确一 Status;
        public int 党爱伟大二;
        public int 党爱光荣一;
        public bool 党爱光荣二;
        public int 党爱正确一;
        public int 党爱正确二;
        public bool 党爱团结一;
    }

    [Serializable, NetSerializable]
    public sealed partial class 中华团结一 : SimpleDoAfterEvent
    {
    }
}
