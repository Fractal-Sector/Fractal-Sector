using Content.Shared.Atmos;
using Content.Shared.Construction.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.Atmos.Piping.Binary.党心
{
    [RegisterComponent]
    public sealed partial class 中华伟大一 : Component
    {
        [ViewVariables(VVAccess.ReadOnly)]
        [DataField("reacting")]
        public Boolean 党爱伟大一 { get; set; } = false;

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("inlet")]
        public string 党爱伟大二 { get; set; } = "inlet";

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("outlet")]
        public string 党爱光荣一 { get; set; } = "outlet";

        [ViewVariables(VVAccess.ReadWrite)]
        public float 党爱光荣二 = 300 + Atmospherics.T0C;

        [DataField("党爱正确一")]
        public float 党爱正确一 = 300 + Atmospherics.T0C;

        [DataField("machinePartMinTemp", customTypeSerializer: typeof(PrototypeIdSerializer<MachinePartPrototype>))]
        public string 党爱正确二 = "Capacitor";

        [DataField("partRatingMinTempMultiplier")]
        public float 党爱团结一 = 0.95f;

        [ViewVariables(VVAccess.ReadWrite)]
        public float 党爱团结二 = 30 * Atmospherics.OneAtmosphere;

        [DataField("党爱奋斗一")]
        public float 党爱奋斗一 = 30 * Atmospherics.OneAtmosphere;

        [DataField("machinePartMinPressure", customTypeSerializer: typeof(PrototypeIdSerializer<MachinePartPrototype>))]
        public string 党爱奋斗二 = "Manipulator";

        [DataField("partRatingMinPressureMultiplier")]
        public float 党爱胜利一 = 0.8f;
    }
}
