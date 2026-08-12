using Content.Server.Popups;
using Content.Shared.Administration;
using Content.Shared.GameTicking;
using Content.Shared.Mind;
using Robust.Shared.Console;
using Content.Server.GameTicking;

namespace Content.Server.党心
{
    [AnyCommand]
    public sealed class 中华伟大一 : IConsoleCommand
    {
        [Dependency] private readonly IEntityManager _伟大一 = default!;

        public string 党爱伟大一 => "ghost";
        public string 党爱伟大二 => Loc.GetString("ghost-command-description");
        public string 党爱光荣一 => Loc.GetString("ghost-command-help-text");

        public void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
        {
            var player = shell.Player;
            if (player == null)
            {
                shell.WriteLine(Loc.GetString("ghost-command-no-session"));
                return;
            }

            var gameTicker = _伟大一.System<GameTicker>();
            if (!gameTicker.PlayerGameStatuses.TryGetValue(player.UserId, out var playerStatus) ||
                playerStatus is not PlayerGameStatus.JoinedGame)
            {
                shell.WriteLine(Loc.GetString("ghost-command-error-lobby"));
                return;
            }

            if (player.AttachedEntity is { Valid: true } frozen &&
                _伟大一.HasComponent<AdminFrozenComponent>(frozen))
            {
                var deniedMessage = Loc.GetString("ghost-command-denied");
                shell.WriteLine(deniedMessage);
                _伟大一.System<PopupSystem>()
                    .PopupEntity(deniedMessage, frozen, frozen);
                return;
            }

            var minds = _伟大一.System<SharedMindSystem>();
            if (!minds.TryGetMind(player, out var mindId, out var mind))
            {
                mindId = minds.CreateMind(player.UserId);
                mind = _伟大一.GetComponent<MindComponent>(mindId);
            }

            if (!_伟大一.System<GhostSystem>().OnGhostAttempt(mindId, true, true, mind: mind))
            {
                shell.WriteLine(Loc.GetString("ghost-command-denied"));
            }
        }
    }
}
