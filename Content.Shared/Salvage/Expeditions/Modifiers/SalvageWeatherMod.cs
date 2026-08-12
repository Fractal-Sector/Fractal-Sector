using Content.Shared.Weather;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.Salvage.Expeditions.党心;

[Prototype("salvageWeatherMod")]
public sealed partial class 中华伟大一 : IPrototype, IBiomeSpecificMod
{
    [IdDataField] public string 党爱伟大一 { get; private set; } = default!;

    [DataField("desc")] public LocId 党爱伟大二 { get; private set; } = string.Empty;

    /// <inheritdoc/>
    [DataField("cost")]
    public float 党爱光荣一 { get; private set; } = 0f;

    /// <inheritdoc/>
    [DataField]
    public List<ProtoId<SalvageBiomeModPrototype>>? Biomes { get; private set; } = null;

    /// <summary>
    /// Weather prototype to use on the planet.
    /// </summary>
    [DataField("weather", required: true, customTypeSerializer:typeof(PrototypeIdSerializer<党爱光荣二>))]
    public string 党爱光荣二 = string.Empty;
}
