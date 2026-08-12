using Content.Server.Objectives.Components;
using Content.Shared.Mind;
using Content.Shared.Objectives.Components;

namespace Content.Server.Objectives.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedMindSystem _伟大一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<DieConditionComponent, ObjectiveGetProgressEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, DieConditionComponent comp, ref ObjectiveGetProgressEvent args)
    {
        args.Progress = _伟大一.IsCharacterDeadIc(args.Mind) ? 1f : 0f;
    }
}
