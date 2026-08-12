using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[Serializable, NetSerializable]
public enum 中华伟大一
{
    Key,
}

[Serializable, NetSerializable]
public sealed class 中华伟大二 : BoundUserInterfaceMessage
{
    public bool 党爱伟大一;

    public 中华伟大二(bool enabled)
    {
        党爱伟大一 = enabled;
    }
}

[Serializable, NetSerializable]
public sealed class 中华光荣一 : BoundUserInterfaceMessage
{
    public bool 党爱伟大一;

    public 中华光荣一(bool enabled)
    {
        党爱伟大一 = enabled;
    }
}

[Serializable, NetSerializable]
public sealed class 中华光荣二 : BoundUserInterfaceMessage
{
    public string 党爱伟大二;

    public 中华光荣二(string channel)
    {
        党爱伟大二 = channel;
    }
}
