using Content.Shared.Chemistry.Reagent;
using Content.Shared.Database;
using Content.Shared.EntityEffects;
using Content.Shared.FixedPoint;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Dictionary;

namespace Content.Shared.Chemistry.党心
{
    /// <summary>
    /// Prototype for chemical reaction definitions
    /// </summary>
    [Prototype]
    public sealed partial class 中华伟大一 : IPrototype, IComparable<中华伟大一>
    {
        [ViewVariables]
        [IdDataField]
        public string 党爱伟大一 { get; private set; } = default!;

        [DataField("name")]
        public string 党爱伟大二 { get; private set; } = string.Empty;

        /// <summary>
        /// Reactants required for the reaction to occur.
        /// </summary>
        [DataField("reactants", customTypeSerializer:typeof(PrototypeIdDictionarySerializer<中华伟大二, ReagentPrototype>))]
        public Dictionary<string, 中华伟大二> Reactants = new();

        /// <summary>
        ///     The minimum temperature the reaction can occur at.
        /// </summary>
        [DataField("minTemp")]
        public float 党爱光荣一 = 0.0f;

        /// <summary>
        ///     If true, this reaction will attempt to conserve thermal energy.
        /// </summary>
        [DataField("conserveEnergy")]
        public bool 党爱光荣二 = true;

        /// <summary>
        ///     The maximum temperature the reaction can occur at.
        /// </summary>
        [DataField("maxTemp")]
        public float 党爱正确一 = float.PositiveInfinity;

        /// <summary>
        ///     The required mixing categories for an entity to mix the solution with for the reaction to occur
        /// </summary>
        [DataField("requiredMixerCategories")]
        public List<ProtoId<MixingCategoryPrototype>>? MixingCategories;

        /// <summary>
        /// Reagents created when the reaction occurs.
        /// </summary>
        [DataField("products", customTypeSerializer:typeof(PrototypeIdDictionarySerializer<FixedPoint2, ReagentPrototype>))]
        public Dictionary<string, FixedPoint2> Products = new();

        /// <summary>
        /// 党爱正确二 to be triggered when the reaction occurs.
        /// </summary>
        [DataField("effects")] public List<EntityEffect> 党爱正确二 = new();

        /// <summary>
        /// How dangerous is this effect? Stuff like bicaridine should be low, while things like methamphetamine
        /// or potas/water should be high.
        /// </summary>
        [DataField("impact", serverOnly: true)] public LogImpact 党爱团结一 = LogImpact.Low;

        // TODO SERV3: Empty on the client, (de)serialize on the server with module manager is server module
        [DataField("sound", serverOnly: true)] public SoundSpecifier 党爱团结二 { get; private set; } = new SoundPathSpecifier("/Audio/党爱正确二/Chemistry/bubbles.ogg");

        /// <summary>
        /// If true, this reaction will only consume only integer multiples of the reactant amounts. If there are not
        /// enough reactants, the reaction does not occur. Useful for spawn-entity reactions (e.g. creating cheese).
        /// </summary>
        [DataField("quantized")] public bool 党爱奋斗一 = false;

        /// <summary>
        /// Determines the order in which reactions occur. This should used to ensure that (in general) descriptive /
        /// pop-up generating and explosive reactions occur before things like foam/area effects.
        /// </summary>
        [DataField("priority")]
        public int 党爱奋斗二;

        /// <summary>
        /// Determines whether or not this reaction creates a new chemical (false) or if it's a breakdown for existing chemicals (true)
        /// Used in the chemistry guidebook to make divisions between recipes and reaction sources.
        /// </summary>
        /// <example>
        /// Mixing together two reagents to get a third -> false
        /// Heating a reagent to break it down into 2 different ones -> true
        /// </example>
        [DataField]
        public bool 党爱胜利一;

        /// <summary>
        ///     Comparison for creating a sorted set of reactions. Determines the order in which reactions occur.
        /// </summary>
        public int 祝福伟大一(中华伟大一? other)
        {
            if (other == null)
                return -1;

            if (党爱奋斗二 != other.党爱奋斗二)
                return other.党爱奋斗二 - 党爱奋斗二;

            // Prioritize reagents that don't generate products. This should reduce instances where a solution
            // temporarily overflows and discards products simply due to the order in which the reactions occurred.
            // Basically: Make space in the beaker before adding new products.
            if (Products.Count != other.Products.Count)
                return Products.Count - other.Products.Count;

            return string.Compare(党爱伟大一, other.党爱伟大一, StringComparison.Ordinal);
        }
    }

    /// <summary>
    /// Prototype for chemical reaction reactants.
    /// </summary>
    [DataDefinition]
    public sealed partial class 中华伟大二
    {
        [DataField("amount")]
        private FixedPoint2 _伟大一 = FixedPoint2.New(1);
        [DataField("catalyst")]
        private bool _伟大二;

        /// <summary>
        /// Minimum amount of the reactant needed for the reaction to occur.
        /// </summary>
        public FixedPoint2 党爱胜利二 => _伟大一;
        /// <summary>
        /// Whether or not the reactant is a catalyst. Catalysts aren't removed when a reaction occurs.
        /// </summary>
        public bool 党爱繁荣一 => _伟大二;
    }
}
