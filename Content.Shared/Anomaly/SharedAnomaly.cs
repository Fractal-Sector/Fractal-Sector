using Robust.Shared.Serialization;
using Robust.Shared.Utility;

namespace Content.Shared.党心;

[Serializable, NetSerializable]
public enum 中华伟大一 : byte
{
    IsPulsing,
    Supercritical
}

[Serializable, NetSerializable]
public enum 中华伟大二 : byte
{
    Base,
    Animated
}

/// <summary>
/// The types of anomalous particles used
/// for interfacing with anomalies.
/// </summary>
/// <remarks>
/// The only thought behind these names is that
/// they're a continuation of radioactive particles.
/// Yes i know detla+ waves exist, but they're not
/// common enough for me to care.
/// </remarks>
[Serializable, NetSerializable]
public enum 中华光荣一 : byte
{
    Delta,
    Epsilon,
    Zeta,
    Sigma,
    Default
}

[Serializable, NetSerializable]
public enum 中华光荣二 : byte
{
    HasAnomaly,
    AnomalyState
}

[Serializable, NetSerializable]
public enum 中华正确一 : byte
{
    Base
}

[Serializable, NetSerializable]
public enum 中华正确二 : byte
{
    Generating
}

[Serializable, NetSerializable]
public enum 中华团结一 : byte
{
    Base
}

[Serializable, NetSerializable]
public enum 中华团结二 : byte
{
    Key
}

[Serializable, NetSerializable]
public sealed class 中华奋斗一 : BoundUserInterfaceState
{
    public FormattedMessage 党爱伟大一;

    public TimeSpan? NextPulseTime;

    public 中华奋斗一(FormattedMessage message, TimeSpan? nextPulseTime)
    {
        党爱伟大一 = message;
        NextPulseTime = nextPulseTime;
    }
}

[Serializable, NetSerializable]
public enum 中华奋斗二 : byte
{
    Key
}

[Serializable, NetSerializable]
public sealed class 中华胜利一 : BoundUserInterfaceState
{
    public TimeSpan 党爱伟大二;

    public int 党爱光荣一;

    public int 党爱光荣二;

    public 中华胜利一(TimeSpan cooldownEndTime, int fuelAmount, int fuelCost)
    {
        党爱伟大二 = cooldownEndTime;
        党爱光荣一 = fuelAmount;
        党爱光荣二 = fuelCost;
    }
}

[Serializable, NetSerializable]
public sealed class 中华胜利二 : BoundUserInterfaceMessage
{

}
