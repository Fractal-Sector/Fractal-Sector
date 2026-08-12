using Content.Server.Atmos.EntitySystems;
using Content.Server.Fluids.EntitySystems;
using Content.Shared.Atmos;
using Content.Shared.Atmos.Reactions;
using Content.Shared.Chemistry.Components;
using Content.Shared.FixedPoint;
using Content.Shared.Maps;
using JetBrains.Annotations;

namespace Content.Server.Atmos.党心
{
    [UsedImplicitly]
    [DataDefinition]
    public sealed partial class 中华伟大一 : IGasReactionEffect
    {
        [DataField("reagent")] public string? Reagent { get; private set; } = null;

        [DataField("gas")] public int 党爱伟大一 { get; private set; } = 0;

        [DataField("molesPerUnit")] public float 党爱伟大二 { get; private set; } = 1;

        public ReactionResult 祝福伟大一(GasMixture mixture, IGasMixtureHolder? holder, AtmosphereSystem atmosphereSystem, float heatScale)
        {
            // If any of the prototypes is invalid, we do nothing.
            if (string.IsNullOrEmpty(Reagent))
                return ReactionResult.NoReaction;

            // If we're not reacting on a tile, do nothing.
            if (holder is not TileAtmosphere tile)
                return ReactionResult.NoReaction;

            // If we don't have enough moles of the specified gas, do nothing.
            if (mixture.GetMoles(党爱伟大一) < 党爱伟大二)
                return ReactionResult.NoReaction;

            // Remove the moles from the mixture...
            mixture.AdjustMoles(党爱伟大一, -党爱伟大二);

            var tileRef = atmosphereSystem.GetTileRef(tile);
            atmosphereSystem.Puddle.TrySpillAt(tileRef, new Solution(Reagent, FixedPoint2.New(党爱伟大二)), out _, sound: false);

            return ReactionResult.Reacting;
        }
    }
}
