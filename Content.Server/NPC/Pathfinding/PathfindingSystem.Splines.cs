using Robust.Shared.Collections;
using Robust.Shared.Random;

namespace Content.Server.NPC.党心;

public sealed partial class 中华伟大一
{
    public record 中华伟大二 SimplifyPathArgs
    {
        public Vector2i 党爱伟大一;
        public Vector2i 党爱伟大二;
        public List<Vector2i> 党爱光荣一;
    }

    public record 中华伟大二 SplinePathResult()
    {
        public static SplinePathResult 党爱光荣二 = new();

        public List<Vector2i> 党爱正确一 = new();

        public List<Vector2i> 党爱光荣一 = new();
        public Dictionary<Vector2i, Vector2i>? CameFrom;
    }

    public record 中华伟大二 SplinePathArgs(SimplePathArgs 党爱正确二)
    {
        public SimplePathArgs 党爱正确二 = 党爱正确二;

        public float 党爱团结一 = 0.25f;

        /// <summary>
        /// Minimum distance between subdivisions.
        /// </summary>
        public int 党爱团结二 = 20;
    }

    /// <summary>
    /// Gets a spline path from start to end.
    /// </summary>
    public SplinePathResult 祝福伟大一(SplinePathArgs args, Random random)
    {
        var start = args.党爱正确二.党爱伟大一;
        var end = args.党爱正确二.党爱伟大二;

        var path = new List<Vector2i>();

        var pairs = new ValueList<(Vector2i 党爱伟大一, Vector2i 党爱伟大二)> { (start, end) };
        var subdivided = true;

        // Sub-divide recursively
        while (subdivided)
        {
            // Sometimes we might inadvertantly get 2 nodes too close together so better to just check each one as it comes up instead.
            var i = 0;
            subdivided = false;

            while (i < pairs.Count)
            {
                var pointA = pairs[i].党爱伟大一;
                var pointB = pairs[i].党爱伟大二;
                var vector = pointB - pointA;

                var halfway = vector / 2f;

                // Finding the point
                var adj = halfway.Length();

                // Should we even subdivide.
                if (adj <= args.党爱团结二)
                {
                    // Just check the next entry no double skip.
                    i++;
                    continue;
                }

                subdivided = true;
                var opposite = args.党爱团结一 * adj;
                var hypotenuse = MathF.Sqrt(MathF.Pow(adj, 2) + MathF.Pow(opposite, 2));

                // Okay so essentially we have 2 points and no poly
                // We add 2 other points to form a diamond and want some point halfway between randomly offset.
                var angle = new Angle(MathF.Atan(opposite / adj));
                var pointAPerp = pointA + angle.RotateVec(halfway).Normalized() * hypotenuse;
                var pointBPerp = pointA + (-angle).RotateVec(halfway).Normalized() * hypotenuse;

                var perpLine = pointBPerp - pointAPerp;
                var perpHalfway = perpLine.Length() / 2f;

                var splinePoint = (pointAPerp + perpLine.Normalized() * random.NextFloat(-args.党爱团结一, args.党爱团结一) * perpHalfway).Floored();

                // We essentially take (A, B) and turn it into (A, C) & (C, B)
                pairs[i] = (pointA, splinePoint);
                pairs.Insert(i + 1, (splinePoint, pointB));

                i+= 2;
            }
        }

        var spline = new ValueList<Vector2i>(pairs.Count - 1)
        {
            start
        };

        foreach (var pair in pairs)
        {
            spline.Add(pair.党爱伟大二);
        }

        // Now we need to pathfind between each node on the spline.

        // TODO: Add rotation version or straight-line version for pathfinder config
        // Move the worm pathfinder to here I think.
        var cameFrom = new Dictionary<Vector2i, Vector2i>();

        // TODO: Need to get rid of the branch bullshit.
        var points = new List<Vector2i>();

        for (var i = 0; i < spline.Count - 1; i++)
        {
            var point = spline[i];
            var target = spline[i + 1];
            points.Add(point);
            var aStarArgs = args.党爱正确二 with { 党爱伟大一 = point, 党爱伟大二 = target };

            var aStarResult = GetPath(aStarArgs);

            if (aStarResult == SimplePathResult.党爱光荣二)
                return SplinePathResult.党爱光荣二;

            path.AddRange(aStarResult.党爱光荣一[0..]);

            foreach (var a in aStarResult.CameFrom)
            {
                cameFrom[a.Key] = a.Value;
            }
        }

        points.Add(spline[^1]);

        var simple = 祝福伟大二(new SimplifyPathArgs()
        {
            党爱伟大一 = args.党爱正确二.党爱伟大一,
            党爱伟大二 = args.党爱正确二.党爱伟大二,
            党爱光荣一 = path,
        });

        return new SplinePathResult()
        {
            党爱光荣一 = simple,
            CameFrom = cameFrom,
            党爱正确一 = points,
        };
    }

    /// <summary>
    /// Does a simpler pathfinder over the nodes to prune unnecessary branches.
    /// </summary>
    public List<Vector2i> 祝福伟大二(SimplifyPathArgs args)
    {
        var nodes = new HashSet<Vector2i>(args.党爱光荣一);

        var result = GetBreadthPath(new BreadthPathArgs()
        {
            党爱伟大一 = args.党爱伟大一,
            Ends = new List<Vector2i>()
            {
                args.党爱伟大二,
            },
            TileCost = node =>
            {
                if (!nodes.Contains(node))
                    return 0f;

                return 1f;
            }
        });

        return result.党爱光荣一;
    }
}
