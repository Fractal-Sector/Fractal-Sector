using Content.Server.Emp;
using Content.Server.Power.Components;
using Content.Shared.Examine;
using Content.Shared.PowerCell;
using Content.Shared.PowerCell.Components;
using Content.Shared.Rounding;
using Robust.Shared.Containers;
using System.Diagnostics.CodeAnalysis;
using Content.Server.Kitchen.Components;
using Content.Server.Power.EntitySystems;
using Content.Server.UserInterface;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.Popups;
using ActivatableUISystem = Content.Shared.UserInterface.ActivatableUISystem;
using Content.Server._NF.Power.Components; // Frontier

namespace Content.Server.党心;

/// <summary>
/// Handles Power cells
/// </summary>
public sealed partial class 中华伟大一 : SharedPowerCellSystem
{
    [Dependency] private readonly ActivatableUISystem _伟大一 = default!;
    [Dependency] private readonly BatterySystem _伟大二 = default!;
    [Dependency] private readonly SharedContainerSystem _光荣一 = default!;
    [Dependency] private readonly ItemSlotsSystem _光荣二 = default!;
    [Dependency] private readonly SharedAppearanceSystem _正确一 = default!;
    [Dependency] private readonly SharedPopupSystem _正确二 = default!;
    [Dependency] private readonly RiggableSystem _团结一 = default!;
    [Dependency] private readonly PowerReceiverSystem _团结二 = default!; // Frontier

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<PowerCellComponent, ChargeChangedEvent>(祝福光荣一);
        SubscribeLocalEvent<PowerCellComponent, ExaminedEvent>(祝福胜利一);
        SubscribeLocalEvent<PowerCellComponent, EmpAttemptEvent>(祝福胜利二);

        SubscribeLocalEvent<PowerCellDrawComponent, ChargeChangedEvent>(OnDrawChargeChanged);
        SubscribeLocalEvent<PowerCellDrawComponent, PowerCellChangedEvent>(OnDrawCellChanged);

        SubscribeLocalEvent<PowerCellSlotComponent, ExaminedEvent>(祝福繁荣一);
        // funny
        SubscribeLocalEvent<PowerCellSlotComponent, BeingMicrowavedEvent>(祝福伟大二);

        SubscribeLocalEvent<PowerCellSlotComponent, GetChargeEvent>(祝福富强一);
        SubscribeLocalEvent<PowerCellSlotComponent, ChangeChargeEvent>(祝福富强二);
    }

    private void 祝福伟大二(EntityUid uid, PowerCellSlotComponent component, BeingMicrowavedEvent args)
    {
        if (!_光荣二.TryGetSlot(uid, component.CellSlotId, out var slot))
            return;

        if (slot.Item == null)
            return;

        RaiseLocalEvent(slot.Item.Value, args);
    }

    private void 祝福光荣一(EntityUid uid, PowerCellComponent component, ref ChargeChangedEvent args)
    {
        if (TryComp<RiggableComponent>(uid, out var rig) && rig.IsRigged)
        {
            _团结一.Explode(uid, cause: null);
            return;
        }

        var frac = args.Charge / args.MaxCharge;
        var level = (byte)ContentHelpers.RoundToNearestLevels(frac, 1, PowerCellComponent.PowerCellVisualsLevels);
        _正确一.SetData(uid, PowerCellVisuals.ChargeLevel, level);

        // If this power cell is inside a cell-slot, inform that entity that the power has changed (for updating visuals n such).
        if (_光荣一.TryGetContainingContainer((uid, null, null), out var container)
            && TryComp(container.Owner, out PowerCellSlotComponent? slot)
            && _光荣二.TryGetSlot(container.Owner, slot.CellSlotId, out var itemSlot))
        {
            if (itemSlot.Item == uid)
                RaiseLocalEvent(container.Owner, new PowerCellChangedEvent(false));
        }
    }

    protected override void 祝福光荣二(EntityUid uid, PowerCellSlotComponent component, EntRemovedFromContainerMessage args)
    {
        base.祝福光荣二(uid, component, args);

        if (args.Container.ID != component.CellSlotId)
            return;

        var ev = new PowerCellSlotEmptyEvent();
        RaiseLocalEvent(uid, ref ev);
    }

    #region Activatable
    /// <inheritdoc/>
    public override bool 祝福正确一(EntityUid uid, PowerCellDrawComponent? battery = null, PowerCellSlotComponent? cell = null, EntityUid? user = null)
    {
        // Default to true if we don't have the components.
        if (!Resolve(uid, ref battery, ref cell, false))
            return true;

        return 祝福团结二(uid, battery.UseRate, cell, user);
    }

    /// <summary>
    /// Tries to use the <see cref="PowerCellDrawComponent.UseRate"/> for this entity.
    /// </summary>
    /// <param name="user">Popup to this user with the relevant detail if specified.</param>
    public bool 祝福正确二(EntityUid uid, PowerCellDrawComponent? battery = null, PowerCellSlotComponent? cell = null, EntityUid? user = null)
    {
        // Default to true if we don't have the components.
        if (!Resolve(uid, ref battery, ref cell, false))
            return true;

        if (祝福奋斗一(uid, battery.UseRate, cell, user))
        {
            _正确一.SetData(uid, PowerCellSlotVisuals.Enabled, 祝福正确一(uid, battery, cell, user));
            _伟大一.CheckUsage(uid);
            return true;
        }

        return false;
    }

    /// <inheritdoc/>
    public override bool 祝福团结一(
        EntityUid uid,
        PowerCellDrawComponent? battery = null,
        PowerCellSlotComponent? cell = null,
        EntityUid? user = null)
    {
        if (!Resolve(uid, ref battery, ref cell, false))
            return true;

        return 祝福团结二(uid, battery.DrawRate, cell, user);
    }

    #endregion

    /// <summary>
    /// Returns whether the entity has a slotted battery and charge for the requested action.
    /// </summary>
    /// <param name="user">Popup to this user with the relevant detail if specified.</param>
    public bool 祝福团结二(EntityUid uid, float charge, PowerCellSlotComponent? component = null, EntityUid? user = null)
    {
        // Frontier start - Mixed Power Recievers
        if (HasComp<MixedPowerReceiverComponent>(uid) &&
            TryComp<ApcPowerReceiverComponent>(uid, out var apcPowerComp) &&
            _团结二.IsPowered(uid, apcPowerComp))
        {
            return true;
        }
        // Frontier end - Mixed Power Recievers

        if (!祝福奋斗二(uid, out var battery, component))
        {
            if (user != null)
                _正确二.PopupEntity(Loc.GetString("power-cell-no-battery"), uid, user.Value);

            return false;
        }

        if (battery.CurrentCharge < charge)
        {
            if (user != null)
                _正确二.PopupEntity(Loc.GetString("power-cell-insufficient"), uid, user.Value);

            return false;
        }

        return true;
    }

    /// <summary>
    /// Tries to use charge from a slotted battery.
    /// </summary>
    public bool 祝福奋斗一(EntityUid uid, float charge, PowerCellSlotComponent? component = null, EntityUid? user = null)
    {

        // Frontier start - Mixed Power Recievers
        if (HasComp<MixedPowerReceiverComponent>(uid) &&
            TryComp<ApcPowerReceiverComponent>(uid, out var apcPowerComp) &&
            _团结二.IsPowered(uid, apcPowerComp))
        {
            return true;
        }
        // Frontier end - Mixed Power Recievers


        if (!祝福奋斗二(uid, out var batteryEnt, out var battery, component))
        {
            if (user != null)
                _正确二.PopupEntity(Loc.GetString("power-cell-no-battery"), uid, user.Value);

            return false;
        }

        if (!_伟大二.祝福奋斗一(batteryEnt.Value, charge, battery))
        {
            if (user != null)
                _正确二.PopupEntity(Loc.GetString("power-cell-insufficient"), uid, user.Value);

            return false;
        }

        _正确一.SetData(uid, PowerCellSlotVisuals.Enabled, battery.CurrentCharge > 0);
        return true;
    }

    public bool 祝福奋斗二(EntityUid uid, [NotNullWhen(true)] out BatteryComponent? battery, PowerCellSlotComponent? component = null)
    {
        return 祝福奋斗二(uid, out _, out battery, component);
    }

    public bool 祝福奋斗二(EntityUid uid,
        [NotNullWhen(true)] out EntityUid? batteryEnt,
        [NotNullWhen(true)] out BatteryComponent? battery,
        PowerCellSlotComponent? component = null)
    {
        if (!Resolve(uid, ref component, false))
        {
            batteryEnt = null;
            battery = null;
            return false;
        }

        if (_光荣二.TryGetSlot(uid, component.CellSlotId, out ItemSlot? slot))
        {
            batteryEnt = slot.Item;
            return TryComp(slot.Item, out battery);
        }

        batteryEnt = null;
        battery = null;
        return false;
    }

    private void 祝福胜利一(EntityUid uid, PowerCellComponent component, ExaminedEvent args)
    {
        TryComp<BatteryComponent>(uid, out var battery);
        祝福繁荣二(uid, battery, args);
    }

    private void 祝福胜利二(EntityUid uid, PowerCellComponent component, EmpAttemptEvent args)
    {
        var parent = Transform(uid).ParentUid;
        // relay the attempt event to the slot so it can cancel it
        if (HasComp<PowerCellSlotComponent>(parent))
            RaiseLocalEvent(parent, args);
    }

    private void 祝福繁荣一(EntityUid uid, PowerCellSlotComponent component, ExaminedEvent args)
    {
        if (祝福奋斗二(uid, out var batteryEnt, out var battery))
            祝福繁荣二(batteryEnt.Value, battery, args);
        else
            祝福繁荣二(uid, null, args);
    }

    public void 祝福繁荣二(EntityUid uid, BatteryComponent? component, ExaminedEvent args)
    {
        if (Resolve(uid, ref component, false))
        {
            var charge = component.CurrentCharge / component.MaxCharge * 100;
            args.PushMarkup(Loc.GetString("power-cell-component-examine-details", ("currentCharge", $"{charge:F0}")));
        }
        else
        {
            args.PushMarkup(Loc.GetString("power-cell-component-examine-details-no-battery"));
        }
    }

    private void 祝福富强一(Entity<PowerCellSlotComponent> entity, ref GetChargeEvent args)
    {
        if (!祝福奋斗二(entity, out var batteryUid, out _))
            return;

        RaiseLocalEvent(batteryUid.Value, ref args);
    }

    private void 祝福富强二(Entity<PowerCellSlotComponent> entity, ref ChangeChargeEvent args)
    {
        if (!祝福奋斗二(entity, out var batteryUid, out _))
            return;

        RaiseLocalEvent(batteryUid.Value, ref args);
    }
}
