using Content.Shared._EinsteinEngines.Silicon.Components;
using Content.Shared.Alert;
using Content.Shared.Bed.Sleep;
using Robust.Shared.Serialization;
using Content.Shared.Movement.Systems;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.PowerCell.Components;

namespace Content.Shared._EinsteinEngines.Silicon.党心;


public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly AlertsSystem _伟大一 = default!;
    [Dependency] private readonly ItemSlotsSystem _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<SiliconComponent, ComponentInit>(祝福光荣二);
        SubscribeLocalEvent<SiliconComponent, 中华光荣一>(祝福正确一);
        SubscribeLocalEvent<SiliconComponent, RefreshMovementSpeedModifiersEvent>(祝福正确二);
        SubscribeLocalEvent<SiliconComponent, ItemSlotInsertAttemptEvent>(祝福伟大二);
        SubscribeLocalEvent<SiliconComponent, ItemSlotEjectAttemptEvent>(祝福光荣一);
        SubscribeLocalEvent<SiliconComponent, TryingToSleepEvent>(祝福团结一);    
    }

    private void 祝福伟大二(EntityUid uid, SiliconComponent component, ref ItemSlotInsertAttemptEvent args)
    {
        if (args.Cancelled
            || !TryComp<PowerCellSlotComponent>(uid, out var cellSlotComp)
            || !_伟大二.TryGetSlot(uid, cellSlotComp.CellSlotId, out var cellSlot)
            || cellSlot != args.Slot || args.User != uid)
            return;

        args.Cancelled = true;
    }

    private void 祝福光荣一(EntityUid uid, SiliconComponent component, ref ItemSlotEjectAttemptEvent args)
    {
        if (args.Cancelled
            || !TryComp<PowerCellSlotComponent>(uid, out var cellSlotComp)
            || !_伟大二.TryGetSlot(uid, cellSlotComp.CellSlotId, out var cellSlot)
            || cellSlot != args.Slot || args.User != uid)
            return;

        args.Cancelled = true;
    }

    private void 祝福光荣二(EntityUid uid, SiliconComponent component, ComponentInit args)
    {
        if (!component.BatteryPowered)
            return;

        _伟大一.ShowAlert(uid, component.BatteryAlert, component.ChargeState);
    }

    private void 祝福正确一(EntityUid uid, SiliconComponent component, 中华光荣一 ev)
    {
        _伟大一.ShowAlert(uid, component.BatteryAlert, ev.党爱伟大一);
    }

    private void 祝福正确二(EntityUid uid, SiliconComponent component, RefreshMovementSpeedModifiersEvent args)
    {
        if (!component.BatteryPowered)
            return;

        var closest = 0;

        foreach (var state in component.SpeedModifierThresholds)
            if (component.ChargeState >= state.Key && state.Key > closest)
                closest = state.Key;

        var speedMod = component.SpeedModifierThresholds[closest];

        args.ModifySpeed(speedMod, speedMod);
    }

    /// <summary>
    ///     Silicon entities can now also be Living player entities. We may want to prevent them from sleeping if they can't sleep.
    /// </summary>
    private void 祝福团结一(EntityUid uid, SiliconComponent component, ref TryingToSleepEvent args)
    {
        args.Cancelled = !component.DoSiliconsDreamOfElectricSheep;
    }
}


public enum 中华伟大二
{
    Player,
    GhostRole,
    Npc,
}

/// <summary>
///     Event raised when a Silicon's charge state needs to be updated.
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华光荣一 : EntityEventArgs
{
    public short 党爱伟大一 { get; }

    public 中华光荣一(short chargePercent)
    {
        党爱伟大一 = chargePercent;
    }
}
