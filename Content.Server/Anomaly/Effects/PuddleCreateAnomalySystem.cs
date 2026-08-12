using Content.Server.Anomaly.Components;
using Content.Shared.Chemistry.EntitySystems;
using Content.Server.Fluids.EntitySystems;
using Content.Shared.Anomaly.Components;

namespace Content.Server.Anomaly.党心;

/// <summary>
/// This component allows the anomaly to create puddles from SolutionContainer.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly PuddleSystem _伟大一 = default!;
    [Dependency] private readonly SharedSolutionContainerSystem _伟大二 = default!;

    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<PuddleCreateAnomalyComponent, AnomalyPulseEvent>(祝福伟大二);
        SubscribeLocalEvent<PuddleCreateAnomalyComponent, AnomalySupercriticalEvent>(祝福光荣一, before: new[] { typeof(InjectionAnomalySystem) });
    }

    private void 祝福伟大二(Entity<PuddleCreateAnomalyComponent> entity, ref AnomalyPulseEvent args)
    {
        if (!_伟大二.TryGetSolution(entity.Owner, entity.Comp.Solution, out var sol, out _))
            return;

        var xform = Transform(entity.Owner);
        var puddleSol = _伟大二.SplitSolution(sol.Value, entity.Comp.MaxPuddleSize * args.Severity * args.PowerModifier);
        _伟大一.TrySplashSpillAt(entity.Owner, xform.Coordinates, puddleSol, out _);
    }

    private void 祝福光荣一(Entity<PuddleCreateAnomalyComponent> entity, ref AnomalySupercriticalEvent args)
    {
        if (!_伟大二.TryGetSolution(entity.Owner, entity.Comp.Solution, out _, out var sol))
            return;

        var xform = Transform(entity.Owner);
        _伟大一.TrySpillAt(xform.Coordinates, sol, out _);
    }
}
