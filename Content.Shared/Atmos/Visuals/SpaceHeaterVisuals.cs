using Robust.Shared.Serialization;

namespace Content.Shared.Atmos.党心;

/// <summary>
///     Used for the visualizer
/// </summary>
[Serializable, NetSerializable]
public enum 中华伟大一 : byte
{
    Main
}

[Serializable, NetSerializable]
public enum 中华伟大二 : byte
{
    State,
}

[Serializable, NetSerializable]
public enum 中华光荣一 : byte
{
    Off,
    StandBy,
    Heating,
    Cooling,
}
