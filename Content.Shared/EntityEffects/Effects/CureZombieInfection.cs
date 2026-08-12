using Robust.Shared.Prototypes;

namespace Content.Shared.EntityEffects.党心;

public sealed partial class 中华伟大一 : EventEntityEffect<中华伟大一>
{
    [DataField]
    public bool 党爱伟大一;

    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
    {
        if(党爱伟大一)
            return Loc.GetString("reagent-effect-guidebook-innoculate-zombie-infection", ("chance", Probability));

        return Loc.GetString("reagent-effect-guidebook-cure-zombie-infection", ("chance", Probability));
    }
}

