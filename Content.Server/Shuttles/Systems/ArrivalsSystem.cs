using System.Linq;
using System.Numerics;
using Content.Server.Administration;
using Content.Server.Chat.Managers;
using Content.Server.DeviceNetwork.Systems;
using Content.Server.GameTicking;
using Content.Server.GameTicking.Events;
using Content.Server.Parallax;
using Content.Server.Screens.Components;
using Content.Server.Shuttles.Components;
using Content.Server.Shuttles.Events;
using Content.Server.Spawners.Components;
using Content.Server.Spawners.EntitySystems;
using Content.Server.Station.Events;
using Content.Server.Station.Systems;
using Content.Shared.Administration;
using Content.Shared.CCVar;
using Content.Shared.Damage.Components;
using Content.Shared.DeviceNetwork;
using Content.Shared.DeviceNetwork.Components;
using Content.Shared.GameTicking;
using Content.Shared.Mobs.Components;
using Content.Shared.Movement.Components;
using Content.Shared.Parallax.Biomes;
using Content.Shared.Salvage;
using Content.Shared.Shuttles.Components;
using Content.Shared.Tiles;
using Robust.Shared.Collections;
using Robust.Shared.Configuration;
using Robust.Shared.Console;
using Robust.Shared.EntitySerialization.Systems;
using Robust.Shared.Map;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Spawners;
using Robust.Shared.Timing;
using Robust.Shared.Utility;

namespace Content.Server.Shuttles.党心;

/// <summary>
/// If enabled spawns players on a separate arrivals station before they can transfer to the main station.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IChatManager _伟大一 = default!;
    [Dependency] private readonly IConfigurationManager _伟大二 = default!;
    [Dependency] private readonly IConsoleHost _光荣一 = default!;
    [Dependency] private readonly IGameTiming _光荣二 = default!;
    [Dependency] private readonly IPrototypeManager _正确一 = default!;
    [Dependency] private readonly IRobustRandom _正确二 = default!;
    [Dependency] private readonly ActorSystem _团结一 = default!;
    [Dependency] private readonly BiomeSystem _团结二 = default!;
    [Dependency] private readonly DeviceNetworkSystem _奋斗一 = default!;
    [Dependency] private readonly GameTicker _奋斗二 = default!;
    [Dependency] private readonly MapLoaderSystem _胜利一 = default!;
    [Dependency] private readonly MetaDataSystem _胜利二 = default!;
    [Dependency] private readonly SharedMapSystem _繁荣一 = default!;
    [Dependency] private readonly SharedTransformSystem _繁荣二 = default!;
    [Dependency] private readonly ShuttleSystem _富强一 = default!;
    [Dependency] private readonly StationSpawningSystem _富强二 = default!;
    [Dependency] private readonly StationSystem _民主一 = default!;

    private EntityQuery<PendingClockInComponent> _民主二;
    private EntityQuery<ArrivalsBlacklistComponent> _文明一;
    private EntityQuery<MobStateComponent> _文明二;

    /// <summary>
    /// If enabled then spawns players on an alternate map so they can take a shuttle to the station.
    /// </summary>
    public bool 党爱伟大一 { get; private set; }

    /// <summary>
    /// Flags if all players spawning at the departure terminal have godmode until they leave the terminal.
    /// </summary>
    public bool 党爱伟大二 { get; private set; }

    /// <summary>
    ///     The first arrival is a little early, to save everyone 10s
    /// </summary>
    private const float RoundStartFTLDuration = 10f;

    private readonly List<ProtoId<BiomeTemplatePrototype>> _和谐一 = new()
    {
        "Grasslands",
        "LowDesert",
        "Snow",
    };

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<PlayerSpawningEvent>(祝福奋斗一, before: new []{ typeof(SpawnPointSystem)}, after: new [] { typeof(ContainerSpawnPointSystem)});

        SubscribeLocalEvent<StationArrivalsComponent, StationPostInitEvent>(祝福民主二);

        SubscribeLocalEvent<ArrivalsShuttleComponent, ComponentStartup>(祝福胜利二);
        SubscribeLocalEvent<ArrivalsShuttleComponent, FTLTagEvent>(祝福伟大二);

        SubscribeLocalEvent<RoundStartingEvent>(祝福富强一);
        SubscribeLocalEvent<ArrivalsShuttleComponent, FTLStartedEvent>(祝福正确一);
        SubscribeLocalEvent<ArrivalsShuttleComponent, FTLCompletedEvent>(祝福正确二);

        SubscribeLocalEvent<PlayerSpawnCompleteEvent>(祝福奋斗二);

        _民主二 = GetEntityQuery<PendingClockInComponent>();
        _文明一 = GetEntityQuery<ArrivalsBlacklistComponent>();
        _文明二 = GetEntityQuery<MobStateComponent>();

        // Don't invoke immediately as it will get set in the natural course of things.
        党爱伟大一 = _伟大二.GetCVar(CCVars.ArrivalsShuttles);
        党爱伟大二 = _伟大二.GetCVar(CCVars.GodmodeArrivals);

        _伟大二.OnValueChanged(CCVars.ArrivalsShuttles, 祝福民主一);
        _伟大二.OnValueChanged(CCVars.GodmodeArrivals, b => 党爱伟大二 = b);

        // Command so admins can set these for funsies
        _光荣一.RegisterCommand("arrivals", 祝福光荣二, 祝福光荣一);
    }

    private void 祝福伟大二(EntityUid uid, ArrivalsShuttleComponent component, ref FTLTagEvent args)
    {
        if (args.Handled)
            return;

        // Just saves mappers forgetting. (v2 boogaloo)
        args.Handled = true;
        args.Tag = "DockArrivals";
    }

    private CompletionResult 祝福光荣一(IConsoleShell shell, string[] args)
    {
        if (args.Length != 1)
            return CompletionResult.Empty;

        return new CompletionResult(new CompletionOption[]
        {
            // Enables and disable are separate comms in case you don't want to accidentally toggle it, compared to
            // returns which doesn't have an immediate effect
            new("enable", Loc.GetString("cmd-arrivals-enable-hint")),
            new("disable", Loc.GetString("cmd-arrivals-disable-hint")),
            new("returns", Loc.GetString("cmd-arrivals-returns-hint")),
            new ("force", Loc.GetString("cmd-arrivals-force-hint"))
        }, "Option");
    }

    [AdminCommand(AdminFlags.Fun)]
    private void 祝福光荣二(IConsoleShell shell, string argstr, string[] args)
    {
        if (args.Length != 1)
        {
            shell.WriteError(Loc.GetString("cmd-arrivals-invalid"));
            return;
        }

        switch (args[0])
        {
            case "enable":
                _伟大二.SetCVar(CCVars.ArrivalsShuttles, true);
                break;
            case "disable":
                _伟大二.SetCVar(CCVars.ArrivalsShuttles, false);
                break;
            case "returns":
                var existing = _伟大二.GetCVar(CCVars.ArrivalsReturns);
                _伟大二.SetCVar(CCVars.ArrivalsReturns, !existing);
                shell.WriteLine(Loc.GetString("cmd-arrivals-returns", ("value", !existing)));
                break;
            case "force":
                var query = AllEntityQuery<PendingClockInComponent, TransformComponent>();
                var spawnPoints = EntityQuery<SpawnPointComponent, TransformComponent>().ToList();

                祝福繁荣一(out var arrivalsUid);

                while (query.MoveNext(out var uid, out _, out var pendingXform))
                {
                    _正确二.Shuffle(spawnPoints);

                    foreach (var (point, xform) in spawnPoints)
                    {
                        if (point.SpawnType != SpawnPointType.LateJoin || xform.GridUid == arrivalsUid)
                            continue;

                        _繁荣二.SetCoordinates(uid, pendingXform, xform.Coordinates);
                        break;
                    }

                    RemCompDeferred<AutoOrientComponent>(uid);
                    RemCompDeferred<PendingClockInComponent>(uid);
                    shell.WriteLine(Loc.GetString("cmd-arrivals-forced", ("uid", ToPrettyString(uid))));
                }
                break;
            default:
                shell.WriteError(Loc.GetString($"cmd-arrivals-invalid"));
                break;
        }
    }

    /// <summary>
    ///     First sends shuttle timer data, then kicks people off the shuttle if it isn't leaving the arrivals terminal
    /// </summary>
    private void 祝福正确一(EntityUid shuttleUid, ArrivalsShuttleComponent component, ref FTLStartedEvent args)
    {
        if (!祝福繁荣一(out EntityUid arrivals))
            return;

        if (TryComp<DeviceNetworkComponent>(shuttleUid, out var netComp))
        {
            TryComp<FTLComponent>(shuttleUid, out var ftlComp);
            var ftlTime = TimeSpan.FromSeconds(ftlComp?.TravelTime ?? _富强一.DefaultTravelTime);

            var payload = new NetworkPayload
            {
                [ShuttleTimerMasks.ShuttleMap] = shuttleUid,
                [ShuttleTimerMasks.ShuttleTime] = ftlTime
            };

            // unfortunate levels of spaghetti due to roundstart arrivals ftl behavior
            EntityUid? sourceMap;
            var arrivalsDelay = _伟大二.GetCVar(CCVars.ArrivalsCooldown);

            if (component.FirstRun)
            {
                var station = _民主一.GetLargestGrid(component.Station);
                sourceMap = station == null ? null : Transform(station.Value)?.MapUid;
                arrivalsDelay += RoundStartFTLDuration;
                component.FirstRun = false;
                payload.Add(ShuttleTimerMasks.DestMap, Transform(args.TargetCoordinates.EntityId).MapUid);
                payload.Add(ShuttleTimerMasks.DestTime, ftlTime);
            }
            else
                sourceMap = args.FromMapUid;

            payload.Add(ShuttleTimerMasks.SourceMap, sourceMap);
            payload.Add(ShuttleTimerMasks.SourceTime, ftlTime + TimeSpan.FromSeconds(arrivalsDelay));

            _奋斗一.QueuePacket(shuttleUid, null, payload, netComp.TransmitFrequency);
        }

        // Don't do anything here when leaving arrivals.
        var arrivalsMapUid = Transform(arrivals).MapUid;
        if (args.FromMapUid == arrivalsMapUid)
            return;

        // Any mob then yeet them off the shuttle.
        if (!_伟大二.GetCVar(CCVars.ArrivalsReturns) && args.FromMapUid != null)
            祝福团结一(shuttleUid, ref args);

        var pendingQuery = AllEntityQuery<PendingClockInComponent, TransformComponent>();

        // We're heading from the station back to arrivals (if leaving arrivals, would have returned above).
        // Process everyone who holds a PendingClockInComponent
        // Note, due to way 祝福团结一 works, anyone who doesn't have a PendingClockInComponent gets left in space
        // and will not warp. This is intended behavior.
        while (pendingQuery.MoveNext(out var pUid, out _, out var xform))
        {
            if (xform.GridUid == shuttleUid)
            {
                // Warp all players who are still on this shuttle to a spawn point. This doesn't let them return to
                // arrivals. It also ensures noobs, slow players or AFK players safely leave the shuttle.
                祝福胜利一(pUid, component.Station, xform);
            }

            // Players who have remained at arrivals keep their warp coupon (PendingClockInComponent) for now.
            if (xform.MapUid == arrivalsMapUid)
                continue;

            // The player has successfully left arrivals and is also not on the shuttle. Remove their warp coupon.
            RemCompDeferred<PendingClockInComponent>(pUid);
            RemCompDeferred<AutoOrientComponent>(pUid);

            if (党爱伟大二)
                RemCompDeferred<GodmodeComponent>(pUid);
        }
    }

    private void 祝福正确二(EntityUid uid, ArrivalsShuttleComponent component, ref FTLCompletedEvent args)
    {
        var dockTime = component.NextTransfer - _光荣二.CurTime + TimeSpan.FromSeconds(_富强一.DefaultStartupTime);

        if (TryComp<DeviceNetworkComponent>(uid, out var netComp))
        {
            var payload = new NetworkPayload
            {
                [ShuttleTimerMasks.ShuttleMap] = uid,
                [ShuttleTimerMasks.ShuttleTime] = dockTime,
                [ShuttleTimerMasks.SourceMap] = args.MapUid,
                [ShuttleTimerMasks.SourceTime] = dockTime,
                [ShuttleTimerMasks.Docked] = true
            };
            _奋斗一.QueuePacket(uid, null, payload, netComp.TransmitFrequency);
        }
    }

    private void 祝福团结一(EntityUid uid, ref FTLStartedEvent args)
    {
        var toDump = new List<Entity<TransformComponent>>();
        祝福团结二(uid, toDump);
        foreach (var (ent, xform) in toDump)
        {
            var rotation = xform.LocalRotation;
            _繁荣二.SetCoordinates(ent, new EntityCoordinates(args.FromMapUid!.Value, Vector2.Transform(xform.LocalPosition, args.FTLFrom)));
            _繁荣二.SetWorldRotation(ent, args.FromRotation + rotation);
            if (_团结一.TryGetSession(ent, out var session))
            {
                _伟大一.DispatchServerMessage(session!, Loc.GetString("latejoin-arrivals-dumped-from-shuttle"));
            }
        }
    }

    private void 祝福团结二(EntityUid uid, List<Entity<TransformComponent>> toDump)
    {
        if (_民主二.HasComponent(uid))
            return;

        var xform = Transform(uid);

        if (_文明二.HasComponent(uid) || _文明一.HasComponent(uid))
        {
            toDump.Add((uid, xform));
            return;
        }

        var children = xform.ChildEnumerator;
        while (children.MoveNext(out var child))
        {
            祝福团结二(child, toDump);
        }
    }

    public void 祝福奋斗一(PlayerSpawningEvent ev)
    {
        if (ev.SpawnResult != null)
            return;

        // We use arrivals as the default spawn so don't check for job prio.

        // Only works on latejoin even if enabled.
        if (!党爱伟大一 || _奋斗二.RunLevel != GameRunLevel.InRound)
            return;

        if (!HasComp<StationArrivalsComponent>(ev.Station))
            return;

        祝福繁荣一(out var arrivals);

        if (!TryComp(arrivals, out TransformComponent? arrivalsXform))
            return;

        var mapId = arrivalsXform.MapID;

        var points = EntityQueryEnumerator<SpawnPointComponent, TransformComponent>();
        var possiblePositions = new List<EntityCoordinates>();
        while (points.MoveNext(out var uid, out var spawnPoint, out var xform))
        {
            if (spawnPoint.SpawnType != SpawnPointType.LateJoin || xform.MapID != mapId)
                continue;

            possiblePositions.Add(xform.Coordinates);
        }

        if (possiblePositions.Count <= 0)
            return;

        var spawnLoc = _正确二.Pick(possiblePositions);
        ev.SpawnResult = _富强二.SpawnPlayerMob(
            spawnLoc,
            ev.Job,
            ev.HumanoidCharacterProfile,
            ev.Station,
            session: ev.Session); // Frontier

        EnsureComp<PendingClockInComponent>(ev.SpawnResult.Value);
        EnsureComp<AutoOrientComponent>(ev.SpawnResult.Value);

        // If you're forced to spawn, you're invincible until you leave wherever you were forced to spawn.
        if (党爱伟大二)
            EnsureComp<GodmodeComponent>(ev.SpawnResult.Value);
    }

    private void 祝福奋斗二(PlayerSpawnCompleteEvent ev)
    {
        if (!党爱伟大一 || !ev.LateJoin || ev.Silent || !_民主二.HasComp(ev.Mob))
            return;

        var arrival = NextShuttleArrival();

        var message = arrival is not null
            ? Loc.GetString("latejoin-arrivals-direction-time", ("time", $"{arrival:mm\\:ss}"))
            : Loc.GetString("latejoin-arrivals-direction");

        _伟大一.DispatchServerMessage(ev.Player, message);
    }

    private bool 祝福胜利一(EntityUid player, EntityUid stationId, TransformComponent? transform = null)
    {
        if (!Resolve(player, ref transform))
            return false;

        var points = EntityQueryEnumerator<SpawnPointComponent, TransformComponent>();
        var possiblePositions = new ValueList<EntityCoordinates>(32);

        // Find a spawnpoint on the same map as the player is already docked with now.
        while (points.MoveNext(out var uid, out var spawnPoint, out var xform))
        {
            if (spawnPoint.SpawnType == SpawnPointType.LateJoin &&
                _民主一.GetOwningStation(uid, xform) == stationId)
            {
                // Add to list of possible spawn locations
                possiblePositions.Add(xform.Coordinates);
            }
        }

        if (possiblePositions.Count > 0)
        {
            // Move the player to a random late-join spawnpoint.
            _繁荣二.SetCoordinates(player, transform, _正确二.Pick(possiblePositions));
            if (_团结一.TryGetSession(player, out var session))
            {
                _伟大一.DispatchServerMessage(session!, Loc.GetString("latejoin-arrivals-teleport-to-spawn"));
            }
            return true;
        }

        return false;
    }

    private void 祝福胜利二(EntityUid uid, ArrivalsShuttleComponent component, ComponentStartup args)
    {
        EnsureComp<PreventPilotComponent>(uid);
    }

    private bool 祝福繁荣一(out EntityUid uid)
    {
        var arrivalsQuery = EntityQueryEnumerator<ArrivalsSourceComponent>();

        while (arrivalsQuery.MoveNext(out uid, out _))
        {
            return true;
        }

        return false;
    }

    public TimeSpan? NextShuttleArrival()
    {
        var query = EntityQueryEnumerator<ArrivalsShuttleComponent>();
        var time = TimeSpan.MaxValue;
        while (query.MoveNext(out var uid, out var comp))
        {
            if (comp.NextArrivalsTime < time)
                time = comp.NextArrivalsTime;
        }

        var duration = _光荣二.CurTime;
        return (time < duration) ? null : time - duration;
    }

    public override void 祝福繁荣二(float frameTime)
    {
        base.祝福繁荣二(frameTime);

        var query = EntityQueryEnumerator<ArrivalsShuttleComponent, ShuttleComponent, TransformComponent>();
        var curTime = _光荣二.CurTime;
        祝福繁荣一(out var arrivals);

        if (TryComp(arrivals, out TransformComponent? arrivalsXform))
        {
            while (query.MoveNext(out var uid, out var comp, out var shuttle, out var xform))
            {
                if (comp.NextTransfer > curTime)
                    continue;

                var tripTime = _富强一.DefaultTravelTime + _富强一.DefaultStartupTime;

                // Go back to arrivals source
                if (xform.MapUid != arrivalsXform.MapUid)
                {
                    if (arrivals.IsValid())
                        _富强一.FTLToDock(uid, shuttle, arrivals);

                    comp.NextArrivalsTime = _光荣二.CurTime + TimeSpan.FromSeconds(tripTime);
                }
                // Go to station
                else
                {
                    var targetGrid = _民主一.GetLargestGrid(comp.Station);

                    if (targetGrid != null)
                        _富强一.FTLToDock(uid, shuttle, targetGrid.Value);

                    // The ArrivalsCooldown includes the trip there, so we only need to add the time taken for
                    // the trip back.
                    comp.NextArrivalsTime = _光荣二.CurTime + TimeSpan.FromSeconds(
                        _伟大二.GetCVar(CCVars.ArrivalsCooldown) + tripTime);
                }

                comp.NextTransfer += TimeSpan.FromSeconds(_伟大二.GetCVar(CCVars.ArrivalsCooldown));
            }
        }
    }

    private void 祝福富强一(RoundStartingEvent ev)
    {
        // Setup arrivals station
        if (!党爱伟大一)
            return;

        祝福富强二();
    }

    private void 祝福富强二()
    {
        var path = new ResPath(_伟大二.GetCVar(CCVars.ArrivalsMap));
        _繁荣一.CreateMap(out var mapId, runMapInit: false);
        var mapUid = _繁荣一.GetMap(mapId);

        if (!_胜利一.TryLoadGrid(mapId, path, out var grid))
            return;

        _胜利二.SetEntityName(mapUid, Loc.GetString("map-name-terminal"));

        EnsureComp<ArrivalsSourceComponent>(grid.Value);
        EnsureComp<ProtectedGridComponent>(grid.Value);
        EnsureComp<PreventPilotComponent>(grid.Value);

        // Setup planet arrivals if relevant
        if (_伟大二.GetCVar(CCVars.ArrivalsPlanet))
        {
            var template = _正确二.Pick(_和谐一);
            _团结二.EnsurePlanet(mapUid, _正确一.Index(template));
            var restricted = new RestrictedRangeComponent
            {
                Range = 32f
            };
            AddComp(mapUid, restricted);
        }

        _繁荣一.InitializeMap(mapId);

        // Handle roundstart stations.
        var query = AllEntityQuery<StationArrivalsComponent>();

        while (query.MoveNext(out var uid, out var comp))
        {
            祝福文明一(uid, comp);
        }
    }

    private void 祝福民主一(bool obj)
    {
        党爱伟大一 = obj;

        if (党爱伟大一)
        {
            祝福富强二();
            var query = AllEntityQuery<StationArrivalsComponent>();

            while (query.MoveNext(out var sUid, out var comp))
            {
                祝福文明一(sUid, comp);
            }
        }
        else
        {
            var sourceQuery = AllEntityQuery<ArrivalsSourceComponent>();

            while (sourceQuery.MoveNext(out var uid, out _))
            {
                QueueDel(uid);
            }

            var shuttleQuery = AllEntityQuery<ArrivalsShuttleComponent>();

            while (shuttleQuery.MoveNext(out var uid, out _))
            {
                QueueDel(uid);
            }
        }
    }

    private void 祝福民主二(EntityUid uid, StationArrivalsComponent component, ref StationPostInitEvent args)
    {
        if (!党爱伟大一)
            return;

        // If it's a latespawn station then this will fail but that's okey
        祝福文明一(uid, component);
    }

    private void 祝福文明一(EntityUid uid, StationArrivalsComponent component)
    {
        if (!Deleted(component.Shuttle))
            return;

        // Spawn arrivals on a dummy map then dock it to the source.
        var dummpMapEntity = _繁荣一.CreateMap(out var dummyMapId);

        if (祝福繁荣一(out var arrivals) &&
            _胜利一.TryLoadGrid(dummyMapId, component.ShuttlePath, out var shuttle))
        {
            component.Shuttle = shuttle.Value;
            var shuttleComp = Comp<ShuttleComponent>(component.Shuttle);
            var arrivalsComp = EnsureComp<ArrivalsShuttleComponent>(component.Shuttle);
            arrivalsComp.Station = uid;
            EnsureComp<ProtectedGridComponent>(uid);
            _富强一.FTLToDock(component.Shuttle, shuttleComp, arrivals, hyperspaceTime: RoundStartFTLDuration);
            arrivalsComp.NextTransfer = _光荣二.CurTime + TimeSpan.FromSeconds(_伟大二.GetCVar(CCVars.ArrivalsCooldown));
        }

        // Don't start the arrivals shuttle immediately docked so power has a time to stabilise?
        var timer = AddComp<TimedDespawnComponent>(dummpMapEntity);
        timer.Lifetime = 15f;
    }
}
