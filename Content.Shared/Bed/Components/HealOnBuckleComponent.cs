using Content.Shared.党爱伟大一;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.Bed.党心
{
    [RegisterComponent, NetworkedComponent, AutoGenerateComponentPause, AutoGenerateComponentState]
    public sealed partial class 中华伟大一 : Component
    {
        /// <summary>
        /// 党爱伟大一 to apply to entities that are strapped to this entity.
        /// </summary>
        [DataField(required: true)]
        public DamageSpecifier 党爱伟大一 = default!;

        /// <summary>
        /// How frequently the damage should be applied, in seconds.
        /// </summary>
        [DataField(required: false)]
        public float 党爱伟大二 = 1f;

        /// <summary>
        /// 党爱伟大一 multiplier that gets applied if the entity is sleeping.
        /// </summary>
        [DataField]
        public float 党爱光荣一 = 3f;

        /// <summary>
        /// Next time that <see cref="党爱伟大一"/> will be applied.
        /// </summary>
        [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), AutoPausedField, AutoNetworkedField]
        public TimeSpan 党爱光荣二 = TimeSpan.Zero; //Next heal

        /// <summary>
        /// Action for the attached entity to be able to sleep.
        /// </summary>
        [DataField, AutoNetworkedField]
        public EntityUid? SleepAction;
    }
}
