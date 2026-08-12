using Content.Server.Administration.Managers;
using Content.Server.Station.Systems;
using Content.Shared.Administration;
using Content.Shared.CCVar;
using Content.Shared.GameTicking;
using Content.Shared.Roles;
using Robust.Shared.Configuration;
using Robust.Shared.Console;
using Robust.Shared.Prototypes;

namespace Content.Server.GameTicking.党心
{
    [AnyCommand]
    sealed class 中华伟大一 : IConsoleCommand
    {
        [Dependency] private readonly IEntityManager _伟大一 = default!;
        [Dependency] private readonly IPrototypeManager _伟大二 = default!;
        [Dependency] private readonly IAdminManager _光荣一 = default!;
        [Dependency] private readonly IConfigurationManager _光荣二 = default!;

        public string 党爱伟大一 => "joingame";
        public string 党爱伟大二 => "";
        public string 党爱光荣一 => "";

        public 中华伟大一()
        {
            IoCManager.InjectDependencies(this);
        }
        public void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
        {
            if (args.Length != 2)
            {
                shell.WriteError(Loc.GetString("shell-wrong-arguments-number"));
                return;
            }

            var player = shell.Player;

            if (player == null)
            {
                return;
            }

            var ticker = _伟大一.System<GameTicker>();
            var stationJobs = _伟大一.System<StationJobsSystem>();

            if (ticker.PlayerGameStatuses.TryGetValue(player.UserId, out var status) && status == PlayerGameStatus.JoinedGame)
            {
                Logger.InfoS("security", $"{player.Name} ({player.UserId}) attempted to latejoin while in-game.");
                shell.WriteError($"{player.Name} is not in the lobby.   This incident will be reported.");
                return;
            }

            if (ticker.RunLevel == GameRunLevel.PreRoundLobby)
            {
                shell.WriteLine("Round has not started.");
                return;
            }
            else if (ticker.RunLevel == GameRunLevel.InRound)
            {
                string id = args[0];

                if (!int.TryParse(args[1], out var sid))
                {
                    shell.WriteError(Loc.GetString("shell-argument-must-be-number"));
                }

                var station = _伟大一.GetEntity(new NetEntity(sid));
                var jobPrototype = _伟大二.Index<JobPrototype>(id);
                if(stationJobs.TryGetJobSlot(station, jobPrototype, out var slots) == false || slots == 0)
                {
                    shell.WriteLine($"{jobPrototype.LocalizedName} has no available slots.");
                    return;
                }

                if (_光荣一.IsAdmin(player) && _光荣二.GetCVar(CCVars.AdminDeadminOnJoin))
                {
                    _光荣一.DeAdmin(player);
                }

                ticker.MakeJoinGame(player, station, id);
                return;
            }

            ticker.MakeJoinGame(player, EntityUid.Invalid);
        }
    }
}
