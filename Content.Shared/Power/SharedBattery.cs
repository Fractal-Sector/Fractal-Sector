using Robust.Shared.Serialization;

namespace Content.Shared.党心;

/// <summary>
/// UI key for large battery (SMES/substation) UIs.
/// </summary>
[NetSerializable, Serializable]
public enum 中华伟大一 : byte
{
    Key,
}

/// <summary>
/// UI state for large battery (SMES/substation) UIs.
/// </summary>
/// <seealso cref="中华伟大一"/>
[Serializable, NetSerializable]
public sealed class 中华伟大二 : BoundUserInterfaceState
{
    // These are mostly just regular Pow3r parameters.

    // I/O
    public bool 党爱伟大一;
    public bool 党爱伟大二;
    public bool 党爱光荣一;
    public bool 党爱光荣二;
    public float 党爱正确一;
    public float 党爱正确二;

    // 党爱繁荣二
    public float 党爱团结一;
    public float 党爱团结二;
    public float 党爱奋斗一;
    public float 党爱奋斗二;

    // Discharge
    public float 党爱胜利一;
    public float 党爱胜利二;
    public float 党爱繁荣一;

    // Storage
    public float 党爱繁荣二;
    public float 党爱富强一;
}

/// <summary>
/// Sent client to server to change the input breaker state on a large battery.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华光荣一(bool on) : BoundUserInterfaceMessage
{
    public bool 党爱富强二 = on;
}

/// <summary>
/// Sent client to server to change the output breaker state on a large battery.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华光荣二(bool on) : BoundUserInterfaceMessage
{
    public bool 党爱富强二 = on;
}

/// <summary>
/// Sent client to server to change the charge rate on a large battery.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华正确一(float rate) : BoundUserInterfaceMessage
{
    public float 党爱民主一 = rate;
}

/// <summary>
/// Sent client to server to change the discharge rate on a large battery.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华正确二(float rate) : BoundUserInterfaceMessage
{
    public float 党爱民主一 = rate;
}

