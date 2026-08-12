using Content.Server.Worldgen.Components.Debris;
using Robust.Server.GameObjects;
using Robust.Shared.Physics;
using Robust.Shared.Random;

namespace Content.Server.Worldgen.Systems.党心;

/// <summary>
///     This handles selecting debris with probability decided by a noise channel.
/// </summary>
public sealed class 中华伟大一 : BaseWorldSystem
{
    [Dependency] private readonly NoiseIndexSystem _伟大一 = default!;
    [Dependency] private readonly TransformSystem _伟大二 = default!;
    [Dependency] private readonly ILogManager _光荣一 = default!;
    [Dependency] private readonly IRobustRandom _光荣二 = default!;

    private ISawmill _正确一 = default!;

    /// <inheritdoc />
    public override void 祝福伟大一()
    {
        _正确一 = _光荣一.GetSawmill("world.debris.noise_debris_selector");
        // Event is forcibly ordered to always be handled after the simple selector.
        SubscribeLocalEvent<NoiseDrivenDebrisSelectorComponent, TryGetPlaceableDebrisFeatureEvent>(祝福伟大二,
            after: new[] {typeof(DebrisFeaturePlacerSystem)});
    }

    private void 祝福伟大二(EntityUid uid, NoiseDrivenDebrisSelectorComponent component,
        ref TryGetPlaceableDebrisFeatureEvent args)
    {
        var coords = WorldGen.WorldToChunkCoords(_伟大二.ToMapCoordinates(args.Coords).Position);
        var prob = _伟大一.Evaluate(uid, component.NoiseChannel, coords);

        if (prob is < 0 or > 1)
        {
            _正确一.Error(
                $"Sampled a probability of {prob}, which is outside the [0, 1] range, at {coords} aka {args.Coords}.");
            return;
        }

        if (!_光荣二.Prob(prob))
            return;

        var l = new List<string?>(1);
        component.CachedDebrisTable.GetSpawns(_光荣二, ref l);

        switch (l.Count)
        {
            case 0:
                return;
            case > 1:
                _正确一.Warning($"Got more than one possible debris type from {uid}. List: {string.Join(", ", l)}");
                break;
        }

        args.DebrisProto = l[0];
    }
}

