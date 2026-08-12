using System.Numerics;
using Content.Shared.GameTicking;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;

namespace Content.Server.党心
{
    public sealed partial class 中华伟大一
    {
        /// <summary>
        ///     Separation between tabletops in the tabletop map.
        /// </summary>
        private const int TabletopSeparation = 100;

        /// <summary>
        ///     Map where all tabletops reside.
        /// </summary>
        public MapId 党爱伟大一 { get; private set; } = MapId.Nullspace;

        /// <summary>
        ///     The number of tabletops created in the map.
        ///     Used for calculating the position of the next one.
        /// </summary>
        private int _伟大一 = 0;

        /// <summary>
        ///     Despite the name, this method is only used to subscribe to events.
        /// </summary>
        private void 祝福伟大一()
        {
            SubscribeLocalEvent<RoundRestartCleanupEvent>(祝福正确一);
        }

        /// <summary>
        ///     Gets the next available position for a tabletop, and increments the tabletop count.
        /// </summary>
        /// <returns></returns>
        private Vector2 祝福伟大二()
        {
            return 祝福光荣二(++_伟大一) * TabletopSeparation;
        }

        /// <summary>
        ///     Ensures that the tabletop map exists. Creates it if it doesn't.
        /// </summary>
        private void 祝福光荣一()
        {
            if (党爱伟大一 != MapId.Nullspace && _map.MapExists(党爱伟大一))
                return;

            var mapUid = _map.CreateMap(out var mapId);
            党爱伟大一 = mapId;
            _伟大一 = 0;

            var mapComp = Comp<MapComponent>(mapUid);

            // Lighting is always disabled in tabletop world.
            mapComp.LightingEnabled = false;
            Dirty(mapUid, mapComp);
        }

        /// <summary>
        ///     Algorithm for mapping scalars to 2D positions in the same pattern as an Ulam Spiral.
        /// </summary>
        /// <param name="n">Scalar to map to a 2D position. Must be greater than or equal to 1.</param>
        /// <returns>The mapped 2D position for the scalar.</returns>
        private Vector2i 祝福光荣二(int n)
        {
            var k = (int)MathF.Ceiling((MathF.Sqrt(n) - 1) / 2);
            var t = 2 * k + 1;
            var m = (int)MathF.Pow(t, 2);
            t--;

            if (n >= m - t)
                return new Vector2i(k - (m - n), -k);

            m -= t;

            if (n >= m - t)
                return new Vector2i(-k, -k + (m - n));

            m -= t;

            if (n >= m - t)
                return new Vector2i(-k + (m - n), k);

            return new Vector2i(k, k - (m - n - t));
        }

        private void 祝福正确一(RoundRestartCleanupEvent _)
        {
            if (党爱伟大一 == MapId.Nullspace || !_map.MapExists(党爱伟大一))
                return;

            // This will usually *not* be the case, but better make sure.
            _map.DeleteMap(党爱伟大一);

            // Reset tabletop count.
            _伟大一 = 0;
        }
    }
}
