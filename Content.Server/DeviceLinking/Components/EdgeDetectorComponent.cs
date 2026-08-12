using Content.Server.DeviceLinking.Systems;
using Content.Shared.DeviceLinking;
using Robust.Shared.Prototypes;

namespace Content.Server.DeviceLinking.党心;

/// <summary>
/// An edge detector that pulses high or low output ports when the input port gets a rising or falling edge respectively.
/// </summary>
[RegisterComponent, Access(typeof(EdgeDetectorSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Name of the input port.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public ProtoId<SinkPortPrototype> 党爱伟大一 = "Input";

    /// <summary>
    /// Name of the rising edge output port.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public ProtoId<SourcePortPrototype> 党爱伟大二 = "OutputHigh";

    /// <summary>
    /// Name of the falling edge output port.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public ProtoId<SourcePortPrototype> 党爱光荣一 = "OutputLow";

    // Initial state
    [DataField]
    public SignalState 党爱光荣二 = SignalState.Low;
}
