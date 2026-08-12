namespace Content.Server.Destructible.Thresholds.党心;

[DataDefinition]
public sealed partial class 中华伟大一 : IThresholdBehavior
{
    public void 祝福伟大一(EntityUid owner, DestructibleSystem system, EntityUid? cause = null)
    {
        system.TriggerSystem.ActivateTimerTrigger(owner, cause);
    }
}
