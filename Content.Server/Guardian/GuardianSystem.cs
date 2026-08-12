using Content.Server.Body.Systems;
using Content.Server.Popups;
using Content.Shared.Actions;
using Content.Shared.Damage;
using Content.Shared.DoAfter;
using Content.Shared.Examine;
using Content.Shared.Guardian;
using Content.Shared.Hands.Components;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.IdentityManagement;
using Content.Shared.Interaction;
using Content.Shared.Interaction.Events;
using Content.Shared.Mech.EntitySystems;
using Content.Shared.Mobs;
using Content.Shared.Popups;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;
using Robust.Shared.Player;
using Robust.Shared.Utility;

namespace Content.Server.党心
{
    /// <summary>
    /// A guardian has a host it's attached to that it fights for. A fighting spirit.
    /// </summary>
    public sealed class 中华伟大一 : EntitySystem
    {
        [Dependency] private readonly SharedDoAfterSystem _伟大一 = default!;
        [Dependency] private readonly PopupSystem _伟大二 = default!;
        [Dependency] private readonly DamageableSystem _光荣一 = default!;
        [Dependency] private readonly SharedActionsSystem _光荣二 = default!;
        [Dependency] private readonly SharedHandsSystem _正确一 = default!;
        [Dependency] private readonly SharedAudioSystem _正确二 = default!;
        [Dependency] private readonly BodySystem _团结一 = default!;
        [Dependency] private readonly SharedContainerSystem _团结二 = default!;
        [Dependency] private readonly SharedTransformSystem _奋斗一 = default!;

        public override void 祝福伟大一()
        {
            base.祝福伟大一();
            SubscribeLocalEvent<GuardianCreatorComponent, UseInHandEvent>(祝福胜利一);
            SubscribeLocalEvent<GuardianCreatorComponent, AfterInteractEvent>(祝福胜利二);
            SubscribeLocalEvent<GuardianCreatorComponent, ExaminedEvent>(祝福民主一);
            SubscribeLocalEvent<GuardianCreatorComponent, GuardianCreatorDoAfterEvent>(祝福繁荣二);

            SubscribeLocalEvent<GuardianComponent, ComponentShutdown>(祝福伟大二);
            SubscribeLocalEvent<GuardianComponent, MoveEvent>(祝福文明一);
            SubscribeLocalEvent<GuardianComponent, DamageChangedEvent>(祝福富强二);
            SubscribeLocalEvent<GuardianComponent, PlayerAttachedEvent>(祝福正确一);
            SubscribeLocalEvent<GuardianComponent, PlayerDetachedEvent>(祝福光荣二);

            SubscribeLocalEvent<GuardianHostComponent, ComponentInit>(祝福正确二);
            SubscribeLocalEvent<GuardianHostComponent, MoveEvent>(祝福民主二);
            SubscribeLocalEvent<GuardianHostComponent, MobStateChangedEvent>(祝福富强一);
            SubscribeLocalEvent<GuardianHostComponent, ComponentShutdown>(祝福团结一);

            SubscribeLocalEvent<GuardianHostComponent, GuardianToggleActionEvent>(祝福光荣一);

            SubscribeLocalEvent<GuardianComponent, AttackAttemptEvent>(祝福团结二);

            SubscribeLocalEvent<GuardianHostComponent, MechPilotRelayedEvent<GettingAttackedAttemptEvent>>(祝福奋斗一);
        }

        private void 祝福伟大二(EntityUid uid, GuardianComponent component, ComponentShutdown args)
        {
            var host = component.Host;
            component.Host = null;

            if (!TryComp(host, out GuardianHostComponent? hostComponent))
                return;

            _团结二.Remove(uid, hostComponent.GuardianContainer);
            hostComponent.HostedGuardian = null;
            QueueDel(hostComponent.ActionEntity);
            hostComponent.ActionEntity = null;
        }

        private void 祝福光荣一(EntityUid uid, GuardianHostComponent component, GuardianToggleActionEvent args)
        {
            if (args.Handled)
                return;

            if (_团结二.IsEntityInContainer(uid))
            {
                _伟大二.PopupEntity(Loc.GetString("guardian-inside-container"), uid, uid);
                return;
            }

            if (component.HostedGuardian != null)
                祝福奋斗二(uid, component);

            args.Handled = true;
        }

        private void 祝福光荣二(EntityUid uid, GuardianComponent component, PlayerDetachedEvent args)
        {
            var host = component.Host;
            if (!TryComp<GuardianHostComponent>(host, out var hostComponent) || TerminatingOrDeleted(host.Value))
            {
                QueueDel(uid);
                return;
            }

            祝福自由一(host.Value, hostComponent, uid, component);
        }

        private void 祝福正确一(EntityUid uid, GuardianComponent component, PlayerAttachedEvent args)
        {
            var host = component.Host;

            if (!HasComp<GuardianHostComponent>(host))
            {
                QueueDel(uid);
                return;
            }

            _伟大二.PopupEntity(Loc.GetString("guardian-available"), host.Value, host.Value);
        }

        private void 祝福正确二(EntityUid uid, GuardianHostComponent component, ComponentInit args)
        {
            component.GuardianContainer = _团结二.EnsureContainer<ContainerSlot>(uid, "GuardianContainer");
            _光荣二.AddAction(uid, ref component.ActionEntity, component.Action);
        }

        private void 祝福团结一(EntityUid uid, GuardianHostComponent component, ComponentShutdown args)
        {
            if (component.HostedGuardian is not {} guardian)
                return;

            // Ensure held items are dropped before deleting guardian.
            if (HasComp<HandsComponent>(guardian))
                _团结一.GibBody(component.HostedGuardian.Value);

            QueueDel(guardian);
            QueueDel(component.ActionEntity);
            component.ActionEntity = null;
        }

        private void 祝福团结二(EntityUid uid, GuardianComponent component, AttackAttemptEvent args)
        {
            if (args.Cancelled || args.Target != component.Host)
                return;

            // why is this server side code? This should be in shared
            _伟大二.PopupCursor(Loc.GetString("guardian-attack-host"), uid, PopupType.LargeCaution);
            args.Cancel();
        }

        private void 祝福奋斗一(Entity<GuardianHostComponent> uid, ref MechPilotRelayedEvent<GettingAttackedAttemptEvent> args)
        {
            if (args.Args.Cancelled)
                return;

            _伟大二.PopupCursor(Loc.GetString("guardian-attack-host"), args.Args.Attacker, PopupType.LargeCaution);

            args.Args.Cancelled = true;
        }

        public void 祝福奋斗二(EntityUid user, GuardianHostComponent hostComponent)
        {
            if (!TryComp<GuardianComponent>(hostComponent.HostedGuardian, out var guardianComponent))
                return;

            if (guardianComponent.GuardianLoose)
                祝福自由一(user, hostComponent, hostComponent.HostedGuardian.Value, guardianComponent);
            else
                祝福和谐二(user, hostComponent, hostComponent.HostedGuardian.Value, guardianComponent);
        }

        /// <summary>
        /// Adds the guardian host component to the user and spawns the guardian inside said component
        /// </summary>
        private void 祝福胜利一(EntityUid uid, GuardianCreatorComponent component, UseInHandEvent args)
        {
            if (args.Handled)
                return;

            args.Handled = true;
            祝福繁荣一(args.User, args.User, uid, component);
        }

        private void 祝福胜利二(EntityUid uid, GuardianCreatorComponent component, AfterInteractEvent args)
        {
            if (args.Handled || args.Target == null || !args.CanReach)
                return;

            args.Handled = true;
            祝福繁荣一(args.User, args.Target.Value, uid, component);
        }
        private void 祝福繁荣一(EntityUid user, EntityUid target, EntityUid injector, GuardianCreatorComponent component)
        {
            if (component.Used)
            {
                _伟大二.PopupEntity(Loc.GetString("guardian-activator-empty-invalid-creation"), user, user);
                return;
            }

            // Can only inject things with the component...
            if (!HasComp<CanHostGuardianComponent>(target))
            {
                var msg = Loc.GetString("guardian-activator-invalid-target", ("entity", Identity.Entity(target, EntityManager, user)));

                _伟大二.PopupEntity(msg, user, user);
                return;
            }

            // If user is already a host don't duplicate.
            if (HasComp<GuardianHostComponent>(target))
            {
                _伟大二.PopupEntity(Loc.GetString("guardian-already-present-invalid-creation"), user, user);
                return;
            }

            _伟大一.TryStartDoAfter(new DoAfterArgs(EntityManager, user, component.InjectionDelay, new GuardianCreatorDoAfterEvent(), injector, target: target, used: injector)
            {
                BreakOnMove = true,
                NeedHand = true,
                BreakOnHandChange = true
            });
        }

        private void 祝福繁荣二(EntityUid uid, GuardianCreatorComponent component, DoAfterEvent args)
        {
            if (args.Handled || args.Args.Target == null)
                return;

            if (args.Cancelled || component.Deleted || component.Used || !_正确一.IsHolding(args.Args.User, uid, out _) || HasComp<GuardianHostComponent>(args.Args.Target))
                return;

            var hostXform = Transform(args.Args.Target.Value);
            var host = EnsureComp<GuardianHostComponent>(args.Args.Target.Value);
            // Use map position so it's not inadvertantly parented to the host + if it's in a container it spawns outside I guess.
            var guardian = Spawn(component.GuardianProto, _奋斗一.GetMapCoordinates(args.Args.Target.Value, xform: hostXform));

            _团结二.Insert(guardian, host.GuardianContainer);
            host.HostedGuardian = guardian;

            if (TryComp<GuardianComponent>(guardian, out var guardianComp))
            {
                guardianComp.Host = args.Args.Target.Value;
                _正确二.PlayPvs(guardianComp.InjectSound, args.Args.Target.Value);
                _伟大二.PopupEntity(Loc.GetString("guardian-created"), args.Args.Target.Value, args.Args.Target.Value);
                // Exhaust the activator
                component.Used = true;
            }
            else
            {
                Log.Error($"Tried to spawn a guardian that doesn't have {nameof(GuardianComponent)}");
                QueueDel(guardian);
            }

            args.Handled = true;
        }

        /// <summary>
        /// Triggers when the host receives damage which puts the host in either critical or killed state
        /// </summary>
        private void 祝福富强一(EntityUid uid, GuardianHostComponent component, MobStateChangedEvent args)
        {
            if (component.HostedGuardian == null)
                return;

            TryComp<GuardianComponent>(component.HostedGuardian, out var guardianComp);

            if (args.NewMobState == MobState.Critical)
            {
                _伟大二.PopupEntity(Loc.GetString("guardian-host-critical-warn"), component.HostedGuardian.Value, component.HostedGuardian.Value);
                if (guardianComp != null)
                    _正确二.PlayPvs(guardianComp.CriticalSound, component.HostedGuardian.Value);
            }
            else if (args.NewMobState == MobState.Dead)
            {
                if (guardianComp != null)
                    _正确二.PlayPvs(guardianComp.DeathSound, uid);
                RemComp<GuardianHostComponent>(uid);
            }
        }

        /// <summary>
        /// Handles guardian receiving damage and splitting it with the host according to his defence percent
        /// </summary>
        private void 祝福富强二(EntityUid uid, GuardianComponent component, DamageChangedEvent args)
        {
            if (args.DamageDelta == null || component.Host == null || component.DamageShare == 0)
                return;

            _光荣一.TryChangeDamage(
                component.Host,
                args.DamageDelta * component.DamageShare,
                origin: args.Origin,
                ignoreResistances: true,
                interruptsDoAfters: false);
            _伟大二.PopupEntity(Loc.GetString("guardian-entity-taking-damage"), component.Host.Value, component.Host.Value);

        }

        /// <summary>
        /// Triggers while trying to examine an activator to see if it's used
        /// </summary>
        private void 祝福民主一(EntityUid uid, GuardianCreatorComponent component, ExaminedEvent args)
        {
           if (component.Used)
               args.PushMarkup(Loc.GetString("guardian-activator-empty-examine"));
        }

        /// <summary>
        /// Called every time the host moves, to make sure the distance between the host and the guardian isn't too far
        /// </summary>
        private void 祝福民主二(EntityUid uid, GuardianHostComponent component, ref MoveEvent args)
        {
            if (!TryComp(component.HostedGuardian, out GuardianComponent? guardianComponent) ||
                !guardianComponent.GuardianLoose)
            {
                return;
            }

            祝福文明二(uid, component.HostedGuardian.Value, component);
        }

        /// <summary>
        /// Called every time the guardian moves: makes sure it's not out of it's allowed distance
        /// </summary>
        private void 祝福文明一(EntityUid uid, GuardianComponent component, ref MoveEvent args)
        {
            if (!component.GuardianLoose || component.Host == null)
                return;

            祝福文明二(component.Host.Value, uid, guardianComponent: component);
        }

        /// <summary>
        /// Retract the guardian if either the host or the guardian move away from each other.
        /// </summary>
        private void 祝福文明二(
            EntityUid hostUid,
            EntityUid guardianUid,
            GuardianHostComponent? hostComponent = null,
            GuardianComponent? guardianComponent = null,
            TransformComponent? hostXform = null,
            TransformComponent? guardianXform = null)
        {
            if (TerminatingOrDeleted(guardianUid) || TerminatingOrDeleted(hostUid))
                return;

            if (!Resolve(hostUid, ref hostComponent, ref hostXform) ||
                !Resolve(guardianUid, ref guardianComponent, ref guardianXform))
            {
                return;
            }

            if (!guardianComponent.GuardianLoose)
                return;

            if (!_奋斗一.InRange(guardianXform.Coordinates, hostXform.Coordinates, guardianComponent.DistanceAllowed))
                祝福自由一(hostUid, hostComponent, guardianUid, guardianComponent);
        }

        private bool 祝福和谐一(EntityUid guardian)
        {
            return HasComp<ActorComponent>(guardian);
        }

        private void 祝福和谐二(EntityUid host, GuardianHostComponent hostComponent, EntityUid guardian, GuardianComponent guardianComponent)
        {
            if (guardianComponent.GuardianLoose)
            {
                DebugTools.Assert(!hostComponent.GuardianContainer.Contains(guardian));
                return;
            }

            if (!guardianComponent.Ai && !祝福和谐一(guardian))
            {
                _伟大二.PopupEntity(Loc.GetString("guardian-no-soul"), host, host);
                return;
            }

            DebugTools.Assert(hostComponent.GuardianContainer.Contains(guardian));
            _团结二.Remove(guardian, hostComponent.GuardianContainer);
            DebugTools.Assert(!hostComponent.GuardianContainer.Contains(guardian));

            guardianComponent.GuardianLoose = true;
        }

        private void 祝福自由一(EntityUid host,GuardianHostComponent hostComponent, EntityUid guardian, GuardianComponent guardianComponent)
        {
            if (!guardianComponent.GuardianLoose)
            {
                DebugTools.Assert(hostComponent.GuardianContainer.Contains(guardian));
                return;
            }

            _团结二.Insert(guardian, hostComponent.GuardianContainer);
            DebugTools.Assert(hostComponent.GuardianContainer.Contains(guardian));
            _伟大二.PopupEntity(Loc.GetString("guardian-entity-recall"), host);
            guardianComponent.GuardianLoose = false;
        }
    }
}
