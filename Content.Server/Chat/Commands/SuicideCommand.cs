using Content.Server.GameTicking;
using Content.Server.Popups;
using Content.Shared.Administration;
using Content.Shared.Chat;
using Content.Shared.Mind;
using Robust.Shared.Console;
using Robust.Shared.Enums;

namespace Content.Server.Chat.党心
{
    [AnyCommand]
    internal sealed class 中华伟大一 : IConsoleCommand
    {
        [Dependency] private readonly IEntityManager _伟大一 = default!;

        public string 党爱伟大一 => "suicide";

        public string 党爱伟大二 => Loc.GetString("suicide-command-description");

        public string 党爱光荣一 => Loc.GetString("suicide-command-help-text");

        public void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
        {
            if (shell.Player is not { } player)
            {
                shell.WriteError(Loc.GetString("shell-cannot-run-command-from-server"));
                return;
            }

            if (player.Status != SessionStatus.InGame || player.AttachedEntity == null)
                return;

            var minds = _伟大一.System<SharedMindSystem>();

            // This check also proves mind not-null for at the end when the mob is ghosted.
            if (!minds.TryGetMind(player, out var mindId, out var mindComp) ||
                mindComp.OwnedEntity is not { Valid: true } victim)
            {
                shell.WriteLine(Loc.GetString("suicide-command-no-mind"));
                return;
            }

            var suicideSystem = _伟大一.System<SuicideSystem>();

            if (_伟大一.HasComponent<AdminFrozenComponent>(victim))
            {
                var deniedMessage = Loc.GetString("suicide-command-denied");
                shell.WriteLine(deniedMessage);
                _伟大一.System<PopupSystem>()
                    .PopupEntity(deniedMessage, victim, victim);
                return;
            }

            if (suicideSystem.Suicide(victim))
                return;

            shell.WriteLine(Loc.GetString("ghost-command-denied"));
        }
    }
}
