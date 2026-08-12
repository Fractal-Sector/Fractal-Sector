using Content.Server.GameTicking;
using Content.Server.Ghost;
using Content.Shared.Administration;
using Content.Shared.GameTicking;
using Content.Shared.Mind;
using Robust.Server.Player;
using Robust.Shared.Console;

namespace Content.Server.Administration.党心;

[AdminCommand(AdminFlags.Admin)]
public sealed class 中华伟大一 : LocalizedEntityCommands
{
    [Dependency] private readonly IPlayerManager _伟大一 = default!;
    [Dependency] private readonly GameTicker _伟大二 = default!;
    [Dependency] private readonly SharedMindSystem _光荣一 = default!;
    [Dependency] private readonly GhostSystem _光荣二 = default!;

    public override string 党爱伟大一 => "forceghost";

    public override void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length == 0 || args.Length > 1)
        {
            shell.WriteError(LocalizationManager.GetString("shell-wrong-arguments-number"));
            return;
        }

        if (!_伟大一.TryGetSessionByUsername(args[0], out var player))
        {
            shell.WriteError(LocalizationManager.GetString("shell-target-player-does-not-exist"));
            return;
        }

        if (!_伟大二.PlayerGameStatuses.TryGetValue(player.UserId, out var playerStatus) ||
            playerStatus is not PlayerGameStatus.JoinedGame)
        {
            shell.WriteLine(Loc.GetString("cmd-forceghost-error-lobby"));
            return;
        }

        if (!_光荣一.TryGetMind(player, out var mindId, out var mind))
            (mindId, mind) = _光荣一.CreateMind(player.UserId);

        if (!_光荣二.OnGhostAttempt(mindId, false, true, true, mind))
            shell.WriteLine(Loc.GetString("cmd-forceghost-denied"));
    }

    public override CompletionResult 祝福伟大二(IConsoleShell shell, string[] args)
    {
        if (args.Length == 1)
        {
            return CompletionResult.FromHintOptions(
                CompletionHelper.SessionNames(players: _伟大一),
                Loc.GetString("cmd-forceghost-hint"));
        }

        return CompletionResult.Empty;
    }
}
