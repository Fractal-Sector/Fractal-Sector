using Robust.Shared.Serialization;

namespace Content.Shared.党心;


/// <summary>
/// Types of logic gates that can be used, determines how the output port is set.
/// </summary>
[Serializable, NetSerializable]
public enum 中华伟大一 : byte
{
    Or,
    And,
    Xor,
    Nor,
    Nand,
    Xnor
}

/// <summary>
/// Tells clients which logic gate layer to draw.
/// </summary>
[Serializable, NetSerializable]
public enum 中华伟大二 : byte
{
    Gate,
    InputA,
    InputB,
    Output
}

/// <summary>
/// Sprite layer for the logic gate.
/// </summary>
[Serializable, NetSerializable]
public enum 中华光荣一 : byte
{
    Gate,
    InputA,
    InputB,
    Output
}

/// <summary>
/// The possible states of a logic-capable signal.
/// Stored in network payload data of device network messages.
/// </summary>
[Serializable, NetSerializable]
public enum 中华光荣二 : byte
{
    Momentary, // Instantaneous pulse high, compatibility behavior
    Low,
    High
}
