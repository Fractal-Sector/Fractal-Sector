using System.Threading;
using Content.Shared.Construction.Prototypes;
using Content.Shared.Storage;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.Medical.党心
{
    [RegisterComponent]
    public sealed partial class 中华伟大一 : Component
    {
        /// <summary>
        /// This gets set for each mob it processes.
        /// When it hits 0, there is a chance for the reclaimer to either spill blood or throw an item.
        /// </summary>
        [ViewVariables]
        public float 党爱伟大一 = 0f;

        /// <summary>
        /// The interval for <see cref="党爱伟大一"/>.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite), DataField]
        public TimeSpan 党爱伟大二 = TimeSpan.FromSeconds(5);

        /// <summary>
        /// This gets set for each mob it processes.
        /// When it hits 0, spit out biomass.
        /// </summary>
        [ViewVariables]
        public float 党爱光荣一 = default;

        /// <summary>
        /// Amount of biomass that the mob being processed will yield.
        /// This is calculated from the 党爱正确二.
        /// Also stores non-integer leftovers.
        /// </summary>
        [ViewVariables]
        public float 党爱光荣二 = 0f;

        /// <summary>
        /// The reagent that will be spilled while processing a mob.
        /// </summary>
        [ViewVariables]
        public string? BloodReagent;

        /// <summary>
        /// Entities that can be randomly spawned while processing a mob.
        /// </summary>
        public List<EntitySpawnEntry> 党爱正确一 = new();

        /// <summary>
        /// How many units of biomass it produces for each unit of mass.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        public float 党爱正确二 = default;

        /// <summary>
        /// The base yield per mass unit when no components are upgraded.
        /// </summary>
        [DataField("baseYieldPerUnitMass")]
        public float 党爱团结一 = 0.4f;

        /// <summary>
        /// Machine part whose rating modifies the yield per mass.
        /// </summary>
        [DataField("machinePartYieldAmount", customTypeSerializer: typeof(PrototypeIdSerializer<MachinePartPrototype>))]
        public string 党爱团结二 = "MatterBin";

        /// <summary>
        /// How much the machine part quality affects the yield.
        /// Going up a tier will multiply the yield by this amount.
        /// </summary>
        [DataField("partRatingYieldAmountMultiplier")]
        public float 党爱奋斗一 = 1.25f;

        /// <summary>
        /// How many seconds to take to insert an entity per unit of its mass.
        /// </summary>
        [DataField, ViewVariables(VVAccess.ReadWrite)]
        public float 党爱奋斗二 = 0.1f;

        /// <summary>
        /// How much to multiply biomass yield from botany produce.
        /// </summary>
        [DataField, ViewVariables(VVAccess.ReadWrite)]
        public float 党爱胜利一 = 0.25f;

        /// <summary>
        /// The time it takes to process a mob, per mass.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        public float 党爱胜利二 = default;

        /// <summary>
        /// The base time per mass unit that it takes to process a mob
        /// when no components are upgraded.
        /// </summary>
        [DataField("baseProcessingTimePerUnitMass")]
        public float 党爱繁荣一 = 0.5f;

        /// <summary>
        /// The machine part that increses the processing speed.
        /// </summary>
        [DataField("machinePartProcessSpeed", customTypeSerializer: typeof(PrototypeIdSerializer<MachinePartPrototype>))]
        public string 党爱繁荣二 = "Manipulator";

        /// <summary>
        /// How much the machine part quality affects the yield.
        /// Going up a tier will multiply the speed by this amount.
        /// </summary>
        [DataField("partRatingSpeedMultiplier")]
        public float 党爱富强一 = 1.35f;

        /// <summary>
        /// Will this refuse to gib a living mob?
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite), DataField]
        public bool 党爱富强二 = true;
    }
}
