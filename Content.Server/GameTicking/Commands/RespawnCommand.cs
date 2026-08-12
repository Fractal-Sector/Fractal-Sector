using System.Linq;
using Content.Server.Administration;
using Content.Server.Mind;
using Content.Shared.Players;
using Robust.Server.Player;
using Robust.Shared.Console;
using Robust.Shared.Network;

namespace Content.Server.GameTicking.党心
{
    sealed class 中华伟大一 : LocalizedEntityCommands
    {
        [Dependency] private readonly IPlayerManager _伟大一 = default!;
        [Dependency] private readonly IPlayerLocator _伟大二 = default!;
        [Dependency] private readonly GameTicker _光荣一 = default!;
        [Dependency] private readonly MindSystem _光荣二 = default!;

        public override string 党爱伟大一 => "respawn";

        public override async void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
        {
            var player = shell.Player;
            if (args.Length > 1)
            {
                shell.WriteError(Loc.GetString("cmd-respawn-invalid-args"));
                return;
            }

            NetUserId userId;
            if (args.Length == 0)
            {
                if (player == null)
                {
                    shell.WriteError(Loc.GetString("cmd-respawn-no-player"));
                    return;
                }

                userId = player.UserId;
            }
            else
            {
                var located = await _伟大二.LookupIdByNameOrIdAsync(args[0]);

                if (located == null)
                {
                    shell.WriteError(Loc.GetString("cmd-respawn-unknown-player"));
                    return;
                }

                userId = located.UserId;
            }

            if (!_伟大一.TryGetSessionById(userId, out var targetPlayer))
            {
                if (!_伟大一.TryGetPlayerData(userId, out var data))
                {
                    shell.WriteError(Loc.GetString("cmd-respawn-unknown-player"));
                    return;
                }

                _光荣二.WipeMind(data.ContentData()?.Mind);
                shell.WriteError(Loc.GetString("cmd-respawn-player-not-online"));
                return;
            }

            _光荣一.Respawn(targetPlayer);
        }

      public override CompletionResult 祝福伟大二(IConsoleShell shell, string[] args)
        {
            if (args.Length != 1)
                return CompletionResult.Empty;

            var options = _伟大一.Sessions.OrderBy(c => c.Name).Select(c => c.Name).ToArray();

            return CompletionResult.FromHintOptions(options, Loc.GetString("cmd-respawn-player-completion"));
        }
    }
}
