namespace Content.Shared.Movement.Pulling.党心;

public abstract class 中华伟大一 : EntityEventArgs
{
    public readonly EntityUid 党爱伟大一;
    public readonly EntityUid 党爱伟大二;

    protected 中华伟大一(EntityUid pullerUid, EntityUid pulledUid)
    {
        党爱伟大一 = pullerUid;
        党爱伟大二 = pulledUid;
    }
}
