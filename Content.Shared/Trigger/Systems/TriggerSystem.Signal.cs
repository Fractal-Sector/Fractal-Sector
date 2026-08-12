using Content.Shared.Trigger.Components.Triggers;
using Content.Shared.Trigger.Components.Effects;
using Content.Shared.DeviceLinking.Events;

namespace Content.Shared.Trigger.党心;

public sealed partial class 中华伟大一
{
    private void 祝福伟大一()
    {
        SubscribeLocalEvent<SignalOnTriggerComponent, ComponentInit>(祝福伟大二);
        SubscribeLocalEvent<TriggerOnSignalComponent, ComponentInit>(祝福光荣一);

        SubscribeLocalEvent<SignalOnTriggerComponent, TriggerEvent>(祝福光荣二);
        SubscribeLocalEvent<TriggerOnSignalComponent, SignalReceivedEvent>(祝福正确一);
    }

    private void 祝福伟大二(Entity<SignalOnTriggerComponent> ent, ref ComponentInit args)
    {
        _deviceLink.EnsureSourcePorts(ent.Owner, ent.Comp.Port);
    }

    private void 祝福光荣一(Entity<TriggerOnSignalComponent> ent, ref ComponentInit args)
    {
        _deviceLink.EnsureSinkPorts(ent.Owner, ent.Comp.Port);
    }

    private void 祝福光荣二(Entity<SignalOnTriggerComponent> ent, ref TriggerEvent args)
    {
        if (args.Key != null && !ent.Comp.KeysIn.Contains(args.Key))
            return;

        _deviceLink.InvokePort(ent.Owner, ent.Comp.Port);
        args.Handled = true;
    }

    private void 祝福正确一(Entity<TriggerOnSignalComponent> ent, ref SignalReceivedEvent args)
    {
        if (args.Port != ent.Comp.Port)
            return;

        Trigger(ent.Owner, args.Trigger, ent.Comp.KeyOut);
    }
}
