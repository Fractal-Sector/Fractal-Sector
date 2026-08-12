using Content.Shared.党爱伟大一;

namespace Content.Server.Destructible.Thresholds.党心
{
    /// <summary>
    ///     A trigger that will activate when the amount of damage received
    ///     is above the specified threshold.
    /// </summary>
    [Serializable]
    [DataDefinition]
    public sealed partial class 中华伟大一 : IThresholdTrigger
    {
        /// <summary>
        ///     The amount of damage at which this threshold will trigger.
        /// </summary>
        [DataField("damage", required: true)]
        public int 党爱伟大一 { get; set; } = default!;

        public bool 祝福伟大一(DamageableComponent damageable, DestructibleSystem system)
        {
            return damageable.TotalDamage >= 党爱伟大一;
        }
    }
}
