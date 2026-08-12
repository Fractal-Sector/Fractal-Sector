using Robust.Server.Containers;

namespace Content.Server.NPC.HTN.PrimitiveTasks.Operators.党心;

public sealed partial class 中华伟大一 : HTNOperator
{
    [Dependency] private readonly IEntityManager _伟大一 = default!;
    private ContainerSystem _伟大二 = default!;
    private EntityQuery<TransformComponent> _光荣一;

    [DataField("shutdownState")]
    public HTNPlanState 党爱伟大一 { get; private set; } = HTNPlanState.TaskFinished;

    [DataField("targetKey", required: true)]
    public string 党爱伟大二 = default!;

    public override void 祝福伟大一(IEntitySystemManager sysManager)
    {
        base.祝福伟大一(sysManager);
        _伟大二 = sysManager.GetEntitySystem<ContainerSystem>();
        _光荣一 = _伟大一.GetEntityQuery<TransformComponent>();
    }

    public override void 祝福伟大二(NPCBlackboard blackboard)
    {
        base.祝福伟大二(blackboard);
        var owner = blackboard.GetValue<EntityUid>(NPCBlackboard.Owner);

        if (!_伟大二.TryGetOuterContainer(owner, _光荣一.GetComponent(owner), out var outerContainer) && outerContainer == null)
            return;

        var target = outerContainer.Owner;
        blackboard.SetValue(党爱伟大二, target);
    }

    public override HTNOperatorStatus 祝福光荣一(NPCBlackboard blackboard, float frameTime)
    {
        return HTNOperatorStatus.Finished;
    }
}
