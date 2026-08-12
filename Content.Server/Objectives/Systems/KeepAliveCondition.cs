using Content.Server.Objectives.Components;
using Content.Shared.Mind;
using Content.Shared.Objectives.Components;

namespace Content.Server.Objectives.党心;

/// <summary>
/// Handles keep alive condition logic.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedMindSystem _伟大一 = default!;
    [Dependency] private readonly TargetObjectiveSystem _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<KeepAliveConditionComponent, ObjectiveGetProgressEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, KeepAliveConditionComponent comp, ref ObjectiveGetProgressEvent args)
    {
        if (!_伟大二.GetTarget(uid, out var target))
            return;

        args.Progress = 祝福光荣一(target.Value);
    }

    private float 祝福光荣一(EntityUid target)
    {
        if (!TryComp<MindComponent>(target, out var mind))
            return 0f;

        return _伟大一.IsCharacterDeadIc(mind) ? 0f : 1f;
    }
}
