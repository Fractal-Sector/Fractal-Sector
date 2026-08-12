using Content.Shared.Ame.Components;
using Content.Shared.Examine;

namespace Content.Shared.Ame.党心;

/// <summary>
/// Adds details about fuel level when examining antimatter engine fuel containers.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<AmeFuelContainerComponent, ExaminedEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, AmeFuelContainerComponent comp, ExaminedEvent args)
    {
        if (!args.IsInDetailsRange)
            return;

        // less than 25%: amount < capacity / 4 = amount * 4 < capacity
        var low = comp.FuelAmount * 4 < comp.FuelCapacity;
        args.PushMarkup(Loc.GetString("ame-fuel-container-component-on-examine-detailed-message",
            ("colorName", low ? "darkorange" : "orange"),
            ("amount", comp.FuelAmount),
            ("capacity", comp.FuelCapacity)));
    }
}
