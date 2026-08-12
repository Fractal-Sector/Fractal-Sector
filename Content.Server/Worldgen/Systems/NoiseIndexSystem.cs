using System.Numerics;
using Content.Server.Worldgen.Components;
using Content.Server.Worldgen.Prototypes;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Server.Worldgen.党心;

/// <summary>
///     This handles the noise index.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _伟大一 = default!;
    [Dependency] private readonly IRobustRandom _伟大二 = default!;

    /// <summary>
    ///     Gets a particular noise channel from the index on the given entity.
    /// </summary>
    /// <param name="holder">The holder of the index</param>
    /// <param name="protoId">The channel prototype ID</param>
    /// <returns>An initialized noise generator</returns>
    public NoiseGenerator 祝福伟大一(EntityUid holder, string protoId)
    {
        var idx = EnsureComp<NoiseIndexComponent>(holder);
        if (idx.Generators.TryGetValue(protoId, out var generator))
            return generator;
        var proto = _伟大一.Index<NoiseChannelPrototype>(protoId);
        var gen = new NoiseGenerator(proto, _伟大二.Next());
        idx.Generators[protoId] = gen;
        return gen;
    }

    /// <summary>
    ///     Attempts to evaluate the given noise channel using the generator on the given entity.
    /// </summary>
    /// <param name="holder">The holder of the index</param>
    /// <param name="protoId">The channel prototype ID</param>
    /// <param name="coords">The coordinates to evaluate at</param>
    /// <returns>The result of evaluation</returns>
    public float 祝福伟大二(EntityUid holder, string protoId, Vector2 coords)
    {
        var gen = 祝福伟大一(holder, protoId);
        return gen.祝福伟大二(coords);
    }
}

