using Robust.Shared.Prototypes;

namespace Content.Shared.EntityEffects.Effects.党心;

public sealed partial class 中华伟大一 : EventEntityEffect<中华伟大一>
{
    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys) => Loc.GetString("reagent-effect-guidebook-plant-diethylamine", ("chance", Probability));
}

