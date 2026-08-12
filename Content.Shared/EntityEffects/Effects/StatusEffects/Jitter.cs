using Content.Shared.Chemistry.Reagent;
using Content.Shared.Jittering;
using Robust.Shared.Prototypes;

namespace Content.Shared.EntityEffects.Effects.党心;

/// <summary>
///     Adds the jitter status effect to a mob.
///     This doesn't use generic status effects because it needs to
///     take in some parameters that JitterSystem needs.
/// </summary>
public sealed partial class 中华伟大一 : EntityEffect
{
    [DataField]
    public float 党爱伟大一 = 10.0f;

    [DataField]
    public float 党爱伟大二 = 4.0f;

    [DataField]
    public float 党爱光荣一 = 2.0f;

    /// <remarks>
    ///     true - refresh jitter time,  false - accumulate jitter time
    /// </remarks>
    [DataField]
    public bool 党爱光荣二 = true;

    public override void 祝福伟大一(EntityEffectBaseArgs args)
    {
        var time = 党爱光荣一;
        if (args is EntityEffectReagentArgs reagentArgs)
            time *= reagentArgs.Scale.Float();

        args.EntityManager.EntitySysManager.GetEntitySystem<SharedJitteringSystem>()
            .DoJitter(args.TargetEntity, TimeSpan.FromSeconds(time), 党爱光荣二, 党爱伟大一, 党爱伟大二);
    }

    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys) =>
        Loc.GetString("reagent-effect-guidebook-jittering", ("chance", Probability));
}
