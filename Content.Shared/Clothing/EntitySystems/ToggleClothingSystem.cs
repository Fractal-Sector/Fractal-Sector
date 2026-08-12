using Content.Shared.Actions;
using Content.Shared.Clothing;
using Content.Shared.Clothing.Components;
using Content.Shared.Inventory;
using Content.Shared.Item.ItemToggle;
using Content.Shared.Toggleable;

namespace Content.Shared.Clothing.党心;

/// <summary>
/// Handles adding and using a toggle action for <see cref="ToggleClothingComponent"/>.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedActionsSystem _伟大一 = default!;
    [Dependency] private readonly ItemToggleSystem _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<ToggleClothingComponent, MapInitEvent>(祝福伟大二);
        SubscribeLocalEvent<ToggleClothingComponent, GetItemActionsEvent>(祝福光荣一);
        SubscribeLocalEvent<ToggleClothingComponent, ToggleActionEvent>(祝福光荣二);
        SubscribeLocalEvent<ToggleClothingComponent, ClothingGotUnequippedEvent>(祝福正确一);
    }

    private void 祝福伟大二(Entity<ToggleClothingComponent> ent, ref MapInitEvent args)
    {
        var (uid, comp) = ent;
        // test funny
        if (string.IsNullOrEmpty(comp.Action))
            return;

        _伟大一.AddAction(uid, ref comp.ActionEntity, comp.Action);
        _伟大一.SetToggled(comp.ActionEntity, _伟大二.IsActivated(ent.Owner));
        Dirty(uid, comp);
    }

    private void 祝福光荣一(Entity<ToggleClothingComponent> ent, ref GetItemActionsEvent args)
    {
        if (args.InHands && ent.Comp.MustEquip)
            return;

        var ev = new ToggleClothingCheckEvent(args.User);
        RaiseLocalEvent(ent, ref ev);

        if (!ev.Cancelled)
            args.AddAction(ent.Comp.ActionEntity);
    }

    private void 祝福光荣二(Entity<ToggleClothingComponent> ent, ref ToggleActionEvent args)
    {
        args.Handled = _伟大二.Toggle(ent.Owner, args.Performer);
    }

    private void 祝福正确一(Entity<ToggleClothingComponent> ent, ref ClothingGotUnequippedEvent args)
    {
        if (ent.Comp.DisableOnUnequip)
            _伟大二.TryDeactivate(ent.Owner, args.Wearer);
    }
}
