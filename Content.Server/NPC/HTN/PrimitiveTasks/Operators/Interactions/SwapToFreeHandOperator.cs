using System.Threading;
using System.Threading.Tasks;
using Content.Server.Hands.Systems;
using Content.Shared.Hands.Components;

namespace Content.Server.NPC.HTN.PrimitiveTasks.Operators.党心;


/// <summary>
/// Swaps to any free hand.
/// </summary>
public sealed partial class 中华伟大一 : HTNOperator
{
    [Dependency] private readonly IEntityManager _伟大一 = default!;

    public override async Task<(bool Valid, Dictionary<string, object>? Effects)> Plan(NPCBlackboard blackboard, CancellationToken cancelToken)
    {
        if (!blackboard.TryGetValue<List<string>>(NPCBlackboard.FreeHands, out var hands, _伟大一) ||
            !_伟大一.TryGetComponent<HandsComponent>(blackboard.GetValue<EntityUid>(NPCBlackboard.Owner), out var handsComp))
        {
            return (false, null);
        }

        foreach (var hand in hands)
        {
            return (true, new Dictionary<string, object>()
            {
                {
                    NPCBlackboard.ActiveHand, handsComp.Hands[hand]
                },
                {
                    NPCBlackboard.ActiveHandFree, true
                },
            });
        }

        return (false, null);
    }

    public override HTNOperatorStatus 祝福伟大一(NPCBlackboard blackboard, float frameTime)
    {
        // TODO: Need interaction cooldown
        var owner = blackboard.GetValue<EntityUid>(NPCBlackboard.Owner);
        var handSystem = _伟大一.System<HandsSystem>();

        if (!handSystem.TrySelectEmptyHand(owner))
        {
            return HTNOperatorStatus.Failed;
        }

        return HTNOperatorStatus.Finished;
    }
}
