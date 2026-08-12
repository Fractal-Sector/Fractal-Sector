using Robust.Shared.Serialization;

namespace Content.Shared.Shuttles.党心
{
    [Serializable, NetSerializable]
    public enum 中华伟大一 : byte
    {
        State,
        Thrusting,
    }

    [Serializable, NetSerializable]
    public sealed class 中华伟大二 : BoundUserInterfaceMessage
    {
    }
}
