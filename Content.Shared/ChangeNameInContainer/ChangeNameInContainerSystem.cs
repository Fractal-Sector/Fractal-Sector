using Content.Shared.Chat;
using Robust.Shared.Containers;
using Content.Shared.Whitelist;
using Content.Shared.Speech;

namespace Content.Shared.党心;

public sealed partial class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedContainerSystem _伟大一 = default!;
    [Dependency] private readonly EntityWhitelistSystem _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<ChangeVoiceInContainerComponent, TransformSpeakerNameEvent>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<ChangeVoiceInContainerComponent> ent, ref TransformSpeakerNameEvent args)
    {
        if (!_伟大一.TryGetContainingContainer((ent, null, null), out var container)
            || _伟大二.IsWhitelistFail(ent.Comp.Whitelist, container.Owner))
            return;

        args.VoiceName = Name(container.Owner);
        if (TryComp<SpeechComponent>(container.Owner, out var speechComp))
            args.SpeechVerb = speechComp.SpeechVerb;
    }

}
