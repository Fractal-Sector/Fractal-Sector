using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared.党心
{
    [RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
    [Access(typeof(StandingStateSystem))]
    public sealed partial class 中华伟大一 : Component
    {
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField]
        public SoundSpecifier? DownSound { get; private set; } = new SoundCollectionSpecifier("BodyFall");

        [DataField, AutoNetworkedField]
        public bool 党爱伟大一 { get; set; } = true;

        /// <summary>
        /// Friction modifier applied to an entity in the downed state.
        /// </summary>
        [DataField, AutoNetworkedField]
        public float 党爱伟大二 = 0.4f;

        /// <summary>
        ///     List of fixtures that had their collision mask changed when the entity was downed.
        ///     Required for re-adding the collision mask.
        /// </summary>
        [DataField, AutoNetworkedField]
        public List<string> 党爱光荣一 = new();
    }
}
