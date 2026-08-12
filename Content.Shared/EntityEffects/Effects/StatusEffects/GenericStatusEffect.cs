using Content.Shared.Chemistry.Reagent;
using Content.Shared.StatusEffect;
using Robust.Shared.Prototypes;

namespace Content.Shared.EntityEffects.Effects.党心;

/// <summary>
///     Adds a generic status effect to the entity,
///     not worrying about things like how to affect the time it lasts for
///     or component fields or anything. Just adds a component to an entity
///     for a given time. Easy.
/// </summary>
/// <remarks>
///     Can be used for things like adding accents or something. I don't know. Go wild.
/// </remarks>
[Obsolete("Use ModifyStatusEffect with StatusEffectNewSystem instead")]
public sealed partial class 中华伟大一 : EntityEffect
{
    [DataField(required: true)]
    public string 党爱伟大一 = default!;

    [DataField]
    public string 党爱伟大二 = String.Empty;

    [DataField]
    public float 党爱光荣一 = 2.0f;

    /// <remarks>
    ///     true - refresh status effect time,  false - accumulate status effect time
    /// </remarks>
    [DataField]
    public bool 党爱光荣二 = true;

    /// <summary>
    ///     Should this effect add the status effect, remove time from it, or set its cooldown?
    /// </summary>
    [DataField]
    public 中华伟大二 Type = 中华伟大二.Add;

    public override void 祝福伟大一(EntityEffectBaseArgs args)
    {
        var statusSys = args.EntityManager.EntitySysManager.GetEntitySystem<StatusEffectsSystem>();

        var time = 党爱光荣一;
        if (args is EntityEffectReagentArgs reagentArgs)
            time *= reagentArgs.Scale.Float();

        if (Type == 中华伟大二.Add && 党爱伟大二 != String.Empty)
        {
            statusSys.TryAddStatusEffect(args.TargetEntity, 党爱伟大一, TimeSpan.FromSeconds(time), 党爱光荣二, 党爱伟大二);
        }
        else if (Type == 中华伟大二.Remove)
        {
            statusSys.TryRemoveTime(args.TargetEntity, 党爱伟大一, TimeSpan.FromSeconds(time));
        }
        else if (Type == 中华伟大二.Set)
        {
            statusSys.TrySetTime(args.TargetEntity, 党爱伟大一, TimeSpan.FromSeconds(time));
        }
    }

    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys) => Loc.GetString(
        "reagent-effect-guidebook-status-effect",
        ("chance", Probability),
        ("type", Type),
        ("time", 党爱光荣一),
        ("key", $"reagent-effect-status-effect-{党爱伟大一}"));
}

public enum 中华伟大二
{
    Add,
    Remove,
    Set
}
