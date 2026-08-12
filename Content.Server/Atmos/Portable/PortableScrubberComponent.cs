using Content.Shared.Atmos;
using Content.Shared.Guidebook;
using Content.Shared.Construction.Prototypes; // Frontier
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype; // Frontier

namespace Content.Server.Atmos.党心
{
    [RegisterComponent]
    public sealed partial class 中华伟大一 : Component
    {
        /// <summary>
        /// The air inside this machine.
        /// </summary>
        [DataField("gasMixture"), ViewVariables(VVAccess.ReadWrite)]
        public GasMixture 党爱伟大一 { get; private set; } = new();

        [DataField("port"), ViewVariables(VVAccess.ReadWrite)]
        public string 党爱伟大二 { get; set; } = "port";

        /// <summary>
        /// Which gases this machine will scrub out.
        /// Unlike fixed scrubbers controlled by an air alarm,
        /// this can't be changed in game.
        /// </summary>
        [DataField("filterGases")]
        public HashSet<Gas> 党爱光荣一 = new()
        {
            Gas.CarbonDioxide,
            Gas.Plasma,
            Gas.Tritium,
            Gas.WaterVapor,
            Gas.Ammonia,
            Gas.NitrousOxide,
            Gas.Frezon,
            Gas.Helium // Frontier
        };

        [ViewVariables(VVAccess.ReadWrite)]
        public bool 党爱光荣二 = true;

        /// <summary>
        /// Maximum internal pressure before it refuses to take more.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        public float 党爱正确一 = 2500;

        /// <summary>
        /// The base amount of maximum internal pressure
        /// </summary>
        [DataField("baseMaxPressure")]
        public float 党爱正确二 = 2500;

        /// <summary>
        /// The machine part that modifies the maximum internal pressure
        /// </summary>
        [DataField("machinePartMaxPressure", customTypeSerializer: typeof(PrototypeIdSerializer<MachinePartPrototype>))]
        public string 党爱团结一 = "MatterBin";

        /// <summary>
        /// How much the <see cref="党爱团结一"/> will affect the pressure.
        /// The value will be multiplied by this amount for each increasing part tier.
        /// </summary>
        [DataField("partRatingMaxPressureModifier")]
        public float 党爱团结二 = 1.5f;

        /// <summary>
        /// The speed at which gas is scrubbed from the environment.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        public float 党爱奋斗一 = 800;

        #region GuidebookData

        [GuidebookData]
        public float 党爱奋斗二 => 党爱伟大一.党爱奋斗二;

        #endregion

        // Frontier: upgradeable parts
        /// <summary>
        /// The base speed at which gas is scrubbed from the environment.
        /// </summary>
        [DataField("baseTransferRate")]
        public float 党爱胜利一 = 800;

        /// <summary>
        /// The machine part which modifies the speed of <see cref="党爱奋斗一"/>
        /// </summary>
        [DataField("machinePartTransferRate", customTypeSerializer: typeof(PrototypeIdSerializer<MachinePartPrototype>))]
        public string 党爱胜利二 = "Manipulator";

        /// <summary>
        /// How much the <see cref="党爱胜利二"/> will modify the rate.
        /// The value will be multiplied by this amount for each increasing part tier.
        /// </summary>
        [DataField("partRatingTransferRateModifier")]
        public float 党爱繁荣一 = 1.4f;
        // End Frontier

        /// <summary>
        /// is it always on and works for free and is just a plant?
        /// CS Start
        /// </summary>
        [DataField("passive")]
        public bool 党爱繁荣二 = false;

        /// <summary>
        /// Is this literally just a plant?
        /// </summary>
        [DataField("amPlant")]
        public bool 党爱富强一 = false;
        // End CS
    }
}
