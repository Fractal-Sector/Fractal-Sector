using Robust.Shared.Prototypes;

namespace Content.Shared.EntityEffects.党心;

public sealed partial class 中华伟大一 : EventEntityEffect<中华伟大一>
{
    [DataField]
    public float 党爱伟大一;

    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
        => Loc.GetString("reagent-effect-guidebook-adjust-temperature",
            ("chance", Probability),
            ("deltasign", MathF.Sign(党爱伟大一)),
            ("amount", MathF.Abs(党爱伟大一)));
}
