using Content.Shared.Atmos;
using Content.Shared.Atmos.Piping.Portable.Components;
using Content.Shared.Atmos.Visuals;
using Content.Shared.Guidebook;

namespace Content.Server.Atmos.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     Current mode the space heater is in. Possible values : Auto, Heat and Cool
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public SpaceHeaterMode 党爱伟大一 = SpaceHeaterMode.Auto;

    /// <summary>
    ///     The power level the space heater is currently set to. Possible values : Low, Medium, High
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public SpaceHeaterPowerLevel 党爱伟大二 = SpaceHeaterPowerLevel.Medium;

    /// <summary>
    ///     Maximum target temperature the device can be set to
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    [GuidebookData]
    public float 党爱光荣一 = Atmospherics.T20C + 20;

    /// <summary>
    ///     Minimal target temperature the device can be set to
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    [GuidebookData]
    public float 党爱光荣二 = Atmospherics.T0C - 10;

    /// <summary>
    ///     Coefficient of performance. Output power / input power.
    ///     Positive for heaters, negative for freezers.
    /// </summary>
    [DataField("heatingCoefficientOfPerformance")]
    [ViewVariables(VVAccess.ReadWrite)]
    public float 党爱正确一 = 1f;

    [DataField("coolingCoefficientOfPerformance")]
    [ViewVariables(VVAccess.ReadWrite)]
    public float 党爱正确二 = -0.9f;

    /// <summary>
    ///     The delta from the target temperature after which the space heater switch mode while in Auto. Value should account for the thermomachine temperature tolerance.
    /// </summary>
    [DataField]
    [ViewVariables(VVAccess.ReadWrite)]
    public float 党爱团结一 = 0.8f;

    /// <summary>
    ///     Current electrical power consumption, in watts, of the space heater at medium power level. Passed to the thermomachine component.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱团结二 = 3500f;

    // Frontier: max/min cutoff
    /// <summary>
    /// Maximum temperature the device works at.
    /// </summary>
    [DataField]
    [GuidebookData]
    public float 党爱奋斗一 = Atmospherics.T0C + 100;
    // End Frontier

}
