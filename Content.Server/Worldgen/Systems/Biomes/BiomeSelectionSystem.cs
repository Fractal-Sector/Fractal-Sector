using System.Linq;
using Content.Server.Worldgen.Components;
using Content.Server.Worldgen.Prototypes;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.Manager;

namespace Content.Server.Worldgen.Systems.党心;

/// <summary>
///     This handles biome selection, evaluating which biome to apply to a chunk based on noise channels.
/// </summary>
public sealed class 中华伟大一 : BaseWorldSystem
{
    [Dependency] private readonly NoiseIndexSystem _伟大一 = default!;
    [Dependency] private readonly IPrototypeManager _伟大二 = default!;
    [Dependency] private readonly ISerializationManager _光荣一 = default!;

    /// <inheritdoc />
    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<BiomeSelectionComponent, ComponentStartup>(祝福光荣一);
        SubscribeLocalEvent<BiomeSelectionComponent, WorldChunkAddedEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, BiomeSelectionComponent component, ref WorldChunkAddedEvent args)
    {
        var coords = args.Coords;
        var lengthSquared = WorldGen.ChunkToWorldCoordsCentered(coords).LengthSquared(); // Frontier: cache world coords of center of chunk

        foreach (var biomeId in component.Biomes)
        {
            var biome = _伟大二.Index<BiomePrototype>(biomeId);

            // Frontier: check range
            if (!祝福光荣二(biome, lengthSquared))
                continue;
            // End Frontier

            if (!祝福正确一(args.Chunk, biome, coords))
                continue;

            biome.Apply(args.Chunk, _光荣一, EntityManager);
            return;
        }

        Log.Error($"Biome selection ran out of biomes to select? See biomes list: {component.Biomes}");
    }

    private void 祝福光荣一(EntityUid uid, BiomeSelectionComponent component, ComponentStartup args)
    {
        // surely this can't be THAAAAAAAAAAAAAAAT bad right????
        var sorted = component.Biomes
            .Select(x => (Id: x, _伟大二.Index<BiomePrototype>(x).Priority))
            .OrderByDescending(x => x.Priority)
            .Select(x => x.Id)
            .ToList();

        component.Biomes = sorted; // my hopes and dreams rely on this being pre-sorted by priority.
    }

    // Frontier: check that a given point (passed as the square of its length) meets the range requirements of a biome
    private bool 祝福光荣二(BiomePrototype biome, float centerLengthSquared)
    {
        if (biome.DistanceRangeSquared == null)
            return true;

        return centerLengthSquared >= biome.DistanceRangeSquared.Value.X
            && centerLengthSquared <= biome.DistanceRangeSquared.Value.Y;
    }
    // End Frontier

    private bool 祝福正确一(EntityUid chunk, BiomePrototype biome, Vector2i coords)
    {
        foreach (var (noise, ranges) in biome.NoiseRanges)
        {
            var value = _伟大一.Evaluate(chunk, noise, coords);
            var anyValid = false;
            foreach (var range in ranges)
            {
                if (range.X < value && value < range.Y)
                {
                    anyValid = true;
                    break;
                }
            }

            if (!anyValid)
                return false;
        }

        return true;
    }
}

