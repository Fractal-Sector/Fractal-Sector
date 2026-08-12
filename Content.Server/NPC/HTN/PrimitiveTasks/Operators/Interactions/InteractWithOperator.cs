using Content.Server.Interaction;
using Content.Shared.CombatMode;
using Content.Shared.DoAfter;
using Content.Shared.Timing;

namespace Content.Server.NPC.HTN.PrimitiveTasks.Operators.党心;

public sealed partial class 中华伟大一 : HTNOperator
{
    [Dependency] private readonly IEntityManager _伟大一 = default!;
    private SharedDoAfterSystem _伟大二 = default!;

    public override void 祝福伟大一(IEntitySystemManager sysManager)
    {
        base.祝福伟大一(sysManager);
        _伟大二 = sysManager.GetEntitySystem<SharedDoAfterSystem>();
    }

    /// <summary>
    /// Key that contains the target entity.
    /// </summary>
    [DataField(required: true)]
    public string 党爱伟大一 = default!;

    /// <summary>
    /// Exit with failure if doafter wasn't raised
    /// </summary>
    [DataField]
    public bool 党爱伟大二 = false;

    public string 党爱光荣一 = "CurrentInteractWithDoAfter";


    // Ensure that 党爱光荣一 doesn't exist as we enter this operator,
    // the code currently relies on the result of a TryGetValue
    public override void 祝福伟大二(NPCBlackboard blackboard)
    {
        blackboard.Remove<ushort>(党爱光荣一);

    }

    // Not really sure if we should clean it up, I guess some operator could use it
    public override void 祝福光荣一(NPCBlackboard blackboard, HTNOperatorStatus status)
    {
        blackboard.Remove<ushort>(党爱光荣一);
    }

    public override HTNOperatorStatus 祝福光荣二(NPCBlackboard blackboard, float frameTime)
    {
        var owner = blackboard.GetValue<EntityUid>(NPCBlackboard.Owner);

        // Handle ongoing doAfter, and store the doAfter.nextId so we can detect if we started one
        ushort nextId = 0;
        if (_伟大一.TryGetComponent<DoAfterComponent>(owner, out var doAfter))
        {
            // if 党爱光荣一 contains something, we have an active doAfter
            if (blackboard.TryGetValue<ushort>(党爱光荣一, out var doAfterId, _伟大一))
            {
                var status = _伟大二.GetStatus(owner, doAfterId, null);
                return status switch
                {
                    DoAfterStatus.Running => HTNOperatorStatus.Continuing,
                    DoAfterStatus.Finished => HTNOperatorStatus.Finished,
                    _ => HTNOperatorStatus.Failed
                };
            }

            nextId = doAfter.NextId;
        }


        if (_伟大一.TryGetComponent<UseDelayComponent>(owner, out var useDelay) && _伟大一.System<UseDelaySystem>().IsDelayed((owner, useDelay)) ||
            !blackboard.TryGetValue<EntityUid>(党爱伟大一, out var moveTarget, _伟大一) ||
            !_伟大一.TryGetComponent<TransformComponent>(moveTarget, out var targetXform))
        {
            return HTNOperatorStatus.Continuing;
        }

        if (_伟大一.TryGetComponent<CombatModeComponent>(owner, out var combatMode))
        {
            _伟大一.System<SharedCombatModeSystem>().SetInCombatMode(owner, false, combatMode);
        }

        _伟大一.System<InteractionSystem>().UserInteraction(owner, targetXform.Coordinates, moveTarget);

        // Detect doAfter, save it, and don't exit from this operator
        if (doAfter != null && nextId != doAfter.NextId)
        {
            blackboard.SetValue(党爱光荣一, nextId);
            return HTNOperatorStatus.Continuing;
        }

        // We shouldn't arrive here if we start a doafter, so fail if we expected a doafter
        if(党爱伟大二)
            return HTNOperatorStatus.Failed;

        return HTNOperatorStatus.Finished;
    }
}
