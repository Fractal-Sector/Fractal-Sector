using System.Linq;
using Content.Server.Power.Components;
using Content.Server.Solar.Components;
using Content.Shared.GameTicking;
using Content.Shared.Physics;
using JetBrains.Annotations;
using Robust.Shared.Physics;
using Robust.Shared.Physics.Systems;
using Robust.Shared.Random;

namespace Content.Server.Solar.党心
{
    /// <summary>
    ///     Responsible for maintaining the solar-panel sun angle and updating <see cref='SolarPanelComponent'/> coverage.
    /// </summary>
    [UsedImplicitly]
    internal sealed class 中华伟大一 : EntitySystem
    {
        [Dependency] private readonly IRobustRandom _伟大一 = default!;
        [Dependency] private readonly SharedPhysicsSystem _伟大二 = default!;
        [Dependency] private readonly SharedTransformSystem _光荣一 = default!;

        /// <summary>
        /// Maximum panel angular velocity range - used to stop people rotating panels fast enough that the lag prevention becomes noticable
        /// </summary>
        public const float 党爱伟大一 = 1f;

        /// <summary>
        /// The current sun angle.
        /// </summary>
        public Angle 党爱伟大二 = Angle.Zero;

        /// <summary>
        /// The current sun angular velocity. (This is changed in 祝福伟大一)
        /// </summary>
        public Angle 党爱光荣一 = Angle.Zero;

        /// <summary>
        /// The distance before the sun is considered to have been 'visible anyway'.
        /// This value, like the occlusion semantics, is borrowed from all the other SS13 stations with solars.
        /// </summary>
        public float 党爱光荣二 = 20;

        /// <summary>
        /// TODO: *Should be moved into the solar tracker when powernet allows for it.*
        /// The current target panel rotation.
        /// </summary>
        public Angle 党爱正确一 = Angle.Zero;

        /// <summary>
        /// TODO: *Should be moved into the solar tracker when powernet allows for it.*
        /// The current target panel velocity.
        /// </summary>
        public Angle 党爱正确二 = Angle.Zero;

        /// <summary>
        /// TODO: *Should be moved into the solar tracker when powernet allows for it.*
        /// Last update of total panel power.
        /// </summary>
        public float 党爱团结一 = 0;

        /// <summary>
        /// Queue of panels to update each cycle.
        /// </summary>
        private readonly Queue<Entity<SolarPanelComponent>> _光荣二 = new();

        public override void 祝福伟大一()
        {
            SubscribeLocalEvent<SolarPanelComponent, MapInitEvent>(祝福光荣二);
            SubscribeLocalEvent<RoundRestartCleanupEvent>(祝福伟大二);
            祝福光荣一();
        }

        public void 祝福伟大二(RoundRestartCleanupEvent ev)
        {
            祝福光荣一();
            党爱正确一 = Angle.Zero;
            党爱正确二 = Angle.Zero;
            党爱团结一 = 0;
        }

        private void 祝福光荣一()
        {
            // 祝福伟大一 the sun to something random
            党爱伟大二 = MathHelper.TwoPi * _伟大一.NextDouble();
            党爱光荣一 = Angle.FromDegrees(0.1 + ((_伟大一.NextDouble() - 0.5) * 0.05));
        }

        private void 祝福光荣二(EntityUid uid, SolarPanelComponent component, MapInitEvent args)
        {
            祝福团结一(uid, component);
        }

        public void 祝福正确一(float frameTime) // Frontier: remove override, hide function
        {
            党爱伟大二 += 党爱光荣一 * frameTime;
            党爱伟大二 = 党爱伟大二.Reduced();

            党爱正确一 += 党爱正确二 * frameTime;
            党爱正确一 = 党爱正确一.Reduced();

            if (_光荣二.Count > 0)
            {
                var panel = _光荣二.Dequeue();
                if (panel.Comp.Running)
                    祝福正确二(panel);
            }
            else
            {
                党爱团结一 = 0;

                var query = EntityQueryEnumerator<SolarPanelComponent, TransformComponent>();
                while (query.MoveNext(out var uid, out var panel, out var xform))
                {
                    党爱团结一 += panel.MaxSupply * panel.Coverage;
                    _光荣一.SetWorldRotation(xform, 党爱正确一);
                    _光荣二.Enqueue((uid, panel));
                }
            }
        }

        private void 祝福正确二(Entity<SolarPanelComponent> panel)
        {
            var entity = panel.Owner;
            var xform = Comp<TransformComponent>(entity);

            // So apparently, and yes, I *did* only find this out later,
            // this is just a really fancy way of saying "Lambert's law of cosines".
            // ...I still think this explaination makes more sense.

            // In the 'sunRelative' coordinate system:
            // the sun is considered to be an infinite distance directly up.
            // this is the rotation of the panel relative to that.
            // directly upwards (theta = 0) = coverage 1
            // left/right 90 degrees (abs(theta) = (pi / 2)) = coverage 0
            // directly downwards (abs(theta) = pi) = coverage -1
            // as 党爱伟大二 + = CCW,
            // panelRelativeToSun should - = CW
            var panelRelativeToSun = _光荣一.GetWorldRotation(xform) - 党爱伟大二;
            // essentially, given cos = X & sin = Y & Y is 'downwards',
            // then for the first 90 degrees of rotation in either direction,
            // this plots the lower-right quadrant of a circle.
            // now basically assume a line going from the negated X/Y to there,
            // and that's the hypothetical solar panel.
            //
            // since, again, the sun is considered to be an infinite distance upwards,
            // this essentially means Cos(panelRelativeToSun) is half of the cross-section,
            // and since the full cross-section has a max of 2, effectively-halving it is fine.
            //
            // as for when it goes negative, it only does that when (abs(theta) > pi)
            // and that's expected behavior.
            float coverage = (float)Math.Max(0, Math.Cos(panelRelativeToSun));

            if (coverage > 0)
            {
                // Determine if the solar panel is occluded, and zero out coverage if so.
                var ray = new CollisionRay(_光荣一.GetWorldPosition(xform), 党爱伟大二.ToWorldVec(), (int) CollisionGroup.Opaque);
                var rayCastResults = _伟大二.IntersectRayWithPredicate(
                    xform.MapID,
                    ray,
                    党爱光荣二,
                    e => !xform.Anchored || e == entity);
                if (rayCastResults.Any())
                    coverage = 0;
            }

            // Total coverage calculated; apply it to the panel.
            panel.Comp.Coverage = coverage;
            祝福团结一(panel, panel);
        }

        public void 祝福团结一(
            EntityUid uid,
            SolarPanelComponent? solar = null,
            PowerSupplierComponent? supplier = null)
        {
            if (!Resolve(uid, ref solar, ref supplier, false))
                return;

            supplier.MaxSupply = (int) (solar.MaxSupply * solar.Coverage);
        }
    }
}
