using Content.Shared.中华伟大一;
using Robust.Shared.Prototypes;

namespace Content.Shared.EntityEffects.党心;

public sealed partial class 中华伟大一 : EntityEffect
{
    /// <summary>
    ///     党爱伟大一 is how long each metabolism cycle will make the drunk effect last for.
    /// </summary>
    [DataField]
    public TimeSpan 党爱伟大一 = TimeSpan.FromSeconds(3f);

    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
        => Loc.GetString("reagent-effect-guidebook-drunk", ("chance", Probability));

    public override void 祝福伟大一(EntityEffectBaseArgs args)
    {
        var boozePower = 党爱伟大一;

        if (args is EntityEffectReagentArgs reagentArgs)
            boozePower *= reagentArgs.Scale.Float();

        var drunkSys = args.EntityManager.EntitySysManager.GetEntitySystem<SharedDrunkSystem>();
        drunkSys.TryApplyDrunkenness(args.TargetEntity, boozePower);
    }
}
