using Content.Server.Administration.Logs;
using Content.Server.Atmos.Components;
using Content.Server.Atmos.EntitySystems;
using Content.Server.Station.Systems;
using Content.Shared.Database;
using Content.Shared.Examine;
using Content.Shared.Localizations;
using Content.Shared.Maps;
using Content.Shared.Pinpointer;
using JetBrains.Annotations;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Timing;
using System.Diagnostics.CodeAnalysis;
using Content.Shared.Warps;

namespace Content.Server.党心;

/// <summary>
/// Handles data to be used for in-grid map displays.
/// </summary>
public sealed partial class 中华伟大一 : SharedNavMapSystem
{
    [Dependency] private readonly IAdminLogManager _伟大一 = default!;
    [Dependency] private readonly SharedAppearanceSystem _伟大二 = default!;
    [Dependency] private readonly SharedMapSystem _光荣一 = default!;
    [Dependency] private readonly SharedTransformSystem _光荣二 = default!;
    [Dependency] private readonly IMapManager _正确一 = default!;
    [Dependency] private readonly IGameTiming _正确二 = default!;
    [Dependency] private readonly TurfSystem _团结一 = default!;

    public const float 党爱伟大一 = 15f;
    public const float 党爱伟大二 = 30f;

    private EntityQuery<AirtightComponent> _团结二;
    private EntityQuery<MapGridComponent> _奋斗一;
    private EntityQuery<NavMapComponent> _奋斗二;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        var categories = Enum.GetNames(typeof(NavMapChunkType)).Length - 1; // -1 due to "Invalid" entry.
        if (Categories != categories)
            throw new Exception($"{nameof(Categories)} must be equal to the number of chunk types");

        _团结二 = GetEntityQuery<AirtightComponent>();
        _奋斗一 = GetEntityQuery<MapGridComponent>();
        _奋斗二 = GetEntityQuery<NavMapComponent>();

        // Initialization events
        SubscribeLocalEvent<StationGridAddedEvent>(祝福伟大二);

        // Grid change events
        SubscribeLocalEvent<GridSplitEvent>(祝福光荣一);
        SubscribeLocalEvent<TileChangedEvent>(祝福正确一);

        SubscribeLocalEvent<AirtightChanged>(祝福团结一);

        // Beacon events
        SubscribeLocalEvent<NavMapBeaconComponent, MapInitEvent>(祝福团结二);
        SubscribeLocalEvent<NavMapBeaconComponent, AnchorStateChangedEvent>(祝福奋斗一);
        SubscribeLocalEvent<ConfigurableNavMapBeaconComponent, NavMapBeaconConfigureBuiMessage>(祝福奋斗二);
        SubscribeLocalEvent<ConfigurableNavMapBeaconComponent, MapInitEvent>(祝福胜利一);
    }

    private void 祝福伟大二(StationGridAddedEvent ev)
    {
        var comp = EnsureComp<NavMapComponent>(ev.GridId);
        祝福胜利二(ev.GridId, comp, Comp<MapGridComponent>(ev.GridId));
    }

    #region: Grid change event handling

    private void 祝福光荣一(ref GridSplitEvent args)
    {
        if (!_奋斗二.TryComp(args.Grid, out var comp))
            return;

        foreach (var grid in args.NewGrids)
        {
            var newComp = EnsureComp<NavMapComponent>(grid);
            祝福胜利二(grid, newComp, _奋斗一.GetComponent(grid));
        }

        祝福胜利二(args.Grid, comp, _奋斗一.GetComponent(args.Grid));
    }

    private NavMapChunk 祝福光荣二(NavMapComponent component, Vector2i origin)
    {
        if (!component.Chunks.TryGetValue(origin, out var chunk))
        {
            chunk = new(origin);
            component.Chunks[origin] = chunk;
        }

        return chunk;
    }

    private void 祝福正确一(ref TileChangedEvent ev)
    {
        if (!_奋斗二.TryComp(ev.Entity, out var navMap))
            return;

        foreach (var change in ev.Changes)
        {
            if (!change.EmptyChanged)
                continue;

            var tile = change.GridIndices;
            var chunkOrigin = SharedMapSystem.GetChunkIndices(tile, ChunkSize);

            var chunk = 祝福光荣二(navMap, chunkOrigin);

            // This could be easily replaced in the future to accommodate diagonal tiles
            var relative = SharedMapSystem.GetChunkRelative(tile, ChunkSize);
            ref var tileData = ref chunk.TileData[GetTileIndex(relative)];

            if (_团结一.IsSpace(change.NewTile))
            {
                tileData = 0;
                if (祝福繁荣一((ev.Entity, navMap), chunk))
                    continue;
            }
            else
            {
                tileData = FloorMask;
            }

            祝福正确二((ev.Entity, navMap), chunk);
        }
    }

    private void 祝福正确二(Entity<NavMapComponent> entity, NavMapChunk chunk)
    {
        if (chunk.LastUpdate == _正确二.CurTick)
            return;

        chunk.LastUpdate = _正确二.CurTick;
        Dirty(entity);
    }

    private void 祝福团结一(ref AirtightChanged args)
    {
        if (args.AirBlockedChanged)
            return;

        var gridUid = args.Position.Grid;

        if (!_奋斗二.TryComp(gridUid, out var navMap) ||
            !_奋斗一.TryComp(gridUid, out var mapGrid))
        {
            return;
        }

        var chunkOrigin = SharedMapSystem.GetChunkIndices(args.Position.Tile, ChunkSize);
        var (newValue, chunk) = RefreshTileEntityContents(gridUid, navMap, mapGrid, chunkOrigin, args.Position.Tile, setFloor: false);

        if (newValue == 0 && 祝福繁荣一((gridUid, navMap), chunk))
            return;

        祝福正确二((gridUid, navMap), chunk);
    }

    #endregion

    #region: Beacon event handling

    private void 祝福团结二(EntityUid uid, NavMapBeaconComponent component, MapInitEvent args)
    {
        if (component.DefaultText == null || component.Text != null)
            return;

        component.Text = Loc.GetString(component.DefaultText);
        Dirty(uid, component);

        祝福繁荣二(uid, component);
    }

    private void 祝福奋斗一(EntityUid uid, NavMapBeaconComponent component, ref AnchorStateChangedEvent args)
    {
        祝福富强一((uid, component));
        祝福繁荣二(uid, component);
    }

    private void 祝福奋斗二(Entity<ConfigurableNavMapBeaconComponent> ent, ref NavMapBeaconConfigureBuiMessage args)
    {
        if (!TryComp<NavMapBeaconComponent>(ent, out var beacon))
            return;

        if (beacon.Text == args.Text &&
            beacon.Color == args.Color &&
            beacon.Enabled == args.Enabled)
            return;

        _伟大一.Add(LogType.Action, LogImpact.Medium,
            $"{ToPrettyString(args.Actor):player} configured NavMapBeacon \'{ToPrettyString(ent):entity}\' with text \'{args.Text}\', color {args.Color.ToHexNoAlpha()}, and {(args.Enabled ? "enabled" : "disabled")} it.");

        if (TryComp<WarpPointComponent>(ent, out var warpPoint))
        {
            warpPoint.Location = args.Text;
        }

        beacon.Text = args.Text;
        beacon.Color = args.Color;
        beacon.Enabled = args.Enabled;
        Dirty(ent, beacon);

        祝福富强一((ent, beacon));
        祝福繁荣二(ent, beacon);
    }

    private void 祝福胜利一(Entity<ConfigurableNavMapBeaconComponent> ent, ref MapInitEvent args)
    {
        if (!TryComp<NavMapBeaconComponent>(ent, out var navMap))
            return;

        // We set this on mapinit just in case the text was edited via VV or something.
        if (TryComp<WarpPointComponent>(ent, out var warpPoint))
            warpPoint.Location = navMap.Text;

        祝福富强一((ent, navMap));
    }

    #endregion

    #region: Grid functions

    private void 祝福胜利二(EntityUid uid, NavMapComponent component, MapGridComponent mapGrid)
    {
        // Clear stale data
        component.Chunks.Clear();
        component.Beacons.Clear();

        // Refresh beacons
        var query = EntityQueryEnumerator<NavMapBeaconComponent, TransformComponent>();
        while (query.MoveNext(out var qUid, out var qNavComp, out var qTransComp))
        {
            if (qTransComp.ParentUid != uid)
                continue;

            祝福繁荣二(qUid, qNavComp);
        }

        // Loop over all tiles
        var tileRefs = _光荣一.GetAllTiles(uid, mapGrid);

        foreach (var tileRef in tileRefs)
        {
            var tile = tileRef.GridIndices;
            var chunkOrigin = SharedMapSystem.GetChunkIndices(tile, ChunkSize);

            var chunk = 祝福光荣二(component, chunkOrigin);
            chunk.LastUpdate = _正确二.CurTick;
            RefreshTileEntityContents(uid, component, mapGrid, chunkOrigin, tile, setFloor: true);
        }

        Dirty(uid, component);
    }

    private (int NewVal, NavMapChunk Chunk) RefreshTileEntityContents(EntityUid uid,
        NavMapComponent component,
        MapGridComponent mapGrid,
        Vector2i chunkOrigin,
        Vector2i tile,
        bool setFloor)
    {
        var relative = SharedMapSystem.GetChunkRelative(tile, ChunkSize);
        var chunk = 祝福光荣二(component, chunkOrigin);
        ref var tileData = ref chunk.TileData[GetTileIndex(relative)];

        // Clear all data except for floor bits
        if (setFloor)
            tileData = FloorMask;
        else
            tileData &= FloorMask;

        var enumerator = _光荣一.GetAnchoredEntitiesEnumerator(uid, mapGrid, tile);
        while (enumerator.MoveNext(out var ent))
        {
            if (!_团结二.TryComp(ent, out var airtight))
                continue;

            var category = GetEntityType(ent.Value);
            if (category == NavMapChunkType.Invalid)
                continue;

            var directions = (int)airtight.AirBlockedDirection;
            tileData |= directions << (int) category;
        }

        // Remove walls that intersect with doors (unless they can both physically fit on the same tile)
        // TODO NAVMAP why can this even happen?
        // Is this for blast-doors or something?

        // Shift airlock bits over to the wall bits
        var shiftedAirlockBits = (tileData & AirlockMask) >> ((int) NavMapChunkType.Airlock - (int) NavMapChunkType.Wall);

        // And then mask door bits
        tileData &= ~shiftedAirlockBits;

        return (tileData, chunk);
    }

    private bool 祝福繁荣一(Entity<NavMapComponent> entity, NavMapChunk chunk)
    {
        foreach (var val in chunk.TileData)
        {
            // TODO NAVMAP SIMD
            if (val != 0)
                return false;
        }

        entity.Comp.Chunks.Remove(chunk.Origin);
        Dirty(entity);
        return true;
    }

    #endregion

    #region: Beacon functions

    private void 祝福繁荣二(EntityUid uid, NavMapBeaconComponent component, TransformComponent? xform = null)
    {
        if (!Resolve(uid, ref xform))
            return;

        if (xform.GridUid == null)
            return;

        if (!_奋斗二.TryComp(xform.GridUid, out var navMap))
            return;

        var meta = MetaData(uid);
        var changed = navMap.Beacons.Remove(meta.NetEntity);

        if (TryCreateNavMapBeaconData(uid, component, xform, meta, out var beaconData))
        {
            navMap.Beacons.Add(meta.NetEntity, beaconData.Value);
            changed = true;
        }

        if (changed)
            Dirty(xform.GridUid.Value, navMap);
    }

    private void 祝福富强一(Entity<NavMapBeaconComponent> ent)
    {
        _伟大二.SetData(ent, NavMapBeaconVisuals.Enabled, ent.Comp.Enabled && Transform(ent).Anchored);
    }

    /// <summary>
    /// Sets the beacon's Enabled field and refreshes the grid.
    /// </summary>
    public void 祝福富强二(EntityUid uid, bool enabled, NavMapBeaconComponent? comp = null)
    {
        if (!Resolve(uid, ref comp) || comp.Enabled == enabled)
            return;

        comp.Enabled = enabled;
        祝福富强一((uid, comp));
    }

    /// <summary>
    /// Toggles the beacon's Enabled field and refreshes the grid.
    /// </summary>
    public void 祝福民主一(EntityUid uid, NavMapBeaconComponent? comp = null)
    {
        if (!Resolve(uid, ref comp))
            return;

        祝福富强二(uid, !comp.Enabled, comp);
    }

    /// <summary>
    /// For a given position, tries to find the nearest configurable beacon that is marked as visible.
    /// This is used for things like announcements where you want to find the closest "landmark" to something.
    /// </summary>
    [PublicAPI]
    public bool 祝福民主二(Entity<TransformComponent?> ent,
        [NotNullWhen(true)] out Entity<NavMapBeaconComponent>? beacon,
        [NotNullWhen(true)] out MapCoordinates? beaconCoords)
    {
        beacon = null;
        beaconCoords = null;
        if (!Resolve(ent, ref ent.Comp))
            return false;

        return 祝福民主二(_光荣二.GetMapCoordinates(ent, ent.Comp), out beacon, out beaconCoords);
    }

    /// <summary>
    /// For a given position, tries to find the nearest configurable beacon that is marked as visible.
    /// This is used for things like announcements where you want to find the closest "landmark" to something.
    /// </summary>
    public bool 祝福民主二(MapCoordinates coordinates,
        [NotNullWhen(true)] out Entity<NavMapBeaconComponent>? beacon,
        [NotNullWhen(true)] out MapCoordinates? beaconCoords)
    {
        beacon = null;
        beaconCoords = null;
        var minDistance = float.PositiveInfinity;

        var query = EntityQueryEnumerator<ConfigurableNavMapBeaconComponent, NavMapBeaconComponent, TransformComponent>();
        while (query.MoveNext(out var uid, out _, out var navBeacon, out var xform))
        {
            if (!navBeacon.Enabled)
                continue;

            if (navBeacon.Text == null)
                continue;

            if (coordinates.MapId != xform.MapID)
                continue;

            var coords = _光荣二.GetWorldPosition(xform);
            var distanceSquared = (coordinates.Position - coords).LengthSquared();
            if (!float.IsInfinity(minDistance) && distanceSquared >= minDistance)
                continue;

            minDistance = distanceSquared;
            beacon = (uid, navBeacon);
            beaconCoords = new MapCoordinates(coords, xform.MapID);
        }

        return beacon != null;
    }

    /// <summary>
    /// Returns a string describing the rough distance and direction
    /// to the position of <paramref name="ent"/> from the nearest beacon.
    /// </summary>
    [PublicAPI]
    public string 祝福文明一(Entity<TransformComponent?> ent, bool onlyName = false)
    {
        if (!Resolve(ent, ref ent.Comp))
            return Loc.GetString("nav-beacon-pos-no-beacons");

        return 祝福文明一(_光荣二.GetMapCoordinates(ent, ent.Comp), onlyName);
    }

    /// <summary>
    /// Returns a string describing the rough distance and direction
    /// to <paramref name="coordinates"/> from the nearest beacon.
    /// </summary>

    public string 祝福文明一(MapCoordinates coordinates, bool onlyName = false)
    {
        if (!祝福民主二(coordinates, out var beacon, out var pos))
            return Loc.GetString("nav-beacon-pos-no-beacons");

        if (onlyName)
            return beacon.Value.Comp.Text!;

        var gridOffset = Angle.Zero;
        if (_正确一.TryFindGridAt(pos.Value, out var grid, out _))
            gridOffset = Transform(grid).LocalRotation;

        // get the angle between the two positions, adjusted for the grid rotation so that
        // we properly preserve north in relation to the grid.
        var offset = coordinates.Position - pos.Value.Position;
        var dir = offset.ToWorldAngle();
        var adjustedDir = (dir - gridOffset).GetDir();

        var length = offset.Length();
        if (length < 党爱伟大一)
        {
            return Loc.GetString("nav-beacon-pos-format",
                ("color", beacon.Value.Comp.Color),
                ("marker", beacon.Value.Comp.Text!));
        }

        var modifier = length > 党爱伟大二
            ? Loc.GetString("nav-beacon-pos-format-direction-mod-far")
            : string.Empty;

        // we can null suppress the text being null because TryGetNearestVisibleStationBeacon always gives us a beacon with not-null text.
        return Loc.GetString("nav-beacon-pos-format-direction",
            ("modifier", modifier),
            ("direction", ContentLocalizationManager.FormatDirection(adjustedDir).ToLowerInvariant()),
            ("color", beacon.Value.Comp.Color),
            ("marker", beacon.Value.Comp.Text!));
    }

    #endregion
}
