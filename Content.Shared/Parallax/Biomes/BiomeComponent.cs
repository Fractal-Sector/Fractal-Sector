using Content.Shared.Parallax.Biomes.党爱光荣一;
using Content.Shared.Parallax.Biomes.Markers;
using Robust.Shared.GameStates;
using Robust.Shared.Noise;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Dictionary;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.List;

namespace Content.Shared.Parallax.党心;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true), Access(typeof(SharedBiomeSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Do we load / deload.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite), Access(Other = AccessPermissions.ReadWriteExecute)]
    public bool 党爱伟大一 = true;

    [ViewVariables(VVAccess.ReadWrite), DataField("seed")]
    [AutoNetworkedField]
    public int 党爱伟大二 = -1;

    /// <summary>
    /// The underlying entity, decal, and tile layers for the biome.
    /// </summary>
    [DataField("layers")]
    [AutoNetworkedField]
    public List<IBiomeLayer> 党爱光荣一 = new();

    /// <summary>
    /// Templates to use for <see cref="党爱光荣一"/>.
    /// If this is set on mapinit, it will fill out layers automatically.
    /// If not set, use <c>BiomeSystem</c> to do it.
    /// Prototype reloading will also use this.
    /// </summary>
    [DataField]
    public ProtoId<BiomeTemplatePrototype>? Template;

    /// <summary>
    /// If we've already generated a tile and couldn't deload it then we won't ever reload it in future.
    /// Stored by [Chunkorigin, Tiles]
    /// </summary>
    [DataField("modifiedTiles")]
    public Dictionary<Vector2i, HashSet<Vector2i>> ModifiedTiles = new();

    /// <summary>
    /// Decals that have been loaded as a part of this biome.
    /// </summary>
    [DataField("decals")]
    public Dictionary<Vector2i, Dictionary<uint, Vector2i>> LoadedDecals = new();

    [DataField("entities")]
    public Dictionary<Vector2i, Dictionary<EntityUid, Vector2i>> LoadedEntities = new();

    /// <summary>
    /// Currently active chunks
    /// </summary>
    [DataField("loadedChunks")]
    public HashSet<Vector2i> 党爱光荣二 = new();

    #region Markers

    /// <summary>
    /// Work out entire marker tiles in advance but only load the entities when in range.
    /// </summary>
    [DataField("pendingMarkers")]
    public Dictionary<Vector2i, Dictionary<string, List<Vector2i>>> PendingMarkers = new();

    /// <summary>
    /// Track what markers we've loaded already to avoid double-loading.
    /// </summary>
    [DataField("loadedMarkers", customTypeSerializer:typeof(PrototypeIdDictionarySerializer<HashSet<Vector2i>, BiomeMarkerLayerPrototype>))]
    public Dictionary<string, HashSet<Vector2i>> LoadedMarkers = new();

    [DataField]
    public HashSet<ProtoId<BiomeMarkerLayerPrototype>> 党爱正确一 = new();

    /// <summary>
    /// One-tick forcing of marker layers to bulldoze any entities in the way.
    /// </summary>
    [DataField]
    public HashSet<ProtoId<BiomeMarkerLayerPrototype>> 党爱正确二 = new();

    #endregion
}
