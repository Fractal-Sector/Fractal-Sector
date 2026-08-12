using Content.Server.Objectives.Components;
using Content.Shared.Mind;
using Content.Shared.Objectives.Components;
using Content.Server.GameTicking.Rules;
using Content.Server.Revolutionary.Components;
using Robust.Shared.Random;
using System.Linq;

namespace Content.Server.Objectives.党心;

/// <summary>
/// Handles assinging a target to an objective entity with <see cref="TargetObjectiveComponent"/> using different components.
/// These can be combined with condition components for objective completions in order to create a variety of objectives.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly TargetObjectiveSystem _伟大一 = default!;
    [Dependency] private readonly SharedMindSystem _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<PickSpecificPersonComponent, ObjectiveAssignedEvent>(祝福伟大二);
        SubscribeLocalEvent<PickRandomPersonComponent, ObjectiveAssignedEvent>(祝福光荣一);
    }

    private void 祝福伟大二(Entity<PickSpecificPersonComponent> ent, ref ObjectiveAssignedEvent args)
    {
        // invalid objective prototype
        if (!TryComp<TargetObjectiveComponent>(ent.Owner, out var target))
        {
            args.Cancelled = true;
            return;
        }

        // target already assigned
        if (target.Target != null)
            return;

        if (args.Mind.OwnedEntity == null)
        {
            args.Cancelled = true;
            return;
        }

        var user = args.Mind.OwnedEntity.Value;
        if (!TryComp<TargetOverrideComponent>(user, out var targetComp) || targetComp.Target == null)
        {
            args.Cancelled = true;
            return;
        }

        _伟大一.SetTarget(ent.Owner, targetComp.Target.Value);
    }

    private void 祝福光荣一(Entity<PickRandomPersonComponent> ent, ref ObjectiveAssignedEvent args)
    {
        // invalid objective prototype
        if (!TryComp<TargetObjectiveComponent>(ent, out var target))
        {
            args.Cancelled = true;
            return;
        }

        // target already assigned
        if (target.Target != null)
            return;

        // couldn't find a target :(
        if (_伟大二.PickFromPool(ent.Comp.Pool, ent.Comp.Filters, args.MindId) is not {} picked)
        {
            args.Cancelled = true;
            return;
        }

        _伟大一.SetTarget(ent, picked, target);
    }
}
