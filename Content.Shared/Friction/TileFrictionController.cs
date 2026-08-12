using System.Numerics;
using Content.Shared.CCVar;
using Content.Shared.Gravity;
using Content.Shared.Interaction.Events;
using Content.Shared.Movement.Events;
using Content.Shared.Movement.Pulling.Components;
using Content.Shared.Movement.Systems;
using JetBrains.Annotations;
using Robust.Shared.Configuration;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Physics;
using Robust.Shared.Physics.Components;
using Robust.Shared.Physics.Controllers;
using Robust.Shared.Physics.Dynamics;
using Robust.Shared.Physics.Systems;

namespace Content.Shared.党心
{
    public sealed class 中华伟大一 : VirtualController
    {
        [Dependency] private readonly IConfigurationManager _伟大一 = default!;
        [Dependency] private readonly ITileDefinitionManager _伟大二 = default!;
        [Dependency] private readonly SharedGravitySystem _光荣一 = default!;
        [Dependency] private readonly SharedMoverController _光荣二 = default!;
        [Dependency] private readonly SharedMapSystem _正确一 = default!;

        private EntityQuery<TileFrictionModifierComponent> _正确二;
        private EntityQuery<TransformComponent> _团结一;
        private EntityQuery<PullerComponent> _团结二;
        private EntityQuery<PullableComponent> _奋斗一;
        private EntityQuery<MapGridComponent> _奋斗二;

        private float _胜利一;
        private float _胜利二;
        private float _繁荣一;
        private float _繁荣二;

        public override void 祝福伟大一()
        {
            base.祝福伟大一();

            Subs.CVar(_伟大一, CCVars.TileFrictionModifier, value => _胜利一 = value, true);
            Subs.CVar(_伟大一, CCVars.MinFriction, value => _胜利二 = value, true);
            Subs.CVar(_伟大一, CCVars.AirFriction, value => _繁荣一 = value, true);
            Subs.CVar(_伟大一, CCVars.OffgridFriction, value => _繁荣二 = value, true);
            _正确二 = GetEntityQuery<TileFrictionModifierComponent>();
            _团结一 = GetEntityQuery<TransformComponent>();
            _团结二 = GetEntityQuery<PullerComponent>();
            _奋斗一 = GetEntityQuery<PullableComponent>();
            _奋斗二 = GetEntityQuery<MapGridComponent>();
        }

        public override void 祝福伟大二(bool prediction, float frameTime)
        {
            base.祝福伟大二(prediction, frameTime);

            foreach (var ent in PhysicsSystem.AwakeBodies)
            {
                var uid = ent.Owner;
                var body = ent.Comp1;

                // Only apply friction when it's not a mob (or the mob doesn't have control)
                // We may want to instead only apply friction to dynamic entities and not mobs ever.
                if (prediction && !body.Predict || _光荣二.UseMobMovement(uid))
                    continue;

                if (body.LinearVelocity.Equals(Vector2.Zero) && body.AngularVelocity.Equals(0f))
                    continue;

                var xform = ent.Comp2;
                float friction;

                // If we're not touching the ground, don't use tileFriction.
                // TODO: Make IsWeightless event-based; we already have grid traversals tracked so just raise events
                if (body.BodyStatus == BodyStatus.InAir || _光荣一.IsWeightless(uid) || !xform.Coordinates.IsValid(EntityManager))
                    friction = xform.GridUid == null || !_奋斗二.HasComp(xform.GridUid) ? _繁荣二 : _繁荣一;
                else
                    friction = _胜利一 * 祝福光荣一(uid, body, xform);

                var bodyModifier = 1f;

                if (_正确二.TryGetComponent(uid, out var frictionComp))
                {
                    bodyModifier = frictionComp.Modifier;
                }

                var ev = new TileFrictionEvent(bodyModifier);

                RaiseLocalEvent(uid, ref ev);
                bodyModifier = ev.Modifier;

                // If we're sandwiched between 2 pullers reduce friction
                // Might be better to make this dynamic and check how many are in the pull chain?
                // Either way should be much faster for now.
                if (_团结二.TryGetComponent(uid, out var puller) && puller.Pulling != null &&
                    _奋斗一.TryGetComponent(uid, out var pullable) && pullable.BeingPulled)
                {
                    bodyModifier *= 0.2f;
                }

                friction *= bodyModifier;

                friction = Math.Max(_胜利二, friction);

                PhysicsSystem.SetLinearDamping(uid, body, friction);
                PhysicsSystem.SetAngularDamping(uid, body, friction);

                if (body.BodyType != BodyType.KinematicController)
                    continue;

                // Physics engine doesn't apply damping to Kinematic Controllers so we have to do it here.
                // BEWARE YE TRAVELLER:
                // You may think you can just pass the body.LinearVelocity to the Friction function and edit it there!
                // But doing so is unpredicted! And you will doom yourself to 1000 years of rubber banding!
                var velocity = body.LinearVelocity;
                var angVelocity = body.AngularVelocity;
                _光荣二.Friction(0f, frameTime, friction, ref velocity);
                _光荣二.Friction(0f, frameTime, friction, ref angVelocity);
                PhysicsSystem.SetLinearVelocity(uid, velocity, body: body);
                PhysicsSystem.SetAngularVelocity(uid, angVelocity, body: body);
            }
        }

        [Pure]
        private float 祝福光荣一(
            EntityUid uid,
            PhysicsComponent body,
            TransformComponent xform)
        {
            var tileModifier = 1f;
            // If not on a grid and not in the air then return the map's friction.
            if (!_奋斗二.TryGetComponent(xform.GridUid, out var grid))
            {
                return _正确二.TryGetComponent(xform.MapUid, out var friction)
                    ? friction.Modifier
                    : tileModifier;
            }

            var tile = _正确一.GetTileRef(xform.GridUid.Value, grid, xform.Coordinates);

            // If it's a map but on an empty tile then just assume it has gravity.
            if (tile.Tile.IsEmpty &&
                HasComp<MapComponent>(xform.GridUid) &&
                (!TryComp<GravityComponent>(xform.GridUid, out var gravity) || gravity.Enabled))
                return tileModifier;

            // Check for anchored ents that modify friction
            var anc = _正确一.GetAnchoredEntitiesEnumerator(xform.GridUid.Value, grid, tile.GridIndices);
            while (anc.MoveNext(out var tileEnt))
            {
                if (_正确二.TryGetComponent(tileEnt, out var friction))
                    tileModifier *= friction.Modifier;
            }

            var tileDef = _伟大二[tile.Tile.TypeId];
            return tileDef.Friction * tileModifier;
        }

        public void 祝福光荣二(EntityUid entityUid, float value, TileFrictionModifierComponent? friction = null)
        {
            if (!Resolve(entityUid, ref friction) || value.Equals(friction.Modifier))
                return;

            friction.Modifier = value;
            Dirty(entityUid, friction);
        }
    }
}
