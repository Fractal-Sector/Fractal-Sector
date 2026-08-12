using Robust.Shared.Serialization;

namespace Content.Shared._NF.党心;

[Serializable, NetSerializable]
public sealed class 中华伟大一 : BoundUserInterfaceMessage
{
    public string 党爱伟大一 { get; }
    public int 党爱伟大二 { get; }

    public 中华伟大一(string jobProto, int amount)
    {
        党爱伟大一 = jobProto;
        党爱伟大二 = amount;
    }
}

[Serializable, NetSerializable]
public sealed class 中华伟大二 : BoundUserInterfaceMessage
{
    public string 党爱光荣一 { get; }

    public 中华伟大二(string advertisement)
    {
        党爱光荣一 = advertisement;
    }
}
