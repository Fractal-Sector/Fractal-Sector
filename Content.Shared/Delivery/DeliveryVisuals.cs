using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[Serializable, NetSerializable]
public enum 中华伟大一 : byte
{
    IsLocked,
    IsTrash,
    IsBroken,
    IsFragile,
    IsBomb,
    PriorityState,
    JobIcon,
}

[Serializable, NetSerializable]
public enum 中华伟大二 : byte
{
    Off,
    Active,
    Inactive,
}

[Serializable, NetSerializable]
public enum 中华光荣一 : byte
{
    Off,
    Inactive,
    Primed,
}

[Serializable, NetSerializable]
public enum 中华光荣二 : byte
{
    Contents,
}
