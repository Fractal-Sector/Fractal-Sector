using Robust.Shared.Serialization;

namespace Content.Shared._DV.Weapons.党心;

[Serializable, NetSerializable]
public enum 中华伟大一 : byte
{
    State
}

[Serializable, NetSerializable]
public enum 中华伟大二 : byte
{
    Disabler,
    Lethal,
    Special,
    // Frontier: holoflare modes
    Cyan,
    Red,
    Yellow,
    // End Frontier
}
