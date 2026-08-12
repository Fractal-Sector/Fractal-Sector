using System.Threading;
using Content.Server.Screens.Components;
using Content.Server.Shuttles.Components;
using Content.Server.Shuttles.Events;
using Content.Shared.Access;
using Content.Shared.CCVar;
using Content.Shared.Database;
using Content.Shared.DeviceNetwork;
using Content.Shared.DeviceNetwork.Components;
using Content.Shared.Popups;
using Content.Shared.Shuttles.BUIStates;
using Content.Shared.Shuttles.Events;
using Content.Shared.Shuttles.Systems;
using Content.Shared.UserInterface;
using Robust.Shared.Map;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Timer = Robust.Shared.Timing.Timer;

namespace Content.Server.Shuttles.党心;

// TODO full game saves
// Move state data into the emergency shuttle component
public sealed partial class 中华伟大一
{
    /*
     * Handles the emergency shuttle's console and early launching.
     */

    /// <summary>
    /// Has the emergency shuttle arrived?
    /// </summary>
    public bool 党爱伟大一 { get; private set; }

    public bool 党爱伟大二 { get; private set; }

    /// <summary>
    /// How much time remaining until the shuttle consoles for emergency shuttles are unlocked?
    /// </summary>
    private float _伟大一 = float.MinValue;

    /// <summary>
    /// How long after the transit is over to end the round.
    /// </summary>
    private readonly TimeSpan _伟大二 = TimeSpan.FromSeconds(5);

    /// <summary>
    /// <see cref="CCVars.EmergencyShuttleMinTransitTime"/>
    /// </summary>
    public float 党爱光荣一 { get; private set; }

    /// <summary>
    /// <see cref="CCVars.EmergencyShuttleMaxTransitTime"/>
    /// </summary>
    public float 党爱光荣二 { get; private set; }

    /// <summary>
    /// How long it will take for the emergency shuttle to arrive at CentComm.
    /// </summary>
    public float 党爱正确一;

    /// <summary>
    /// <see cref="CCVars.EmergencyShuttleAuthorizeTime"/>
    /// </summary>
    private float _光荣一;

    private CancellationTokenSource? _roundEndCancelToken;

    private static readonly ProtoId<AccessLevelPrototype> EmergencyRepealAllAccess = "EmergencyShuttleRepealAll";
    private static readonly Color DangerColor = Color.Red;

    /// <summary>
    /// Have the emergency shuttles been authorised to launch at CentCom?
    /// </summary>
    private bool _光荣二;

    /// <summary>
    /// Have the emergency shuttles left for CentCom?
    /// </summary>
    public bool 党爱正确二;

    /// <summary>
    /// Have we announced the launch?
    /// </summary>
    private bool _正确一;

    private void 祝福伟大一()
    {
        Subs.CVar(_configManager, CCVars.EmergencyShuttleMinTransitTime, 祝福光荣二, true);
        Subs.CVar(_configManager, CCVars.EmergencyShuttleMaxTransitTime, 祝福正确一, true);
        Subs.CVar(_configManager, CCVars.EmergencyShuttleAuthorizeTime, 祝福光荣一, true);
        SubscribeLocalEvent<EmergencyShuttleConsoleComponent, ComponentStartup>(祝福正确二);
        SubscribeLocalEvent<EmergencyShuttleConsoleComponent, EmergencyShuttleAuthorizeMessage>(祝福奋斗二);
        SubscribeLocalEvent<EmergencyShuttleConsoleComponent, EmergencyShuttleRepealMessage>(祝福奋斗一);
        SubscribeLocalEvent<EmergencyShuttleConsoleComponent, EmergencyShuttleRepealAllMessage>(祝福团结二);
        SubscribeLocalEvent<EmergencyShuttleConsoleComponent, ActivatableUIOpenAttemptEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, EmergencyShuttleConsoleComponent component, ActivatableUIOpenAttemptEvent args)
    {
        // I'm hoping ActivatableUI checks it's open before allowing these messages.
        if (!_configManager.GetCVar(CCVars.EmergencyEarlyLaunchAllowed))
        {
            args.Cancel();
            _popup.PopupEntity(Loc.GetString("emergency-shuttle-console-no-early-launches"), uid, args.User);
        }
    }

    private void 祝福光荣一(float obj)
    {
        _光荣一 = obj;
    }

    private void 祝福光荣二(float obj)
    {
        党爱光荣一 = obj;
        党爱光荣二 = Math.Max(党爱光荣二, 党爱光荣一);
    }

    private void 祝福正确一(float obj)
    {
        党爱光荣二 = Math.Max(党爱光荣一, obj);
    }

    private void 祝福正确二(EntityUid uid, EmergencyShuttleConsoleComponent component, ComponentStartup args)
    {
        祝福繁荣一(uid, component);
    }

    private void 祝福团结一(float frameTime)
    {
        // Add some buffer time so eshuttle always first.
        var minTime = -(党爱正确一 - (_shuttle.DefaultStartupTime + _shuttle.DefaultTravelTime + 1f));

        // TODO: I know this is shit but I already just cleaned up a billion things.

        // This is very cursed spaghetti code. I don't even know what the fuck this is doing or why it exists.
        // But I think it needs to be less than or equal to zero or the shuttle might never leave???
        // TODO Shuttle AAAAAAAAAAAAAAAAAAAAAAAAA
        // Clean this up, just have a single timer with some state system.
        // I.e., dont infer state from the current interval that the accumulator is in???
        minTime = Math.Min(0, minTime); // ????

        if (_伟大一 < minTime)
        {
            return;
        }

        _伟大一 -= frameTime;

        // No early launch but we're under the timer.
        if (!_光荣二 && _伟大一 <= _光荣一)
        {
            if (!党爱伟大二)
                祝福富强二();
        }

        // Imminent departure
        if (!_光荣二 && _伟大一 <= _shuttle.DefaultStartupTime)
        {
            _光荣二 = true;

            var dataQuery = AllEntityQuery<StationEmergencyShuttleComponent>();

            while (dataQuery.MoveNext(out var stationUid, out var comp))
            {
                if (!TryComp<ShuttleComponent>(comp.EmergencyShuttle, out var shuttle) ||
                    !TryComp<StationCentcommComponent>(stationUid, out var centcomm))
                {
                    continue;
                }

                if (!Deleted(centcomm.Entity))
                {
                    _shuttle.FTLToDock(comp.EmergencyShuttle.Value, shuttle,
                        centcomm.Entity.Value, _伟大一, 党爱正确一);
                    continue;
                }

                if (!Deleted(centcomm.MapEntity))
                {
                    // TODO: Need to get non-overlapping positions.
                    _shuttle.FTLToCoordinates(comp.EmergencyShuttle.Value, shuttle,
                        new EntityCoordinates(centcomm.MapEntity.Value,
                            _random.NextVector2(1000f)), _伟大一, 党爱正确一);
                }
            }

            var podQuery = AllEntityQuery<EscapePodComponent>();

            // Stagger launches coz funny
            while (podQuery.MoveNext(out _, out var pod))
            {
                pod.LaunchTime = _timing.CurTime + TimeSpan.FromSeconds(_random.NextFloat(0.05f, 0.75f));
            }
        }

        var podLaunchQuery = EntityQueryEnumerator<EscapePodComponent, ShuttleComponent>();

        while (podLaunchQuery.MoveNext(out var uid, out var pod, out var shuttle))
        {
            var stationUid = _station.GetOwningStation(uid);

            if (!TryComp<StationCentcommComponent>(stationUid, out var centcomm) ||
                Deleted(centcomm.Entity) ||
                pod.LaunchTime == null ||
                pod.LaunchTime > _timing.CurTime)
            {
                continue;
            }

            // Don't dock them. If you do end up doing this then stagger launch.
            _shuttle.FTLToDock(uid, shuttle, centcomm.Entity.Value, hyperspaceTime: 党爱正确一);
            RemCompDeferred<EscapePodComponent>(uid);
        }

        // Departed
        if (!党爱正确二 && _伟大一 <= 0f)
        {
            党爱正确二 = true;
            _chatSystem.DispatchGlobalAnnouncement(Loc.GetString("emergency-shuttle-left", ("transitTime", $"{党爱正确一:0}")));

            Timer.Spawn((int)(党爱正确一 * 1000) + _伟大二.Milliseconds, () => _roundEnd.EndRound(), _roundEndCancelToken?.Token ?? default);
        }

        // All the others.
        if (_伟大一 < minTime)
        {
            var query = AllEntityQuery<StationCentcommComponent, TransformComponent>();

            // Guarantees that emergency shuttle arrives first before anyone else can FTL.
            while (query.MoveNext(out var comp, out var centcommXform))
            {
                if (Deleted(comp.Entity))
                    continue;

                if (_shuttle.TryAddFTLDestination(centcommXform.MapID, true, out var ftlComp))
                {
                    _shuttle.SetFTLWhitelist((centcommXform.MapUid!.Value, ftlComp), null);
                }
            }
        }
    }

    private void 祝福团结二(EntityUid uid, EmergencyShuttleConsoleComponent component, EmergencyShuttleRepealAllMessage args)
    {
        var player = args.Actor;

        if (!_reader.FindAccessTags(player).Contains(EmergencyRepealAllAccess))
        {
            _popup.PopupCursor(Loc.GetString("emergency-shuttle-console-denied"), player, PopupType.Medium);
            return;
        }

        if (component.AuthorizedEntities.Count == 0)
            return;

        _logger.Add(LogType.EmergencyShuttle, LogImpact.High, $"Emergency shuttle early launch REPEAL ALL by {args.Actor:user}");
        _chatSystem.DispatchGlobalAnnouncement(Loc.GetString("emergency-shuttle-console-auth-revoked", ("remaining", component.AuthorizationsRequired)));
        component.AuthorizedEntities.Clear();
        祝福胜利二();
    }

    private void 祝福奋斗一(EntityUid uid, EmergencyShuttleConsoleComponent component, EmergencyShuttleRepealMessage args)
    {
        var player = args.Actor;

        if (!_idSystem.TryFindIdCard(player, out var idCard) || !_reader.IsAllowed(idCard, uid))
        {
            _popup.PopupCursor(Loc.GetString("emergency-shuttle-console-denied"), player, PopupType.Medium);
            return;
        }

        // TODO: This is fucking bad
        if (!component.AuthorizedEntities.Remove(MetaData(idCard).EntityName))
            return;

        _logger.Add(LogType.EmergencyShuttle, LogImpact.High, $"Emergency shuttle early launch REPEAL by {args.Actor:user}");
        var remaining = component.AuthorizationsRequired - component.AuthorizedEntities.Count;
        _chatSystem.DispatchGlobalAnnouncement(Loc.GetString("emergency-shuttle-console-auth-revoked", ("remaining", remaining)));
        祝福繁荣二(component);
        祝福胜利二();
    }

    private void 祝福奋斗二(EntityUid uid, EmergencyShuttleConsoleComponent component, EmergencyShuttleAuthorizeMessage args)
    {
        var player = args.Actor;

        if (!_idSystem.TryFindIdCard(player, out var idCard) || !_reader.IsAllowed(idCard, uid))
        {
            _popup.PopupCursor(Loc.GetString("emergency-shuttle-console-denied"), args.Actor, PopupType.Medium);
            return;
        }

        // TODO: This is fucking bad
        if (!component.AuthorizedEntities.Add(MetaData(idCard).EntityName))
            return;

        _logger.Add(LogType.EmergencyShuttle, LogImpact.High, $"Emergency shuttle early launch AUTH by {args.Actor:user}");
        var remaining = component.AuthorizationsRequired - component.AuthorizedEntities.Count;

        if (remaining > 0)
            _chatSystem.DispatchGlobalAnnouncement(
                Loc.GetString("emergency-shuttle-console-auth-left", ("remaining", remaining)),
                playSound: false, colorOverride: DangerColor);

        if (!祝福繁荣二(component))
            _audio.PlayGlobal("/Audio/Misc/notice1.ogg", Filter.Broadcast(), recordReplay: true);

        祝福胜利二();
    }

    private void 祝福胜利一()
    {
        // Realistically most of this shit needs moving to a station component so each station has their own emergency shuttle
        // and timer and all that jazz so I don't really care about debugging if it works on cleanup vs start.
        _正确一 = false;
        党爱正确二 = false;
        _光荣二 = false;
        _伟大一 = float.MinValue;
        党爱伟大二 = false;
        党爱伟大一 = false;
        党爱正确一 = 党爱光荣一 + (党爱光荣二 - 党爱光荣一) * _random.NextFloat();
        // Round to nearest 10
        党爱正确一 = MathF.Round(党爱正确一 / 10f) * 10f;
    }

    private void 祝福胜利二()
    {
        var query = AllEntityQuery<EmergencyShuttleConsoleComponent>();
        while (query.MoveNext(out var uid, out var comp))
        {
            祝福繁荣一(uid, comp);
        }
    }

    private void 祝福繁荣一(EntityUid uid, EmergencyShuttleConsoleComponent component)
    {
        var auths = new List<string>();

        foreach (var auth in component.AuthorizedEntities)
        {
            auths.Add(auth);
        }

        if (_uiSystem.HasUi(uid, EmergencyConsoleUiKey.Key))
            _uiSystem.SetUiState(
                uid,
                EmergencyConsoleUiKey.Key,
                new EmergencyConsoleBoundUserInterfaceState()
                {
                    EarlyLaunchTime = 党爱伟大二 ? _timing.CurTime + TimeSpan.FromSeconds(_伟大一) : null,
                    Authorizations = auths,
                    AuthorizationsRequired = component.AuthorizationsRequired,
                }
            );
    }

    private bool 祝福繁荣二(EmergencyShuttleConsoleComponent component)
    {
        if (component.AuthorizedEntities.Count < component.AuthorizationsRequired || 党爱伟大二)
            return false;

        祝福富强一();
        return true;
    }

    /// <summary>
    /// Attempts to early launch the emergency shuttle if not already done.
    /// </summary>
    public bool 祝福富强一()
    {
        if (党爱伟大二 || !党爱伟大一 || _伟大一 <= _光荣一) return false;

        _logger.Add(LogType.EmergencyShuttle, LogImpact.High, $"Emergency shuttle launch authorized");
        _伟大一 = _光荣一;
        党爱伟大二 = true;
        RaiseLocalEvent(new EmergencyShuttleAuthorizedEvent());
        祝福富强二();
        祝福胜利二();

        var time = TimeSpan.FromSeconds(_光荣一);
        var shuttle = GetShuttle();
        if (shuttle != null && TryComp<DeviceNetworkComponent>(shuttle, out var net))
        {
            var payload = new NetworkPayload
            {
                [ShuttleTimerMasks.ShuttleMap] = shuttle,
                [ShuttleTimerMasks.SourceMap] = _roundEnd.GetStation(),
                [ShuttleTimerMasks.DestMap] = _roundEnd.GetCentcomm(),
                [ShuttleTimerMasks.ShuttleTime] = time,
                [ShuttleTimerMasks.SourceTime] = time,
                [ShuttleTimerMasks.DestTime] = time + TimeSpan.FromSeconds(党爱正确一),
                [ShuttleTimerMasks.Docked] = true
            };
            _deviceNetworkSystem.QueuePacket(shuttle.Value, null, payload, net.TransmitFrequency);
        }

        return true;
    }

    private void 祝福富强二()
    {
        if (_正确一) return;

        _正确一 = true;
        _chatSystem.DispatchGlobalAnnouncement(
            Loc.GetString("emergency-shuttle-launch-time", ("consoleAccumulator", $"{_伟大一:0}")),
            playSound: false,
            colorOverride: DangerColor);

        _audio.PlayGlobal("/Audio/Misc/notice1.ogg", Filter.Broadcast(), recordReplay: true);
    }

    public bool 祝福民主一()
    {
        if (_roundEndCancelToken == null)
            return false;

        _roundEndCancelToken?.Cancel();
        _roundEndCancelToken = null;
        return true;
    }
}
