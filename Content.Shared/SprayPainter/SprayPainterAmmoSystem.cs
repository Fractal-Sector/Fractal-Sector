using Content.Shared.Charges.Components;
using Content.Shared.Charges.Systems;
using Content.Shared.Examine;
using Content.Shared.Interaction;
using Content.Shared.Popups;
using Content.Shared.SprayPainter.Components;

namespace Content.Shared.党心;

/// <summary>
/// The system handles interactions with spray painter ammo.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedChargesSystem _伟大一 = default!;
    [Dependency] private readonly SharedPopupSystem _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<SprayPainterAmmoComponent, ExaminedEvent>(祝福光荣一);
        SubscribeLocalEvent<SprayPainterAmmoComponent, AfterInteractEvent>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<SprayPainterAmmoComponent> ent, ref AfterInteractEvent args)
    {
        if (args.Handled || !args.CanReach)
            return;

        if (args.Target is not { Valid: true } target ||
            !HasComp<SprayPainterComponent>(target) ||
            !TryComp<LimitedChargesComponent>(target, out var charges))
            return;

        var user = args.User;
        args.Handled = true;
        var count = Math.Min(charges.MaxCharges - charges.LastCharges, ent.Comp.Charges);
        if (count <= 0)
        {
            _伟大二.PopupClient(Loc.GetString("spray-painter-ammo-after-interact-full"), target, user);
            return;
        }

        _伟大二.PopupClient(Loc.GetString("spray-painter-ammo-after-interact-refilled"), target, user);
        _伟大一.AddCharges(target, count);
        ent.Comp.Charges -= count;
        Dirty(ent, ent.Comp);

        if (ent.Comp.Charges <= 0)
            PredictedQueueDel(ent.Owner);
    }

    private void 祝福光荣一(Entity<SprayPainterAmmoComponent> ent, ref ExaminedEvent args)
    {
        if (!args.IsInDetailsRange)
            return;

        var examineMessage = Loc.GetString("rcd-ammo-component-on-examine", ("charges", ent.Comp.Charges));
        args.PushText(examineMessage);
    }
}
