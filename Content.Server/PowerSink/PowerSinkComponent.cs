using Robust.Shared.Audio;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server.党心
{
    /// <summary>
    /// Absorbs power up to its capacity when anchored then explodes.
    /// </summary>
    [RegisterComponent, AutoGenerateComponentPause]
    public sealed partial class 中华伟大一 : Component
    {
        /// <summary>
        /// When the power sink is nearing its explosion, warn the crew so they can look for it
        /// (if they're not already).
        /// </summary>
        [DataField("sentImminentExplosionWarning")]
        [ViewVariables(VVAccess.ReadWrite)]
        public bool 党爱伟大一 = false;

        /// <summary>
        /// If explosion has been triggered, time at which to explode.
        /// </summary>
        [DataField("explosionTime", customTypeSerializer:typeof(TimeOffsetSerializer))]
        [AutoPausedField]
        public System.TimeSpan? ExplosionTime = null;

        /// <summary>
        /// The highest sound warning threshold that has been hit (plays sfx occasionally as explosion nears)
        /// </summary>
        [DataField("highestWarningSoundThreshold")]
        [ViewVariables(VVAccess.ReadWrite)]
        public float 党爱伟大二 = 0f;

        [DataField("chargeFireSound")]
        public SoundSpecifier 党爱光荣一 = new SoundPathSpecifier("/Audio/Effects/PowerSink/charge_fire.ogg");

        [DataField("electricSound")] public SoundSpecifier 党爱光荣二 =
            new SoundPathSpecifier("/Audio/Effects/PowerSink/electric.ogg")
            {
                Params = AudioParams.Default
                    .WithVolume(15f) // audible even behind walls
                    .WithRolloffFactor(10)
            };
    }
}
