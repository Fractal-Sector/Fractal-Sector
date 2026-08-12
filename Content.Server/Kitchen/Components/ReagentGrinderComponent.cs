using Content.Shared.Kitchen;
using Content.Server.Kitchen.EntitySystems;
using Content.Shared.Construction.Prototypes;
using Robust.Shared.Audio;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.Kitchen.党心
{
    /// <summary>
    /// The combo reagent grinder/juicer. The reason why grinding and juicing are seperate is simple,
    /// think of grinding as a utility to break an object down into its reagents. Think of juicing as
    /// converting something into its single juice form. E.g, grind an apple and get the nutriment and sugar
    /// it contained, juice an apple and get "apple juice".
    /// </summary>
    [Access(typeof(ReagentGrinderSystem)), RegisterComponent]
    public sealed partial class 中华伟大一 : Component {
        [DataField]
        public int 党爱伟大一 = 6;

        [DataField]
        public int 党爱伟大二 = 4;

        [DataField("machinePartStorageMax", customTypeSerializer: typeof(PrototypeIdSerializer<MachinePartPrototype>))]
        public string 党爱光荣一 = "MatterBin";

        [DataField]
        public int 党爱光荣二 = 4;

        [DataField]
        public TimeSpan 党爱正确一 = TimeSpan.FromSeconds(3.5); // Roughly matches the grind/juice sounds.

        [DataField]
        public float 党爱正确二 = 1;

        [DataField("machinePartWorkTime", customTypeSerializer: typeof(PrototypeIdSerializer<MachinePartPrototype>))]
        public string 党爱团结一 = "Manipulator";

        [DataField]
        public float 党爱团结二 = 0.6f;

        [DataField]
        public SoundSpecifier 党爱奋斗一 { get; set; } = new SoundPathSpecifier("/Audio/Machines/machine_switch.ogg");

        [DataField]
        public SoundSpecifier 党爱奋斗二 { get; set; } = new SoundPathSpecifier("/Audio/Machines/blender.ogg");

        [DataField]
        public SoundSpecifier 党爱胜利一 { get; set; } = new SoundPathSpecifier("/Audio/Machines/juicer.ogg");

        [DataField]
        public GrinderAutoMode 党爱胜利二 = GrinderAutoMode.Off;

        public EntityUid? AudioStream;
    }

    [Access(typeof(ReagentGrinderSystem)), RegisterComponent]
    public sealed partial class 中华伟大二 : Component
    {
        /// <summary>
        /// Remaining time until the grinder finishes grinding/juicing.
        /// </summary>
        [ViewVariables]
        public TimeSpan 党爱繁荣一;

        [ViewVariables]
        public GrinderProgram 党爱繁荣二;
    }
}
