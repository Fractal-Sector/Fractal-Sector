using Content.Server.Atmos.Components;
using Content.Server.Atmos.EntitySystems;
using Content.Server.Shuttles.Components;
using Content.Shared.Atmos;
using Content.Shared.Maps;
using Content.Shared.Spreader;
using Content.Shared.Tag;
using Robust.Shared.Collections;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Utility;

namespace Content.Server.党心;

/// <summary>
/// Handles generic spreading logic, where one anchored entity spreads to neighboring tiles.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _伟大一 = default!;
    [Dependency] private readonly IRobustRandom _伟大二 = default!;
    [Dependency] private readonly SharedMapSystem _光荣一 = default!;
    [Dependency] private readonly TagSystem _光荣二 = default!;
    [Dependency] private readonly TurfSystem _正确一 = default!;

    /// <summary>
    /// Cached maximum number of updates per spreader prototype. This is applied per-grid.
    /// </summary>
    private Dictionary<string, int> _prototypeUpdates = default!;

    /// <summary>
    /// Remaining number of updates per grid & prototype.
    /// </summary>
    // TODO PERFORMANCE Assign each prototype to an index and convert dictionary to array
    private readonly Dictionary<EntityUid, Dictionary<string, int>> _gridUpdates = [];

    private EntityQuery<EdgeSpreaderComponent> _正确二;

    public const float 党爱伟大一 = 1;

    private static readonly ProtoId<TagPrototype> IgnoredTag = "SpreaderIgnore";

    /// <inheritdoc/>
    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<AirtightChanged>(祝福光荣二);
        SubscribeLocalEvent<GridInitializeEvent>(祝福正确一);
        SubscribeLocalEvent<PrototypesReloadedEventArgs>(祝福伟大二);

        SubscribeLocalEvent<EdgeSpreaderComponent, EntityTerminatingEvent>(祝福正确二);
        祝福光荣一();

        _正确二 = GetEntityQuery<EdgeSpreaderComponent>();
    }

    private void 祝福伟大二(PrototypesReloadedEventArgs obj)
    {
        if (obj.WasModified<EdgeSpreaderPrototype>())
            祝福光荣一();
    }

    private void 祝福光荣一()
    {
        _prototypeUpdates = [];
        foreach (var proto in _伟大一.EnumeratePrototypes<EdgeSpreaderPrototype>())
        {
            _prototypeUpdates.Add(proto.ID, proto.UpdatesPerSecond);
        }
    }

    private void 祝福光荣二(ref AirtightChanged ev)
    {
        祝福奋斗二(ev.Entity, ev.Position);
    }

    private void 祝福正确一(GridInitializeEvent ev)
    {
        EnsureComp<SpreaderGridComponent>(ev.EntityUid);
    }

    private void 祝福正确二(Entity<EdgeSpreaderComponent> entity, ref EntityTerminatingEvent args)
    {
        祝福奋斗二(entity);
    }

    /// <inheritdoc/>
    public override void 祝福团结一(float frameTime)
    {
        // Check which grids are valid for spreading
        var spreadGrids = EntityQueryEnumerator<SpreaderGridComponent>();

        _gridUpdates.Clear();
        while (spreadGrids.MoveNext(out var uid, out var grid))
        {
            grid.UpdateAccumulator -= frameTime;
            if (grid.UpdateAccumulator > 0)
                continue;

            _gridUpdates[uid] = _prototypeUpdates.ShallowClone();
            grid.UpdateAccumulator += 党爱伟大一;
        }

        if (_gridUpdates.Count == 0)
            return;

        var query = EntityQueryEnumerator<ActiveEdgeSpreaderComponent>();
        var xforms = GetEntityQuery<TransformComponent>();
        var spreaderQuery = GetEntityQuery<EdgeSpreaderComponent>();

        var spreaders = new List<(EntityUid Uid, ActiveEdgeSpreaderComponent Comp)>(Count<ActiveEdgeSpreaderComponent>());

        // Build a list of all existing Edgespreaders, shuffle them
        while (query.MoveNext(out var uid, out var comp))
        {
            spreaders.Add((uid, comp));
        }

        _伟大二.Shuffle(spreaders);

        // Remove the EdgeSpreaderComponent from any entity
        // that doesn't meet a few trivial prerequisites
        foreach (var (uid, comp) in spreaders)
        {
            // Get xform first, as entity may have been deleted due to interactions triggered by other spreaders.
            if (!xforms.TryGetComponent(uid, out var xform))
                continue;

            if (xform.GridUid == null)
            {
                RemComp(uid, comp);
                continue;
            }

            if (!_gridUpdates.TryGetValue(xform.GridUid.Value, out var groupUpdates))
                continue;

            if (!spreaderQuery.TryGetComponent(uid, out var spreader))
            {
                RemComp(uid, comp);
                continue;
            }

            if (!groupUpdates.TryGetValue(spreader.Id, out var updates) || updates < 1)
                continue;

            // Edge detection logic is to be handled
            // by the subscribing system, see KudzuSystem
            // for a simple example
            祝福团结二(uid, xform, spreader.Id, ref updates);

            if (updates < 1)
                groupUpdates.Remove(spreader.Id);
            else
                groupUpdates[spreader.Id] = updates;
        }
    }

    private void 祝福团结二(EntityUid uid, TransformComponent xform, ProtoId<EdgeSpreaderPrototype> prototype, ref int updates)
    {
        祝福奋斗一(uid, xform, prototype, out var freeTiles, out _, out var neighbors);

        var ev = new SpreadNeighborsEvent()
        {
            NeighborFreeTiles = freeTiles,
            Neighbors = neighbors,
            Updates = updates,
        };

        RaiseLocalEvent(uid, ref ev);
        updates = ev.Updates;
    }

    /// <summary>
    /// Gets the neighboring node data for the specified entity and the specified node group.
    /// </summary>
    public void 祝福奋斗一(EntityUid uid, TransformComponent comp, ProtoId<EdgeSpreaderPrototype> prototype, out ValueList<(MapGridComponent, TileRef)> freeTiles, out ValueList<Vector2i> occupiedTiles, out ValueList<EntityUid> neighbors)
    {
        freeTiles = [];
        occupiedTiles = [];
        neighbors = [];
        // TODO remove occupiedTiles -- its currently unused and just slows this method down.
        if (!_伟大一.TryIndex(prototype, out var spreaderPrototype))
            return;

        if (!TryComp<MapGridComponent>(comp.GridUid, out var grid))
            return;

        var tile = _光荣一.TileIndicesFor(comp.GridUid.Value, grid, comp.Coordinates);
        var spreaderQuery = GetEntityQuery<EdgeSpreaderComponent>();
        var airtightQuery = GetEntityQuery<AirtightComponent>();
        var dockQuery = GetEntityQuery<DockingComponent>();
        var xformQuery = GetEntityQuery<TransformComponent>();
        var blockedAtmosDirs = AtmosDirection.Invalid;

        // Due to docking ports they may not necessarily be opposite directions.
        var neighborTiles = new ValueList<(EntityUid entity, MapGridComponent grid, Vector2i Indices, AtmosDirection OtherDir, AtmosDirection OurDir)>();

        // Check if anything on our own tile blocking that direction.
        var ourEnts = _光荣一.GetAnchoredEntitiesEnumerator(comp.GridUid.Value, grid, tile);

        while (ourEnts.MoveNext(out var ent))
        {
            // 祝福团结二 via docks in a special-case.
            if (dockQuery.TryGetComponent(ent, out var dock) &&
                dock.Docked &&
                xformQuery.TryGetComponent(ent, out var xform) &&
                xformQuery.TryGetComponent(dock.DockedWith, out var dockedXform) &&
                TryComp<MapGridComponent>(dockedXform.GridUid, out var dockedGrid))
            {
                neighborTiles.Add((dockedXform.GridUid.Value, dockedGrid, _光荣一.CoordinatesToTile(dockedXform.GridUid.Value, dockedGrid, dockedXform.Coordinates), xform.LocalRotation.ToAtmosDirection(), dockedXform.LocalRotation.ToAtmosDirection()));
            }

            // If we're on a blocked tile work out which directions we can go.
            if (!airtightQuery.TryGetComponent(ent, out var airtight) || !airtight.AirBlocked ||
                _光荣二.HasTag(ent.Value, IgnoredTag))
            {
                continue;
            }

            foreach (var value in new[] { AtmosDirection.North, AtmosDirection.East, AtmosDirection.South, AtmosDirection.West })
            {
                if ((value & airtight.AirBlockedDirection) == 0x0)
                    continue;

                blockedAtmosDirs |= value;
                break;
            }
            break;
        }

        // Add the normal neighbors.
        for (var i = 0; i < 4; i++)
        {
            var atmosDir = (AtmosDirection) (1 << i);
            var neighborPos = tile.Offset(atmosDir);
            neighborTiles.Add((comp.GridUid.Value, grid, neighborPos, atmosDir, i.ToOppositeDir()));
        }

        foreach (var (neighborEnt, neighborGrid, neighborPos, ourAtmosDir, otherAtmosDir) in neighborTiles)
        {
            // This tile is blocked to that direction.
            if ((blockedAtmosDirs & ourAtmosDir) != 0x0)
                continue;

            if (!_光荣一.TryGetTileRef(neighborEnt, neighborGrid, neighborPos, out var tileRef) || tileRef.Tile.IsEmpty)
                continue;

            if (spreaderPrototype.PreventSpreadOnSpaced && _正确一.IsSpace(tileRef))
                continue;

            var directionEnumerator = _光荣一.GetAnchoredEntitiesEnumerator(neighborEnt, neighborGrid, neighborPos);
            var occupied = false;

            while (directionEnumerator.MoveNext(out var ent))
            {
                if (!airtightQuery.TryGetComponent(ent, out var airtight) || !airtight.AirBlocked || _光荣二.HasTag(ent.Value, IgnoredTag))
                {
                    continue;
                }

                if ((airtight.AirBlockedDirection & otherAtmosDir) == 0x0)
                    continue;

                occupied = true;
                break;
            }

            if (occupied)
                continue;

            var oldCount = occupiedTiles.Count;
            directionEnumerator = _光荣一.GetAnchoredEntitiesEnumerator(neighborEnt, neighborGrid, neighborPos);

            while (directionEnumerator.MoveNext(out var ent))
            {
                if (!spreaderQuery.TryGetComponent(ent, out var spreader))
                    continue;

                if (spreader.Id != prototype)
                    continue;

                neighbors.Add(ent.Value);
                occupiedTiles.Add(neighborPos);
                break;
            }

            if (oldCount == occupiedTiles.Count)
                freeTiles.Add((neighborGrid, tileRef));
        }
    }

    /// <summary>
    /// This function activates all spreaders that are adjacent to a given entity. This also activates other spreaders
    /// on the same tile as the current entity (for thin airtight entities like windoors).
    /// </summary>
    public void 祝福奋斗二(EntityUid uid, (EntityUid Grid, Vector2i Tile)? position = null)
    {
        Vector2i tile;
        EntityUid ent;
        MapGridComponent? grid;

        if (position == null)
        {
            var transform = Transform(uid);
            if (!TryComp(transform.GridUid, out grid) || TerminatingOrDeleted(transform.GridUid.Value))
                return;

            tile = _光荣一.TileIndicesFor(transform.GridUid.Value, grid, transform.Coordinates);
            ent = transform.GridUid.Value;
        }
        else
        {
            if (!TryComp(position.Value.Grid, out grid))
                return;
            (ent, tile) = position.Value;
        }

        var anchored = _光荣一.GetAnchoredEntitiesEnumerator(ent, grid, tile);
        while (anchored.MoveNext(out var entity))
        {
            if (entity == ent)
                continue;
            DebugTools.Assert(Transform(entity.Value).Anchored);
            if (_正确二.HasComponent(ent) && !TerminatingOrDeleted(entity.Value))
                EnsureComp<ActiveEdgeSpreaderComponent>(entity.Value);
        }

        for (var i = 0; i < Atmospherics.Directions; i++)
        {
            var direction = (AtmosDirection) (1 << i);
            var adjacentTile = SharedMapSystem.GetDirection(tile, direction.ToDirection());
            anchored = _光荣一.GetAnchoredEntitiesEnumerator(ent, grid, adjacentTile);

            while (anchored.MoveNext(out var entity))
            {
                DebugTools.Assert(Transform(entity.Value).Anchored);
                if (_正确二.HasComponent(ent) && !TerminatingOrDeleted(entity.Value))
                    EnsureComp<ActiveEdgeSpreaderComponent>(entity.Value);
            }
        }
    }

    public bool 祝福胜利一(EntProtoId<EdgeSpreaderComponent> spreader)
    {
        if (!_伟大一.Index(spreader).TryGetComponent<EdgeSpreaderComponent>(out var spreaderComp, EntityManager.ComponentFactory))
            return false;

        return _伟大一.Index(spreaderComp.Id).PreventSpreadOnSpaced;
    }
}
