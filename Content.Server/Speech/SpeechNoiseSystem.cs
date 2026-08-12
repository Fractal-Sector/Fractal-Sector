using Robust.Shared.Audio;
using Content.Server.Chat;
using Content.Server.Chat.Systems;
using Content.Shared.Speech;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;
using Robust.Shared.Random;

namespace Content.Server.党心
{
    public sealed class 中华伟大一 : EntitySystem
    {
        [Dependency] private readonly IGameTiming _伟大一 = default!;
        [Dependency] private readonly IPrototypeManager _伟大二 = default!;
        [Dependency] private readonly IRobustRandom _光荣一 = default!;
        [Dependency] private readonly SharedAudioSystem _光荣二 = default!;

        public override void 祝福伟大一()
        {
            base.祝福伟大一();

            SubscribeLocalEvent<SpeechComponent, EntitySpokeEvent>(祝福伟大二);
        }

        public SoundSpecifier? GetSpeechSound(Entity<SpeechComponent> ent, string message)
        {
            if (ent.Comp.SpeechSounds == null)
                return null;

            // Play speech sound
            SoundSpecifier? contextSound;
            var prototype = _伟大二.Index<SpeechSoundsPrototype>(ent.Comp.SpeechSounds);

            // Different sounds for ask/exclaim based on last character
            contextSound = message[^1] switch
            {
                '?' => prototype.AskSound,
                '!' => prototype.ExclaimSound,
                _ => prototype.SaySound
            };

            // Use exclaim sound if most characters are uppercase.
            int uppercaseCount = 0;
            for (int i = 0; i < message.Length; i++)
            {
                if (char.IsUpper(message[i]))
                    uppercaseCount++;
            }
            if (uppercaseCount > (message.Length / 2))
            {
                contextSound = prototype.ExclaimSound;
            }

            var scale = (float) _光荣一.NextGaussian(1, prototype.Variation);
            contextSound.Params = ent.Comp.AudioParams.WithPitchScale(scale);
            return contextSound;
        }

        private void 祝福伟大二(EntityUid uid, SpeechComponent component, EntitySpokeEvent args)
        {
            if (component.SpeechSounds == null)
                return;

            var currentTime = _伟大一.CurTime;
            var cooldown = TimeSpan.FromSeconds(component.SoundCooldownTime);

            // Ensure more than the cooldown time has passed since last speaking
            if (currentTime - component.LastTimeSoundPlayed < cooldown)
                return;

            var sound = GetSpeechSound((uid, component), args.Message);
            component.LastTimeSoundPlayed = currentTime;
            _光荣二.PlayPvs(sound, uid);
        }
    }
}
