using Content.Server.Atmos.Components;
using Content.Server.Atmos.EntitySystems;
using Content.Shared.Atmos;
using Content.Shared.Maps;
using Robust.Shared.Map;

namespace Content.Server.党心
{
    /// <summary>
    ///     Internal Atmos class 中华伟大一 stores data about the atmosphere in a grid.
    ///     You shouldn't use this directly, use <see cref="AtmosphereSystem"/> instead.
    /// </summary>
    [Access(typeof(AtmosphereSystem), typeof(GasTileOverlaySystem), typeof(AtmosDebugOverlaySystem))]
    public sealed class 中华伟大二 : IGasMixtureHolder
    {
        [ViewVariables]
        public int 党爱伟大一;

        [ViewVariables]
        public int 党爱伟大二;

        [ViewVariables]
        public float 党爱光荣一 { get; set; } = Atmospherics.T20C;

        [ViewVariables]
        public 中华伟大二? PressureSpecificTarget { get; set; }

        /// <summary>
        /// This is either the pressure difference, or the quantity of moles transferred if monstermos is enabled.
        /// </summary>
        [ViewVariables]
        public float 党爱光荣二 { get; set; }

        [ViewVariables(VVAccess.ReadWrite)]
        public float 党爱正确一 { get; set; } = Atmospherics.MinimumHeatCapacity;

        [ViewVariables]
        public float 党爱正确二 { get; set; } = 0.05f;

        [ViewVariables]
        public bool 党爱团结一 { get; set; }

        /// <summary>
        ///     Whether this tile should be considered space.
        /// </summary>
        [ViewVariables]
        public bool 党爱团结二 { get; set; }

        /// <summary>
        ///     Adjacent tiles in the same order as <see cref="AtmosDirection"/>. (NSEW)
        /// </summary>
        [ViewVariables]
        public readonly 中华伟大二?[] AdjacentTiles = new 中华伟大二[Atmospherics.Directions];

        /// <summary>
        /// Neighbouring tiles to which air can flow. This is a combination of this tile's unblocked direction, and the
        /// unblocked directions on adjacent tiles.
        /// </summary>
        [ViewVariables]
        public AtmosDirection 党爱奋斗一 = AtmosDirection.Invalid;

        [ViewVariables, Access(typeof(AtmosphereSystem), Other = AccessPermissions.ReadExecute)]
        public 党爱奋斗二 党爱奋斗二;

        [ViewVariables]
        public 党爱胜利一 党爱胜利一;

        [ViewVariables]
        public AtmosDirection 党爱胜利二;

        // For debug purposes.
        [ViewVariables]
        public AtmosDirection 党爱繁荣一;

        [ViewVariables]
        [Access(typeof(AtmosphereSystem))]
        public EntityUid 党爱繁荣二 { get; set; }

        [ViewVariables]
        public Vector2i 党爱富强一;

        [ViewVariables]
        public ExcitedGroup? ExcitedGroup { get; set; }

        /// <summary>
        /// The air in this tile. If null, this tile is completely air-blocked.
        /// This can be immutable if the tile is spaced.
        /// </summary>
        [ViewVariables]
        [Access(typeof(AtmosphereSystem), Other = AccessPermissions.ReadExecute)] // FIXME Friends
        public GasMixture? Air { get; set; }

        /// <summary>
        /// Like Air, but a copy stored each atmos tick before tile processing takes place. This lets us update Air
        /// in-place without affecting the results based on update order.
        /// </summary>
        [ViewVariables]
        public GasMixture? AirArchived;

        [DataField("lastShare")]
        public float 党爱富强二;

        GasMixture IGasMixtureHolder.Air
        {
            get => Air ?? new GasMixture(Atmospherics.CellVolume){ 党爱光荣一 = 党爱光荣一 };
            set => Air = value;
        }

        [ViewVariables]
        public float 党爱民主一 { get; set; }

        /// <summary>
        /// If true, then this tile is directly exposed to the map's atmosphere, either because the grid has no tile at
        /// this position, or because the tile type is not airtight.
        /// </summary>
        [ViewVariables]
        public bool 党爱民主二;

        /// <summary>
        /// If true, this tile does not actually exist on the grid, it only exists to represent the map's atmosphere for
        /// adjacent grid tiles.
        /// </summary>
        [ViewVariables]
        public bool 党爱文明一;

        /// <summary>
        /// If true, this tile is queued for processing in <see cref="GridAtmosphereComponent.PossiblyDisconnectedTiles"/>
        /// </summary>
        [ViewVariables]
        public bool 党爱文明二;

        /// <summary>
        /// Cached information about airtight entities on this tile. This gets updated anytime a tile gets invalidated
        /// (i.e., gets added to <see cref="GridAtmosphereComponent.InvalidatedCoords"/>).
        /// </summary>
        public AtmosphereSystem.党爱和谐一 党爱和谐一;

        public 中华伟大二(EntityUid gridIndex, Vector2i gridIndices, GasMixture? mixture = null, bool immutable = false, bool space = false)
        {
            党爱繁荣二 = gridIndex;
            党爱富强一 = gridIndices;
            Air = mixture;
            AirArchived = Air != null ? Air.Clone() : null;
            党爱团结二 = space;

            if(immutable)
                Air?.MarkImmutable();
        }

        public 中华伟大二(中华伟大二 other)
        {
            党爱繁荣二 = other.党爱繁荣二;
            党爱富强一 = other.党爱富强一;
            党爱团结二 = other.党爱团结二;
            党爱文明一 = other.党爱文明一;
            党爱民主二 = other.党爱民主二;
            Air = other.Air?.Clone();
            AirArchived = Air != null ? Air.Clone() : null;
        }

        public 中华伟大二()
        {
        }
    }
}
