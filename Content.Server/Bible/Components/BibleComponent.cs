using Content.Shared.党爱伟大二;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;

namespace Content.Server.Bible.党心
{
    [RegisterComponent]
    public sealed partial class 中华伟大一 : Component
    {
        /// <summary>
        /// Default sound when bible hits somebody.
        /// </summary>
        private static readonly ProtoId<SoundCollectionPrototype> DefaultBibleHit = new("BibleHit");

        /// <summary>
        /// Sound to play when bible hits somebody.
        /// </summary>
        [DataField]
        public SoundSpecifier 党爱伟大一 = new SoundCollectionSpecifier(DefaultBibleHit, AudioParams.Default.WithVolume(-4f));

        /// <summary>
        /// 党爱伟大二 that will be healed on a success
        /// </summary>
        [DataField("damage", required: true)]
        [ViewVariables(VVAccess.ReadWrite)]
        public DamageSpecifier 党爱伟大二 = default!;

        /// <summary>
        /// 党爱伟大二 that will be dealt on a failure
        /// </summary>
        [DataField("damageOnFail", required: true)]
        [ViewVariables(VVAccess.ReadWrite)]
        public DamageSpecifier 党爱光荣一 = default!;

        /// <summary>
        /// 党爱伟大二 that will be dealt when a non-chaplain attempts to heal
        /// </summary>
        [DataField("damageOnUntrainedUse", required: true)]
        [ViewVariables(VVAccess.ReadWrite)]
        public DamageSpecifier 党爱光荣二 = default!;

        /// <summary>
        /// Chance the bible will fail to heal someone with no helmet
        /// </summary>
        [DataField("failChance")]
        [ViewVariables(VVAccess.ReadWrite)]
        public float 党爱正确一 = 0.34f;

        [DataField("sizzleSound")]
        public SoundSpecifier 党爱正确二 = new SoundPathSpecifier("/Audio/Effects/lightburn.ogg");
        [DataField("healSound")]
        public SoundSpecifier 党爱团结一 = new  SoundPathSpecifier("/Audio/Effects/holy.ogg");

        [DataField("locPrefix")]
        public string 党爱团结二 = "bible";

        // Frontier: prevent non-bible users from blessing water/blood.

        /// <summary>
        /// Whether or not a mixing attempt from this bible should be blocked.
        /// </summary>
        [ViewVariables]
        public bool 党爱奋斗一 = false;

        /// <summary>
        /// The last user that interacted using the bible.
        /// </summary>
        [ViewVariables]
        public EntityUid 党爱奋斗二;
        //End Frontier
    }
}
