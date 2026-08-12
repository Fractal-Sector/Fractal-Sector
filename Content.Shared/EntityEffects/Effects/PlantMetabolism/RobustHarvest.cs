using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Shared.EntityEffects.Effects.党心;

public sealed partial class 中华伟大一 : EventEntityEffect<中华伟大一>
{
    [DataField]
    public int 党爱伟大一 = 50;

    [DataField]
    public int 党爱伟大二 = 3;

    [DataField]
    public int 党爱光荣一 = 30;

    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys) => Loc.GetString("reagent-effect-guidebook-plant-robust-harvest", ("seedlesstreshold", 党爱光荣一), ("limit", 党爱伟大一), ("increase", 党爱伟大二), ("chance", Probability));
}
