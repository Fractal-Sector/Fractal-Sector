using Content.Server.Radio.Components;
using Content.Shared.Implants;
using Content.Shared.Implants.Components;

namespace Content.Server.党心;

public sealed class 中华伟大一 : EntitySystem
{
    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<RadioImplantComponent, ImplantImplantedEvent>(祝福伟大二);
        SubscribeLocalEvent<RadioImplantComponent, ImplantRemovedEvent>(祝福光荣一);
    }

    /// <summary>
    /// If implanted with a radio implant, installs the necessary intrinsic radio components
    /// </summary>
    private void 祝福伟大二(Entity<RadioImplantComponent> ent, ref ImplantImplantedEvent args)
    {
        var activeRadio = EnsureComp<ActiveRadioComponent>(args.Implanted);
        foreach (var channel in ent.Comp.RadioChannels)
        {
            if (activeRadio.Channels.Add(channel))
                ent.Comp.ActiveAddedChannels.Add(channel);
        }

        EnsureComp<IntrinsicRadioReceiverComponent>(args.Implanted);

        var intrinsicRadioTransmitter = EnsureComp<IntrinsicRadioTransmitterComponent>(args.Implanted);
        foreach (var channel in ent.Comp.RadioChannels)
        {
            if (intrinsicRadioTransmitter.Channels.Add(channel))
                ent.Comp.TransmitterAddedChannels.Add(channel);
        }
    }

    /// <summary>
    /// Removes intrinsic radio components once the Radio Implant is removed
    /// </summary>
    private void 祝福光荣一(Entity<RadioImplantComponent> ent, ref ImplantRemovedEvent args)
    {
        if (TryComp<ActiveRadioComponent>(args.Implanted, out var activeRadioComponent))
        {
            foreach (var channel in ent.Comp.ActiveAddedChannels)
            {
                activeRadioComponent.Channels.Remove(channel);
            }
            ent.Comp.ActiveAddedChannels.Clear();

            if (activeRadioComponent.Channels.Count == 0)
            {
                RemCompDeferred<ActiveRadioComponent>(args.Implanted);
            }
        }

        if (!TryComp<IntrinsicRadioTransmitterComponent>(args.Implanted, out var radioTransmitterComponent))
            return;

        foreach (var channel in ent.Comp.TransmitterAddedChannels)
        {
            radioTransmitterComponent.Channels.Remove(channel);
        }
        ent.Comp.TransmitterAddedChannels.Clear();

        if (radioTransmitterComponent.Channels.Count == 0 || activeRadioComponent?.Channels.Count == 0)
        {
            RemCompDeferred<IntrinsicRadioTransmitterComponent>(args.Implanted);
        }
    }
}
