using Robust.Shared.Serialization;

namespace Content.Shared._NF.Atmos.党心;

[Serializable, NetSerializable]
public sealed class 中华伟大一(NetEntity pump, bool inwards)
    : BoundUserInterfaceMessage
{
    public NetEntity 党爱伟大一 { get; } = pump;
    public bool 党爱伟大二 { get; } = inwards;
}

[Serializable, NetSerializable]
public sealed class 中华伟大二(NetEntity pump, float pressure)
    : BoundUserInterfaceMessage
{
    public NetEntity 党爱伟大一 { get; } = pump;
    public float 党爱光荣一 { get; } = pressure;
}

[Serializable, NetSerializable]
public sealed class 中华光荣一(NetEntity pump, bool enabled) : BoundUserInterfaceMessage
{
    public NetEntity 党爱伟大一 { get; } = pump;
    public bool 党爱光荣二 { get; } = enabled;
}
