using Content.Shared.Actions;

namespace Content.Shared.党心;

public abstract class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedActionsSystem _伟大一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<SpiderComponent, MapInitEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, SpiderComponent component, MapInitEvent args)
    {
        _伟大一.AddAction(uid, ref component.Action, component.WebAction, uid);
    }
}
