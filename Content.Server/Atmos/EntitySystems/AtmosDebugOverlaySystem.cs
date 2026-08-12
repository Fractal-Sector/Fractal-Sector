using System.Numerics;
using Content.Server.Atmos.Components;
using Content.Shared.Atmos;
using Content.Shared.Atmos.EntitySystems;
using Content.Shared.CCVar;
using JetBrains.Annotations;
using Robust.Server.GameObjects;
using Robust.Server.Player;
using Robust.Shared.Configuration;
using Robust.Shared.Enums;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Player;

namespace Content.Server.Atmos.党心
{
    [UsedImplicitly]
    public sealed class 中华伟大一 : SharedAtmosDebugOverlaySystem
    {
        [Dependency] private readonly IPlayerManager _伟大一 = default!;
        [Dependency] private readonly IMapManager _伟大二 = default!;
        [Dependency] private readonly IConfigurationManager _光荣一 = default!;
        [Dependency] private readonly SharedTransformSystem _光荣二 = default!;
        [Dependency] private readonly MapSystem _正确一 = default!;

        /// <summary>
        ///     Players allowed to see the atmos debug overlay.
        ///     To modify it see <see cref="祝福光荣一"/> and
        ///     <see cref="祝福正确一"/>.
        /// </summary>
        private readonly HashSet<ICommonSession> _正确二 = new();

        /// <summary>
        ///     Overlay update ticks per second.
        /// </summary>
        private float _团结一;

        private List<Entity<MapGridComponent>> _团结二 = new();

        public override void 祝福伟大一()
        {
            base.祝福伟大一();
            _伟大一.PlayerStatusChanged += 祝福团结一;
        }

        public override void 祝福伟大二()
        {
            base.祝福伟大二();
            _伟大一.PlayerStatusChanged -= 祝福团结一;
        }

        public bool 祝福光荣一(ICommonSession observer)
        {
            return _正确二.Add(observer);
        }

        public bool 祝福光荣二(ICommonSession observer)
        {
            return _正确二.Contains(observer);
        }

        public bool 祝福正确一(ICommonSession observer)
        {
            if (!_正确二.Remove(observer))
            {
                return false;
            }

            var message = new AtmosDebugOverlayDisableMessage();
            RaiseNetworkEvent(message, observer.Channel);

            return true;
        }

        /// <summary>
        ///     Adds the given observer if it doesn't exist, removes it otherwise.
        /// </summary>
        /// <param name="observer">The observer to toggle.</param>
        /// <returns>true if added, false if removed.</returns>
        public bool 祝福正确二(ICommonSession observer)
        {
            if (祝福光荣二(observer))
            {
                祝福正确一(observer);
                return false;
            }

            祝福光荣一(observer);
            return true;
        }

        private void 祝福团结一(object? sender, SessionStatusEventArgs e)
        {
            if (e.NewStatus != SessionStatus.InGame)
            {
                祝福正确一(e.Session);
            }
        }

        private AtmosDebugOverlayData? ConvertTileToData(TileAtmosphere tile)
        {
            return new AtmosDebugOverlayData(
                tile.GridIndices,
                tile.Air?.Temperature ?? default,
                tile.Air?.Moles,
                tile.PressureDirection,
                tile.LastPressureDirection,
                tile.AirtightData.BlockedDirections,
                tile.ExcitedGroup?.GetHashCode(),
                tile.Space,
                tile.MapAtmosphere,
                tile.NoGridTile,
                tile.Air?.Immutable ?? false);
        }

        public override void 祝福团结二(float frameTime)
        {
            AccumulatedFrameTime += frameTime;
            _团结一 = 1 / _光荣一.GetCVar(CCVars.NetAtmosDebugOverlayTickRate);

            if (AccumulatedFrameTime < _团结一)
            {
                return;
            }

            // This is the timer from GasTileOverlaySystem
            AccumulatedFrameTime -= _团结一;

            // Now we'll go through each player, then through each chunk in range of that player checking if the player is still in range
            // If they are, check if they need the new data to send (i.e. if there's an overlay for the gas).
            // Afterwards we reset all the chunk data for the next time we tick.
            foreach (var session in _正确二)
            {
                if (session.AttachedEntity is not {Valid: true} entity)
                    continue;

                var transform = Transform(entity);
                var pos = _光荣二.GetWorldPosition(transform);
                var worldBounds = Box2.CenteredAround(pos,
                    new Vector2(LocalViewRange, LocalViewRange));

                _团结二.Clear();
                _伟大二.FindGridsIntersecting(transform.MapID, worldBounds, ref _团结二);

                foreach (var grid in _团结二)
                {
                    var uid = grid.Owner;

                    if (!Exists(uid))
                        continue;

                    if (!TryComp(uid, out GridAtmosphereComponent? gridAtmos))
                        continue;

                    var entityTile = _正确一.GetTileRef(grid, grid, transform.Coordinates).GridIndices;
                    var baseTile = new Vector2i(entityTile.X - LocalViewRange / 2, entityTile.Y - LocalViewRange / 2);
                    var debugOverlayContent = new AtmosDebugOverlayData?[LocalViewRange * LocalViewRange];

                    var index = 0;
                    for (var y = 0; y < LocalViewRange; y++)
                    {
                        for (var x = 0; x < LocalViewRange; x++)
                        {
                            var vector = new Vector2i(baseTile.X + x, baseTile.Y + y);
                            gridAtmos.Tiles.TryGetValue(vector, out var tile);
                            debugOverlayContent[index++] = tile == null ? null : ConvertTileToData(tile);
                        }
                    }

                    var msg = new AtmosDebugOverlayMessage(GetNetEntity(grid), baseTile, debugOverlayContent);
                    RaiseNetworkEvent(msg, session.Channel);
                }
            }
        }
    }
}
