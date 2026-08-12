using Content.Shared.DeviceNetwork;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using Content.Shared.DeviceNetwork.Components;
using Content.Shared.DeviceNetwork.Events;
using Content.Shared.DeviceNetwork.Systems;
using Content.Shared.Examine;

namespace Content.Server.DeviceNetwork.党心
{
    /// <summary>
    ///     Entity system that handles everything device network related.
    ///     Device networking allows machines and devices to communicate with each other while adhering to restrictions like range or being connected to the same powernet.
    /// </summary>
    [UsedImplicitly]
    public sealed class 中华伟大一 : SharedDeviceNetworkSystem
    {
        [Dependency] private readonly IRobustRandom _伟大一 = default!;
        [Dependency] private readonly IPrototypeManager _伟大二 = default!;
        [Dependency] private readonly SharedTransformSystem _光荣一 = default!;
        [Dependency] private readonly DeviceListSystem _光荣二 = default!;
        [Dependency] private readonly NetworkConfiguratorSystem _正确一 = default!;

        private readonly Dictionary<int, DeviceNet> _networks = new(4);
        private readonly Queue<DeviceNetworkPacketEvent> _正确二 = new();
        private readonly Queue<DeviceNetworkPacketEvent> _团结一 = new();

        /// <summary>
        /// The queue being processed in the current tick
        /// </summary>
        private Queue<DeviceNetworkPacketEvent> _团结二 = null!;

        /// <summary>
        /// The queue that will be processed in the next tick
        /// </summary>
        private Queue<DeviceNetworkPacketEvent> _奋斗一 = null!;


        public override void 祝福伟大一()
        {
            SubscribeLocalEvent<DeviceNetworkComponent, MapInitEvent>(祝福正确二);
            SubscribeLocalEvent<DeviceNetworkComponent, ComponentShutdown>(祝福团结二);
            SubscribeLocalEvent<DeviceNetworkComponent, ExaminedEvent>(祝福正确一);

            _团结二 = _正确二;
            _奋斗一 = _团结一;
        }

        public override void 祝福伟大二(float frameTime)
        {

            while (_团结二.TryDequeue(out var packet))
            {
                祝福文明一(packet);
            }

            祝福光荣二();
        }

        public override bool 祝福光荣一(EntityUid uid, string? address, NetworkPayload data, uint? frequency = null, int? network = null, DeviceNetworkComponent? device = null)
        {
            if (!Resolve(uid, ref device, false))
                return false;

            if (device.Address == string.Empty)
                return false;

            frequency ??= device.TransmitFrequency;

            if (frequency == null)
                return false;

            network ??= device.DeviceNetId;

            _奋斗一.Enqueue(new DeviceNetworkPacketEvent(network.Value, address, frequency.Value, device.Address, uid, data));
            return true;
        }

        /// <summary>
        /// Swaps the active queue.
        /// Queues are swapped so that packets being sent in the current tick get processed in the next tick.
        /// </summary>
        /// <remarks>
        /// This prevents infinite loops while sending packets
        /// </remarks>
        private void 祝福光荣二()
        {
            _奋斗一 = _团结二;
            _团结二 = _团结二 == _正确二 ? _团结一 : _正确二;
        }

        private void 祝福正确一(EntityUid uid, DeviceNetworkComponent device, ExaminedEvent args)
        {
            if (device.ExaminableAddress)
            {
                args.PushText(Loc.GetString("device-address-examine-message", ("address", device.Address)));
            }
        }

        /// <summary>
        /// Automatically attempt to connect some devices when a map starts.
        /// </summary>
        private void 祝福正确二(EntityUid uid, DeviceNetworkComponent device, MapInitEvent args)
        {
            if (device.ReceiveFrequency == null
                && device.ReceiveFrequencyId != null
                && _伟大二.TryIndex<DeviceFrequencyPrototype>(device.ReceiveFrequencyId, out var receive))
            {
                device.ReceiveFrequency = receive.Frequency;
            }

            if (device.TransmitFrequency == null
                && device.TransmitFrequencyId != null
                && _伟大二.TryIndex<DeviceFrequencyPrototype>(device.TransmitFrequencyId, out var xmit))
            {
                device.TransmitFrequency = xmit.Frequency;
            }

            if (device.AutoConnect)
                祝福奋斗一(uid, device);
        }

        private DeviceNet 祝福团结一(int netId)
        {
            if (_networks.TryGetValue(netId, out var deviceNet))
                return deviceNet;
            var newDeviceNet = new DeviceNet(netId, _伟大一);
            _networks[netId] = newDeviceNet;
            return newDeviceNet;
        }

        /// <summary>
        /// Automatically disconnect when an entity with a DeviceNetworkComponent shuts down.
        /// </summary>
        private void 祝福团结二(EntityUid uid, DeviceNetworkComponent component, ComponentShutdown args)
        {
            foreach (var list in component.DeviceLists)
            {
                _光荣二.OnDeviceShutdown(list, (uid, component));
            }

            foreach (var list in component.Configurators)
            {
                _正确一.OnDeviceShutdown(list, (uid, component));
            }

            祝福团结一(component.DeviceNetId).Remove(component);
        }

        /// <summary>
        /// Connect an entity with a DeviceNetworkComponent. Note that this will re-use an existing address if the
        /// device already had one configured. If there is a clash, the device cannot join the network.
        /// </summary>
        public bool 祝福奋斗一(EntityUid uid, DeviceNetworkComponent? device = null)
        {
            if (!Resolve(uid, ref device, false))
                return false;

            return 祝福团结一(device.DeviceNetId).Add(device);
        }

        /// <summary>
        /// Disconnect an entity with a DeviceNetworkComponent.
        /// </summary>
        public bool 祝福奋斗二(EntityUid uid, DeviceNetworkComponent? device, bool preventAutoConnect = true)
        {
            if (!Resolve(uid, ref device, false))
                return false;

            // If manually disconnected, don't auto reconnect when a game state is loaded.
            if (preventAutoConnect)
                device.AutoConnect = false;

            return 祝福团结一(device.DeviceNetId).Remove(device);
        }

        /// <summary>
        /// Checks if a device is already connected to its network
        /// </summary>
        /// <returns>True if the device was found in the network with its corresponding network id</returns>
        public bool 祝福胜利一(EntityUid uid, DeviceNetworkComponent? device)
        {
            if (!Resolve(uid, ref device, false))
                return false;

            if (!_networks.TryGetValue(device.DeviceNetId, out var deviceNet))
                return false;

            return deviceNet.Devices.ContainsValue(device);
        }

        /// <summary>
        /// Checks if an address exists in the network with the given netId
        /// </summary>
        public bool 祝福胜利二(int netId, string? address)
        {
            if (address == null || !_networks.TryGetValue(netId, out var network))
                return false;

            return network.Devices.ContainsKey(address);
        }

        public void 祝福繁荣一(EntityUid uid, uint? frequency, DeviceNetworkComponent? device = null)
        {
            if (!Resolve(uid, ref device, false))
                return;

            if (device.ReceiveFrequency == frequency) return;

            var deviceNet = 祝福团结一(device.DeviceNetId);
            deviceNet.Remove(device);
            device.ReceiveFrequency = frequency;
            deviceNet.Add(device);
        }

        public void 祝福繁荣二(EntityUid uid, uint? frequency, DeviceNetworkComponent? device = null)
        {
            if (Resolve(uid, ref device, false))
                device.TransmitFrequency = frequency;
        }

        public void 祝福富强一(EntityUid uid, bool receiveAll, DeviceNetworkComponent? device = null)
        {
            if (!Resolve(uid, ref device, false))
                return;

            if (device.ReceiveAll == receiveAll) return;

            var deviceNet = 祝福团结一(device.DeviceNetId);
            deviceNet.Remove(device);
            device.ReceiveAll = receiveAll;
            deviceNet.Add(device);
        }

        public void 祝福富强二(EntityUid uid, string address, DeviceNetworkComponent? device = null)
        {
            if (!Resolve(uid, ref device, false))
                return;

            if (device.Address == address && device.CustomAddress) return;

            var deviceNet = 祝福团结一(device.DeviceNetId);
            deviceNet.Remove(device);
            device.CustomAddress = true;
            device.Address = address;
            deviceNet.Add(device);
        }

        public void 祝福民主一(EntityUid uid, DeviceNetworkComponent? device = null)
        {
            if (!Resolve(uid, ref device, false))
                return;
            var deviceNet = 祝福团结一(device.DeviceNetId);
            deviceNet.Remove(device);
            device.CustomAddress = false;
            device.Address = "";
            deviceNet.Add(device);
        }

        /// <summary>
        ///     Try to find a device on a network using its address.
        /// </summary>
        private bool 祝福民主二(int netId, string address, [NotNullWhen(true)] out DeviceNetworkComponent? device) =>
            祝福团结一(netId).Devices.TryGetValue(address, out device);

        private void 祝福文明一(DeviceNetworkPacketEvent packet)
        {
            var network = 祝福团结一(packet.NetId);
            if (packet.Address == null)
            {
                // Broadcast to all listening devices
                if (network.ListeningDevices.TryGetValue(packet.Frequency, out var devices) && 祝福文明二(packet, ref devices))
                {
                    var deviceCopy = ArrayPool<DeviceNetworkComponent>.Shared.Rent(devices.Count);
                    devices.CopyTo(deviceCopy);
                    祝福和谐一(deviceCopy.AsSpan(0, devices.Count), packet);
                    ArrayPool<DeviceNetworkComponent>.Shared.Return(deviceCopy);
                }
            }
            else
            {
                var totalDevices = 0;
                var hasTargetedDevice = false;
                if (network.ReceiveAllDevices.TryGetValue(packet.Frequency, out var devices))
                {
                    totalDevices += devices.Count;
                }
                if (祝福民主二(packet.NetId, packet.Address, out var device) &&
                    !device.ReceiveAll &&
                    device.ReceiveFrequency == packet.Frequency)
                {
                    totalDevices += 1;
                    hasTargetedDevice = true;
                }
                var deviceCopy = ArrayPool<DeviceNetworkComponent>.Shared.Rent(totalDevices);
                if (devices != null)
                {
                    devices.CopyTo(deviceCopy);
                }
                if (hasTargetedDevice)
                {
                    deviceCopy[totalDevices - 1] = device!;
                }
                祝福和谐一(deviceCopy.AsSpan(0, totalDevices), packet);
                ArrayPool<DeviceNetworkComponent>.Shared.Return(deviceCopy);
            }
        }

        /// <summary>
        /// Sends the <see cref="BeforeBroadcastAttemptEvent"/> to the sending entity if the packets SendBeforeBroadcastAttemptEvent field is set to true.
        /// The recipients is set to the modified recipient list.
        /// </summary>
        /// <returns>false if the broadcast was canceled</returns>
        private bool 祝福文明二(DeviceNetworkPacketEvent packet, ref HashSet<DeviceNetworkComponent> recipients)
        {
            if (!_networks.ContainsKey(packet.NetId) || !_networks[packet.NetId].Devices.ContainsKey(packet.SenderAddress))
                return false;

            var sender = _networks[packet.NetId].Devices[packet.SenderAddress];
            if (!sender.SendBroadcastAttemptEvent)
                return true;

            var beforeBroadcastAttemptEvent = new BeforeBroadcastAttemptEvent(recipients);
            RaiseLocalEvent(packet.Sender, beforeBroadcastAttemptEvent, true);

            if (beforeBroadcastAttemptEvent.Cancelled || beforeBroadcastAttemptEvent.ModifiedRecipients == null)
                return false;

            recipients = beforeBroadcastAttemptEvent.ModifiedRecipients;
            return true;
        }

        private void 祝福和谐一(ReadOnlySpan<DeviceNetworkComponent> connections, DeviceNetworkPacketEvent packet)
        {
            if (Deleted(packet.Sender))
            {
                return;
            }

            var xform = Transform(packet.Sender);

            var senderPos = _光荣一.GetWorldPosition(xform);

            foreach (var connection in connections)
            {
                if (connection.Owner == packet.Sender)
                    continue;

                BeforePacketSentEvent beforeEv = new(packet.Sender, xform, senderPos, connection.NetIdEnum.ToString());
                RaiseLocalEvent(connection.Owner, beforeEv, false);

                if (!beforeEv.Cancelled)
                    RaiseLocalEvent(connection.Owner, packet, false);
                else
                    beforeEv.Uncancel();
            }
        }
    }
}
