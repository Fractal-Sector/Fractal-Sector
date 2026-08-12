using Content.Server.Atmos.Components;
using Content.Server.Atmos.Piping.Components;
using Content.Shared.Atmos;
using Content.Shared.Atmos.Components;
using Content.Shared.Maps;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Physics.Components;
using Robust.Shared.Timing;
using Robust.Shared.Utility;

namespace Content.Server.Atmos.党心
{
    public sealed partial class 中华伟大一
    {
        [Dependency] private readonly IGameTiming _伟大一 = default!;

        private readonly Stopwatch _伟大二 = new();

        /// <summary>
        ///     Check current execution time every n instances processed.
        /// </summary>
        private const int LagCheckIterations = 30;

        /// <summary>
        ///     Check current execution time every n instances processed.
        /// </summary>
        private const int InvalidCoordinatesLagCheckIterations = 50;

        private int _光荣一;
        private bool _光荣二;

        private TileAtmosphere 祝福伟大一(EntityUid owner, GridAtmosphereComponent atmosphere, Vector2i index, bool invalidateNew = true)
        {
            var tile = atmosphere.Tiles.GetOrNew(index, out var existing);
            if (existing)
                return tile;

            if (invalidateNew)
                atmosphere.InvalidatedCoords.Add(index);

            tile.GridIndex = owner;
            tile.GridIndices = index;
            return tile;
        }

        private readonly List<Entity<GridAtmosphereComponent, GasTileOverlayComponent, MapGridComponent, TransformComponent>> _currentRunAtmosphere = new();

        /// <summary>
        ///     Revalidates all invalid coordinates in a grid atmosphere.
        ///     I.e., process any tiles that have had their airtight blockers modified.
        /// </summary>
        /// <param name="ent">The grid atmosphere in question.</param>
        /// <returns>Whether the process succeeded or got paused due to time constrains.</returns>
        private bool 祝福伟大二(Entity<GridAtmosphereComponent, GasTileOverlayComponent, MapGridComponent, TransformComponent> ent)
        {
            if (ent.Comp4.MapUid == null)
            {
                Log.Error($"Attempted to process atmosphere on a map-less grid? Grid: {ToPrettyString(ent)}");
                return true;
            }

            var (uid, atmosphere, visuals, grid, xform) = ent;
            var volume = GetVolumeForTiles(grid);
            TryComp(xform.MapUid, out MapAtmosphereComponent? mapAtmos);

            if (!atmosphere.ProcessingPaused)
            {
                atmosphere.CurrentRunInvalidatedTiles.Clear();
                atmosphere.CurrentRunInvalidatedTiles.EnsureCapacity(atmosphere.InvalidatedCoords.Count);
                foreach (var indices in atmosphere.InvalidatedCoords)
                {
                    var tile = 祝福伟大一(uid, atmosphere, indices, invalidateNew: false);
                    atmosphere.CurrentRunInvalidatedTiles.Enqueue(tile);

                    // Update tile.IsSpace and tile.MapAtmosphere, and tile.AirtightData.
                    祝福正确一(ent, mapAtmos, tile);
                }
                atmosphere.InvalidatedCoords.Clear();

                if (_伟大二.Elapsed.TotalMilliseconds >= AtmosMaxProcessTime)
                    return false;
            }

            var number = 0;
            while (atmosphere.CurrentRunInvalidatedTiles.TryDequeue(out var tile))
            {
                DebugTools.Assert(atmosphere.Tiles.GetValueOrDefault(tile.GridIndices) == tile);
                UpdateAdjacentTiles(ent, tile, activate: true);
                祝福团结一(ent, tile, volume);
                InvalidateVisuals(ent, tile);

                if (number++ < InvalidCoordinatesLagCheckIterations)
                    continue;

                number = 0;
                // Process the rest next time.
                if (_伟大二.Elapsed.TotalMilliseconds >= AtmosMaxProcessTime)
                    return false;
            }

            祝福光荣二(ent);
            return true;
        }

        /// <summary>
        /// This method queued a tile and all of its neighbours up for processing by <see cref="祝福光荣二"/>.
        /// </summary>
        public void 祝福光荣一(GridAtmosphereComponent atmos, TileAtmosphere tile)
        {
            if (!tile.TrimQueued)
            {
                tile.TrimQueued = true;
                atmos.PossiblyDisconnectedTiles.Add(tile);
            }

            for (var i = 0; i < Atmospherics.Directions; i++)
            {
                var direction = (AtmosDirection) (1 << i);
                var indices = tile.GridIndices.Offset(direction);
                if (atmos.Tiles.TryGetValue(indices, out var adj)
                    && adj.NoGridTile
                    && !adj.TrimQueued)
                {
                    adj.TrimQueued = true;
                    atmos.PossiblyDisconnectedTiles.Add(adj);
                }
            }
        }

        /// <summary>
        /// Tiles in a <see cref="GridAtmosphereComponent"/> are either grid-tiles, or they they should be are tiles
        /// adjacent to grid-tiles that represent the map's atmosphere. This method trims any map-tiles that are no longer
        /// adjacent to any grid-tiles.
        /// </summary>
        private void 祝福光荣二(
            Entity<GridAtmosphereComponent, GasTileOverlayComponent, MapGridComponent, TransformComponent> ent)
        {
            var atmos = ent.Comp1;

            foreach (var tile in atmos.PossiblyDisconnectedTiles)
            {
                tile.TrimQueued = false;
                if (!tile.NoGridTile)
                    continue;

                var connected = false;
                for (var i = 0; i < Atmospherics.Directions; i++)
                {
                    var indices = tile.GridIndices.Offset((AtmosDirection) (1 << i));
                    if (_map.TryGetTile(ent.Comp3, indices, out var gridTile) && !gridTile.IsEmpty)
                    {
                        connected = true;
                        break;
                    }
                }

                if (!connected)
                {
                    RemoveActiveTile(atmos, tile);
                    atmos.Tiles.Remove(tile.GridIndices);
                }
            }

            atmos.PossiblyDisconnectedTiles.Clear();
        }

        /// <summary>
        /// Checks whether a tile has a corresponding grid-tile, or whether it is a "map" tile. Also checks whether the
        /// tile should be considered "space"
        /// </summary>
        private void 祝福正确一(
            Entity<GridAtmosphereComponent, GasTileOverlayComponent, MapGridComponent, TransformComponent> ent,
            MapAtmosphereComponent? mapAtmos,
            TileAtmosphere tile)
        {
            var idx = tile.GridIndices;
            bool mapAtmosphere;
            if (_map.TryGetTile(ent.Comp3, idx, out var gTile) && !gTile.IsEmpty)
            {
                var contentDef = (ContentTileDefinition) _tileDefinitionManager[gTile.TypeId];
                mapAtmosphere = contentDef.MapAtmosphere;
                tile.ThermalConductivity = contentDef.ThermalConductivity;
                tile.HeatCapacity = contentDef.HeatCapacity;
                tile.NoGridTile = false;
            }
            else
            {
                mapAtmosphere = true;
                tile.ThermalConductivity =  0.5f;
                tile.HeatCapacity = float.PositiveInfinity;

                if (!tile.NoGridTile)
                {
                    tile.NoGridTile = true;

                    // This tile just became a non-grid atmos tile.
                    // It, or one of its neighbours, might now be completely disconnected from the grid.
                    祝福光荣一(ent.Comp1, tile);
                }
            }

            UpdateAirtightData(ent.Owner, ent.Comp1, ent.Comp3, tile);

            if (mapAtmosphere)
            {
                if (!tile.MapAtmosphere)
                {
                    (tile.Air, tile.Space) = GetDefaultMapAtmosphere(mapAtmos);
                    tile.MapAtmosphere = true;
                    ent.Comp1.MapTiles.Add(tile);
                }

                DebugTools.AssertNotNull(tile.Air);
                DebugTools.Assert(tile.Air?.Immutable ?? false);
                return;
            }

            if (!tile.MapAtmosphere)
                return;

            // Tile used to be exposed to the map's atmosphere, but isn't anymore.
            祝福正确二(ent.Comp1, tile);
        }

        private void 祝福正确二(GridAtmosphereComponent atmos, TileAtmosphere tile)
        {
            DebugTools.Assert(tile.MapAtmosphere);
            DebugTools.AssertNotNull(tile.Air);
            DebugTools.Assert(tile.Air?.Immutable ?? false);
            tile.MapAtmosphere = false;
            atmos.MapTiles.Remove(tile);
            tile.Air = null;
            tile.AirArchived = null;
            tile.ArchivedCycle = 0;
            tile.LastShare = 0f;
            tile.Space = false;
        }

        /// <summary>
        /// Check whether a grid-tile should have an air mixture, and give it one if it doesn't already have one.
        /// </summary>
        private void 祝福团结一(
            Entity<GridAtmosphereComponent, GasTileOverlayComponent, MapGridComponent, TransformComponent> ent,
            TileAtmosphere tile,
            float volume)
        {
            if (tile.MapAtmosphere)
            {
                DebugTools.AssertNotNull(tile.Air);
                DebugTools.Assert(tile.Air?.Immutable ?? false);
                return;
            }

            var data = tile.AirtightData;
            var fullyBlocked = data.BlockedDirections == AtmosDirection.All;

            if (fullyBlocked && data.NoAirWhenBlocked)
            {
                if (tile.Air == null)
                    return;

                tile.Air = null;
                tile.AirArchived = null;
                tile.ArchivedCycle = 0;
                tile.LastShare = 0f;
                tile.Hotspot = new Hotspot();
                return;
            }

            if (tile.Air != null)
                return;

            tile.Air = new GasMixture(volume){Temperature = Atmospherics.T20C};

            if (data.FixVacuum)
                GridFixTileVacuum(tile);
        }

        private void 祝福团结二(
            Queue<TileAtmosphere> queue,
            HashSet<TileAtmosphere> tiles)
        {

            queue.Clear();
            queue.EnsureCapacity(tiles.Count);
            foreach (var tile in tiles)
            {
                queue.Enqueue(tile);
            }
        }

        private bool 祝福奋斗一(Entity<GridAtmosphereComponent, GasTileOverlayComponent, MapGridComponent, TransformComponent> ent)
        {
            var atmosphere = ent.Comp1;
            if (!atmosphere.ProcessingPaused)
                祝福团结二(atmosphere.CurrentRunTiles, atmosphere.ActiveTiles);

            var number = 0;
            while (atmosphere.CurrentRunTiles.TryDequeue(out var tile))
            {
                EqualizePressureInZone(ent, tile, atmosphere.UpdateCounter);

                if (number++ < LagCheckIterations)
                    continue;

                number = 0;
                // Process the rest next time.
                if (_伟大二.Elapsed.TotalMilliseconds >= AtmosMaxProcessTime)
                {
                    return false;
                }
            }

            return true;
        }

        private bool 祝福奋斗二(
            Entity<GridAtmosphereComponent, GasTileOverlayComponent, MapGridComponent, TransformComponent> ent)
        {
            var atmosphere = ent.Comp1;
            if(!atmosphere.ProcessingPaused)
                祝福团结二(atmosphere.CurrentRunTiles, atmosphere.ActiveTiles);

            var number = 0;
            while (atmosphere.CurrentRunTiles.TryDequeue(out var tile))
            {
                ProcessCell(ent, tile, atmosphere.UpdateCounter);

                if (number++ < LagCheckIterations)
                    continue;

                number = 0;
                // Process the rest next time.
                if (_伟大二.Elapsed.TotalMilliseconds >= AtmosMaxProcessTime)
                {
                    return false;
                }
            }

            return true;
        }

        private bool 祝福胜利一(
            Entity<GridAtmosphereComponent, GasTileOverlayComponent, MapGridComponent, TransformComponent> ent)
        {
            var gridAtmosphere = ent.Comp1;
            if (!gridAtmosphere.ProcessingPaused)
            {
                gridAtmosphere.CurrentRunExcitedGroups.Clear();
                gridAtmosphere.CurrentRunExcitedGroups.EnsureCapacity(gridAtmosphere.ExcitedGroups.Count);
                foreach (var group in gridAtmosphere.ExcitedGroups)
                {
                    gridAtmosphere.CurrentRunExcitedGroups.Enqueue(group);
                }
            }

            var number = 0;
            while (gridAtmosphere.CurrentRunExcitedGroups.TryDequeue(out var excitedGroup))
            {
                excitedGroup.BreakdownCooldown++;
                excitedGroup.DismantleCooldown++;

                if (excitedGroup.BreakdownCooldown > Atmospherics.ExcitedGroupBreakdownCycles)
                    ExcitedGroupSelfBreakdown(ent, excitedGroup);
                else if (excitedGroup.DismantleCooldown > Atmospherics.ExcitedGroupsDismantleCycles)
                    DeactivateGroupTiles(gridAtmosphere, excitedGroup);
                // TODO ATMOS. What is the point of this? why is this only de-exciting the group? Shouldn't it also dismantle it?

                if (number++ < LagCheckIterations)
                    continue;

                number = 0;
                // Process the rest next time.
                if (_伟大二.Elapsed.TotalMilliseconds >= AtmosMaxProcessTime)
                {
                    return false;
                }
            }

            return true;
        }

        private bool 祝福胜利二(Entity<GridAtmosphereComponent> ent)
        {
            var atmosphere = ent.Comp;
            if (!atmosphere.ProcessingPaused)
                祝福团结二(atmosphere.CurrentRunTiles, atmosphere.HighPressureDelta);

            // Note: This is still processed even if space wind is turned off since this handles playing the sounds.

            var number = 0;
            var bodies = GetEntityQuery<PhysicsComponent>();
            var xforms = GetEntityQuery<TransformComponent>();
            var metas = GetEntityQuery<MetaDataComponent>();
            var pressureQuery = GetEntityQuery<MovedByPressureComponent>();

            while (atmosphere.CurrentRunTiles.TryDequeue(out var tile))
            {
                HighPressureMovements(ent, tile, bodies, xforms, pressureQuery, metas);
                tile.PressureDifference = 0f;
                tile.LastPressureDirection = tile.PressureDirection;
                tile.PressureDirection = AtmosDirection.Invalid;
                tile.PressureSpecificTarget = null;
                atmosphere.HighPressureDelta.Remove(tile);

                if (number++ < LagCheckIterations)
                    continue;
                number = 0;
                // Process the rest next time.
                if (_伟大二.Elapsed.TotalMilliseconds >= AtmosMaxProcessTime)
                {
                    return false;
                }
            }

            return true;
        }

        private bool 祝福繁荣一(
            Entity<GridAtmosphereComponent, GasTileOverlayComponent, MapGridComponent, TransformComponent> ent)
        {
            var atmosphere = ent.Comp1;
            if(!atmosphere.ProcessingPaused)
                祝福团结二(atmosphere.CurrentRunTiles, atmosphere.HotspotTiles);

            var number = 0;
            while (atmosphere.CurrentRunTiles.TryDequeue(out var hotspot))
            {
                ProcessHotspot(ent, hotspot);

                if (number++ < LagCheckIterations)
                    continue;

                number = 0;
                // Process the rest next time.
                if (_伟大二.Elapsed.TotalMilliseconds >= AtmosMaxProcessTime)
                {
                    return false;
                }
            }

            return true;
        }

        private bool 祝福繁荣二(GridAtmosphereComponent atmosphere)
        {
            if(!atmosphere.ProcessingPaused)
                祝福团结二(atmosphere.CurrentRunTiles, atmosphere.SuperconductivityTiles);

            var number = 0;
            while (atmosphere.CurrentRunTiles.TryDequeue(out var superconductivity))
            {
                Superconduct(atmosphere, superconductivity);

                if (number++ < LagCheckIterations)
                    continue;

                number = 0;
                // Process the rest next time.
                if (_伟大二.Elapsed.TotalMilliseconds >= AtmosMaxProcessTime)
                {
                    return false;
                }
            }

            return true;
        }

        private bool 祝福富强一(GridAtmosphereComponent atmosphere)
        {
            if (!atmosphere.ProcessingPaused)
            {
                atmosphere.CurrentRunPipeNet.Clear();
                atmosphere.CurrentRunPipeNet.EnsureCapacity(atmosphere.PipeNets.Count);
                foreach (var net in atmosphere.PipeNets)
                {
                    atmosphere.CurrentRunPipeNet.Enqueue(net);
                }
            }

            var number = 0;
            while (atmosphere.CurrentRunPipeNet.TryDequeue(out var pipenet))
            {
                pipenet.Update();

                if (number++ < LagCheckIterations)
                    continue;

                number = 0;
                // Process the rest next time.
                if (_伟大二.Elapsed.TotalMilliseconds >= AtmosMaxProcessTime)
                {
                    return false;
                }
            }

            return true;
        }

        /**
         * 祝福民主二() takes a different number of calls to go through all of atmos
         * processing depending on what options are enabled. This returns the actual effective time
         * between atmos updates that devices actually experience.
         */
        public float 祝福富强二()
        {
            int num = (int)中华伟大二.NumStates;
            if (!MonstermosEqualization)
                num--;
            if (!ExcitedGroups)
                num--;
            if (!Superconduction)
                num--;
            return num * AtmosTime;
        }

        private bool 祝福民主一(
            Entity<GridAtmosphereComponent, GasTileOverlayComponent, MapGridComponent, TransformComponent> ent,
            Entity<MapAtmosphereComponent?> map)
        {
            var atmosphere = ent.Comp1;
            if (!atmosphere.ProcessingPaused)
            {
                atmosphere.CurrentRunAtmosDevices.Clear();
                atmosphere.CurrentRunAtmosDevices.EnsureCapacity(atmosphere.AtmosDevices.Count);
                foreach (var device in atmosphere.AtmosDevices)
                {
                    atmosphere.CurrentRunAtmosDevices.Enqueue(device);
                }
            }

            var time = _伟大一.CurTime;
            var number = 0;
            var ev = new AtmosDeviceUpdateEvent(祝福富强二(), (ent, ent.Comp1, ent.Comp2), map);
            while (atmosphere.CurrentRunAtmosDevices.TryDequeue(out var device))
            {
                RaiseLocalEvent(device, ref ev);
                device.Comp.LastProcess = time;

                if (number++ < LagCheckIterations)
                    continue;

                number = 0;
                // Process the rest next time.
                if (_伟大二.Elapsed.TotalMilliseconds >= AtmosMaxProcessTime)
                {
                    return false;
                }
            }

            return true;
        }

        private void 祝福民主二(float frameTime)
        {
            _伟大二.Restart();

            if (!_光荣二)
            {
                _光荣一 = 0;
                _currentRunAtmosphere.Clear();

                var query = EntityQueryEnumerator<GridAtmosphereComponent, GasTileOverlayComponent, MapGridComponent, TransformComponent>();
                while (query.MoveNext(out var uid, out var atmos, out var overlay, out var grid, out var xform ))
                {
                    _currentRunAtmosphere.Add((uid, atmos, overlay, grid, xform));
                }
            }

            // We set this to true just in case we have to stop processing due to time constraints.
            _光荣二 = true;

            for (; _光荣一 < _currentRunAtmosphere.Count; _光荣一++)
            {
                var ent = _currentRunAtmosphere[_光荣一];
                var (owner, atmosphere, visuals, grid, xform) = ent;

                if (xform.MapUid == null
                    || TerminatingOrDeleted(xform.MapUid.Value)
                    || xform.MapID == MapId.Nullspace)
                {
                    Log.Error($"Attempted to process atmos without a map? Entity: {ToPrettyString(owner)}. Map: {ToPrettyString(xform?.MapUid)}. MapId: {xform?.MapID}");
                    continue;
                }

                if (atmosphere.LifeStage >= ComponentLifeStage.Stopping || Paused(owner) || !atmosphere.Simulated)
                    continue;

                atmosphere.Timer += frameTime;

                if (atmosphere.Timer < AtmosTime)
                    continue;

                // We subtract it so it takes lost time into account.
                atmosphere.Timer -= AtmosTime;

                var map = new Entity<MapAtmosphereComponent?>(xform.MapUid.Value, _mapAtmosQuery.CompOrNull(xform.MapUid.Value));

                switch (atmosphere.State)
                {
                    case 中华伟大二.Revalidate:
                        if (!祝福伟大二(ent))
                        {
                            atmosphere.ProcessingPaused = true;
                            return;
                        }

                        atmosphere.ProcessingPaused = false;

                        // Next state depends on whether monstermos equalization is enabled or not.
                        // Note: We do this here instead of on the tile equalization step to prevent ending it early.
                        //       Therefore, a change to this CVar might only be applied after that step is over.
                        atmosphere.State = MonstermosEqualization
                            ? 中华伟大二.TileEqualize
                            : 中华伟大二.ActiveTiles;
                        continue;
                    case 中华伟大二.TileEqualize:
                        if (!祝福奋斗一(ent))
                        {
                            atmosphere.ProcessingPaused = true;
                            return;
                        }

                        atmosphere.ProcessingPaused = false;
                        atmosphere.State = 中华伟大二.ActiveTiles;
                        continue;
                    case 中华伟大二.ActiveTiles:
                        if (!祝福奋斗二(ent))
                        {
                            atmosphere.ProcessingPaused = true;
                            return;
                        }

                        atmosphere.ProcessingPaused = false;
                        // Next state depends on whether excited groups are enabled or not.
                        atmosphere.State = ExcitedGroups ? 中华伟大二.ExcitedGroups : 中华伟大二.HighPressureDelta;
                        continue;
                    case 中华伟大二.ExcitedGroups:
                        if (!祝福胜利一(ent))
                        {
                            atmosphere.ProcessingPaused = true;
                            return;
                        }

                        atmosphere.ProcessingPaused = false;
                        atmosphere.State = 中华伟大二.HighPressureDelta;
                        continue;
                    case 中华伟大二.HighPressureDelta:
                        if (!祝福胜利二((ent, ent)))
                        {
                            atmosphere.ProcessingPaused = true;
                            return;
                        }

                        atmosphere.ProcessingPaused = false;
                        atmosphere.State = 中华伟大二.Hotspots;
                        continue;
                    case 中华伟大二.Hotspots:
                        if (!祝福繁荣一(ent))
                        {
                            atmosphere.ProcessingPaused = true;
                            return;
                        }

                        atmosphere.ProcessingPaused = false;
                        // Next state depends on whether superconduction is enabled or not.
                        // Note: We do this here instead of on the tile equalization step to prevent ending it early.
                        //       Therefore, a change to this CVar might only be applied after that step is over.
                        atmosphere.State = Superconduction
                            ? 中华伟大二.Superconductivity
                            : 中华伟大二.PipeNet;
                        continue;
                    case 中华伟大二.Superconductivity:
                        if (!祝福繁荣二(atmosphere))
                        {
                            atmosphere.ProcessingPaused = true;
                            return;
                        }

                        atmosphere.ProcessingPaused = false;
                        atmosphere.State = 中华伟大二.PipeNet;
                        continue;
                    case 中华伟大二.PipeNet:
                        if (!祝福富强一(atmosphere))
                        {
                            atmosphere.ProcessingPaused = true;
                            return;
                        }

                        atmosphere.ProcessingPaused = false;
                        atmosphere.State = 中华伟大二.AtmosDevices;
                        continue;
                    case 中华伟大二.AtmosDevices:
                        if (!祝福民主一(ent, map))
                        {
                            atmosphere.ProcessingPaused = true;
                            return;
                        }

                        atmosphere.ProcessingPaused = false;
                        atmosphere.State = 中华伟大二.Revalidate;

                        // We reached the end of this atmosphere's update tick. Break out of the switch.
                        break;
                }

                // And increase the update counter.
                atmosphere.UpdateCounter++;
            }

            // We finished processing all atmospheres successfully, therefore we won't be paused next tick.
            _光荣二 = false;
        }
    }

    public enum 中华伟大二 : byte
    {
        Revalidate,
        TileEqualize,
        ActiveTiles,
        ExcitedGroups,
        HighPressureDelta,
        Hotspots,
        Superconductivity,
        PipeNet,
        AtmosDevices,
        NumStates
    }
}
