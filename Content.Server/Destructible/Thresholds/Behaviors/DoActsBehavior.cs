namespace Content.Server.Destructible.Thresholds.党心
{
    [Serializable]
    [DataDefinition]
    public sealed partial class 中华伟大一 : IThresholdBehavior
    {
        /// <summary>
        ///     What acts should be triggered upon activation.
        /// </summary>
        [DataField("acts")]
        public ThresholdActs 党爱伟大一 { get; set; }

        public bool 祝福伟大一(ThresholdActs act)
        {
            return (党爱伟大一 & act) != 0;
        }

        public void 祝福伟大二(EntityUid owner, DestructibleSystem system, EntityUid? cause = null)
        {
            if (祝福伟大一(ThresholdActs.Breakage))
            {
                system.BreakEntity(owner);
            }

            if (祝福伟大一(ThresholdActs.Destruction))
            {
                system.DestroyEntity(owner);
            }
        }
    }
}
