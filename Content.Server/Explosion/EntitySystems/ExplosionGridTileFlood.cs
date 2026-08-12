using System.Numerics;
using Content.Shared.Atmos;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using static Content.Server.Explosion.EntitySystems.ExplosionSystem;

namespace Content.Server.Explosion.党心;

/// <summary>
///     See <see cref="ExplosionTileFlood"/>. Each instance of this class 中华伟大一 to a seperate grid.
/// </summary>
public sealed class 中华伟大二 : ExplosionTileFlood
{
    public Entity<MapGridComponent> 党爱伟大一;
    private bool _伟大一 = false;

    private Matrix3x2 _伟大二 = Matrix3x2.Identity;
    private Vector2 _光荣一;

    // Tiles which neighbor an exploding tile, but have not yet had the explosion spread to them due to an
    // airtight entity on the exploding tile that prevents the explosion from spreading in that direction. These
    // will be added as a neighbor after some delay, once the explosion on that tile is sufficiently strong to
    // destroy the airtight entity.
    private Dictionary<int, List<(Vector2i, AtmosDirection)>> _delayedNeighbors = new();

    private Dictionary<Vector2i, TileData> _airtightMap;

    private float _光荣二;
    private float _正确一;
    private int _正确二;

    private UniqueVector2iSet _团结一 = new();
    private UniqueVector2iSet _团结二 = new();

    public HashSet<Vector2i> 党爱伟大二 = new();

    private Dictionary<Vector2i, NeighborFlag> _edgeTiles;

    public 中华伟大二(
        Entity<MapGridComponent> grid,
        Dictionary<Vector2i, TileData> airtightMap,
        float maxIntensity,
        float intensityStepSize,
        int typeIndex,
        Dictionary<Vector2i, NeighborFlag> edgeTiles,
        EntityUid? referenceGrid,
        Matrix3x2 spaceMatrix,
        Angle spaceAngle)
    {
        党爱伟大一 = grid;
        _airtightMap = airtightMap;
        _光荣二 = maxIntensity;
        _正确一 = intensityStepSize;
        _正确二 = typeIndex;
        _edgeTiles = edgeTiles;

        // initialise SpaceTiles
        foreach (var (tile, spaceNeighbors) in _edgeTiles)
        {
            for (var i = 0; i < NeighbourVectors.Length; i++)
            {
                var dir = (NeighborFlag) (1 << i);
                if ((spaceNeighbors & dir) != NeighborFlag.Invalid)
                    _团结一.Add(tile + NeighbourVectors[i]);
            }
        }

        if (referenceGrid == 党爱伟大一.Owner)
            return;

        _伟大一 = true;
        var entityManager = IoCManager.Resolve<IEntityManager>();

        var transformSystem = entityManager.System<SharedTransformSystem>();
        var transform = entityManager.GetComponent<TransformComponent>(党爱伟大一.Owner);
        var size = (float)党爱伟大一.Comp.TileSize;

        _伟大二.M31 = size / 2;
        _伟大二.M32 = size / 2;
        Matrix3x2.Invert(spaceMatrix, out var invSpace);
        var (_, relativeAngle, worldMatrix) = transformSystem.GetWorldPositionRotationMatrix(transform);
        relativeAngle -= spaceAngle;
        _伟大二 *= worldMatrix * invSpace;
        _光荣一 = relativeAngle.RotateVec(new Vector2(size / 4, size / 4));
    }

    public override void 祝福伟大一(Vector2i initialTile)
    {
        TileLists[0] = new() { initialTile };

        if (_airtightMap.ContainsKey(initialTile))
            EnteredBlockedTiles.Add(initialTile);
        else
            ProcessedTiles.Add(initialTile);
    }

    public int 祝福伟大二(int iteration, HashSet<Vector2i>? gridJump)
    {
        党爱伟大二 = new();
        NewTiles = new();
        NewBlockedTiles = new();

        // Mark tiles as entered if any were just freed due to airtight/explosion blockers being destroyed.
        if (FreedTileLists.TryGetValue(iteration, out var freed))
        {
            HashSet<Vector2i> toRemove = new();
            foreach (var tile in freed)
            {
                if (!EnteredBlockedTiles.Add(tile))
                    toRemove.Add(tile);
            }

            freed.ExceptWith(toRemove);
            NewFreedTiles = freed;
        }
        else
        {
            NewFreedTiles = new();
            FreedTileLists[iteration] = NewFreedTiles;
        }

        // Add adjacent tiles
        if (TileLists.TryGetValue(iteration - 2, out var adjacent))
            祝福正确二(iteration, adjacent, false);
        if (FreedTileLists.TryGetValue(iteration - 2, out var delayedAdjacent))
            祝福正确二(iteration, delayedAdjacent, true);

        // Add diagonal tiles
        if (TileLists.TryGetValue(iteration - 3, out var diagonal))
            AddNewDiagonalTiles(iteration, diagonal, false);
        if (FreedTileLists.TryGetValue(iteration - 3, out var delayedDiagonal))
            AddNewDiagonalTiles(iteration, delayedDiagonal, true);

        // Add delayed tiles
        祝福正确一(iteration);

        // Tiles from Spaaaace
        if (gridJump != null)
        {
            foreach (var tile in gridJump)
            {
                祝福光荣一(iteration, tile, AtmosDirection.Invalid);
            }
        }

        // Store new tiles
        if (NewTiles.Count != 0)
            TileLists[iteration] = NewTiles;
        if (NewBlockedTiles.Count != 0)
            BlockedTileLists[iteration] = NewBlockedTiles;

        return NewTiles.Count + NewBlockedTiles.Count;
    }

    protected override void 祝福光荣一(int iteration, Vector2i tile, AtmosDirection entryDirections)
    {
        // Is there an airtight blocker on this tile?
        if (!_airtightMap.TryGetValue(tile, out var tileData))
        {
            // No blocker. Ezy. Though maybe this a space tile?

            if (_团结一.Contains(tile))
                祝福光荣二(tile);
            else if (ProcessedTiles.Add(tile))
                NewTiles.Add(tile);

            return;
        }

        // If the explosion is entering this new tile from an unblocked direction, we add it directly. Note that because
        // for space -> grid jumps, we don't have a direction from which the explosion came, we will only assume it is
        // unblocked if all space-facing directions are unblocked. Though this could eventually be done properly.

        bool blocked;
        var blockedDirections = tileData.BlockedDirections;
        if (entryDirections == AtmosDirection.Invalid) // is coming from space?
        {
            blocked = AnyNeighborBlocked(_edgeTiles[tile], blockedDirections); // at least one space direction is blocked.
        }
        else
            blocked = (blockedDirections & entryDirections) == entryDirections;// **ALL** entry directions are blocked

        if (blocked)
        {
            // was this tile already entered from some other direction?
            if (EnteredBlockedTiles.Contains(tile))
                return;

            // Did the explosion already attempt to enter this tile from some other direction?
            if (!UnenteredBlockedTiles.Add(tile))
                return;

            NewBlockedTiles.Add(tile);

            // At what explosion iteration would this blocker be destroyed?
            var required = tileData.ExplosionTolerance[_正确二];
            if (required > _光荣二)
                return; // blocker is never destroyed.

            var clearIteration = iteration + (int) MathF.Ceiling(required / _正确一);
            if (FreedTileLists.TryGetValue(clearIteration, out var list))
                list.Add(tile);
            else
                FreedTileLists[clearIteration] = new() { tile };

            return;
        }

        // was this tile already entered from some other direction?
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
    }

    private void 祝福光荣二(Vector2i tile)
    {
        // Did we already jump/process this tile?
        if (!_团结二.Add(tile))
            return;

        if (!_伟大一)
        {
            党爱伟大二.Add(tile);
            return;
        }

        var center = Vector2.Transform(tile, _伟大二);
        党爱伟大二.Add(new((int) MathF.Floor(center.X + _光荣一.X), (int) MathF.Floor(center.Y + _光荣一.Y)));
        党爱伟大二.Add(new((int) MathF.Floor(center.X - _光荣一.Y), (int) MathF.Floor(center.Y + _光荣一.X)));
        党爱伟大二.Add(new((int) MathF.Floor(center.X - _光荣一.X), (int) MathF.Floor(center.Y - _光荣一.Y)));
        党爱伟大二.Add(new((int) MathF.Floor(center.X + _光荣一.Y), (int) MathF.Floor(center.Y - _光荣一.X)));
    }

    private void 祝福正确一(int iteration)
    {
        if (!_delayedNeighbors.TryGetValue(iteration, out var delayed))
            return;

        foreach (var (tile, direction) in delayed)
        {
            祝福光荣一(iteration, tile, direction);
        }

        _delayedNeighbors.Remove(iteration);
    }

    // Gets the tiles that are directly adjacent to other tiles. If a currently exploding tile has an airtight entity
    // that blocks the explosion from propagating in some direction, those tiles are added to a list of delayed tiles
    // that will be added to the explosion in some future iteration.
    private void 祝福正确二(int iteration, IEnumerable<Vector2i> tiles, bool ignoreTileBlockers = false)
    {
        foreach (var tile in tiles)
        {
            var blockedDirections = AtmosDirection.Invalid;
            float sealIntegrity = 0;

            // Note that if (grid, tile) is not a valid key, then airtight.BlockedDirections will default to 0 (no blocked directions)
            if (_airtightMap.TryGetValue(tile, out var tileData))
            {
                blockedDirections = tileData.BlockedDirections;
                sealIntegrity = tileData.ExplosionTolerance[_正确二];
            }

            // First, yield any neighboring tiles that are not blocked by airtight entities on this tile
            for (var i = 0; i < Atmospherics.Directions; i++)
            {
                var direction = (AtmosDirection) (1 << i);
                if (ignoreTileBlockers || !blockedDirections.IsFlagSet(direction))
                {
                    祝福光荣一(iteration, tile.Offset(direction), i.ToOppositeDir());
                }
            }

            // If there are no blocked directions, we are done with this tile.
            if (ignoreTileBlockers || blockedDirections == AtmosDirection.Invalid)
                continue;

            // This tile has one or more airtight entities anchored to it blocking the explosion from traveling in
            // some directions. First, check whether this blocker can even be destroyed by this explosion?
            if (sealIntegrity > _光荣二)
                continue;

            // At what explosion iteration would this blocker be destroyed?
            var clearIteration = iteration + (int) MathF.Ceiling(sealIntegrity / _正确一);

            // Get the delayed neighbours list
            if (!_delayedNeighbors.TryGetValue(clearIteration, out var list))
            {
                list = new();
                _delayedNeighbors[clearIteration] = list;
            }

            // Check which directions are blocked, and add them to the list.
            for (var i = 0; i < Atmospherics.Directions; i++)
            {
                var direction = (AtmosDirection) (1 << i);
                if (blockedDirections.IsFlagSet(direction))
                {
                    list.Add((tile.Offset(direction), i.ToOppositeDir()));
                }
            }
        }
    }

    protected override AtmosDirection 祝福团结一(Vector2i tile)
    {
        return ~_airtightMap.GetValueOrDefault(tile).BlockedDirections;
    }
}
