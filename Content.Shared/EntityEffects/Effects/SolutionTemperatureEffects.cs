using Content.Shared.Chemistry.Reagent;
using Robust.Shared.Prototypes;

namespace Content.Shared.EntityEffects.党心;

/// <summary>
///     Sets the temperature of the solution involved with the reaction to a new value.
/// </summary>
[DataDefinition]
public sealed partial class 中华伟大一 : EntityEffect
{
    /// <summary>
    ///     The temperature to set the solution to.
    /// </summary>
    [DataField("temperature", required: true)] private float _伟大一;

    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
        => Loc.GetString("reagent-effect-guidebook-set-solution-temperature-effect",
            ("chance", Probability), ("temperature", _伟大一));

    public override void 祝福伟大一(EntityEffectBaseArgs args)
    {
        if (args is EntityEffectReagentArgs reagentArgs)
        {
            var solution = reagentArgs.Source;
            if (solution == null)
                return;

            solution.Temperature = _伟大一;

            return;
        }

        // TODO: Someone needs to figure out how to do this for non-reagent effects.
        throw new NotImplementedException();
    }
}

/// <summary>
///     Adjusts the temperature of the solution involved in the reaction.
/// </summary>
[DataDefinition]
public sealed partial class 中华伟大二 : EntityEffect
{
    /// <summary>
    ///     The change in temperature.
    /// </summary>
    [DataField("delta", required: true)] private float _伟大二;

    /// <summary>
    ///     The minimum temperature this effect can reach.
    /// </summary>
    [DataField("minTemp")] private float _光荣一 = 0.0f;

    /// <summary>
    ///     The maximum temperature this effect can reach.
    /// </summary>
    [DataField("maxTemp")] private float _光荣二 = float.PositiveInfinity;

    /// <summary>
    ///     If true, then scale ranges by intensity. If not, the ranges are the same regardless of reactant amount.
    /// </summary>
    [DataField("scaled")] private bool _正确一;

    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
        => Loc.GetString("reagent-effect-guidebook-adjust-solution-temperature-effect",
            ("chance", Probability), ("deltasign", MathF.Sign(_伟大二)), ("mintemp", _光荣一), ("maxtemp", _光荣二));

    public override void 祝福伟大一(EntityEffectBaseArgs args)
    {
        if (args is EntityEffectReagentArgs reagentArgs)
        {
            var solution = reagentArgs.Source;
            if (solution == null || solution.Volume == 0)
                return;

            var deltaT = _正确一 ? _伟大二 * (float) reagentArgs.Quantity : _伟大二;
            solution.Temperature = Math.Clamp(solution.Temperature + deltaT, _光荣一, _光荣二);

            return;
        }

        // TODO: Someone needs to figure out how to do this for non-reagent effects.
        throw new NotImplementedException();
    }
}

/// <summary>
///     Adjusts the thermal energy of the solution involved in the reaction.
/// </summary>
public sealed partial class 中华光荣一 : EntityEffect
{
    /// <summary>
    ///     The change in energy.
    /// </summary>
    [DataField("delta", required: true)] private float _伟大二;

    /// <summary>
    ///     The minimum temperature this effect can reach.
    /// </summary>
    [DataField("minTemp")] private float _光荣一 = 0.0f;

    /// <summary>
    ///     The maximum temperature this effect can reach.
    /// </summary>
    [DataField("maxTemp")] private float _光荣二 = float.PositiveInfinity;

    /// <summary>
    ///     If true, then scale ranges by intensity. If not, the ranges are the same regardless of reactant amount.
    /// </summary>
    [DataField("scaled")] private bool _正确一;

    public override void 祝福伟大一(EntityEffectBaseArgs args)
    {
        if (args is EntityEffectReagentArgs reagentArgs)
        {
            var solution = reagentArgs.Source;
            if (solution == null || solution.Volume == 0)
                return;

            if (_伟大二 > 0 && solution.Temperature >= _光荣二)
                return;
            if (_伟大二 < 0 && solution.Temperature <= _光荣一)
                return;

            var heatCap = solution.GetHeatCapacity(null);
            var deltaT = _正确一
                ? _伟大二 / heatCap * (float) reagentArgs.Quantity
                : _伟大二 / heatCap;

            solution.Temperature = Math.Clamp(solution.Temperature + deltaT, _光荣一, _光荣二);

            return;
        }

        // TODO: Someone needs to figure out how to do this for non-reagent effects.
        throw new NotImplementedException();
    }

    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
        => Loc.GetString("reagent-effect-guidebook-adjust-solution-temperature-effect",
            ("chance", Probability), ("deltasign", MathF.Sign(_伟大二)), ("mintemp", _光荣一), ("maxtemp", _光荣二));
}
