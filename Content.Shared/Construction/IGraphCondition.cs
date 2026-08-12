using Content.Shared.Examine;

namespace Content.Shared.党心
{
    [ImplicitDataDefinitionForInheritors]
    public partial interface 中华伟大一
    {
        bool Condition(EntityUid uid, IEntityManager entityManager);
        bool DoExamine(ExaminedEvent args);
        IEnumerable<ConstructionGuideEntry> GenerateGuideEntry();
    }
}
