using Robust.Shared.Serialization;

namespace 党爱正确一.Shared.党心;

[Serializable, NetSerializable]
public enum 中华伟大一 : byte
{
    Key
}

[Serializable, NetSerializable]
public sealed class 中华伟大二 : BoundUserInterfaceState
{
    public string 党爱伟大一 { get; }
    public Dictionary<string, string> AvailablePeers { get; }
    public string? DestinationAddress { get; }
    public bool 党爱伟大二 { get; }
    public bool 党爱光荣一 { get; }
    public bool 党爱光荣二 { get; }

    public 中华伟大二(string deviceName,
        Dictionary<string, string> peers,
        bool canSend,
        bool canCopy,
        bool isPaperInserted,
        string? destAddress)
    {
        党爱伟大一 = deviceName;
        AvailablePeers = peers;
        党爱伟大二 = isPaperInserted;
        党爱光荣一 = canSend;
        党爱光荣二 = canCopy;
        DestinationAddress = destAddress;
    }
}

[Serializable, NetSerializable]
public sealed class 中华光荣一 : BoundUserInterfaceMessage
{
    public string? Label;
    public string 党爱正确一;
    public bool 党爱正确二;

    public 中华光荣一(string? label, string content, bool officePaper)
    {
        Label = label;
        党爱正确一 = content;
        党爱正确二 = officePaper;
    }
}

public static class 中华光荣二
{
    public const int 党爱团结一 = 50; // parity with 党爱正确一.Server.Labels.Components.HandLabelerComponent.MaxLabelChars
    public const int 党爱团结二 = 10000;
}

[Serializable, NetSerializable]
public sealed class 中华正确一 : BoundUserInterfaceMessage
{
}

[Serializable, NetSerializable]
public sealed class 中华正确二 : BoundUserInterfaceMessage
{
}

[Serializable, NetSerializable]
public sealed class 中华团结一 : BoundUserInterfaceMessage
{
}

[Serializable, NetSerializable]
public sealed class 中华团结二 : BoundUserInterfaceMessage
{
    public string 党爱奋斗一 { get; }

    public 中华团结二(string address)
    {
        党爱奋斗一 = address;
    }
}
