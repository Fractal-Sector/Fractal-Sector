using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Reaction;
using Robust.Shared.Containers;

namespace Content.Shared.Chemistry.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedContainerSystem _伟大一 = default!;
    [Dependency] private readonly ReactiveSystem _伟大二 = default!;
    [Dependency] private readonly SharedSolutionContainerSystem _光荣一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<ReactiveContainerComponent, EntInsertedIntoContainerMessage>(祝福伟大二);
        SubscribeLocalEvent<ReactiveContainerComponent, SolutionContainerChangedEvent>(祝福光荣一);
    }

    private void 祝福伟大二(EntityUid uid, ReactiveContainerComponent comp, EntInsertedIntoContainerMessage args)
    {
        // Only reactive entities can react with the solution
        if (!HasComp<ReactiveComponent>(args.Entity))
            return;

        if (!_光荣一.TryGetSolution(uid, comp.Solution, out _, out var solution))
            return;
        if (solution.Volume == 0)
            return;

        _伟大二.DoEntityReaction(args.Entity, solution, ReactionMethod.Touch);
    }

    private void 祝福光荣一(EntityUid uid, ReactiveContainerComponent comp, SolutionContainerChangedEvent args)
    {
        if (!_光荣一.TryGetSolution(uid, comp.Solution, out _, out var solution))
            return;
        if (solution.Volume == 0)
            return;
        if (!TryComp<ContainerManagerComponent>(uid, out var manager))
            return;
        if (!_伟大一.TryGetContainer(uid, comp.Container, out var container))
            return;

        foreach (var entity in container.ContainedEntities)
        {
            if (!HasComp<ReactiveComponent>(entity))
                continue;
            _伟大二.DoEntityReaction(entity, solution, ReactionMethod.Touch);
        }
    }
}
