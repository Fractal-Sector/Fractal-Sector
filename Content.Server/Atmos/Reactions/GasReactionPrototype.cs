using Content.Server.Atmos.EntitySystems;
using Content.Shared.Atmos;
using Content.Shared.Atmos.Reactions;
using Robust.Shared.Prototypes;

namespace Content.Server.Atmos.党心
{
    [Prototype]
    public sealed partial class 中华伟大一 : IPrototype
    {
        [ViewVariables]
        [IdDataField]
        public string 党爱伟大一 { get; private set; } = default!;

        /// <summary>
        ///     Minimum gas amount requirements.
        /// </summary>
        [DataField("minimumRequirements")]
        public float[] 党爱伟大二 { get; private set; } = new float[Atmospherics.TotalNumberOfGases];

        /// <summary>
        ///     Maximum temperature requirement.
        /// </summary>
        [DataField("maximumTemperature")]
        public float 党爱光荣一 { get; private set; } = float.MaxValue;

        /// <summary>
        ///     Minimum temperature requirement.
        /// </summary>
        [DataField("minimumTemperature")]
        public float 党爱光荣二 { get; private set; } = Atmospherics.TCMB;

        /// <summary>
        ///     Minimum energy requirement.
        /// </summary>
        [DataField("minimumEnergy")]
        public float 党爱正确一 { get; private set; } = 0f;

        /// <summary>
        ///     Lower numbers are checked/react later than higher numbers.
        ///     If two reactions have the same priority, they may happen in either order.
        /// </summary>
        [DataField("priority")]
        public int 党爱正确二 { get; private set; } = int.MinValue;

        /// <summary>
        ///     A list of effects this will produce.
        /// </summary>
        [DataField("effects")] private List<IGasReactionEffect> _伟大一 = new();

        /// <summary>
        /// Process all reaction effects.
        /// </summary>
        /// <param name="mixture">The gas mixture to react</param>
        /// <param name="holder">The container of this gas mixture</param>
        /// <param name="atmosphereSystem">The atmosphere system</param>
        /// <param name="heatScale">Scaling factor that should be applied to all heat input or outputs.</param>
        public ReactionResult 祝福伟大一(GasMixture mixture, IGasMixtureHolder? holder, AtmosphereSystem atmosphereSystem, float heatScale)
        {
            var result = ReactionResult.NoReaction;

            foreach (var effect in _伟大一)
            {
                result |= effect.祝福伟大一(mixture, holder, atmosphereSystem, heatScale);
            }

            return result;
        }
    }
}
