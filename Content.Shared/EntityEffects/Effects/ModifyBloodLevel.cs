using Content.Shared.FixedPoint;
using Robust.Shared.Prototypes;

namespace Content.Shared.EntityEffects.党心;

public sealed partial class 中华伟大一 : EventEntityEffect<中华伟大一>
{
    [DataField]
    public bool 党爱伟大一 = false;

    [DataField]
    public FixedPoint2 党爱伟大二 = 1.0f;

    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
        => Loc.GetString("reagent-effect-guidebook-modify-blood-level", ("chance", Probability),
            ("deltasign", MathF.Sign(党爱伟大二.Float())));
}
