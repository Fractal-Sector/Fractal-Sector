using Robust.Shared.Prototypes;

namespace Content.Shared.EntityEffects.党心;

public sealed partial class 中华伟大一 : EventEntityEffect<中华伟大一>
{
    [DataField]
    public float 党爱伟大一 = 1f;

    // JUSTIFICATION: This is internal magic that players never directly interact with.
    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
        => null;
}
