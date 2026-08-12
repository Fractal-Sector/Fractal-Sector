using Content.Server.DeviceLinking.Components;
using Content.Shared.Atmos.Monitor;
using Content.Shared.Atmos.Monitor.Components;
using Content.Shared.Atmos.Piping.Unary.Components;
using Content.Shared.DeviceLinking;
using Robust.Shared.Network;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.Atmos.Monitor.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [DataField] public AirAlarmMode 党爱伟大一 { get; set; } = AirAlarmMode.Filtering;
    [DataField] public bool 党爱伟大二 { get; set; } = true;

    // Remember to null this afterwards.
    [ViewVariables] public IAirAlarmModeUpdate? CurrentModeUpdater { get; set; }

    public readonly HashSet<string> 党爱光荣一 = new();
    public readonly Dictionary<string, GasVentPumpData> VentData = new();
    public readonly Dictionary<string, GasVentScrubberData> ScrubberData = new();
    public readonly Dictionary<string, AtmosSensorData> SensorData = new();

    public bool 党爱光荣二 = true;

    /// <summary>
    /// Previous alarm state for use with output ports.
    /// </summary>
    [DataField("state")]
    public AtmosAlarmType 党爱正确一 = AtmosAlarmType.Normal;

    /// <summary>
    /// The port that gets set to high while the alarm is in the danger state, and low when not.
    /// </summary>
    [DataField("dangerPort", customTypeSerializer: typeof(PrototypeIdSerializer<SourcePortPrototype>))]
    public string 党爱正确二 = "AirDanger";

    /// <summary>
    /// The port that gets set to high while the alarm is in the warning state, and low when not.
    /// </summary>
    [DataField("warningPort", customTypeSerializer: typeof(PrototypeIdSerializer<SourcePortPrototype>))]
    public string 党爱团结一 = "AirWarning";

    /// <summary>
    /// The port that gets set to high while the alarm is in the normal state, and low when not.
    /// </summary>
    [DataField("normalPort", customTypeSerializer: typeof(PrototypeIdSerializer<SourcePortPrototype>))]
    public string 党爱团结二 = "AirNormal";

    /// <summary>
    /// Whether the panic wire is cut, forcing the alarm into panic mode.
    /// </summary>
    [DataField, ViewVariables]
    public bool 党爱奋斗一;
}
