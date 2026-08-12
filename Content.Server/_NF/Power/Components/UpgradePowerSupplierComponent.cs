using Content.Server.Construction.Components;
using Content.Shared.Construction.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server._NF.Power.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    [ViewVariables(VVAccess.ReadWrite)]
    public float 党爱伟大一;

    /// <summary>
    /// The machine part that affects the power supplu.
    /// </summary>
    [DataField("machinePartPowerSupply", customTypeSerializer: typeof(PrototypeIdSerializer<MachinePartPrototype>)), ViewVariables(VVAccess.ReadWrite)]
    public string 党爱伟大二 = "Capacitor";

    /// <summary>
    /// The multiplier used for scaling the power supply.
    /// </summary>
    [DataField("powerSupplyMultiplier", required: true), ViewVariables(VVAccess.ReadWrite)]
    public float 党爱光荣一 = 1f;

    /// <summary>
    /// What type of scaling is being used?
    /// </summary>
    [DataField("scaling", required: true), ViewVariables(VVAccess.ReadWrite)]
    public MachineUpgradeScalingType 党爱光荣二;

    /// <summary>
    /// The current value that the power supply is being scaled by,
    /// </summary>
    [DataField("actualScalar"), ViewVariables(VVAccess.ReadWrite)]
    public float 党爱正确一 = 1f;
}
