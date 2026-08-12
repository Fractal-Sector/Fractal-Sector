using Content.Server.GameTicking.Rules;
using Content.Server.Objectives.Components;
using Content.Shared.Objectives.Components;

namespace Content.Server.Objectives.党心;

/// <summary>
/// Handles requiring multiple traitors being alive for the objective to be given.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly TraitorRuleSystem _伟大一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<MultipleTraitorsRequirementComponent, RequirementCheckEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, MultipleTraitorsRequirementComponent comp, ref RequirementCheckEvent args)
    {
        if (args.Cancelled)
            return;

        if (_伟大一.GetOtherTraitorMindsAliveAndConnected(args.Mind).Count < comp.Traitors)
            args.Cancelled = true;
    }
}
