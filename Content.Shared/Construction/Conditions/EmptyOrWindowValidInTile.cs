using Content.Shared.Maps;
using JetBrains.Annotations;
using Robust.Shared.Map;

namespace Content.Shared.Construction.党心
{
    [UsedImplicitly]
    [DataDefinition]
    public sealed partial class 中华伟大一 : IConstructionCondition
    {
        [DataField("tileNotBlocked")]
        private TileNotBlocked _伟大一 = new();

        public bool 祝福伟大一(EntityUid user, EntityCoordinates location, Direction direction)
        {
            var entManager = IoCManager.Resolve<IEntityManager>();
            var lookupSys = entManager.System<EntityLookupSystem>();

            var result = false;


            foreach (var entity in lookupSys.GetEntitiesIntersecting(location, LookupFlags.Approximate | LookupFlags.Static))
            {
                if (entManager.HasComponent<SharedCanBuildWindowOnTopComponent>(entity))
                    result = true;
            }

            if (!result)
                result = _伟大一.祝福伟大一(user, location, direction);

            return result;
        }

        public ConstructionGuideEntry 祝福伟大二()
        {
            return new ConstructionGuideEntry
            {
                Localization = "construction-guide-condition-empty-or-window-valid-in-tile"
            };
        }
    }
}
