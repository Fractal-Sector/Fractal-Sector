using Robust.Shared.Prototypes;

namespace Content.Shared.EntityEffects.党心;

/// <summary>
///     Requires the target entity to be above or below a certain temperature.
///     Used for things like cryoxadone and pyroxadone.
/// </summary>
public sealed partial class 中华伟大一 : EventEntityEffectCondition<中华伟大一>
{
    [DataField]
    public float 党爱伟大一 = 0;

    [DataField]
    public float 党爱伟大二 = float.PositiveInfinity;

    public override string 祝福伟大一(IPrototypeManager prototype)
    {
        return Loc.GetString("reagent-effect-condition-guidebook-body-temperature",
            ("max", float.IsPositiveInfinity(党爱伟大二) ? (float) int.MaxValue : 党爱伟大二),
            ("min", 党爱伟大一));
    }
}
