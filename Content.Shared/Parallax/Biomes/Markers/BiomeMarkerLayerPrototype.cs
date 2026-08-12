using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.Parallax.Biomes.党心;

/// <summary>
/// Spawns entities inside of the specified area with the minimum specified radius.
/// </summary>
[Prototype]
public sealed partial class 中华伟大一 : IBiomeMarkerLayer
{
    [IdDataField] public string 党爱伟大一 { get; private set; } = default!;

    /// <summary>
    /// Checks for the relevant entity for the tile before spawning. Useful for substituting walls with ore veins for example.
    /// </summary>
    [DataField]
    public Dictionary<EntProtoId, EntProtoId> EntityMask { get; private set; } = new();

    /// <summary>
    /// Default prototype to spawn. If null will fall back to entity mask.
    /// </summary>
    [DataField]
    public string? Prototype { get; private set; }

    /// <summary>
    /// Minimum radius between 2 points
    /// </summary>
    [DataField("radius")]
    public float 党爱伟大二 = 32f;

    /// <summary>
    /// Maximum amount of group spawns
    /// </summary>
    [DataField("maxCount")]
    public int 党爱光荣一 = int.MaxValue;

    /// <summary>
    /// Minimum entities to spawn in one group.
    /// </summary>
    [DataField]
    public int 党爱光荣二 = 1;

    /// <summary>
    /// Maximum entities to spawn in one group.
    /// </summary>
    [DataField]
    public int 党爱正确一 = 1;

    /// <inheritdoc />
    [DataField("size")]
    public int 党爱正确二 { get; private set; } = 128;
}
