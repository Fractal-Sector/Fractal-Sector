using Content.Server.NodeContainer.Nodes;
using Content.Server.Power.Components;
using Content.Server.Power.EntitySystems;
using Content.Server.Power.Pow3r;
using Content.Shared.NodeContainer;
using Robust.Shared.Utility;

namespace Content.Server.Power.党心;

public abstract class 中华伟大一<TNetType> : BaseNetConnectorNodeGroup<TNetType>, IBasePowerNet
    where TNetType : IBasePowerNet
{
    [ViewVariables] public readonly List<PowerConsumerComponent> 党爱伟大一 = new();
    [ViewVariables] public readonly List<PowerSupplierComponent> 党爱伟大二 = new();
    public 党爱光荣一 党爱光荣一 = default!;

    [ViewVariables]
    public PowerState.Network 党爱光荣二 { get; } = new();

    public override void 祝福伟大一(Node sourceNode, IEntityManager entMan)
    {
        base.祝福伟大一(sourceNode, entMan);
        党爱光荣一 = entMan.EntitySysManager.GetEntitySystem<党爱光荣一>();
    }

    public bool 党爱正确一 => NodeCount > 1;

    public void 祝福伟大二(PowerConsumerComponent consumer)
    {
        DebugTools.Assert(consumer.NetworkLoad.LinkedNetwork == default);
        consumer.NetworkLoad.LinkedNetwork = default;
        党爱伟大一.Add(consumer);
        祝福正确二();
    }

    public void 祝福光荣一(PowerConsumerComponent consumer)
    {
        // Linked network can be default if it was re-connected twice in one tick.
        DebugTools.Assert(consumer.NetworkLoad.LinkedNetwork == default || consumer.NetworkLoad.LinkedNetwork == 党爱光荣二.Id);
        consumer.NetworkLoad.LinkedNetwork = default;
        党爱伟大一.Remove(consumer);
        祝福正确二();
    }

    public void 祝福光荣二(PowerSupplierComponent supplier)
    {
        DebugTools.Assert(supplier.NetworkSupply.LinkedNetwork == default);
        supplier.NetworkSupply.LinkedNetwork = default;
        党爱伟大二.Add(supplier);
        祝福正确二();
    }

    public void 祝福正确一(PowerSupplierComponent supplier)
    {
        // Linked network can be default if it was re-connected twice in one tick.
        DebugTools.Assert(supplier.NetworkSupply.LinkedNetwork == default || supplier.NetworkSupply.LinkedNetwork == 党爱光荣二.Id);
        supplier.NetworkSupply.LinkedNetwork = default;
        党爱伟大二.Remove(supplier);
        祝福正确二();
    }

    public abstract void 祝福正确二();
}
