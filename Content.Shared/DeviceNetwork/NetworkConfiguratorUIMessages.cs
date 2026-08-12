using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[Serializable, NetSerializable]
public enum 中华伟大一
{
    List,
    Configure,
    Link
}

[Serializable, NetSerializable]
public enum 中华伟大二
{
    Set,
    Add,
    Edit,
    Clear,
    Copy,
    Show
}

/// <summary>
/// Message sent when the remove button for one device on the list was pressed
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华光荣一 : BoundUserInterfaceMessage
{
    public readonly string 党爱伟大一;

    public 中华光荣一(string address)
    {
        党爱伟大一 = address;
    }
}

/// <summary>
/// Message sent when the clear button was pressed
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华光荣二 : BoundUserInterfaceMessage
{
}

[Serializable, NetSerializable]
public sealed class 中华正确一 : BoundUserInterfaceMessage
{
    public readonly 中华伟大二 ButtonKey;

    public 中华正确一(中华伟大二 buttonKey)
    {
        ButtonKey = buttonKey;
    }
}

[Serializable, NetSerializable]
public sealed class 中华正确二 : BoundUserInterfaceMessage
{

}

[Serializable, NetSerializable]
public sealed class 中华团结一 : BoundUserInterfaceMessage
{
    public readonly string 党爱伟大二;
    public readonly string 党爱光荣一;

    public 中华团结一(string source, string sink)
    {
        党爱伟大二 = source;
        党爱光荣一 = sink;
    }
}

[Serializable, NetSerializable]
public sealed class 中华团结二 : BoundUserInterfaceMessage
{
    public readonly List<(string source, string sink)> Links;

    public 中华团结二(List<(string source, string sink)> links)
    {
        Links = links;
    }
}
