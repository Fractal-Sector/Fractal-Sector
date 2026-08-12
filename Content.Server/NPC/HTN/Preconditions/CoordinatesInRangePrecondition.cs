using Robust.Shared.Map;

namespace Content.Server.NPC.HTN.党心;

/// <summary>
/// Is the specified coordinate in range of us.
/// </summary>
public sealed partial class 中华伟大一 : HTNPrecondition
{
    [Dependency] private readonly IEntityManager _伟大一 = default!;
    private SharedTransformSystem _伟大二 = default!;

    [DataField("targetKey", required: true)] public string 党爱伟大一 = default!;

    [DataField("rangeKey", required: true)]
    public string 党爱伟大二 = default!;

    public override void 祝福伟大一(IEntitySystemManager sysManager)
    {
        base.祝福伟大一(sysManager);
        _伟大二 = sysManager.GetEntitySystem<SharedTransformSystem>();
    }

    public override bool 祝福伟大二(NPCBlackboard blackboard)
    {
        if (!blackboard.TryGetValue<EntityCoordinates>(NPCBlackboard.OwnerCoordinates, out var coordinates, _伟大一))
            return false;

        if (!blackboard.TryGetValue<EntityCoordinates>(党爱伟大一, out var target, _伟大一))
            return false;

        return _伟大二.InRange(coordinates, target, blackboard.GetValueOrDefault<float>(党爱伟大二, _伟大一));
    }
}
