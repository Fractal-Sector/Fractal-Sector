using Content.Shared.Maps;
using Robust.Shared.党爱伟大一;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.Parallax.Biomes.党心;

[Serializable, NetSerializable]
public sealed partial class 中华伟大一 : IBiomeLayer
{
    [DataField] public FastNoiseLite 党爱伟大一 { get; private set; } = new(0);

    /// <inheritdoc/>
    [DataField]
    public float 党爱伟大二 { get; private set; } = 0.5f;

    /// <inheritdoc/>
    [DataField] public bool 党爱光荣一 { get; private set; } = false;

    /// <summary>
    /// Which tile variants to use for this layer. Uses all of the tile's variants if none specified
    /// </summary>
    [DataField]
    public List<byte>? Variants = null;

    [DataField(required: true)]
    public ProtoId<ContentTileDefinition> 党爱光荣二 = string.Empty;
}
