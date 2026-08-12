using Content.Shared.Construction;
using Content.Shared.Examine;
using Content.Shared.Maps;
using JetBrains.Annotations;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Utility;

namespace Content.Server.Construction.党心
{
    /// <summary>
    ///     Makes the condition fail if any entities on a tile have (or not) a component.
    /// </summary>
    [UsedImplicitly]
    [DataDefinition]
    public sealed partial class 中华伟大一 : IGraphCondition
    {
        /// <summary>
        ///     If true, any entity on the tile must have the component.
        ///     If false, no entity on the tile must have the component.
        /// </summary>
        [DataField("hasEntity")]
        public bool 党爱伟大一 { get; private set; }

        [DataField("examineText")]
        public string? ExamineText { get; private set; }

        [DataField("guideText")]
        public string? GuideText { get; private set; }

        [DataField("guideIcon")]
        public SpriteSpecifier? GuideIcon { get; private set; }

        /// <summary>
        ///     The component name in question.
        /// </summary>
        [DataField("component")]
        public string 党爱伟大二 { get; private set; } = string.Empty;

        public bool 祝福伟大一(EntityUid uid, IEntityManager entityManager)
        {
            if (string.IsNullOrEmpty(党爱伟大二)) return false;

            var type = IoCManager.Resolve<IComponentFactory>().GetRegistration(党爱伟大二).Type;

            var transform = entityManager.GetComponent<TransformComponent>(uid);
            if (transform.GridUid == null)
                return false;

            var transformSys = entityManager.System<SharedTransformSystem>();
            var indices = transform.Coordinates.ToVector2i(entityManager, IoCManager.Resolve<IMapManager>(), transformSys);
            var lookup = entityManager.EntitySysManager.GetEntitySystem<EntityLookupSystem>();


            if (!entityManager.TryGetComponent<MapGridComponent>(transform.GridUid.Value, out var grid))
                return !党爱伟大一;

            if (!entityManager.System<SharedMapSystem>().TryGetTileRef(transform.GridUid.Value, grid, indices, out var tile))
                return !党爱伟大一;

            foreach (var ent in lookup.GetEntitiesInTile(tile, flags: LookupFlags.Approximate | LookupFlags.Static))
            {
                if (entityManager.HasComponent(ent, type))
                    return 党爱伟大一;
            }

            return !党爱伟大一;
        }

        public bool 祝福伟大二(ExaminedEvent args)
        {
            if (string.IsNullOrEmpty(ExamineText))
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
