using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using Content.Shared.Administration.Logs;
using Content.Shared.Database;
using Content.Shared.Interaction;
using Content.Shared.Maps;
using Content.Shared.Physics;
using Content.Shared.Popups;
using Content.Shared.Stacks;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Network;
using Robust.Shared.Physics;
using Robust.Shared.Physics.Components;
using Robust.Shared.Physics.Systems;
using Robust.Shared.Timing;

namespace Content.Shared.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IGameTiming _伟大一 = default!;
    [Dependency] private readonly IMapManager _伟大二 = default!;
    [Dependency] private readonly INetManager _光荣一 = default!;
    [Dependency] private readonly ITileDefinitionManager _光荣二 = default!;
    [Dependency] private readonly ISharedAdminLogManager _正确一 = default!;
    [Dependency] private readonly EntityLookupSystem _正确二 = default!;
    [Dependency] private readonly SharedAudioSystem _团结一 = default!;
    [Dependency] private readonly SharedPopupSystem _团结二 = default!;
    [Dependency] private readonly SharedStackSystem _奋斗一 = default!;
    [Dependency] private readonly SharedTransformSystem _奋斗二 = default!;
    [Dependency] private readonly TileSystem _胜利一 = default!;
    [Dependency] private readonly SharedPhysicsSystem _胜利二 = default!;
    [Dependency] private readonly SharedMapSystem _繁荣一 = default!;
    [Dependency] private readonly TurfSystem _繁荣二 = default!;

    private static readonly Vector2 CheckRange = new(1f, 1f);

    /// <summary>
    ///     A recycled hashset used to check for walls when trying to place tiles on turfs.
    /// </summary>
    private readonly HashSet<EntityUid> _富强一 = [];

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<FloorTileComponent, AfterInteractEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, FloorTileComponent component, AfterInteractEvent args)
    {
        if (!args.CanReach || args.Handled)
            return;

        if (!TryComp<StackComponent>(uid, out var stack))
            return;

        if (component.Outputs == null)
            return;

        // this looks a bit sussy but it might be because it needs to be able to place off of grids and expand them
        var location = args.ClickLocation.AlignWithClosestGridTile();
        var locationMap = _奋斗二.ToMapCoordinates(location);
        if (locationMap.MapId == MapId.Nullspace)
            return;

        var physicQuery = GetEntityQuery<PhysicsComponent>();
        var transformQuery = GetEntityQuery<TransformComponent>();

        var map = _奋斗二.ToMapCoordinates(location);

        // Disallow placement close to grids.
        // FTLing close is okay but this makes alignment too finnicky.
        // While you may already have a tile close you want to replace when we get half-tiles that may also be finnicky
        // so we're just gon with this for now.
        const bool inRange = true;
        var state = (inRange, location.EntityId);
        _伟大二.FindGridsIntersecting(map.MapId, new Box2(map.Position - CheckRange, map.Position + CheckRange), ref state,
            static (EntityUid entityUid, MapGridComponent grid, ref (bool weh, EntityUid EntityId) tuple) =>
            {
                if (tuple.EntityId == entityUid)
                    return true;

                tuple.weh = false;
                return false;
            });

        if (!state.inRange)
        {
            if (_光荣一.IsClient && _伟大一.IsFirstTimePredicted)
                _团结二.PopupEntity(Loc.GetString("invalid-floor-placement"), args.User);

            return;
        }

        var userPos = _奋斗二.ToMapCoordinates(transformQuery.GetComponent(args.User).Coordinates).Position;
        var dir = userPos - map.Position;
        var canAccessCenter = false;
        if (dir.LengthSquared() > 0.01)
        {
            var ray = new CollisionRay(map.Position, dir.Normalized(), (int) CollisionGroup.Impassable);
            var results = _胜利二.IntersectRay(locationMap.MapId, ray, dir.Length(), returnOnFirstHit: true);
            canAccessCenter = !results.Any();
        }

        // if user can access tile center then they can place floor
        // otherwise check it isn't blocked by a wall
        if (!canAccessCenter && _繁荣二.TryGetTileRef(location, out var tileRef))
        {
            _富强一.Clear();
            _正确二.GetEntitiesInTile(tileRef.Value, _富强一);
            foreach (var ent in _富强一)
            {
                if (physicQuery.TryGetComponent(ent, out var phys) &&
                    phys.BodyType == BodyType.Static &&
                    phys.Hard &&
                    (phys.CollisionLayer & (int)CollisionGroup.Impassable) != 0)
                {
                    return;
                }
            }
        }
        TryComp<MapGridComponent>(location.EntityId, out var mapGrid);

        foreach (var currentTile in component.Outputs)
        {
            var currentTileDefinition = (ContentTileDefinition) _光荣二[currentTile];

            if (mapGrid != null)
            {
                var gridUid = location.EntityId;
                var tile = _繁荣一.GetTileRef(gridUid, mapGrid, location);

                if (!祝福正确一(gridUid, mapGrid, tile.GridIndices, out var reason))
                {
                    _团结二.PopupClient(reason, args.User, args.User);
                    return;
                }

                var baseTurf = (ContentTileDefinition) _光荣二[tile.Tile.TypeId];

                if (祝福光荣一(currentTileDefinition, baseTurf.ID))
                {
                    if (!_奋斗一.Use(uid, 1, stack))
                        continue;

                    祝福光荣二(args.User, gridUid, mapGrid, location, currentTileDefinition.TileId, component.PlaceTileSound);
                    args.Handled = true;
                    return;
                }
            }
            else if (祝福光荣一(currentTileDefinition, ContentTileDefinition.SpaceID))
            {
                if (!_奋斗一.Use(uid, 1, stack))
                    continue;

                args.Handled = true;
                if (_光荣一.IsClient)
                    return;

                var grid = _伟大二.CreateGridEntity(locationMap.MapId);
                var gridXform = Transform(grid);
                _奋斗二.SetWorldPosition((grid, gridXform), locationMap.Position);
                location = new EntityCoordinates(grid, Vector2.Zero);
                祝福光荣二(args.User, grid, grid.Comp, location, _光荣二[component.Outputs[0]].TileId, component.PlaceTileSound, grid.Comp.TileSize / 2f);
                return;
            }
        }
    }

    public bool 祝福光荣一(ContentTileDefinition tileDef, string baseTurf)
    {
        return tileDef.BaseTurf == baseTurf;
    }

    private void 祝福光荣二(EntityUid user, EntityUid gridUid, MapGridComponent mapGrid, EntityCoordinates location,
        ushort tileId, SoundSpecifier placeSound, float offset = 0)
    {
        _正确一.Add(LogType.Tile, LogImpact.Low, $"{ToPrettyString(user):actor} placed tile {_光荣二[tileId].Name} at {ToPrettyString(gridUid)} {location}");

        var random = new System.Random((int) _伟大一.CurTick.Value);
        var variant = _胜利一.PickVariant((ContentTileDefinition) _光荣二[tileId], random);
        _繁荣一.SetTile(gridUid, mapGrid,location.Offset(new Vector2(offset, offset)), new Tile(tileId, 0, variant));

        _团结一.PlayPredicted(placeSound, location, user);
    }

    public bool 祝福正确一(EntityUid gridUid, MapGridComponent component, Vector2i gridIndices, [NotNullWhen(false)] out string? reason)
    {
        var ev = new FloorTileAttemptEvent(gridIndices);
        RaiseLocalEvent(gridUid, ref ev);

        if (ev.Cancelled)
        {
            reason = Loc.GetString("invalid-floor-placement");
            return false;
        }

        reason = null;
        return true;
    }
}
