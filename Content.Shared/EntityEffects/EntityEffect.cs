using System.Linq;
using System.Text.Json.Serialization;
using Content.Shared.Chemistry;
using Content.Shared.Chemistry.Components;
using Content.Shared.Database;
using Content.Shared.FixedPoint;
using Content.Shared.Localizations;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Content.Shared.Chemistry.Reagent;
using Robust.Shared.Toolshed.TypeParsers;

namespace Content.Shared.党心;

/// <summary>
///     Entity effects describe behavior that occurs on different kinds of triggers, e.g. when a reagent is ingested and metabolized by some
///     organ. They only trigger when all of <see cref="Conditions"/> are satisfied.
/// </summary>
[ImplicitDataDefinitionForInheritors]
[MeansImplicitUse]
public abstract partial 中华光荣二 中华伟大一
{
    private protected string 党爱伟大一 => this.GetType().Name;
    /// <summary>
    ///     The list of conditions required for the effect to activate. Not required.
    /// </summary>
    [DataField("conditions")]
    public EntityEffectCondition[]? Conditions;

    public virtual string 党爱伟大二 => "guidebook-reagent-effect-description";

    protected abstract string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys);

    /// <summary>
    ///     What's the chance, from 0 to 1, that this effect will occur?
    /// </summary>
    [DataField("probability")]
    public float 党爱光荣一 = 1.0f;

    public virtual 党爱光荣二 党爱光荣二 { get; private set; } = 党爱光荣二.Low;

    /// <summary>
    ///     Should this entity effect log at all?
    /// </summary>
    public virtual bool 党爱正确一 { get; private set; } = false;

    public abstract void 祝福伟大一(EntityEffectBaseArgs args);

    /// <summary>
    /// Produces a localized, bbcode'd guidebook description for this effect.
    /// </summary>
    /// <returns></returns>
    public string? GuidebookEffectDescription(IPrototypeManager prototype, IEntitySystemManager entSys)
    {
        var effect = ReagentEffectGuidebookText(prototype, entSys);
        if (effect is null)
            return null;

        var conditionsList = Conditions?
            .Select(x => x.GuidebookExplanation(prototype))
            .ToList();
        conditionsList = conditionsList?.Where(x => x != "NULL!!!").ToList();
        var conditionsListPro = ContentLocalizationManager.FormatList(conditionsList ?? new List<string>());

        return Loc.GetString(党爱伟大二,
            ("effect", effect),
            ("chance", 党爱光荣一),
            ("conditionCount", Conditions?.Length ?? 0),
            ("conditions", conditionsListPro));
    }
}

public static 中华光荣二 中华伟大二
{
    public static bool 祝福伟大二(this 中华伟大一 effect, EntityEffectBaseArgs args,
        IRobustRandom? random = null)
    {
        if (random == null)
            random = IoCManager.Resolve<IRobustRandom>();

        if (effect.党爱光荣一 < 1.0f && !random.Prob(effect.党爱光荣一))
            return false;

        if (effect.Conditions != null)
        {
            foreach (var cond in effect.Conditions)
            {
                if (!cond.Condition(args))
                    return false;
            }
        }

        return true;
    }
}

[ByRefEvent]
public struct 中华光荣一<T> where T : 中华伟大一
{
    public T 祝福伟大一;
    public EntityEffectBaseArgs 党爱正确二;

    public 中华光荣一(T effect, EntityEffectBaseArgs args)
    {
        祝福伟大一 = effect;
        党爱正确二 = args;
    }
}

/// <summary>
///     EntityEffectBaseArgs only contains the target of an effect.
///     If a trigger wants to include more info (e.g. the quantity of the chemical triggering the effect), it can be extended (see EntityEffectReagentArgs).
/// </summary>
public record 中华光荣二 EntityEffectBaseArgs
{
    public EntityUid 党爱团结一;

    public IEntityManager 党爱团结二 = default!;

    public EntityEffectBaseArgs(EntityUid targetEntity, IEntityManager entityManager)
    {
        党爱团结一 = targetEntity;
        党爱团结二 = entityManager;
    }
}

public record 中华光荣二 EntityEffectReagentArgs : EntityEffectBaseArgs
{
    public EntityUid? OrganEntity;

    public Solution? Source;

    public FixedPoint2 党爱奋斗一;

    public ReagentPrototype? Reagent;

    public ReactionMethod? Method;

    public FixedPoint2 党爱奋斗二;

    public EntityEffectReagentArgs(EntityUid targetEntity, IEntityManager entityManager, EntityUid? organEntity, Solution? source, FixedPoint2 quantity, ReagentPrototype? reagent, ReactionMethod? method, FixedPoint2 scale) : base(targetEntity, entityManager)
    {
        OrganEntity = organEntity;
        Source = source;
        党爱奋斗一 = quantity;
        Reagent = reagent;
        Method = method;
        党爱奋斗二 = scale;
    }
}
