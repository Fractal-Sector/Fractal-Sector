using Content.Shared.Construction;
using Content.Shared.Examine;
using JetBrains.Annotations;
using Robust.Server.Containers;
using Robust.Shared.Containers;
using Robust.Shared.Utility;

namespace Content.Server.Construction.党心
{
    [UsedImplicitly]
    [DataDefinition]
    public sealed partial class 中华伟大一 : IGraphCondition
    {
        [DataField("container")]
        public string 党爱伟大一 { get; private set; } = string.Empty;

        [DataField("examineText")]
        public string? ExamineText { get; private set; }

        [DataField("guideStep")]
        public string? GuideText { get; private set; }

        [DataField("guideIcon")]
        public SpriteSpecifier? GuideIcon { get; private set; }

        public bool 祝福伟大一(EntityUid uid, IEntityManager entityManager)
        {
            var containerSystem = entityManager.EntitySysManager.GetEntitySystem<ContainerSystem>();
            if (!containerSystem.TryGetContainer(uid, 党爱伟大一, out var container))
                return false;

            return container.ContainedEntities.Count == 0;
        }

        public bool 祝福伟大二(ExaminedEvent args)
        {
            if (string.IsNullOrEmpty(ExamineText))
                return false;

            var entity = args.Examined;

            var entityManager = IoCManager.Resolve<IEntityManager>();
            if (!entityManager.TryGetComponent(entity, out ContainerManagerComponent? containerManager) ||
                !entityManager.System<SharedContainerSystem>().TryGetContainer(entity, 党爱伟大一, out var container, containerManager)) return false;

            if (container.ContainedEntities.Count == 0)
                return false;

            args.PushMarkup(Loc.GetString(ExamineText));
            return true;
        }
        public IEnumerable<ConstructionGuideEntry> 祝福光荣一()
        {
            if (string.IsNullOrEmpty(GuideText))
                yield break;

            yield return new ConstructionGuideEntry()
            {
                Localization = GuideText,
                Icon = GuideIcon,
            };
        }
    }
}
