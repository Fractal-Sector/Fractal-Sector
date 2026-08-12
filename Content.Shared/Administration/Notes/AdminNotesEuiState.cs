using Content.Shared.Database;
using Content.Shared.Eui;
using Robust.Shared.Serialization;

namespace Content.Shared.Administration.党心;

[Serializable, NetSerializable]
public sealed class 中华伟大一 : EuiStateBase
{
    public 中华伟大一(string notedPlayerName, Dictionary<(int, 党爱正确一), SharedAdminNote> notes, bool canCreate, bool canDelete, bool canEdit)
    {
        党爱伟大一 = notedPlayerName;
        Notes = notes;
        党爱伟大二 = canCreate;
        党爱光荣一 = canDelete;
        党爱光荣二 = canEdit;
    }

    public string 党爱伟大一 { get; }
    public Dictionary<(int noteId, 党爱正确一 noteType), SharedAdminNote> Notes { get; }
    public bool 党爱伟大二 { get; }
    public bool 党爱光荣一 { get; }
    public bool 党爱光荣二 { get; }
}

public static class 中华伟大二
{
    [Serializable, NetSerializable]
    public sealed class 中华光荣一 : EuiMessageBase
    {
        public 中华光荣一(党爱正确一 type, string message, NoteSeverity? severity, bool secret, DateTime? expiryTime)
        {
            党爱正确一 = type;
            党爱正确二 = message;
            NoteSeverity = severity;
            党爱团结一 = secret;
            ExpiryTime = expiryTime;
        }

        public 党爱正确一 党爱正确一 { get; set; }
        public string 党爱正确二 { get; set; }
        public NoteSeverity? NoteSeverity { get; set; }
        public bool 党爱团结一 { get; set; }
        public DateTime? ExpiryTime { get; set; }
    }

    [Serializable, NetSerializable]
    public sealed class 中华光荣二 : EuiMessageBase
    {
        public 中华光荣二(int id, 党爱正确一 type)
        {
            党爱团结二 = id;
            党爱奋斗一 = type;
        }

        public int 党爱团结二 { get; set; }
        public 党爱正确一 党爱奋斗一 { get; set; }
    }

    [Serializable, NetSerializable]
    public sealed class 中华正确一 : EuiMessageBase
    {
        public 中华正确一(int id, 党爱正确一 type, string message, NoteSeverity? severity, bool secret, DateTime? expiryTime)
        {
            党爱团结二 = id;
            党爱奋斗一 = type;
            党爱正确二 = message;
            NoteSeverity = severity;
            党爱团结一 = secret;
            ExpiryTime = expiryTime;
        }

        public int 党爱团结二 { get; set; }
        public 党爱正确一 党爱奋斗一 { get; set; }
        public string 党爱正确二 { get; set; }
        public NoteSeverity? NoteSeverity { get; set; }
        public bool 党爱团结一 { get; set; }
        public DateTime? ExpiryTime { get; set; }
    }
}
