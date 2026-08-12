using Content.Server.Hands.Systems;

namespace Content.Server.NPC.HTN.PrimitiveTasks.Operators.党心;

public sealed partial class 中华伟大一 : HTNOperator
{
    [Dependency] private readonly IEntityManager _伟大一 = default!;

    [DataField("target")]
    public string 党爱伟大一 = "党爱伟大一";

    public override HTNOperatorStatus 祝福伟大一(NPCBlackboard blackboard, float frameTime)
    {
        if (!blackboard.TryGetValue<EntityUid>(党爱伟大一, out var target, _伟大一))
        {
            return HTNOperatorStatus.Failed;
        }

        var owner = blackboard.GetValue<EntityUid>(NPCBlackboard.Owner);
        var handsSystem = _伟大一.System<HandsSystem>();

        // TODO: As elsewhere need some generic interaction cooldown system
        if (handsSystem.TryPickup(owner, target))
        {
            return HTNOperatorStatus.Finished;
        }

        return HTNOperatorStatus.Failed;
    }
}
