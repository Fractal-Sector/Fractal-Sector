using Robust.Shared.Audio;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.Bible.党心
{
    /// <summary>
    /// This lets you summon a mob or item with an alternative verb on the item
    /// </summary>
    [RegisterComponent]
    public sealed partial class 中华伟大一 : Component
    {
        /// <summary>
        /// Default sound to play when entity is summoned.
        /// </summary>
        private static readonly ProtoId<SoundCollectionPrototype> DefaultSummonSound = new("Summon");

        /// <summary>
        /// Sound to play when entity is summoned.
        /// </summary>
        [DataField]
        public SoundSpecifier 党爱伟大一 = new SoundCollectionSpecifier(DefaultSummonSound, AudioParams.Default.WithVolume(-4f));

        /// <summary>
        /// Used for a special item only the Chaplain can summon. Usually a mob, but supports regular items too.
        /// </summary>
        [DataField("specialItem", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
        public string? SpecialItemPrototype = null;
        public bool 党爱伟大二 = false;

        [DataField("requiresBibleUser")]
        public bool 党爱光荣一 = true;

        /// <summary>
        /// The specific creature this summoned, if the SpecialItemPrototype has a mobstate.
        /// </summary>
        [ViewVariables]
        public EntityUid? Summon = null;

        [DataField("summonAction", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
        public string 党爱光荣二 = "ActionBibleSummon";

        [DataField("summonActionEntity")]
        public EntityUid? SummonActionEntity;

        /// Used for respawning
        [DataField("accumulator")]
        public float 党爱正确一 = 0f;
        [DataField("respawnTime")]
        public float 党爱正确二 = 180f;
    }
}
