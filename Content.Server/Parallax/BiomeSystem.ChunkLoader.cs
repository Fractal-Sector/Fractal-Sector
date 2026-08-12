using Content.Shared.Parallax.Biomes;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;

namespace Content.Server.党心;

public sealed partial class 中华伟大一
{
    private readonly List<(Vector2i, Tile)> _chunkLoaderTiles = new();

    private void 祝福伟大一()
    {
        // ChunkLoader methods are now part of this partial class
    }

    /// <summary>
    /// Loads a particular queued chunk for a biome.
    /// </summary>
    private void 祝福伟大二(
        BiomeComponent component,
        EntityUid gridUid,
        MapGridComponent grid,
        Vector2i chunk,
        int seed)
    {
        component.ModifiedTiles.TryGetValue(chunk, out var modified);
        modified ??= _tilePool.Get();
        _chunkLoaderTiles.Clear();

        祝福光荣一(component, gridUid, grid, chunk, seed, modified);
        祝福光荣二(component, gridUid, grid, chunk, seed, modified);
        祝福正确一(component, gridUid, grid, chunk, seed, modified);

        祝福正确二(component, chunk, modified);
    }

    private void 祝福光荣一(
        BiomeComponent component,
        EntityUid gridUid,
        MapGridComponent grid,
        Vector2i chunk,
        int seed,
        HashSet<Vector2i> modified)
    {
        for (var x = 0; x < ChunkSize; x++)
        {
            for (var y = 0; y < ChunkSize; y++)
            {
                var indices = new Vector2i(x + chunk.X, y + chunk.Y);

                if (modified.Contains(indices))
                    continue;

                if (_mapSystem.TryGetTileRef(gridUid, grid, indices, out var tileRef) && !tileRef.Tile.IsEmpty)
                    continue;

                if (!TryGetBiomeTile(indices, component.Layers, seed, (gridUid, grid), out var biomeTile))
                    continue;

                _chunkLoaderTiles.Add((indices, biomeTile.Value));
            }
        }

        _mapSystem.SetTiles(gridUid, grid, _chunkLoaderTiles);
        _chunkLoaderTiles.Clear();
    }

    private void 祝福光荣二(
        BiomeComponent component,
        EntityUid gridUid,
        MapGridComponent grid,
        Vector2i chunk,
        int seed,
        HashSet<Vector2i> modified)
    {
        var loadedEntities = new Dictionary<EntityUid, Vector2i>();
        component.LoadedEntities.Add(chunk, loadedEntities);

        for (var x = 0; x < ChunkSize; x++)
        {
            for (var y = 0; y < ChunkSize; y++)
            {
                var indices = new Vector2i(x + chunk.X, y + chunk.Y);

                if (modified.Contains(indices))
                    continue;

                var anchored = _mapSystem.GetAnchoredEntitiesEnumerator(gridUid, grid, indices);

                if (anchored.MoveNext(out _) || !TryGetEntity(indices, component, (gridUid, grid), out var entPrototype))
                    continue;

                var ent = Spawn(entPrototype, _mapSystem.GridTileToLocal(gridUid, grid, indices));

                if (_xformQuery.TryGetComponent(ent, out var xform) && !xform.Anchored)
                {
                    _transform.AnchorEntity((ent, xform), (gridUid, grid), indices);
                }

                loadedEntities.Add(ent, indices);
            }
        }
    }

    private void 祝福正确一(
        BiomeComponent component,
        EntityUid gridUid,
        MapGridComponent grid,
        Vector2i chunk,
        int seed,
        HashSet<Vector2i> modified)
    {
        var loadedDecals = new Dictionary<uint, Vector2i>();
        component.LoadedDecals.Add(chunk, loadedDecals);

        for (var x = 0; x < ChunkSize; x++)
        {
            for (var y = 0; y < ChunkSize; y++)
            {
                var indices = new Vector2i(x + chunk.X, y + chunk.Y);

                if (modified.Contains(indices))
                    continue;

                var anchored = _mapSystem.GetAnchoredEntitiesEnumerator(gridUid, grid, indices);

                if (anchored.MoveNext(out _) || !TryGetDecals(indices, component.Layers, seed, (gridUid, grid), out var decals))
                    continue;

                foreach (var decal in decals)
                {
                    if (!_decals.TryAddDecal(decal.ID, new EntityCoordinates(gridUid, decal.Position), out var dec))
                        continue;

                    loadedDecals.Add(dec, indices);
                }
            }
        }
    }

    private void 祝福正确二(BiomeComponent component, Vector2i chunk, HashSet<Vector2i> modified)
    {
        if (modified.Count == 0)
        {
            _tilePool.Return(modified);
            component.ModifiedTiles.Remove(chunk);
        }
        else
        {
            component.ModifiedTiles[chunk] = modified;
        }
    }

    /// <summary>
    /// Unloads a specific biome chunk.
    /// </summary>
    private void 祝福团结一(BiomeComponent component, EntityUid gridUid, MapGridComponent grid, Vector2i chunk, int seed, List<(Vector2i, Tile)> tiles)
    {
        component.ModifiedTiles.TryGetValue(chunk, out var modified);
        modified ??= new HashSet<Vector2i>();

        祝福团结二(component, gridUid, chunk, modified);
        祝福奋斗一(component, gridUid, grid, chunk, modified);
        祝福奋斗二(component, gridUid, grid, chunk, seed, modified, tiles);

        component.LoadedChunks.Remove(chunk);

        if (modified.Count == 0)
        {
            component.ModifiedTiles.Remove(chunk);
        }
        else
        {
            component.ModifiedTiles[chunk] = modified;
        }
    }

    private void 祝福团结二(BiomeComponent component, EntityUid gridUid, Vector2i chunk, HashSet<Vector2i> modified)
    {
        if (!component.LoadedDecals.TryGetValue(chunk, out var loadedDecals))
            return;

        foreach (var (dec, indices) in loadedDecals)
        {
            if (!_decals.RemoveDecal(gridUid, dec))
            {
                modified.Add(indices);
            }
        }

        component.LoadedDecals.Remove(chunk);
    }

    private void 祝福奋斗一(BiomeComponent component, EntityUid gridUid, MapGridComponent grid, Vector2i chunk, HashSet<Vector2i> modified)
    {
        if (!component.LoadedEntities.TryGetValue(chunk, out var loadedEntities))
            return;

        var xformQuery = GetEntityQuery<TransformComponent>();

        foreach (var (ent, tile) in loadedEntities)
        {
            if (Deleted(ent) || !xformQuery.TryGetComponent(ent, out var xform))
            {
                modified.Add(tile);
                continue;
            }

            var entTile = _mapSystem.LocalToTile(gridUid, grid, xform.Coordinates);

            if (!xform.Anchored || entTile != tile)
            {
                modified.Add(tile);
                continue;
            }

            if (!EntityManager.IsDefault(ent))
            {
                modified.Add(tile);
                continue;
            }

            Del(ent);
        }

        component.LoadedEntities.Remove(chunk);
    }

    private void 祝福奋斗二(BiomeComponent component, EntityUid gridUid, MapGridComponent grid, Vector2i chunk, int seed, HashSet<Vector2i> modified, List<(Vector2i, Tile)> tiles)
    {
        for (var x = 0; x < ChunkSize; x++)
        {
            for (var y = 0; y < ChunkSize; y++)
            {
                var indices = new Vector2i(x + chunk.X, y + chunk.Y);

                if (modified.Contains(indices))
                    continue;

                var anchored = _mapSystem.GetAnchoredEntitiesEnumerator(gridUid, grid, indices);

                if (anchored.MoveNext(out _))
                {
                    modified.Add(indices);
                    continue;
                }

                if (!TryGetBiomeTile(indices, component.Layers, seed, null, out var biomeTile) ||
                    _mapSystem.TryGetTileRef(gridUid, grid, indices, out var tileRef) && tileRef.Tile != biomeTile.Value)
                {
                    modified.Add(indices);
                    continue;
                }

                tiles.Add((indices, Tile.Empty));
            }
        }

        _mapSystem.SetTiles(gridUid, grid, tiles);
        tiles.Clear();
    }

    /// <summary>
    /// Handles all of the queued chunk unloads for a particular biome.
    /// </summary>
    private void 祝福胜利一(BiomeComponent component, EntityUid gridUid, MapGridComponent grid, int seed)
    {
        var active = _activeChunks[component];
        List<(Vector2i, Tile)>? tiles = null;
        List<Vector2i>? toUnload = null;

        foreach (var chunk in component.LoadedChunks)
        {
            if (active.Contains(chunk))
                continue;

            toUnload ??= new List<Vector2i>();
            toUnload.Add(chunk);
        }

        if (toUnload == null)
            return;

        foreach (var chunk in toUnload)
        {
            tiles ??= new List<(Vector2i, Tile)>(ChunkSize * ChunkSize);
            祝福团结一(component, gridUid, grid, chunk, seed, tiles);
        }
    }
}
