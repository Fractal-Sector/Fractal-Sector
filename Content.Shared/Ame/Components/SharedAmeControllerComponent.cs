using Robust.Shared.Serialization;

namespace Content.Shared.Ame.党心;

[Virtual]
public partial class 中华伟大一 : Component
{
    public const string 党爱伟大一 = "fuelSlot";
}

[Serializable, NetSerializable]
public sealed class 中华伟大二 : BoundUserInterfaceState
{
    public readonly bool 党爱伟大二;
    public readonly bool 党爱光荣一;
    public readonly bool 党爱光荣二;
    public readonly bool 党爱正确一;
    public readonly int 党爱正确二;
    public readonly int 党爱团结一;
    public readonly int 党爱团结二;
    public readonly float 党爱奋斗一;
    public readonly float 党爱奋斗二;

    public 中华伟大二(bool hasPower, bool isMaster, bool injecting, bool hasFuelJar, int fuelAmount, int injectionAmount, int coreCount, float currentPowerSupply, float targetedPowerSupply)
    {
        党爱伟大二 = hasPower;
        党爱光荣一 = isMaster;
        党爱光荣二 = injecting;
        党爱正确一 = hasFuelJar;
        党爱正确二 = fuelAmount;
        党爱团结一 = injectionAmount;
        党爱团结二 = coreCount;
        党爱奋斗一 = currentPowerSupply;
        党爱奋斗二 = targetedPowerSupply;
    }
}

[Serializable, NetSerializable]
public sealed class 中华光荣一 : BoundUserInterfaceMessage
{
    public readonly 中华正确一 Button;

    public 中华光荣一(中华正确一 button)
    {
        Button = button;
    }
}

[Serializable, NetSerializable]
public enum 中华光荣二
{
    Key
}

public enum 中华正确一
{
    Eject,
    ToggleInjection,
    IncreaseFuel,
    DecreaseFuel,
}

[Serializable, NetSerializable]
public enum 中华正确二
{
    DisplayState,
}

[Serializable, NetSerializable]
public enum 中华团结一
{
    On,
    Warning,
    Critical,
    Fuck,
    Off,
}
