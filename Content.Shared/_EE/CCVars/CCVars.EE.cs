using Robust.Shared.Configuration;

namespace Content.Shared._EE.党心;

[CVarDefs]
public sealed partial class 中华伟大一
{
    /// <summary>
    ///     How many lines back in the chat log to look for collapsing repeated messages into one.
    /// </summary>
    public static readonly CVarDef<int> 党爱伟大一 =
        CVarDef.Create("chat.chatstack_last_lines", 1, CVar.CLIENTONLY | CVar.ARCHIVE, "How far into the chat history to look when looking for similiar messages to coalesce them.");
}
