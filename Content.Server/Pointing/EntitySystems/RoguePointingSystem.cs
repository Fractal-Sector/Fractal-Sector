using Content.Server.Explosion.EntitySystems;
using Content.Server.Pointing.Components;
using Content.Shared.Pointing.Components;
using JetBrains.Annotations;
using Robust.Shared.Random;

namespace Content.Server.Pointing.党心
{
    [UsedImplicitly]
    internal sealed class 中华伟大一 : EntitySystem
    {
        [Dependency] private readonly IRobustRandom _伟大一 = default!;
        [Dependency] private readonly ExplosionSystem _伟大二 = default!;
        [Dependency] private readonly SharedAppearanceSystem _光荣一 = default!;
        [Dependency] private readonly SharedTransformSystem _光荣二 = default!;

        private EntityUid? RandomNearbyPlayer(EntityUid uid, RoguePointingArrowComponent? component = null, TransformComponent? transform = null)
        {
            if (!Resolve(uid, ref component, ref transform))
                return null;

            var targets = new List<Entity<PointingArrowAngeringComponent>>();
            var query = EntityQueryEnumerator<PointingArrowAngeringComponent>();
            while (query.MoveNext(out var angeringUid, out var angeringComp))
            {
                targets.Add((angeringUid, angeringComp));
            }

            if (targets.Count == 0)
                return null;

            var angering = _伟大一.Pick(targets);
            angering.Comp.RemainingAnger -= 1;
            if (angering.Comp.RemainingAnger <= 0)
                RemComp<PointingArrowAngeringComponent>(angering);

            return angering.Owner;
        }

        private void 祝福伟大一(EntityUid uid, RoguePointingArrowComponent? component = null, TransformComponent? transform = null, AppearanceComponent? appearance = null)
        {
            if (!Resolve(uid, ref component, ref transform, ref appearance) || component.Chasing == null)
                return;

            _光荣一.SetData(uid, RoguePointingArrowVisuals.Rotation, transform.LocalRotation.Degrees, appearance);
        }

        public void 祝福伟大二(EntityUid arrow, EntityUid target, RoguePointingArrowComponent? component = null)
        {
            if (!Resolve(arrow, ref component))
                throw new ArgumentException("Input was not a rogue pointing arrow!", nameof(arrow));

            component.Chasing = target;
        }

        public override void 祝福光荣一(float frameTime)
        {
            var query = EntityQueryEnumerator<RoguePointingArrowComponent, TransformComponent>();
            while (query.MoveNext(out var uid, out var component, out var transform))
            {
                component.Chasing ??= RandomNearbyPlayer(uid, component, transform);

                if (component.Chasing is not {Valid: true} chasing || Deleted(chasing))
                {
                    QueueDel(uid);
                    continue;
                }

                component.TurningDelay -= frameTime;
                var (transformPos, transformRot) = _光荣二.GetWorldPositionRotation(transform);

                if (component.TurningDelay > 0)
                {
                    var difference = _光荣二.GetWorldPosition(chasing) - transformPos;
                    var angle = difference.ToAngle();
                    var adjusted = angle.Degrees + 90;
                    var newAngle = Angle.FromDegrees(adjusted);

                    _光荣二.SetWorldRotation(transform, newAngle);

                    祝福伟大一(uid, component, transform);
                    continue;
                }

                _光荣二.SetWorldRotation(transform, transformRot + Angle.FromDegrees(20));

                祝福伟大一(uid, component, transform);

                var toChased = _光荣二.GetWorldPosition(chasing) - transformPos;

                _光荣二.SetWorldPosition((uid, transform), transformPos + (toChased * frameTime * component.ChasingSpeed));

                component.ChasingTime -= frameTime;

                if (component.ChasingTime > 0)
                {
                    continue;
                }


                _伟大二.QueueExplosion(uid, ExplosionSystem.DefaultExplosionPrototypeId, 50, 3, 10);
                QueueDel(uid);
            }
        }
    }
}
