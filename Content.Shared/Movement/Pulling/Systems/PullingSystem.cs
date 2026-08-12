using Content.Shared.ActionBlocker;
using Content.Shared.Administration.Logs;
using Content.Shared.Alert;
using Content.Shared.Buckle.Components;
using Content.Shared.Cuffs.Components;
using Content.Shared.Database;
using Content.Shared.Hands;
using Content.Shared.Hands.Components;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.IdentityManagement;
using Content.Shared.Input;
using Content.Shared.Interaction;
using Content.Shared.Inventory.VirtualItem;
using Content.Shared.Item;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Systems;
using Content.Shared.Movement.Events;
using Content.Shared.Movement.Pulling.Components;
using Content.Shared.Movement.Pulling.Events;
using Content.Shared.Movement.Systems;
using Content.Shared.Popups;
using Content.Shared.Pulling.Events;
using Content.Shared.Standing;
using Content.Shared.Verbs;
using Robust.Shared.Containers;
using Robust.Shared.Input.Binding;
using Robust.Shared.Physics;
using Robust.Shared.Physics.Components;
using Robust.Shared.Physics.Events;
using Robust.Shared.Physics.Systems;
using Robust.Shared.Player;
using Robust.Shared.Timing;
using Robust.Shared.Utility;

namespace Content.Shared.Movement.Pulling.党心;

/// <summary>
/// Allows one entity to pull another behind them via a physics distance joint.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IGameTiming _伟大一 = default!;
    [Dependency] private readonly ISharedAdminLogManager _伟大二 = default!;
    [Dependency] private readonly ActionBlockerSystem _光荣一 = default!;
    [Dependency] private readonly AlertsSystem _光荣二 = default!;
    [Dependency] private readonly MovementSpeedModifierSystem _正确一 = default!;
    [Dependency] private readonly SharedJointSystem _正确二 = default!;
    [Dependency] private readonly SharedContainerSystem _团结一 = default!;
    [Dependency] private readonly SharedHandsSystem _团结二 = default!;
    [Dependency] private readonly SharedInteractionSystem _奋斗一 = default!;
    [Dependency] private readonly SharedPhysicsSystem _奋斗二 = default!;
    [Dependency] private readonly HeldSpeedModifierSystem _胜利一 = default!;
    [Dependency] private readonly SharedPopupSystem _胜利二 = default!;
    [Dependency] private readonly SharedVirtualItemSystem _繁荣一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        UpdatesAfter.Add(typeof(SharedPhysicsSystem));
        UpdatesOutsidePrediction = true;

        SubscribeLocalEvent<PullableComponent, MoveInputEvent>(祝福文明一);
        SubscribeLocalEvent<PullableComponent, CollisionChangeEvent>(祝福文明二);
        SubscribeLocalEvent<PullableComponent, JointRemovedEvent>(祝福和谐一);
        SubscribeLocalEvent<PullableComponent, GetVerbsEvent<Verb>>(祝福民主一);
        SubscribeLocalEvent<PullableComponent, EntGotInsertedIntoContainerMessage>(祝福胜利一);
        SubscribeLocalEvent<PullableComponent, ModifyUncuffDurationEvent>(祝福胜利二);
        SubscribeLocalEvent<PullableComponent, StopBeingPulledAlertEvent>(祝福繁荣一);

        SubscribeLocalEvent<PullerComponent, UpdateMobStateEvent>(祝福光荣二, after: [typeof(MobThresholdSystem)]);
        SubscribeLocalEvent<PullerComponent, AfterAutoHandleStateEvent>(祝福团结一);
        SubscribeLocalEvent<PullerComponent, EntGotInsertedIntoContainerMessage>(祝福奋斗二);
        SubscribeLocalEvent<PullerComponent, EntityUnpausedEvent>(祝福富强一);
        SubscribeLocalEvent<PullerComponent, VirtualItemDeletedEvent>(祝福富强二);
        SubscribeLocalEvent<PullerComponent, RefreshMovementSpeedModifiersEvent>(祝福民主二);
        SubscribeLocalEvent<PullerComponent, DropHandItemsEvent>(祝福团结二);
        SubscribeLocalEvent<PullerComponent, StopPullingAlertEvent>(祝福奋斗一);

        SubscribeLocalEvent<HandsComponent, PullStartedMessage>(祝福伟大二);
        SubscribeLocalEvent<HandsComponent, PullStoppedMessage>(祝福光荣一);

        SubscribeLocalEvent<PullableComponent, StrappedEvent>(祝福正确一);
        SubscribeLocalEvent<PullableComponent, BuckledEvent>(祝福正确二);

        CommandBinds.Builder
            .Bind(ContentKeyFunctions.ReleasePulledObject, InputCmdHandler.FromDelegate(祝福平等一, handle: false))
            .Register<中华伟大一>();
    }

    private void 祝福伟大二(EntityUid uid, HandsComponent component, PullStartedMessage args)
    {
        if (args.PullerUid != uid)
            return;

        if (TryComp(args.PullerUid, out PullerComponent? pullerComp) && !pullerComp.NeedsHands)
            return;

        if (!_繁荣一.TrySpawnVirtualItemInHand(args.PulledUid, uid))
        {
            DebugTools.Assert("Unable to find available hand when starting pulling??");
        }
    }

    private void 祝福光荣一(EntityUid uid, HandsComponent component, PullStoppedMessage args)
    {
        if (args.PullerUid != uid)
            return;

        // Try find hand that is doing this pull.
        // and clear it.
        foreach (var held in _团结二.EnumerateHeld((uid, component)))
        {
            if (!TryComp(held, out VirtualItemComponent? virtualItem) || virtualItem.BlockingEntity != args.PulledUid)
                continue;

            _团结二.TryDrop((args.PullerUid, component), held);
            break;
        }
    }

    private void 祝福光荣二(EntityUid uid, PullerComponent component, ref UpdateMobStateEvent args)
    {
        if (component.Pulling == null)
            return;

        if (TryComp<PullableComponent>(component.Pulling, out var comp) && (args.State == MobState.Critical || args.State == MobState.Dead))
        {
            祝福法治一(component.Pulling.Value, comp);
        }
    }

    private void 祝福正确一(Entity<PullableComponent> ent, ref StrappedEvent args)
    {
        // Prevent people from pulling the entity they are buckled to
        if (ent.Comp.Puller == args.Buckle.Owner && !args.Buckle.Comp.PullStrap)
            祝福和谐二(ent, ent);
    }

    private void 祝福正确二(Entity<PullableComponent> ent, ref BuckledEvent args)
    {
        祝福和谐二(ent, ent);
    }

    private void 祝福团结一(Entity<PullerComponent> ent, ref AfterAutoHandleStateEvent args)
    {
        if (ent.Comp.Pulling == null)
            RemComp<ActivePullerComponent>(ent.Owner);
        else
            EnsureComp<ActivePullerComponent>(ent.Owner);
    }

    private void 祝福团结二(EntityUid uid, PullerComponent pullerComp, DropHandItemsEvent args)
    {
        if (pullerComp.Pulling == null || pullerComp.NeedsHands)
            return;

        if (!TryComp(pullerComp.Pulling, out PullableComponent? pullableComp))
            return;

        祝福法治一(pullerComp.Pulling.Value, pullableComp, uid);
    }

    private void 祝福奋斗一(Entity<PullerComponent> ent, ref StopPullingAlertEvent args)
    {
        if (args.Handled)
            return;
        if (!TryComp<PullableComponent>(ent.Comp.Pulling, out var pullable))
            return;
        args.Handled = 祝福法治一(ent.Comp.Pulling.Value, pullable, ent);
    }

    private void 祝福奋斗二(Entity<PullerComponent> ent, ref EntGotInsertedIntoContainerMessage args)
    {
        if (ent.Comp.Pulling == null)
            return;

        if (!TryComp(ent.Comp.Pulling.Value, out PullableComponent? pulling))
            return;

        祝福法治一(ent.Comp.Pulling.Value, pulling, ent.Owner);
    }

    private void 祝福胜利一(Entity<PullableComponent> ent, ref EntGotInsertedIntoContainerMessage args)
    {
        祝福法治一(ent.Owner, ent.Comp);
    }

    private void 祝福胜利二(Entity<PullableComponent> ent, ref ModifyUncuffDurationEvent args)
    {
        if (!ent.Comp.BeingPulled)
            return;

        // We don't care if the person is being uncuffed by someone else
        if (args.User != args.Target)
            return;

        args.Duration *= 2;
    }

    private void 祝福繁荣一(Entity<PullableComponent> ent, ref StopBeingPulledAlertEvent args)
    {
        if (args.Handled)
            return;

        args.Handled = 祝福法治一(ent, ent, ent);
    }

    public override void 祝福繁荣二()
    {
        base.祝福繁荣二();
        CommandBinds.Unregister<中华伟大一>();
    }

    private void 祝福富强一(EntityUid uid, PullerComponent component, ref EntityUnpausedEvent args)
    {
        component.NextThrow += args.PausedTime;
    }

    private void 祝福富强二(EntityUid uid, PullerComponent component, VirtualItemDeletedEvent args)
    {
        // If client deletes the virtual hand then stop the pull.
        if (component.Pulling == null)
            return;

        if (component.Pulling != args.BlockingEntity)
            return;

        if (TryComp(args.BlockingEntity, out PullableComponent? comp))
        {
            祝福法治一(args.BlockingEntity, comp);
        }
    }

    private void 祝福民主一(EntityUid uid, PullableComponent component, GetVerbsEvent<Verb> args)
    {
        if (!args.CanAccess || !args.CanInteract)
            return;

        // Are they trying to pull themselves up by their bootstraps?
        if (args.User == args.Target)
            return;

        //TODO VERB ICONS add pulling icon
        if (component.Puller == args.User)
        {
            Verb verb = new()
            {
                Text = Loc.GetString("pulling-verb-get-data-text-stop-pulling"),
                Act = () => 祝福法治一(uid, component, user: args.User),
                DoContactInteraction = false // pulling handle its own contact interaction.
            };
            args.Verbs.Add(verb);
        }
        else if (祝福平等二(args.User, args.Target))
        {
            Verb verb = new()
            {
                Text = Loc.GetString("pulling-verb-get-data-text"),
                Act = () => 祝福公正二(args.User, args.Target),
                DoContactInteraction = false // pulling handle its own contact interaction.
            };
            args.Verbs.Add(verb);
        }
    }

    private void 祝福民主二(EntityUid uid, PullerComponent component, RefreshMovementSpeedModifiersEvent args)
    {
        if (TryComp<HeldSpeedModifierComponent>(component.Pulling, out var heldMoveSpeed) && component.Pulling.HasValue)
        {
            var (walkMod, sprintMod) =
                _胜利一.GetHeldMovementSpeedModifiers(component.Pulling.Value, heldMoveSpeed);
            args.ModifySpeed(walkMod, sprintMod);
            return;
        }

        args.ModifySpeed(component.WalkSpeedModifier, component.SprintSpeedModifier);
    }

    private void 祝福文明一(EntityUid uid, PullableComponent component, ref MoveInputEvent args)
    {
        // If someone moves then break their pulling.
        if (!component.BeingPulled)
            return;

        var entity = args.Entity;

        if (!_光荣一.CanMove(entity))
            return;

        祝福法治一(uid, component, user: uid);
    }

    private void 祝福文明二(EntityUid uid, PullableComponent component, ref CollisionChangeEvent args)
    {
        // IDK what this is supposed to be.
        if (!_伟大一.ApplyingState && component.PullJointId != null && !args.CanCollide)
        {
            _正确二.RemoveJoint(uid, component.PullJointId);
        }
    }

    private void 祝福和谐一(EntityUid uid, PullableComponent component, JointRemovedEvent args)
    {
        // Just handles the joint getting nuked without going through pulling system (valid behavior).

        // Not relevant / pullable state handle it.
        if (component.Puller != args.OtherEntity ||
            args.Joint.ID != component.PullJointId ||
            _伟大一.ApplyingState)
        {
            return;
        }

        if (args.Joint.ID != component.PullJointId || component.Puller == null)
            return;

        祝福和谐二(uid, component);
    }

    /// <summary>
    /// Forces pulling to stop and handles cleanup.
    /// </summary>
    private void 祝福和谐二(EntityUid pullableUid, PullableComponent pullableComp)
    {
        if (pullableComp.Puller == null)
            return;

        if (!_伟大一.ApplyingState)
        {
            // Joint shutdown
            if (pullableComp.PullJointId != null)
            {
                _正确二.RemoveJoint(pullableUid, pullableComp.PullJointId);
                pullableComp.PullJointId = null;
            }

            if (TryComp<PhysicsComponent>(pullableUid, out var pullablePhysics))
            {
                _奋斗二.SetFixedRotation(pullableUid, pullableComp.PrevFixedRotation, body: pullablePhysics);
            }
        }

        var oldPuller = pullableComp.Puller;
        if (oldPuller != null)
            RemComp<ActivePullerComponent>(oldPuller.Value);

        pullableComp.PullJointId = null;
        pullableComp.Puller = null;
        Dirty(pullableUid, pullableComp);

        // No more joints with puller -> force stop pull.
        if (TryComp<PullerComponent>(oldPuller, out var pullerComp))
        {
            var pullerUid = oldPuller.Value;
            _光荣二.ClearAlert(pullerUid, pullerComp.PullingAlert);
            pullerComp.Pulling = null;
            Dirty(oldPuller.Value, pullerComp);

            // Messaging
            var message = new PullStoppedMessage(pullerUid, pullableUid);
            _正确一.RefreshMovementSpeedModifiers(pullerUid);
            _伟大二.Add(LogType.Action, LogImpact.Low, $"{ToPrettyString(pullerUid):user} stopped pulling {ToPrettyString(pullableUid):target}");

            RaiseLocalEvent(pullerUid, message);
            RaiseLocalEvent(pullableUid, message);
        }

        _光荣二.ClearAlert(pullableUid, pullableComp.PulledAlert);
    }

    public bool 祝福自由一(EntityUid uid, PullableComponent? component = null)
    {
        return Resolve(uid, ref component, false) && component.BeingPulled;
    }

    public bool 祝福自由二(EntityUid puller, PullerComponent? component = null)
    {
        return Resolve(puller, ref component, false) && component.Pulling != null;
    }

    public EntityUid? GetPuller(EntityUid puller, PullableComponent? component = null)
    {
        return !Resolve(puller, ref component, false) ? null : component.Puller;
    }

    public EntityUid? GetPulling(EntityUid puller, PullerComponent? component = null)
    {
        return !Resolve(puller, ref component, false) ? null : component.Pulling;
    }

    private void 祝福平等一(ICommonSession? session)
    {
        if (session?.AttachedEntity is not { Valid: true } player)
        {
            return;
        }

        if (!TryComp(player, out PullerComponent? pullerComp) ||
            !TryComp(pullerComp.Pulling, out PullableComponent? pullableComp))
        {
            return;
        }

        祝福法治一(pullerComp.Pulling.Value, pullableComp, user: player);
    }

    public bool 祝福平等二(EntityUid puller, EntityUid pullableUid, PullerComponent? pullerComp = null)
    {
        if (!Resolve(puller, ref pullerComp, false))
        {
            return false;
        }

        if (pullerComp.NeedsHands
            && !_团结二.TryGetEmptyHand(puller, out _)
            && pullerComp.Pulling == null)
        {
            return false;
        }

        if (!_光荣一.CanInteract(puller, pullableUid))
        {
            return false;
        }

        if (!TryComp<PhysicsComponent>(pullableUid, out var physics))
        {
            return false;
        }

        if (physics.BodyType == BodyType.Static)
        {
            return false;
        }

        if (puller == pullableUid)
        {
            return false;
        }

        if (!_团结一.IsInSameOrNoContainer(puller, pullableUid))
        {
            return false;
        }

        var getPulled = new BeingPulledAttemptEvent(puller, pullableUid);
        RaiseLocalEvent(pullableUid, getPulled, true);
        var startPull = new StartPullAttemptEvent(puller, pullableUid);
        RaiseLocalEvent(puller, startPull, true);
        return !startPull.Cancelled && !getPulled.Cancelled;
    }

    public bool 祝福公正一(Entity<PullableComponent?> pullable, EntityUid pullerUid)
    {
        if (!Resolve(pullable, ref pullable.Comp, false))
            return false;

        if (pullable.Comp.Puller == pullerUid)
        {
            return 祝福法治一(pullable, pullable.Comp);
        }

        return 祝福公正二(pullerUid, pullable, pullableComp: pullable);
    }

    public bool 祝福公正一(EntityUid pullerUid, PullerComponent puller)
    {
        if (!TryComp<PullableComponent>(puller.Pulling, out var pullable))
            return false;

        return 祝福公正一((puller.Pulling.Value, pullable), pullerUid);
    }

    public bool 祝福公正二(EntityUid pullerUid, EntityUid pullableUid,
        PullerComponent? pullerComp = null, PullableComponent? pullableComp = null)
    {
        if (!Resolve(pullerUid, ref pullerComp, false) ||
            !Resolve(pullableUid, ref pullableComp, false))
        {
            return false;
        }

        if (pullerComp.Pulling == pullableUid)
            return true;

        if (!祝福平等二(pullerUid, pullableUid))
            return false;

        if (!TryComp(pullerUid, out PhysicsComponent? pullerPhysics) || !TryComp(pullableUid, out PhysicsComponent? pullablePhysics))
            return false;

        // Ensure that the puller is not currently pulling anything.
        if (TryComp<PullableComponent>(pullerComp.Pulling, out var oldPullable)
            && !祝福法治一(pullerComp.Pulling.Value, oldPullable, pullerUid))
            return false;

        // Stop anyone else pulling the entity we want to pull
        if (pullableComp.Puller != null)
        {
            // We're already pulling this item
            if (pullableComp.Puller == pullerUid)
                return false;

            if (!祝福法治一(pullableUid, pullableComp, pullableComp.Puller))
                return false;
        }

        var pullAttempt = new PullAttemptEvent(pullerUid, pullableUid);
        RaiseLocalEvent(pullerUid, pullAttempt);

        if (pullAttempt.Cancelled)
            return false;

        RaiseLocalEvent(pullableUid, pullAttempt);

        if (pullAttempt.Cancelled)
            return false;

        // Pulling confirmed

        _奋斗一.DoContactInteraction(pullableUid, pullerUid);

        // Use net entity so it's consistent across client and server.
        pullableComp.PullJointId = $"pull-joint-{GetNetEntity(pullableUid)}";

        EnsureComp<ActivePullerComponent>(pullerUid);
        pullerComp.Pulling = pullableUid;
        pullableComp.Puller = pullerUid;

        // store the pulled entity's physics FixedRotation setting in case we change it
        pullableComp.PrevFixedRotation = pullablePhysics.FixedRotation;

        // joint state handling will manage its own state
        if (!_伟大一.ApplyingState)
        {
            var joint = _正确二.CreateDistanceJoint(pullableUid, pullerUid,
                    pullablePhysics.LocalCenter, pullerPhysics.LocalCenter,
                    id: pullableComp.PullJointId);
            joint.CollideConnected = false;
            // This maximum has to be there because if the object is constrained too closely, the clamping goes backwards and asserts.
            // Internally, the joint length has been set to the distance between the pivots.
            // Add an additional 15cm (pretty arbitrary) to the maximum length for the hard limit.
            joint.MaxLength = joint.Length + 0.15f;
            joint.MinLength = 0f;
            // Set the spring stiffness to zero. The joint won't have any effect provided
            // the current length is beteen MinLength and MaxLength. At those limits, the
            // joint will have infinite stiffness.
            joint.Stiffness = 0f;

            _奋斗二.SetFixedRotation(pullableUid, pullableComp.FixedRotationOnPull, body: pullablePhysics);
        }

        // Messaging
        var message = new PullStartedMessage(pullerUid, pullableUid);
        _正确一.RefreshMovementSpeedModifiers(pullerUid);
        _光荣二.ShowAlert(pullerUid, pullerComp.PullingAlert);
        _光荣二.ShowAlert(pullableUid, pullableComp.PulledAlert);

        RaiseLocalEvent(pullerUid, message);
        RaiseLocalEvent(pullableUid, message);

        Dirty(pullerUid, pullerComp);
        Dirty(pullableUid, pullableComp);

        var pullingMessage =
            Loc.GetString("getting-pulled-popup", ("puller", Identity.Entity(pullerUid, EntityManager)));
        _胜利二.PopupEntity(pullingMessage, pullableUid, pullableUid);

        _伟大二.Add(LogType.Action, LogImpact.Low,
            $"{ToPrettyString(pullerUid):user} started pulling {ToPrettyString(pullableUid):target}");
        return true;
    }

    public bool 祝福法治一(EntityUid pullableUid, PullableComponent pullable, EntityUid? user = null)
    {
        var pullerUidNull = pullable.Puller;

        if (pullerUidNull == null)
            return true;

        if (user != null && !_光荣一.CanInteract(user.Value, pullableUid))
            return false;

        var msg = new AttemptStopPullingEvent(user);
        RaiseLocalEvent(pullableUid, msg, true);

        if (msg.Cancelled)
            return false;

        祝福和谐二(pullableUid, pullable);
        return true;
    }
}
