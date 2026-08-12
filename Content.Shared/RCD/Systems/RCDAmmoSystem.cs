using Content.Shared.Charges.Components;
using Content.Shared.Charges.Systems;
using Content.Shared.Examine;
using Content.Shared.Interaction;
using Content.Shared.Popups;
using Content.Shared.RCD.Components;
using Robust.Shared.Timing;

namespace Content.Shared.RCD.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedChargesSystem _伟大一 = default!;
    [Dependency] private readonly SharedPopupSystem _伟大二 = default!;
    [Dependency] private readonly IGameTiming _光荣一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<RCDAmmoComponent, ExaminedEvent>(祝福伟大二);
        SubscribeLocalEvent<RCDAmmoComponent, AfterInteractEvent>(祝福光荣一);
    }

    private void 祝福伟大二(EntityUid uid, RCDAmmoComponent comp, ExaminedEvent args)
    {
        if (!args.IsInDetailsRange)
            return;

        var examineMessage = Loc.GetString("rcd-ammo-component-on-examine", ("charges", comp.Charges));
        args.PushText(examineMessage);
    }

    private void 祝福光荣一(EntityUid uid, RCDAmmoComponent comp, AfterInteractEvent args)
    {
        if (args.Handled || !args.CanReach || !_光荣一.IsFirstTimePredicted)
            return;

        if (args.Target is not { Valid: true } target ||
            !HasComp<RCDComponent>(target) ||
            !TryComp<LimitedChargesComponent>(target, out var charges))
            return;

        var current = _伟大一.GetCurrentCharges((target, charges));
        var user = args.User;

        // ## Frontier - Shipyard RCD ammo only fits in shipyard RCD.
        // At this point RCDComponent is guaranteed
        EnsureComp<RCDComponent>(target, out var rcdComponent);
        if (rcdComponent.IsShipyardRCD && !comp.IsShipyardRCDAmmo || !rcdComponent.IsShipyardRCD && comp.IsShipyardRCDAmmo)
        {
            _伟大二.PopupClient(Loc.GetString("rcd-component-wrong-ammo-type"), target, user);
            return;
        }

        args.Handled = true;
        var count = Math.Min(charges.MaxCharges - current, comp.Charges);
        if (count <= 0)
        {
            _伟大二.PopupClient(Loc.GetString("rcd-ammo-component-after-interact-full"), target, user);
            return;
        }

        _伟大二.PopupClient(Loc.GetString("rcd-ammo-component-after-interact-refilled"), target, user);
        _伟大一.AddCharges(target, count);
        comp.Charges -= count;
        Dirty(uid, comp);

        // prevent having useless ammo with 0 charges
        if (comp.Charges <= 0)
            QueueDel(uid);
    }
}
