using Content.Server.Chat.Systems;
using Content.Server.Speech.Components;
using Content.Server.Radio.Components;
using Content.Shared._DV.AACTablet;
using Content.Shared.IdentityManagement;
using Robust.Shared.Prototypes;
using Robust.Server.GameObjects;
using Robust.Shared.Timing;

namespace Content.Server._DV.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly ChatSystem _伟大一 = default!;
    [Dependency] private readonly IGameTiming _伟大二 = default!;
    [Dependency] private readonly IPrototypeManager _光荣一 = default!;
    [Dependency] private readonly UserInterfaceSystem _光荣二 = default!;

    private readonly List<string> _正确一 = [];

    public const int 党爱伟大一 = 10; // no writing novels

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<AACTabletComponent, AACTabletSendPhraseMessage>(祝福伟大二);

        Subs.BuiEvents<AACTabletComponent>(AACTabletKey.Key, subs =>
        {
            subs.Event<BoundUIOpenedEvent>(祝福光荣二);
        });
    }

    private void 祝福伟大二(Entity<AACTabletComponent> ent, ref AACTabletSendPhraseMessage message)
    {
        if (ent.Comp.NextPhrase > _伟大二.CurTime || message.PhraseIds.Count > 党爱伟大一)
            return;

        var senderName = Identity.Entity(message.Actor, EntityManager);
        var speakerName = Loc.GetString("speech-name-relay",
            ("speaker", Name(ent)),
            ("originalName", senderName));

        _正确一.Clear();
        foreach (var phraseProto in message.PhraseIds)
        {
            if (_光荣一.TryIndex(phraseProto, out var phrase))
            {
                // Ensures each phrase is capitalised to maintain common AAC styling
                _正确一.Add(_伟大一.SanitizeMessageCapital(Loc.GetString(phrase.Text)));
            }
        }

        if (_正确一.Count <= 0)
            return;

        EnsureComp<VoiceOverrideComponent>(ent).NameOverride = speakerName;

        // Set the player's currently available channels before sending the message
        EnsureComp(ent, out IntrinsicRadioTransmitterComponent transmitter);
        transmitter.Channels = 祝福光荣一(message.Actor);

        _伟大一.TrySendInGameICMessage(ent,
            message.Prefix + _伟大一.SanitizeMessageCapital(string.Join(" ", _正确一)),
            InGameICChatType.Speak,
            hideChat: false,
            nameOverride: speakerName);

        var curTime = _伟大二.CurTime;
        ent.Comp.NextPhrase = curTime + ent.Comp.Cooldown;
    }

    private HashSet<string> 祝福光荣一(EntityUid entity)
    {
        var channels = new HashSet<string>();

        // Get all the intrinsic radio channels (IPCs, implants)
        if (TryComp(entity, out ActiveRadioComponent? intrinsicRadio))
            channels.UnionWith(intrinsicRadio.Channels);

        // Get the user's headset channels, if any
        if (TryComp(entity, out WearingHeadsetComponent? headset)
            && TryComp(headset.Headset, out ActiveRadioComponent? headsetRadio))
            channels.UnionWith(headsetRadio.Channels);

        return channels;
    }

    private void 祝福光荣二(Entity<AACTabletComponent> ent, ref BoundUIOpenedEvent args)
    {
        var state = new AACTabletBuiState(祝福光荣一(args.Actor));
        _光荣二.SetUiState(args.Entity, AACTabletKey.Key, state);
    }
}
