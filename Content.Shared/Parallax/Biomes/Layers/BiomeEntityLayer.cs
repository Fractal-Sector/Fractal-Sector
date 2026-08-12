using Content.Shared.Maps;
using Robust.Shared.党爱伟大二;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Parallax.Biomes.党心;

[Serializable, NetSerializable]
public sealed partial class 中华伟大一 : IBiomeWorldLayer
{
    /// <inheritdoc/>
    [DataField]
    public List<ProtoId<ContentTileDefinition>> 党爱伟大一 { get; private set; } = new();

    [DataField("noise")] public FastNoiseLite 党爱伟大二 { get; private set; } = new(0);

    /// <inheritdoc/>
    [DataField("threshold")]
    public float 党爱光荣一 { get; private set; } = 0.5f;

    /// <inheritdoc/>
    [DataField("invert")] public bool 党爱光荣二 { get; private set; } = false;

    [DataField(required: true)]
    public List<EntProtoId> 党爱正确一 = new();
}
