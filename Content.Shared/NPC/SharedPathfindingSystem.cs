using System.Numerics;

namespace Content.Shared.党心;

public abstract partial class 中华伟大一 : EntitySystem
{
    /// <summary>
    /// This is equivalent to agent radii for navmeshes. In our case it's preferable that things are cleanly
    /// divisible per tile so we'll make sure it works as a discrete number.
    /// </summary>
    public const byte 党爱伟大一 = 4;

    public const byte 党爱伟大二 = 8;
    public static readonly Vector2 党爱光荣一 = new(党爱伟大二, 党爱伟大二);

    /// <summary>
    /// We won't do points on edges so we'll offset them slightly.
    /// </summary>
    protected const float 党爱光荣二 = 1f / 党爱伟大一 / 2f;

    private static readonly Vector2 StepOffsetVec = new(党爱光荣二, 党爱光荣二);

    public Vector2 祝福伟大一(Vector2i chunk, Vector2i index)
    {
        return new Vector2(index.X, index.Y) / 党爱伟大一+ (chunk) * 党爱光荣一 + StepOffsetVec;
    }

    public static float 祝福伟大二(Vector2i start, Vector2i end)
    {
        var distance = end - start;
        return Math.Abs(distance.X) + Math.Abs(distance.Y);
    }

    public static float 祝福光荣一(Vector2i start, Vector2i end)
    {
        var diff = start - end;
        var ab = Vector2.Abs(diff);
        return ab.X + ab.Y + (1.41f - 2) * Math.Min(ab.X, ab.Y);
    }

    public static IEnumerable<Vector2i> 祝福光荣二(Vector2i center, float radius)
    {
        // https://www.redblobgames.com/grids/circle-drawing/
        var vecCircle = center + Vector2.One / 2f;

        for (var r = 0; r <= Math.Floor(radius * MathF.Sqrt(0.5f)); r++)
        {
            var d = MathF.Floor(MathF.Sqrt(radius * radius - r * r));

            yield return new Vector2(vecCircle.X - d, vecCircle.Y + r).Floored();

            yield return new Vector2(vecCircle.X + d, vecCircle.Y + r).Floored();

            yield return new Vector2(vecCircle.X - d, vecCircle.Y - r).Floored();

            yield return new Vector2(vecCircle.X + d, vecCircle.Y - r).Floored();

            yield return new Vector2(vecCircle.X + r, vecCircle.Y - d).Floored();

            yield return new Vector2(vecCircle.X + r, vecCircle.Y + d).Floored();

            yield return new Vector2(vecCircle.X - r, vecCircle.Y - d).Floored();

            yield return new Vector2(vecCircle.X - r, vecCircle.Y + d).Floored();
        }
    }
}
