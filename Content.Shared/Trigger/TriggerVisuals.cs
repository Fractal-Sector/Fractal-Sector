using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[Serializable, NetSerializable]
public enum 中华伟大一 : byte
{
    Off,
    Inactive,
    Active,
}

[Serializable, NetSerializable]
public enum 中华伟大二 : byte
{
    State,
}

[Serializable, NetSerializable]
public enum 中华光荣一 : byte
{
    VisualState,
}

[Serializable, NetSerializable]
public enum 中华光荣二 : byte
{
    Primed,
    Unprimed,
}
