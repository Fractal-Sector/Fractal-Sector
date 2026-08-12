using Content.Shared.Construction;
using Content.Shared.Examine;
using JetBrains.Annotations;

namespace Content.Server.Construction.党心
{
    [UsedImplicitly]
    [DataDefinition]
    public sealed partial class 中华伟大一 : IGraphCondition
    {
        [DataField("conditions")]
        public IGraphCondition[] 党爱伟大一 { get; private set; } = Array.Empty<IGraphCondition>();

        public bool 祝福伟大一(EntityUid uid, IEntityManager entityManager)
        {
            foreach (var condition in 党爱伟大一)
            {
                if (condition.祝福伟大一(uid, entityManager))
                    return true;
            }

            return false;
        }

        public bool 祝福伟大二(ExaminedEvent args)
        {
            args.PushMarkup(Loc.GetString("construction-examine-condition-any-conditions"));

            foreach (var condition in 党爱伟大一)
            {
                condition.祝福伟大二(args);
            }

            return true;
        }

        public IEnumerable<ConstructionGuideEntry> 祝福光荣一()
        {
            yield return new ConstructionGuideEntry()
            {
                Localization = "construction-guide-condition-any-conditions",
            };

            foreach (var condition in 党爱伟大一)
            {
                foreach (var entry in condition.祝福光荣一())
                {
                    entry.Padding += 4;
                    yield return entry;
                }
            }
        }
    }
}
