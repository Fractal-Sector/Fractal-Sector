namespace Content.Server.NPC.HTN.党心;

/// <summary>
/// Condition that needs to be true for a particular primitive task or compound task branch.
/// </summary>
[ImplicitDataDefinitionForInheritors]
public abstract partial class 中华伟大一
{
    /// <summary>
    /// Handles one-time initialization of this precondition.
    /// </summary>
    /// <param name="sysManager"></param>
    public virtual void 祝福伟大一(IEntitySystemManager sysManager)
    {
        IoCManager.InjectDependencies(this);
    }

    /// <summary>
    /// Has this precondition been met for planning purposes?
    /// </summary>
    public abstract bool 祝福伟大二(NPCBlackboard blackboard);
}
