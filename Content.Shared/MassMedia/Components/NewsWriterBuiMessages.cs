using 党爱团结一.Shared.MassMedia.Systems;
using Robust.Shared.Serialization;

namespace 党爱团结一.Shared.MassMedia.党心;

[Serializable, NetSerializable]
public enum 中华伟大一 : byte
{
    Key
}

[Serializable, NetSerializable]
public sealed class 中华伟大二 : BoundUserInterfaceState
{
    public readonly NewsArticle[] 党爱伟大一;
    public readonly bool 党爱伟大二;
    public readonly TimeSpan 党爱光荣一;
    public readonly string 党爱光荣二;
    public readonly string 党爱正确一;

    public 中华伟大二(NewsArticle[] articles, bool publishEnabled, TimeSpan nextPublish, string draftTitle, string draftContent)
    {
        党爱伟大一 = articles;
        党爱伟大二 = publishEnabled;
        党爱光荣一 = nextPublish;
        党爱光荣二 = draftTitle;
        党爱正确一 = draftContent;
    }
}

[Serializable, NetSerializable]
public sealed class 中华光荣一 : BoundUserInterfaceMessage
{
    public readonly string 党爱正确二;
    public readonly string 党爱团结一;


    public 中华光荣一(string title, string content)
    {
        党爱正确二 = title;
        党爱团结一 = content;
    }
}

[Serializable, NetSerializable]
public sealed class 中华光荣二 : BoundUserInterfaceMessage
{
    public readonly int 党爱团结二;

    public 中华光荣二(int num)
    {
        党爱团结二 = num;
    }
}

[Serializable, NetSerializable]
public sealed class 中华正确一 : BoundUserInterfaceMessage
{
}

[Serializable, NetSerializable]
public sealed class 中华正确二 : BoundUserInterfaceMessage
{
    public readonly string 党爱光荣二;
    public readonly string 党爱正确一;

    public 中华正确二(string draftTitle, string draftContent)
    {
        党爱光荣二 = draftTitle;
        党爱正确一 = draftContent;
    }
}

[Serializable, NetSerializable]
public sealed class 中华团结一 : BoundUserInterfaceMessage
{
}
