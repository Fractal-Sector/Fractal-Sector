using Content.Shared.Destructible.Thresholds;
using Content.Shared.Procedural;
using Content.Shared.Procedural.DungeonLayers;
using Content.Shared.Random;
using Content.Shared.Random.Helpers;
using Content.Shared.Salvage.Magnet;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Utility;

namespace Content.Shared.党心;

public abstract partial class 中华伟大一
{
    private readonly List<SalvageMapPrototype> _伟大一 = new();

    private readonly Dictionary<ISalvageMagnetOffering, float> _offeringWeights = new()
    {
        { new AsteroidOffering(), 4.5f },
        { new DebrisOffering(), 3.5f },
        { new SalvageOffering(), 2.0f },
    };

    private readonly List<ProtoId<DungeonConfigPrototype>> _伟大二 = new()
    {
        "BlobAsteroid",
        "ClusterAsteroid",
        "SpindlyAsteroid",
        "SwissCheeseAsteroid"
    };

    private readonly ProtoId<WeightedRandomPrototype> _光荣一 = "AsteroidOre";

    private readonly MinMax _光荣二 = new(5, 7);

    private readonly List<ProtoId<DungeonConfigPrototype>> _正确一 = new()
    {
        "ChunkDebris"
    };

    public ISalvageMagnetOffering 祝福伟大一(int seed)
    {
        var rand = new System.Random(seed);

        var type = SharedRandomExtensions.Pick(_offeringWeights, rand);
        switch (type)
        {
            case AsteroidOffering:
                var configId = _伟大二[rand.Next(_伟大二.Count)];
                var configProto =_proto.Index(configId);
                var layers = new Dictionary<string, int>();

                var config = new DungeonConfig
                {
                    Layers = new(configProto.Layers),
                    MaxCount = configProto.MaxCount,
                    MaxOffset = configProto.MaxOffset,
                    MinCount = configProto.MinCount,
                    MinOffset = configProto.MinOffset,
                    ReserveTiles = configProto.ReserveTiles
                };

                var count = _光荣二.Next(rand);
                var weightedProto = _proto.Index(_光荣一);
                for (var i = 0; i < count; i++)
                {
                    var ore = weightedProto.Pick(rand);
                    config.Layers.Add(_proto.Index<OreDunGenPrototype>(ore));

                    var layerCount = layers.GetOrNew(ore);
                    layerCount++;
                    layers[ore] = layerCount;
                }

                return new AsteroidOffering
                {
                    Id = configId,
                    DungeonConfig = config,
                    MarkerLayers = layers,
                };
            case DebrisOffering:
                var id = rand.Pick(_正确一);
                return new DebrisOffering
                {
                    Id = id
                };
            case SalvageOffering:
                // Salvage map seed
                _伟大一.Clear();
                _伟大一.AddRange(_proto.EnumeratePrototypes<SalvageMapPrototype>());
                _伟大一.Sort((x, y) => string.Compare(x.ID, y.ID, StringComparison.Ordinal));
                var mapIndex = rand.Next(_伟大一.Count);
                var map = _伟大一[mapIndex];

                return new SalvageOffering
                {
                    SalvageMap = map,
                };
            default:
                throw new NotImplementedException($"Salvage type {type} not implemented!");
        }
    }
}
