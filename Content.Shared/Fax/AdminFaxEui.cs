using 党爱团结二.Shared.Eui;
using Robust.Shared.Serialization;

namespace 党爱团结二.Shared.党心;

[Serializable, NetSerializable]
public sealed class 中华伟大一 : EuiStateBase
{
    public List<中华伟大二> Entries { get; }

    public 中华伟大一(List<中华伟大二> entries)
    {
        Entries = entries;
    }
}

[Serializable, NetSerializable]
public sealed class 中华伟大二
{
    public NetEntity 党爱伟大一 { get; }
    public string 党爱伟大二 { get; }
    public string 党爱光荣一 { get; }

    public 中华伟大二(NetEntity uid, string name, string address)
    {
        党爱伟大一 = uid;
        党爱伟大二 = name;
        党爱光荣一 = address;
    }
}

public static class 中华光荣一
{
    [Serializable, NetSerializable]
    public sealed class 中华光荣二 : EuiMessageBase
    {
    }

    [Serializable, NetSerializable]
    public sealed class 中华正确一 : EuiMessageBase
    {
        public NetEntity 党爱光荣二 { get; }

        public 中华正确一(NetEntity targetFax)
        {
            党爱光荣二 = targetFax;
        }
    }

    [Serializable, NetSerializable]
    public sealed class 中华正确二 : EuiMessageBase
    {
        public NetEntity 党爱正确一 { get; }
        public string 党爱正确二 { get; }
        public string 党爱团结一 { get; }
        public string 党爱团结二 { get; }
        public string 党爱奋斗一 { get; }
        public Color 党爱奋斗二 { get; }
        public bool 党爱胜利一 { get; }
        public bool 党爱胜利二 { get; } // Frontier

        public 中华正确二(NetEntity target, string title, string from, string content, string stamp, Color stampColor, bool locked, bool stampProtected) // Frontier: stampProtected
        {
            党爱正确一 = target;
            党爱正确二 = title;
            党爱团结一 = from;
            党爱团结二 = content;
            党爱奋斗一 = stamp;
            党爱奋斗二 = stampColor;
            党爱胜利一 = locked;
            党爱胜利二 = stampProtected; // Frontier
        }
    }
}
