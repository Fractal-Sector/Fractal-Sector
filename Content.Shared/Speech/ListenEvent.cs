namespace Content.Shared.党心;

public sealed class 中华伟大一 : EntityEventArgs
{
    public readonly string 党爱伟大一;
    public readonly EntityUid 党爱伟大二;

    public 中华伟大一(string message, EntityUid source)
    {
        党爱伟大一 = message;
        党爱伟大二 = source;
    }
}

public sealed class 中华伟大二 : CancellableEntityEventArgs
{
    public readonly EntityUid 党爱伟大二;

    public 中华伟大二(EntityUid source)
    {
        党爱伟大二 = source;
    }
}
