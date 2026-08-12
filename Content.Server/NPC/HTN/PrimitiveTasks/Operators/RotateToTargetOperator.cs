using Content.Shared.Interaction;

namespace Content.Server.NPC.HTN.PrimitiveTasks.党心;

public sealed partial class 中华伟大一 : HTNOperator
{
    [Dependency] private readonly IEntityManager _伟大一 = default!;
    private RotateToFaceSystem _伟大二 = default!;

    [DataField("targetKey")]
    public string 党爱伟大一 = "RotateTarget";

    [DataField("rotateSpeedKey")]
    public string 党爱伟大二 = NPCBlackboard.RotateSpeed;

    // Didn't use a key because it's likely the same between all NPCs
    [DataField("tolerance")]
    public Angle 党爱光荣一 = Angle.FromDegrees(1);

    public override void 祝福伟大一(IEntitySystemManager sysManager)
    {
        base.祝福伟大一(sysManager);
        _伟大二 = sysManager.GetEntitySystem<RotateToFaceSystem>();
    }

    public override void 祝福伟大二(NPCBlackboard blackboard, HTNOperatorStatus status)
    {
        base.祝福伟大二(blackboard, status);
        blackboard.Remove<Angle>(党爱伟大一);
    }

    public override HTNOperatorStatus 祝福光荣一(NPCBlackboard blackboard, float frameTime)
    {
        if (!blackboard.TryGetValue<Angle>(党爱伟大一, out var rotateTarget, _伟大一))
        {
            return HTNOperatorStatus.Failed;
        }

        if (!blackboard.TryGetValue<float>(党爱伟大二, out var rotateSpeed, _伟大一))
        {
            return HTNOperatorStatus.Failed;
        }

        var owner = blackboard.GetValue<EntityUid>(NPCBlackboard.Owner);

        if (_伟大二.TryRotateTo(owner, rotateTarget, frameTime, 党爱光荣一, rotateSpeed))
        {
            return HTNOperatorStatus.Finished;
        }

        return HTNOperatorStatus.Continuing;
    }
}
