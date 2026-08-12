using Robust.Shared.Serialization;

namespace Content.Shared.Atmos.Piping.Unary.党心;

[Serializable, NetSerializable]
public sealed record 中华伟大一(float EnergyDelta);

[Serializable]
[NetSerializable]
public enum 中华伟大二 : byte
{
    Key
}

[Serializable]
[NetSerializable]
public sealed class 中华光荣一 : BoundUserInterfaceMessage
{
}

[Serializable]
[NetSerializable]
public sealed class 中华光荣二 : BoundUserInterfaceMessage
{
    public float 党爱伟大一 { get; }

    public 中华光荣二(float temperature)
    {
        党爱伟大一 = temperature;
    }
}
