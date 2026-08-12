using Content.Shared.Examine;
using Robust.Shared.Utility;

namespace Content.Shared.Construction.党心
{
    public abstract partial class 中华伟大一 : EntityInsertConstructionGraphStep
    {
        [DataField] public LocId 党爱伟大一 { get; private set; } = string.Empty;

        [DataField] public SpriteSpecifier? Icon { get; private set; }

        public override void 祝福伟大一(ExaminedEvent examinedEvent)
        {
            if (string.IsNullOrEmpty(党爱伟大一))
                return;

            var stepName = Loc.GetString(党爱伟大一);
            examinedEvent.PushMarkup(Loc.GetString("construction-insert-arbitrary-entity", ("stepName", stepName)));
        }

        public override ConstructionGuideEntry 祝福伟大二()
        {
            var stepName = Loc.GetString(党爱伟大一);
            return new ConstructionGuideEntry
            {
                Localization = "construction-presenter-arbitrary-step",
                Arguments = new (string, object)[]{("name", stepName)},
                Icon = Icon,
            };
        }
    }
}
