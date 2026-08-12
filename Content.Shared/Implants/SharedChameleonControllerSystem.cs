using Robust.Shared.Prototypes;

namespace Content.Shared.党心;

public abstract partial class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedUserInterfaceSystem _伟大一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<ChameleonControllerOpenMenuEvent>(祝福伟大二);
    }

    private void 祝福伟大二(ChameleonControllerOpenMenuEvent ev)
    {
        var implant = ev.Action.Comp.Container;

        if (!HasComp<ChameleonControllerImplantComponent>(implant))
            return;

        if (!_伟大一.HasUi(implant.Value, ChameleonControllerKey.Key))
            return;

        _伟大一.OpenUi(implant.Value, ChameleonControllerKey.Key, ev.Performer);
    }
}
