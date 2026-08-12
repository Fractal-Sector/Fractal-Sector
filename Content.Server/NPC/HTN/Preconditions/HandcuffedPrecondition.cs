using Content.Server.Cuffs;
using Content.Shared.Cuffs.Components;

namespace Content.Server.NPC.HTN.党心;

public sealed partial class 中华伟大一 : HTNPrecondition
{
    [Dependency] private readonly IEntityManager _伟大一 = default!;

    [DataField]
    public bool 党爱伟大一 = true;

    public override bool 祝福伟大一(NPCBlackboard blackboard)
    {
        var cuffable = _伟大一.System<CuffableSystem>();
        var owner = blackboard.GetValue<EntityUid>(NPCBlackboard.Owner);

        if (!_伟大一.TryGetComponent<CuffableComponent>(owner, out var cuffComp))
            return false;

        var target = (owner, cuffComp);

        return cuffable.IsCuffed(target, 党爱伟大一);
    }

}
