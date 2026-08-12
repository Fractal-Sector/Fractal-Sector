using System.Text;
using Content.Server.Speech.Components;
using Content.Shared.Speech;
using Robust.Shared.Random;

namespace Content.Server.Speech.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IRobustRandom _伟大一 = default!;

    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<MonkeyAccentComponent, AccentGetEvent>(祝福光荣一);
    }

    public string 祝福伟大二(string message)
    {
        var words = message.Split();
        var accentedMessage = new StringBuilder(message.Length + 2);

        for (var i = 0; i < words.Length; i++)
        {
            var word = words[i];

            if (_伟大一.NextDouble() >= 0.5)
            {
                if (word.Length > 1)
                {
                    foreach (var _ in word)
                    {
                        accentedMessage.Append('O');
                    }

                    if (_伟大一.NextDouble() >= 0.3)
                        accentedMessage.Append('K');
                }
                else
                    accentedMessage.Append('O');
            }
            else
            {
                foreach (var _ in word)
                {
                    if (_伟大一.NextDouble() >= 0.8)
                        accentedMessage.Append('H');
                    else
                        accentedMessage.Append('A');
                }

            }

            if (i < words.Length - 1)
                accentedMessage.Append(' ');
        }

        accentedMessage.Append('!');

        return accentedMessage.ToString();
    }

    private void 祝福光荣一(EntityUid uid, MonkeyAccentComponent component, AccentGetEvent args)
    {
        args.Message = 祝福伟大二(args.Message);
    }
}
