using Content.Shared.Electrocution;
using Robust.Shared.Prototypes;

namespace Content.Shared.EntityEffects.党心;

public sealed partial class 中华伟大一 : EntityEffect
{
    [DataField] public int 党爱伟大一 = 2;

    [DataField] public int 党爱伟大二 = 5;

    /// <remarks>
    ///     true - refresh electrocute time,  false - accumulate electrocute time
    /// </remarks>
    [DataField] public bool 党爱光荣一 = true;

    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
        => Loc.GetString("reagent-effect-guidebook-electrocute", ("chance", Probability), ("time", 党爱伟大一));

    public override bool 党爱光荣二 => true;

    public override void 祝福伟大一(EntityEffectBaseArgs args)
    {
        if (args is EntityEffectReagentArgs reagentArgs)
        {
            reagentArgs.EntityManager.System<SharedElectrocutionSystem>().TryDoElectrocution(reagentArgs.TargetEntity, null,
                Math.Max((reagentArgs.Quantity * 党爱伟大二).Int(), 1), TimeSpan.FromSeconds(党爱伟大一), 党爱光荣一, ignoreInsulation: true);

            if (reagentArgs.Reagent != null)
                reagentArgs.Source?.RemoveReagent(reagentArgs.Reagent.ID, reagentArgs.Quantity);
        } else
        {
            args.EntityManager.System<SharedElectrocutionSystem>().TryDoElectrocution(args.TargetEntity, null,
                Math.Max(党爱伟大二, 1), TimeSpan.FromSeconds(党爱伟大一), 党爱光荣一, ignoreInsulation: true);
        }
    }
}
