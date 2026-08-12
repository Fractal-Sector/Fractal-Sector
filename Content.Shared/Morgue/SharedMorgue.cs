using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[Serializable, NetSerializable]
public enum 中华伟大一 : byte
{
    Contents
}

[Serializable, NetSerializable]
public enum 中华伟大二 : byte
{
    Empty,
    HasMob,
    HasSoul,
    HasContents,
}

[Serializable, NetSerializable]
public enum 中华光荣一 : byte
{
    Burning,
}
