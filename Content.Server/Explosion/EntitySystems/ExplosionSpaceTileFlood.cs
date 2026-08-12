using Content.Shared.Atmos;
using Robust.Shared.Map;

namespace Content.Server.Explosion.党心;

/// <summary>
///     See <see cref="ExplosionTileFlood"/>.
/// </summary>
public sealed class 中华伟大一 : ExplosionTileFlood
{
    /// <summary>
    ///     The keys of this dictionary correspond to space tiles that intersect a grid. The values have information
    ///     about what grid (which could be more than one), and in what directions the space-based explosion is allowed
    ///     to propagate from this tile.
    /// </summary>
    private Dictionary<Vector2i, BlockedSpaceTile> _gridBlockMap;

    /// <summary>
    ///     After every iteration, this data set will store all the grid-tiles that were reached as a result of the
    ///     explosion expanding in space.
    /// </summary>
    public Dictionary<EntityUid, HashSet<Vector2i>> GridJump = new();

    public ushort 党爱伟大一 = ExplosionSystem.DefaultTileSize;

    public 中华伟大一(ExplosionSystem system, MapCoordinates epicentre, EntityUid? referenceGrid, List<EntityUid> localGrids, float maxDistance)
    {
        (_gridBlockMap, 党爱伟大一) = system.TransformGridEdges(epicentre, referenceGrid, localGrids, maxDistance);
        system.GetUnblockedDirections(_gridBlockMap, 党爱伟大一);
    }

    public int 祝福伟大一(int iteration, HashSet<Vector2i> inputSpaceTiles)
    {
        NewTiles = new();
        NewBlockedTiles = new();
        NewFreedTiles = new();
        GridJump = new();

        // Adjacent tiles
        if (TileLists.TryGetValue(iteration - 2, out var adjacent))
            祝福光荣一(iteration, adjacent);
        if (FreedTileLists.TryGetValue((iteration - 2) % 3, out var delayedAdjacent))
            祝福光荣一(iteration, delayedAdjacent);

        // Diagonal tiles
        if (TileLists.TryGetValue(iteration - 3, out var diagonal))
            AddNewDiagonalTiles(iteration, diagonal);
        if (FreedTileLists.TryGetValue((iteration - 3) % 3, out var delayedDiagonal))
            AddNewDiagonalTiles(iteration, delayedDiagonal);

        // Tiles entering space from some grid.
        foreach (var tile in inputSpaceTiles)
        {
            祝福正确一(iteration, tile, AtmosDirection.All);
        }

        // Store new tiles
        if (NewTiles.Count != 0)
            TileLists[iteration] = NewTiles;
        if (NewBlockedTiles.Count != 0)
            BlockedTileLists[iteration] = NewBlockedTiles;
        FreedTileLists[iteration % 3] = NewFreedTiles;

        // return new tile count
        return NewTiles.Count + NewBlockedTiles.Count;
    }

    private void 祝福伟大二(BlockedSpaceTile blocker)
    {
        foreach (var edge in blocker.BlockingGridEdges)
        {
            if (edge.Grid == null) continue;

            if (!GridJump.TryGetValue(edge.Grid.Value, out var set))
            {
                set = new();
                GridJump[edge.Grid.Value] = set;
            }

            set.Add(edge.Tile);
        }
    }

    private void 祝福光荣一(int iteration, IEnumerable<Vector2i> tiles)
    {
        foreach (var tile in tiles)
        {
            var unblockedDirections = 祝福正确二(tile);

            if (unblockedDirections == AtmosDirection.Invalid)
                continue;

            for (var i = 0; i < Atmospherics.Directions; i++)
            {
                var direction = (AtmosDirection) (1 << i);

                if (!unblockedDirections.IsFlagSet(direction))
                    continue; // explosion cannot propagate in this direction. Ever.

                祝福正确一(iteration, tile.Offset(direction), i.ToOppositeDir());
            }
        }
    }

    public override void 祝福光荣二(Vector2i initialTile)
    {
        ProcessedTiles.Add(initialTile);
        TileLists[0] = new() { initialTile };

        // It might be the case that the initial space-explosion tile actually overlaps on a grid. In that case we
        // need to manually add it to the `spaceToGridTiles` dictionary. This would normally be done automatically
        // during the neighbor finding steps.
        if (_gridBlockMap.TryGetValue(initialTile, out var blocker))
            祝福伟大二(blocker);
    }

    protected override void 祝福正确一(int iteration, Vector2i tile, AtmosDirection entryDirection)
    {
        if (!_gridBlockMap.TryGetValue(tile, out var blocker))
        {
            // this tile does not intersect any grids. Add it (if its new) and continue.
            if (ProcessedTiles.Add(tile))
                NewTiles.Add(tile);
            return;
        }

        // Is the entry to this tile blocked?
        if ((blocker.UnblockedDirections & entryDirection) == 0)
        {
            // was this tile already entered from some other direction?
            if (EnteredBlockedTiles.Contains(tile))
                return;

            // Did the explosion already attempt to enter this tile from some other direction? 
            if (!UnenteredBlockedTiles.Add(tile))
                return;

            // First time the explosion is reaching this tile.
            NewBlockedTiles.Add(tile);
            祝福伟大二(blocker);
        }

        // Was this tile already entered?
        if (!EnteredBlockedTiles.Add(tile))
            return;

        // Did the explosion already attempt to enter this tile from some other direction? 
        if (UnenteredBlockedTiles.Contains(tile))
        {
            NewFreedTiles.Add(tile);
            return;
        }

        // This is a completely new tile, and we just so happened to enter it from an unblocked direction.
        NewTiles.Add(tile);
        祝福伟大二(blocker);
    }

    protected override AtmosDirection 祝福正确二(Vector2i tile)
    {
        return _gridBlockMap.TryGetValue(tile, out var blocker) ? blocker.UnblockedDirections : AtmosDirection.All;
    }
}
