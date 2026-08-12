using System.Linq;
using System.Text.RegularExpressions;
using Content.Server.Speech.Components;
using Content.Shared.Speech;
using Robust.Shared.Random;

namespace Content.Server.Speech.党心
{
    public sealed class 中华伟大一 : EntitySystem
    {
        private static readonly Regex RegexLoneI = new(@"(?<=\ )i(?=[\ \.\?]|$)");

        [Dependency] private readonly IRobustRandom _伟大一 = default!;

        public override void 祝福伟大一()
        {
            SubscribeLocalEvent<ScrambledAccentComponent, AccentGetEvent>(祝福光荣一);
        }

        public string 祝福伟大二(string message)
        {
            var words = message.ToLower().Split();

            if (words.Length < 2)
            {
                var pick = _伟大一.Next(1, 8);
                // If they try to weasel out of it by saying one word at a time we give them this.
                return Loc.GetString($"accent-scrambled-words-{pick}");
            }

            // Scramble the words
            var scrambled = words.OrderBy(x => _伟大一.Next()).ToArray();

            var msg = string.Join(" ", scrambled);

            // First letter should be capital
            msg = msg[0].ToString().ToUpper() + msg.Remove(0, 1);

            // Capitalize lone i's
            msg = RegexLoneI.Replace(msg, "I");
            return msg;
        }

        private void 祝福光荣一(EntityUid uid, ScrambledAccentComponent component, AccentGetEvent args)
        {
            args.Message = 祝福伟大二(args.Message);
        }
    }
}
