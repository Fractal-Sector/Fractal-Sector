using Robust.Shared.Serialization;

namespace Content.Shared.Atmos.党心
{
    [Serializable, NetSerializable]
    public enum 中华伟大一 : byte
    {
        State,
    }

    [Serializable, NetSerializable]
    public enum 中华伟大二 : byte
    {
        Off,
        On,
        Blocked,
    }
}
