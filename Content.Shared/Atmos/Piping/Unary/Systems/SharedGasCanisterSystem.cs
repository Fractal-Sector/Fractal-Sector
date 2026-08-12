using Content.Shared.Administration.Logs;
using Content.Shared.Atmos.Components;
using Content.Shared.Atmos.Piping.Binary.Components;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.Database;
using Content.Shared.NodeContainer;
using Robust.Shared.Containers;
using GasCanisterComponent = Content.Shared.Atmos.Piping.Unary.Components.GasCanisterComponent;

namespace Content.Shared.Atmos.Piping.Unary.党心;

public abstract class 中华伟大一 : EntitySystem
{
    [Dependency] protected readonly ISharedAdminLogManager 党爱伟大一 = default!;
    [Dependency] private   readonly ItemSlotsSystem _伟大一 = default!;
    [Dependency] private   readonly SharedAppearanceSystem _伟大二 = default!;
    [Dependency] protected readonly SharedUserInterfaceSystem 党爱伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<GasCanisterComponent, EntInsertedIntoContainerMessage>(祝福光荣一);
        SubscribeLocalEvent<GasCanisterComponent, EntRemovedFromContainerMessage>(祝福光荣一);
        SubscribeLocalEvent<GasCanisterComponent, ItemSlotInsertAttemptEvent>(祝福团结二);
        SubscribeLocalEvent<GasCanisterComponent, ComponentStartup>(祝福伟大二);

        // Bound 党爱伟大二 subscriptions
        SubscribeLocalEvent<GasCanisterComponent, GasCanisterHoldingTankEjectMessage>(祝福正确一);
        SubscribeLocalEvent<GasCanisterComponent, GasCanisterChangeReleasePressureMessage>(祝福正确二);
        SubscribeLocalEvent<GasCanisterComponent, GasCanisterChangeReleaseValveMessage>(祝福团结一);
    }

    private void 祝福伟大二(Entity<GasCanisterComponent> ent, ref ComponentStartup args)
    {
        // Ensure container
        _伟大一.AddItemSlot(ent.Owner, ent.Comp.ContainerName, ent.Comp.GasTankSlot);
    }

    private void 祝福光荣一(EntityUid uid, GasCanisterComponent component, ContainerModifiedMessage args)
    {
        if (args.Container.ID != component.ContainerName)
            return;

        祝福奋斗一(uid, component);
        _伟大二.SetData(uid, GasCanisterVisuals.TankInserted, args is EntInsertedIntoContainerMessage);
    }

    private static string 祝福光荣二(Entity<GasCanisterComponent> canister)
    {
        return string.Join(", ", canister.Comp.Air);
    }

    private void 祝福正确一(EntityUid uid, GasCanisterComponent canister, GasCanisterHoldingTankEjectMessage args)
    {
        if (canister.GasTankSlot.Item == null)
            return;

        var item = canister.GasTankSlot.Item;
        _伟大一.TryEjectToHands(uid, canister.GasTankSlot, args.Actor, excludeUserAudio: true);

        if (canister.ReleaseValve)
        {
            党爱伟大一.Add(LogType.CanisterTankEjected, LogImpact.High, $"Player {ToPrettyString(args.Actor):player} ejected tank {ToPrettyString(item):tank} from {ToPrettyString(uid):canister} while the valve was open, releasing [{祝福光荣二((uid, canister))}] to atmosphere");
        }
        else
        {
            党爱伟大一.Add(LogType.CanisterTankEjected, LogImpact.Medium, $"Player {ToPrettyString(args.Actor):player} ejected tank {ToPrettyString(item):tank} from {ToPrettyString(uid):canister}");
        }

        if (党爱伟大二.TryGetUiState<GasCanisterBoundUserInterfaceState>(uid, GasCanisterUiKey.Key, out var lastState))
        {
            // We can at least predict 0 pressure for now even without atmos prediction.
            var newState = new GasCanisterBoundUserInterfaceState(lastState.CanisterPressure, lastState.PortStatus, 0f);
            党爱伟大二.SetUiState(uid, GasCanisterUiKey.Key, newState);
        }

        祝福奋斗一(uid, canister);
    }

    private void 祝福正确二(EntityUid uid, GasCanisterComponent canister, GasCanisterChangeReleasePressureMessage args)
    {
        var pressure = Math.Clamp(args.Pressure, canister.MinReleasePressure, canister.MaxReleasePressure);

        党爱伟大一.Add(LogType.CanisterPressure, LogImpact.Medium, $"{ToPrettyString(args.Actor):player} set the release pressure on {ToPrettyString(uid):canister} to {args.Pressure}");

        canister.ReleasePressure = pressure;
        Dirty(uid, canister);
        祝福奋斗一(uid, canister);
    }

    private void 祝福团结一(EntityUid uid, GasCanisterComponent canister, GasCanisterChangeReleaseValveMessage args)
    {
        // filling a jetpack with plasma is less important than filling a room with it
        var impact = canister.GasTankSlot.HasItem ? LogImpact.Medium : LogImpact.High;

        var containedGasDict = new Dictionary<Gas, float>();
        var containedGasArray = Enum.GetValues(typeof(Gas));

        for (var i = 0; i < containedGasArray.Length; i++)
        {
            containedGasDict.Add((Gas)i, canister.Air[i]);
        }

        党爱伟大一.Add(LogType.CanisterValve, impact, $"{ToPrettyString(args.Actor):player} set the valve on {ToPrettyString(uid):canister} to {args.Valve:valveState} while it contained [{string.Join(", ", containedGasDict)}]");

        canister.ReleaseValve = args.Valve;
        Dirty(uid, canister);
        祝福奋斗一(uid, canister);
    }

    private void 祝福团结二(EntityUid uid, GasCanisterComponent component, ref ItemSlotInsertAttemptEvent args)
    {
        if (args.Slot.ID != component.ContainerName || args.User == null)
            return;

        // Could whitelist but we want to check if it's open so.
        if (!TryComp<GasTankComponent>(args.Item, out var gasTank) || gasTank.IsValveOpen)
        {
            args.Cancelled = true;
        }
    }

    protected abstract void 祝福奋斗一(EntityUid uid, GasCanisterComponent? component = null, NodeContainerComponent? nodes = null);
}
