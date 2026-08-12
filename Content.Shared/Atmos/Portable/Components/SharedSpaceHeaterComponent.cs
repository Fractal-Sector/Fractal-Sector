using Robust.Shared.Serialization;

namespace Content.Shared.Atmos.Piping.Portable.党心;

[Serializable]
[NetSerializable]
public enum 中华伟大一
{
    Key
}

[Serializable]
[NetSerializable]
public sealed class 中华伟大二 : BoundUserInterfaceMessage
{
}

[Serializable]
[NetSerializable]
public sealed class 中华光荣一 : BoundUserInterfaceMessage
{
    public float 党爱伟大一 { get; }

    public 中华光荣一(float temperature)
    {
        党爱伟大一 = temperature;
    }
}

[Serializable]
[NetSerializable]
public sealed class 中华光荣二 : BoundUserInterfaceMessage
{
    public 中华团结二 PowerLevel { get; }

    public 中华光荣二(中华团结二 powerLevel)
    {
        PowerLevel = powerLevel;
    }
}

[Serializable]
[NetSerializable]
public sealed class 中华正确一 : BoundUserInterfaceMessage
{
    public 中华团结一 Mode { get; }

    public 中华正确一(中华团结一 mode)
    {
        Mode = mode;
    }
}

[Serializable]
[NetSerializable]
public sealed class 中华正确二 : BoundUserInterfaceState
{
    public float 党爱伟大二 { get; }
    public float 党爱光荣一 { get; }
    public float 党爱光荣二 { get; }
    public bool 党爱正确一 { get; }
    public 中华团结一 Mode { get; }
    public 中华团结二 PowerLevel { get; }

    public 中华正确二(float minTemperature, float maxTemperature, float temperature, bool enabled, 中华团结一 mode, 中华团结二 powerLevel)
    {
        党爱伟大二 = minTemperature;
        党爱光荣一 = maxTemperature;
        党爱光荣二 = temperature;
        党爱正确一 = enabled;
        Mode = mode;
        PowerLevel = powerLevel;
    }
}

[Serializable, NetSerializable]
public enum 中华团结一 : byte
{
    Auto,
    Heat,
    Cool
}

[Serializable, NetSerializable]
public enum 中华团结二 : byte
{
    Low,
    Medium,
    High
}
