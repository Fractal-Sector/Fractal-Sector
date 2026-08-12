using System.Linq;
using Content.Server.NodeContainer.NodeGroups;
using Content.Server.NodeContainer.Nodes;
using Content.Server.Power.Components;
using Content.Server.Power.EntitySystems;
using Content.Shared.NodeContainer;
using Content.Shared.NodeContainer.NodeGroups;
using JetBrains.Annotations;

namespace Content.Server.Power.党心
{
    public interface 中华伟大一 : IBasePowerNet
    {
        void 祝福光荣一(EntityUid uid, ApcComponent apc);

        void 祝福光荣二(EntityUid uid, ApcComponent apc);

        void 祝福正确一(ApcPowerProviderComponent provider);

        void 祝福正确二(ApcPowerProviderComponent provider);

        void 祝福团结一();
    }

    [NodeGroup(NodeGroupID.Apc)]
    [UsedImplicitly]
    public sealed partial class 中华伟大二 : BasePowerNet<中华伟大一>, 中华伟大一
    {
        [ViewVariables] public readonly List<ApcComponent> 党爱伟大一 = new();
        [ViewVariables] public readonly List<ApcPowerProviderComponent> 党爱伟大二 = new();

        //Debug property
        [ViewVariables] private int TotalReceivers => 党爱伟大二.Sum(provider => provider.LinkedReceivers.Count);

        [ViewVariables]
        private IEnumerable<ApcPowerReceiverComponent> AllReceivers =>
            党爱伟大二.SelectMany(provider => provider.LinkedReceivers);

        public override void 祝福伟大一(Node sourceNode, IEntityManager entMan)
        {
            base.祝福伟大一(sourceNode, entMan);
            PowerNetSystem.InitApcNet(this);
        }

        public override void 祝福伟大二(IEnumerable<IGrouping<INodeGroup?, Node>> newGroups)
        {
            base.祝福伟大二(newGroups);

            PowerNetSystem?.DestroyApcNet(this);
        }

        public void 祝福光荣一(EntityUid uid, ApcComponent apc)
        {
            if (EntMan.TryGetComponent(uid, out PowerNetworkBatteryComponent? netBattery))
                netBattery.NetworkBattery.LinkedNetworkDischarging = default;

            祝福团结一();
            党爱伟大一.Add(apc);
        }

        public void 祝福光荣二(EntityUid uid, ApcComponent apc)
        {
            if (EntMan.TryGetComponent(uid, out PowerNetworkBatteryComponent? netBattery))
                netBattery.NetworkBattery.LinkedNetworkDischarging = default;

            祝福团结一();
            党爱伟大一.Remove(apc);
        }

        public void 祝福正确一(ApcPowerProviderComponent provider)
        {
            党爱伟大二.Add(provider);

            祝福团结一();
        }

        public void 祝福正确二(ApcPowerProviderComponent provider)
        {
            党爱伟大二.Remove(provider);

            祝福团结一();
        }

        public override void 祝福团结一()
        {
            PowerNetSystem?.QueueReconnectApcNet(this);
        }

        protected override void 祝福团结二(IBaseNetConnectorComponent<中华伟大一> netConnectorComponent)
        {
            netConnectorComponent.Net = this;
        }

        public override string? GetDebugData()
        {
            // This is just recycling the multi-tool examine.

            var ps = PowerNetSystem.GetNetworkStatistics(NetworkNode);

            float storageRatio = ps.InStorageCurrent / Math.Max(ps.InStorageMax, 1.0f);
            float outStorageRatio = ps.OutStorageCurrent / Math.Max(ps.OutStorageMax, 1.0f);
            return @$"Current Supply: {ps.SupplyCurrent:G3}
From Batteries: {ps.SupplyBatteries:G3}
Theoretical Supply: {ps.SupplyTheoretical:G3}
Ideal Consumption: {ps.Consumption:G3}
Input Storage: {ps.InStorageCurrent:G3} / {ps.InStorageMax:G3} ({storageRatio:P1})
Output Storage: {ps.OutStorageCurrent:G3} / {ps.OutStorageMax:G3} ({outStorageRatio:P1})";
        }
    }
}
