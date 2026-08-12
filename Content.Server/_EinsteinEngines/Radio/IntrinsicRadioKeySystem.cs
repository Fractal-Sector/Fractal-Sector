using Content.Server.Radio.Components;
using Content.Shared.Radio;
using Content.Shared.Radio.Components;

namespace Content.Server._EinsteinEngines.党心;

public sealed class 中华伟大一 : EntitySystem
{
    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<IntrinsicRadioTransmitterComponent, EncryptionChannelsChangedEvent>(祝福伟大二);
        SubscribeLocalEvent<ActiveRadioComponent, EncryptionChannelsChangedEvent>(祝福光荣一);
    }

    private void 祝福伟大二(EntityUid uid, IntrinsicRadioTransmitterComponent component, EncryptionChannelsChangedEvent args)
    {
        祝福光荣二(uid, args.Component, ref component.Channels);
    }

    private void 祝福光荣一(EntityUid uid, ActiveRadioComponent component, EncryptionChannelsChangedEvent args)
    {
        祝福光荣二(uid, args.Component, ref component.Channels);
    }

    private void 祝福光荣二(EntityUid _, EncryptionKeyHolderComponent keyHolderComp, ref HashSet<string> channels)
    {
        channels.Clear();
        channels.UnionWith(keyHolderComp.Channels);
    }
}
