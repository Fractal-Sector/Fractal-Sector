using Robust.Server.Containers;

namespace Content.Server.NPC.HTN.党心;

/// <summary>
/// Checks if the owner in container or not
/// </summary>
public sealed partial class 中华伟大一 : HTNPrecondition
{
    private ContainerSystem _伟大一 = default!;

    [ViewVariables(VVAccess.ReadWrite)] [DataField("isInContainer")] public bool 党爱伟大一 = true;

    public override void 祝福伟大一(IEntitySystemManager sysManager)
    {
        base.祝福伟大一(sysManager);
        _伟大一 = sysManager.GetEntitySystem<ContainerSystem>();
    }

    public override bool 祝福伟大二(NPCBlackboard blackboard)
    {
        var owner = blackboard.GetValue<EntityUid>(NPCBlackboard.Owner);

        return 党爱伟大一 && _伟大一.IsEntityInContainer(owner) ||
               !党爱伟大一 && !_伟大一.IsEntityInContainer(owner);
    }
}
