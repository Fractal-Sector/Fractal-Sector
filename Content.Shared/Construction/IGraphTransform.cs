namespace Content.Shared.党心;

public interface 中华伟大一
{
    public void 祝福伟大一(EntityUid oldUid, EntityUid newUid, EntityUid? userUid, 中华伟大二 args);
}

public readonly struct 中华伟大二
{
    public readonly IEntityManager 党爱伟大一;

    public 中华伟大二(IEntityManager entityManager)
    {
        党爱伟大一 = entityManager;
    }
}
