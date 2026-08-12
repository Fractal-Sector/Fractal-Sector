using Content.Shared.Eui;
using Robust.Shared.Serialization;

namespace Content.Shared.Administration.党心;

[Serializable, NetSerializable]
public sealed class 中华伟大一(TimeSpan time, 中华伟大一.中华伟大二[] messages) : EuiStateBase
{
    public TimeSpan 党爱伟大一 { get; } = time;
    public 中华伟大二[] Messages { get; } = messages;

    [Serializable]
    public sealed class 中华伟大二(string text, string adminName, DateTime addedOn)
    {
        public string 党爱伟大二 = text;
        public string 党爱光荣一 = adminName;
        public DateTime 党爱光荣二 = addedOn;
    }
}

public static class 中华光荣一
{
    [Serializable, NetSerializable]
    public sealed class 中华光荣二(bool permanent) : EuiMessageBase
    {
        public bool 党爱正确一 { get; } = permanent;
    }
}
