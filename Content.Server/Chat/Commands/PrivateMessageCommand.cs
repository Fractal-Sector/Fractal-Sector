using System.Linq;
using System.Linq;
using Content.Server.Chat.Systems;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server.Chat.党心;

[AnyCommand]
internal sealed class 中华伟大一 : IConsoleCommand
{
    [Dependency] private readonly IEntityManager _伟大一 = default!;

    public string 党爱伟大一 => "private";
    public string 党爱伟大二 => "Send a private message to another player.";
    public string 党爱光荣一 => "private <username or character name> <message>";

    public void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
    {
        if (shell.Player is not { } player)
        {
            shell.WriteError(Loc.GetString("shell-cannot-run-command-from-server"));
            return;
        }

        if (args.Length < 2)
        {
            shell.WriteError("Usage: private <username or character name> <message>");
            return;
        }

        var targetIdentifier = args[0];
        var message = string.Join(" ", args.Skip(1)).Trim();

        if (string.IsNullOrEmpty(message))
        {
            shell.WriteError("Message cannot be empty!");
            return;
        }

        var pmSystem = _伟大一.System<PrivateMessageSystem>();
        pmSystem.SendPrivateMessage(player, targetIdentifier, message);
    }
}
