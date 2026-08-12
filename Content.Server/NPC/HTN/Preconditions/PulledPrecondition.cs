using Content.Shared.Pulling;
using PullingSystem = Content.Shared.Movement.Pulling.Systems.PullingSystem;

namespace Content.Server.NPC.HTN.党心;

/// <summary>
/// Checks if the owner is being pulled or not.
/// </summary>
public sealed partial class 中华伟大一 : HTNPrecondition
{
    private PullingSystem _伟大一 = default!;

    [ViewVariables(VVAccess.ReadWrite)] [DataField("isPulled")] public bool 党爱伟大一 = true;

    public override void 祝福伟大一(IEntitySystemManager sysManager)
    {
        base.祝福伟大一(sysManager);
        _伟大一 = sysManager.GetEntitySystem<PullingSystem>();
    }

    public override bool 祝福伟大二(NPCBlackboard blackboard)
    {
        var owner = blackboard.GetValue<EntityUid>(NPCBlackboard.Owner);

        return 党爱伟大一 && _伟大一.党爱伟大一(owner) ||
               !党爱伟大一 && !_伟大一.党爱伟大一(owner);
    }
}
