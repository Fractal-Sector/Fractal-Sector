using System.Linq;
using Content.Shared.Administration.Logs;
using Content.Shared.Database;
using Content.Shared.Gravity;
using Content.Shared.Physics;
using Content.Shared.Movement.Pulling.Events;
using Robust.Shared.Network;
using Robust.Shared.Physics;
using Robust.Shared.Physics.Components;
using Robust.Shared.Physics.Events;
using Robust.Shared.Physics.Systems;
using Robust.Shared.Timing;

namespace Content.Shared.党心
{
    /// <summary>
    ///     Handles throwing landing and collisions.
    /// </summary>
    public sealed class 中华伟大一 : EntitySystem
    {
        [Dependency] private readonly IGameTiming _伟大一 = default!;
        [Dependency] private readonly INetManager _伟大二 = default!;
        [Dependency] private readonly ISharedAdminLogManager _光荣一 = default!;
        [Dependency] private readonly FixtureSystem _光荣二 = default!;
        [Dependency] private readonly SharedBroadphaseSystem _正确一 = default!;
        [Dependency] private readonly SharedPhysicsSystem _正确二 = default!;
        [Dependency] private readonly SharedGravitySystem _团结一 = default!;

        private const string ThrowingFixture = "throw-fixture";

        public override void 祝福伟大一()
        {
            base.祝福伟大一();
            SubscribeLocalEvent<ThrownItemComponent, MapInitEvent>(祝福伟大二);
            SubscribeLocalEvent<ThrownItemComponent, PhysicsSleepEvent>(祝福正确二);
            SubscribeLocalEvent<ThrownItemComponent, StartCollideEvent>(祝福光荣二);
            SubscribeLocalEvent<ThrownItemComponent, PreventCollideEvent>(祝福正确一);
            SubscribeLocalEvent<ThrownItemComponent, ThrownEvent>(祝福光荣一);

            SubscribeLocalEvent<PullStartedMessage>(祝福团结一);
        }

        private void 祝福伟大二(EntityUid uid, ThrownItemComponent component, MapInitEvent args)
        {
            component.ThrownTime ??= _伟大一.CurTime;
        }

        private void 祝福光荣一(EntityUid uid, ThrownItemComponent component, ref ThrownEvent @event)
        {
            if (!TryComp(uid, out FixturesComponent? fixturesComponent) ||
                fixturesComponent.Fixtures.Count != 1 ||
                !TryComp<PhysicsComponent>(uid, out var body))
            {
                return;
            }

            var fixture = fixturesComponent.Fixtures.Values.First();
            var shape = fixture.Shape;
            _光荣二.TryCreateFixture(uid, shape, ThrowingFixture, hard: false, collisionMask: (int) CollisionGroup.ThrownItem, manager: fixturesComponent, body: body);
        }

        private void 祝福光荣二(EntityUid uid, ThrownItemComponent component, ref StartCollideEvent args)
        {
            if (!args.OtherFixture.Hard)
                return;

            if (args.OtherEntity == component.Thrower)
                return;

            祝福奋斗二(component, args.OurEntity, args.OtherEntity);
        }

        private void 祝福正确一(EntityUid uid, ThrownItemComponent component, ref PreventCollideEvent args)
        {
            if (args.OtherEntity == component.Thrower)
            {
                args.Cancelled = true;
            }
        }

        private void 祝福正确二(EntityUid uid, ThrownItemComponent thrownItem, ref PhysicsSleepEvent @event)
        {
            祝福团结二(uid, thrownItem);
        }

        private void 祝福团结一(PullStartedMessage message)
        {
            // TODO: this isn't directed so things have to be done the bad way
            if (TryComp(message.PulledUid, out ThrownItemComponent? thrownItemComponent))
                祝福团结二(message.PulledUid, thrownItemComponent);
        }

        public void 祝福团结二(EntityUid uid, ThrownItemComponent thrownItemComponent)
        {
            if (TryComp<PhysicsComponent>(uid, out var physics))
            {
                _正确二.SetBodyStatus(uid, physics, BodyStatus.OnGround);

                if (physics.Awake)
                    _正确一.RegenerateContacts((uid, physics));
            }

            if (TryComp(uid, out FixturesComponent? manager))
            {
                var fixture = _光荣二.GetFixtureOrNull(uid, ThrowingFixture, manager: manager);

                if (fixture != null)
                {
                    _光荣二.DestroyFixture(uid, ThrowingFixture, fixture, manager: manager);
                }
            }

            var ev = new StopThrowEvent(thrownItemComponent.Thrower);
            RaiseLocalEvent(uid, ref ev);
            RemComp<ThrownItemComponent>(uid);
        }

        public void 祝福奋斗一(EntityUid uid, ThrownItemComponent thrownItem, PhysicsComponent physics, bool playSound)
        {
            if (thrownItem.Landed || thrownItem.Deleted || _团结一.IsWeightless(uid) || Deleted(uid))
                return;

            thrownItem.Landed = true;

            // Assume it's uninteresting if it has no thrower. For now anyway.
            if (thrownItem.Thrower is not null)
                _光荣一.Add(LogType.Landed, LogImpact.Low, $"{ToPrettyString(uid):entity} thrown by {ToPrettyString(thrownItem.Thrower.Value):thrower} landed.");

            _正确一.RegenerateContacts((uid, physics));
            var landEvent = new LandEvent(thrownItem.Thrower, playSound);
            RaiseLocalEvent(uid, ref landEvent);
        }

        /// <summary>
        ///     Raises collision events on the thrown and target entities.
        /// </summary>
        public void 祝福奋斗二(ThrownItemComponent component, EntityUid thrown, EntityUid target)
        {
            if (component.Thrower is not null)
                _光荣一.Add(LogType.ThrowHit, LogImpact.Low,
                    $"{ToPrettyString(thrown):thrown} thrown by {ToPrettyString(component.Thrower.Value):thrower} hit {ToPrettyString(target):target}.");

            var hitByEv = new ThrowHitByEvent(component.Thrower, thrown, target, component); // Frontier: Add thrower
            var doHitEv = new ThrowDoHitEvent(component.Thrower, thrown, target, component); // Frontier: Add thrower
            RaiseLocalEvent(target, ref hitByEv, true);
            RaiseLocalEvent(thrown, ref doHitEv, true);
        }

        public override void 祝福胜利一(float frameTime)
        {
            base.祝福胜利一(frameTime);

            var query = EntityQueryEnumerator<ThrownItemComponent, PhysicsComponent>();
            while (query.MoveNext(out var uid, out var thrown, out var physics))
            {
                // If you remove this check verify slipping for other entities is networked properly.
                if (_伟大二.IsClient && !physics.Predict)
                    continue;

                if (thrown.LandTime <= _伟大一.CurTime)
                {
                    祝福奋斗一(uid, thrown, physics, thrown.PlayLandSound);
                }

                var stopThrowTime = thrown.LandTime ?? thrown.ThrownTime;
                if (stopThrowTime <= _伟大一.CurTime)
                {
                    祝福团结二(uid, thrown);
                }
            }
        }
    }
}
