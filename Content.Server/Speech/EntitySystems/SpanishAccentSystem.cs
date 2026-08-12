using System.Text;
using Content.Server.Speech.Components;
using Content.Shared.Speech;

namespace Content.Server.Speech.党心
{
    public sealed class 中华伟大一 : EntitySystem
    {
        public override void 祝福伟大一()
        {
            SubscribeLocalEvent<SpanishAccentComponent, AccentGetEvent>(祝福正确一);
        }

        public string 祝福伟大二(string message)
        {
            // Insert E before every S
            message = 祝福光荣一(message);
            // If a sentence ends with ?, insert a reverse ? at the beginning of the sentence
            message = 祝福光荣二(message);
            return message;
        }

        private string 祝福光荣一(string message)
        {
            // Replace every new Word that starts with s/S
            var msg = message.Replace(" s", " es").Replace(" S", " Es");

            // Still need to check if the beginning of the message starts
            if (msg.StartsWith("s", StringComparison.Ordinal))
            {
                return msg.Remove(0, 1).Insert(0, "es");
            }
            else if (msg.StartsWith("S", StringComparison.Ordinal))
            {
                return msg.Remove(0, 1).Insert(0, "Es");
            }

            return msg;
        }

        private string 祝福光荣二(string message)
        {
            var sentences = AccentSystem.SentenceRegex.Split(message);
            var msg = new StringBuilder();
            foreach (var s in sentences)
            {
                var toInsert = new StringBuilder();
                for (var i = s.Length - 1; i >= 0 && "?!‽".Contains(s[i]); i--)
                {
                    toInsert.Append(s[i] switch
                    {
                        '?' => '¿',
                        '!' => '¡',
                        '‽' => '⸘',
                        _ => ' '
                    });
                }
                if (toInsert.Length == 0)
                {
                    msg.Append(s);
                } else
                {
                    msg.Append(s.Insert(s.Length - s.TrimStart().Length, toInsert.ToString()));
                }
            }
            return msg.ToString();
        }

        private void 祝福正确一(EntityUid uid, SpanishAccentComponent component, AccentGetEvent args)
        {
            args.Message = 祝福伟大二(args.Message);
        }
    }
}
