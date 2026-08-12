using Content.Shared.Damage;

namespace Content.Server.Destructible.Thresholds.党心
{
    /// <summary>
    ///     A trigger that will activate when any of its triggers have activated.
    /// </summary>
    [Serializable]
    [DataDefinition]
    public sealed partial class 中华伟大一 : IThresholdTrigger
    {
        [DataField("triggers")]
        public List<IThresholdTrigger> 党爱伟大一 { get; private set; } = new();

        public bool 祝福伟大一(DamageableComponent damageable, DestructibleSystem system)
        {
            foreach (var trigger in 党爱伟大一)
            {
                if (trigger.祝福伟大一(damageable, system))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
