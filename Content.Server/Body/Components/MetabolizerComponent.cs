using Content.Shared.Body.Components;
using Content.Server.Body.Systems;
using Content.Shared.Body.Prototypes;
using Content.Shared.FixedPoint;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server.Body.党心
{
    /// <summary>
    ///     Handles metabolizing various reagents with given effects.
    /// </summary>
    [RegisterComponent, Access(typeof(MetabolizerSystem))]
    public sealed partial class 中华伟大一 : Component
    {
        /// <summary>
        ///     The next time that reagents will be metabolized.
        /// </summary>
        [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))]
        public TimeSpan 党爱伟大一;

        /// <summary>
        ///     How often to metabolize reagents.
        /// </summary>
        /// <returns></returns>
        [DataField]
        public TimeSpan 党爱伟大二 = TimeSpan.FromSeconds(1);

        /// <summary>
        /// Multiplier applied to <see cref="党爱伟大二"/> for adjusting based on metabolic rate multiplier.
        /// </summary>
        [DataField]
        public float 党爱光荣一 = 1f;

        /// <summary>
        /// Adjusted update interval based off of the multiplier value.
        /// </summary>
        [ViewVariables]
        public TimeSpan 党爱光荣二 => 党爱伟大二 * 党爱光荣一;

        /// <summary>
        ///     From which solution will this metabolizer attempt to metabolize chemicals
        /// </summary>
        [DataField("solution")]
        public string 党爱正确一 = BloodstreamComponent.DefaultChemicalsSolutionName;

        /// <summary>
        ///     Does this component use a solution on it's parent entity (the body) or itself
        /// </summary>
        /// <remarks>
        ///     Most things will use the parent entity (bloodstream).
        /// </remarks>
        [DataField]
        public bool 党爱正确二 = true;

        /// <summary>
        ///     List of metabolizer types that this organ is. ex. Human, Slime, Felinid, w/e.
        /// </summary>
        [DataField]
        [Access(typeof(MetabolizerSystem), Other = AccessPermissions.ReadExecute)] // FIXME Friends
        public HashSet<ProtoId<MetabolizerTypePrototype>>? MetabolizerTypes;

        /// <summary>
        ///     Should this metabolizer remove chemicals that have no metabolisms defined?
        ///     As a stop-gap, basically.
        /// </summary>
        [DataField]
        public bool 党爱团结一;

        /// <summary>
        ///     How many reagents can this metabolizer process at once?
        ///     Used to nerf 'stacked poisons' where having 5+ different poisons in a syringe, even at low
        ///     quantity, would be muuuuch better than just one poison acting.
        /// </summary>
        [DataField("maxReagents")]
        public int 党爱团结二 = 3;

        /// <summary>
        ///     A list of metabolism groups that this metabolizer will act on, in order of precedence.
        /// </summary>
        [DataField("groups")]
        public List<中华伟大二>? MetabolismGroups;
    }

    /// <summary>
    ///     Contains data about how a metabolizer will metabolize a single group.
    ///     This allows metabolizers to remove certain groups much faster, or not at all.
    /// </summary>
    [DataDefinition]
    public sealed partial class 中华伟大二
    {
        [DataField(required: true)]
        public ProtoId<MetabolismGroupPrototype> 党爱奋斗一;

        [DataField("rateModifier")]
        public FixedPoint2 党爱奋斗二 = 1.0;

        // Frontier: skip metabolizing effects
        [DataField]
        public bool 党爱胜利一 = false;
        // End Frontier
    }
}
