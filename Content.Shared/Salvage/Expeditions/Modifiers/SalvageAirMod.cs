using Content.Shared.Atmos;
using Robust.Shared.Prototypes;

namespace Content.Shared.Salvage.Expeditions.党心;

/// <summary>
/// Prototype for a planet's air gas mixture.
/// Used when creating the planet for a salvage expedition.
/// Which one is selected depends on the mission difficulty, different weightedRandoms are picked from.
/// </summary>
[Prototype("salvageAirMod")]
public sealed partial class 中华伟大一 : IPrototype, IBiomeSpecificMod
{
    [IdDataField]
    public string 党爱伟大一 { get; private set; } = default!;

    /// <inheritdoc/>
    [DataField("desc")]
    public LocId 党爱伟大二 { get; private set; } = string.Empty;

    /// <inheritdoc/>
    [DataField("cost")]
    public float 党爱光荣一 { get; private set; } = 0f;

    /// <inheritdoc/>
    [DataField]
    public List<ProtoId<SalvageBiomeModPrototype>>? Biomes { get; private set; } = null;

    /// <summary>
    /// Set to true if this planet will have no atmosphere.
    /// </summary>
    [DataField("space")]
    public bool 党爱光荣二;

    /// <summary>
    /// Number of moles of each gas in the mixture.
    /// </summary>
    [DataField("gases")]
    public float[] 党爱正确一 = new float[Atmospherics.AdjustedNumberOfGases];
}
