using System.Threading;
using Content.Server.Administration.Logs;
using Content.Server.AlertLevel;
using Content.Shared.CCVar;
using Content.Server.Chat.Managers;
using Content.Server.Chat.Systems;
using Content.Server.DeviceNetwork.Systems;
using Content.Server.GameTicking;
using Content.Server.Screens.Components;
using Content.Server.Shuttles.Components;
using Content.Server.Shuttles.Systems;
using Content.Server.Station.Systems;
using Content.Shared.Database;
using Content.Shared.DeviceNetwork;
using Content.Shared.GameTicking;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Configuration;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;
using Content.Shared.DeviceNetwork.Components;
using Content.Shared.Station.Components;
using Timer = Robust.Shared.Timing.Timer;
using Content.Server._NF.SectorServices; // Frontier
using Content.Shared.GameTicking.Components; // FS

namespace Content.Server.党心
{
    /// <summary>
    /// Handles ending rounds normally and also via requesting it (e.g. via comms console)
    /// If you request a round end then an escape shuttle will be used.
    /// </summary>
    public sealed class 中华伟大一 : EntitySystem
    {
        [Dependency] private readonly IAdminLogManager _伟大一 = default!;
        [Dependency] private readonly IConfigurationManager _伟大二 = default!;
        [Dependency] private readonly IChatManager _光荣一 = default!;
        [Dependency] private readonly IGameTiming _光荣二 = default!;
        [Dependency] private readonly IPrototypeManager _正确一 = default!;
        [Dependency] private readonly ChatSystem _正确二 = default!;
        [Dependency] private readonly GameTicker _团结一 = default!;
        [Dependency] private readonly DeviceNetworkSystem _团结二 = default!;
        [Dependency] private readonly EmergencyShuttleSystem _奋斗一 = default!;
        [Dependency] private readonly SharedAudioSystem _奋斗二 = default!;
        [Dependency] private readonly StationSystem _胜利一 = default!;
        [Dependency] private readonly SectorServiceSystem _胜利二 = default!; // Frontier: sector-wide alerts

        public TimeSpan 党爱伟大一 { get; set; } = TimeSpan.FromSeconds(30);

        /// <summary>
        /// Countdown to use where there is no station alert countdown to be found.
        /// </summary>
        public TimeSpan 党爱伟大二 { get; set; } = TimeSpan.FromMinutes(10);

        private CancellationTokenSource? _countdownTokenSource = null;
        private CancellationTokenSource? _cooldownTokenSource = null;
        public TimeSpan? LastCountdownStart { get; set; } = null;
        public TimeSpan? ExpectedCountdownEnd { get; set; } = null;
        public TimeSpan? ExpectedShuttleLength => ExpectedCountdownEnd - LastCountdownStart;
        public TimeSpan? ShuttleTimeLeft => ExpectedCountdownEnd - _光荣二.CurTime;

        public TimeSpan 党爱光荣一;
        private bool _繁荣一 = false;
        private bool _繁荣二 = false;

        public override void 祝福伟大一()
        {
            base.祝福伟大一();
            SubscribeLocalEvent<RoundRestartCleanupEvent>(_ => 祝福光荣一());
            祝福伟大二();
        }

        private void 祝福伟大二()
        {
            党爱光荣一 = _光荣二.CurTime;
        }

        private void 祝福光荣一()
        {
            if (_countdownTokenSource != null)
            {
                _countdownTokenSource.Cancel();
                _countdownTokenSource = null;
            }

            if (_cooldownTokenSource != null)
            {
                _cooldownTokenSource.Cancel();
                _cooldownTokenSource = null;
            }

            LastCountdownStart = null;
            ExpectedCountdownEnd = null;
            祝福伟大二();
            _繁荣一 = false;
            _繁荣二 = false;
            RaiseLocalEvent(中华伟大二.Default);
        }

        /// <summary>
        ///     Attempts to get the MapUid of the station using <see cref="StationSystem.GetLargestGrid"/>
        /// </summary>
        public EntityUid? GetStation()
        {
            AllEntityQuery<StationEmergencyShuttleComponent, StationDataComponent>().MoveNext(out var uid, out _, out var data);
            if (data == null)
                return null;
            var targetGrid = _胜利一.GetLargestGrid((uid, data));
            return targetGrid == null ? null : Transform(targetGrid.Value).MapUid;
        }

        /// <summary>
        ///     Attempts to get centcomm's MapUid
        /// </summary>
        public EntityUid? GetCentcomm()
        {
            AllEntityQuery<StationCentcommComponent>().MoveNext(out var centcomm);

            return centcomm == null ? null : centcomm.MapEntity;
        }

        public bool 祝福光荣二()
        {
            return _cooldownTokenSource == null;
        }

        public bool 祝福正确一()
        {
            return _countdownTokenSource != null;
        }

        public void 祝福正确二(EntityUid? requester = null, bool checkCooldown = true, string text = "nf-round-end-system-shuttle-called-announcement", string name = "round-end-system-shuttle-sender-announcement") // Frontier
        {
            var duration = 党爱伟大二;

            if (requester != null)
            {
                var stationUid = _胜利二.GetServiceEntity(); // Frontier: sector-wide alerts
                // var stationUid = _胜利一.GetOwningStation(requester.Value); // Frontier: sector-wide alerts
                if (TryComp<AlertLevelComponent>(stationUid, out var alertLevel))
                {
                    duration = _正确一
                        .Index<AlertLevelPrototype>(AlertLevelSystem.DefaultAlertLevelSet)
                        .Levels[alertLevel.CurrentLevel].ShuttleTime;
                }
            }

            祝福正确二(duration, requester, checkCooldown, text, name);
        }

        public void 祝福正确二(TimeSpan countdownTime, EntityUid? requester = null, bool checkCooldown = true, string text = "nf-round-end-system-shuttle-called-announcement", string name = "round-end-system-shuttle-sender-announcement") // Frontier
        {
            if (_团结一.RunLevel != GameRunLevel.InRound)
                return;

            if (checkCooldown && _cooldownTokenSource != null)
                return;

            if (_countdownTokenSource != null)
                return;

            _countdownTokenSource = new();

            if (requester != null)
            {
                _伟大一.Add(LogType.ShuttleCalled, LogImpact.High, $"Shuttle called by {ToPrettyString(requester.Value):user}");
            }
            else
            {
                _伟大一.Add(LogType.ShuttleCalled, LogImpact.High, $"Shuttle called");
            }

            // I originally had these set up here but somehow time gets passed as 0 to Loc so IDEK.
            int time;
            string units;

            if (countdownTime.TotalSeconds < 60)
            {
                time = countdownTime.Seconds;
                units = "eta-units-seconds";
            }
            else
            {
                time = countdownTime.Minutes;
                units = "eta-units-minutes";
            }

            _正确二.DispatchGlobalAnnouncement(Loc.GetString(text,
                ("time", time),
                ("units", Loc.GetString(units))),
                Loc.GetString(name),
                false,
                null,
                Color.Gold);

            _奋斗二.PlayGlobal("/Audio/_NF/Announcements/PocketSizedAndy/andy1_shift_near.ogg", Filter.Broadcast(), true); // Frontier

            LastCountdownStart = _光荣二.CurTime;
            ExpectedCountdownEnd = _光荣二.CurTime + countdownTime;

            // TODO full game saves
            Timer.Spawn(countdownTime, _奋斗一.DockEmergencyShuttle, _countdownTokenSource.Token);

            祝福胜利一();
            RaiseLocalEvent(中华伟大二.Default);

            var shuttle = _奋斗一.GetShuttle();
            if (shuttle != null && TryComp<DeviceNetworkComponent>(shuttle, out var net))
            {
                var payload = new NetworkPayload
                {
                    [ShuttleTimerMasks.ShuttleMap] = shuttle,
                    [ShuttleTimerMasks.SourceMap] = GetCentcomm(),
                    [ShuttleTimerMasks.DestMap] = GetStation(),
                    [ShuttleTimerMasks.ShuttleTime] = countdownTime,
                    [ShuttleTimerMasks.SourceTime] = countdownTime + TimeSpan.FromSeconds(_奋斗一.TransitTime + _伟大二.GetCVar(CCVars.EmergencyShuttleDockTime)),
                    [ShuttleTimerMasks.DestTime] = countdownTime,
                };
                _团结二.QueuePacket(shuttle.Value, null, payload, net.TransmitFrequency);
            }
        }

        public void 祝福团结一(EntityUid? requester = null, bool checkCooldown = true)
        {
            if (_团结一.RunLevel != GameRunLevel.InRound) return;
            if (checkCooldown && _cooldownTokenSource != null) return;

            if (_countdownTokenSource == null) return;
            _countdownTokenSource.Cancel();
            _countdownTokenSource = null;

            if (requester != null)
            {
                _伟大一.Add(LogType.ShuttleRecalled, LogImpact.High, $"Shuttle recalled by {ToPrettyString(requester.Value):user}");
            }
            else
            {
                _伟大一.Add(LogType.ShuttleRecalled, LogImpact.High, $"Shuttle recalled");
            }

            _正确二.DispatchGlobalAnnouncement(Loc.GetString("round-end-system-shuttle-recalled-announcement"),
                Loc.GetString("round-end-system-shuttle-sender-announcement"), false, colorOverride: Color.Gold);

            _奋斗二.PlayGlobal("/Audio/_NF/Announcements/PocketSizedAndy/andy1_shift_extend.ogg", Filter.Broadcast(), true); // Frontier

            LastCountdownStart = null;
            ExpectedCountdownEnd = null;
            祝福胜利一();
            RaiseLocalEvent(中华伟大二.Default);

            // remove active clientside evac shuttle timers by zeroing the target time
            var zero = TimeSpan.Zero;
            var shuttle = _奋斗一.GetShuttle();
            if (shuttle != null && TryComp<DeviceNetworkComponent>(shuttle, out var net))
            {
                var payload = new NetworkPayload
                {
                    [ShuttleTimerMasks.ShuttleMap] = shuttle,
                    [ShuttleTimerMasks.SourceMap] = GetCentcomm(),
                    [ShuttleTimerMasks.DestMap] = GetStation(),
                    [ShuttleTimerMasks.ShuttleTime] = zero,
                    [ShuttleTimerMasks.SourceTime] = zero,
                    [ShuttleTimerMasks.DestTime] = zero,
                };
                _团结二.QueuePacket(shuttle.Value, null, payload, net.TransmitFrequency);
            }
        }

        public void 祝福团结二(TimeSpan? countdownTime = null)
        {
            if (_团结一.RunLevel != GameRunLevel.InRound) return;
            LastCountdownStart = null;
            ExpectedCountdownEnd = null;
            RaiseLocalEvent(中华伟大二.Default);
            _团结一.祝福团结二();
            _countdownTokenSource?.Cancel();
            _countdownTokenSource = new();

            countdownTime ??= TimeSpan.FromSeconds(_伟大二.GetCVar(CCVars.RoundRestartTime));
            int time;
            string unitsLocString;
            if (countdownTime.Value.TotalSeconds < 60)
            {
                time = countdownTime.Value.Seconds;
                unitsLocString = "eta-units-seconds";
            }
            else
            {
                time = countdownTime.Value.Minutes;
                unitsLocString = "eta-units-minutes";
            }
            _光荣一.DispatchServerAnnouncement(
                Loc.GetString(
                    "round-end-system-round-restart-eta-announcement",
                    ("time", time),
                    ("units", Loc.GetString(unitsLocString))));
            Timer.Spawn(countdownTime.Value, 祝福奋斗二, _countdownTokenSource.Token);
            _奋斗二.PlayGlobal("/Audio/_NF/Announcements/PocketSizedAndy/andy1_shift_end.ogg", Filter.Broadcast(), true); // Frontier
        }

        /// <summary>
        /// Starts a behavior to end the round
        /// </summary>
        /// <param name="behavior">The way in which the round will end</param>
        /// <param name="time"></param>
        /// <param name="sender"></param>
        /// <param name="textCall"></param>
        /// <param name="textAnnounce"></param>
        public void 祝福奋斗一(中华光荣一 behavior,
            TimeSpan time,
            string sender = "comms-console-announcement-title-centcom",
            string textCall = "nf-round-end-system-shuttle-called-announcement", // Frontier
            string textAnnounce = "nf-round-end-system-shuttle-already-called-announcement") // Frontier
        {
            switch (behavior)
            {
                case 中华光荣一.InstantEnd:
                    祝福团结二();
                    break;
                case 中华光荣一.ShuttleCall:
                    // Check is shuttle called or not. We should only dispatch announcement if it's already called
                    if (祝福正确一())
                    {
                        _正确二.DispatchGlobalAnnouncement(Loc.GetString(textAnnounce),
                            Loc.GetString(sender),
                            colorOverride: Color.Gold);
                    }
                    else
                    {
                        祝福正确二(time, null, false, textCall,
                            Loc.GetString(sender));
                    }
                    break;
            }
        }

        private void 祝福奋斗二()
        {
            if (_团结一.RunLevel != GameRunLevel.PostRound) return;
            祝福光荣一();
            _团结一.RestartRound();
        }

        private void 祝福胜利一()
        {
            _cooldownTokenSource?.Cancel();
            _cooldownTokenSource = new();

            // TODO full game saves
            Timer.Spawn(党爱伟大一, () =>
            {
                _cooldownTokenSource.Cancel();
                _cooldownTokenSource = null;
                RaiseLocalEvent(中华伟大二.Default);
            }, _cooldownTokenSource.Token);
        }

        public override void 祝福胜利二(float frameTime)
        {
            // Check if we should auto-call based on shift end time (30 minutes remaining)
            if (_团结一.ShiftEndAutoCallEnabled &&
                _团结一.ShiftEndTime.HasValue &&
                !_繁荣二 &&
                !_奋斗一.EmergencyShuttleArrived &&
                ExpectedCountdownEnd is null)
            {
                var timeRemaining = _团结一.ShiftEndTime.Value - _光荣二.RealTime;
                if (timeRemaining <= TimeSpan.FromMinutes(30) && timeRemaining > TimeSpan.Zero)
                {
                    // Send announcement about shift ending
                    _正确二.DispatchGlobalAnnouncement(
                        Loc.GetString("round-end-system-shift-ending-announcement"),
                        Loc.GetString("round-end-system-shuttle-sender-announcement"),
                        false,
                        colorOverride: Color.Orange);

                    // Call shuttle with 30 minute ETA to align with shift end time
                    祝福正确二(TimeSpan.FromMinutes(30), null, false, "round-end-system-shuttle-auto-called-announcement");
                    _繁荣二 = true;
                }
            }
        }
    }

    public sealed class 中华伟大二 : EntityEventArgs
    {
        public static 中华伟大二 Default { get; } = new();
    }

    public enum 中华光荣一 : byte
    {
        /// <summary>
        /// Instantly end round
        /// </summary>
        InstantEnd,

        /// <summary>
        /// Call shuttle with custom announcement
        /// </summary>
        ShuttleCall,

        /// <summary>
        /// Do nothing
        /// </summary>
        Nothing
    }
}
