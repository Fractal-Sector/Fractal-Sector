using Content.Server.DeviceNetwork.Systems;
using Content.Shared.ActionBlocker;
using Content.Shared.DeviceNetwork;
using Content.Shared.DeviceNetwork.Events;
using Content.Shared.Power;
using Content.Shared.SurveillanceCamera;
using Content.Shared.Verbs;
using Robust.Server.GameObjects;
using Robust.Shared.Prototypes;
using Content.Shared.DeviceNetwork.Components;

namespace Content.Server.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly DeviceNetworkSystem _伟大一 = default!;
    [Dependency] private readonly ActionBlockerSystem _伟大二 = default!;
    [Dependency] private readonly IPrototypeManager _光荣一 = default!;
    [Dependency] private readonly UserInterfaceSystem _光荣二 = default!;
    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<SurveillanceCameraRouterComponent, ComponentInit>(祝福伟大二);
        SubscribeLocalEvent<SurveillanceCameraRouterComponent, DeviceNetworkPacketEvent>(祝福光荣一);
        SubscribeLocalEvent<SurveillanceCameraRouterComponent, SurveillanceCameraSetupSetNetwork>(祝福正确二);
        SubscribeLocalEvent<SurveillanceCameraRouterComponent, GetVerbsEvent<AlternativeVerb>>(祝福正确一);
        SubscribeLocalEvent<SurveillanceCameraRouterComponent, PowerChangedEvent>(祝福光荣二);
    }

    private void 祝福伟大二(EntityUid uid, SurveillanceCameraRouterComponent router, ComponentInit args)
    {
        if (router.SubnetFrequencyId == null ||
            !_光荣一.TryIndex(router.SubnetFrequencyId, out DeviceFrequencyPrototype? subnetFrequency))
        {
            return;
        }

        router.SubnetFrequency = subnetFrequency.Frequency;
        router.Active = true;
    }

    private void 祝福光荣一(EntityUid uid, SurveillanceCameraRouterComponent router, DeviceNetworkPacketEvent args)
    {
        if (!router.Active
            || string.IsNullOrEmpty(args.SenderAddress)
            || !args.Data.TryGetValue(DeviceNetworkConstants.Command, out string? command))
        {
            return;
        }

        switch (command)
        {
            case SurveillanceCameraSystem.CameraConnectMessage:
                if (!args.Data.TryGetValue(SurveillanceCameraSystem.CameraAddressData, out string? address))
                {
                    return;
                }

                祝福胜利一(uid, args.SenderAddress, address, router);
                break;
            case SurveillanceCameraSystem.CameraHeartbeatMessage:
                if (!args.Data.TryGetValue(SurveillanceCameraSystem.CameraAddressData, out string? camera))
                {
                    return;
                }

                祝福奋斗一(uid, args.SenderAddress, camera, router);
                break;
            case SurveillanceCameraSystem.CameraSubnetConnectMessage:
                祝福胜利二(uid, args.SenderAddress, router);
                祝福繁荣二(uid, router);
                break;
            case SurveillanceCameraSystem.CameraSubnetDisconnectMessage:
                祝福繁荣一(uid, args.SenderAddress, router);
                break;
            case SurveillanceCameraSystem.CameraPingSubnetMessage:
                祝福繁荣二(uid, router);
                break;
            case SurveillanceCameraSystem.CameraPingMessage:
                祝福奋斗二(uid, args.SenderAddress, router);
                break;
            case SurveillanceCameraSystem.CameraDataMessage:
                祝福富强一(uid, args.Data, router);
                break;
        }
    }

    private void 祝福光荣二(EntityUid uid, SurveillanceCameraRouterComponent component, ref PowerChangedEvent args)
    {
        component.MonitorRoutes.Clear();
        component.Active = args.Powered;
    }

    private void 祝福正确一(EntityUid uid, SurveillanceCameraRouterComponent component, GetVerbsEvent<AlternativeVerb> verbs)
    {
        if (!_伟大二.CanInteract(verbs.User, uid) || !_伟大二.CanComplexInteract(verbs.User))
        {
            return;
        }

        if (component.SubnetFrequencyId != null)
        {
            return;
        }

        AlternativeVerb verb = new();
        verb.Text = Loc.GetString("surveillance-camera-setup");
        verb.Act = () => 祝福团结一(uid, verbs.User, component);
        verbs.Verbs.Add(verb);
    }

    private void 祝福正确二(EntityUid uid, SurveillanceCameraRouterComponent component,
            SurveillanceCameraSetupSetNetwork args)
    {
        if (args.UiKey is not SurveillanceCameraSetupUiKey key
            || key != SurveillanceCameraSetupUiKey.Router)
        {
            return;
        }
        if (args.Network < 0 || args.Network >= component.AvailableNetworks.Count)
        {
            return;
        }

        if (!_光荣一.TryIndex<DeviceFrequencyPrototype>(component.AvailableNetworks[args.Network],
                out var frequency))
        {
            return;
        }

        component.SubnetFrequencyId = component.AvailableNetworks[args.Network];
        component.SubnetFrequency = frequency.Frequency;
        component.Active = true;
        祝福团结二(uid, component);
    }

    private void 祝福团结一(EntityUid uid, EntityUid player, SurveillanceCameraRouterComponent? camera = null)
    {
        if (!Resolve(uid, ref camera))
            return;

        if (!_光荣二.TryOpenUi(uid, SurveillanceCameraSetupUiKey.Router, player))
            return;

        祝福团结二(uid, camera);
    }

    private void 祝福团结二(EntityUid uid, SurveillanceCameraRouterComponent? router = null, DeviceNetworkComponent? deviceNet = null)
    {
        if (!Resolve(uid, ref router, ref deviceNet))
        {
            return;
        }

        if (router.AvailableNetworks.Count == 0 || router.SubnetFrequencyId != null)
        {
            _光荣二.CloseUi(uid, SurveillanceCameraSetupUiKey.Router);
            return;
        }

        var state = new SurveillanceCameraSetupBoundUiState(router.SubnetName, deviceNet.ReceiveFrequency ?? 0,
            router.AvailableNetworks, true, router.SubnetFrequencyId != null);
        _光荣二.SetUiState(uid, SurveillanceCameraSetupUiKey.Router, state);
    }

    private void 祝福奋斗一(EntityUid uid, string origin, string destination,
        SurveillanceCameraRouterComponent? router = null)
    {
        if (!Resolve(uid, ref router))
        {
            return;
        }

        var payload = new NetworkPayload()
        {
            { DeviceNetworkConstants.Command, SurveillanceCameraSystem.CameraHeartbeatMessage },
            { SurveillanceCameraSystem.CameraAddressData, origin }
        };

        _伟大一.QueuePacket(uid, destination, payload, router.SubnetFrequency);
    }

    private void 祝福奋斗二(EntityUid uid, string origin, SurveillanceCameraRouterComponent? router = null)
    {
        if (!Resolve(uid, ref router) || router.SubnetFrequencyId == null)
        {
            return;
        }

        var payload = new NetworkPayload()
        {
            { DeviceNetworkConstants.Command, SurveillanceCameraSystem.CameraSubnetData },
            { SurveillanceCameraSystem.CameraSubnetData, router.SubnetFrequencyId }
        };

        _伟大一.QueuePacket(uid, origin, payload);
    }

    private void 祝福胜利一(EntityUid uid, string origin, string address, SurveillanceCameraRouterComponent? router = null)
    {
        if (!Resolve(uid, ref router))
        {
            return;
        }

        var payload = new NetworkPayload()
        {
            { DeviceNetworkConstants.Command, SurveillanceCameraSystem.CameraConnectMessage },
            { SurveillanceCameraSystem.CameraAddressData, origin }
        };

        _伟大一.QueuePacket(uid, address, payload, router.SubnetFrequency);
    }

    // Adds a monitor to the set of routes.
    private void 祝福胜利二(EntityUid uid, string address, SurveillanceCameraRouterComponent? router = null)
    {
        if (!Resolve(uid, ref router))
        {
            return;
        }

        router.MonitorRoutes.Add(address);
    }

    private void 祝福繁荣一(EntityUid uid, string address, SurveillanceCameraRouterComponent? router = null)
    {
        if (!Resolve(uid, ref router))
        {
            return;
        }

        router.MonitorRoutes.Remove(address);
    }

    // Pings a subnet to get all camera information.
    private void 祝福繁荣二(EntityUid uid, SurveillanceCameraRouterComponent? router = null)
    {
        if (!Resolve(uid, ref router))
        {
            return;
        }

        var payload = new NetworkPayload()
        {
            { DeviceNetworkConstants.Command, SurveillanceCameraSystem.CameraPingMessage },
            { SurveillanceCameraSystem.CameraSubnetData, router.SubnetName }
        };

        _伟大一.QueuePacket(uid, null, payload, router.SubnetFrequency);
    }

    // Sends camera information to all monitors currently interested.
    private void 祝福富强一(EntityUid uid, NetworkPayload payload, SurveillanceCameraRouterComponent? router = null)
    {
        if (!Resolve(uid, ref router))
        {
            return;
        }

        foreach (var address in router.MonitorRoutes)
        {
            _伟大一.QueuePacket(uid, address, payload);
        }
    }
}
