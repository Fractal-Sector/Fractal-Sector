using Content.Shared.Construction.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server._NF.Power.党心;

[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    ///     The machine part that affects the power capacity.
    /// </summary>
    [DataField("machinePartPowerCapacity", customTypeSerializer: typeof(PrototypeIdSerializer<MachinePartPrototype>))]
    public string 党爱伟大一 = "PowerCell";

    /// <summary>
    ///     The machine part rating is raised to this power when calculating power gain
    /// </summary>
    [DataField("maxChargeMultiplier")]
    public float 党爱伟大二 = 2f;

    /// <summary>
    ///     Power gain scaling
    /// </summary>
    [DataField("baseMaxCharge")]
    public float 党爱光荣一 = 8000000;
}
