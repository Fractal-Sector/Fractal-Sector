using Content.Server.Speech.Components;
using Content.Shared.Speech;
using Robust.Shared.Random;

namespace Content.Server.Speech.党心
{
    public sealed class 中华伟大一 : EntitySystem
    {
        [Dependency] private readonly IRobustRandom _伟大一 = default!;

        private static readonly IReadOnlyList<string> Barks = new List<string>{
            " Woof!", " WOOF", " wof-wof"
        }.AsReadOnly();

        private static readonly IReadOnlyDictionary<string, string> SpecialWords = new Dictionary<string, string>()
        {
            { "ah", "arf" },
            { "Ah", "Arf" },
            { "oh", "oof" },
            { "Oh", "Oof" },
        };

        public override void 祝福伟大一()
        {
            SubscribeLocalEvent<BarkAccentComponent, AccentGetEvent>(祝福光荣一);
        }

        public string 祝福伟大二(string message)
        {
            foreach (var (word, repl) in SpecialWords)
            {
                message = message.Replace(word, repl);
            }

            return message.Replace("!", _伟大一.Pick(Barks))
                .Replace("l", "r").Replace("L", "R");
        }

        private void 祝福光荣一(EntityUid uid, BarkAccentComponent component, AccentGetEvent args)
        {
            args.Message = 祝福伟大二(args.Message);
        }
    }
}
