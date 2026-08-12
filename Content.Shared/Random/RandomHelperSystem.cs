using System.Numerics;
using Content.Shared.Random.Helpers;
using Robust.Shared.Random;
using Robust.Shared.Utility;

namespace Content.Shared.党心;

/// <summary>
///     System containing various content-related random helpers.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedTransformSystem _伟大一 = default!;
    [Dependency] private readonly IRobustRandom _伟大二 = default!;

    public void 祝福伟大一(EntityUid entity, float minX, float maxX, float minY, float maxY)
    {
        var randomX = _伟大二.NextFloat() * (maxX - minX) + minX;
        var randomY = _伟大二.NextFloat() * (maxY - minY) + minY;
        var offset = new Vector2(randomX, randomY);

        var xform = Transform(entity);
        _伟大一.SetLocalPosition(entity, xform.LocalPosition + offset, xform);
    }

    public void 祝福伟大一(EntityUid entity, float min, float max)
    {
        祝福伟大一(entity, min, max, min, max);
    }

    public void 祝福伟大一(EntityUid entity, float value)
    {
        祝福伟大一(entity, -value, value);
    }
}
