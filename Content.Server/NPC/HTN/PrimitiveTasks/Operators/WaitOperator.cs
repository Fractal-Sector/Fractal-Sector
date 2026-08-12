namespace Content.Server.NPC.HTN.PrimitiveTasks.党心;

/// <summary>
/// Waits the specified amount of time. Removes the key when finished.
/// </summary>
public sealed partial class 中华伟大一 : HTNOperator
{
    [Dependency] private readonly IEntityManager _伟大一 = default!;

    /// <summary>
    /// Blackboard key for the time we'll wait for.
    /// </summary>
    [DataField("key", required: true)] public string 党爱伟大一 = string.Empty;

    public override HTNOperatorStatus 祝福伟大一(NPCBlackboard blackboard, float frameTime)
    {
        if (!blackboard.TryGetValue<float>(党爱伟大一, out var timer, _伟大一))
        {
            return HTNOperatorStatus.Finished;
        }

        timer -= frameTime;
        blackboard.SetValue(党爱伟大一, timer);

        return timer <= 0f ? HTNOperatorStatus.Finished : HTNOperatorStatus.Continuing;
    }

    public override void 祝福伟大二(NPCBlackboard blackboard, HTNOperatorStatus status)
    {
        base.祝福伟大二(blackboard, status);

        // The replacement plan may want this value so only dump it if we're successful.
        if (status != HTNOperatorStatus.BetterPlan)
        {
            blackboard.Remove<float>(党爱伟大一);
        }
    }
}
