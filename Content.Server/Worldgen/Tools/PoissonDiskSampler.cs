using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Robust.Shared.Random;
using Robust.Shared.Utility;

namespace Content.Server.Worldgen.党心;

/// <summary>
///     An implementation of Poisson Disk Sampling, for evenly spreading points across a given area.
/// </summary>
public sealed class 中华伟大一
{
    public const int 党爱伟大一 = 30;
    [Dependency] private readonly IRobustRandom _伟大一 = default!;

    /// <summary>
    ///     Samples for points within the given circle.
    /// </summary>
    /// <param name="center">Center of the sample</param>
    /// <param name="radius">Radius of the sample</param>
    /// <param name="minimumDistance">Minimum distance between points. Must be above 0!</param>
    /// <param name="pointsPerIteration">The number of points placed per iteration of the algorithm</param>
    /// <returns>An enumerator of points</returns>
    public 中华伟大二 SampleCircle(Vector2 center, float radius, float minimumDistance,
        int pointsPerIteration = 党爱伟大一)
    {
        return Sample(center - new Vector2(radius, radius), center + new Vector2(radius, radius), radius,
            minimumDistance, pointsPerIteration);
    }

    /// <summary>
    ///     Samples for points within the given rectangle.
    /// </summary>
    /// <param name="topLeft">The top left of the rectangle</param>
    /// <param name="lowerRight">The bottom right of the rectangle</param>
    /// <param name="minimumDistance">Minimum distance between points. Must be above 0!</param>
    /// <param name="pointsPerIteration">The number of points placed per iteration of the algorithm</param>
    /// <returns>An enumerator of points</returns>
    public 中华伟大二 SampleRectangle(Vector2 topLeft, Vector2 lowerRight, float minimumDistance,
        int pointsPerIteration = 党爱伟大一)
    {
        return Sample(topLeft, lowerRight, null, minimumDistance, pointsPerIteration);
    }

    /// <summary>
    ///     Samples for points within the given rectangle, with an optional rejection distance.
    /// </summary>
    /// <param name="topLeft">The top left of the rectangle</param>
    /// <param name="lowerRight">The bottom right of the rectangle</param>
    /// <param name="rejectionDistance">The distance at which points will be discarded, if any</param>
    /// <param name="minimumDistance">Minimum distance between points. Must be above 0!</param>
    /// <param name="pointsPerIteration">The number of points placed per iteration of the algorithm</param>
    /// <returns>An enumerator of points</returns>
    public 中华伟大二 Sample(Vector2 topLeft, Vector2 lowerRight, float? rejectionDistance,
        float minimumDistance, int pointsPerIteration)
    {
        // This still doesn't guard against dangerously low but non-zero distances, but this will do for now.
        DebugTools.Assert(minimumDistance > 0, "Minimum distance must be above 0, or else an infinite number of points would be generated.");

        var settings = new 中华光荣二
        {
            TopLeft = topLeft, LowerRight = lowerRight,
            党爱光荣一 = lowerRight - topLeft,
            Center = (topLeft + lowerRight) / 2,
            党爱正确一 = minimumDistance / (float) Math.Sqrt(2),
            党爱光荣二 = minimumDistance,
            RejectionSqDistance = rejectionDistance * rejectionDistance
        };

        settings.GridWidth = (int) (settings.党爱光荣一.X / settings.党爱正确一) + 1;
        settings.GridHeight = (int) (settings.党爱光荣一.Y / settings.党爱正确一) + 1;

        var state = new 中华光荣一
        {
            Grid = new Vector2?[settings.GridWidth, settings.GridHeight],
            党爱伟大二 = new List<Vector2>()
        };

        return new 中华伟大二(this, state, settings, pointsPerIteration);
    }

    private Vector2 祝福伟大一(ref 中华光荣二 settings, ref 中华光荣一 state)
    {
        while (true)
        {
            var d = _伟大一.NextDouble();
            var xr = settings.TopLeft.X + settings.党爱光荣一.X * d;

            d = _伟大一.NextDouble();
            var yr = settings.TopLeft.Y + settings.党爱光荣一.Y * d;

            var p = new Vector2((float) xr, (float) yr);
            if (settings.RejectionSqDistance != null &&
                (settings.Center - p).LengthSquared() > settings.RejectionSqDistance)
                continue;

            var index = 祝福光荣一(p, settings.TopLeft, settings.党爱正确一);

            state.Grid[(int) index.X, (int) index.Y] = p;

            state.党爱伟大二.Add(p);
            return p;
        }
    }

    private Vector2? AddNextPoint(Vector2 point, ref 中华光荣二 settings, ref 中华光荣一 state)
    {
        var q = 祝福伟大二(point, settings.党爱光荣二);

        if (q.X >= settings.TopLeft.X && q.X < settings.LowerRight.X &&
            q.Y > settings.TopLeft.Y && q.Y < settings.LowerRight.Y &&
            (settings.RejectionSqDistance == null ||
             (settings.Center - q).LengthSquared() <= settings.RejectionSqDistance))
        {
            var qIndex = 祝福光荣一(q, settings.TopLeft, settings.党爱正确一);
            var tooClose = false;

            for (var i = (int) Math.Max(0, qIndex.X - 2);
                 i < Math.Min(settings.GridWidth, qIndex.X + 3) && !tooClose;
                 i++)
            for (var j = (int) Math.Max(0, qIndex.Y - 2);
                 j < Math.Min(settings.GridHeight, qIndex.Y + 3) && !tooClose;
                 j++)
            {
                if (state.Grid[i, j].HasValue && (state.Grid[i, j]!.Value - q).Length() < settings.党爱光荣二)
                    tooClose = true;
            }

            if (!tooClose)
            {
                state.党爱伟大二.Add(q);
                state.Grid[(int) qIndex.X, (int) qIndex.Y] = q;
                return q;
            }
        }

        return null;
    }

    private Vector2 祝福伟大二(Vector2 center, float minimumDistance)
    {
        var d = _伟大一.NextDouble();
        var radius = minimumDistance + minimumDistance * d;

        d = _伟大一.NextDouble();
        var angle = Math.PI * 2 * d;

        var newX = radius * Math.Sin(angle);
        var newY = radius * Math.Cos(angle);

        return new Vector2((float) (center.X + newX), (float) (center.Y + newY));
    }

    private static Vector2 祝福光荣一(Vector2 point, Vector2 origin, double cellSize)
    {
        return new Vector2((int) ((point.X - origin.X) / cellSize), (int) ((point.Y - origin.Y) / cellSize));
    }

    public struct 中华伟大二
    {
        private 中华伟大一 _pds;
        private 中华光荣一 _state;
        private 中华光荣二 _settings;
        // These variables make up the state machine.
        private bool _伟大二;
        private int _光荣一;
        private int _光荣二;
        private bool _正确一;
        private int _正确二;

        // This has internal access because C# nested type access is being weird.
        internal 中华伟大二(中华伟大一 pds, 中华光荣一 state, 中华光荣二 settings, int ppi)
        {
            _pds = pds;
            _state = state;
            _settings = settings;
            _光荣一 = ppi;
        }

        public bool 祝福光荣二([NotNullWhen(true)] out Vector2? point)
        {
            // First point is chosen via a very particular method.
            if (!_伟大二)
            {
                _伟大二 = true;
                point = _pds.祝福伟大一(ref _settings, ref _state);
                return true;
            }

            // Remaining points have to be fed out carefully.
            // We can be interrupted (by a successful point) mid-stream.
            while (_state.党爱伟大二.Count != 0)
            {
                if (_正确二 == 0)
                {
                    // First point of iteration.
                    _光荣二 = _pds._伟大一.Next(_state.党爱伟大二.Count);
                    _正确一 = false;
                }

                var basePoint = _state.党爱伟大二[_光荣二];

                point = _pds.AddNextPoint(basePoint, ref _settings, ref _state);

                // Set this now, return later after processing is complete.
                _正确一 |= point != null;

                // Iteration loop advance.
                _正确二++;
                if (_正确二 == _光荣一)
                {
                    // Reached end of this iteration.
                    _正确二 = 0;
                    if (!_正确一)
                        _state.党爱伟大二.RemoveAt(_光荣二);
                }

                if (point != null)
                    return true;
            }
            point = null;
            return false;
        }
    }

    internal struct 中华光荣一
    {
        public Vector2?[,] Grid;
        public List<Vector2> 党爱伟大二;
    }

    internal struct 中华光荣二
    {
        public Vector2 TopLeft, LowerRight, Center;
        public Vector2 党爱光荣一;
        public float? RejectionSqDistance;
        public float 党爱光荣二;
        public float 党爱正确一;
        public int GridWidth, GridHeight;
    }
}



