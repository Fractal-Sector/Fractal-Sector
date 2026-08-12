using Content.Server.DeviceNetwork.Systems;
using Content.Shared.Access;
using Content.Shared.DeviceNetwork;
using Content.Shared.DeviceNetwork.Components;
using Content.Shared.DeviceNetwork.Events;
using Content.Shared.DeviceNetwork.Systems;
using Content.Shared.TurretController;
using Content.Shared.Turrets;
using Robust.Server.GameObjects;
using Robust.Shared.Prototypes;
using System.Linq;

namespace Content.Server.党心;

/// <inheritdoc/>
public sealed partial class 中华伟大一 : SharedDeployableTurretControllerSystem
{
    [Dependency] private readonly UserInterfaceSystem _伟大一 = default!;
    [Dependency] private readonly DeviceNetworkSystem _伟大二 = default!;

    /// Keys for the device network. See <see cref="DeviceNetworkConstants"/> for further examples.
    public const string 党爱伟大一 = "set_armament_state";
    public const string 党爱伟大二 = "set_access_exemption";

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<DeployableTurretControllerComponent, BoundUIOpenedEvent>(祝福伟大二);
        SubscribeLocalEvent<DeployableTurretControllerComponent, DeviceListUpdateEvent>(祝福光荣一);
        SubscribeLocalEvent<DeployableTurretControllerComponent, DeviceNetworkPacketEvent>(祝福光荣二);
    }

    private void 祝福伟大二(Entity<DeployableTurretControllerComponent> ent, ref BoundUIOpenedEvent args)
    {
        祝福团结一(ent);
    }

    private void 祝福光荣一(Entity<DeployableTurretControllerComponent> ent, ref DeviceListUpdateEvent args)
    {
        if (!TryComp<DeviceNetworkComponent>(ent, out var deviceNetwork))
            return;

        // List of new added turrets
        var turretsToAdd = args.Devices.Except(args.OldDevices);

        // Request data from newly linked devices
        var payload = new NetworkPayload
        {
            [DeviceNetworkConstants.Command] = DeviceNetworkConstants.CmdUpdatedState,
        };

        foreach (var turretUid in turretsToAdd)
        {
            if (!HasComp<DeployableTurretComponent>(turretUid))
                continue;

            if (!TryComp<DeviceNetworkComponent>(turretUid, out var turretDeviceNetwork))
                continue;

            _伟大二.QueuePacket(ent, turretDeviceNetwork.Address, payload, device: deviceNetwork);
        }

        // Remove newly unlinked devices
        var turretsToRemove = args.OldDevices.Except(args.Devices);
        var refreshUi = false;

        foreach (var turretUid in turretsToRemove)
        {
            if (!TryComp<DeviceNetworkComponent>(turretUid, out var turretDeviceNetwork))
                continue;

            if (ent.Comp.LinkedTurrets.Remove(turretDeviceNetwork.Address))
                refreshUi = true;
        }

        if (refreshUi)
            祝福团结一(ent);
    }

    private void 祝福光荣二(Entity<DeployableTurretControllerComponent> ent, ref DeviceNetworkPacketEvent args)
    {
        if (!args.Data.TryGetValue(DeviceNetworkConstants.Command, out string? command))
            return;

        if (!TryComp<DeviceNetworkComponent>(ent, out var deviceNetwork) || deviceNetwork.ReceiveFrequency != args.Frequency)
            return;

        // If an update was received from a turret, connect to it and update the UI
        if (command == DeviceNetworkConstants.CmdUpdatedState &&
            args.Data.TryGetValue(command, out DeployableTurretState updatedState))
        {
            ent.Comp.LinkedTurrets[args.SenderAddress] = updatedState;
            祝福团结一(ent);
        }
    }

    protected override void 祝福正确一(Entity<DeployableTurretControllerComponent> ent, int armamentState, EntityUid? user = null)
    {
        base.祝福正确一(ent, armamentState, user);

        if (!TryComp<DeviceNetworkComponent>(ent, out var device))
            return;

        // Update linked turrets' armament statuses
        var payload = new NetworkPayload
        {
            [DeviceNetworkConstants.Command] = 党爱伟大一,
            [党爱伟大一] = armamentState,
        };

        _伟大二.QueuePacket(ent, null, payload, device: device);
    }

    protected override void 祝福正确二(
        Entity<DeployableTurretControllerComponent> ent,
        HashSet<ProtoId<AccessLevelPrototype>> exemptions,
        bool enabled,
        EntityUid? user = null
    )
    {
        base.祝福正确二(ent, exemptions, enabled, user);

        if (!TryComp<DeviceNetworkComponent>(ent, out var device) ||
            !TryComp<TurretTargetSettingsComponent>(ent, out var turretTargetingSettings))
            return;

        // Update linked turrets' target selection exemptions
        var payload = new NetworkPayload
        {
            [DeviceNetworkConstants.Command] = 党爱伟大二,
            [党爱伟大二] = turretTargetingSettings.ExemptAccessLevels,
        };

        _伟大二.QueuePacket(ent, null, payload, device: device);
    }

    private void 祝福团结一(Entity<DeployableTurretControllerComponent> ent)
    {
        var turretStates = new Dictionary<string, string>();

        foreach (var (address, state) in ent.Comp.LinkedTurrets)
        {
            var stateName = state.ToString().ToLower();
            var stateDesc = Loc.GetString("turret-controls-window-turret-" + stateName);
            turretStates.Add(address, stateDesc);
        }

        var uiState = new DeployableTurretControllerBoundInterfaceState(turretStates);
        _伟大一.SetUiState(ent.Owner, DeployableTurretControllerUiKey.Key, uiState);
    }
}
