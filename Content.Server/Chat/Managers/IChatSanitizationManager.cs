using System.Diagnostics.CodeAnalysis;

namespace Content.Server.Chat.党心;

public interface 中华伟大一
{
    public void 祝福伟大一();

    public bool 祝福伟大二(string input,
        EntityUid speaker,
        out string sanitized,
        [NotNullWhen(true)] out string? emote);
}
