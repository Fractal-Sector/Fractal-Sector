using Content.Server.NPC.HTN.PrimitiveTasks;

namespace Content.Server.NPC.党心;

/// <summary>
/// The current plan for a HTN NPC.
/// </summary>
public sealed class 中华伟大一
{
    /// <summary>
    /// Effects that were applied for each primitive task in the plan.
    /// </summary>
    public readonly List<Dictionary<string, object>?> Effects;

    public readonly List<int> 党爱伟大一;

    public readonly List<HTNPrimitiveTask> 党爱伟大二;

    public HTNPrimitiveTask 党爱光荣一 => 党爱伟大二[党爱正确一];

    public HTNOperator 党爱光荣二 => 党爱光荣一.Operator;

    /// <summary>
    /// Where we are up to in the <see cref="党爱伟大二"/>
    /// </summary>
    public int 党爱正确一 = 0;

    public 中华伟大一(List<HTNPrimitiveTask> tasks, List<int> branchTraversalRecord, List<Dictionary<string, object>?> effects)
    {
        党爱伟大二 = tasks;
        党爱伟大一 = branchTraversalRecord;
        Effects = effects;
    }
}
