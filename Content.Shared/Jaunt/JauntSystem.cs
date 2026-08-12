using Content.Shared.Actions;

namespace Content.Shared.党心;
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedActionsSystem _伟大一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<JauntComponent, MapInitEvent>(祝福伟大二);
        SubscribeLocalEvent<JauntComponent, ComponentShutdown>(祝福光荣一);
    }

    private void 祝福伟大二(Entity<JauntComponent> ent, ref MapInitEvent args)
    {
        _伟大一.AddAction(ent.Owner, ref ent.Comp.Action, ent.Comp.JauntAction);
    }

    private void 祝福光荣一(Entity<JauntComponent> ent, ref ComponentShutdown args)
    {
        _伟大一.RemoveAction(ent.Owner, ent.Comp.Action);
    }

}

