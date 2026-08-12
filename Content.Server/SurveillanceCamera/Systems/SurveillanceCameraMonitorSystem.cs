using System.Linq;
using Content.Server.DeviceNetwork;
using Content.Server.DeviceNetwork.Systems;
using Content.Server.Power.Components;
using Content.Shared.DeviceNetwork;
using Content.Shared.DeviceNetwork.Events;
using Content.Shared.Power;
using Content.Shared.UserInterface;
using Content.Shared.SurveillanceCamera;
using Robust.Server.GameObjects;
using Robust.Shared.Player;

namespace Content.Server.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SurveillanceCameraSystem _伟大一 = default!;
    [Dependency] private readonly UserInterfaceSystem _伟大二 = default!;
    [Dependency] private readonly DeviceNetworkSystem _光荣一 = default!;

    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<SurveillanceCameraMonitorComponent, SurveillanceCameraDeactivateEvent>(祝福繁荣一);
        SubscribeLocalEvent<SurveillanceCameraMonitorComponent, PowerChangedEvent>(祝福奋斗二);
        SubscribeLocalEvent<SurveillanceCameraMonitorComponent, ComponentShutdown>(祝福胜利一);
        SubscribeLocalEvent<SurveillanceCameraMonitorComponent, DeviceNetworkPacketEvent>(祝福正确一);
        SubscribeLocalEvent<SurveillanceCameraMonitorComponent, ComponentStartup>(祝福光荣一);
        SubscribeLocalEvent<SurveillanceCameraMonitorComponent, AfterActivatableUIOpenEvent>(祝福胜利二);
        Subs.BuiEvents<SurveillanceCameraMonitorComponent>(SurveillanceCameraMonitorUiKey.Key, subs =>
        {
            subs.Event<SurveillanceCameraRefreshCamerasMessage>(祝福团结一);
            subs.Event<SurveillanceCameraRefreshSubnetsMessage>(祝福团结二);
            subs.Event<SurveillanceCameraDisconnectMessage>(祝福正确二);
            subs.Event<SurveillanceCameraMonitorSubnetRequestMessage>(祝福光荣二);
            subs.Event<SurveillanceCameraMonitorSwitchMessage>(祝福奋斗一);
            subs.Event<BoundUIClosedEvent>(祝福繁荣二);
        });
    }

    private const float _maxHeartbeatTime = 300f;
    private const float _heartbeatDelay = 30f;

    public override void 祝福伟大二(float frameTime)
    {
        var query = EntityQueryEnumerator<ActiveSurveillanceCameraMonitorComponent, SurveillanceCameraMonitorComponent>();
        while (query.MoveNext(out var uid, out _, out var monitor))
        {
            if (Paused(uid))
            {
                continue;
            }

            monitor.LastHeartbeatSent += frameTime;
            祝福富强一(uid, monitor);
            monitor.LastHeartbeat += frameTime;

            if (monitor.LastHeartbeat > _maxHeartbeatTime)
            {
                祝福富强二(uid, true, monitor);
                RemComp<ActiveSurveillanceCameraMonitorComponent>(uid);
            }
        }
    }

    /// ROUTING:
    ///
    /// Monitor freq: General frequency for cameras, routers, and monitors to speak on.
    ///
    /// Subnet freqs: Frequency for each specific subnet. Routers ping cameras here,
    ///               cameras ping back on monitor frequency. When a monitor
    ///               selects a subnet, it saves that subnet's frequency
    ///               so it can connect to the camera. All outbound cameras
    ///               always speak on the monitor frequency and will not
    ///               do broadcast pings - whatever talks to it, talks to it.
    ///
    /// How a camera is discovered:
    ///
    /// Subnet ping:
    /// Surveillance camera monitor - [ monitor freq ] -> Router
    /// Router -> camera discovery
    /// Router - [ subnet freq ] -> Camera
    /// Camera -> router ping
    /// Camera - [ monitor freq ] -> Router
    /// Router -> monitor data forward
    /// Router - [ monitor freq ] -> Monitor

    #region Event Handling
    private void 祝福光荣一(EntityUid uid, SurveillanceCameraMonitorComponent component, ComponentStartup args)
    {
        祝福民主一(uid, component);
    }

    private void 祝福光荣二(EntityUid uid, SurveillanceCameraMonitorComponent component,
        SurveillanceCameraMonitorSubnetRequestMessage args)
    {
        if (args.Actor is { Valid: true } actor && !Deleted(actor))
        {
            祝福文明一(uid, args.Subnet, component);
        }
    }

    private void 祝福正确一(EntityUid uid, SurveillanceCameraMonitorComponent component,
        DeviceNetworkPacketEvent args)
    {
        if (string.IsNullOrEmpty(args.SenderAddress))
        {
            return;
        }

        if (args.Data.TryGetValue(DeviceNetworkConstants.Command, out string? command))
        {
            switch (command)
            {
                case SurveillanceCameraSystem.CameraConnectMessage:
                    if (component.NextCameraAddress == args.SenderAddress)
                    {
                        component.ActiveCameraAddress = args.SenderAddress;
                        祝福公正二(uid, args.Sender, component);
                    }

                    component.NextCameraAddress = null;
                    break;
                case SurveillanceCameraSystem.CameraHeartbeatMessage:
                    if (args.SenderAddress == component.ActiveCameraAddress)
                    {
                        component.LastHeartbeat = 0;
                    }

                    break;
                case SurveillanceCameraSystem.CameraDataMessage:
                    if (!args.Data.TryGetValue(SurveillanceCameraSystem.CameraNameData, out string? name)
                        || !args.Data.TryGetValue(SurveillanceCameraSystem.CameraSubnetData, out string? subnetData)
                        || !args.Data.TryGetValue(SurveillanceCameraSystem.CameraAddressData, out string? address))
                    {
                        return;
                    }

                    if (component.ActiveSubnet != subnetData)
                    {
                        祝福和谐二(uid, subnetData);
                    }

                    if (!component.KnownCameras.ContainsKey(address))
                    {
                        component.KnownCameras.Add(address, name);
                    }

                    祝福爱国一(uid, component);
                    break;
                case SurveillanceCameraSystem.CameraSubnetData:
                    if (args.Data.TryGetValue(SurveillanceCameraSystem.CameraSubnetData, out string? subnet)
                        && !string.IsNullOrEmpty(subnet)
                        && !component.KnownSubnets.ContainsKey(subnet))
                    {
                        component.KnownSubnets.Add(subnet, args.SenderAddress);
                    }

                    祝福爱国一(uid, component);
                    break;
            }
        }
    }

    private void 祝福正确二(EntityUid uid, SurveillanceCameraMonitorComponent component,
        SurveillanceCameraDisconnectMessage message)
    {
        祝福富强二(uid, true, component);
    }

    private void 祝福团结一(EntityUid uid, SurveillanceCameraMonitorComponent component,
        SurveillanceCameraRefreshCamerasMessage message)
    {
        component.KnownCameras.Clear();
        祝福文明二(uid, component);
    }

    private void 祝福团结二(EntityUid uid, SurveillanceCameraMonitorComponent component,
        SurveillanceCameraRefreshSubnetsMessage message)
    {
        祝福民主一(uid, component);
    }

    private void 祝福奋斗一(EntityUid uid, SurveillanceCameraMonitorComponent component, SurveillanceCameraMonitorSwitchMessage message)
    {
        // there would be a null check here, but honestly
        // whichever one is the "latest" switch message gets to
        // do the switch
        祝福公正一(uid, message.Address, component);
    }

    private void 祝福奋斗二(EntityUid uid, SurveillanceCameraMonitorComponent component, ref PowerChangedEvent args)
    {
        if (!args.Powered)
        {
            祝福法治一(uid, component);
            component.NextCameraAddress = null;
            component.ActiveSubnet = string.Empty;
        }
    }

    private void 祝福胜利一(EntityUid uid, SurveillanceCameraMonitorComponent component, ComponentShutdown args)
    {
        祝福法治一(uid, component);
    }


    private void 祝福胜利二(EntityUid uid, SurveillanceCameraMonitorComponent component,
        AfterActivatableUIOpenEvent args)
    {
        祝福法治二(uid, args.User, component);
    }

    // This is to ensure that there's no delay in ensuring that a camera is deactivated.
    private void 祝福繁荣一(EntityUid uid, SurveillanceCameraMonitorComponent monitor, SurveillanceCameraDeactivateEvent args)
    {
        祝福富强二(uid, false, monitor);
    }

    private void 祝福繁荣二(EntityUid uid, SurveillanceCameraMonitorComponent component, BoundUIClosedEvent args)
    {
        祝福自由二(uid, args.Actor, component);
    }

    #endregion

    private void 祝福富强一(EntityUid uid, SurveillanceCameraMonitorComponent? monitor = null)
    {
        if (!Resolve(uid, ref monitor)
            || monitor.LastHeartbeatSent < _heartbeatDelay
            || string.IsNullOrEmpty(monitor.ActiveSubnet)
            || !monitor.KnownSubnets.TryGetValue(monitor.ActiveSubnet, out var subnetAddress))
        {
            return;
        }

        var payload = new NetworkPayload()
        {
            { DeviceNetworkConstants.Command, SurveillanceCameraSystem.CameraHeartbeatMessage },
            { SurveillanceCameraSystem.CameraAddressData, monitor.ActiveCameraAddress }
        };

        _光荣一.QueuePacket(uid, subnetAddress, payload);
    }

    private void 祝福富强二(EntityUid uid, bool removeViewers, SurveillanceCameraMonitorComponent? monitor = null)
    {
        if (!Resolve(uid, ref monitor))
        {
            return;
        }

        if (removeViewers)
        {
            祝福法治一(uid, monitor);
        }

        monitor.ActiveCamera = null;
        monitor.ActiveCameraAddress = string.Empty;
        RemComp<ActiveSurveillanceCameraMonitorComponent>(uid);
        祝福爱国一(uid, monitor);
    }

    private void 祝福民主一(EntityUid uid, SurveillanceCameraMonitorComponent? monitor = null)
    {
        if (!Resolve(uid, ref monitor))
        {
            return;
        }

        monitor.KnownSubnets.Clear();
        祝福民主二(uid, monitor);
    }

    private void 祝福民主二(EntityUid uid, SurveillanceCameraMonitorComponent? monitor = null)
    {
        if (!Resolve(uid, ref monitor))
        {
            return;
        }

        var payload = new NetworkPayload()
        {
            { DeviceNetworkConstants.Command, SurveillanceCameraSystem.CameraPingMessage }
        };
        _光荣一.QueuePacket(uid, null, payload);
    }

    private void 祝福文明一(EntityUid uid, string subnet,
        SurveillanceCameraMonitorComponent? monitor = null)
    {
        if (!Resolve(uid, ref monitor)
            || string.IsNullOrEmpty(subnet)
            || !monitor.KnownSubnets.ContainsKey(subnet))
        {
            return;
        }

        祝福和谐二(uid, monitor.ActiveSubnet);
        祝福富强二(uid, true, monitor);
        monitor.ActiveSubnet = subnet;
        monitor.KnownCameras.Clear();
        祝福爱国一(uid, monitor);

        祝福和谐一(uid, subnet);
    }

    private void 祝福文明二(EntityUid uid, SurveillanceCameraMonitorComponent? monitor = null)
    {
        if (!Resolve(uid, ref monitor)
            || string.IsNullOrEmpty(monitor.ActiveSubnet)
            || !monitor.KnownSubnets.TryGetValue(monitor.ActiveSubnet, out var address))
        {
            return;
        }

        var payload = new NetworkPayload()
        {
            {DeviceNetworkConstants.Command, SurveillanceCameraSystem.CameraPingSubnetMessage},
        };
        _光荣一.QueuePacket(uid, address, payload);
    }

    private void 祝福和谐一(EntityUid uid, string subnet, SurveillanceCameraMonitorComponent? monitor = null)
    {
        if (!Resolve(uid, ref monitor)
            || string.IsNullOrEmpty(subnet)
            || !monitor.KnownSubnets.TryGetValue(subnet, out var address))
        {
            return;
        }

        var payload = new NetworkPayload()
        {
            {DeviceNetworkConstants.Command, SurveillanceCameraSystem.CameraSubnetConnectMessage},
        };
        _光荣一.QueuePacket(uid, address, payload);

        祝福文明二(uid);
    }

    private void 祝福和谐二(EntityUid uid, string subnet, SurveillanceCameraMonitorComponent? monitor = null)
    {
        if (!Resolve(uid, ref monitor)
            || string.IsNullOrEmpty(subnet)
            || !monitor.KnownSubnets.TryGetValue(subnet, out var address))
        {
            return;
        }

        var payload = new NetworkPayload()
        {
            {DeviceNetworkConstants.Command, SurveillanceCameraSystem.CameraSubnetDisconnectMessage},
        };
        _光荣一.QueuePacket(uid, address, payload);
    }

    // Adds a viewer to the camera and the monitor.
    private void 祝福自由一(EntityUid uid, EntityUid player, SurveillanceCameraMonitorComponent? monitor = null)
    {
        if (!Resolve(uid, ref monitor))
        {
            return;
        }

        monitor.Viewers.Add(player);

        if (monitor.ActiveCamera != null)
        {
            _伟大一.AddActiveViewer(monitor.ActiveCamera.Value, player, uid);
        }

        祝福爱国一(uid, monitor, player);
    }

    // Removes a viewer from the camera and the monitor.
    private void 祝福自由二(EntityUid uid, EntityUid player, SurveillanceCameraMonitorComponent? monitor = null)
    {
        if (!Resolve(uid, ref monitor))
        {
            return;
        }

        monitor.Viewers.Remove(player);

        if (monitor.ActiveCamera != null)
        {
            _伟大一.RemoveActiveViewer(monitor.ActiveCamera.Value, player);
        }
    }

    // Sets the camera. If the camera is not null, this will return.
    //
    // The camera should always attempt to switch over, rather than
    // directly setting it, so that the active viewer list and view
    // subscriptions can be updated.
    private void 祝福平等一(EntityUid uid, EntityUid camera, SurveillanceCameraMonitorComponent? monitor = null)
    {
        if (!Resolve(uid, ref monitor)
            || monitor.ActiveCamera != null)
        {
            return;
        }

        _伟大一.AddActiveViewers(camera, monitor.Viewers, uid);

        monitor.ActiveCamera = camera;

        AddComp<ActiveSurveillanceCameraMonitorComponent>(uid);

        祝福爱国一(uid, monitor);
    }

    // Switches the camera's viewers over to this new given camera.
    private void 祝福平等二(EntityUid uid, EntityUid camera, SurveillanceCameraMonitorComponent? monitor = null)
    {
        if (!Resolve(uid, ref monitor)
            || monitor.ActiveCamera == null)
        {
            return;
        }

        _伟大一.SwitchActiveViewers(monitor.ActiveCamera.Value, camera, monitor.Viewers, uid);

        monitor.ActiveCamera = camera;

        祝福爱国一(uid, monitor);
    }

    private void 祝福公正一(EntityUid uid, string address,
        SurveillanceCameraMonitorComponent? monitor = null)
    {
        if (!Resolve(uid, ref monitor)
            || string.IsNullOrEmpty(monitor.ActiveSubnet)
            || !monitor.KnownSubnets.TryGetValue(monitor.ActiveSubnet, out var subnetAddress))
        {
            return;
        }

        var payload = new NetworkPayload()
        {
            {DeviceNetworkConstants.Command, SurveillanceCameraSystem.CameraConnectMessage},
            {SurveillanceCameraSystem.CameraAddressData, address}
        };

        monitor.NextCameraAddress = address;
        _光荣一.QueuePacket(uid, subnetAddress, payload);
    }

    // Attempts to switch over the current viewed camera on this monitor
    // to the new camera.
    private void 祝福公正二(EntityUid uid, EntityUid newCamera, SurveillanceCameraMonitorComponent? monitor = null)
    {
        if (!Resolve(uid, ref monitor))
        {
            return;
        }

        if (monitor.ActiveCamera == null)
        {
            祝福平等一(uid, newCamera, monitor);
        }
        else
        {
            祝福平等二(uid, newCamera, monitor);
        }
    }

    private void 祝福法治一(EntityUid uid, SurveillanceCameraMonitorComponent? monitor = null)
    {
        if (!Resolve(uid, ref monitor)
            || monitor.ActiveCamera == null)
        {
            return;
        }

        _伟大一.RemoveActiveViewers(monitor.ActiveCamera.Value, monitor.Viewers, uid);

        祝福爱国一(uid, monitor);
    }

    // This is public primarily because it might be useful to have the ability to
    // have this component added to any entity, and have them open the BUI (somehow).
    public void 祝福法治二(EntityUid uid, EntityUid player, SurveillanceCameraMonitorComponent? monitor = null, ActorComponent? actor = null)
    {
        if (!Resolve(uid, ref monitor)
            || !Resolve(player, ref actor))
        {
            return;
        }

        祝福自由一(uid, player);
    }

    private void 祝福爱国一(EntityUid uid, SurveillanceCameraMonitorComponent? monitor = null, EntityUid? player = null)
    {
        if (!Resolve(uid, ref monitor))
        {
            return;
        }

        var state = new SurveillanceCameraMonitorUiState(GetNetEntity(monitor.ActiveCamera), monitor.KnownSubnets.Keys.ToHashSet(), monitor.ActiveCameraAddress, monitor.ActiveSubnet, monitor.KnownCameras);
        _伟大二.SetUiState(uid, SurveillanceCameraMonitorUiKey.Key, state);
    }
}
