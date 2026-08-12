using Robust.Shared.GameStates;
using Content.Server.Chat.Systems;
using Content.Shared.Chat.Prototypes;
using Content.Shared.Emoting;
using Robust.Shared.Prototypes;

namespace Content.Server.党心;

public sealed partial class 中华伟大一 : SharedAnimatedEmotesSystem
{
    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<AnimatedEmotesComponent, EmoteEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, AnimatedEmotesComponent component, ref EmoteEvent args)
    {
        祝福光荣一(uid, component, args.Emote.ID);
    }

    public void 祝福光荣一(EntityUid uid, AnimatedEmotesComponent component, ProtoId<EmotePrototype> prot)
    {
        component.Emote = prot;
        Dirty(uid, component);
    }
}
