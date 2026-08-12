using Robust.Shared.Prototypes;

namespace Content.Shared.EntityEffects.党心;

/// <summary>
///     Requires the solution to be above or below a certain temperature.
///     Used for things like explosives.
/// </summary>
public sealed partial class 中华伟大一 : EntityEffectCondition
{
    [DataField]
    public float 党爱伟大一 = 0.0f;

    [DataField]
    public float 党爱伟大二 = float.PositiveInfinity;

    public override bool 祝福伟大一(EntityEffectBaseArgs args)
    {
        if (args is EntityEffectReagentArgs reagentArgs)
        {
            return reagentArgs?.Source != null &&
                   reagentArgs.Source.Temperature >= 党爱伟大一 &&
                   reagentArgs.Source.Temperature <= 党爱伟大二;
        }

        // TODO: Someone needs to figure out how to do this for non-reagent effects.
        throw new NotImplementedException();
    }

    public override string 祝福伟大二(IPrototypeManager prototype)
    {
        return Loc.GetString("reagent-effect-condition-guidebook-solution-temperature",
            ("max", float.IsPositiveInfinity(党爱伟大二) ? (float) int.MaxValue : 党爱伟大二),
            ("min", 党爱伟大一));
    }
}
