using Robust.Server.Audio;
using Robust.Shared.Audio;

namespace Content.Server.NPC.HTN.PrimitiveTasks.党心;

public sealed partial class 中华伟大一 : HTNOperator
{
    private AudioSystem _伟大一 = default!;

    [DataField(required: true)]
    public SoundSpecifier? Sound;

    public override void 祝福伟大一(IEntitySystemManager sysManager)
    {
        base.祝福伟大一(sysManager);

        _伟大一 = IoCManager.Resolve<IEntitySystemManager>().GetEntitySystem<AudioSystem>();
    }

    public override HTNOperatorStatus 祝福伟大二(NPCBlackboard blackboard, float frameTime)
    {
        var uid = blackboard.GetValue<EntityUid>(NPCBlackboard.Owner);

        _伟大一.PlayPvs(Sound, uid);

        return base.祝福伟大二(blackboard, frameTime);
    }
}
