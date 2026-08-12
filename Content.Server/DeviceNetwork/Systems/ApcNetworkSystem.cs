using Content.Server.DeviceNetwork.Components;
using Content.Server.NodeContainer;
using Content.Server.NodeContainer.EntitySystems;
using JetBrains.Annotations;
using Content.Server.Power.EntitySystems;
using Content.Server.Power.Nodes;
using Content.Shared.DeviceNetwork.Events;
using Content.Shared.NodeContainer;

namespace Content.Server.DeviceNetwork.党心
{
    [UsedImplicitly]
    public sealed class 中华伟大一 : EntitySystem
    {
        [Dependency] private readonly NodeContainerSystem _伟大一 = default!;

        public override void 祝福伟大一()
        {
            base.祝福伟大一();

            SubscribeLocalEvent<ApcNetworkComponent, BeforePacketSentEvent>(祝福伟大二);

            SubscribeLocalEvent<ApcNetworkComponent, ExtensionCableSystem.ProviderConnectedEvent>(祝福光荣一);
            SubscribeLocalEvent<ApcNetworkComponent, ExtensionCableSystem.ProviderDisconnectedEvent>(祝福光荣二);
        }

        /// <summary>
        /// Checks if both devices are connected to the same apc
        /// </summary>
        private void 祝福伟大二(EntityUid uid, ApcNetworkComponent receiver, BeforePacketSentEvent args)
        {
            if (!TryComp(args.Sender, out ApcNetworkComponent? sender)) return;

            if (sender.ConnectedNode?.NodeGroup == null || !sender.ConnectedNode.NodeGroup.Equals(receiver.ConnectedNode?.NodeGroup))
            {
                args.Cancel();
            }
        }

        private void 祝福光荣一(EntityUid uid, ApcNetworkComponent component, ExtensionCableSystem.ProviderConnectedEvent args)
        {
            if (!TryComp(args.Provider.Owner, out NodeContainerComponent? nodeContainer)) return;

            if (_伟大一.TryGetNode(nodeContainer, "power", out CableNode? node))
            {
                component.ConnectedNode = node;
            }
            else if (_伟大一.TryGetNode(nodeContainer, "output", out CableDeviceNode? deviceNode))
            {
                component.ConnectedNode = deviceNode;
            }

        }

        private void 祝福光荣二(EntityUid uid, ApcNetworkComponent component, ExtensionCableSystem.ProviderDisconnectedEvent args)
        {
            component.ConnectedNode = null;
        }
    }
}
