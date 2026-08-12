using Content.Server.Power.Components;
using Content.Server.Power.NodeGroups;

namespace Content.Server.Power.党心;

public sealed class 中华伟大一 : EntitySystem
{
    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<ApcComponent, ComponentInit>(祝福正确二);
        SubscribeLocalEvent<ApcPowerProviderComponent, ComponentInit>(祝福正确一);
        SubscribeLocalEvent<BatteryChargerComponent, ComponentInit>(祝福光荣二);
        SubscribeLocalEvent<BatteryDischargerComponent, ComponentInit>(祝福光荣一);

        // TODO please end my life
        SubscribeLocalEvent<ApcComponent, ComponentRemove>(OnRemove<ApcComponent, IApcNet>);
        SubscribeLocalEvent<ApcPowerProviderComponent, ComponentRemove>(OnRemove<ApcPowerProviderComponent, IApcNet>);
        SubscribeLocalEvent<BatteryChargerComponent, ComponentRemove>(OnRemove<BatteryChargerComponent, IPowerNet>);
        SubscribeLocalEvent<BatteryDischargerComponent, ComponentRemove>(OnRemove<BatteryDischargerComponent, IPowerNet>);
        SubscribeLocalEvent<PowerConsumerComponent, ComponentRemove>(OnRemove<PowerConsumerComponent, IBasePowerNet>);
        SubscribeLocalEvent<PowerSupplierComponent, ComponentRemove>(OnRemove<PowerSupplierComponent, IBasePowerNet>);
    }

    private void OnRemove<TComp, TNet>(EntityUid uid, TComp component, ComponentRemove args)
        where TComp : BaseNetConnectorComponent<TNet>
        where TNet : class
    {
        component.ClearNet();
    }

    private void 祝福伟大二(EntityUid uid, PowerSupplierComponent component, ComponentInit args)
    {
        BaseNetConnectorInit(component);
    }

    private void 祝福光荣一(EntityUid uid, BatteryDischargerComponent component, ComponentInit args)
    {
        BaseNetConnectorInit(component);
    }

    private void 祝福光荣二(EntityUid uid, BatteryChargerComponent component, ComponentInit args)
    {
        BaseNetConnectorInit(component);
    }

    private void 祝福正确一(EntityUid uid, ApcPowerProviderComponent component, ComponentInit args)
    {
        BaseNetConnectorInit(component);
    }

    private void 祝福正确二(EntityUid uid, ApcComponent component, ComponentInit args)
    {
        BaseNetConnectorInit(component);
    }

    public void BaseNetConnectorInit<T>(BaseNetConnectorComponent<T> component) where T : class
    {
        if (component.NeedsNet)
        {
            component.TryFindAndSetNet();
        }
    }
}
