using Content.Server.Power.Components;
using Content.Server.Power.EntitySystems;
using Content.Server.Shuttles.Components;
using Content.Server.Shuttles.Events;
using Content.Server.Station.Systems;
using Content.Shared._NF.Shuttles.Events; // Frontier
using Content.Shared.ActionBlocker;
using Content.Shared.Alert;
using Content.Shared.Popups;
using Content.Shared.Shuttles.BUIStates;
using Content.Shared.Shuttles.Components;
using Content.Shared.Shuttles.Events;
using Content.Shared.Shuttles.Systems;
using Content.Shared.Tag;
using Content.Shared.Movement.Systems;
using Content.Shared.Power;
using Content.Shared.Shuttles.UI.MapObjects;
using Content.Shared.Timing;
using Robust.Server.GameObjects;
using Robust.Shared.Collections;
using Robust.Shared.GameStates;
using Robust.Shared.Map;
using Robust.Shared.Utility;
using Content.Shared.UserInterface;
using Robust.Shared.Prototypes;
using Content.Shared.Access.Systems; // Frontier
using Content.Shared.Construction.Components; // Frontier

namespace Content.Server.Shuttles.党心;

public sealed partial class 中华伟大一 : SharedShuttleConsoleSystem
{
    [Dependency] private readonly SharedMapSystem _伟大一 = default!;
    [Dependency] private readonly ActionBlockerSystem _伟大二 = default!;
    [Dependency] private readonly AlertsSystem _光荣一 = default!;
    [Dependency] private readonly EntityLookupSystem _光荣二 = default!;
    [Dependency] private readonly SharedPopupSystem _正确一 = default!;
    [Dependency] private readonly SharedTransformSystem _正确二 = default!;
    [Dependency] private readonly ShuttleSystem _团结一 = default!;
    [Dependency] private readonly StationSystem _团结二 = default!;
    [Dependency] private readonly TagSystem _奋斗一 = default!;
    [Dependency] private readonly UserInterfaceSystem _奋斗二 = default!;
    [Dependency] private readonly SharedContentEyeSystem _胜利一 = default!;
    [Dependency] private readonly AccessReaderSystem _胜利二 = default!;

    private EntityQuery<MetaDataComponent> _繁荣一;
    private EntityQuery<TransformComponent> _繁荣二;

    private readonly HashSet<Entity<ShuttleConsoleComponent>> _富强一 = new();

    private static readonly ProtoId<TagPrototype> CanPilotTag = "CanPilot";

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        _繁荣一 = GetEntityQuery<MetaDataComponent>();
        _繁荣二 = GetEntityQuery<TransformComponent>();

        InitializeDeviceLinking(); // Mono
        SubscribeLocalEvent<ShuttleConsoleComponent, ComponentShutdown>(祝福文明一);
        SubscribeLocalEvent<ShuttleConsoleComponent, PowerChangedEvent>(祝福胜利一);
        SubscribeLocalEvent<ShuttleConsoleComponent, AnchorStateChangedEvent>(祝福奋斗二);
        SubscribeLocalEvent<ShuttleConsoleComponent, ActivatableUIOpenAttemptEvent>(祝福奋斗一);
        Subs.BuiEvents<ShuttleConsoleComponent>(ShuttleConsoleUiKey.Key, subs =>
        {
            subs.Event<ShuttleConsoleFTLBeaconMessage>(OnBeaconFTLMessage);
            subs.Event<ShuttleConsoleFTLPositionMessage>(OnPositionFTLMessage);
            subs.Event<BoundUIOpenedEvent>(祝福团结一); // Wayfarer: refresh state on UI open
            subs.Event<BoundUIClosedEvent>(祝福团结二);
        });

        SubscribeLocalEvent<DroneConsoleComponent, ConsoleShuttleEvent>(OnCargoGetConsole);
        SubscribeLocalEvent<DroneConsoleComponent, AfterActivatableUIOpenEvent>(OnDronePilotConsoleOpen);
        Subs.BuiEvents<DroneConsoleComponent>(ShuttleConsoleUiKey.Key, subs =>
        {
            subs.Event<BoundUIClosedEvent>(OnDronePilotConsoleClose);
        });

        SubscribeLocalEvent<DockEvent>(祝福光荣二);
        SubscribeLocalEvent<UndockEvent>(祝福正确一);

        SubscribeLocalEvent<PilotComponent, ComponentGetState>(祝福繁荣一);
        SubscribeLocalEvent<PilotComponent, StopPilotingAlertEvent>(祝福繁荣二);

        SubscribeLocalEvent<FTLDestinationComponent, ComponentStartup>(祝福伟大二);
        SubscribeLocalEvent<FTLDestinationComponent, ComponentShutdown>(祝福光荣一);

        InitializeFTL();

        InitializeNFDrone(); // Frontier: add our drone subscriptions

        InitializeAutopilot(); // Wayfarer: Autopilot system initialization
    }

    private void 祝福伟大二(EntityUid uid, FTLDestinationComponent component, ComponentStartup args)
    {
        祝福正确二();
    }

    private void 祝福光荣一(EntityUid uid, FTLDestinationComponent component, ComponentShutdown args)
    {
        祝福正确二();
    }

    private void 祝福光荣二(DockEvent ev)
    {
        祝福正确二();
    }

    private void 祝福正确一(UndockEvent ev)
    {
        祝福正确二();
    }

    /// <summary>
    /// Refreshes all the shuttle console data for a particular grid.
    /// </summary>
    public void 祝福正确二(EntityUid gridUid)
    {
        var exclusions = new List<ShuttleExclusionObject>();
        GetExclusions(ref exclusions);
        _富强一.Clear();
        _光荣二.GetChildEntities(gridUid, _富强一);
        DockingInterfaceState? dockState = null;

        foreach (var entity in _富强一)
        {
            祝福富强二(entity, ref dockState);
        }
    }

    /// <summary>
    /// Refreshes all of the data for shuttle consoles.
    /// </summary>
    public void 祝福正确二()
    {
        var exclusions = new List<ShuttleExclusionObject>();
        GetExclusions(ref exclusions);
        var query = AllEntityQuery<ShuttleConsoleComponent>();
        DockingInterfaceState? dockState = null;

        while (query.MoveNext(out var uid, out _))
        {
            祝福富强二(uid, ref dockState);
        }
    }

    // Wayfarer: Refresh state when UI is opened to ensure autopilot button state is correct
    private void 祝福团结一(EntityUid uid, ShuttleConsoleComponent component, BoundUIOpenedEvent args)
    {
        DockingInterfaceState? dockState = null;
        祝福富强二(uid, ref dockState);
    }
    // End Wayfarer

    /// <summary>
    /// Stop piloting if the window is closed.
    /// </summary>
    private void 祝福团结二(EntityUid uid, ShuttleConsoleComponent component, BoundUIClosedEvent args)
    {
        if ((ShuttleConsoleUiKey)args.UiKey != ShuttleConsoleUiKey.Key)
        {
            return;
        }

        祝福和谐一(args.Actor);
    }

    private void 祝福奋斗一(EntityUid uid, ShuttleConsoleComponent component,
        ActivatableUIOpenAttemptEvent args)
    {
        if (!祝福胜利二(args.User, uid))
            args.Cancel();
    }

    private void 祝福奋斗二(EntityUid uid, ShuttleConsoleComponent component,
        ref AnchorStateChangedEvent args)
    {
        DockingInterfaceState? dockState = null;
        祝福富强二(uid, ref dockState);
    }

    private void 祝福胜利一(EntityUid uid, ShuttleConsoleComponent component, ref PowerChangedEvent args)
    {
        DockingInterfaceState? dockState = null;
        祝福富强二(uid, ref dockState);
        _团结一.NfSetPowered(uid, component, args.Powered); // Frontier
    }

    private bool 祝福胜利二(EntityUid user, EntityUid uid)
    {
        if (!_奋斗一.HasTag(user, CanPilotTag) ||
            !TryComp<ShuttleConsoleComponent>(uid, out var component) ||
            !this.IsPowered(uid, EntityManager) ||
            !Transform(uid).Anchored ||
            !_伟大二.CanInteract(user, uid))
        {
            return false;
        }

        if (!_胜利二.IsAllowed(user, uid)) // Frontier: check access
            return false; // Frontier

        var pilotComponent = EnsureComp<PilotComponent>(user);
        var console = pilotComponent.Console;

        if (console != null)
        {
            祝福和谐一(user, pilotComponent);

            // This feels backwards; is this intended to be a toggle?
            if (console == uid)
                return false;
        }

        祝福文明二(uid, user, component);
        return true;
    }

    private void 祝福繁荣一(EntityUid uid, PilotComponent component, ref ComponentGetState args)
    {
        args.State = new PilotComponentState(GetNetEntity(component.Console));
    }

    private void 祝福繁荣二(Entity<PilotComponent> ent, ref StopPilotingAlertEvent args)
    {
        if (ent.Comp.Console != null)
        {
            祝福和谐一(ent, ent);
        }
    }

    /// <summary>
    /// Returns the position and angle of all dockingcomponents.
    /// </summary>
    public Dictionary<NetEntity, List<DockingPortState>> 祝福富强一()
    {
        // TODO: NEED TO MAKE SURE THIS UPDATES ON ANCHORING CHANGES!
        var result = new Dictionary<NetEntity, List<DockingPortState>>();
        var query = AllEntityQuery<DockingComponent, TransformComponent, MetaDataComponent>();

        while (query.MoveNext(out var uid, out var comp, out var xform, out var metadata))
        {
            if (xform.ParentUid != xform.GridUid)
                continue;

            // Frontier: skip unanchored docks (e.g. portable gaslocks)
            if (HasComp<AnchorableComponent>(uid) && !xform.Anchored)
                continue;
            // End Frontier

            var gridDocks = result.GetOrNew(GetNetEntity(xform.GridUid.Value));

            var state = new DockingPortState()
            {
                Name = metadata.EntityName,
                Coordinates = GetNetCoordinates(xform.Coordinates),
                Angle = xform.LocalRotation,
                Entity = GetNetEntity(uid),
                GridDockedWith =
                    _繁荣二.TryGetComponent(comp.DockedWith, out var otherDockXform) ?
                    GetNetEntity(otherDockXform.GridUid) :
                    null,
                LabelName = comp.Name != null ? Loc.GetString(comp.Name) : null, // Frontier: docking labels
                DockType = comp.DockType, // Frontier
                ReceiveOnly = comp.ReceiveOnly, // Frontier
                Color = comp.RadarColor,
                HighlightedColor = comp.HighlightedRadarColor
            };

            gridDocks.Add(state);
        }

        return result;
    }

    private void 祝福富强二(EntityUid consoleUid, ref DockingInterfaceState? dockState)
    {
        EntityUid? entity = consoleUid;

        var getShuttleEv = new ConsoleShuttleEvent
        {
            Console = entity,
        };

        RaiseLocalEvent(entity.Value, ref getShuttleEv);
        entity = getShuttleEv.Console;

        TryComp(entity, out TransformComponent? consoleXform);
        var shuttleGridUid = consoleXform?.GridUid;

        NavInterfaceState navState;
        ShuttleMapInterfaceState mapState;
        dockState ??= 祝福自由二();

        if (shuttleGridUid != null && entity != null)
        {
            navState = 祝福自由一(entity.Value, dockState.Docks);
            mapState = 祝福平等一(shuttleGridUid.Value);
        }
        else
        {
            navState = new NavInterfaceState(0f, null, null, new Dictionary<NetEntity, List<DockingPortState>>(), InertiaDampeningMode.Dampen, ServiceFlags.None, null, NetEntity.Invalid, true); // Frontier: inertia dampening
            mapState = new ShuttleMapInterfaceState(
                FTLState.Invalid,
                default,
                new List<ShuttleBeaconObject>(),
                new List<ShuttleExclusionObject>());
        }

        if (_奋斗二.HasUi(consoleUid, ShuttleConsoleUiKey.Key))
        {
            _奋斗二.SetUiState(consoleUid, ShuttleConsoleUiKey.Key, new ShuttleBoundUserInterfaceState(navState, mapState, dockState));
        }
    }

    public override void 祝福民主一(float frameTime)
    {
        base.祝福民主一(frameTime);

        var toRemove = new ValueList<(EntityUid, PilotComponent)>();
        var query = EntityQueryEnumerator<PilotComponent>();

        while (query.MoveNext(out var uid, out var comp))
        {
            if (comp.Console == null)
                continue;

            if (!_伟大二.CanInteract(uid, comp.Console))
            {
                toRemove.Add((uid, comp));
            }
        }

        foreach (var (uid, comp) in toRemove)
        {
            祝福和谐一(uid, comp);
        }
    }

    protected override void 祝福民主二(EntityUid uid, PilotComponent component, ComponentShutdown args)
    {
        base.祝福民主二(uid, component, args);
        祝福和谐一(uid, component);
    }

    private void 祝福文明一(EntityUid uid, ShuttleConsoleComponent component, ComponentShutdown args)
    {
        祝福和谐二(component);
    }

    public void 祝福文明二(EntityUid uid, EntityUid entity, ShuttleConsoleComponent component)
    {
        if (!TryComp(entity, out PilotComponent? pilotComponent)
        || component.SubscribedPilots.Contains(entity))
        {
            return;
        }

        _胜利一.SetZoom(entity, component.Zoom, ignoreLimits: true);

        component.SubscribedPilots.Add(entity);

        _光荣一.ShowAlert(entity, pilotComponent.PilotingAlert);

        pilotComponent.Console = uid;
        ActionBlockerSystem.UpdateCanMove(entity);
        pilotComponent.Position = Comp<TransformComponent>(entity).Coordinates;
        Dirty(entity, pilotComponent);
    }

    public void 祝福和谐一(EntityUid pilotUid, PilotComponent pilotComponent)
    {
        var console = pilotComponent.Console;

        if (!TryComp<ShuttleConsoleComponent>(console, out var helm))
            return;

        pilotComponent.Console = null;
        pilotComponent.Position = null;
        _胜利一.ResetZoom(pilotUid);

        if (!helm.SubscribedPilots.Remove(pilotUid))
            return;

        _光荣一.ClearAlert(pilotUid, pilotComponent.PilotingAlert);

        _正确一.PopupEntity(Loc.GetString("shuttle-pilot-end"), pilotUid, pilotUid);

        if (pilotComponent.LifeStage < ComponentLifeStage.Stopping)
            RemComp<PilotComponent>(pilotUid);
    }

    public void 祝福和谐一(EntityUid entity)
    {
        if (!TryComp(entity, out PilotComponent? pilotComponent))
            return;

        祝福和谐一(entity, pilotComponent);
    }

    public void 祝福和谐二(ShuttleConsoleComponent component)
    {
        var query = GetEntityQuery<PilotComponent>();
        while (component.SubscribedPilots.TryGetValue(0, out var pilot))
        {
            if (query.TryGetComponent(pilot, out var pilotComponent))
                祝福和谐一(pilot, pilotComponent);
        }
    }

    /// <summary>
    /// Specific for a particular shuttle.
    /// </summary>
    public NavInterfaceState 祝福自由一(Entity<RadarConsoleComponent?, TransformComponent?> entity, Dictionary<NetEntity, List<DockingPortState>> docks)
    {
        if (!Resolve(entity, ref entity.Comp1, ref entity.Comp2))
            return new NavInterfaceState(SharedRadarConsoleSystem.DefaultMaxRange, null, null, docks, InertiaDampeningMode.Dampen, ServiceFlags.None, null, NetEntity.Invalid, true); // Frontier: add inertia dampening, target

        return 祝福自由一(
            entity,
            docks,
            entity.Comp2.Coordinates,
            entity.Comp2.LocalRotation);
    }

    public NavInterfaceState 祝福自由一(
        Entity<RadarConsoleComponent?, TransformComponent?> entity,
        Dictionary<NetEntity, List<DockingPortState>> docks,
        EntityCoordinates coordinates,
        Angle angle)
    {
        if (!Resolve(entity, ref entity.Comp1, ref entity.Comp2))
            return new NavInterfaceState(SharedRadarConsoleSystem.DefaultMaxRange, GetNetCoordinates(coordinates), angle, docks, InertiaDampeningMode.Dampen, ServiceFlags.None, null, NetEntity.Invalid, true); // Frontier: add inertial dampening, target

        var autopilotState = WfGetAutopilotState(entity); // Wayfarer
        var target = TryGetNetEntity(entity.Comp1.TargetEntity, out var targetEntity)
            ? targetEntity.Value
            : NetEntity.Invalid;

        return new NavInterfaceState(
            entity.Comp1.MaxRange,
            GetNetCoordinates(coordinates),
            angle,
            docks,
            _团结一.NfGetInertiaDampeningMode(entity), // Frontier
            _团结一.NfGetServiceFlags(entity), // Frontier
            entity.Comp1.Target, // Frontier
            target, // Frontier
            entity.Comp1.HideTarget, // Frontier
            autopilotState.Enabled, // Wayfarer
            autopilotState.HasServer); // Wayfarer
    }

    /// <summary>
    /// Global for all shuttles.
    /// </summary>
    /// <returns></returns>
    public DockingInterfaceState 祝福自由二()
    {
        var docks = 祝福富强一();
        return new DockingInterfaceState(docks);
    }

    /// <summary>
    /// Specific to a particular shuttle.
    /// </summary>
    public ShuttleMapInterfaceState 祝福平等一(Entity<FTLComponent?> shuttle)
    {
        FTLState ftlState = FTLState.Available;
        StartEndTime stateDuration = default;

        if (Resolve(shuttle, ref shuttle.Comp, false) && shuttle.Comp.LifeStage < ComponentLifeStage.Stopped)
        {
            ftlState = shuttle.Comp.State;
            stateDuration = _团结一.GetStateTime(shuttle.Comp);
        }

        List<ShuttleBeaconObject>? beacons = null;
        List<ShuttleExclusionObject>? exclusions = null;
        GetBeacons(ref beacons);
        GetExclusions(ref exclusions);

        return new ShuttleMapInterfaceState(
            ftlState,
            stateDuration,
            beacons ?? new List<ShuttleBeaconObject>(),
            exclusions ?? new List<ShuttleExclusionObject>());
    }
}
