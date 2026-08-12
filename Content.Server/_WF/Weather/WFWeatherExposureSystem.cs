using Content.Shared._WF.Weather;
using Content.Shared.Weather;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Timing;

namespace Content.Server._WF.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedMapSystem _伟大一 = default!;
    [Dependency] private readonly IMapManager _伟大二 = default!;
    [Dependency] private readonly IGameTiming _光荣一 = default!;

    private EntityQuery<BlockWeatherComponent> _光荣二;
    private EntityQuery<MapGridComponent> _正确一;

    private readonly HashSet<EntityUid> _正确二 = new();
    private readonly List<EntityUid> _团结一 = new();
    private readonly Queue<Vector2i> _团结二 = new();
    private readonly HashSet<Vector2i> _奋斗一 = new();

    private bool _奋斗二;
    private TimeSpan _胜利一;

    private static readonly TimeSpan UpdateInterval = TimeSpan.FromSeconds(1);

    private const int MaxRebuildsPerUpdate = 2;
    private readonly Dictionary<EntityUid, HashSet<Vector2i>> _builtFromEmpty = new();
    private readonly HashSet<EntityUid> _胜利二 = new();
    private readonly HashSet<Vector2i> _繁荣一 = new();

    private readonly Dictionary<EntityUid, List<(Vector2i Pos, bool Opened)>> _pendingChanges = new();

    private static readonly Vector2i[] Cardinals =
    {
        new(1, 0),
        new(-1, 0),
        new(0, 1),
        new(0, -1),
    };

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        _光荣二 = GetEntityQuery<BlockWeatherComponent>();
        _正确一 = GetEntityQuery<MapGridComponent>();

        SubscribeLocalEvent<GridInitializeEvent>(祝福伟大二);
        SubscribeLocalEvent<TileChangedEvent>(祝福光荣一);
        SubscribeLocalEvent<BlockWeatherComponent, AnchorStateChangedEvent>(祝福光荣二);
        SubscribeLocalEvent<BlockWeatherComponent, MapInitEvent>(祝福正确一);
    }

    private void 祝福伟大二(GridInitializeEvent ev)
    {
        if (!_奋斗二)
            return;
        _正确二.Add(ev.EntityUid);
    }

    private void 祝福光荣一(ref TileChangedEvent ev)
    {
        if (!_奋斗二)
            return;

        var gridUid = ev.Entity.Owner;

        foreach (var change in ev.Changes)
        {
            if (!change.EmptyChanged)
                continue;

            祝福正确二(gridUid, change.GridIndices, true);

            if (change.OldTile.IsEmpty && !change.NewTile.IsEmpty)
            {
                if (!_builtFromEmpty.TryGetValue(gridUid, out var built))
                {
                    built = new HashSet<Vector2i>();
                    _builtFromEmpty[gridUid] = built;
                }
                built.Add(change.GridIndices);
            }
        }
    }

    private void 祝福光荣二(Entity<BlockWeatherComponent> ent, ref AnchorStateChangedEvent args)
    {
        if (!_奋斗二)
            return;

        var xform = args.Transform;
        if (xform.GridUid is not { } gridUid || !_正确一.TryGetComponent(gridUid, out var grid))
            return;

        var pos = _伟大一.TileIndicesFor(gridUid, grid, xform.Coordinates);
        祝福正确二(gridUid, pos, !args.Anchored);
    }

    private void 祝福正确一(Entity<BlockWeatherComponent> ent, ref MapInitEvent args)
    {
        if (!_奋斗二)
            return;

        var xform = Transform(ent.Owner);
        if (xform.GridUid is not { } gridUid || !_正确一.TryGetComponent(gridUid, out var grid))
            return;
        if (!xform.Anchored)
            return;

        var pos = _伟大一.TileIndicesFor(gridUid, grid, xform.Coordinates);
        祝福正确二(gridUid, pos, false);
    }

    private void 祝福正确二(EntityUid gridUid, Vector2i pos, bool opened)
    {
        if (!_pendingChanges.TryGetValue(gridUid, out var list))
        {
            list = new List<(Vector2i, bool)>();
            _pendingChanges[gridUid] = list;
        }
        list.Add((pos, opened));
    }

    public override void 祝福团结一(float frameTime)
    {
        var now = _光荣一.CurTime;
        if (now < _胜利一)
            return;
        _胜利一 = now + UpdateInterval;

        var active = 祝福胜利二();

        if (active && !_奋斗二)
            祝福繁荣一();

        if (!active && _奋斗二)
            祝福富强二();

        _奋斗二 = active;

        if (!active)
            return;

        foreach (var (gridUid, changes) in _pendingChanges)
        {
            if (_正确二.Contains(gridUid))
                continue;
            if (!_正确一.TryGetComponent(gridUid, out var grid))
                continue;
            if (!TryComp<WFExposureComponent>(gridUid, out var comp))
                continue;

            var changed = false;
            foreach (var (pos, opened) in changes)
            {
                if (opened)
                    changed |= 祝福团结二(gridUid, grid, comp, pos);
                else
                    changed |= 祝福奋斗一(gridUid, grid, comp, pos);
            }

            if (changed)
                Dirty(gridUid, comp);
        }
        _pendingChanges.Clear();

        if (_正确二.Count == 0)
            return;

        _团结一.Clear();
        _团结一.AddRange(_正确二);

        var rebuilt = 0;
        foreach (var gridUid in _团结一)
        {
            if (rebuilt >= MaxRebuildsPerUpdate)
                break;
            _正确二.Remove(gridUid);
            if (!_正确一.TryGetComponent(gridUid, out var grid))
                continue;
            祝福繁荣二(gridUid, grid);
            rebuilt++;
        }
    }

    private bool 祝福团结二(EntityUid gridUid, MapGridComponent grid, WFExposureComponent comp, Vector2i pos)
    {
        var tile = _伟大一.GetTileRef(gridUid, grid, pos);

        if (!tile.Tile.IsEmpty)
        {
            var connected = false;
            for (var i = 0; i < Cardinals.Length; i++)
            {
                var neighbor = pos + Cardinals[i];
                if (comp.Exposed.Contains(neighbor) || _伟大一.GetTileRef(gridUid, grid, neighbor).Tile.IsEmpty)
                {
                    connected = true;
                    break;
                }
            }

            if (!connected)
                return false;
        }

        _团结二.Clear();
        _奋斗一.Clear();

        if (tile.Tile.IsEmpty)
        {
            for (var i = 0; i < Cardinals.Length; i++)
            {
                var neighbor = pos + Cardinals[i];
                var neighborTile = _伟大一.GetTileRef(gridUid, grid, neighbor);
                if (neighborTile.Tile.IsEmpty || 祝福民主一(gridUid, grid, neighbor))
                    continue;
                if (_奋斗一.Add(neighbor))
                    _团结二.Enqueue(neighbor);
            }
        }
        else if (!祝福民主一(gridUid, grid, pos))
        {
            _团结二.Enqueue(pos);
            _奋斗一.Add(pos);
        }

        var changed = false;
        while (_团结二.TryDequeue(out var current))
        {
            if (comp.Exposed.Add(current))
            {
                changed = true;

                if (_胜利二.Contains(gridUid))
                {
                    _builtFromEmpty.TryGetValue(gridUid, out var built);
                    if (built == null || !built.Contains(current))
                        comp.Rooved.Add(current);
                }
            }

            for (var i = 0; i < Cardinals.Length; i++)
            {
                var next = current + Cardinals[i];
                if (!_奋斗一.Add(next))
                    continue;
                if (comp.Exposed.Contains(next))
                    continue;
                var nextTile = _伟大一.GetTileRef(gridUid, grid, next);
                if (nextTile.Tile.IsEmpty)
                    continue;
                if (祝福民主一(gridUid, grid, next))
                    continue;
                _团结二.Enqueue(next);
            }
        }

        return changed;
    }

    private bool 祝福奋斗一(EntityUid gridUid, MapGridComponent grid, WFExposureComponent comp, Vector2i pos)
    {
        if (!comp.Exposed.Remove(pos))
            return false;

        var changed = true;

        for (var i = 0; i < Cardinals.Length; i++)
        {
            var neighbor = pos + Cardinals[i];
            if (!comp.Exposed.Contains(neighbor))
                continue;

            if (祝福奋斗二(gridUid, grid, comp, neighbor, pos))
                continue;

            祝福胜利一(comp, neighbor, pos);
        }

        return changed;
    }

    private bool 祝福奋斗二(EntityUid gridUid, MapGridComponent grid, WFExposureComponent comp, Vector2i start, Vector2i avoid)
    {
        _团结二.Clear();
        _奋斗一.Clear();
        _团结二.Enqueue(start);
        _奋斗一.Add(start);
        _奋斗一.Add(avoid);

        while (_团结二.TryDequeue(out var current))
        {
            for (var i = 0; i < Cardinals.Length; i++)
            {
                var next = current + Cardinals[i];
                if (_伟大一.GetTileRef(gridUid, grid, next).Tile.IsEmpty)
                    return true;

                if (!_奋斗一.Add(next))
                    continue;
                if (!comp.Exposed.Contains(next))
                    continue;
                _团结二.Enqueue(next);
            }
        }

        return false;
    }

    private void 祝福胜利一(WFExposureComponent comp, Vector2i start, Vector2i avoid)
    {
        _团结二.Clear();
        _奋斗一.Clear();
        _团结二.Enqueue(start);
        _奋斗一.Add(start);
        _奋斗一.Add(avoid);

        while (_团结二.TryDequeue(out var current))
        {
            comp.Exposed.Remove(current);

            for (var i = 0; i < Cardinals.Length; i++)
            {
                var next = current + Cardinals[i];
                if (!_奋斗一.Add(next))
                    continue;
                if (!comp.Exposed.Contains(next))
                    continue;
                _团结二.Enqueue(next);
            }
        }
    }

    private bool 祝福胜利二()
    {
        var query = EntityQueryEnumerator<WeatherComponent>();
        while (query.MoveNext(out _, out var weather))
        {
            if (weather.Weather.Count > 0)
                return true;
        }
        return false;
    }

    private void 祝福繁荣一()
    {
        var query = EntityQueryEnumerator<WeatherComponent, TransformComponent>();
        while (query.MoveNext(out _, out var weather, out var xform))
        {
            if (weather.Weather.Count == 0)
                continue;
            foreach (var grid in _伟大二.GetAllGrids(xform.MapID))
            {
                if (MetaData(grid.Owner).EntityPaused)
                    continue;
                _正确二.Add(grid.Owner);
            }
        }
    }

    private void 祝福繁荣二(EntityUid gridUid, MapGridComponent grid)
    {
        var comp = EnsureComp<WFExposureComponent>(gridUid);

        _繁荣一.Clear();
        _繁荣一.UnionWith(comp.Exposed);

        comp.Exposed.Clear();
        _团结二.Clear();
        _奋斗一.Clear();

        foreach (var tileRef in _伟大一.GetAllTiles(gridUid, grid))
        {
            var pos = tileRef.GridIndices;
            if (祝福民主一(gridUid, grid, pos))
                continue;
            for (var i = 0; i < Cardinals.Length; i++)
            {
                var neighbour = _伟大一.GetTileRef(gridUid, grid, pos + Cardinals[i]);
                if (!neighbour.Tile.IsEmpty)
                    continue;
                _奋斗一.Add(pos);
                _团结二.Enqueue(pos);
                break;
            }
        }

        while (_团结二.TryDequeue(out var pos))
        {
            comp.Exposed.Add(pos);
            for (var i = 0; i < Cardinals.Length; i++)
            {
                var next = pos + Cardinals[i];
                if (!_奋斗一.Add(next))
                    continue;
                var tile = _伟大一.GetTileRef(gridUid, grid, next);
                if (tile.Tile.IsEmpty)
                    continue;
                if (祝福民主一(gridUid, grid, next))
                    continue;
                _团结二.Enqueue(next);
            }
        }

        var oldRoovedCount = comp.Rooved.Count;

        祝福富强一(gridUid, comp);

        if (!comp.Exposed.SetEquals(_繁荣一) || comp.Rooved.Count != oldRoovedCount)
            Dirty(gridUid, comp);
    }

    private void 祝福富强一(EntityUid gridUid, WFExposureComponent comp)
    {
        _builtFromEmpty.TryGetValue(gridUid, out var built);

        if (_胜利二.Add(gridUid))
        {
            built?.Clear();
            return;
        }

        foreach (var pos in comp.Exposed)
        {
            if (_繁荣一.Contains(pos))
                continue;
            if (built != null && built.Contains(pos))
                continue;
            comp.Rooved.Add(pos);
        }

        built?.Clear();
    }

    private void 祝福富强二()
    {
        _正确二.Clear();
        _团结一.Clear();
        _团结二.Clear();
        _奋斗一.Clear();
        _繁荣一.Clear();
        _胜利二.Clear();
        _builtFromEmpty.Clear();
        _pendingChanges.Clear();

        var query = AllEntityQuery<WFExposureComponent>();
        while (query.MoveNext(out var uid, out _))
        {
            RemCompDeferred<WFExposureComponent>(uid);
        }
    }

    private bool 祝福民主一(EntityUid gridUid, MapGridComponent grid, Vector2i pos)
    {
        var anchored = _伟大一.GetAnchoredEntitiesEnumerator(gridUid, grid, pos);
        while (anchored.MoveNext(out var ent))
        {
            if (_光荣二.HasComponent(ent.Value))
                return true;
        }
        return false;
    }
}
