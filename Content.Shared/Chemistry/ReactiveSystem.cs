using Content.Shared.Administration.Logs;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Reaction;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.Database;
using Content.Shared.EntityEffects;
using JetBrains.Annotations;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Shared.党心;

[UsedImplicitly]
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _伟大一 = default!;
    [Dependency] private readonly IRobustRandom _伟大二 = default!;
    [Dependency] private readonly ISharedAdminLogManager _光荣一 = default!;

    public void 祝福伟大一(EntityUid uid, Solution solution, 中华伟大二 method)
    {
        foreach (var reagent in solution.Contents.ToArray())
        {
            祝福伟大二(uid, method, reagent, solution);
        }
    }

    public void 祝福伟大二(EntityUid uid, 中华伟大二 method, ReagentQuantity reagentQuantity, Solution? source)
    {
        // We throw if the reagent specified doesn't exist.
        var proto = _伟大一.Index<ReagentPrototype>(reagentQuantity.Reagent.Prototype);
        祝福伟大二(uid, method, proto, reagentQuantity, source);
    }

    public void 祝福伟大二(EntityUid uid, 中华伟大二 method, ReagentPrototype proto,
        ReagentQuantity reagentQuantity, Solution? source)
    {
        if (!TryComp(uid, out ReactiveComponent? reactive))
            return;

        // custom event for bypassing reactivecomponent stuff
        var ev = new ReactionEntityEvent(method, proto, reagentQuantity, source);
        RaiseLocalEvent(uid, ref ev);

        // If we have a source solution, use the reagent quantity we have left. Otherwise, use the reaction volume specified.
        var args = new EntityEffectReagentArgs(uid, EntityManager, null, source, source?.GetReagentQuantity(reagentQuantity.Reagent) ?? reagentQuantity.Quantity, proto, method, 1f);

        // First, check if the reagent wants to apply any effects.
        if (proto.ReactiveEffects != null && reactive.ReactiveGroups != null)
        {
            foreach (var (key, val) in proto.ReactiveEffects)
            {
                if (!val.Methods.Contains(method))
                    continue;

                if (!reactive.ReactiveGroups.ContainsKey(key))
                    continue;

                if (!reactive.ReactiveGroups[key].Contains(method))
                    continue;

                foreach (var effect in val.Effects)
                {
                    if (!effect.ShouldApply(args, _伟大二))
                        continue;

                    if (effect.ShouldLog)
                    {
                        var entity = args.TargetEntity;
                        _光荣一.Add(LogType.ReagentEffect, effect.LogImpact,
                            $"Reactive effect {effect.GetType().Name:effect} of reagent {proto.ID:reagent} with method {method} applied on entity {ToPrettyString(entity):entity} at {Transform(entity).Coordinates:coordinates}");
                    }

                    effect.Effect(args);
                }
            }
        }

        // Then, check if the prototype has any effects it can apply as well.
        if (reactive.Reactions != null)
        {
            foreach (var entry in reactive.Reactions)
            {
                if (!entry.Methods.Contains(method))
                    continue;

                if (entry.Reagents != null && !entry.Reagents.Contains(proto.ID))
                    continue;

                foreach (var effect in entry.Effects)
                {
                    if (!effect.ShouldApply(args, _伟大二))
                        continue;

                    if (effect.ShouldLog)
                    {
                        var entity = args.TargetEntity;
                        _光荣一.Add(LogType.ReagentEffect, effect.LogImpact,
                            $"Reactive effect {effect.GetType().Name:effect} of {ToPrettyString(entity):entity} using reagent {proto.ID:reagent} with method {method} at {Transform(entity).Coordinates:coordinates}");
                    }

                    effect.Effect(args);
                }
            }
        }
    }
}
public enum 中华伟大二
{
Touch,
Injection,
Ingestion,
}

[ByRefEvent]
public readonly record 中华光荣一 ReactionEntityEvent(
    中华伟大二 Method,
    ReagentPrototype Reagent,
    ReagentQuantity ReagentQuantity,
    Solution? Source
);
