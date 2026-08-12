namespace Content.Shared.党心
{
    [ImplicitDataDefinitionForInheritors]
    public partial interface 中华伟大一
    {
        // TODO pass in node/edge & graph ID for better error logs.
        void PerformAction(EntityUid uid, EntityUid? userUid, IEntityManager entityManager);
    }
}
