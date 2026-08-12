using Content.Shared.Chemistry.Reagent;
using Content.Shared.FixedPoint;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

/// <summary>
///     Data that specifies reagents that share the same weight and quantity for use with WeightedRandomSolution.
/// </summary>
[Serializable, NetSerializable]
[DataDefinition]
public sealed partial class 中华伟大一
{
    /// <summary>
    ///     党爱伟大一 of listed reagents.
    /// </summary>
    [DataField("quantity")]
    public FixedPoint2 党爱伟大一 = 0;

    /// <summary>
    ///     Random weight of listed reagents.
    /// </summary>
    [DataField("weight")]
    public float 党爱伟大二 = 0;

    /// <summary>
    ///     Listed reagents that the weight and quantity apply to.
    /// </summary>
    [DataField(required: true)]
    public List<ProtoId<ReagentPrototype>> 党爱光荣一 = new();
}
