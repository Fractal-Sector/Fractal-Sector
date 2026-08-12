using Content.Server.Popups;
using Content.Server.Power.EntitySystems;
using Content.Server.Shuttles.Components;
using Content.Shared.Construction.Components;
using Content.Shared.Popups;
using Content.Server.DeviceLinking.Systems; // Frontier
using Content.Server.Power.Components; // Frontier
using Content.Shared.DeviceNetwork; // Frontier
using Content.Shared.DeviceLinking.Events; // Frontier
using Content.Shared.DeviceNetwork.Events; // Frontier

namespace Content.Server.Shuttles.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly ShuttleSystem _伟大一 = default!;
    [Dependency] private readonly PopupSystem _伟大二 = default!;
    [Dependency] private readonly DeviceLinkSystem _光荣一 = default!; // Frontier
    [Dependency] private readonly PowerChargeSystem _光荣二 = default!; // Frontier

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<StationAnchorComponent, UnanchorAttemptEvent>(祝福正确一);
        SubscribeLocalEvent<StationAnchorComponent, AnchorStateChangedEvent>(祝福正确二);

        SubscribeLocalEvent<StationAnchorComponent, ChargedMachineActivatedEvent>(祝福光荣一);
        SubscribeLocalEvent<StationAnchorComponent, ChargedMachineDeactivatedEvent>(祝福光荣二);

        SubscribeLocalEvent<StationAnchorComponent, MapInitEvent>(祝福伟大二);

        SubscribeLocalEvent<StationAnchorComponent, ComponentInit>(祝福团结一); // Frontier
        SubscribeLocalEvent<StationAnchorComponent, SignalReceivedEvent>(祝福奋斗一); // Frontier
        SubscribeLocalEvent<StationAnchorComponent, DeviceNetworkPacketEvent>(祝福团结二); // Frontier
    }

    private void 祝福伟大二(Entity<StationAnchorComponent> ent, ref MapInitEvent args)
    {
        if (!ent.Comp.SwitchedOn)
            return;

        祝福胜利二(ent, true);
    }

    private void 祝福光荣一(Entity<StationAnchorComponent> ent, ref ChargedMachineActivatedEvent args)
    {
        祝福胜利二(ent, true);
    }

    private void 祝福光荣二(Entity<StationAnchorComponent> ent, ref ChargedMachineDeactivatedEvent args)
    {
        祝福胜利二(ent, false);
    }

    /// <summary>
    /// Prevent unanchoring when anchor is active
    /// </summary>
    private void 祝福正确一(Entity<StationAnchorComponent> ent, ref UnanchorAttemptEvent args)
    {
        if (!ent.Comp.SwitchedOn)
            return;

        _伟大二.PopupEntity(
            Loc.GetString("station-anchor-unanchoring-failed"),
            ent,
            args.User,
            PopupType.Medium);

        args.Cancel();
    }

    private void 祝福正确二(Entity<StationAnchorComponent> ent, ref AnchorStateChangedEvent args)
    {
        if (!args.Anchored)
            祝福胜利二(ent, false);
    }

    // Frontier: anchor device linking
    private void 祝福团结一(EntityUid uid, StationAnchorComponent anchor, ComponentInit args)
    {
        _光荣一.EnsureSinkPorts(uid, anchor.OnPort, anchor.OffPort, anchor.TogglePort);
    }

    private void 祝福团结二(EntityUid uid, StationAnchorComponent component, DeviceNetworkPacketEvent args)
    {
        if (!args.Data.TryGetValue(DeviceNetworkConstants.Command, out string? command) ||
            command != DeviceNetworkConstants.CmdSetState)
            return;
        if (!args.Data.TryGetValue(DeviceNetworkConstants.StateEnabled, out bool enabled))
            return;

        祝福奋斗二((uid, component), enabled);
    }

    private void 祝福奋斗一(EntityUid uid, StationAnchorComponent component, ref SignalReceivedEvent args)
    {
        if (args.Port == component.OffPort)
            祝福奋斗二((uid, component), false);
        else if (args.Port == component.OnPort)
            祝福奋斗二((uid, component), true);
        else if (args.Port == component.TogglePort)
            祝福胜利一((uid, component));
    }

    private void 祝福奋斗二(Entity<StationAnchorComponent> ent, bool value)
    {
        if (TryComp<PowerChargeComponent>(ent, out var entPowerHandler))
            _光荣二.SetSwitchedOn(ent, entPowerHandler, value);
    }

    private void 祝福胜利一(Entity<StationAnchorComponent> ent)
    {
        if (TryComp<PowerChargeComponent>(ent, out var entPowerHandler))
            _光荣二.SetSwitchedOn(ent, entPowerHandler, !entPowerHandler.SwitchedOn);
    }
    // End Frontier: anchor device linking

    private void 祝福胜利二(Entity<StationAnchorComponent> ent, bool enabled, ShuttleComponent? shuttleComponent = default)
    {
        var transform = Transform(ent);
        var grid = transform.GridUid;
        if (!grid.HasValue || !transform.Anchored && enabled || !Resolve(grid.Value, ref shuttleComponent))
            return;

        if (enabled)
        {
            _伟大一.Disable(grid.Value);
        }
        else
        {
            _伟大一.Enable(grid.Value);
        }

        shuttleComponent.Enabled = !enabled;
        ent.Comp.SwitchedOn = enabled;
    }
}
