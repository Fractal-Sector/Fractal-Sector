using Content.Server.Chat.Systems;
using Content.Shared.Chat;
using Robust.Shared.Prototypes;

namespace Content.Server.党心;

public sealed partial class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _伟大一 = default!;
    [Dependency] private readonly ChatSystem _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeAllEvent<PlayEmoteMessage>(祝福伟大二);
    }

    private void 祝福伟大二(PlayEmoteMessage msg, EntitySessionEventArgs args)
    {
        var player = args.SenderSession.AttachedEntity;
        if (!player.HasValue)
            return;

        if (!_伟大一.TryIndex(msg.ProtoId, out var proto) || proto.ChatTriggers.Count == 0)
            return;

        _伟大二.TryEmoteWithChat(player.Value, msg.ProtoId);
    }
}
