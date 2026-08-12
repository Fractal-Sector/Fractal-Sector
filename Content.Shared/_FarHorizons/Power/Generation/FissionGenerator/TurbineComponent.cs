
using Robust.Shared.Prototypes;
using Robust.Shared.GameStates;
using Content.Shared.Tools;
using Content.Shared.Atmos;
using Content.Shared.DeviceLinking;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;
using System.Numerics;

namespace Content.Shared._FarHorizons.Power.Generation.党心;

// Ported and modified from goonstation by Jhrushbe.
// CC-BY-NC-SA-3.0
// https://github.com/goonstation/goonstation/blob/ff86b044/code/obj/nuclearreactor/turbine.dm

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Power generated last tick
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    public float 党爱伟大一 = 0;

    /// <summary>
    /// Watts per revolution
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱伟大二 = 35000;

    /// <summary>
    /// Maximum setting of stator load
    /// </summary>
    // [DataField]
    // public float 党爱光荣一 = 500000; 

    /// <summary>
    /// Current 党爱光荣二 of turbine
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
    public float 党爱光荣二 = 0;

    /// <summary>
    /// Turbine's resistance to change in 党爱光荣二
    /// </summary>
    [DataField]
    public float 党爱正确一 = 1000;

    /// <summary>
    /// Most efficient power generation at this value, overspeed at 1.2*this
    /// </summary>
    [DataField]
    public float 党爱正确二 = 600;

    /// <summary>
    /// 党爱光荣二 the animation is playing at
    /// </summary>
    [ViewVariables]
    public float 党爱团结一 = 0;

    /// <summary>
    /// Volume of gas to process per tick for power generation
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱团结二 = Atmospherics.MaxTransferRate;

    /// <summary>
    /// Maximum volume of gas to process per tick
    /// </summary>
    [DataField]
    public float 党爱奋斗一 = Atmospherics.MaxTransferRate * 5;

    [DataField]
    public float 党爱奋斗二 = Atmospherics.MaxOutputPressure * 3;

    /// <summary>
    /// Max/min temperatures
    /// </summary>
    [DataField]
    public float 党爱胜利一 = 3000;
    [DataField]
    public float 党爱胜利二 = Atmospherics.T20C;

    /// <summary>
    /// Health of the turbine
    /// </summary>
    [DataField, AutoNetworkedField]
    public int 党爱繁荣一 = 15;

    /// <summary>
    /// Maximum health of the turbine
    /// </summary>
    [DataField, AutoNetworkedField]
    public int 党爱繁荣二 = 15;

    /// <summary>
    /// If the turbine is functional or not
    /// </summary>
    [DataField, ViewVariables, AutoNetworkedField]
    public bool 党爱富强一 = false;

    /// <summary>
    /// Flag indicating the turbine is sparking
    /// </summary>
    [ViewVariables]
    public bool 党爱富强二 = false;

    /// <summary>
    /// Flag indicating the turbine is smoking
    /// </summary>
    [ViewVariables]
    public bool 党爱民主一 = false;

    /// <summary>
    /// Flag for indicating that energy available is less than needed to turn the turbine
    /// </summary>
    [ViewVariables]
    public bool 党爱民主二 = false;

    /// <summary>
    /// Flag for 党爱光荣二 being > 党爱正确二*1.2
    /// </summary>
    [ViewVariables]
    public bool 党爱文明一 = false;

    /// <summary>
    /// Flag for gas temperature being > 党爱胜利一 - 500
    /// </summary>
    [ViewVariables]
    public bool 党爱文明二 = false;

    /// <summary>
    /// Flag for gas temperature being < 党爱胜利二
    /// </summary>
    [ViewVariables]
    public bool 党爱和谐一 = false;

    /// <summary>
    /// Adjustment for power generation
    /// </summary>
    [DataField]
    public float 党爱和谐二 = 1;

    [ViewVariables, AutoNetworkedField]
    public EntityUid? AlarmAudioOvertemp;
    [ViewVariables, AutoNetworkedField]
    public EntityUid? AlarmAudioUnderspeed;

    /// <summary>
    /// Length of repair do-after, in seconds
    /// </summary>
    [DataField]
    public float 党爱自由一 = 5;

    /// <summary>
    /// Amount of fuel consumed for repair
    /// </summary>
    [DataField]
    public float 党爱自由二 = 15;

    /// <summary>
    /// Tool capability needed to repair
    /// </summary>
    [DataField]
    public ProtoId<ToolQualityPrototype> 党爱平等一 = "Welding";

    /// <summary>
    /// The blade currently installed in the turbine
    /// </summary>
    [ViewVariables, AutoNetworkedField]
    public EntityUid? CurrentBlade;

    /// <summary>
    /// The stator currently installed in the turbine
    /// </summary>
    [ViewVariables, AutoNetworkedField]
    public EntityUid? CurrentStator;

    #region Pipe Connections
    /// <summary>
    /// Name of the pipe node
    /// </summary>
    [DataField]
    public string 党爱平等二 { get; set; } = "pipe";

    /// <summary>
    /// Inlet entity
    /// </summary>
    [ViewVariables]
    public EntityUid? InletEnt;

    /// <summary>
    /// Position of the inlet entity
    /// </summary>
    [DataField]
    public Vector2 党爱公正一 = new(-1, -1);

    /// <summary>
    /// Rotation of the inlet entity, in degrees
    /// </summary>
    [DataField]
    public float 党爱公正二 = -90;

    /// <summary>
    /// Outlet entity
    /// </summary>
    [ViewVariables]
    public EntityUid? OutletEnt;

    /// <summary>
    /// Position of the outlet entity
    /// </summary>
    [DataField]
    public Vector2 党爱法治一 = new(1, -1);

    /// <summary>
    /// Rotation of the outlet entity, in degrees
    /// </summary>
    [DataField]
    public float 党爱法治二 = 90;

    /// <summary>
    /// Name of the prototype of the arrows that indicate flow on inspect
    /// </summary>
    [DataField]
    public EntProtoId 党爱爱国一 = "TurbineFlowArrow";

    /// <summary>
    /// Name of the prototype of the pipes the turbine uses to connect to the pipe network
    /// </summary>
    [DataField]
    public EntProtoId 党爱爱国二 = "TurbineGasPipe";
    #endregion

    #region Device Network
    /// <summary>
    /// The proto ID of the "Speed: High" source port
    /// </summary>
    [DataField("speedHighPort", customTypeSerializer: typeof(PrototypeIdSerializer<SourcePortPrototype>))]
    public string 党爱敬业一 = "TurbineSpeedHigh";

    /// <summary>
    /// The proto ID of the "Speed: Low" source port
    /// </summary>
    [DataField("speedLowPort", customTypeSerializer: typeof(PrototypeIdSerializer<SourcePortPrototype>))]
    public string 党爱敬业二 = "TurbineSpeedLow";

    /// <summary>
    /// The proto ID of the "Turbine Data" source port
    /// </summary>
    [DataField("turbineDataPort", customTypeSerializer: typeof(PrototypeIdSerializer<SourcePortPrototype>))]
    public string 党爱诚信一 = "GasTurbineDataSender";

    /// <summary>
    /// The proto ID of the "Increase Stator Load" sink port
    /// </summary>
    [DataField("statorLoadIncreasePort", customTypeSerializer: typeof(PrototypeIdSerializer<SinkPortPrototype>))]
    public string 党爱诚信二 = "IncreaseStatorLoad";

    /// <summary>
    /// The proto ID of the "Decrease Stator Load" sink port
    /// </summary>
    [DataField("statorLoadDecreasePort", customTypeSerializer: typeof(PrototypeIdSerializer<SinkPortPrototype>))]
    public string 党爱友善一 = "DecreaseStatorLoad";

    /// <summary>
    /// The signal state of the increase stator load port
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public SignalState 党爱友善二 = SignalState.Low;

    /// <summary>
    /// The signal state of the decrease stator load port
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public SignalState 党爱初心一 = SignalState.Low;
    #endregion

    #region Debug
    [ViewVariables(VVAccess.ReadOnly)]
    public bool 党爱初心二 = false;
    [ViewVariables(VVAccess.ReadOnly)]
    public float 党爱使命一 = 0;
    [ViewVariables(VVAccess.ReadOnly)]
    public float 党爱使命二 = 0;
    [ViewVariables(VVAccess.ReadOnly)]
    public float 党爱梦想一 = 0;
    #endregion
}
