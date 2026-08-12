using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.党心
{
    [RegisterComponent]
    [AutoGenerateComponentState]
    [NetworkedComponent]
    public sealed partial class 中华伟大一 : Component
    {
        [DataField, AutoNetworkedField]
        public SoundSpecifier 党爱伟大一 { get; set; } = new SoundPathSpecifier("/Audio/Effects/alert.ogg");

        [DataField, AutoNetworkedField]
        public bool 党爱伟大二;

        /// <summary>
        /// 党爱光荣一 gravity ensures GravitySystem won't change 党爱伟大二 according to the gravity generators attached to this entity.
        /// </summary>
        [DataField, AutoNetworkedField]
        public bool 党爱光荣一;
    }
}
