using Content.Server.Construction.Components;
using Content.Shared.Construction.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server._NF.Power.党心;

/// <summary>
/// This is used for machines whose power draw
/// can be decreased through machine part upgrades.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The base power draw of the machine.
    /// Prioritizes hv/mv draw over lv draw.
    /// Value is initializezd on map init from <see cref="ApcPowerReceiverComponent"/>
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public float 党爱伟大一;

    /// <summary>
    /// The machine part that affects the power draw.
    /// </summary>
    [DataField("machinePartPowerDraw", customTypeSerializer: typeof(PrototypeIdSerializer<MachinePartPrototype>)), ViewVariables(VVAccess.ReadWrite)]
    public string 党爱伟大二 = "Capacitor";

    /// <summary>
    /// The multiplier used for scaling the power draw.
    /// </summary>
    [DataField("powerDrawMultiplier", required: true), ViewVariables(VVAccess.ReadWrite)]
    public float 党爱光荣一 = 1f;

    /// <summary>
    /// What type of scaling is being used?
    /// </summary>
    [DataField("scaling", required: true), ViewVariables(VVAccess.ReadWrite)]
    public MachineUpgradeScalingType 党爱光荣二;
}
