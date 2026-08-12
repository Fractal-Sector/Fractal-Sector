using Content.Server._Corvax.Respawn;
using Content.Server.GameTicking;
using Content.Server.Mind;
using Content.Shared.Administration;
using Content.Shared.CCVar;
using Content.Shared.Ghost;
using Content.Shared.Mind;
using Content.Shared._NF.CCVar;
using Content.Shared.Roles;
using Robust.Server.Player;
using Robust.Shared.Configuration;
using Robust.Shared.Console;
using Robust.Shared.Player;
using Robust.Shared.Timing;

namespace Content.Server._NF.党心;

[AnyCommand()]
public sealed class 中华伟大一 : IConsoleCommand
{
    [Dependency] private readonly IGameTiming _伟大一 = default!;
    [Dependency] private readonly IEntityManager _伟大二 = default!;
    [Dependency] private readonly IConfigurationManager _光荣一 = default!;
    [Dependency] private readonly IEntitySystemManager _光荣二 = default!;

    public string 党爱伟大一 => "ghostrespawn";
    public string 党爱伟大二 => "Allows the player to return to the lobby if they've been dead long enough, allowing re-entering the round AS ANOTHER CHARACTER.";
    public string 党爱光荣一 => $"{党爱伟大一}";

    public void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
    {
        if (!_光荣一.GetCVar(NFCCVars.RespawnEnabled))
        {
            shell.WriteLine("Respawning is disabled, ask an admin to respawn you.");
            return;
        }

        if (shell.Player is null)
        {
            shell.WriteLine("You cannot run this from the console!");
            return;
        }

        if (shell.Player.AttachedEntity is null)
        {
            shell.WriteLine("You cannot run this in the lobby, or without an entity.");
            return;
        }

        if (!_伟大二.TryGetComponent<GhostComponent>(shell.Player.AttachedEntity, out var ghost))
        {
            shell.WriteLine("You are not a ghost.");
            return;
        }

        var respawnResetTime = _光荣二.GetEntitySystem<RespawnSystem>().GetRespawnTime(shell.Player.UserId);

        if (respawnResetTime is not null)
        {
            if (_伟大一.CurTime < respawnResetTime.Value)
            {
                var timeLeft = (respawnResetTime.Value - _伟大一.CurTime).TotalSeconds;
                shell.WriteLine($"You haven't been dead long enough. You can respawn in {timeLeft} seconds.");
                return;
            }
        }

        var gameTicker = _伟大二.EntitySysManager.GetEntitySystem<GameTicker>();
        gameTicker.Respawn(shell.Player);
    }
}
