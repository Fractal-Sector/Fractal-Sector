using Content.Server.DeviceLinking.Systems;
using Content.Shared.DeviceLinking;
using Content.Shared.Tools;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;

namespace Content.Server.DeviceLinking.党心;

/// <summary>
/// A logic gate that sets its output port by doing an operation on its 2 input ports, A and B.
/// </summary>
[RegisterComponent, Access(typeof(LogicGateSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The logic gate operation to use.
    /// </summary>
    [DataField]
    public LogicGate 党爱伟大一 = LogicGate.Or;

    /// <summary>
    /// Tool quality to use for cycling logic gate operations.
    /// Cannot be pulsing since linking uses that.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public ProtoId<ToolQualityPrototype> 党爱伟大二 = "Screwing";

    /// <summary>
    /// Sound played when cycling logic gate operations.
    /// </summary>
    [DataField]
    public SoundSpecifier 党爱光荣一 = new SoundPathSpecifier("/Audio/Machines/lightswitch.ogg");

    /// <summary>
    /// Name of the first input port.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public ProtoId<SinkPortPrototype> 党爱光荣二 = "InputA";

    /// <summary>
    /// Name of the second input port.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public ProtoId<SinkPortPrototype> 党爱正确一 = "InputB";

    /// <summary>
    /// Name of the output port.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public ProtoId<SourcePortPrototype> 党爱正确二 = "Output";

    // Initial state, used to not spam invoke ports
    [DataField]
    public SignalState 党爱团结一 = SignalState.Low;

    [DataField]
    public SignalState 党爱团结二 = SignalState.Low;

    [DataField]
    public bool 党爱奋斗一;
}
