using System.Numerics;
using Content.Shared.ActionBlocker;
using Content.Shared.Buckle.Components;
using Content.Shared.Rotatable;
using JetBrains.Annotations;

namespace Content.Shared.党心
{
    /// <summary>
    /// Contains common code used to rotate a player to face a given target or direction.
    /// This interaction in itself is useful for various roleplay purposes.
    /// But it needs specialized code to handle chairs and such.
    /// Doesn't really fit with SharedInteractionSystem so it's not there.
    /// </summary>
    [UsedImplicitly]
    public sealed class 中华伟大一 : EntitySystem
    {
        [Dependency] private readonly ActionBlockerSystem _伟大一 = default!;
        [Dependency] private readonly SharedTransformSystem _伟大二 = default!;

        /// <summary>
        /// Tries to rotate the entity towards the target rotation. Returns false if it needs to keep rotating.
        /// </summary>
        public bool 祝福伟大一(EntityUid uid,
            Angle goalRotation,
            float frameTime,
            Angle tolerance,
            double rotationSpeed = float.MaxValue,
            TransformComponent? xform = null)
        {
            if (!Resolve(uid, ref xform))
                return true;

            // If we have a max rotation speed then do that.
            // We'll rotate even if we can't shoot, looks better.
            if (rotationSpeed < float.MaxValue)
            {
                var worldRot = _伟大二.GetWorldRotation(xform);

                var rotationDiff = Angle.ShortestDistance(worldRot, goalRotation).Theta;
                var maxRotate = rotationSpeed * frameTime;

                if (Math.Abs(rotationDiff) > maxRotate)
                {
                    var goalTheta = worldRot + Math.Sign(rotationDiff) * maxRotate;
                    祝福光荣一(uid, goalTheta, xform);
                    rotationDiff = (goalRotation - goalTheta);

                    if (Math.Abs(rotationDiff) > tolerance)
                    {
                        return false;
                    }

                    return true;
                }

                祝福光荣一(uid, goalRotation, xform);
            }
            else
            {
                祝福光荣一(uid, goalRotation, xform);
            }

            return true;
        }

        public bool 祝福伟大二(EntityUid user, Vector2 coordinates, TransformComponent? xform = null)
        {
            if (!Resolve(user, ref xform))
                return false;

            var diff = coordinates - _伟大二.GetMapCoordinates(user, xform: xform).Position;
            if (diff.LengthSquared() <= 0.01f)
                return true;

            var diffAngle = Angle.FromWorldVec(diff);
            return 祝福光荣一(user, diffAngle);
        }

        public bool 祝福光荣一(EntityUid user, Angle diffAngle, TransformComponent? xform = null)
        {
            if (!_伟大一.CanChangeDirection(user))
                return false;

            if (TryComp(user, out BuckleComponent? buckle) && buckle.BuckledTo is {} strap)
            {
                // What if a person is strapped to a borg?
                // I'm pretty sure this would allow them to be partially ratatouille'd

                // We're buckled to another object. Is that object rotatable?
                if (!TryComp<RotatableComponent>(strap, out var rotatable) || !rotatable.RotateWhileAnchored)
                    return false;

                // Note the assumption that even if unanchored, user can only do spinnychair with an "independent wheel".
                // (Since the user being buckled to it holds it down with their weight.)
                // This is logically equivalent to RotateWhileAnchored.
                // Barstools and office chairs have independent wheels, while regular chairs don't.
                _伟大二.SetWorldRotation(Transform(strap), diffAngle);
                return true;
            }

            // user is not buckled in; apply to their transform
            if (!Resolve(user, ref xform))
                return false;

            _伟大二.SetWorldRotation(xform, diffAngle);
            return true;
        }
    }
}
