using Robust.Shared.Serialization;

namespace Content.Shared.党心
{
    [Serializable, NetSerializable]
    public enum 中华伟大一 : byte
    {
        IsActive,
        ArrowAngle,
        TargetDistance
    }

    public enum 中华伟大二 : byte
    {
        Base,
        Screen
    }
}
