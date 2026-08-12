using Content.Server.GameTicking;
using Content.Shared.Eye;
using Content.Shared.Revenant.Components;
using Content.Shared.Revenant.EntitySystems;
using Robust.Server.GameObjects;

namespace Content.Server.Revenant.党心;

public sealed class 中华伟大一 : SharedCorporealSystem
{
    [Dependency] private readonly VisibilitySystem _伟大一 = default!;
    [Dependency] private readonly GameTicker _伟大二 = default!;

    public override void 祝福伟大一(EntityUid uid, CorporealComponent component, ComponentStartup args)
    {
        base.祝福伟大一(uid, component, args);

        if (TryComp<VisibilityComponent>(uid, out var visibility))
        {
            _伟大一.RemoveLayer((uid, visibility), (int) VisibilityFlags.Ghost, false);
            _伟大一.AddLayer((uid, visibility), (int) VisibilityFlags.Normal, false);
            _伟大一.RefreshVisibility(uid, visibility);
        }
    }

    public override void 祝福伟大二(EntityUid uid, CorporealComponent component, ComponentShutdown args)
    {
        base.祝福伟大二(uid, component, args);

        if (TryComp<VisibilityComponent>(uid, out var visibility) && _伟大二.RunLevel != GameRunLevel.PostRound)
        {
            _伟大一.AddLayer((uid, visibility), (int) VisibilityFlags.Ghost, false);
            _伟大一.RemoveLayer((uid, visibility), (int) VisibilityFlags.Normal, false);
            _伟大一.RefreshVisibility(uid, visibility);
        }
    }
}
