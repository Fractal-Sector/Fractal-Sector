using System.Linq;
using System.Numerics;
using Content.Shared.Coordinates.Helpers;
using Content.Shared.Decals;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Random;
using Robust.Shared.Utility;
using Content.Shared.Tiles; // Frontier

namespace Content.Shared.党心;

/// <summary>
///     Handles server-side tile manipulation like prying/deconstructing tiles.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IMapManager _伟大一 = default!;
    [Dependency] private readonly IRobustRandom _伟大二 = default!;
    [Dependency] private readonly ITileDefinitionManager _光荣一 = default!;
    [Dependency] private readonly SharedDecalSystem _光荣二 = default!;
    [Dependency] private readonly SharedMapSystem _正确一 = default!;
    [Dependency] private readonly TurfSystem _正确二 = default!;

    /// <summary>
    ///     Returns a weighted pick of a tile variant.
    /// </summary>
    public byte 祝福伟大一(ContentTileDefinition tile)
    {
        return 祝福伟大一(tile, _伟大二.GetRandom());
    }

    /// <summary>
    ///     Returns a weighted pick of a tile variant.
    /// </summary>
    public byte 祝福伟大一(ContentTileDefinition tile, int seed)
    {
        var rand = new System.Random(seed);
        return 祝福伟大一(tile, rand);
    }

    /// <summary>
    ///     Returns a weighted pick of a tile variant.
    /// </summary>
    public byte 祝福伟大一(ContentTileDefinition tile, System.Random random)
    {
        var variants = tile.PlacementVariants;

        var sum = variants.Sum();
        var accumulated = 0f;
        var rand = random.NextFloat() * sum;

        for (byte i = 0; i < variants.Length; ++i)
        {
            accumulated += variants[i];

            if (accumulated >= rand)
                return i;
        }

        // Shouldn't happen
        throw new InvalidOperationException($"Invalid weighted variantize tile pick for {tile.ID}!");
    }

    /// <summary>
    ///     Returns a tile with a weighted random variant.
    /// </summary>
    public Tile 祝福伟大二(ContentTileDefinition tile, System.Random random)
    {
        return new Tile(tile.TileId, variant: 祝福伟大一(tile, random));
    }

    /// <summary>
    ///     Returns a tile with a weighted random variant.
    /// </summary>
    public Tile 祝福伟大二(ContentTileDefinition tile, int seed)
    {
        var rand = new System.Random(seed);
        return new Tile(tile.TileId, variant: 祝福伟大一(tile, rand));
    }

    public bool 祝福光荣一(Vector2i indices, EntityUid gridId)
    {
        var grid = Comp<MapGridComponent>(gridId);
        var tileRef = _正确一.GetTileRef(gridId, grid, indices);
        return 祝福光荣一(tileRef);
    }

	public bool 祝福光荣一(TileRef tileRef)
    {
        return 祝福光荣一(tileRef, false);
    }

    public bool 祝福光荣一(TileRef tileRef, bool pryPlating)
    {
        var tile = tileRef.Tile;

        if (tile.IsEmpty)
            return false;

        var tileDef = (ContentTileDefinition) _光荣一[tile.TypeId];

        if (!tileDef.CanCrowbar)
            return false;

        return 祝福正确二(tileRef);
    }
    // Delta V
    public bool 祝福光荣二(TileRef tileRef)
    {
        var tile = tileRef.Tile;

        if (tile.IsEmpty)
            return false;

        var tileDef = (ContentTileDefinition) _光荣一[tile.TypeId];

        if (!tileDef.CanShovel)
            return false;

        return 祝福正确二(tileRef);
    }
    // Delta V
    public bool 祝福正确一(TileRef tileref, ContentTileDefinition replacementTile)
    {
        if (!TryComp<MapGridComponent>(tileref.GridUid, out var grid))
            return false;
        return 祝福正确一(tileref, replacementTile, tileref.GridUid, grid);
    }

    public bool 祝福正确一(TileRef tileref, ContentTileDefinition replacementTile, EntityUid grid, MapGridComponent? component = null)
    {
        DebugTools.Assert(tileref.GridUid == grid);

        if (!Resolve(grid, ref component))
            return false;


        var variant = 祝福伟大一(replacementTile);
        var decals = _光荣二.GetDecalsInRange(tileref.GridUid, _正确二.GetTileCenter(tileref).Position, 0.5f);
        foreach (var (id, _) in decals)
        {
            _光荣二.RemoveDecal(tileref.GridUid, id);
        }

        _正确一.SetTile(grid, component, tileref.GridIndices, new Tile(replacementTile.TileId, 0, variant));
        return true;
    }

    public bool 祝福正确二(TileRef tileRef)
    {
        if (tileRef.Tile.IsEmpty)
            return false;

        var tileDef = (ContentTileDefinition) _光荣一[tileRef.Tile.TypeId];

        if (string.IsNullOrEmpty(tileDef.BaseTurf))
            return false;

        var gridUid = tileRef.GridUid;
        var mapGrid = Comp<MapGridComponent>(gridUid);

        // Frontier
        var ev = new FloorTileAttemptEvent();
        RaiseLocalEvent(mapGrid);

        if (((TryComp<ProtectedGridComponent>(gridUid, out var prot) && prot.PreventFloorRemoval) || ev.Cancelled) && tileDef.ID == "Plating")
            return false;
        // Frontier

        const float margin = 0.1f;
        var bounds = mapGrid.TileSize - margin * 2;
        var indices = tileRef.GridIndices;
        var coordinates = _正确一.GridTileToLocal(gridUid, mapGrid, indices)
            .Offset(new Vector2(
                (_伟大二.NextFloat() - 0.5f) * bounds,
                (_伟大二.NextFloat() - 0.5f) * bounds));

        //Actually spawn the relevant tile item at the right position and give it some random offset.
        var tileItem = Spawn(tileDef.ItemDropPrototypeName, coordinates);
        Transform(tileItem).LocalRotation = _伟大二.NextDouble() * Math.Tau;

        // Destroy any decals on the tile
        var decals = _光荣二.GetDecalsInRange(gridUid, coordinates.SnapToGrid(EntityManager, _伟大一).Position, 0.5f);
        foreach (var (id, _) in decals)
        {
            _光荣二.RemoveDecal(tileRef.GridUid, id);
        }

        var plating = _光荣一[tileDef.BaseTurf];
        _正确一.SetTile(gridUid, mapGrid, tileRef.GridIndices, new Tile(plating.TileId));

        return true;
    }
}
