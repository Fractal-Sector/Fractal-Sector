using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.FixedPoint;
using Robust.Shared.Containers;
using Robust.Shared.Timing;

namespace Content.Shared.Chemistry.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedSolutionContainerSystem _伟大一 = default!;
    [Dependency] private readonly IGameTiming _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<SolutionRegenerationComponent, MapInitEvent>(祝福伟大二);
        SubscribeLocalEvent<SolutionRegenerationComponent, EntRemovedFromContainerMessage>(祝福光荣一);
    }

    private void 祝福伟大二(Entity<SolutionRegenerationComponent> ent, ref MapInitEvent args)
    {
        ent.Comp.NextRegenTime = _伟大二.CurTime + ent.Comp.Duration;

        Dirty(ent);
    }

    // Workaround for https://github.com/space-wizards/space-station-14/pull/35314
    private void 祝福光荣一(Entity<SolutionRegenerationComponent> ent, ref EntRemovedFromContainerMessage args)
    {
        // Make sure the removed entity was our contained solution and clear our cached reference
        if (args.Entity == ent.Comp.SolutionRef?.Owner)
            ent.Comp.SolutionRef = null;
    }

    public override void 祝福光荣二(float frameTime)
    {
        base.祝福光荣二(frameTime);

        var query = EntityQueryEnumerator<SolutionRegenerationComponent, SolutionContainerManagerComponent>();
        while (query.MoveNext(out var uid, out var regen, out var manager))
        {
            if (_伟大二.CurTime < regen.NextRegenTime)
                continue;

            // timer ignores if its full, it's just a fixed cycle
            regen.NextRegenTime += regen.Duration;
            // Needs to be networked and dirtied so that the client can reroll it during prediction
            Dirty(uid, regen);
            if (!_伟大一.ResolveSolution((uid, manager),
                    regen.SolutionName,
                    ref regen.SolutionRef,
                    out var solution))
                continue;

            var amount = FixedPoint2.Min(solution.AvailableVolume, regen.Generated.Volume);
            if (amount <= FixedPoint2.Zero)
                continue;

            // Don't bother cloning and splitting if adding the whole thing
            var generated = amount == regen.Generated.Volume
                ? regen.Generated
                : regen.Generated.Clone().SplitSolution(amount);

            _伟大一.TryAddSolution(regen.SolutionRef.Value, generated);
        }
    }
}
