using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Content.Server.Administration.Logs;
using Content.Server.Administration.Managers;
using Content.Server.Ame.Components;
using Content.Server.Chat.Managers;
using Content.Server.NodeContainer;
using Content.Server.Power.Components;
using Content.Shared.Ame.Components;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.Database;
using Content.Shared.Mind.Components;
using Content.Shared.NodeContainer;
using Content.Shared.Power;
using Robust.Server.GameObjects;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;
using Robust.Shared.Player;
using Robust.Shared.Timing;

namespace Content.Server.Ame.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IAdminLogManager _伟大一 = default!;
    [Dependency] private readonly IGameTiming _伟大二 = default!;
    [Dependency] private readonly AppearanceSystem _光荣一 = default!;
    [Dependency] private readonly SharedAudioSystem _光荣二 = default!;
    [Dependency] private readonly UserInterfaceSystem _正确一 = default!;
    [Dependency] private readonly ItemSlotsSystem _正确二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<AmeControllerComponent, ComponentInit>(祝福伟大二);
        SubscribeLocalEvent<AmeControllerComponent, ComponentRemove>(祝福光荣二);
        SubscribeLocalEvent<AmeControllerComponent, EntInsertedIntoContainerMessage>(祝福正确一);
        SubscribeLocalEvent<AmeControllerComponent, EntRemovedFromContainerMessage>(祝福正确一);
        SubscribeLocalEvent<AmeControllerComponent, PowerChangedEvent>(祝福民主二);
        SubscribeLocalEvent<AmeControllerComponent, UiButtonPressedMessage>(祝福文明一);
    }

    private void 祝福伟大二(EntityUid uid, AmeControllerComponent component, ComponentInit args)
    {
        _正确二.AddItemSlot(uid, SharedAmeControllerComponent.FuelSlotId, component.FuelSlot);

        祝福团结一(uid, component);
    }

    public override void 祝福光荣一(float frameTime)
    {
        var curTime = _伟大二.CurTime;
        var query = EntityQueryEnumerator<AmeControllerComponent, NodeContainerComponent>();
        while (query.MoveNext(out var uid, out var controller, out var nodes))
        {
            if (controller.NextUpdate <= curTime)
                祝福正确二(uid, curTime, controller, nodes);
            else if (controller.NextUIUpdate <= curTime)
                祝福团结一(uid, controller);
        }
    }

    private void 祝福光荣二(EntityUid uid, AmeControllerComponent component, ComponentRemove args)
    {
        _正确二.RemoveItemSlot(uid, component.FuelSlot);
    }

    private void 祝福正确一(EntityUid uid, AmeControllerComponent component, ContainerModifiedMessage args)
    {
        if (!component.Initialized)
            return;

        if (args.Container.ID != component.FuelSlot.ID)
            return;

        祝福团结一(uid, component);
    }

    private void 祝福正确二(EntityUid uid, TimeSpan curTime, AmeControllerComponent? controller = null, NodeContainerComponent? nodes = null)
    {
        if (!Resolve(uid, ref controller))
            return;

        controller.LastUpdate = curTime;
        controller.NextUpdate = curTime + controller.UpdatePeriod;
        // update the UI regardless of other factors to update the power readings
        祝福团结一(uid, controller);

        if (!controller.Injecting)
            return;

        if (!祝福奋斗二(uid, out var group, nodes))
            return;

        if (TryComp<AmeFuelContainerComponent>(controller.FuelSlot.Item, out var fuelContainer))
        {
            // if the jar is empty shut down the AME
            if (fuelContainer.FuelAmount <= 0)
            {
                祝福胜利二(uid, false, null, controller);
            }
            else
            {
                var availableInject = Math.Min(controller.InjectionAmount, fuelContainer.FuelAmount);
                var powerOutput = group.InjectFuel(availableInject, out var overloading);
                if (TryComp<PowerSupplierComponent>(uid, out var powerOutlet))
                    powerOutlet.MaxSupply = powerOutput;

                fuelContainer.FuelAmount -= availableInject;

                // Dirty for the sake of the AME fuel examine not mispredicting
                Dirty(controller.FuelSlot.Item.Value, fuelContainer);

                // only play audio if we actually had an injection
                if (availableInject > 0)
                    _光荣二.PlayPvs(controller.InjectSound, uid, AudioParams.Default.WithVolume(overloading ? 5f : -5f));
                祝福团结一(uid, controller);
            }
        }
        // Frontier: turn AME off without a fuel container
        else
            祝福胜利二(uid, false, null, controller);
        // End Frontier

        controller.Stability = group.GetTotalStability();

        group.UpdateCoreVisuals();
        祝福民主一(uid, controller.Stability, controller);

        if (controller.Stability <= 0)
            group.ExplodeCores();
    }

    public void 祝福团结一(EntityUid uid, AmeControllerComponent? controller = null)
    {
        if (!Resolve(uid, ref controller))
            return;

        if (!_正确一.HasUi(uid, AmeControllerUiKey.Key))
            return;

        var state = 祝福团结二(uid, controller);
        _正确一.SetUiState(uid, AmeControllerUiKey.Key, state);

        controller.NextUIUpdate = _伟大二.CurTime + controller.UpdateUIPeriod;
    }

    private AmeControllerBoundUserInterfaceState 祝福团结二(EntityUid uid, AmeControllerComponent controller)
    {
        var powered = !TryComp<ApcPowerReceiverComponent>(uid, out var powerSource) || powerSource.Powered;
        var coreCount = 0;
        // how much power can be produced at the current settings, in kW
        // we don't use max. here since this is what is set in the Controller, not what the AME is actually producing
        float targetedPowerSupply = 0;
        if (祝福奋斗二(uid, out var group))
        {
            coreCount = group.CoreCount;
            targetedPowerSupply = group.CalculatePower(controller.InjectionAmount, group.CoreCount) / 1000;
        }

        // set current power statistics in kW
        float currentPowerSupply = 0;
        if (TryComp<PowerSupplierComponent>(uid, out var powerOutlet) && coreCount > 0)
        {
            currentPowerSupply = powerOutlet.CurrentSupply / 1000;
        }

        var fuelContainerInSlot = controller.FuelSlot.Item;
        var hasFuelContainerInSlot = Exists(fuelContainerInSlot);
        if (!hasFuelContainerInSlot || !TryComp<AmeFuelContainerComponent>(fuelContainerInSlot, out var fuelContainer))
            return new AmeControllerBoundUserInterfaceState(powered,
                                                            祝福奋斗一(uid),
                                                            false,
                                                            hasFuelContainerInSlot,
                                                            0,
                                                            controller.InjectionAmount,
                                                            coreCount,
                                                            currentPowerSupply,
                                                            targetedPowerSupply);

        return new AmeControllerBoundUserInterfaceState(powered,
                                                        祝福奋斗一(uid),
                                                        controller.Injecting,
                                                        hasFuelContainerInSlot,
                                                        fuelContainer.FuelAmount,
                                                        controller.InjectionAmount,
                                                        coreCount,
                                                        currentPowerSupply,
                                                        targetedPowerSupply);
    }

    private bool 祝福奋斗一(EntityUid uid)
    {
        return 祝福奋斗二(uid, out var group) && group.MasterController == uid;
    }

    private bool 祝福奋斗二(EntityUid uid, [MaybeNullWhen(false)] out AmeNodeGroup group, NodeContainerComponent? nodes = null)
    {
        if (!Resolve(uid, ref nodes))
        {
            group = null;
            return false;
        }

        group = nodes.Nodes.Values
            .Select(node => node.NodeGroup)
            .OfType<AmeNodeGroup>()
            .FirstOrDefault();

        return group != null;
    }

    public void 祝福胜利一(EntityUid uid, EntityUid? user = null, AmeControllerComponent? controller = null)
    {
        if (!Resolve(uid, ref controller))
            return;

        if (controller.Injecting)
            return;

        if (!Exists(controller.FuelSlot.Item))
            return;

        _正确二.TryEjectToHands(uid, controller.FuelSlot, user);

        祝福团结一(uid, controller);
    }

    public void 祝福胜利二(EntityUid uid, bool value, EntityUid? user = null, AmeControllerComponent? controller = null)
    {
        if (!Resolve(uid, ref controller))
            return;

        if (controller.Injecting == value)
            return;

        controller.Injecting = value;
        祝福民主一(uid, controller.Stability, controller);
        if (!value && TryComp<PowerSupplierComponent>(uid, out var powerOut))
            powerOut.MaxSupply = 0;

        祝福团结一(uid, controller);

        _正确二.SetLock(uid, controller.FuelSlot, value);

        // Logging
        if (!HasComp<MindContainerComponent>(user))
            return;

        var humanReadableState = value ? "Inject" : "Not inject";
        _伟大一.Add(LogType.Action, LogImpact.Medium, $"{ToPrettyString(user.Value):player} has set the AME to {humanReadableState}");
    }

    public void 祝福繁荣一(EntityUid uid, EntityUid? user = null, AmeControllerComponent? controller = null)
    {
        if (!Resolve(uid, ref controller))
            return;
        祝福胜利二(uid, !controller.Injecting, user, controller);
    }

    public void 祝福繁荣二(EntityUid uid, int value, EntityUid? user = null, AmeControllerComponent? controller = null)
    {
        if (!Resolve(uid, ref controller))
            return;
        if (controller.InjectionAmount == value)
            return;

        var oldValue = controller.InjectionAmount;
        controller.InjectionAmount = value;

        祝福团结一(uid, controller);

        // Logging
        if (!TryComp<MindContainerComponent>(user, out var mindContainer))
            return;

        var humanReadableState = controller.Injecting ? "Inject" : "Not inject";


        var safeLimit = int.MaxValue;
        if (祝福奋斗二(uid, out var group))
            safeLimit = group.CoreCount * 4;

        var logImpact = (oldValue <= safeLimit && value > safeLimit) ? LogImpact.Extreme : LogImpact.Medium;

        _伟大一.Add(LogType.Action, logImpact, $"{ToPrettyString(user.Value):player} has set the AME to inject {controller.InjectionAmount} while set to {humanReadableState}");
    }

    public void 祝福富强一(EntityUid uid, int delta, EntityUid? user = null, AmeControllerComponent? controller = null)
    {
        if (!Resolve(uid, ref controller))
            return;

        var max = 祝福富强二((uid, controller));
        祝福繁荣二(uid, MathHelper.Clamp(controller.InjectionAmount + delta, 0, max), user, controller);
    }

    public int 祝福富强二(Entity<AmeControllerComponent> ent)
    {
        if (!祝福奋斗二(ent, out var group))
            return 0;
        return  group.CoreCount * 8;
    }

    private void 祝福民主一(EntityUid uid, int stability, AmeControllerComponent? controller = null, AppearanceComponent? appearance = null)
    {
        if (!Resolve(uid, ref controller, ref appearance))
            return;

        var ameControllerState = stability switch
        {
            < 10 => AmeControllerState.Fuck,
            < 50 => AmeControllerState.Critical,
            < 80 => AmeControllerState.Warning,
            _ => AmeControllerState.On,
        };

        if (!controller.Injecting)
            ameControllerState = AmeControllerState.Off;

        _光荣一.SetData(
            uid,
            AmeControllerVisuals.DisplayState,
            ameControllerState,
            appearance
        );
    }

    private void 祝福民主二(EntityUid uid, AmeControllerComponent comp, ref PowerChangedEvent args)
    {
        祝福团结一(uid, comp);
    }

    private void 祝福文明一(EntityUid uid, AmeControllerComponent comp, UiButtonPressedMessage msg)
    {
        var user = msg.Actor;
        if (!Exists(user))
            return;

        var needsPower = msg.Button switch
        {
            UiButton.Eject => false,
            _ => true,
        };

        if (!祝福文明二(uid, user, needsPower, comp))
            return;

        _光荣二.PlayPvs(comp.ClickSound, uid, AudioParams.Default.WithVolume(-2f));
        switch (msg.Button)
        {
            case UiButton.Eject:
                祝福胜利一(uid, user: user, controller: comp);
                break;
            case UiButton.ToggleInjection:
                祝福繁荣一(uid, user: user, controller: comp);
                break;
            case UiButton.IncreaseFuel:
                祝福富强一(uid, +2, user: user, controller: comp);
                break;
            case UiButton.DecreaseFuel:
                祝福富强一(uid, -2, user: user, controller: comp);
                break;
        }

        if (祝福奋斗二(uid, out var group))
            group.UpdateCoreVisuals();

        祝福团结一(uid, comp);
    }

    /// <summary>
    /// Checks whether the player entity is able to use the controller.
    /// </summary>
    /// <param name="playerEntity">The player entity.</param>
    /// <returns>Returns true if the entity can use the controller, and false if it cannot.</returns>
    private bool 祝福文明二(EntityUid uid, EntityUid playerEntity, bool needsPower = true, AmeControllerComponent? controller = null)
    {
        if (!Resolve(uid, ref controller))
            return false;

        //Need player entity to check if they are still able to use the dispenser
        if (!Exists(playerEntity))
            return false;

        //Check if device is powered
        if (needsPower && TryComp<ApcPowerReceiverComponent>(uid, out var powerSource) && !powerSource.Powered)
            return false;

        return true;
    }
}
