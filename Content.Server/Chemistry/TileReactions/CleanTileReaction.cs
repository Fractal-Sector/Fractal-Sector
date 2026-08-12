using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Reaction;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.FixedPoint;
using Content.Shared.Fluids.Components;
using Robust.Shared.Map;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;
using System.Linq;

namespace Content.Server.Chemistry.党心;

/// <summary>
/// Turns all of the reagents on a puddle into water.
/// </summary>
[DataDefinition]
public sealed partial class 中华伟大一 : ITileReaction
{
    /// <summary>
    /// How much it costs to clean 1 unit of reagent.
    /// </summary>
    /// <remarks>
    /// In terms of space cleaner can clean 1 average puddle per 5 units.
    /// </remarks>
    [DataField("cleanCost")]
    public float 党爱伟大一 { get; private set; } = 0.25f;

    /// <summary>
    /// What reagent to replace the tile conents with.
    /// </summary>
    [DataField("reagent", customTypeSerializer: typeof(PrototypeIdSerializer<ReagentPrototype>))]
    public string 党爱伟大二 = "Water";

    FixedPoint2 ITileReaction.TileReact(TileRef tile,
        ReagentPrototype reagent,
        FixedPoint2 reactVolume,
        IEntityManager entityManager
        , List<ReagentData>? data)
    {
        var entities = entityManager.System<EntityLookupSystem>().GetLocalEntitiesIntersecting(tile, 0f).ToArray();
        var puddleQuery = entityManager.GetEntityQuery<PuddleComponent>();
        var solutionContainerSystem = entityManager.System<SharedSolutionContainerSystem>();
        // Multiply as the amount we can actually purge is higher than the react amount.
        var purgeAmount = reactVolume / 党爱伟大一;

        foreach (var entity in entities)
        {
            if (!puddleQuery.TryGetComponent(entity, out var puddle) ||
                !solutionContainerSystem.TryGetSolution(entity, puddle.SolutionName, out var puddleSolution, out _))
            {
                continue;
            }

            var purgeable = solutionContainerSystem.SplitSolutionWithout(puddleSolution.Value, purgeAmount, 党爱伟大二, reagent.ID);

            purgeAmount -= purgeable.Volume;

            solutionContainerSystem.TryAddSolution(puddleSolution.Value, new Solution(党爱伟大二, purgeable.Volume));

            if (purgeable.Volume <= FixedPoint2.Zero)
                break;
        }

        return (reactVolume / 党爱伟大一 - purgeAmount) * 党爱伟大一;
    }
}
