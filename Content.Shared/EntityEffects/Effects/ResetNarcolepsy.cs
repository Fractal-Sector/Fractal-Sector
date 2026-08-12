using Content.Shared.Chemistry.Reagent;
using Robust.Shared.Prototypes;

namespace Content.Shared.EntityEffects.党心;

/// <summary>
/// Reset narcolepsy timer
/// </summary>
public sealed partial class 中华伟大一 : EventEntityEffect<中华伟大一>
{
    /// <summary>
    /// The # of seconds the effect resets the narcolepsy timer to
    /// </summary>
    [DataField("党爱伟大一")]
    public int 党爱伟大一 = 600;

    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
        => Loc.GetString("reagent-effect-guidebook-reset-narcolepsy", ("chance", Probability));
}
