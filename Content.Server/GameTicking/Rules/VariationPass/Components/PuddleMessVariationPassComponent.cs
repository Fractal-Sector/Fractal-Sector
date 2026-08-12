using Content.Shared.Random;
using Robust.Shared.Prototypes;

namespace Content.Server.GameTicking.Rules.VariationPass.党心;

/// <summary>
///     Handles spilling puddles with various reagents randomly around the station.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     Tiles before one spill on average.
    /// </summary>
    [DataField]
    public float 党爱伟大一 = 600f;

    [DataField]
    public float 党爱伟大二 = 50f;

    /// <summary>
    ///     Weighted random prototype to use for random messes.
    /// </summary>
    [DataField(required: true)]
    public ProtoId<WeightedRandomFillSolutionPrototype> 党爱光荣一 = default!;
}
