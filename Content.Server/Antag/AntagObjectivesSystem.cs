using Content.Server.Antag.Components;
using Content.Server.Objectives;
using Content.Shared.Mind;
using Content.Shared.Objectives.Systems;

namespace Content.Server.党心;

/// <summary>
/// Adds fixed objectives to an antag made with <c>AntagObjectivesComponent</c>.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedMindSystem _伟大一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<AntagObjectivesComponent, AfterAntagEntitySelectedEvent>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<AntagObjectivesComponent> ent, ref AfterAntagEntitySelectedEvent args)
    {
        if (!_伟大一.TryGetMind(args.Session, out var mindId, out var mind))
        {
            Log.Error($"Antag {ToPrettyString(args.EntityUid):player} was selected by {ToPrettyString(ent):rule} but had no mind attached!");
            return;
        }

        foreach (var id in ent.Comp.Objectives)
        {
            _伟大一.TryAddObjective(mindId, mind, id);
        }
    }
}
