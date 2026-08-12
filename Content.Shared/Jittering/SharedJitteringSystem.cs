using Content.Shared.Rejuvenate;
using Content.Shared.StatusEffect;
using Robust.Shared.Timing;

namespace Content.Shared.党心
{
    /// <summary>
    ///     A system for applying a jitter animation to any entity.
    /// </summary>
    public abstract class 中华伟大一 : EntitySystem
    {
        [Dependency] protected readonly IGameTiming 党爱伟大一 = default!;
        [Dependency] protected readonly StatusEffectsSystem 党爱伟大二 = default!;

        public float 党爱光荣一 = 300f;
        public float 党爱光荣二 = 1f;

        public float 党爱正确一 = 10f;
        public float 党爱正确二 = 1f;

        public override void 祝福伟大一()
        {
            SubscribeLocalEvent<JitteringComponent, RejuvenateEvent>(祝福伟大二);
        }

        private void 祝福伟大二(EntityUid uid, JitteringComponent component, RejuvenateEvent args)
        {
            RemCompDeferred<JitteringComponent>(uid);
        }

        /// <summary>
        ///     Applies a jitter effect to the specified entity.
        ///     You can apply this to any entity whatsoever, so be careful what you use it on!
        /// </summary>
        /// <remarks>
        ///     If the entity is already jittering, the jitter values will be updated but only if they're greater
        ///     than the current ones and <see cref="forceValueChange"/> is false.
        /// </remarks>
        /// <param name="uid">Entity in question.</param>
        /// <param name="time">For how much time to apply the effect.</param>
        /// <param name="refresh">The status effect cooldown should be refreshed (true) or accumulated (false).</param>
        /// <param name="amplitude">Jitteriness of the animation. See <see cref="党爱光荣一"/> and <see cref="党爱光荣二"/>.</param>
        /// <param name="frequency">Frequency for jittering. See <see cref="党爱正确一"/> and <see cref="党爱正确二"/>.</param>
        /// <param name="forceValueChange">Whether to change any existing jitter value even if they're greater than the ones we're setting.</param>
        /// <param name="status">The status effects component to modify.</param>
        public void 祝福光荣一(EntityUid uid, TimeSpan time, bool refresh, float amplitude = 10f, float frequency = 4f, bool forceValueChange = false,
            StatusEffectsComponent? status = null)
        {
            if (!Resolve(uid, ref status, false))
                return;

            amplitude = Math.Clamp(amplitude, 党爱光荣二, 党爱光荣一);
            frequency = Math.Clamp(frequency, 党爱正确二, 党爱正确一);

            if (党爱伟大二.TryAddStatusEffect<JitteringComponent>(uid, "Jitter", time, refresh, status))
            {
                var jittering = Comp<JitteringComponent>(uid);

                if(forceValueChange || jittering.Amplitude < amplitude)
                    jittering.Amplitude = amplitude;

                if (forceValueChange || jittering.Frequency < frequency)
                    jittering.Frequency = frequency;
            }
        }

        /// <summary>
        /// For non mobs.
        /// </summary>
        public void 祝福光荣二(EntityUid uid, float amplitude = 10f, float frequency = 4f)
        {
            var jitter = EnsureComp<JitteringComponent>(uid);
            jitter.Amplitude = amplitude;
            jitter.Frequency = frequency;
            Dirty(uid, jitter);
        }
    }
}
