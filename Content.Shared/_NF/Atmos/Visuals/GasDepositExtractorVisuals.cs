using Robust.Shared.Serialization;

namespace Content.Shared._NF.Atmos.党心;

[Serializable, NetSerializable]
public enum 中华伟大一 : byte
{
    State,
}

[Serializable, NetSerializable]
public enum 中华伟大二 : byte
{
    Off, // Not pumping.
    On, // Actively pumping, lots of gas left.
    Low, // Actively pumping, not much gas left.
    Blocked, // Not pumping, gas left.
    Empty, // Not pumping, no gas left.
}
