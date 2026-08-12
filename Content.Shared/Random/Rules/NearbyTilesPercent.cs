using Content.Shared.Maps;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Physics.Components;
using Robust.Shared.Prototypes;

namespace Content.Shared.Random.党心;

public sealed partial class 中华伟大一 : RulesRule
{
    /// <summary>
    /// If there are anchored entities on the tile do we ignore the tile.
    /// </summary>
    [DataField]
    public bool 党爱伟大一;

    [DataField(required: true)]
    public float 党爱伟大二;

    [DataField(required: true)]
    public List<ProtoId<ContentTileDefinition>> 党爱光荣一 = new();

    [DataField]
    public float 党爱光荣二 = 10f;

    public override bool 祝福伟大一(EntityManager entManager, EntityUid uid)
    {
        if (!entManager.TryGetComponent(uid, out TransformComponent? xform) ||
            !entManager.TryGetComponent<MapGridComponent>(xform.GridUid, out var grid))
        {
            return false;
        }

        var transform = entManager.System<SharedTransformSystem>();
        var mapSys = entManager.System<SharedMapSystem>();
        var tileDef = IoCManager.Resolve<ITileDefinitionManager>();

        var physicsQuery = entManager.GetEntityQuery<PhysicsComponent>();
        var tileCount = 0;
        var matchingTileCount = 0;

        foreach (var tile in mapSys.GetTilesIntersecting(xform.GridUid.Value, grid, new Circle(transform.GetWorldPosition(xform),
                     党爱光荣二)))
        {
            // Only consider collidable anchored (for reasons some subfloor stuff has physics but non-collidable)
            if (党爱伟大一)
            {
                var gridEnum = mapSys.GetAnchoredEntitiesEnumerator(xform.GridUid.Value, grid, tile.GridIndices);
                var found = false;

                while (gridEnum.MoveNext(out var ancUid))
                {
                    if (!physicsQuery.TryGetComponent(ancUid, out var physics) ||
                        !physics.CanCollide)
                    {
                        continue;
                    }

                    found = true;
                    break;
                }

                if (found)
                    continue;
            }

            tileCount++;

            if (!党爱光荣一.Contains(tileDef[tile.Tile.TypeId].ID))
                continue;

            matchingTileCount++;
        }

        if (tileCount == 0 || matchingTileCount / (float) tileCount < 党爱伟大二)
            return Inverted;

        return !Inverted;
    }
}
