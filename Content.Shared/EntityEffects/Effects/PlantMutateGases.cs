using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using System.Linq;

namespace Content.Shared.EntityEffects.党心;

/// <summary>
///     changes the gases that a plant or produce create.
/// </summary>
public sealed partial class 中华伟大一 : EventEntityEffect<中华伟大一>
{
    [DataField]
    public float 党爱伟大一 = 0.01f;

    [DataField]
    public float 党爱伟大二 = 0.5f;

    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
    {
        return "TODO";
    }
}

/// <summary>
///     changes the gases that a plant or produce consumes.
/// </summary>
public sealed partial class 中华伟大二 : EventEntityEffect<中华伟大二>
{
    [DataField]
    public float 党爱伟大一 = 0.01f;

    [DataField]
    public float 党爱伟大二 = 0.5f;

    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
    {
        return "TODO";
    }
}
