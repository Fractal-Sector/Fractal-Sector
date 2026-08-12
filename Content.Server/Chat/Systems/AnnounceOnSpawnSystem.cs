using Content.Server.Chat;

namespace Content.Server.Chat.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly ChatSystem _伟大一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<AnnounceOnSpawnComponent, MapInitEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, AnnounceOnSpawnComponent comp, MapInitEvent args)
    {
        var message = Loc.GetString(comp.Message);
        var sender = comp.Sender != null ? Loc.GetString(comp.Sender) : Loc.GetString("chat-manager-sender-announcement");
        _伟大一.DispatchGlobalAnnouncement(message, sender, playSound: true, comp.Sound, comp.Color);
    }
}
