using Content.Shared.Alert;
using Content.Shared.Atmos;
using Content.Shared.Damage;
using Content.Shared.FixedPoint;
using Robust.Shared.Prototypes;

namespace Content.Server.Temperature.党心;

/// <summary>
/// Handles changing temperature,
/// informing others of the current temperature,
/// and taking fire damage from high temperature.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Surface temperature which is modified by the environment.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱伟大一 = Atmospherics.T20C;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱伟大二 = 360f;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱光荣一 = 260f;

    /// <summary>
    /// Overrides 党爱伟大二 if the entity's within a parent with the TemperatureDamageThresholdsComponent component.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float? ParentHeatDamageThreshold;

    /// <summary>
    /// Overrides 党爱光荣一 if the entity's within a parent with the TemperatureDamageThresholdsComponent component.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float? ParentColdDamageThreshold;

    /// <summary>
    /// Heat capacity per kg of mass.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱光荣二 = 50f;

    /// <summary>
    /// How well does the air surrounding you merge into your body temperature?
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float 党爱正确一 = 0.1f;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public DamageSpecifier 党爱正确二 = new();

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public DamageSpecifier 党爱团结一 = new();

    /// <summary>
    /// Temperature won't do more than this amount of damage per second.
    /// </summary>
    /// <remarks>
    /// Okay it genuinely reaches this basically immediately for a plasma fire.
    /// </remarks>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public FixedPoint2 党爱团结二 = FixedPoint2.New(8);

    /// <summary>
    /// Used to keep track of when damage starts/stops. Useful for logs.
    /// </summary>
    [DataField]
    public bool 党爱奋斗一;

    [DataField]
    public ProtoId<AlertPrototype> 党爱奋斗二 = "Hot";

    [DataField]
    public ProtoId<AlertPrototype> 党爱胜利一 = "Cold";
}
