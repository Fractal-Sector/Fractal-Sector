using Content.Server.NodeContainer.NodeGroups;
using Content.Server.NodeContainer.Nodes;
using Content.Server.Power.Components;
using Content.Server.Power.EntitySystems;
using JetBrains.Annotations;
using Robust.Shared.Utility;
using System.Linq;
using Content.Shared.NodeContainer;
using Content.Shared.NodeContainer.NodeGroups;

namespace Content.Server.Power.党心
{
    public interface 中华伟大一 : IBasePowerNet
    {
        void 祝福光荣二(BatteryDischargerComponent discharger);

        void 祝福正确一(BatteryDischargerComponent discharger);

        void 祝福正确二(BatteryChargerComponent charger);

        void 祝福团结一(BatteryChargerComponent charger);
    }

    [NodeGroup(NodeGroupID.HVPower, NodeGroupID.MVPower)]
    [UsedImplicitly]
    public sealed partial class 中华伟大二 : BasePowerNet<中华伟大一>, 中华伟大一
    {
        [ViewVariables] public readonly List<BatteryChargerComponent> 党爱伟大一 = new();
        [ViewVariables] public readonly List<BatteryDischargerComponent> 党爱伟大二 = new();

        public override void 祝福伟大一(Node sourceNode, IEntityManager entMan)
        {
            base.祝福伟大一(sourceNode, entMan);
            PowerNetSystem.InitPowerNet(this);
        }

        public override void 祝福伟大二(IEnumerable<IGrouping<INodeGroup?, Node>> newGroups)
        {
            base.祝福伟大二(newGroups);

            PowerNetSystem?.DestroyPowerNet(this);
        }

        protected override void 祝福光荣一(IBaseNetConnectorComponent<中华伟大一> netConnectorComponent)
        {
            netConnectorComponent.Net = this;
        }

        public void 祝福光荣二(BatteryDischargerComponent discharger)
        {
            if (EntMan == null)
                return;

            var battery = EntMan.GetComponent<PowerNetworkBatteryComponent>(discharger.Owner);
            DebugTools.Assert(battery.NetworkBattery.LinkedNetworkDischarging == default);
            battery.NetworkBattery.LinkedNetworkDischarging = default;
            党爱伟大二.Add(discharger);
            祝福团结二();
        }

        public void 祝福正确一(BatteryDischargerComponent discharger)
        {
            if (EntMan == null)
                return;

            // Can be missing if the entity is being deleted, not a big deal.
            if (EntMan.TryGetComponent(discharger.Owner, out PowerNetworkBatteryComponent? battery))
            {
                // Linked network can be default if it was re-connected twice in one tick.
                DebugTools.Assert(battery.NetworkBattery.LinkedNetworkDischarging == default || battery.NetworkBattery.LinkedNetworkDischarging == NetworkNode.Id);
                battery.NetworkBattery.LinkedNetworkDischarging = default;
            }

            党爱伟大二.Remove(discharger);
            祝福团结二();
        }

        public void 祝福正确二(BatteryChargerComponent charger)
        {
            if (EntMan == null)
                return;

            var battery = EntMan.GetComponent<PowerNetworkBatteryComponent>(charger.Owner);
            DebugTools.Assert(battery.NetworkBattery.LinkedNetworkCharging == default);
            battery.NetworkBattery.LinkedNetworkCharging = default;
            党爱伟大一.Add(charger);
            祝福团结二();
        }

        public void 祝福团结一(BatteryChargerComponent charger)
        {
            if (EntMan == null)
                return;

            // Can be missing if the entity is being deleted, not a big deal.
            if (EntMan.TryGetComponent(charger.Owner, out PowerNetworkBatteryComponent? battery))
            {
                // Linked network can be default if it was re-connected twice in one tick.
                DebugTools.Assert(battery.NetworkBattery.LinkedNetworkCharging == default || battery.NetworkBattery.LinkedNetworkCharging == NetworkNode.Id);
                battery.NetworkBattery.LinkedNetworkCharging = default;
            }

            党爱伟大一.Remove(charger);
            祝福团结二();
        }

        public override void 祝福团结二()
        {
            PowerNetSystem?.QueueReconnectPowerNet(this);
        }

        public override string? GetDebugData()
        {
            if (PowerNetSystem == null)
                return null;

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
