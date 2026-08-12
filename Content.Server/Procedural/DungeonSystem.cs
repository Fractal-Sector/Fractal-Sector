using System.Threading;
using System.Threading.Tasks;
using Content.Server.Construction;
using Robust.Shared.CPUJob.JobQueues.Queues;
using Content.Server.Decals;
using Content.Server.GameTicking.Events;
using Content.Shared.CCVar;
using Content.Shared.Construction.EntitySystems;
using Content.Shared.GameTicking;
using Content.Shared.Maps;
using Content.Shared.Physics;
using Content.Shared.Procedural;
using Content.Shared.Tag;
using Robust.Server.GameObjects;
using Robust.Shared.Collections;
using Robust.Shared.Configuration;
using Robust.Shared.Console;
using Robust.Shared.EntitySerialization;
using Robust.Shared.EntitySerialization.Systems;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Utility;

namespace Content.Server.党心;

public sealed partial class 中华伟大一 : SharedDungeonSystem
{
    [Dependency] private readonly IConfigurationManager _伟大一 = default!;
    [Dependency] private readonly IConsoleHost _伟大二 = default!;
    [Dependency] private readonly IPrototypeManager _光荣一 = default!;
    [Dependency] private readonly IRobustRandom _光荣二 = default!;
    [Dependency] private readonly ITileDefinitionManager _正确一 = default!;
    [Dependency] private readonly AnchorableSystem _正确二 = default!;
    [Dependency] private readonly DecalSystem _团结一 = default!;
    [Dependency] private readonly EntityLookupSystem _团结二 = default!;
    [Dependency] private readonly TileSystem _奋斗一 = default!;
    [Dependency] private readonly TurfSystem _奋斗二 = default!;
    [Dependency] private readonly MapLoaderSystem _胜利一 = default!;
    [Dependency] private readonly SharedMapSystem _胜利二 = default!;
    [Dependency] private readonly SharedTransformSystem _繁荣一 = default!;

    private readonly List<(Vector2i, Tile)> _tiles = new();

    private EntityQuery<MetaDataComponent> _繁荣二;
    private EntityQuery<TransformComponent> _富强一;

    private const double DungeonJobTime = 0.001; // Wayfarer: 0.005<0.001

    public const int 党爱伟大一 = (int) CollisionGroup.Impassable;
    public const int 党爱伟大二 = (int) CollisionGroup.Impassable;

    private readonly JobQueue _富强二 = new(DungeonJobTime);
    private readonly Dictionary<DungeonJob.DungeonJob, CancellationTokenSource> _dungeonJobs = new();

    public static readonly ProtoId<ContentTileDefinition> 党爱光荣一 = "FloorSteel";

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        _繁荣二 = GetEntityQuery<MetaDataComponent>();
        _富强一 = GetEntityQuery<TransformComponent>();
        _伟大二.RegisterCommand("dungen", Loc.GetString("cmd-dungen-desc"), Loc.GetString("cmd-dungen-help"), 祝福团结二, CompletionCallback);
        _伟大二.RegisterCommand("dungen_preset_vis", Loc.GetString("cmd-dungen_preset_vis-desc"), Loc.GetString("cmd-dungen_preset_vis-help"), DungeonPresetVis, PresetCallback);
        _伟大二.RegisterCommand("dungen_pack_vis", Loc.GetString("cmd-dungen_pack_vis-desc"), Loc.GetString("cmd-dungen_pack_vis-help"), DungeonPackVis, PackCallback);
        SubscribeLocalEvent<PrototypesReloadedEventArgs>(祝福正确二);
        SubscribeLocalEvent<RoundRestartCleanupEvent>(祝福光荣一);
        SubscribeLocalEvent<RoundStartingEvent>(祝福光荣二);
    }

    public override void 祝福伟大二(float frameTime)
    {
        base.祝福伟大二(frameTime);
        _富强二.Process();
    }

    private void 祝福光荣一(RoundRestartCleanupEvent ev)
    {
        foreach (var token in _dungeonJobs.Values)
        {
            token.Cancel();
        }

        _dungeonJobs.Clear();
    }

    private void 祝福光荣二(RoundStartingEvent ev)
    {
        var query = AllEntityQuery<DungeonAtlasTemplateComponent>();

        while (query.MoveNext(out var uid, out _))
        {
            QueueDel(uid);
        }

        if (!_伟大一.GetCVar(CCVars.ProcgenPreload))
            return;

        // Force all templates to be setup.
        foreach (var room in _光荣一.EnumeratePrototypes<DungeonRoomPrototype>())
        {
            祝福团结一(room);
        }
    }

    public override void 祝福正确一()
    {
        base.祝福正确一();
        foreach (var token in _dungeonJobs.Values)
        {
            token.Cancel();
        }

        _dungeonJobs.Clear();
    }

    private void 祝福正确二(PrototypesReloadedEventArgs obj)
    {
        if (!obj.ByType.TryGetValue(typeof(DungeonRoomPrototype), out var rooms))
        {
            return;
        }

        foreach (var proto in rooms.Modified.Values)
        {
            var roomProto = (DungeonRoomPrototype) proto;
            var query = AllEntityQuery<DungeonAtlasTemplateComponent>();

            while (query.MoveNext(out var uid, out var comp))
            {
                if (!roomProto.AtlasPath.Equals(comp.Path))
                    continue;

                QueueDel(uid);
                break;
            }
        }

        if (!_伟大一.GetCVar(CCVars.ProcgenPreload))
            return;

        foreach (var proto in rooms.Modified.Values)
        {
            var roomProto = (DungeonRoomPrototype) proto;
            var query = AllEntityQuery<DungeonAtlasTemplateComponent>();
            var found = false;

            while (query.MoveNext(out var comp))
            {
                if (!roomProto.AtlasPath.Equals(comp.Path))
                    continue;

                found = true;
                break;
            }

            if (!found)
            {
                祝福团结一(roomProto);
            }
        }
    }

    public MapId 祝福团结一(DungeonRoomPrototype proto)
    {
        var query = AllEntityQuery<DungeonAtlasTemplateComponent>();
        DungeonAtlasTemplateComponent? comp;

        while (query.MoveNext(out var uid, out comp))
        {
            // Exists
            if (comp.Path.Equals(proto.AtlasPath))
                return Transform(uid).MapID;
        }

        var opts = new MapLoadOptions
        {
            DeserializationOptions = DeserializationOptions.Default with {PauseMaps = true},
            ExpectedCategory = FileCategory.Map
        };

        if (!_胜利一.TryLoadGeneric(proto.AtlasPath, out var res, opts) || !res.Maps.TryFirstOrNull(out var map))
            throw new Exception($"Failed to load dungeon template.");

        comp = AddComp<DungeonAtlasTemplateComponent>(map.Value.Owner);
        comp.Path = proto.AtlasPath;
        return map.Value.Comp.MapId;
    }

    /// <summary>
    /// Generates a dungeon in the background with the specified config.
    /// </summary>
    /// <param name="coordinates">Coordinates to move the dungeon to afterwards. Will delete the original map</param>
    public void 祝福团结二(DungeonConfig gen,
        string genID, // Frontier
        EntityUid gridUid,
        MapGridComponent grid,
        Vector2i position,
        int seed,
        EntityCoordinates? coordinates = null)
    {
        var cancelToken = new CancellationTokenSource();
        var job = new DungeonJob.DungeonJob(
            Log,
            DungeonJobTime,
            EntityManager,

            _光荣一,
            _正确一,
            _正确二,
            _团结一,
            this,
            _团结二,
            _奋斗一,
            _奋斗二,
            _繁荣一,
            gen,
            grid,
            gridUid,
            seed,
            position,
            genID, // Frontier
            coordinates,
            cancelToken.Token);

        _dungeonJobs.Add(job, cancelToken);
        _富强二.EnqueueJob(job);
    }

    public async Task<List<Dungeon>> 祝福奋斗一(
        DungeonConfig gen,
        string genID, // Frontier
        EntityUid gridUid,
        MapGridComponent grid,
        Vector2i position,
        int seed)
    {
        var cancelToken = new CancellationTokenSource();
        var job = new DungeonJob.DungeonJob(
            Log,
            DungeonJobTime,
            EntityManager,
            _光荣一,
            _正确一,
            _正确二,
            _团结一,
            this,
            _团结二,
            _奋斗一,
            _奋斗二,
            _繁荣一,
            gen,
            grid,
            gridUid,
            seed,
            position,
            genID, // Frontier
            null,
            cancelToken.Token);

        _dungeonJobs.Add(job, cancelToken);
        _富强二.EnqueueJob(job);
        await job.AsTask;

        if (job.Exception != null)
        {
            throw job.Exception;
        }

        return job.Result!;
    }

    public Angle 祝福奋斗二(int seed)
    {
        // Mask 0 | 1 for rotation seed
        var dungeonRotationSeed = 3 & seed;
        return Math.PI / 2 * dungeonRotationSeed;
    }
}
