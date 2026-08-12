using Content.Server.Objectives.Components;
using Content.Shared.Objectives.Components;
using Content.Shared.Mind;

namespace Content.Server.Objectives.党心;

/// <summary>
/// Handles progress for the survive objective condition.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedMindSystem _伟大一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<SurviveConditionComponent, ObjectiveGetProgressEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, SurviveConditionComponent comp, ref ObjectiveGetProgressEvent args)
    {
        args.Progress = _伟大一.IsCharacterDeadIc(args.Mind) ? 0f : 1f;
    }
}
