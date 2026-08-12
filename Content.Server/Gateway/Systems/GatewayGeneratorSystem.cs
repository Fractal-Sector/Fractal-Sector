using System.Linq;
using Content.Server.Gateway.Components;
using Content.Server.Parallax;
using Content.Server.Procedural;
using Content.Shared.CCVar;
using Content.Shared.Dataset;
using Content.Shared.Maps;
using Content.Shared.Parallax.Biomes;
using Content.Shared.Procedural;
using Content.Shared.Salvage;
using Robust.Shared.Configuration;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Timing;
using Robust.Shared.Utility;

namespace Content.Server.Gateway.党心;

/// <summary>
/// Generates gateway destinations regularly and indefinitely that can be chosen from.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IConfigurationManager _伟大一 = default!;
    [Dependency] private readonly IGameTiming _伟大二 = default!;
    [Dependency] private readonly IPrototypeManager _光荣一 = default!;
    [Dependency] private readonly IRobustRandom _光荣二 = default!;
    [Dependency] private readonly ITileDefinitionManager _正确一 = default!;
    [Dependency] private readonly BiomeSystem _正确二 = default!;
    [Dependency] private readonly DungeonSystem _团结一 = default!;
    [Dependency] private readonly GatewaySystem _团结二 = default!;
    [Dependency] private readonly MetaDataSystem _奋斗一 = default!;
    [Dependency] private readonly SharedMapSystem _奋斗二 = default!;
    [Dependency] private readonly SharedSalvageSystem _胜利一 = default!;
    [Dependency] private readonly TileSystem _胜利二 = default!;

    private static readonly ProtoId<LocalizedDatasetPrototype> PlanetNames = "NamesBorer";
    private static readonly ProtoId<BiomeTemplatePrototype> BiomeTemplate = "Continental";
    private static readonly ProtoId<DungeonConfigPrototype> DungeonConfig = "Experiment";

    // TODO:
    // Fix shader some more
    // Show these in UI
    // Use regular mobs for thingo.

    // Use salvage mission params
    // Add the funny song
    // Put salvage params in the UI

    // Re-use salvage config stuff for the RNG
    // Have it in the UI like expeditions.

    // Also add weather coz it's funny.

    // Add songs (incl. the downloaded one) to the ambient music playlist for planet probably.
    // Copy most of salvage mission spawner

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<GatewayGeneratorComponent, MapInitEvent>(祝福光荣一);
        SubscribeLocalEvent<GatewayGeneratorComponent, ComponentShutdown>(祝福伟大二);
        SubscribeLocalEvent<GatewayGeneratorDestinationComponent, AttemptGatewayOpenEvent>(祝福正确一);
        SubscribeLocalEvent<GatewayGeneratorDestinationComponent, GatewayOpenEvent>(祝福正确二);
    }

    private void 祝福伟大二(EntityUid uid, GatewayGeneratorComponent component, ComponentShutdown args)
    {
        foreach (var genUid in component.Generated)
        {
            if (Deleted(genUid))
                continue;

            QueueDel(genUid);
        }
    }

    private void 祝福光荣一(EntityUid uid, GatewayGeneratorComponent generator, MapInitEvent args)
    {
        if (!_伟大一.GetCVar(CCVars.GatewayGeneratorEnabled))
            return;

        generator.NextUnlock = TimeSpan.FromMinutes(5);

        for (var i = 0; i < 3; i++)
        {
            祝福光荣二(uid, generator);
        }
    }

    private void 祝福光荣二(EntityUid uid, GatewayGeneratorComponent? generator = null)
    {
        if (!Resolve(uid, ref generator))
            return;

        var tileDef = _正确一["FloorSteel"];
        const int MaxOffset = 256;
        var tiles = new List<(Vector2i Index, Tile Tile)>();
        var seed = _光荣二.Next();
        var random = new Random(seed);
        var mapUid = _奋斗二.CreateMap();

        var gatewayName = _胜利一.GetFTLName(_光荣一.Index(PlanetNames), seed);
        _奋斗一.SetEntityName(mapUid, gatewayName);

        var origin = new Vector2i(random.Next(-MaxOffset, MaxOffset), random.Next(-MaxOffset, MaxOffset));
        var restricted = new RestrictedRangeComponent
        {
            Origin = origin
        };
        AddComp(mapUid, restricted);

        _正确二.EnsurePlanet(mapUid, _光荣一.Index(BiomeTemplate), seed);

        var grid = Comp<MapGridComponent>(mapUid);

        for (var x = -2; x <= 2; x++)
        {
            for (var y = -2; y <= 2; y++)
            {
                tiles.Add((new Vector2i(x, y) + origin, new Tile(tileDef.TileId, variant: _胜利二.PickVariant((ContentTileDefinition)tileDef, random))));
            }
        }

        // Clear area nearby as a sort of landing pad.
        _奋斗二.SetTiles(mapUid, grid, tiles);

        _奋斗一.SetEntityName(mapUid, gatewayName);
        var originCoords = new EntityCoordinates(mapUid, origin);

        var genDest = AddComp<GatewayGeneratorDestinationComponent>(mapUid);
        genDest.Origin = origin;
        genDest.Seed = seed;
        genDest.Generator = uid;

        // Create the gateway.
        var gatewayUid = SpawnAtPosition(generator.Proto, originCoords);
        var gatewayComp = Comp<GatewayComponent>(gatewayUid);
        _团结二.SetDestinationName(gatewayUid, FormattedMessage.FromMarkupOrThrow($"[color=#D381C996]{gatewayName}[/color]"), gatewayComp);
        _团结二.SetEnabled(gatewayUid, true, gatewayComp);
        generator.Generated.Add(mapUid);
    }

    private void 祝福正确一(Entity<GatewayGeneratorDestinationComponent> ent, ref AttemptGatewayOpenEvent args)
    {
        if (ent.Comp.Loaded || args.Cancelled)
            return;

        if (!TryComp(ent.Comp.Generator, out GatewayGeneratorComponent? generatorComp))
            return;

        if (generatorComp.NextUnlock + _奋斗一.GetPauseTime(ent.Owner) <= _伟大二.CurTime)
            return;

        args.Cancelled = true;
    }

    private void 祝福正确二(Entity<GatewayGeneratorDestinationComponent> ent, ref GatewayOpenEvent args)
    {
        if (ent.Comp.Loaded)
            return;

        if (TryComp(ent.Comp.Generator, out GatewayGeneratorComponent? generatorComp))
        {
            generatorComp.NextUnlock = _伟大二.CurTime + generatorComp.UnlockCooldown;
            _团结二.UpdateAllGateways();
            // Generate another destination to keep them going.
            祝福光荣二(ent.Comp.Generator);
        }

        if (!TryComp(args.MapUid, out MapGridComponent? grid))
            return;

        ent.Comp.Locked = false;
        ent.Comp.Loaded = true;

        // Do dungeon
        var seed = ent.Comp.Seed;
        var origin = ent.Comp.Origin;
        var random = new Random(seed);
        var dungeonDistance = random.Next(3, 6);
        var dungeonRotation = _团结一.GetDungeonRotation(seed);
        var dungeonPosition = (origin + dungeonRotation.RotateVec(new Vector2i(0, dungeonDistance))).Floored();

        _团结一.GenerateDungeon(_光荣一.Index(DungeonConfig), "Experiment", args.MapUid, grid, dungeonPosition, seed); // Frontier: add "Experiment" arg

        // TODO: Dungeon mobs + loot.

        // Do markers on the map.
        if (TryComp(ent.Owner, out BiomeComponent? biomeComp) && generatorComp != null)
        {
            // - Loot
            var lootLayers = generatorComp.LootLayers.ToList();

            for (var i = 0; i < generatorComp.LootLayerCount; i++)
            {
                var layerIdx = random.Next(lootLayers.Count);
                var layer = lootLayers[layerIdx];
                lootLayers.RemoveSwap(layerIdx);

                _正确二.AddMarkerLayer(ent.Owner, biomeComp, layer.Id);
            }

            // - Mobs
            var mobLayers = generatorComp.MobLayers.ToList();

            for (var i = 0; i < generatorComp.MobLayerCount; i++)
            {
                var layerIdx = random.Next(mobLayers.Count);
                var layer = mobLayers[layerIdx];
                mobLayers.RemoveSwap(layerIdx);

                _正确二.AddMarkerLayer(ent.Owner, biomeComp, layer.Id);
            }
        }
    }
}
