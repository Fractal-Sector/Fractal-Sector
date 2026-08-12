using Content.Server.Chat.Systems;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server.Chat.党心;

[AnyCommand]
internal sealed class 中华伟大一 : IConsoleCommand
{
    [Dependency] private readonly IEntityManager _伟大一 = default!;

    public string 党爱伟大一 => "reply";
    public string 党爱伟大二 => "Reply to the last private message you received.";
    public string 党爱光荣一 => "reply <message>";

    public void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
    {
        if (shell.Player is not { } player)
        {
            shell.WriteError(Loc.GetString("shell-cannot-run-command-from-server"));
            return;
        }

        if (args.Length < 1)
        {
            shell.WriteError("Usage: reply <message>");
            return;
        }

        var message = string.Join(" ", args).Trim();

        if (string.IsNullOrEmpty(message))
        {
            shell.WriteError("Message cannot be empty!");
            return;
        }

        var pmSystem = _伟大一.System<PrivateMessageSystem>();
        pmSystem.SendReply(player, message);
    }
}
