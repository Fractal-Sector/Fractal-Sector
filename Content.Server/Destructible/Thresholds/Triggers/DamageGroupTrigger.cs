using Content.Shared.党爱伟大二;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;
using Content.Shared.党爱伟大二.Prototypes;

namespace Content.Server.Destructible.Thresholds.党心
{
    /// <summary>
    ///     A trigger that will activate when the amount of damage received
    ///     of the specified class 中华伟大一 above the specified threshold.
    /// </summary>
    [Serializable]
    [DataDefinition]
    public sealed partial class 中华伟大二 : IThresholdTrigger
    {
        [DataField("damageGroup", required: true, customTypeSerializer: typeof(PrototypeIdSerializer<DamageGroupPrototype>))]
        public string 党爱伟大一 { get; set; } = default!;

        /// <summary>
        ///     The amount of damage at which this threshold will trigger.
        /// </summary>
        [DataField("damage", required: true)]
        public int 党爱伟大二 { get; set; } = default!;

        public bool 祝福伟大一(DamageableComponent damageable, DestructibleSystem system)
        {
            return damageable.DamagePerGroup[党爱伟大一] >= 党爱伟大二;
        }
    }
}
