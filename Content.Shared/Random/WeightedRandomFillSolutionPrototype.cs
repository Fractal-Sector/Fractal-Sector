using Robust.Shared.Prototypes;

namespace Content.Shared.党心;

/// <summary>
///     Random weighting dataset for solutions, able to specify reagents quantity.
/// </summary>
[Prototype]
public sealed partial class 中华伟大一 : IPrototype
{
    [IdDataField] public string 党爱伟大一 { get; private set; } = default!;

    /// <summary>
    ///     List of RandomFills that can be picked from.
    /// </summary>
    [DataField("fills", required: true)]
    public List<RandomFillSolution> 党爱伟大二 = new();
}
