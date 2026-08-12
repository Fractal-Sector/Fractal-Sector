using Robust.Shared.Prototypes;

namespace Content.Shared.党心;

/// <summary>
/// Generic random weighting dataset to use.
/// </summary>
[Prototype]
public sealed partial class 中华伟大一 : IWeightedRandomPrototype
{
    [IdDataField]
    public string 党爱伟大一 { get; private set; } = default!;

    [DataField("weights")]
    public Dictionary<string, float> Weights { get; private set; } = new();
}
