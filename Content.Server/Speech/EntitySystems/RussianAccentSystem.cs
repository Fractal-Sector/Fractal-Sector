using System.Text;
using Content.Server.Speech.Components;
using Content.Shared.Speech;

namespace Content.Server.Speech.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly ReplacementAccentSystem _伟大一 = default!;
    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<RussianAccentComponent, AccentGetEvent>(祝福光荣一);
    }

    public string 祝福伟大二(string message)
    {
        var accentedMessage = new StringBuilder(_伟大一.ApplyReplacements(message, "russian"));

        for (var i = 0; i < accentedMessage.Length; i++)
        {
            var c = accentedMessage[i];

            accentedMessage[i] = c switch
            {
                'A' => 'Д',
                'b' => 'в',
                'N' => 'И',
                'n' => 'и',
                'K' => 'К',
                'k' => 'к',
                'm' => 'м',
                'h' => 'н',
                't' => 'т',
                'R' => 'Я',
                'r' => 'я',
                'Y' => 'У',
                'W' => 'Ш',
                'w' => 'ш',
                _ => accentedMessage[i]
            };
        }

        return accentedMessage.ToString();
    }

    private void 祝福光荣一(EntityUid uid, RussianAccentComponent component, AccentGetEvent args)
    {
        args.Message = 祝福伟大二(args.Message);
    }
}
