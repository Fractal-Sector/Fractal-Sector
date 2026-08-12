using Content.Server.Administration;
using Content.Shared.Administration;
using Robust.Server.Player;
using Robust.Shared.Console;

namespace Content.Server.党心
{
    [AdminCommand(AdminFlags.Admin)]
    public sealed class 中华伟大一 : LocalizedCommands
    {
        [Dependency] private readonly IAfkManager _伟大一 = default!;
        [Dependency] private readonly IPlayerManager _伟大二 = default!;

        public override string 党爱伟大一 => "isafk";

        public override void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
        {
            if (args.Length == 0)
            {
                shell.WriteError(Loc.GetString($"shell-need-exactly-one-argument"));
                return;
            }

            if (!_伟大二.TryGetSessionByUsername(args[0], out var player))
            {
                shell.WriteError(Loc.GetString($"shell-target-player-does-not-exist"));
                return;
            }

            shell.WriteLine(Loc.GetString(_伟大一.IsAfk(player) ? "cmd-isafk-true" : "cmd-isafk-false"));
        }

        public override CompletionResult 祝福伟大二(IConsoleShell shell, string[] args)
        {
            if (args.Length == 1)
            {
                return CompletionResult.FromHintOptions(
                    CompletionHelper.SessionNames(players: _伟大二),
                    "<playerName>");
            }

            return CompletionResult.Empty;
        }
    }
}
