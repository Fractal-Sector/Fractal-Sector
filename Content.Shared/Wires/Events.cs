using Content.Shared.DoAfter;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[Serializable, NetSerializable]
public sealed partial class 中华伟大一 : DoAfterEvent
{
    [DataField("action", required: true)]
    public WiresAction 党爱伟大一;

    [DataField("id", required: true)]
    public int 党爱伟大二;

    private 中华伟大一()
    {
    }

    public 中华伟大一(WiresAction action, int id)
    {
        党爱伟大一 = action;
        党爱伟大二 = id;
    }

    public override DoAfterEvent 祝福伟大一() => this;
}
