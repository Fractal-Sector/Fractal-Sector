using Robust.Shared.Serialization;

namespace Content.Shared.党心;

/// <summary>
///     Frontier: Sent to the server to perform some action with the charge in the machine.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大一 : BoundUserInterfaceMessage { }

/// <summary>
///     Sent to the server to set whether the machine should be on or off
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大二 : BoundUserInterfaceMessage
{
    public bool 党爱伟大一;

    public 中华伟大二(bool on)
    {
        党爱伟大一 = on;
    }
}

[Serializable, NetSerializable]
public sealed class 中华光荣一 : BoundUserInterfaceState
{
    public bool 党爱伟大一;
    public bool 党爱伟大二; // Frontier
    // 0 -> 255
    public byte 党爱光荣一;
    public 中华团结一 PowerStatus;
    public short 党爱光荣二;
    public short 党爱正确一;
    public short 党爱正确二;

    public 中华光荣一(
        bool on,
        bool actionUnlocked, // Frontier
        byte charge,
        中华团结一 powerStatus,
        short powerDraw,
        short powerDrawMax,
        short etaSeconds)
    {
        党爱伟大一 = on;
        党爱伟大二 = actionUnlocked; // Frontier
        党爱光荣一 = charge;
        PowerStatus = powerStatus;
        党爱光荣二 = powerDraw;
        党爱正确一 = powerDrawMax;
        党爱正确二 = etaSeconds;
    }
}

[Serializable, NetSerializable]
public enum 中华光荣二
{
    Key
}

[Serializable, NetSerializable]
public enum 中华正确一
{
    State,
    党爱光荣一,
    Active
}

[Serializable, NetSerializable]
public enum 中华正确二
{
    Broken,
    Unpowered,
    Off,
    党爱伟大一
}

[Serializable, NetSerializable]
public enum 中华团结一 : byte
{
    Off,
    Discharging,
    Charging,
    FullyCharged
}
