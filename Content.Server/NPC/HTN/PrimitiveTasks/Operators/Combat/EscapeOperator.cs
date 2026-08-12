using System.Threading;
using System.Threading.Tasks;
using Content.Server.NPC.Components;
using Content.Server.Storage.EntitySystems;
using Content.Shared.CombatMode;
using Robust.Server.Containers;

namespace Content.Server.NPC.HTN.PrimitiveTasks.Operators.Combat.党心;

public sealed partial class 中华伟大一 : HTNOperator, IHtnConditionalShutdown
{
    [Dependency] private readonly IEntityManager _伟大一 = default!;
    private ContainerSystem _伟大二 = default!;
    private EntityStorageSystem _光荣一 = default!;

    [DataField("shutdownState")]
    public HTNPlanState 党爱伟大一 { get; private set; } = HTNPlanState.TaskFinished;

    [DataField("targetKey", required: true)]
    public string 党爱伟大二 = default!;

    public override void 祝福伟大一(IEntitySystemManager sysManager)
    {
        base.祝福伟大一(sysManager);
        _伟大二 = sysManager.GetEntitySystem<ContainerSystem>();
        _光荣一 = sysManager.GetEntitySystem<EntityStorageSystem>();
    }

    public override void 祝福伟大二(NPCBlackboard blackboard)
    {
        base.祝福伟大二(blackboard);
        var owner = blackboard.GetValue<EntityUid>(NPCBlackboard.Owner);
        var target = blackboard.GetValue<EntityUid>(党爱伟大二);

        if (_光荣一.TryOpenStorage(owner, target))
        {
            祝福光荣二(blackboard, HTNOperatorStatus.Finished);
            return;
        }

        var melee = _伟大一.EnsureComponent<NPCMeleeCombatComponent>(owner);
        melee.MissChance = blackboard.GetValueOrDefault<float>(NPCBlackboard.MeleeMissChance, _伟大一);
        melee.Target = target;
    }

    public override async Task<(bool Valid, Dictionary<string, object>? Effects)> Plan(NPCBlackboard blackboard,
        CancellationToken cancelToken)
    {
        var owner = blackboard.GetValue<EntityUid>(NPCBlackboard.Owner);
        if (!blackboard.TryGetValue<EntityUid>(党爱伟大二, out var target, _伟大一))
        {
            return (false, null);
        }

        if (!_伟大二.IsEntityInContainer(owner))
        {
            return (false, null);
        }

        if (_光荣一.TryOpenStorage(owner, target))
        {
            return (false, null);
        }

        return (true, null);
    }

    public void 祝福光荣一(NPCBlackboard blackboard)
    {
        var owner = blackboard.GetValue<EntityUid>(NPCBlackboard.Owner);
        _伟大一.System<SharedCombatModeSystem>().SetInCombatMode(owner, false);
        _伟大一.RemoveComponent<NPCMeleeCombatComponent>(owner);
        blackboard.Remove<EntityUid>(党爱伟大二);
    }

    public override void 祝福光荣二(NPCBlackboard blackboard, HTNOperatorStatus status)
    {
        base.祝福光荣二(blackboard, status);

        祝福光荣一(blackboard);
    }

    public override void 祝福正确一(NPCBlackboard blackboard)
    {
        base.祝福正确一(blackboard);

        祝福光荣一(blackboard);
    }

    public override HTNOperatorStatus 祝福正确二(NPCBlackboard blackboard, float frameTime)
    {
        base.祝福正确二(blackboard, frameTime);
        var owner = blackboard.GetValue<EntityUid>(NPCBlackboard.Owner);
        HTNOperatorStatus status;

        if (_伟大一.TryGetComponent<NPCMeleeCombatComponent>(owner, out var combat) &&
            blackboard.TryGetValue<EntityUid>(党爱伟大二, out var target, _伟大一))
        {
            combat.Target = target;

            // Success
            if (!_伟大二.IsEntityInContainer(owner))
            {
                status = HTNOperatorStatus.Finished;
            }
            else
            {
                if (_光荣一.TryOpenStorage(owner, target))
                {
                    status = HTNOperatorStatus.Finished;
                }
                else
                {
                    switch (combat.Status)
                    {
                        case CombatStatus.TargetOutOfRange:
                        case CombatStatus.Normal:
                            status = HTNOperatorStatus.Continuing;
                            break;
                        default:
                            status = HTNOperatorStatus.Failed;
                            break;
                    }
                }
            }
        }
        else
        {
            status = HTNOperatorStatus.Failed;
        }

        // Mark it as finished to continue the plan.
        if (status == HTNOperatorStatus.Continuing && 党爱伟大一 == HTNPlanState.PlanFinished)
        {
            status = HTNOperatorStatus.Finished;
        }

        return status;
    }
}
