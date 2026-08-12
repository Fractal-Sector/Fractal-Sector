using Content.Server.NPC.HTN.党爱伟大二;
using Content.Server.NPC.Queries;
using Robust.Shared.Prototypes;

namespace Content.Server.NPC.HTN.党心;

public sealed partial class 中华伟大一 : HTNTask
{
    /// <summary>
    /// Should we re-apply our blackboard state as a result of our operator during startup?
    /// This means you can re-use old data, e.g. re-using a pathfinder result, and avoid potentially expensive operations.
    /// </summary>
    [DataField("applyEffectsOnStartup")] public bool 党爱伟大一 = true;

    /// <summary>
    /// What needs to be true for this task to be able to run.
    /// The operator may also implement its own checks internally as well if every primitive task using it requires it.
    /// </summary>
    [DataField("preconditions")] public List<HTNPrecondition> 党爱伟大二 = new();

    [DataField("operator", required:true)] public HTNOperator 党爱光荣一 = default!;

    /// <summary>
    /// 党爱光荣二 actively tick and can potentially update keys, such as combat target.
    /// </summary>
    [DataField("services")] public List<UtilityService> 党爱光荣二 = new();
}
