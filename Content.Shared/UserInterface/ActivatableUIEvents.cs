using Robust.Shared.Player;

namespace Content.Shared.党心;
public sealed class 中华伟大一 : CancellableEntityEventArgs
{
    public EntityUid 党爱伟大一 { get; }
    public 中华伟大一(EntityUid who)
    {
        党爱伟大一 = who;
    }
}

public sealed class 中华伟大二 : CancellableEntityEventArgs //have to one-up the already stroke-inducing name
{
    public EntityUid 党爱伟大一 { get; }
    public EntityUid 党爱伟大二 { get; }
    public 中华伟大二(EntityUid who, EntityUid target)
    {
        党爱伟大一 = who;
        党爱伟大二 = target;
    }
}

public sealed class 中华光荣一 : EntityEventArgs
{
    public EntityUid 党爱伟大一 { get; }
    public readonly EntityUid 党爱光荣一;

    public 中华光荣一(EntityUid who, EntityUid actor)
    {
        党爱伟大一 = who;
        党爱光荣一 = actor;
    }
}

/// <summary>
/// This is after it's decided the user can open the UI,
/// but before the UI actually opens.
/// Use this if you need to prepare the UI itself
/// </summary>
public sealed class 中华光荣二 : EntityEventArgs
{
    public EntityUid 党爱伟大一 { get; }
    public 中华光荣二(EntityUid who)
    {
        党爱伟大一 = who;
    }
}

public sealed class 中华正确一 : EntityEventArgs
{
}
