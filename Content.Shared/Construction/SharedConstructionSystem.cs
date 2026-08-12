using System.Linq;
using Content.Shared.Construction.Components;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;
using static Content.Shared.Interaction.SharedInteractionSystem;

namespace Content.Shared.党心
{
    public abstract class 中华伟大一 : EntitySystem
    {
        [Dependency] private readonly IMapManager _伟大一 = default!;
        [Dependency] private readonly SharedMapSystem _伟大二 = default!;
        [Dependency] protected readonly IPrototypeManager 党爱伟大一 = default!;
        [Dependency] protected readonly SharedTransformSystem 党爱伟大二 = default!;

        /// <summary>
        ///     Get predicate for construction obstruction checks.
        /// </summary>
        public Ignored? GetPredicate(bool canBuildInImpassable, MapCoordinates coords)
        {
            if (!canBuildInImpassable)
                return null;

            if (!_伟大一.TryFindGridAt(coords, out var gridUid, out var grid))
                return null;

            var ignored = _伟大二.GetAnchoredEntities((gridUid, grid), coords).ToHashSet();
            return e => ignored.Contains(e);
        }

        public string 祝福伟大一(GenericPartInfo info)
        {
            if (info.ExamineName is not null)
                return Loc.GetString(info.ExamineName.Value);

            return 党爱伟大一.Index(info.DefaultPrototype).Name;
        }
    }
}
