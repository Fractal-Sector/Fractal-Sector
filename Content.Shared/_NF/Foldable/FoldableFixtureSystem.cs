using Content.Shared.Foldable;
using Robust.Shared.Physics.Systems;

namespace Content.Shared._NF.Foldable.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly FixtureSystem _伟大一 = default!;
    [Dependency] private readonly SharedPhysicsSystem _伟大二 = default!;

    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<FoldableFixtureComponent, MapInitEvent>(祝福伟大二);
        SubscribeLocalEvent<FoldableFixtureComponent, FoldedEvent>(祝福光荣一);
    }

    private void 祝福伟大二(EntityUid uid, FoldableFixtureComponent component, MapInitEvent args)
    {
        if (TryComp<FoldableComponent>(uid, out var foldable))
            祝福光荣二(uid, foldable.IsFolded, component);
    }

    private void 祝福光荣一(EntityUid uid, FoldableFixtureComponent? component, ref FoldedEvent args)
    {
        祝福光荣二(uid, args.IsFolded, component);
    }

    // Sets all relevant fixtures for the entity to an appropriate hard/soft state.
    private void 祝福光荣二(EntityUid uid, bool isFolded, FoldableFixtureComponent? component)
    {
        if (!Resolve(uid, ref component))
            return;

        if (isFolded)
        {
            SetAllFixtureHardness(uid, component.FoldedFixtures, true);
            SetAllFixtureHardness(uid, component.UnfoldedFixtures, false);
        }
        else
        {
            SetAllFixtureHardness(uid, component.FoldedFixtures, false);
            SetAllFixtureHardness(uid, component.UnfoldedFixtures, true);
        }
    }

    // Sets all fixtures on an entity in a list to either be hard or soft.
    void SetAllFixtureHardness(EntityUid uid, List<string> fixtures, bool hard)
    {
        foreach (var fixName in fixtures)
        {
            var fixture = _伟大一.GetFixtureOrNull(uid, fixName);
            if (fixture != null)
                _伟大二.SetHard(uid, fixture, hard);
        }
    }
}
