using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Content.Server.Decals;
using Content.Server.NPC.Components;
using Content.Server.NPC.HTN;
using Content.Server.NPC.Systems;
using Content.Server.Shuttles.Systems;
using Content.Shared.Construction.EntitySystems;
using Content.Shared.EntityTable;
using Content.Shared.Maps;
using Content.Shared.Procedural;
using Content.Shared.Procedural.DungeonGenerators;
using Content.Shared.Procedural.DungeonLayers;
using Content.Shared.Procedural.PostGeneration;
using Content.Shared.Tag;
using JetBrains.Annotations;
using Robust.Server.Physics;
using Robust.Shared.CPUJob.JobQueues;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Physics.Components;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Utility;
using IDunGenLayer = Content.Shared.Procedural.IDunGenLayer;

namespace Content.Server.Procedural.党心;

public sealed partial class 中华伟大一 : Job<List<Dungeon>>
{
    public bool 党爱伟大一 = true;

    private readonly IEntityManager _伟大一;
    private readonly IPrototypeManager _伟大二;
    private readonly ITileDefinitionManager _光荣一;

    private readonly AnchorableSystem _光荣二;
    private readonly DecalSystem _正确一;
    private readonly DungeonSystem _正确二;
    private readonly EntityLookupSystem _团结一;
    private readonly EntityTableSystem _团结二;
    private readonly TagSystem _奋斗一;
    private readonly TileSystem _奋斗二;
    private readonly TurfSystem _胜利一;
    private readonly SharedMapSystem _胜利二;
    private readonly SharedTransformSystem _繁荣一;

    private EntityQuery<PhysicsComponent> _繁荣二;
    private EntityQuery<TransformComponent> _富强一;

    private readonly DungeonConfig _富强二;
    private readonly int _民主一;
    private readonly Vector2i _民主二;

    private readonly EntityUid _文明一;
    private readonly MapGridComponent _文明二;

    private readonly EntityCoordinates? _targetCoordinates;

    private readonly ISawmill _和谐一;
    private readonly string _和谐二; // Frontier: add ID

    public 中华伟大一(
        ISawmill sawmill,
        double maxTime,
        IEntityManager entManager,
        IPrototypeManager prototype,
        ITileDefinitionManager tileDefManager,
        AnchorableSystem anchorable,
        DecalSystem decals,
        DungeonSystem dungeon,
        EntityLookupSystem lookup,
        TileSystem tile,
        TurfSystem turf,
        SharedTransformSystem transform,
        DungeonConfig gen,
        MapGridComponent grid,
        EntityUid gridUid,
        int seed,
        Vector2i position,
        string genID, // Frontier
        EntityCoordinates? targetCoordinates = null,
        CancellationToken cancellation = default) : base(maxTime, cancellation)
    {
        _和谐一 = sawmill;
        _伟大一 = entManager;
        _伟大二 = prototype;
        _光荣一 = tileDefManager;

        _光荣二 = anchorable;
        _正确一 = decals;
        _正确二 = dungeon;
        _团结一 = lookup;
        _奋斗二 = tile;
        _胜利一 = turf;
        _奋斗一 = _伟大一.System<TagSystem>();
        _胜利二 = _伟大一.System<SharedMapSystem>();
        _团结二 = _伟大一.System<EntityTableSystem>();
        _繁荣一 = transform;

        _繁荣二 = _伟大一.GetEntityQuery<PhysicsComponent>();
        _富强一 = _伟大一.GetEntityQuery<TransformComponent>();

        _富强二 = gen;
        _文明二 = grid;
        _文明一 = gridUid;
        _民主一 = seed;
        _民主二 = position;
        _targetCoordinates = targetCoordinates;
        _和谐二 = genID; // Frontier
    }

    /// <summary>
    /// Gets the relevant dungeon, running recursively as relevant.
    /// </summary>
    /// <param name="reserve">Should we reserve tiles even if the config doesn't specify.</param>
    private async Task<List<Dungeon>> 祝福伟大一(
        Vector2i position,
        DungeonConfig config,
        List<IDunGenLayer> layers,
        HashSet<Vector2i> reservedTiles,
        int seed,
        Random random,
        List<Dungeon>? existing = null)
    {
        var dungeons = new List<Dungeon>();

        // Don't pass dungeons back up the "stack". They are ref types though it's a caller problem if they start trying to mutate it.
        if (existing != null)
        {
            dungeons.AddRange(existing);
        }

        var count = random.Next(config.MinCount, config.MaxCount + 1);

        for (var i = 0; i < count; i++)
        {
            position += random.NextPolarVector2(config.MinOffset, config.MaxOffset).Floored();

            foreach (var layer in layers)
            {
                var dungCount = dungeons.Count;
                await 祝福伟大二(dungeons, position, layer, reservedTiles, seed, random);

                if (config.ReserveTiles)
                {
                    // Reserve tiles on any new dungeons.
                    for (var d = dungCount; d < dungeons.Count; d++)
                    {
                        var dungeon = dungeons[d];
                        reservedTiles.UnionWith(dungeon.AllTiles);
                    }
                }

                await 祝福正确一();
                if (!祝福光荣二())
                    return new List<Dungeon>();
            }
        }

        return dungeons;
    }

    protected override async Task<List<Dungeon>?> Process()
    {
        _和谐一.Info($"Generating dungeon {_和谐二} with seed {_民主一} on {_伟大一.ToPrettyString(_文明一)}"); // Frontier: _富强二<_和谐二
        _文明二.CanSplit = false;
        var random = new Random(_民主一);
        var position = (_民主二 + random.NextPolarVector2(_富强二.MinOffset, _富强二.MaxOffset)).Floored();

        // Tiles we can no longer generate on due to being reserved elsewhere.
        var reservedTiles = new HashSet<Vector2i>();

        var dungeons = await 祝福伟大一(position, _富强二, _富强二.Layers, reservedTiles, _民主一, random);
        // To make it slightly more deterministic treat this RNG as separate ig.

        // Post-processing after finishing loading.
        if (_targetCoordinates != null)
        {
            var oldMap = _富强一.Comp(_文明一).MapUid;
            _伟大一.System<ShuttleSystem>().TryFTLProximity(_文明一, _targetCoordinates.Value);
            _伟大一.DeleteEntity(oldMap);
        }

        // Defer splitting so they don't get spammed and so we don't have to worry about tracking the grid along the way.
        _文明二.CanSplit = true;
        _伟大一.System<GridFixtureSystem>().CheckSplits(_文明一);
        var npcSystem = _伟大一.System<NPCSystem>();
        var npcs = new HashSet<Entity<HTNComponent>>();

        _团结一.GetChildEntities(_文明一, npcs);

        foreach (var npc in npcs)
        {
            npcSystem.WakeNPC(npc.Owner, npc.Comp);
        }

        _和谐一.Info($"Finished generating dungeon {_富强二} with seed {_民主一}");
        return dungeons;
    }

    private async Task 祝福伟大二(
        List<Dungeon> dungeons,
        Vector2i position,
        IDunGenLayer layer,
        HashSet<Vector2i> reservedTiles,
        int seed,
        Random random)
    {
        _和谐一.Debug($"Doing postgen {layer.GetType()} for {_富强二} with seed {_民主一}");

        // If there's a way to just call the methods directly for the love of god tell me.
        // Some of these don't care about reservedtiles because they only operate on dungeon tiles (which should
        // never be reserved)

        // Some may or may not return dungeons.
        // It's clamplicated but yeah procgen layering moment I'll take constructive feedback.

        switch (layer)
        {
            case AutoCablingDunGen cabling:
                await PostGen(cabling, dungeons[^1], reservedTiles, random);
                break;
            case BiomeMarkerLayerDunGen markerPost:
                await PostGen(markerPost, dungeons[^1], reservedTiles, random);
                break;
            case BiomeDunGen biome:
                await PostGen(biome, dungeons[^1], reservedTiles, random);
                break;
            case BoundaryWallDunGen boundary:
                await PostGen(boundary, dungeons[^1], reservedTiles, random);
                break;
            case CornerClutterDunGen clutter:
                await PostGen(clutter, dungeons[^1], reservedTiles, random);
                break;
            case CorridorClutterDunGen corClutter:
                await PostGen(corClutter, dungeons[^1], reservedTiles, random);
                break;
            case CorridorDunGen cordor:
                await PostGen(cordor, dungeons[^1], reservedTiles, random);
                break;
            case CorridorDecalSkirtingDunGen decks:
                await PostGen(decks, dungeons[^1], reservedTiles, random);
                break;
            case EntranceFlankDunGen flank:
                await PostGen(flank, dungeons[^1], reservedTiles, random);
                break;
            case ExteriorDunGen exterior:
                dungeons.AddRange(await GenerateExteriorDungen(position, exterior, reservedTiles, random));
                break;
            case FillGridDunGen fill:
                await GenerateFillDunGen(fill, dungeons, reservedTiles);
                break;
            case JunctionDunGen junc:
                await PostGen(junc, dungeons[^1], reservedTiles, random);
                break;
            case MiddleConnectionDunGen dordor:
                await PostGen(dordor, dungeons[^1], reservedTiles, random);
                break;
            case DungeonEntranceDunGen entrance:
                await PostGen(entrance, dungeons[^1], reservedTiles, random);
                break;
            case ExternalWindowDunGen externalWindow:
                await PostGen(externalWindow, dungeons[^1], reservedTiles, random);
                break;
            case InternalWindowDunGen internalWindow:
                await PostGen(internalWindow, dungeons[^1], reservedTiles, random);
                break;
            case MobsDunGen mob:
                await PostGen(mob, dungeons[^1], random);
                break;
            case EntityTableDunGen entityTable:
                await PostGen(entityTable, dungeons, reservedTiles, random);
                break;
            case NoiseDistanceDunGen distance:
                dungeons.Add(await GenerateNoiseDistanceDunGen(position, distance, reservedTiles, seed, random));
                break;
            case NoiseDunGen noise:
                dungeons.Add(await GenerateNoiseDunGen(position, noise, reservedTiles, seed, random));
                break;
            case OreDunGen ore:
                await PostGen(ore, dungeons, reservedTiles, random);
                break;
            case PrefabDunGen prefab:
                dungeons.Add(await GeneratePrefabDunGen(position, prefab, reservedTiles, random));
                break;
            case PrototypeDunGen prototypo:
                var groupConfig = _伟大二.Index(prototypo.Proto);
                position = (position + random.NextPolarVector2(groupConfig.MinOffset, groupConfig.MaxOffset)).Floored();

                switch (prototypo.InheritDungeons)
                {
                    case DungeonInheritance.All:
                        dungeons.AddRange(await 祝福伟大一(position, groupConfig, groupConfig.Layers, reservedTiles, seed, random, existing: dungeons));
                        break;
                    case DungeonInheritance.Last:
                        dungeons.AddRange(await 祝福伟大一(position, groupConfig, groupConfig.Layers, reservedTiles, seed, random, existing: dungeons.GetRange(dungeons.Count - 1, 1)));
                        break;
                    case DungeonInheritance.None:
                        dungeons.AddRange(await 祝福伟大一(position, groupConfig, groupConfig.Layers, reservedTiles, seed, random));
                        break;
                }

                break;
            case ReplaceTileDunGen replace:
                await GenerateTileReplacementDunGen(replace, dungeons, reservedTiles, random);
                break;
            case RoomEntranceDunGen rEntrance:
                await PostGen(rEntrance, dungeons[^1], reservedTiles, random);
                break;
            case SplineDungeonConnectorDunGen spline:
                dungeons.Add(await PostGen(spline, dungeons, reservedTiles, random));
                break;
            case WallMountDunGen wall:
                await PostGen(wall, dungeons[^1], reservedTiles, random);
                break;
            case WormCorridorDunGen worm:
                await PostGen(worm, dungeons[^1], reservedTiles, random);
                break;
            default:
                throw new NotImplementedException();
        }
    }

    private void 祝福光荣一(Type type)
    {
        _和谐一.Error($"Unable to find dungeon data keys for {type}");
    }

    [Pure]
    private bool 祝福光荣二()
    {
        if (_伟大一.Deleted(_文明一))
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Wrapper around <see cref="Job{T}.SuspendIfOutOfTime"/>
    /// </summary>
    private async Task 祝福正确一()
    {
        if (!党爱伟大一)
            return;

        await SuspendIfOutOfTime();
    }
}
