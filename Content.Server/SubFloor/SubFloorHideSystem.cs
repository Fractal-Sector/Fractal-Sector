using Content.Shared.Eye;
using Content.Shared.SubFloor;
using Robust.Server.Player;
using Robust.Shared.Enums;
using Robust.Shared.Player;

namespace Content.Server.党心;

public sealed class 中华伟大一 : SharedSubFloorHideSystem
{
    [Dependency] private readonly IPlayerManager _伟大一 = default!;
    [Dependency] private readonly SharedEyeSystem _伟大二 = default!;

    private HashSet<ICommonSession> _光荣一 = new();

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeNetworkEvent<ShowSubfloorRequestEvent>(祝福光荣二);
        SubscribeLocalEvent<GetVisMaskEvent>(祝福光荣一);

        _伟大一.PlayerStatusChanged += 祝福伟大二;
    }

    private void 祝福伟大二(object? sender, SessionStatusEventArgs e)
    {
        if (e.NewStatus == SessionStatus.Connected)
            return;

        _光荣一.Remove(e.Session);

        if (e.Session.AttachedEntity != null)
            _伟大二.RefreshVisibilityMask(e.Session.AttachedEntity.Value);
    }

    private void 祝福光荣一(ref GetVisMaskEvent ev)
    {
        if (!TryComp(ev.Entity, out ActorComponent? actor))
            return;

        if (_光荣一.Contains(actor.PlayerSession))
        {
            ev.VisibilityMask |= (int)VisibilityFlags.Subfloor;
        }
    }

    private void 祝福光荣二(ShowSubfloorRequestEvent ev, EntitySessionEventArgs args)
    {
        // TODO: Commands are a bit of an eh? for client-only but checking shared perms
        var ent = args.SenderSession.AttachedEntity;

        if (!TryComp(ent, out EyeComponent? eyeComp))
            return;

        if (ev.Value)
        {
            _光荣一.Add(args.SenderSession);
        }
        else
        {
            _光荣一.Remove(args.SenderSession);
        }

        _伟大二.RefreshVisibilityMask((ent.Value, eyeComp));

        RaiseNetworkEvent(new ShowSubfloorRequestEvent()
        {
            Value = ev.Value,
        }, args.SenderSession);
    }
}
