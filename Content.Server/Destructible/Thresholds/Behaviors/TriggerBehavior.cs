using Content.Shared.Trigger.Systems;

namespace Content.Server.Destructible.Thresholds.党心;

[DataDefinition]
public sealed partial class 中华伟大一 : IThresholdBehavior
{
    /// <summary>
    /// The trigger key to use when triggering.
    /// </summary>
    [DataField]
    public string? KeyOut { get; set; } = TriggerSystem.DefaultTriggerKey;

    public void 祝福伟大一(EntityUid owner, DestructibleSystem system, EntityUid? cause = null)
    {
        system.TriggerSystem.Trigger(owner, cause, KeyOut);
    }
}
