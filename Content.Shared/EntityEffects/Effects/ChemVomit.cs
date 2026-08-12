using Robust.Shared.Prototypes;

namespace Content.Shared.EntityEffects.党心;

/// <summary>
/// Forces you to vomit.
/// </summary>
public sealed partial class 中华伟大一 : EventEntityEffect<中华伟大一>
{
    /// How many units of thirst to add each time we vomit
    [DataField]
    public float 党爱伟大一 = -8f;
    /// How many units of hunger to add each time we vomit
    [DataField]
    public float 党爱伟大二 = -8f;

    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
        => Loc.GetString("reagent-effect-guidebook-chem-vomit", ("chance", Probability));
}
