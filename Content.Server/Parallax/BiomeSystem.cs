using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Content.Server.Atmos;
using Content.Server.Atmos.Components;
using Content.Server.Atmos.EntitySystems;
using Content.Server.Decals;
using Content.Server.Ghost.Roles.Components;
using Content.Server.Shuttles.Events;
using Content.Server.Shuttles.Systems;
using Content.Shared.Atmos;
using Content.Shared.Decals;
using Content.Shared.Ghost;
using Content.Shared.Gravity;
using Content.Shared.Light.Components;
using Content.Shared.Parallax.Biomes;
using Content.Shared.Parallax.Biomes.Layers;
using Content.Shared.Parallax.Biomes.Markers;
using Content.Shared.Tag;
using Microsoft.Extensions.ObjectPool;
using Robust.Server.Player;
using Robust.Shared;
using Robust.Shared.Collections;
using Robust.Shared.Configuration;
using Robust.Shared.Console;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Physics;
using Robust.Shared.Physics.Systems;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Threading;
using Robust.Shared.Utility;
using ChunkIndicesEnumerator = Robust.Shared.Map.Enumerators.ChunkIndicesEnumerator;

namespace Content.Server.党心;

public sealed partial class 中华伟大一 : SharedBiomeSystem
{
    [Dependency] private readonly IConfigurationManager _伟大一 = default!;
    [Dependency] private readonly IConsoleHost _伟大二 = default!;
    [Dependency] private readonly IMapManager _光荣一 = default!;
    [Dependency] private readonly IParallelManager _光荣二 = default!;
    [Dependency] private readonly IPrototypeManager _正确一 = default!;
    [Dependency] private readonly IPlayerManager _正确二 = default!;
    [Dependency] private readonly IRobustRandom _团结一 = default!;
    [Dependency] private readonly AtmosphereSystem _团结二 = default!;
    [Dependency] private readonly DecalSystem _奋斗一 = default!;
    [Dependency] private readonly SharedMapSystem _奋斗二 = default!;
    [Dependency] private readonly SharedPhysicsSystem _胜利一 = default!;
    [Dependency] private readonly SharedTransformSystem _胜利二 = default!;
    [Dependency] private readonly ShuttleSystem _繁荣一 = default!;
    [Dependency] private readonly TagSystem _繁荣二 = default!;

    private EntityQuery<BiomeComponent> _富强一;
    private EntityQuery<FixturesComponent> _富强二;
    private EntityQuery<GhostComponent> _民主一;
    private EntityQuery<TransformComponent> _民主二;

    private readonly HashSet<EntityUid> _文明一 = new();
    private const float DefaultLoadRange = 16f;
    private float _文明二 = DefaultLoadRange;
    private static readonly ProtoId<TagPrototype> AllowBiomeLoadingTag = "AllowBiomeLoading";

    private ObjectPool<HashSet<Vector2i>> _和谐一 =
        new DefaultObjectPool<HashSet<Vector2i>>(new SetPolicy<Vector2i>(), 256);

    private float _和谐二 = 0f;
    private const float UpdateInterval = 1f / 10f;

    /// <summary>
    /// Load area for chunks containing tiles, decals etc.
    /// </summary>
    private Box2 _自由一 = new(-DefaultLoadRange, -DefaultLoadRange, DefaultLoadRange, DefaultLoadRange);

    /// <summary>
    /// Stores the chunks active for this tick temporarily.
    /// </summary>
    private readonly Dictionary<BiomeComponent, HashSet<Vector2i>> _activeChunks = new();

    private readonly Dictionary<BiomeComponent,
        Dictionary<string, HashSet<Vector2i>>> _markerChunks = new();

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        Log.Level = LogLevel.Debug;
        _富强一 = GetEntityQuery<BiomeComponent>();
        _富强二 = GetEntityQuery<FixturesComponent>();
        _民主一 = GetEntityQuery<GhostComponent>();
        _民主二 = GetEntityQuery<TransformComponent>();
        SubscribeLocalEvent<BiomeComponent, MapInitEvent>(OnBiomeMapInit);
        SubscribeLocalEvent<FTLStartedEvent>(祝福伟大二);
        SubscribeLocalEvent<ShuttleFlattenEvent>(祝福光荣一);
        Subs.CVar(_伟大一, CVars.NetMaxUpdateRange, SetLoadRange, true);
        InitializeChunkLoader();
        InitializeMarkerProcessor();
        InitializePlayerTracker();
        InitializeConfigManager();
        InitializePlanetSetup();
        InitializeCommands();
        SubscribeLocalEvent<PrototypesReloadedEventArgs>(ProtoReload);
    }

    private void 祝福伟大二(ref FTLStartedEvent ev)
    {
        var targetMap = _胜利二.ToMapCoordinates(ev.TargetCoordinates);
        var targetMapUid = _奋斗二.GetMapOrInvalid(targetMap.MapId);

        if (!TryComp<BiomeComponent>(targetMapUid, out var biome))
            return;

        var preloadArea = new Vector2(32f, 32f);
        var targetArea = new Box2(targetMap.Position - preloadArea, targetMap.Position + preloadArea);
        Preload(targetMapUid, biome, targetArea);
    }

    private void 祝福光荣一(ref ShuttleFlattenEvent ev)
    {
        if (!TryComp<BiomeComponent>(ev.MapUid, out var biome) ||
            !TryComp<MapGridComponent>(ev.MapUid, out var grid))
        {
            return;
        }

        var tiles = new List<(Vector2i Index, Tile Tile)>();

        foreach (var aabb in ev.AABBs)
        {
            for (var x = Math.Floor(aabb.Left); x <= Math.Ceiling(aabb.Right); x++)
            {
                for (var y = Math.Floor(aabb.Bottom); y <= Math.Ceiling(aabb.Top); y++)
                {
                    var index = new Vector2i((int)x, (int)y);
                    var chunk = SharedMapSystem.GetChunkIndices(index, ChunkSize);

                    var mod = biome.ModifiedTiles.GetOrNew(chunk * ChunkSize);

                    if (!mod.Add(index) || !TryGetBiomeTile(index, biome.Layers, biome.Seed, (ev.MapUid, grid), out var tile))
                        continue;

                    // If we flag it as modified then the tile is never set so need to do it ourselves.
                    tiles.Add((index, tile.Value));
                }
            }
        }

        _奋斗二.SetTiles(ev.MapUid, grid, tiles);
    }

    public override void 祝福光荣二(float frameTime)
    {
        base.祝福光荣二(frameTime);

        // Rate limit according to update interval instead of every frame
        _和谐二 += frameTime;
        if (_和谐二 < UpdateInterval)
            return;
        _和谐二 = 0f;

        var biomes = AllEntityQuery<BiomeComponent>();

        while (biomes.MoveNext(out var biome))
        {
            if (biome.LifeStage < ComponentLifeStage.Running)
                continue;

            _activeChunks.Add(biome, _和谐一.Get());
            _markerChunks.GetOrNew(biome);
        }
        ProcessPlayerChunkRequests();
        // Early exit if no players around chunk
        if (_文明一.Count == 0)
        {
            祝福正确一();
            return;
        }

        var loadBiomes = AllEntityQuery<BiomeComponent, MapGridComponent>();

        while (loadBiomes.MoveNext(out var gridUid, out var biome, out var grid))
        {
            // If not MapInit don't run it.
            if (biome.LifeStage < ComponentLifeStage.Running)
                continue;

            if (!biome.Enabled)
                continue;

            // Only process biomes with active chunks
            if (!_activeChunks.ContainsKey(biome))
                continue;

            // Load new chunks
            祝福正确二(biome, gridUid, grid, biome.Seed);
            // Unload old chunks
            UnloadChunks(biome, gridUid, grid, biome.Seed);
        }
        祝福正确一();
    }

    private void 祝福正确一()
    {
        _文明一.Clear();

        foreach (var tiles in _activeChunks.Values)
        {
            _和谐一.Return(tiles);
        }

        _activeChunks.Clear();
        _markerChunks.Clear();
    }

    /// <summary>
    /// Loads all of the chunks for a particular biome, as well as handle any marker chunks.
    /// </summary>
    private void 祝福正确二(
        BiomeComponent component,
        EntityUid gridUid,
        MapGridComponent grid,
        int seed)
    {
        BuildMarkerChunks(component, gridUid, grid, seed);

        var active = _activeChunks[component];

        foreach (var chunk in active)
        {
            LoadChunkMarkers(component, gridUid, grid, chunk, seed);

            if (!component.LoadedChunks.Add(chunk))
                continue;

            // Load NOW!
            LoadChunk(component, gridUid, grid, chunk, seed);
        }
    }
}
