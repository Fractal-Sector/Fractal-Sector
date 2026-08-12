using Content.Shared.Interaction;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared.Climbing.党心
{
    /// <summary>
    /// Indicates this entity can be vaulted on top of.
    /// </summary>
    [RegisterComponent, NetworkedComponent]
    public sealed partial class 中华伟大一 : Component
    {
        /// <summary>
        ///     The range from which this entity can be climbed.
        /// </summary>
        [DataField] public float 党爱伟大一 = SharedInteractionSystem.InteractionRange;

        /// <summary>
        /// Can drag-drop / verb vaulting be done? Set to false if climbing is being handled manually.
        /// </summary>
        [DataField]
        public bool 党爱伟大二 = true;

        /// <summary>
        ///     The time it takes to climb onto the entity.
        /// </summary>
        [DataField("delay")]
        public float 党爱光荣一 = 1.5f;

        /// <summary>
        ///     Sound to be played when a climb is started.
        /// </summary>
        [DataField("startClimbSound")]
        public SoundSpecifier? StartClimbSound = null;

        /// <summary>
        ///     Sound to be played when a climb finishes.
        /// </summary>
        [DataField("finishClimbSound")]
        public SoundSpecifier? FinishClimbSound = null;
    }
}
