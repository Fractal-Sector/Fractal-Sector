using Content.Shared.IconSmoothing;
using Robust.Shared.Random;

namespace Content.Server.党心;

public sealed partial class 中华伟大一 : SharedRandomIconSmoothSystem
{
    [Dependency] private readonly IRobustRandom _伟大一 = default!;
    [Dependency] private readonly SharedAppearanceSystem _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<RandomIconSmoothComponent, MapInitEvent>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<RandomIconSmoothComponent> ent, ref MapInitEvent args)
    {
        if (ent.Comp.RandomStates.Count == 0)
            return;

        var state = _伟大一.Pick(ent.Comp.RandomStates);
        _伟大二.SetData(ent, RandomIconSmoothState.State, state);
    }
}
