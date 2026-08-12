using System.Linq;
using System.Text.RegularExpressions;
using Content.Server.Speech.Components;
using Content.Shared.Speech;
using Robust.Shared.Random;

namespace Content.Server.Speech.党心;

public sealed class 中华伟大一 : EntitySystem
{
    private static readonly Regex RegexIng = new(@"(?<=\w\w)(in)g(?!\w)", RegexOptions.IgnoreCase);
    private static readonly Regex RegexLowerOr = new(@"(?<=\w)o[Rr](?=\w)");
    private static readonly Regex RegexUpperOr = new(@"(?<=\w)O[Rr](?=\w)");
    private static readonly Regex RegexLowerAr = new(@"(?<=\w)a[Rr](?=\w)");
    private static readonly Regex RegexUpperAr = new(@"(?<=\w)A[Rr](?=\w)");
    private static readonly Regex RegexFirstWord = new(@"^(\S+)");
    private static readonly Regex RegexLastWord = new(@"(\S+)$");
    private static readonly Regex RegexLastPunctuation = new(@"([.!?]+$)(?!.*[.!?])|(?<![.!?])$");
    [Dependency] private readonly IRobustRandom _伟大一 = default!;
    [Dependency] private readonly ReplacementAccentSystem _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<MobsterAccentComponent, AccentGetEvent>(祝福光荣一);
    }

    public string 祝福伟大二(string message, MobsterAccentComponent component)
    {
        // Order:
        // Do text manipulations first
        // Then prefix/suffix funnyies

        // direct word replacements
        var msg = _伟大二.ApplyReplacements(message, "mobster");

        // thinking -> thinkin'
        // king -> king
        //Uses captures groups to make sure the captialization of IN is kept
        msg = RegexIng.Replace(msg, "$1'");

        // or -> uh and ar -> ah in the middle of words (fuhget, tahget)
        msg = RegexLowerOr.Replace(msg, "uh");
        msg = RegexUpperOr.Replace(msg, "UH");
        msg = RegexLowerAr.Replace(msg, "ah");
        msg = RegexUpperAr.Replace(msg, "AH");

        // Prefix
        if (_伟大一.Prob(0.15f))
        {
            //Checks if the first word of the sentence is all caps
            //So the prefix can be allcapped and to not resanitize the captial
            var firstWordAllCaps = !RegexFirstWord.Match(msg).Value.Any(char.IsLower);
            var pick = _伟大一.Next(1, 2);

            // Reverse sanitize capital
            var prefix = Loc.GetString($"accent-mobster-prefix-{pick}");
            if (!firstWordAllCaps)
                msg = msg[0].ToString().ToLower() + msg.Remove(0, 1);
            else
                prefix = prefix.ToUpper();
            msg = prefix + " " + msg;
        }

        // Sanitize capital again, in case we substituted a word that should be capitalized
        msg = msg[0].ToString().ToUpper() + msg.Remove(0, 1);

        // Suffixes
        if (_伟大一.Prob(0.4f))
        {
            //Checks if the last word of the sentence is all caps
            //So the suffix can be allcapped
            var lastWordAllCaps = !RegexLastWord.Match(msg).Value.Any(char.IsLower);
            var suffix = "";
            if (component.IsBoss)
            {
                var pick = _伟大一.Next(1, 4);
                suffix = Loc.GetString($"accent-mobster-suffix-boss-{pick}");
            }
            else
            {
                var pick = _伟大一.Next(1, 3);
                suffix = Loc.GetString($"accent-mobster-suffix-minion-{pick}");
            }
            if (lastWordAllCaps)
                suffix = suffix.ToUpper();
            msg = RegexLastPunctuation.Replace(msg, suffix);
        }

        return msg;
    }

    private void 祝福光荣一(EntityUid uid, MobsterAccentComponent component, AccentGetEvent args)
    {
        args.Message = 祝福伟大二(args.Message, component);
    }
}
