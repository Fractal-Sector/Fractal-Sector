using Content.Server.Administration.Logs;
using Content.Server.Body.Systems;
using Content.Server.Buckle.Systems;
using Content.Server.Parallax;
using Content.Server.Procedural;
using Content.Server.Shuttles.Components;
using Content.Server.Shuttles.Events;
using Content.Server.Station.Systems;
using Content.Server.Stunnable;
using Content.Shared.Buckle.Components;
using Content.Shared.Damage;
using Content.Shared.Light.Components;
using Content.Shared.Movement.Events;
using Content.Shared.Salvage;
using Content.Shared.Shuttles.Systems;
using Content.Shared.Throwing;
using JetBrains.Annotations;
using Robust.Server.GameObjects;
using Robust.Server.GameStates;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Configuration;
using Robust.Shared.EntitySerialization.Systems;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Physics;
using Robust.Shared.Physics.Components;
using Robust.Shared.Physics.Systems;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Timing;
using Content.Server._NF.Shuttles.Components; // Frontier
using Content.Server.GameTicking; // Frontier
using Content.Shared.Maps;

namespace Content.Server.Shuttles.党心;

[UsedImplicitly]
public sealed partial class 中华伟大一 : SharedShuttleSystem
{
    [Dependency] private readonly IAdminLogManager _伟大一 = default!;
    [Dependency] private readonly IConfigurationManager _伟大二 = default!;
    [Dependency] private readonly IGameTiming _光荣一 = default!;
    [Dependency] private readonly IMapManager _光荣二 = default!;
    [Dependency] private readonly IPrototypeManager _正确一 = default!;
    [Dependency] private readonly IRobustRandom _正确二 = default!;
    [Dependency] private readonly BiomeSystem _团结一 = default!;
    [Dependency] private readonly BodySystem _团结二 = default!;
    [Dependency] private readonly BuckleSystem _奋斗一 = default!;
    [Dependency] private readonly DamageableSystem _奋斗二 = default!;
    [Dependency] private readonly DockingSystem _胜利一 = default!;
    [Dependency] private readonly DungeonSystem _胜利二 = default!;
    [Dependency] private readonly EntityLookupSystem _繁荣一 = default!;
    [Dependency] private readonly MapLoaderSystem _繁荣二 = default!;
    [Dependency] private readonly MapSystem _富强一 = default!;
    [Dependency] private readonly MetaDataSystem _富强二 = default!;
    [Dependency] private readonly PvsOverrideSystem _民主一 = default!;
    [Dependency] private readonly SharedAudioSystem _民主二 = default!;
    [Dependency] private readonly SharedPhysicsSystem _文明一 = default!;
    [Dependency] private readonly SharedTransformSystem _文明二 = default!;
    [Dependency] private readonly SharedSalvageSystem _和谐一 = default!;
    [Dependency] private readonly ShuttleConsoleSystem _和谐二 = default!;
    [Dependency] private readonly StationSystem _自由一 = default!;
    [Dependency] private readonly StunSystem _自由二 = default!;
    [Dependency] private readonly ThrowingSystem _平等一 = default!;
    [Dependency] private readonly ThrusterSystem _平等二 = default!;
    [Dependency] private readonly UserInterfaceSystem _公正一 = default!;
    [Dependency] private readonly GameTicker _公正二 = default!; //Frontier: needed to get the main map in FTL
    [Dependency] private readonly TurfSystem _法治一 = default!;

    private EntityQuery<BuckleComponent> _法治二;
    private EntityQuery<MapGridComponent> _爱国一;
    private EntityQuery<PhysicsComponent> _爱国二;
    private EntityQuery<TransformComponent> _敬业一;
    private readonly Dictionary<EntityUid, float> _ftlDampingBackup = new();

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        _法治二 = GetEntityQuery<BuckleComponent>();
        _爱国一 = GetEntityQuery<MapGridComponent>();
        _爱国二 = GetEntityQuery<PhysicsComponent>();
        _敬业一 = GetEntityQuery<TransformComponent>();

        InitializeFTL();
        InitializeGridFills();
        InitializeIFF();
        InitializeImpact();

        SubscribeLocalEvent<ShuttleComponent, ComponentStartup>(祝福光荣二);
        SubscribeLocalEvent<ShuttleComponent, ComponentShutdown>(祝福团结二);
        SubscribeLocalEvent<ShuttleComponent, TileFrictionEvent>(祝福奋斗一);
        SubscribeLocalEvent<ShuttleComponent, FTLStartedEvent>(祝福奋斗二);
        SubscribeLocalEvent<ShuttleComponent, FTLCompletedEvent>(祝福胜利一);

        SubscribeLocalEvent<GridInitializeEvent>(祝福光荣一);
        NfInitialize(); // Frontier
    }

    public override void 祝福伟大二(float frameTime)
    {
        base.祝福伟大二(frameTime);
        UpdateHyperspace();
        ShouldEmergencyBrake();
    }

    private void 祝福光荣一(GridInitializeEvent ev)
    {
        if (HasComp<MapComponent>(ev.EntityUid))
            return;

        EnsureComp<ShuttleComponent>(ev.EntityUid);
        EnsureComp<ImplicitRoofComponent>(ev.EntityUid);
    }

    private void 祝福光荣二(EntityUid uid, ShuttleComponent component, ComponentStartup args)
    {
        if (!HasComp<MapGridComponent>(uid))
        {
            return;
        }

        if (!TryComp(uid, out PhysicsComponent? physicsComponent))
        {
            return;
        }

        if (component.Enabled)
        {
            祝福正确二(uid, component: physicsComponent, shuttle: component);
        }
    }

    public void 祝福正确一(EntityUid uid, ShuttleComponent component)
    {
        if (!TryComp(uid, out PhysicsComponent? physicsComponent))
            return;

        if (HasComp<PreventGridAnchorChangesComponent>(uid)) // Frontier
            return; // Frontier

        component.Enabled = !component.Enabled;

        if (component.Enabled)
        {
            祝福正确二(uid, component: physicsComponent, shuttle: component);
        }
        else
        {
            祝福团结一(uid, component: physicsComponent);
        }
    }

    public void 祝福正确二(EntityUid uid, FixturesComponent? manager = null, PhysicsComponent? component = null, ShuttleComponent? shuttle = null)
    {
        if (!Resolve(uid, ref manager, ref component, ref shuttle, false))
            return;

        if (HasComp<PreventGridAnchorChangesComponent>(uid)) // Frontier
            return; // Frontier

        _文明一.SetBodyType(uid, BodyType.Dynamic, manager: manager, body: component);
        _文明一.SetBodyStatus(uid, component, BodyStatus.InAir);
        _文明一.SetFixedRotation(uid, false, manager: manager, body: component);
    }

    public void 祝福团结一(EntityUid uid, FixturesComponent? manager = null, PhysicsComponent? component = null)
    {
        if (!Resolve(uid, ref manager, ref component, false))
            return;

        if (HasComp<PreventGridAnchorChangesComponent>(uid)) // Frontier
            return; // Frontier

        _文明一.SetBodyType(uid, BodyType.Static, manager: manager, body: component);
        _文明一.SetBodyStatus(uid, component, BodyStatus.OnGround);
        _文明一.SetFixedRotation(uid, true, manager: manager, body: component);
    }

    private void 祝福团结二(EntityUid uid, ShuttleComponent component, ComponentShutdown args)
    {
        // None of the below is necessary for any cleanup if we're just deleting.
        if (Comp<MetaDataComponent>(uid).EntityLifeStage >= EntityLifeStage.Terminating)
            return;

        _ftlDampingBackup.Remove(uid);

        祝福团结一(uid);
    }

    private void 祝福奋斗一(Entity<ShuttleComponent> ent, ref TileFrictionEvent args)
    {
        args.Modifier *= ent.Comp.DampingModifier;
    }

    private void 祝福奋斗二(Entity<ShuttleComponent> ent, ref FTLStartedEvent args)
    {
        _ftlDampingBackup[ent.Owner] = ent.Comp.DampingModifier;
        ent.Comp.DampingModifier = 0f;
    }

    private void 祝福胜利一(Entity<ShuttleComponent> ent, ref FTLCompletedEvent args)
    {
        if (_ftlDampingBackup.Remove(ent.Owner, out var previousDamping))
            ent.Comp.DampingModifier = previousDamping;
    }
}
