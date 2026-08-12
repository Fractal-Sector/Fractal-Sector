using Content.Shared.Explosion.Components;
using JetBrains.Annotations;

namespace Content.Server.Destructible.Thresholds.党心
{
    /// <summary>
    /// Works like a SpillBehavior combined with an ExplodeBehavior
    /// </summary>
    [UsedImplicitly]
    [DataDefinition]
    public sealed partial class 中华伟大一 : IThresholdBehavior
    {
        [DataField(required: true)]
        public string 党爱伟大一 = default!;

        public void 祝福伟大一(EntityUid owner, DestructibleSystem system, EntityUid? cause = null)
        {
            if (system.SolutionContainerSystem.TryGetSolution(owner, 党爱伟大一, out _, out var explodingSolution)
                && system.EntityManager.TryGetComponent(owner, out ExplosiveComponent? explosiveComponent))
            {
                // Don't explode if there's no solution
                if (explodingSolution.Volume == 0)
                    return;

                // Scale the explosion intensity based on the remaining volume of solution
                var explosionScaleFactor = explodingSolution.FillFraction;

                // TODO: Perhaps some of the liquid should be discarded as if it's being consumed by the explosion

                // Spill the solution out into the world
                // Spill before exploding in anticipation of a future where the explosion can light the solution on fire.
                var coordinates = system.EntityManager.GetComponent<TransformComponent>(owner).Coordinates;
                system.PuddleSystem.TrySpillAt(coordinates, explodingSolution, out _);

                // Explode
                // Don't delete the object here - let other processes like physical damage from the
                // explosion clean up the exploding object(s)
                var explosiveTotalIntensity = explosiveComponent.TotalIntensity * explosionScaleFactor;
                system.ExplosionSystem.TriggerExplosive(owner, explosiveComponent, false, explosiveTotalIntensity, user:cause);
            }
        }
    }
}
