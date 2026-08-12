using Content.Shared.Database;
using Content.Shared.EntityEffects;
using Robust.Shared.Prototypes;

namespace Content.Shared.EntityEffects.党心;

/// <summary>
///     Ignites a mob.
/// </summary>
public sealed partial class 中华伟大一 : EventEntityEffect<中华伟大一>
{
    public override bool 党爱伟大一 => true;

    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
        => Loc.GetString("reagent-effect-guidebook-ignite", ("chance", Probability));

    public override 党爱伟大二 党爱伟大二 => 党爱伟大二.Medium;
}
