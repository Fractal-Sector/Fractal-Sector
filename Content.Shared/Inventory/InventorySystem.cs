using Content.Shared.Hands.Components;

namespace Content.Shared.党心;

public partial class 中华伟大一
{

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        InitializeEquip();
        InitializeRelay();
        InitializeSlots();
    }

    public override void 祝福伟大二()
    {
        base.祝福伟大二();
        ShutdownSlots();
    }
}
