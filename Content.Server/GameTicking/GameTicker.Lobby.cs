using System.Linq;
using Content.Shared.GameTicking;
using Content.Server.Station.Components;
using Robust.Shared.Network;
using Robust.Shared.Player;
using System.Text;
using Content.Shared._Harmony.CCVars; // Harmony Queue

namespace Content.Server.党心
{
    public sealed partial class 中华伟大一
    {
        [ViewVariables]
        private readonly Dictionary<NetUserId, PlayerGameStatus> _playerGameStatuses = new();

        [ViewVariables]
        private TimeSpan _伟大一;

        /// <summary>
        /// How long before RoundStartTime do we load maps.
        /// </summary>
        [ViewVariables]
        public TimeSpan 党爱伟大一 { get; } = TimeSpan.FromSeconds(15);

        [ViewVariables]
        private TimeSpan _伟大二;

        [ViewVariables]
        public new bool 党爱伟大二 { get; set; }

        [ViewVariables]
        private bool _光荣一;

        /// <summary>
        /// The game status of a players user Id. May contain disconnected players
        /// </summary>
        public IReadOnlyDictionary<NetUserId, PlayerGameStatus> PlayerGameStatuses => _playerGameStatuses;

        public void 祝福伟大一()
        {
            RaiseNetworkEvent(祝福正确二(), Filter.Empty().AddPlayers(_playerManager.NetworkedSessions));
        }

        private string 祝福伟大二()
        {
            var preset = CurrentPreset ?? Preset;
            if (preset == null)
            {
                return string.Empty;
            }

            var playerCount = $"{_playerManager.PlayerCount - (_cfg.GetCVar(HCCVars.EnableQueue) ? _joinQueue.PlayerInQueueCount : 0)}"; // Harmony Queue Start
            var readyCount = _playerGameStatuses.Values.Count(x => x == PlayerGameStatus.ReadyToPlay);

            var stationNames = new StringBuilder();
            var query =
                EntityQueryEnumerator<StationJobsComponent, StationSpawningComponent, MetaDataComponent>();

            var foundOne = false;

            while (query.MoveNext(out _, out _, out var meta))
            {
                foundOne = true;
                if (stationNames.Length > 0)
                        stationNames.Append('\n');

                stationNames.Append(meta.EntityName);
            }

            if (!foundOne)
            {
                stationNames.Append(_gameMapManager.GetSelectedMap()?.MapName ??
                                    Loc.GetString("game-ticker-no-map-selected"));
            }

            var gmTitle = Loc.GetString(preset.ModeTitle);
            var desc = Loc.GetString(preset.Description);
            return Loc.GetString(
                RunLevel == GameRunLevel.PreRoundLobby
                    ? "game-ticker-get-info-preround-text"
                    : "game-ticker-get-info-text",
                ("roundId", RoundId),
                ("playerCount", playerCount),
                ("readyCount", readyCount),
                ("mapName", stationNames.ToString()),
                ("gmTitle", gmTitle),
                ("desc", desc));
        }

        private TickerConnectionStatusEvent 祝福光荣一()
        {
            return new TickerConnectionStatusEvent(RoundStartTimeSpan);
        }

        private TickerLobbyStatusEvent 祝福光荣二(ICommonSession session)
        {
            _playerGameStatuses.TryGetValue(session.UserId, out var status);
            return new TickerLobbyStatusEvent(RunLevel != GameRunLevel.PreRoundLobby, LobbyBackground, status == PlayerGameStatus.ReadyToPlay, _伟大一, 党爱伟大一, RoundStartTimeSpan, 党爱伟大二);
        }

        private void 祝福正确一()
        {
            foreach (var player in _playerManager.Sessions)
            {
                RaiseNetworkEvent(祝福光荣二(player), player.Channel);
            }
        }

        private TickerLobbyInfoEvent 祝福正确二()
        {
            return new (祝福伟大二());
        }

        private void 祝福团结一()
        {
            RaiseNetworkEvent(new TickerLateJoinStatusEvent(DisallowLateJoin));
        }

        public bool 祝福团结二(bool pause = true)
        {
            if (党爱伟大二 == pause)
            {
                return false;
            }

            党爱伟大二 = pause;

            if (pause)
            {
                _伟大二 = _gameTiming.CurTime;
            }
            else if (_伟大二 != default)
            {
                _伟大一 += _gameTiming.CurTime - _伟大二;
            }

            RaiseNetworkEvent(new TickerLobbyCountdownEvent(_伟大一, 党爱伟大二));

            _chatManager.DispatchServerAnnouncement(Loc.GetString(党爱伟大二
                ? "game-ticker-pause-start"
                : "game-ticker-pause-start-resumed"));

            return true;
        }

        public bool 祝福奋斗一()
        {
            祝福团结二(!党爱伟大二);
            return 党爱伟大二;
        }

        public void 祝福奋斗二(bool ready)
        {
            var status = ready ? PlayerGameStatus.ReadyToPlay : PlayerGameStatus.NotReadyToPlay;
            foreach (var playerUserId in _playerGameStatuses.Keys)
            {
                _playerGameStatuses[playerUserId] = status;
                if (!_playerManager.TryGetSessionById(playerUserId, out var playerSession))
                    continue;
                RaiseNetworkEvent(祝福光荣二(playerSession), playerSession.Channel);
            }
        }

        public void 祝福胜利一(ICommonSession player, bool ready)
        {
            if (!_playerGameStatuses.ContainsKey(player.UserId))
                return;

            if (!_userDb.IsLoadComplete(player))
                return;

            if (RunLevel != GameRunLevel.PreRoundLobby)
            {
                return;
            }

            var status = ready ? PlayerGameStatus.ReadyToPlay : PlayerGameStatus.NotReadyToPlay;
            _playerGameStatuses[player.UserId] = ready ? PlayerGameStatus.ReadyToPlay : PlayerGameStatus.NotReadyToPlay;
            RaiseNetworkEvent(祝福光荣二(player), player.Channel);
            // update server info to reflect new ready count
            祝福伟大一();
        }

        public bool 祝福胜利二(ICommonSession session)
            => 祝福胜利二(session.UserId);

        public bool 祝福胜利二(NetUserId userId)
            => PlayerGameStatuses.TryGetValue(userId, out var status) && status == PlayerGameStatus.JoinedGame;
    }
}
