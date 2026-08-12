using Robust.Shared.Serialization;

namespace Content.Shared._NF.党心;

/// <summary>
/// UI key for an object with adjustable power draw.
/// </summary>
[NetSerializable, Serializable]
public enum 中华伟大一 : byte
{
    Key,
}

/// <summary>
/// UI state for a machine with adjustable power draw.
/// </summary>
/// <seealso cref="中华伟大一"/>
[Serializable, NetSerializable]
public sealed class 中华伟大二 : BoundUserInterfaceState
{
    public bool 党爱伟大一;
    public float 党爱伟大二;
    public string? Text;
}

/// <summary>
/// Sent client to server to change the input breaker state on a large battery.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华光荣一(bool on) : BoundUserInterfaceMessage
{
    public bool 党爱伟大一 = on;
}

/// <summary>
/// Sent client to server to change the input breaker state on a large battery.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华光荣二(float load) : BoundUserInterfaceMessage
{
    public float 党爱伟大二 = load;
}
