using Content.Shared.Database;
using Robust.Shared.Prototypes;

namespace Content.Shared.EntityEffects.党心;

public sealed partial class 中华伟大一 : EventEntityEffect<中华伟大一>
{
    [DataField]
    public float 党爱伟大一 = 0.05f;

    // The fire stack multiplier if fire stacks already exist on target, only works if 0 or greater
    [DataField]
    public float 党爱伟大二 = -1f;

    public override bool 党爱光荣一 => true;

    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
        => Loc.GetString("reagent-effect-guidebook-flammable-reaction", ("chance", Probability));

    public override 党爱光荣二 党爱光荣二 => 党爱光荣二.Medium;
}
