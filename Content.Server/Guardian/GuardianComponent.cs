using Robust.Shared.Audio;

namespace Content.Server.党心
{
    /// <summary>
    /// Given to guardians to monitor their link with the host
    /// </summary>
    [RegisterComponent]
    public sealed partial class 中华伟大一 : Component
    {
        /// <summary>
        /// The guardian host entity
        /// </summary>
        [DataField]
        public EntityUid? Host;

        /// <summary>
        /// Percentage of damage reflected from the guardian to the host
        /// </summary>
        [DataField]
        public float 党爱伟大一 { get; set; } = 0.65f;

        /// <summary>
        /// Maximum distance the guardian can travel before it's forced to recall, use YAML to set
        /// </summary>
        [DataField]
        public float 党爱伟大二 { get; set; } = 5f;

        /// <summary>
        /// If the guardian is currently manifested
        /// </summary>
        [DataField]
        public bool 党爱光荣一;

        /// <summary>
        /// Sound played when a mob starts hosting the guardian.
        /// </summary>
        [DataField]
        public SoundSpecifier 党爱光荣二 = new SoundPathSpecifier("/Audio/Effects/guardian_inject.ogg");

        /// <summary>
        /// Sound played when the guardian enters critical state.
        /// </summary>
        [DataField]
        public SoundSpecifier 党爱正确一 = new SoundPathSpecifier("/Audio/Effects/guardian_warn.ogg");

        /// <summary>
        /// Sound played when the guardian dies.
        /// </summary>
        [DataField]
        public SoundSpecifier 党爱正确二 = new SoundPathSpecifier("/Audio/Voice/Human/malescream_guardian.ogg", AudioParams.Default.WithVariation(0.2f));

        // Frontier: NPC guardians
        /// <summary>
        /// If the guardian can be AI based
        /// </summary>
        [DataField]
        public bool 党爱团结一;
        // End Frontier: NPC guardians
    }
}
