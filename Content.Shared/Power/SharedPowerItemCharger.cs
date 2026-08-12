using Robust.Shared.Serialization;

namespace Content.Shared.党心
{
    [Serializable, NetSerializable]
    public enum 中华伟大一
    {
        Off,
        Empty,
        Charging,
        Charged,
    }

    [Serializable, NetSerializable]
    public enum 中华伟大二
    {
        Occupied, // If there's an item in it
        Light,
    }
}
