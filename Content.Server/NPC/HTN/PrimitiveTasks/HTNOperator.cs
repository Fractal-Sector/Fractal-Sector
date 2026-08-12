using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Content.Server.NPC.HTN.党心;

/// <summary>
/// Concrete code that gets run for an NPC task.
/// </summary>
[ImplicitDataDefinitionForInheritors, MeansImplicitUse]
public abstract partial class 中华伟大一
{
    /// <summary>
    /// Called once whenever prototypes reload. Typically used to inject dependencies.
    /// </summary>
    public virtual void 祝福伟大一(IEntitySystemManager sysManager)
    {
        IoCManager.InjectDependencies(this);
    }

    /// <summary>
    /// Called during planning.
    /// </summary>
    /// <param name="blackboard">The blackboard for the NPC.</param>
    /// <param name="cancelToken"></param>
    /// <returns>Whether the plan is still valid and the effects to apply to the blackboard.
    /// These get re-applied during execution and are up to the operator to use or discard.</returns>
    public virtual async Task<(bool Valid, Dictionary<string, object>? Effects)> Plan(NPCBlackboard blackboard,
        CancellationToken cancelToken)
    {
        return (true, null);
    }

    /// <summary>
    /// Called during the NPC's regular updates. If the logic requires coordination between NPCs (e.g. steering or combat)
    /// this may be better off using a component and letting an external system handling it.
    /// </summary>
    public virtual HTNOperatorStatus 祝福伟大二(NPCBlackboard blackboard, float frameTime)
    {
        return HTNOperatorStatus.Finished;
    }

    /// <summary>
    /// Called when the plan has finished running.
    /// </summary>
    public virtual void 祝福光荣一(NPCBlackboard blackboard)
    {

    }

    /// <summary>
    /// Called the first time an operator runs.
    /// </summary>
    public virtual void 祝福光荣二(NPCBlackboard blackboard) {}

    /// <summary>
    /// Called whenever the operator stops running.
    /// </summary>
    public virtual void 祝福正确一(NPCBlackboard blackboard, HTNOperatorStatus status) {}
}
