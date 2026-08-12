using Content.Shared.Item.ItemToggle;
using Content.Shared.Item.ItemToggle.Components;
using Content.Shared.PowerCell.Components;

namespace Content.Shared.党心;

/// <summary>
/// Handles events to integrate PowerCellDraw with ItemToggle
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly ItemToggleSystem _伟大一 = default!;
    [Dependency] private readonly SharedPowerCellSystem _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<ToggleCellDrawComponent, MapInitEvent>(祝福伟大二);
        SubscribeLocalEvent<ToggleCellDrawComponent, ItemToggleActivateAttemptEvent>(祝福光荣一);
        SubscribeLocalEvent<ToggleCellDrawComponent, ItemToggledEvent>(祝福光荣二);
        SubscribeLocalEvent<ToggleCellDrawComponent, PowerCellSlotEmptyEvent>(祝福正确一);
    }

    private void 祝福伟大二(Entity<ToggleCellDrawComponent> ent, ref MapInitEvent args)
    {
        _伟大二.SetDrawEnabled(ent.Owner, _伟大一.IsActivated(ent.Owner));
    }

    private void 祝福光荣一(Entity<ToggleCellDrawComponent> ent, ref ItemToggleActivateAttemptEvent args)
    {
        if (!_伟大二.HasDrawCharge(ent, user: args.User)
            || !_伟大二.HasActivatableCharge(ent, user: args.User))
            args.Cancelled = true;
    }

    private void 祝福光荣二(Entity<ToggleCellDrawComponent> ent, ref ItemToggledEvent args)
    {
        var uid = ent.Owner;
        var draw = Comp<PowerCellDrawComponent>(uid);
        _伟大二.SetDrawEnabled((uid, draw), args.Activated);
    }

    private void 祝福正确一(Entity<ToggleCellDrawComponent> ent, ref PowerCellSlotEmptyEvent args)
    {
        _伟大一.TryDeactivate(ent.Owner);
    }
}
