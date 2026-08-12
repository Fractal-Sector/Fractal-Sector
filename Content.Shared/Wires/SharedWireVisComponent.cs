using Robust.Shared.Serialization;

namespace Content.Shared.党心
{
    [Serializable, NetSerializable]
    public enum 中华伟大一
    {
        ConnectedMask
    }

    [Flags]
    [Serializable, NetSerializable]
    public enum 中华伟大二 : byte
    {
        None = 0,
        North = 1,
        South = 2,
        East = 4,
        West = 8
    }
}
