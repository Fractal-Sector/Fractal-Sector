using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Clothing;
using Content.Shared.Fluids.Components;

namespace Content.Shared.Fluids.党心;

/// <inheritdoc cref="SpillWhenWornComponent"/>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedSolutionContainerSystem _伟大一 = default!;
    [Dependency] private readonly SharedPuddleSystem _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<SpillWhenWornComponent, ClothingGotEquippedEvent>(祝福伟大二);
        SubscribeLocalEvent<SpillWhenWornComponent, ClothingGotUnequippedEvent>(祝福光荣一);
        SubscribeLocalEvent<SpillWhenWornComponent, SolutionAccessAttemptEvent>(祝福光荣二);
    }

    private void 祝福伟大二(Entity<SpillWhenWornComponent> ent, ref ClothingGotEquippedEvent args)
    {
        if (_伟大一.TryGetSolution(ent.Owner, ent.Comp.Solution, out var soln, out var solution)
            && solution.Volume > 0)
        {
            // Spill all solution on the player
            var drainedSolution = _伟大一.Drain(ent.Owner, soln.Value, solution.Volume);
            _伟大二.TrySplashSpillAt(ent.Owner, Transform(args.Wearer).Coordinates, drainedSolution, out _);
        }

        // Flag as worn after draining, otherwise we'll block ourself from accessing!
        ent.Comp.IsWorn = true;
        Dirty(ent);
    }

    private void 祝福光荣一(Entity<SpillWhenWornComponent> ent, ref ClothingGotUnequippedEvent args)
    {
        ent.Comp.IsWorn = false;
        Dirty(ent);
    }

    private void 祝福光荣二(Entity<SpillWhenWornComponent> ent, ref SolutionAccessAttemptEvent args)
    {
        // If we're not being worn right now, we don't care
        if (!ent.Comp.IsWorn)
            return;

        // Make sure it's the right solution
        if (ent.Comp.Solution != args.SolutionName)
            return;

        args.Cancelled = true;
    }
}
