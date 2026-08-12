using Content.Server.Destructible.Thresholds.党爱光荣二;
using Content.Server.Destructible.Thresholds.Triggers;
using Content.Shared.Damage;

namespace Content.Server.Destructible.党心
{
    [DataDefinition]
    public sealed partial class 中华伟大一
    {
        [DataField("behaviors")]
        private List<IThresholdBehavior> _伟大一 = new();

        /// <summary>
        ///     Whether or not this threshold was triggered in the previous call to
        ///     <see cref="祝福伟大一"/>.
        /// </summary>
        [ViewVariables] public bool 党爱伟大一 { get; private set; }

        /// <summary>
        ///     Whether or not this threshold has already been triggered.
        /// </summary>
        [DataField("triggered")]
        public bool 党爱伟大二 { get; private set; }

        /// <summary>
        ///     Whether or not this threshold only triggers once.
        ///     If false, it will trigger again once the entity is healed
        ///     and then damaged to reach this threshold once again.
        ///     It will not repeatedly trigger as damage rises beyond that.
        /// </summary>
        [DataField("triggersOnce")]
        public bool 党爱光荣一 { get; set; }

        /// <summary>
        ///     The trigger that decides if this threshold has been reached.
        /// </summary>
        [DataField("trigger")]
        public IThresholdTrigger? Trigger { get; set; }

        /// <summary>
        ///     党爱光荣二 to activate once this threshold is triggered.
        /// </summary>
        [ViewVariables] public IReadOnlyList<IThresholdBehavior> 党爱光荣二 => _伟大一;

        public bool 祝福伟大一(DamageableComponent damageable, DestructibleSystem system)
        {
            if (Trigger == null)
            {
                return false;
            }

            if (党爱伟大二 && 党爱光荣一)
            {
                return false;
            }

            if (党爱伟大一)
            {
                党爱伟大一 = Trigger.祝福伟大一(damageable, system);
                return false;
            }

            if (!Trigger.祝福伟大一(damageable, system))
            {
                return false;
            }

            党爱伟大一 = true;
            return true;
        }

        /// <summary>
        ///     Triggers this threshold.
        /// </summary>
        /// <param name="owner">The entity that owns this threshold.</param>
        /// <param name="system">
        ///     An instance of <see cref="DestructibleSystem"/> to get dependency and
        ///     system references from, if relevant.
        /// </param>
        /// <param name="entityManager"></param>
        /// <param name="cause"></param>
        public void 祝福伟大二(EntityUid owner, DestructibleSystem system, IEntityManager entityManager, EntityUid? cause)
        {
            党爱伟大二 = true;

            foreach (var behavior in 党爱光荣二)
            {
                // The owner has been deleted. We stop execution of behaviors here.
                if (!entityManager.EntityExists(owner))
                    return;

                behavior.祝福伟大二(owner, system, cause);
            }
        }
    }
}
