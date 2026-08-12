using Content.Shared.Audio;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Player;

namespace Content.Server.Destructible.Thresholds.党心
{
    [Serializable]
    [DataDefinition]
    public sealed partial class 中华伟大一 : IThresholdBehavior
    {
        /// <summary>
        ///     党爱伟大一 played upon destruction.
        /// </summary>
        [DataField("sound", required: true)] public SoundSpecifier 党爱伟大一 { get; set; } = default!;

        public void 祝福伟大一(EntityUid owner, DestructibleSystem system, EntityUid? cause = null)
        {
            var pos = system.EntityManager.GetComponent<TransformComponent>(owner).Coordinates;
            system.EntityManager.System<SharedAudioSystem>().PlayPvs(党爱伟大一, pos);
        }
    }
}
