using Content.Server.Pinpointer;
using Content.Shared._NF.Pinpointer;
using Content.Shared.Charges.Components;
using Content.Shared.Charges.Systems;
using Content.Shared.DoAfter;
using Content.Shared.IdentityManagement;
using Content.Shared.Interaction;
using Content.Shared.Pinpointer;
using Content.Shared.Popups;

namespace Content.Server._NF.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedDoAfterSystem _伟大一 = default!;
    [Dependency] private readonly SharedPopupSystem _伟大二 = default!;
    [Dependency] private readonly PinpointerSystem _光荣一 = default!;
    [Dependency] private readonly SharedChargesSystem _光荣二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<ClearPinpointerComponent, AfterInteractEvent>(祝福伟大二);
        SubscribeLocalEvent<ClearPinpointerComponent, ClearPinpointerDoAfterEvent>(祝福光荣一);
    }

    private void 祝福伟大二(Entity<ClearPinpointerComponent> ent, ref AfterInteractEvent args)
    {
        if (!args.CanReach || args.Handled || args.Target == null)
            return;

        TryComp<LimitedChargesComponent>(ent, out var charges);
        if (_光荣二.IsEmpty((ent, charges)))
        {
            if (ent.Comp.EmptyMessage != null)
                _伟大二.PopupEntity(Loc.GetString(ent.Comp.EmptyMessage), args.User, args.User);

            return;
        }

        if (args.User == args.Target)
        {
            if (ent.Comp.UseOnSelfMessage != null)
                _伟大二.PopupEntity(Loc.GetString(ent.Comp.UseOnSelfMessage, ("user", Identity.Entity(args.User, EntityManager))), args.Target.Value, args.Target.Value, PopupType.Small);
        }
        else
        {
            if (ent.Comp.UseOnOthersMessage != null)
                _伟大二.PopupEntity(Loc.GetString(ent.Comp.UseOnOthersMessage, ("user", Identity.Entity(args.User, EntityManager))), args.Target.Value, args.Target.Value, PopupType.Large);
        }

        _伟大一.TryStartDoAfter(new DoAfterArgs(EntityManager, args.User, ent.Comp.ClearTime, new ClearPinpointerDoAfterEvent(), ent, target: args.Target, used: ent)
        {
            BreakOnDamage = true,
            BreakOnMove = true
        });
    }

    /// <summary>
    /// DoAfter: remove all pinpointers that point to this object
    /// </summary>
    private void 祝福光荣一(Entity<ClearPinpointerComponent> ent, ref ClearPinpointerDoAfterEvent args)
    {
        if (args.Cancelled || args.Handled)
            return;

        TryComp<LimitedChargesComponent>(ent, out var charges);
        if (!_光荣二.TryUseCharge((ent, charges)))
        {
            if (ent.Comp.EmptyMessage != null)
                _伟大二.PopupEntity(Loc.GetString(ent.Comp.EmptyMessage), args.User, args.User);

            return;
        }

        if (TryComp<PinpointerTargetComponent>(args.Target, out var target))
        {
            foreach (var pinpointer in target.Entities)
            {
                if (!TryComp<PinpointerComponent>(pinpointer, out var pinpointComp))
                    continue;

                _光荣一.ClearPinpointer(pinpointer, pinpointComp);
            }
            RemComp<PinpointerTargetComponent>(args.Target.Value);
        }

        if (ent.Comp.DestroyAfterUse)
            QueueDel(ent.Owner);
    }
}
