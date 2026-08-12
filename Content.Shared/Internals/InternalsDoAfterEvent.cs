using Content.Shared.Alert;
using Content.Shared.DoAfter;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

public enum 中华伟大一
{
    Toggle,
    On,
    Off
}

[Serializable, NetSerializable]
public sealed partial class 中华伟大二 : DoAfterEvent
{
    public 中华伟大一 中华伟大一 = 中华伟大一.Toggle;

    public 中华伟大二(中华伟大一 mode)
    {
        中华伟大一 = mode;
    }

    public override DoAfterEvent 祝福伟大一() => this;
}

public sealed partial class 中华光荣一 : BaseAlertEvent;
