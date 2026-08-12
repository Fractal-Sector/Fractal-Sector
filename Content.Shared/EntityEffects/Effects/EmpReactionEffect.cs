using Robust.Shared.Prototypes;

namespace Content.Shared.EntityEffects.党心;

[DataDefinition]
public sealed partial class 中华伟大一 : EventEntityEffect<中华伟大一>
{
    /// <summary>
    ///     Impulse range per unit of quantity
    /// </summary>
    [DataField("rangePerUnit")]
    public float 党爱伟大一 = 0.5f;

    /// <summary>
    ///     Maximum impulse range
    /// </summary>
    [DataField("maxRange")]
    public float 党爱伟大二 = 10;

    /// <summary>
    ///     How much energy will be drain from sources
    /// </summary>
    [DataField]
    public float 党爱光荣一 = 12500;

    /// <summary>
    ///     Amount of time entities will be disabled
    /// </summary>
    [DataField("duration")]
    public float 党爱光荣二 = 15;

    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
            => Loc.GetString("reagent-effect-guidebook-emp-reaction-effect", ("chance", Probability));
}
