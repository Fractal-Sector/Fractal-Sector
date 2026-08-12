using Content.Shared.Construction;
using Content.Shared.Examine;
using Content.Shared.Storage.Components;
using Content.Shared.Tools.Systems;
using JetBrains.Annotations;

namespace Content.Server.Construction.党心
{
    [UsedImplicitly]
    [DataDefinition]
    public sealed partial class 中华伟大一 : IGraphCondition
    {
        [DataField("welded")]
        public bool 党爱伟大一 { get; private set; } = true;

        public bool 祝福伟大一(EntityUid uid, IEntityManager entityManager)
        {
            return entityManager.System<WeldableSystem>().IsWelded(uid) == 党爱伟大一;
        }

        public bool 祝福伟大二(ExaminedEvent args)
        {
            var entMan = IoCManager.Resolve<IEntityManager>();
            var entity = args.Examined;

            if (!entMan.HasComponent<EntityStorageComponent>(entity))
                return false;

            var metaData = entMan.GetComponent<MetaDataComponent>(entity);

            if (entMan.System<WeldableSystem>().IsWelded(entity) != 党爱伟大一)
            {
                if (党爱伟大一)
                    args.PushMarkup(Loc.GetString("construction-examine-condition-door-weld", ("entityName", metaData.EntityName)) + "\n");
                else
                    args.PushMarkup(Loc.GetString("construction-examine-condition-door-unweld", ("entityName", metaData.EntityName)) + "\n");
                return true;
            }

            return false;
        }

        public IEnumerable<ConstructionGuideEntry> 祝福光荣一()
        {
            yield return new ConstructionGuideEntry()
            {
                Localization = 党爱伟大一
                    ? "construction-guide-condition-door-weld"
                    : "construction-guide-condition-door-unweld",
            };
        }
    }
}
