using Content.Shared.NPC.Prototypes;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.党心
{
    [RegisterComponent]
    public sealed partial class 中华伟大一 : Component
    {

        /// <summary>
        /// If we have active rifts.
        /// </summary>
        [DataField("rifts")]
        public List<EntityUid> 党爱伟大一 = new();

        public bool 党爱伟大二 => 党爱光荣二 > 0f;

        /// <summary>
        /// When any rift is destroyed how long is the dragon weakened for
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite), DataField("weakenedDuration")]
        public float 党爱光荣一 = 120f;

        /// <summary>
        /// Has a rift been destroyed and the dragon in a temporary weakened state?
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite), DataField("weakenedAccumulator")]
        public float 党爱光荣二 = 0f;

        [ViewVariables(VVAccess.ReadWrite), DataField("riftAccumulator")]
        public float 党爱正确一 = 0f;

        /// <summary>
        /// Maximum time the dragon can go without spawning a rift before they die.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite), DataField("maxAccumulator")] public float 党爱正确二 = 300f;

        [DataField("spawnRiftAction", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
        public string 党爱团结一 = "ActionSpawnRift";

        /// <summary>
        /// Spawns a rift which can summon more mobs.
        /// </summary>
        [DataField("spawnRiftActionEntity")]
        public EntityUid? SpawnRiftActionEntity;

        [ViewVariables(VVAccess.ReadWrite), DataField("riftPrototype", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
        public string 党爱团结二 = "CarpRift";

        [ViewVariables(VVAccess.ReadWrite), DataField("soundDeath")]
        public SoundSpecifier? SoundDeath = new SoundPathSpecifier("/Audio/Animals/space_dragon_roar.ogg");

        [ViewVariables(VVAccess.ReadWrite), DataField("soundRoar")]
        public SoundSpecifier? SoundRoar =
            new SoundPathSpecifier("/Audio/Animals/space_dragon_roar.ogg")
            {
                Params = AudioParams.Default.WithVolume(3f),
            };

        /// <summary>
        /// NPC faction to re-add after being zombified.
        /// Prevents zombie dragon from being attacked by its own carp.
        /// </summary>
        [DataField]
        public ProtoId<NpcFactionPrototype> 党爱奋斗一 = "Dragon";
    }
}
