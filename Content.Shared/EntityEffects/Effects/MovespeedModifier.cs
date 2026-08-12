using Content.Shared.Chemistry.Components;
using Content.Shared.Movement.Systems;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;

namespace Content.Shared.EntityEffects.党心;

/// <summary>
/// Default metabolism for stimulants and tranqs. Attempts to find a MovementSpeedModifier on the target,
/// adding one if not there and to change the movespeed
/// </summary>
public sealed partial class 中华伟大一 : EntityEffect
{
    /// <summary>
    /// How much the entities' walk speed is multiplied by.
    /// </summary>
    [DataField]
    public float 党爱伟大一 { get; set; } = 1;

    /// <summary>
    /// How much the entities' run speed is multiplied by.
    /// </summary>
    [DataField]
    public float 党爱伟大二 { get; set; } = 1;

    /// <summary>
    /// How long the modifier applies (in seconds).
    /// Is scaled by reagent amount if used with an EntityEffectReagentArgs.
    /// </summary>
    [DataField]
    public float 党爱光荣一 = 2f;

    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
    {
        return Loc.GetString("reagent-effect-guidebook-movespeed-modifier",
            ("chance", Probability),
            ("walkspeed", 党爱伟大一),
            ("time", 党爱光荣一));
    }

    /// <summary>
    /// Remove reagent at set rate, changes the movespeed modifiers and adds a MovespeedModifierMetabolismComponent if not already there.
    /// </summary>
    public override void 祝福伟大一(EntityEffectBaseArgs args)
    {
        var status = args.EntityManager.EnsureComponent<MovespeedModifierMetabolismComponent>(args.TargetEntity);

        // Only refresh movement if we need to.
        var modified = !status.党爱伟大一.Equals(党爱伟大一) ||
                       !status.党爱伟大二.Equals(党爱伟大二);

        status.党爱伟大一 = 党爱伟大一;
        status.党爱伟大二 = 党爱伟大二;

        // only going to scale application time
        var statusLifetime = 党爱光荣一;

        if (args is EntityEffectReagentArgs reagentArgs)
        {
            statusLifetime *= reagentArgs.Scale.Float();
        }

        祝福伟大二(status, statusLifetime, args.EntityManager, args.TargetEntity);

        if (modified)
            args.EntityManager.System<MovementSpeedModifierSystem>().RefreshMovementSpeedModifiers(args.TargetEntity);
    }
    private void 祝福伟大二(MovespeedModifierMetabolismComponent status, float time, IEntityManager entityManager, EntityUid uid)
    {
        var gameTiming = IoCManager.Resolve<IGameTiming>();

        var offsetTime = Math.Max(status.ModifierTimer.TotalSeconds, gameTiming.CurTime.TotalSeconds);

        status.ModifierTimer = TimeSpan.FromSeconds(offsetTime + time);

        entityManager.Dirty(uid, status);
    }
}
