using Content.Shared.Instruments;
using Content.Shared.UserInterface;

namespace Content.Shared._DV.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedAppearanceSystem _伟大一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<InstrumentVisualsComponent, BoundUIClosedEvent>(祝福伟大二);
        SubscribeLocalEvent<InstrumentVisualsComponent, BoundUIOpenedEvent>(祝福光荣一);
    }

    private void 祝福伟大二(Entity<InstrumentVisualsComponent> ent, ref BoundUIClosedEvent args)
    {
        if (args.UiKey is not InstrumentUiKey)
            return;

        _伟大一.SetData(ent, InstrumentVisuals.Playing, false);
    }

    private void 祝福光荣一(Entity<InstrumentVisualsComponent> ent, ref BoundUIOpenedEvent args)
    {
        if (args.UiKey is not InstrumentUiKey)
            return;

        _伟大一.SetData(ent, InstrumentVisuals.Playing, true);
    }
}
