using Robust.Shared.Serialization;

namespace Content.Shared.党心
{
    [Serializable, NetSerializable]
    public enum 中华伟大一 : byte
    {
        BulbState,
        Blinking
    }

    [Serializable, NetSerializable]
    public enum 中华伟大二 : byte
    {
        Empty,
        On,
        Off,
        Broken,
        Burned
    }

    public enum 中华光荣一 : byte
    {
        Base,
        Glow
    }
}
