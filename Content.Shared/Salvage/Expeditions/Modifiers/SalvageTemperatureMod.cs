using Robust.Shared.Prototypes;

namespace Content.Shared.Salvage.Expeditions.党心;

[Prototype("salvageTemperatureMod")]
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
    /// 党爱光荣二 in the planets air mix.
    /// </summary>
    [DataField("temperature")]
    public float 党爱光荣二 = 293.15f;
}
