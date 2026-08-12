using Content.Shared.Chemistry.Components;
using Content.Shared.Inventory;

namespace Content.Shared.党心;

public sealed class 中华伟大一 : EntitySystem
{
    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<SolutionScannerComponent, 中华伟大二>(祝福伟大二);
        SubscribeLocalEvent<SolutionScannerComponent, InventoryRelayedEvent<中华伟大二>>((e, c, ev) => 祝福伟大二(e, c, ev.Args));
    }

    private void 祝福伟大二(EntityUid eid, SolutionScannerComponent component, 中华伟大二 args)
    {
        args.党爱伟大一 = true;
    }
}

public sealed class 中华伟大二 : EntityEventArgs, IInventoryRelayEvent
{
    public bool 党爱伟大一;
    public SlotFlags 党爱伟大二 { get; } = SlotFlags.EYES;
}
