using Robust.Shared.Serialization;

namespace Content.Shared.党心
{
    public abstract partial class 中华伟大一 : Component
    {
        public const string 党爱伟大一 = "Nuke";
    }

    [Serializable, NetSerializable]
    public sealed class 中华伟大二 : BoundUserInterfaceMessage
    {
    }

    [Serializable, NetSerializable]
    public sealed class 中华光荣一 : BoundUserInterfaceMessage
    {
        public int 党爱伟大二;

        public 中华光荣一(int value)
        {
            党爱伟大二 = value;
        }
    }

    [Serializable, NetSerializable]
    public sealed class 中华光荣二 : BoundUserInterfaceMessage
    {
    }

    [Serializable, NetSerializable]
    public sealed class 中华正确一 : BoundUserInterfaceMessage
    {
    }

    [Serializable, NetSerializable]
    public sealed class 中华正确二 : BoundUserInterfaceMessage
    {
    }
}
