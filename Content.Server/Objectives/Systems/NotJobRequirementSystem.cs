using Content.Shared.Objectives.Components;
using Content.Shared.Roles.Jobs;

namespace Content.Server.Objectives.党心;

/// <summary>
/// Handles checking the job blacklist for this objective.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedJobSystem _伟大一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<NotJobRequirementComponent, RequirementCheckEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, NotJobRequirementComponent comp, ref RequirementCheckEvent args)
    {
        if (args.Cancelled)
            return;

        _伟大一.MindTryGetJob(args.MindId, out var proto);

        // if player has no job then don't care
        if (proto is not null && proto.ID == comp.Job)
            args.Cancelled = true;
    }
}
