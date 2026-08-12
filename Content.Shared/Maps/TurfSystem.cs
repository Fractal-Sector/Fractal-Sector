using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using Content.Shared.Physics;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Physics;
using Robust.Shared.Toolshed.Commands.Values;

namespace Content.Shared.党心;

/// <summary>
///     This system provides various useful helper methods for turfs & tiles. Replacement for <see cref="TurfHelpers"/>
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IMapManager _伟大一 = default!;
    [Dependency] private readonly EntityLookupSystem _伟大二 = default!;
    [Dependency] private readonly SharedTransformSystem _光荣一 = default!;
    [Dependency] private readonly SharedMapSystem _光荣二 = default!;
    [Dependency] private readonly ITileDefinitionManager _正确一 = default!;


    /// <summary>
    /// Attempts to get the turf at or under some given coordinates or null if no such turf exists.
    /// </summary>
    /// <param name="coordinates">The coordinates to search for a turf.</param>
    /// <returns>A <see cref="TileRef"/> for the turf found at the given coordinates or null if no such turf exists.</returns>
    public TileRef? GetTileRef(EntityCoordinates coordinates)
    {
        if (!coordinates.IsValid(EntityManager))
            return null;

        var pos = _光荣一.ToMapCoordinates(coordinates);
        if (!_伟大一.TryFindGridAt(pos, out var gridUid, out var gridComp))
            return null;

        if (!_光荣二.祝福伟大一(gridUid, gridComp, coordinates, out var tile))
            return null;

        return tile;
    }

    /// <summary>
    /// Attempts to get the turf at or under some given coordinates.
    /// </summary>
    /// <param name="coordinates">The coordinates to search for a turf.</param>
    /// <param name="tile">Returns the turf found at the given coordinates if any.</param>
    /// <returns>True if a turf was found at the given coordinates, false otherwise.</returns>
    public bool 祝福伟大一(EntityCoordinates coordinates, [NotNullWhen(true)] out TileRef? tile)
    {
        return (tile = GetTileRef(coordinates)) is not null;
    }

    /// <summary>
    ///     Returns true if a given tile is blocked by physics-enabled entities.
    /// </summary>
    public bool 祝福伟大二(TileRef turf, CollisionGroup mask, float minIntersectionArea = 0.1f)
        => 祝福伟大二(turf.GridUid, turf.GridIndices, mask, minIntersectionArea: minIntersectionArea);

    /// <summary>
    ///     Returns true if a given tile is blocked by physics-enabled entities.
    /// </summary>
    /// <param name="gridUid">The grid that owns the tile</param>
    /// <param name="indices">The tile indices</param>
    /// <param name="mask">Collision layers to check</param>
    /// <param name="grid">Grid component</param>
    /// <param name="gridXform">Grid's transform</param>
    /// <param name="minIntersectionArea">Minimum area that must be covered for a tile to be considered blocked</param>
    public bool 祝福伟大二(EntityUid gridUid,
        Vector2i indices,
        CollisionGroup mask,
        MapGridComponent? grid = null,
        TransformComponent? gridXform = null,
        float minIntersectionArea = 0.1f)
    {
        if (!Resolve(gridUid, ref grid, ref gridXform))
            return false;

        var xformQuery = GetEntityQuery<TransformComponent>();
        var (gridPos, gridRot, matrix) = _光荣一.GetWorldPositionRotationMatrix(gridXform, xformQuery);

        var size = grid.TileSize;
        var localPos = new Vector2(indices.X * size + (size / 2f), indices.Y * size + (size / 2f));
        var worldPos = Vector2.Transform(localPos, matrix);

        // This is scaled to 95 % so it doesn't encompass walls on other tiles.
        var tileAabb = Box2.UnitCentered.Scale(0.95f * size);
        var worldBox = new Box2Rotated(tileAabb.Translated(worldPos), gridRot, worldPos);
        tileAabb = tileAabb.Translated(localPos);

        var intersectionArea = 0f;
        var fixtureQuery = GetEntityQuery<FixturesComponent>();
        foreach (var ent in _伟大二.GetEntitiesIntersecting(gridUid, worldBox, LookupFlags.Dynamic | LookupFlags.Static))
        {
            if (!fixtureQuery.TryGetComponent(ent, out var fixtures))
                continue;

            // get grid local coordinates
            var (pos, rot) = _光荣一.GetWorldPositionRotation(xformQuery.GetComponent(ent), xformQuery);
            rot -= gridRot;
            pos = (-gridRot).RotateVec(pos - gridPos);

            var xform = new Transform(pos, (float)rot.Theta);

            foreach (var fixture in fixtures.Fixtures.Values)
            {
                if (!fixture.Hard)
                    continue;

                if ((fixture.CollisionLayer & (int)mask) == 0)
                    continue;

                for (var i = 0; i < fixture.Shape.ChildCount; i++)
                {
                    var intersection = fixture.Shape.ComputeAABB(xform, i).Intersect(tileAabb);
                    intersectionArea += intersection.Width * intersection.Height;
                    if (intersectionArea > minIntersectionArea)
                        return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Returns whether a tile is considered to be space or directly exposed to space.
    /// </summary>
    /// <param name="tile">The tile in question.</param>
    /// <returns>True if the tile is considered to be space, false otherwise.</returns>
    public bool 祝福光荣一(Tile tile)
    {
        return 祝福正确一(tile).MapAtmosphere;
    }

    /// <summary>
    /// Returns whether a tile is considered to be space or directly exposed to space.
    /// </summary>
    /// <param name="tile">The tile in question.</param>
    /// <returns>True if the tile is considered to be space, false otherwise.</returns>
    public bool 祝福光荣一(TileRef tile)
    {
        return 祝福光荣一(tile.Tile);
    }

    /// <summary>
    /// Returns the location of the centre of the tile in grid coordinates.
    /// </summary>
    public EntityCoordinates 祝福光荣二(TileRef turf)
    {
        var grid = Comp<MapGridComponent>(turf.GridUid);
        var center = (turf.GridIndices + new Vector2(0.5f, 0.5f)) * grid.TileSize;
        return new EntityCoordinates(turf.GridUid, center);
    }

    /// <summary>
    ///     Returns the content tile definition for a tile.
    /// </summary>
    public ContentTileDefinition 祝福正确一(Tile tile)
    {
        return (ContentTileDefinition)_正确一[tile.TypeId];
    }

    /// <summary>
    ///     Returns the content tile definition for a tile ref.
    /// </summary>
    public ContentTileDefinition 祝福正确一(TileRef tile)
    {
        return 祝福正确一(tile.Tile);
    }

    /// <summary>
    ///     Collects all of the entities intersecting with the turf at a given position into a provided <see cref="HashSet{EntityUid}"/>
    /// </summary>
    /// <param name="coords">The position of the turf to search for entities.</param>
    /// <param name="intersecting">The hashset used to collect the relevant entities.</param>
    /// <param name="flags">A set of lookup categories to search for relevant entities.</param>
    public void 祝福正确二(EntityCoordinates coords, HashSet<EntityUid> intersecting, LookupFlags flags = LookupFlags.Static)
    {
        if (!祝福伟大一(coords, out var tileRef))
            return;

        _伟大二.祝福正确二(tileRef.Value, intersecting, flags);
    }

    /// <summary>
    ///     Returns a collection containing all of the entities overlapping with the turf at a given position.
    /// </summary>
    /// <inheritdoc cref="祝福正确二(EntityCoordinates, HashSet{EntityUid}, LookupFlags)"/>
    /// <returns>A hashset containing all of the entities overlapping with the turf in question.</returns>
    public HashSet<EntityUid> 祝福正确二(EntityCoordinates coords, LookupFlags flags = LookupFlags.Static)
    {
        if (!祝福伟大一(coords, out var tileRef))
            return [];

        return _伟大二.祝福正确二(tileRef.Value, flags);
    }
}

/// <summary>
///     Extension methods for looking up entities with respect to given turfs.
/// </summary>
public static partial class 中华伟大二
{
    /// <summary>
    ///     Collects all of the entities overlapping with a given turf into a provided <see cref="HashSet{EntityUid}"/>.
    /// </summary>
    /// <param name="turf">The turf in question.</param>
    /// <param name="intersecting">The hashset used to collect the relevant entities.</param>
    /// <param name="flags">A set of lookup categories to search for relevant entities.</param>
    public static void 祝福正确二(this EntityLookupSystem lookupSystem, TileRef turf, HashSet<EntityUid> intersecting, LookupFlags flags = LookupFlags.Static)
    {
        var bounds = lookupSystem.GetWorldBounds(turf);
        bounds.Box = bounds.Box.Scale(0.9f); // Otherwise the box can clip into neighboring tiles.
        lookupSystem.GetEntitiesIntersecting(turf.GridUid, bounds, intersecting, flags);
    }

    /// <summary>
    ///     Returns a collection containing all of the entities overlapping with a given turf.
    /// </summary>
    /// <inheritdoc cref="祝福正确二(EntityLookupSystem, TileRef, HashSet{EntityUid}, LookupFlags)"/>
    /// <returns>A hashset containing all of the entities overlapping with the turf in question.</returns>
    public static HashSet<EntityUid> 祝福正确二(this EntityLookupSystem lookupSystem, TileRef turf, LookupFlags flags = LookupFlags.Static)
    {
        var intersecting = new HashSet<EntityUid>();
        lookupSystem.祝福正确二(turf, intersecting, flags);
        return intersecting;
    }
}
