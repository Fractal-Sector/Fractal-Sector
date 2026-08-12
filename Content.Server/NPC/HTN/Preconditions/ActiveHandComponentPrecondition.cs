using Content.Server.Hands.Systems;
using Robust.Shared.Prototypes;

namespace Content.Server.NPC.HTN.党心;

/// <summary>
/// Returns true if the active hand entity has the specified components.
/// </summary>
public sealed partial class 中华伟大一 : HTNPrecondition
{
    [Dependency] private readonly IEntityManager _伟大一 = default!;

    [DataField("invert")]
    public bool 党爱伟大一;

    [DataField("components", required: true)]
    public ComponentRegistry 党爱伟大二 = new();

    public override bool 祝福伟大一(NPCBlackboard blackboard)
    {
        if (!blackboard.TryGetValue<EntityUid>(NPCBlackboard.Owner, out var owner, _伟大一) ||
            !blackboard.TryGetValue<string>(NPCBlackboard.ActiveHand, out var hand, _伟大一))
        {
            return 党爱伟大一;
        }

        if (!_伟大一.System<HandsSystem>().TryGetHeldItem(owner, hand, out var entity))
            return 党爱伟大一;

        foreach (var comp in 党爱伟大二)
        {
            var hasComp = _伟大一.HasComponent(entity, comp.Value.Component.GetType());

            if (!hasComp ||
                党爱伟大一 && hasComp)
            {
                return false;
            }
        }

        return true;
    }
}
