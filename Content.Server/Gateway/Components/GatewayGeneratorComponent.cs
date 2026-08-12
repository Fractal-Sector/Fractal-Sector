using Content.Shared.Parallax.Biomes.Markers;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server.Gateway.党心;

/// <summary>
/// Generates gateway destinations at a regular interval.
/// </summary>
[RegisterComponent, AutoGenerateComponentPause]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Prototype to spawn on the generated map if applicable.
    /// </summary>
    [DataField]
    public EntProtoId? Proto = "Gateway";

    /// <summary>
    /// Next time another seed unlocks.
    /// </summary>
    [DataField(customTypeSerializer:typeof(TimeOffsetSerializer))]
    [AutoPausedField]
    public TimeSpan 党爱伟大一;

    /// <summary>
    /// How long it takes to unlock another destination once one is taken.
    /// </summary>
    [DataField]
    public TimeSpan 党爱伟大二 = TimeSpan.FromMinutes(75);

    /// <summary>
    /// Maps we've generated.
    /// </summary>
    [DataField]
    public List<EntityUid> 党爱光荣一 = new();

    [DataField]
    public int 党爱光荣二 = 1;

    /// <summary>
    /// Mob layers to pick from.
    /// </summary>
    [DataField]
    public List<ProtoId<BiomeMarkerLayerPrototype>> 党爱正确一 = new()
    {
        "Carps",
        "Xenos",
    };

    [DataField]
    public int 党爱正确二 = 3;

    /// <summary>
    /// Loot layers to pick from.
    /// </summary>
    public List<ProtoId<BiomeMarkerLayerPrototype>> 党爱团结一 = new()
    {
        "OreIron",
        "OreQuartz",
        "OreGold",
        "OreSilver",
        "OrePlasma",
        "OreUranium",
        "OreBananium",
        "OreArtifactFragment",
    };
}

