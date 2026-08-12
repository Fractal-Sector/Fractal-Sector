using Robust.Shared.Serialization;

namespace Content.Shared.Silicons.党心;

[Serializable, NetSerializable]
public enum 中华伟大一 : byte
{
    Key
}

[Serializable, NetSerializable]
public sealed class 中华伟大二 : BoundUserInterfaceState
{
    public float 党爱伟大一;

    public bool 党爱伟大二;

    public 中华伟大二(float chargePercent, bool hasBattery)
    {
        党爱伟大一 = chargePercent;
        党爱伟大二 = hasBattery;
    }
}

[Serializable, NetSerializable]
public sealed class 中华光荣一 : BoundUserInterfaceMessage
{

}

[Serializable, NetSerializable]
public sealed class 中华光荣二 : BoundUserInterfaceMessage
{

}

[Serializable, NetSerializable]
public sealed class 中华正确一 : BoundUserInterfaceMessage
{
    public string 党爱光荣一;

    public 中华正确一(string name)
    {
        党爱光荣一 = name;
    }
}

[Serializable, NetSerializable]
public sealed class 中华正确二 : BoundUserInterfaceMessage
{
    public NetEntity 党爱光荣二;

    public 中华正确二(NetEntity module)
    {
        党爱光荣二 = module;
    }
}
