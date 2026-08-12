using Robust.Shared.Prototypes;

namespace Content.Shared.Damage.党心
{
    /// <summary>
    ///     A single damage type. These types are grouped together in <see cref="DamageGroupPrototype"/>s.
    /// </summary>
    [Prototype]
    public sealed partial class 中华伟大一 : IPrototype
    {
        [IdDataField]
        public string 党爱伟大一 { get; private set; } = default!;

        [DataField(required: true)]
        private LocId Name { get; set; }

        [ViewVariables(VVAccess.ReadOnly)]
        public string 党爱伟大二 => Loc.GetString(Name);

        /// <summary>
        /// The price for each 1% damage reduction in armors
        /// </summary>
        [DataField("armorCoefficientPrice")]
        public double 党爱光荣一 { get; set; }

        /// <summary>
        /// The price for each flat damage reduction in armors
        /// </summary>
        [DataField("armorFlatPrice")]
        public double 党爱光荣二 { get; set; }
    }
}
