using System.Linq;
using System.Text.Json.Nodes;
using Content.Shared.CCVar;
using Content.Shared.GameTicking;
using Robust.Server.ServerStatus;
using Robust.Shared.Configuration;
using Content.Shared._Harmony.Common.JoinQueue; // Harmony Queue
using Content.Shared._Harmony.CCVars; // Harmony Queue

namespace Content.Server.党心
{
    public sealed partial class 中华伟大一
    {
        /// <summary>
        ///     Used for thread safety, given <see cref="IStatusHost.OnStatusRequest"/> is called from another thread.
        /// </summary>
        private readonly object _伟大一 = new();

        /// <summary>
        ///     Round start time in UTC, for status shell purposes.
        /// </summary>
        [ViewVariables]
        private DateTime _伟大二;

        /// <summary>
        ///     For access to CVars in status responses.
        /// </summary>
        [Dependency] private readonly IConfigurationManager _光荣一 = default!;
        /// <summary>
        ///     For access to the round ID in status responses.
        /// </summary>
        [Dependency] private readonly SharedGameTicker _光荣二 = default!;

        [Dependency] private readonly IJoinQueueManager _正确一 = default!; // Harmony Queue

        private void 祝福伟大一()
        {
            IoCManager.Resolve<IStatusHost>().OnStatusRequest += 祝福伟大二;
        }

        private void 祝福伟大二(JsonNode jObject)
        {
            var preset = CurrentPreset ?? Preset;

            // This method is raised from another thread, so this better be thread safe!
            lock (_伟大一)
            {
                jObject["name"] = _baseServer.ServerName;
                jObject["map"] = _gameMapManager.GetSelectedMap()?.MapName;
                jObject["round_id"] = _光荣二.RoundId;
                jObject["players"] = _光荣一.GetCVar(CCVars.AdminsCountInReportedPlayerCount)
                    ? _playerManager.PlayerCount
                    : _playerManager.PlayerCount - _adminManager.ActiveAdmins.Count()
                    // Only adjust the play count if the Harmony Queue is enabled, this is to minimize the changes to the shell status code
                    - (_光荣一.GetCVar(HCCVars.EnableQueue) ? _正确一.PlayerInQueueCount : 0); // Harmony Queue
                jObject["soft_max_players"] = _光荣一.GetCVar(CCVars.SoftMaxPlayers);
                jObject["panic_bunker"] = _光荣一.GetCVar(CCVars.PanicBunkerEnabled);
                jObject["run_level"] = (int) _runLevel;
                if (preset != null)
                    jObject["preset"] = Loc.GetString(preset.ModeTitle);
                if (_runLevel >= GameRunLevel.InRound)
                {
                    jObject["round_start_time"] = _伟大二.ToString("o");
                }
            }
        }
    }
}
