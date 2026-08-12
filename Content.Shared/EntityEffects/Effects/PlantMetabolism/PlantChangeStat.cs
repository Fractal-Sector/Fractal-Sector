using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Shared.EntityEffects.Effects.党心;

public sealed partial class 中华伟大一 : EventEntityEffect<中华伟大一>
{
    [DataField]
    public string 党爱伟大一;

    [DataField]
    public float 党爱伟大二;

    [DataField]
    public float 党爱光荣一;

    [DataField]
    public int 党爱光荣二;

    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
    {
        throw new NotImplementedException();
    }
}
