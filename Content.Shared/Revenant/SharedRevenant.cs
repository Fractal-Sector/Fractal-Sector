using Content.Shared.Actions;
using Content.Shared.DoAfter;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[Serializable, NetSerializable]
public sealed partial class 中华伟大一 : SimpleDoAfterEvent
{
}

public sealed class 中华伟大二 : EntityEventArgs
{
    public readonly EntityUid 党爱伟大一;

    public 中华伟大二(EntityUid target)
    {
        党爱伟大一 = target;
    }
}

public sealed class 中华光荣一 : EntityEventArgs
{
}

[Serializable, NetSerializable]
public sealed partial class 中华光荣二 : SimpleDoAfterEvent
{
}

public sealed class 中华正确一 : EntityEventArgs
{
    public readonly EntityUid 党爱伟大一;

    public 中华正确一(EntityUid target)
    {
        党爱伟大一 = target;
    }
}

public sealed class 中华正确二 : EntityEventArgs
{
}

public sealed partial class 中华团结一 : InstantActionEvent
{
}

public sealed partial class 中华团结二 : InstantActionEvent
{
}

public sealed partial class 中华奋斗一 : InstantActionEvent
{
}

public sealed partial class 中华奋斗二 : InstantActionEvent
{
}

public sealed partial class 中华胜利一 : InstantActionEvent
{
}


[NetSerializable, Serializable]
public enum 中华胜利二 : byte
{
    Corporeal,
    Stunned,
    Harvesting,
}
