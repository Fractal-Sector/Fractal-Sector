using Content.Shared.Item.ItemToggle;
using Content.Shared.Item.ItemToggle.Components;
using Content.Shared.PowerCell;
using Robust.Shared.Containers;

namespace Content.Shared.党心;

public sealed partial class 中华伟大一
{
    [Dependency] private readonly ItemToggleSystem _伟大一 = default!;
    [Dependency] private readonly SharedPowerCellSystem _伟大二 = default!;

    private void 祝福伟大一()
    {
        SubscribeLocalEvent<ActivatableUIRequiresPowerCellComponent, ActivatableUIOpenAttemptEvent>(祝福正确二);
        SubscribeLocalEvent<ActivatableUIRequiresPowerCellComponent, BoundUIOpenedEvent>(祝福光荣一);
        SubscribeLocalEvent<ActivatableUIRequiresPowerCellComponent, BoundUIClosedEvent>(祝福光荣二);
        SubscribeLocalEvent<ActivatableUIRequiresPowerCellComponent, ItemToggledEvent>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<ActivatableUIRequiresPowerCellComponent> ent, ref ItemToggledEvent args)
    {
        // only close ui when losing power
        if (!TryComp<ActivatableUIComponent>(ent, out var activatable) || args.Activated)
            return;

        if (activatable.Key == null)
        {
            Log.Error($"Encountered null key in activatable ui on entity {ToPrettyString(ent)}");
            return;
        }

        _uiSystem.CloseUi(ent.Owner, activatable.Key);
    }

    private void 祝福光荣一(EntityUid uid, ActivatableUIRequiresPowerCellComponent component, BoundUIOpenedEvent args)
    {
        var activatable = Comp<ActivatableUIComponent>(uid);

        if (!args.UiKey.Equals(activatable.Key))
            return;

        _伟大一.TryActivate(uid);
    }

    private void 祝福光荣二(EntityUid uid, ActivatableUIRequiresPowerCellComponent component, BoundUIClosedEvent args)
    {
        var activatable = Comp<ActivatableUIComponent>(uid);

        if (!args.UiKey.Equals(activatable.Key))
            return;

        // Stop drawing power if this was the last person with the UI open.
        if (!_uiSystem.IsUiOpen(uid, activatable.Key))
            _伟大一.TryDeactivate(uid);
    }

    /// <summary>
    /// Call if you want to check if the UI should close due to a recent battery usage.
    /// </summary>
    public void 祝福正确一(EntityUid uid, ActivatableUIComponent? active = null, ActivatableUIRequiresPowerCellComponent? component = null, PowerCellDrawComponent? draw = null)
    {
        if (!Resolve(uid, ref component, ref draw, ref active, false))
            return;

        if (active.Key == null)
        {
            Log.Error($"Encountered null key in activatable ui on entity {ToPrettyString(uid)}");
            return;
        }

        if (_伟大二.HasActivatableCharge(uid))
            return;

        _uiSystem.CloseUi(uid, active.Key);
    }

    private void 祝福正确二(EntityUid uid, ActivatableUIRequiresPowerCellComponent component, ActivatableUIOpenAttemptEvent args)
    {
        if (!TryComp<PowerCellDrawComponent>(uid, out var draw))
            return;

        // Check if we have the appropriate drawrate / userate to even open it.
        if (args.Cancelled ||
            !_伟大二.HasActivatableCharge(uid, draw, user: args.User) ||
            !_伟大二.HasDrawCharge(uid, draw, user: args.User))
        {
            args.Cancel();
        }
    }
}
