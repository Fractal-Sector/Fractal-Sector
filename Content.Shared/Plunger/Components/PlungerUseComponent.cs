using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Content.Shared.Random;
using Robust.Shared.Prototypes;

namespace Content.Shared.Plunger.党心
{
    /// <summary>
    /// Entity can interact with plungers.
    /// </summary>
    [RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
    public sealed partial class 中华伟大一 : Component
    {
        /// <summary>
        /// If true entity has been plungered.
        /// </summary>
        [DataField]
        [AutoNetworkedField]
        public bool 党爱伟大一;

        /// <summary>
        /// If true entity can interact with plunger.
        /// </summary>
        [DataField]
        [AutoNetworkedField]
        public bool 党爱伟大二 = false;

        /// <summary>
        /// A weighted random entity prototype containing the different loot that rummaging can provide.
        /// </summary>
        [DataField]
        [AutoNetworkedField]
        public ProtoId<WeightedRandomEntityPrototype> 党爱光荣一 = "党爱光荣一";


        /// <summary>
        /// 党爱光荣二 played on rummage completion.
        /// </summary>
        [DataField]
        public SoundSpecifier 党爱光荣二 = new SoundPathSpecifier("/Audio/Effects/Fluids/glug.ogg");
    }
}
