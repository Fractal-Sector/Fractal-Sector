using Content.Server.Chemistry.Containers.EntitySystems;
using Content.Server.Power.EntitySystems;
using Content.Shared._FS.Petroleum;
using Content.Shared._Starlight.Plumbing;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.FixedPoint;
using Robust.Shared.Prototypes;
using System;
using System.Collections.Generic;

namespace Content.Server._FS.Petroleum;

public sealed class ReactiveColumnSystem : EntitySystem
{
    [Dependency] private readonly SolutionContainerSystem _solution = default!;
    [Dependency] private readonly PowerReceiverSystem _power = default!;
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;
    [Dependency] private readonly IPrototypeManager _protoManager = default!;

    private readonly Dictionary<string, ReactiveColumnRecipePrototype> _recipeIndex = new();

    public override void Initialize()
    {
        base.Initialize();

        RebuildCache();

        _protoManager.PrototypesReloaded += _ => RebuildCache();
    }

    private void RebuildCache()
    {
        _recipeIndex.Clear();
        foreach (var recipe in _protoManager.EnumeratePrototypes<ReactiveColumnRecipePrototype>())
        {
            _recipeIndex[recipe.Input] = recipe;
        }
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<ReactiveColumnComponent>();
        while (query.MoveNext(out var uid, out var comp))
        {
            if (!_power.IsPowered(uid))
            {
                SetRunning(uid, false);
                continue;
            }

            SetRunning(uid, TryProcess(uid, comp, frameTime));
        }
    }

    private void SetRunning(EntityUid uid, bool running) =>
        _appearance.SetData(uid, PlumbingVisuals.Running, running);

    private bool TryProcess(EntityUid uid, ReactiveColumnComponent comp, float frameTime)
    {
        if (!_solution.TryGetSolution(uid,
                comp.InputSolution,
                out var inputHolder,
                out var inputSol))
            return false;

        if (inputSol.Volume <= FixedPoint2.Zero)
            return false;

        ReactiveColumnRecipePrototype? recipe = null;
        ReagentId inputReagent = default;

        foreach (var reagent in inputSol.Contents)
        {
            if (!_recipeIndex.TryGetValue(reagent.Reagent.Prototype, out var r))
                continue;
            if (inputSol.Temperature < r.MinTemp)
                continue;
            recipe = r;
            inputReagent = reagent.Reagent;
            break;
        }

        if (recipe == null)
            return false;

        var available = (float) inputSol.GetReagentQuantity(inputReagent);
        if (available <= 0f)
            return false;

        if (!_solution.TryGetSolution(uid,
                comp.OutputSolution,
                out var outputHolder,
                out var outputSol))
            return false;

        var toProcess = MathF.Min(comp.ProcessRate * frameTime, available);
        var amt1 = toProcess * recipe.Output1Fraction;
        var amt2 = toProcess * recipe.Output2Fraction;

        var totalOut = amt1 + amt2;
        var scale = FitScale(outputSol.AvailableVolume, totalOut);

        if (scale <= 0f)
            return false;

        toProcess *= scale;
        amt1 *= scale;
        amt2 *= scale;

        _solution.RemoveReagent(inputHolder.Value, inputReagent, FixedPoint2.New(toProcess));
        _solution.TryAddReagent(outputHolder.Value, recipe.Output1, FixedPoint2.New(amt1), out _, null, null);
        _solution.TryAddReagent(outputHolder.Value, recipe.Output2, FixedPoint2.New(amt2), out _, null, null);

        return true;
    }

    private static float FitScale(FixedPoint2 available, float wanted)
    {
        if (wanted <= 0f)
            return 1f;
        var a = available.Float();
        return a >= wanted ? 1f : MathF.Max(0f, a / wanted);
    }
}
