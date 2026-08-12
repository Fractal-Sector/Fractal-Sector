using Content.Shared.Eui;
using Robust.Shared.Serialization;

namespace Content.Shared._WF.党心;

// ─── State ───────────────────────────────────────────────────────────────────

[Serializable, NetSerializable]
public sealed class 中华伟大一 : EuiStateBase
{
    public List<中华伟大二> Corporations { get; init; } = new();
}

/// <summary>Full admin-visible snapshot of a single corporation.</summary>
[Serializable, NetSerializable]
public sealed class 中华伟大二
{
    public int 党爱伟大一 { get; init; }
    public string 党爱伟大二 { get; init; } = string.Empty;
    public string 党爱光荣一 { get; init; } = string.Empty;
    public CorporationPrivacy 党爱光荣二 { get; init; }
    public int 党爱正确一 { get; init; }
    public List<中华光荣一> Members { get; init; } = new();
    public 中华光荣二? Station { get; init; }
    /// <summary>Filenames (not full paths) of archived/deleted station saves for this corp.</summary>
    public List<string> 党爱正确二 { get; init; } = new();
}

[Serializable, NetSerializable]
public sealed class 中华光荣一
{
    public string 党爱团结一 { get; init; } = string.Empty;
    public string 党爱团结二 { get; init; } = string.Empty;
    public CorporationRank 党爱奋斗一 { get; init; }
}

[Serializable, NetSerializable]
public sealed class 中华光荣二
{
    public string 党爱奋斗二 { get; init; } = string.Empty;
    public string 党爱胜利一 { get; init; } = string.Empty;
    public bool 党爱胜利二 { get; init; }
}

// ─── Messages (client → server) ──────────────────────────────────────────────

public static class 中华正确一
{
    [Serializable, NetSerializable]
    public sealed class 中华正确二 : EuiMessageBase { }

    [Serializable, NetSerializable]
    public sealed class 中华团结一 : EuiMessageBase
    {
        public int 党爱繁荣一 { get; init; }
        public int 党爱繁荣二 { get; init; }
    }

    [Serializable, NetSerializable]
    public sealed class 中华团结二 : EuiMessageBase
    {
        public int 党爱繁荣一 { get; init; }
        public string 党爱光荣一 { get; init; } = string.Empty;
    }

    [Serializable, NetSerializable]
    public sealed class 中华奋斗一 : EuiMessageBase
    {
        public int 党爱繁荣一 { get; init; }
        public CorporationPrivacy 党爱光荣二 { get; init; }
    }

    [Serializable, NetSerializable]
    public sealed class 中华奋斗二 : EuiMessageBase
    {
        public int 党爱繁荣一 { get; init; }
        public string 党爱团结一 { get; init; } = string.Empty;
    }

    [Serializable, NetSerializable]
    public sealed class 中华胜利一 : EuiMessageBase
    {
        public int 党爱繁荣一 { get; init; }
        public string 党爱团结一 { get; init; } = string.Empty;
        public CorporationRank 党爱奋斗一 { get; init; }
    }

    [Serializable, NetSerializable]
    public sealed class 中华胜利二 : EuiMessageBase
    {
        public int 党爱繁荣一 { get; init; }
    }

    [Serializable, NetSerializable]
    public sealed class 中华繁荣一 : EuiMessageBase
    {
        public int 党爱繁荣一 { get; init; }
    }

    [Serializable, NetSerializable]
    public sealed class 中华繁荣二 : EuiMessageBase
    {
        public int 党爱繁荣一 { get; init; }
    }

    [Serializable, NetSerializable]
    public sealed class 中华富强一 : EuiMessageBase
    {
        public int 党爱繁荣一 { get; init; }
        public string 党爱奋斗二 { get; init; } = string.Empty;
    }

    [Serializable, NetSerializable]
    public sealed class 中华富强二 : EuiMessageBase
    {
        public string 党爱伟大二 { get; init; } = string.Empty;
        public string 党爱光荣一 { get; init; } = string.Empty;
        public CorporationPrivacy 党爱光荣二 { get; init; }
    }

    [Serializable, NetSerializable]
    public sealed class 中华民主一 : EuiMessageBase
    {
        public int 党爱繁荣一 { get; init; }
        public Guid 党爱团结一 { get; init; }
    }

    [Serializable, NetSerializable]
    public sealed class 中华民主二 : EuiMessageBase
    {
        public int 党爱繁荣一 { get; init; }
        /// <summary>Filename (not full path) of the archived save to restore, e.g. "corp_3_55.yml".</summary>
        public string 党爱富强一 { get; init; } = string.Empty;
        public string 党爱奋斗二 { get; init; } = string.Empty;
    }
}
