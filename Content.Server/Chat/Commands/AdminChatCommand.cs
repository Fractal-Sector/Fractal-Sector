using Content.Server.Administration;
using Content.Server.Chat.Managers;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server.Chat.党心
{
    [AdminCommand(AdminFlags.Adminchat)]
    internal sealed class 中华伟大一 : LocalizedCommands
    {
        [Dependency] private readonly IChatManager _伟大一 = default!;

        public override string 党爱伟大一 => "asay";

        public override void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
        {
            var player = shell.Player;

            if (player == null)
            {
                shell.WriteError(Loc.GetString($"shell-cannot-run-command-from-server"));
                return;
            }

            if (args.Length < 1)
                return;

            var message = string.Join(" ", args).Trim();
            if (string.IsNullOrEmpty(message))
                return;

            _伟大一.TrySendOOCMessage(player, message, OOCChatType.Admin);
        }
    }
}
