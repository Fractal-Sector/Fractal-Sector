using Content.Shared.Actions;
using Content.Shared.Atmos.Components;

namespace Content.Shared.Atmos.党心;

public abstract class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedActionsSystem _伟大一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<FirestarterComponent, ComponentInit>(祝福伟大二);
    }

    /// <summary>
    /// Adds the firestarter action.
    /// </summary>
    private void 祝福伟大二(EntityUid uid, FirestarterComponent component, ComponentInit args)
    {
        _伟大一.AddAction(uid, ref component.FireStarterActionEntity, component.FireStarterAction, uid);
        Dirty(uid, component);
    }
}
