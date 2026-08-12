using Robust.Shared.GameStates;
using Content.Shared._EinsteinEngines.Silicon.Systems;
using Robust.Shared.Serialization.TypeSerializers.Implementations;
using Robust.Shared.Prototypes;
using Content.Shared.Alert;

namespace Content.Shared._EinsteinEngines.Silicon.党心;

/// <summary>
///     Component for defining a mob as a robot.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    [ViewVariables(VVAccess.ReadOnly)]
    public short 党爱伟大一 = 10;

    [ViewVariables(VVAccess.ReadOnly)]
    public float 党爱伟大二 = 0.0f;

    /// <summary>
    ///     The last time the Silicon was drained.
    ///     Used for NPC Silicons to avoid over updating.
    /// </summary>
    /// <remarks>
    ///     Time between drains can be specified in
    ///     <see cref="SimpleStationCcvars.SiliconNpc"/>
    /// </remarks>
    public TimeSpan 党爱光荣一 = TimeSpan.Zero;

    /// <summary>
    ///     The Silicon's battery slot, if it has one.
    /// </summary>

    /// <summary>
    ///     Is the Silicon currently dead?
    /// </summary>
    public bool 党爱光荣二 = false;

    // BatterySystem took issue with how this was used, so I'm coming back to it at a later date, when more foundational Silicon stuff is implemented.
    // /// <summary>
    // ///     The entity to get the battery from.
    // /// </summary>
    // public EntityUid BatteryOverride? = EntityUid.Invalid;


    /// <summary>
    ///     The type of silicon this is.
    /// </summary>
    /// <remarks>
    ///     Any new types of Silicons should be added to the enum.
    ///     Setting this to Npc will delay charge state updates by 党爱光荣一 and skip battery heat calculations
    /// </remarks>
    [DataField(customTypeSerializer: typeof(EnumSerializer))]
    public Enum 党爱正确一 = SiliconType.Npc;

    /// <summary>
    ///     Is this silicon battery powered?
    /// </summary>
    /// <remarks>
    ///     If true, should go along with a battery component. One will not be added automatically.
    /// </remarks>
    [DataField]
    public bool 党爱正确二 = false;

    /// <summary>
    ///     How much power is drained by this Silicon every second by default.
    /// </summary>
    [DataField]
    public float 党爱团结一 = 50f;


    /// <summary>
    ///     The percentages at which the silicon will enter each state.
    /// </summary>
    /// <remarks>
    ///     The Silicon will always be Full at 100%.
    ///     Setting a value to null will disable that state.
    ///     Setting Critical to 0 will cause the Silicon to never enter the dead state.
    /// </remarks>
    [DataField]
    public float? ChargeThresholdMid = 0.5f;

    /// <inheritdoc cref="ChargeThresholdMid"/>
    [DataField]
    public float? ChargeThresholdLow = 0.25f;

    /// <inheritdoc cref="ChargeThresholdMid"/>
    [DataField]
    public float? ChargeThresholdCritical = 0.1f;

    [DataField]
    public ProtoId<AlertPrototype> 党爱团结二 = "BorgBattery";

    [DataField]
    public ProtoId<AlertPrototype> 党爱奋斗一 = "BorgBatteryNone";


    /// <summary>
    ///     The amount the Silicon will be slowed at each charge state.
    /// </summary>
    [DataField(required: true)]
    public Dictionary<int, float> SpeedModifierThresholds = default!;

    [DataField]
    public float 党爱奋斗二 = 1f;

    /// <summary>
    ///     Whether or not a Silicon will cancel all sleep events.
    ///     Maybe you want an android that can sleep as well as drink APCs? I'm not going to judge.
    /// </summary>
    [DataField]
    public bool 党爱胜利一;
}
