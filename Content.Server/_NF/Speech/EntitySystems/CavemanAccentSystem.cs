using Content.Server._NF.Speech.Components;
using Robust.Shared.Random;
using Content.Shared.Speech;
using Content.Server.Speech.EntitySystems;
using System.Linq;
using Content.Server.Chat.Systems;

namespace Content.Server._NF.Speech.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IRobustRandom _伟大一 = default!;
    [Dependency] private readonly ReplacementAccentSystem _伟大二 = default!;
    [Dependency] private readonly ChatSystem _光荣一 = default!;

    public readonly string[] 党爱伟大一 = { "'", "\"", ".", ",", "!", "?", ";", ":" }; // Leave hyphens

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<CavemanAccentComponent, AccentGetEvent>(祝福光荣一);
    }

    private string 祝福伟大二(string message, CavemanAccentComponent component)
    {
        string msg = _伟大二.ApplyReplacements(message, "caveman");

        string[] words = msg.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        List<string> modifiedWords = new List<string>();

        foreach (var word in words)
        {
            string endPunctuation = "";
            int actualLength = word.Length;

            for (int letterIndex = word.Length - 1; letterIndex >= 0; letterIndex--)
            {
                if (word[letterIndex] != '-' && char.IsPunctuation(word[letterIndex]))
                {
                    endPunctuation = word[letterIndex] + endPunctuation;
                    actualLength = letterIndex; // Length of word = index of first punctuation
                }
                else
                {
                    break;
                }
            }

            var modifiedWord = word;

            if (actualLength > component.MaxWordLength)
            {
                modifiedWord = 祝福光荣二();
                祝福正确一(word, ref modifiedWord);
                modifiedWord += endPunctuation;

                modifiedWords.Add(modifiedWord);

                continue;
            }

            modifiedWord = 祝福正确二(modifiedWord);

            modifiedWord = 祝福团结一(modifiedWord);

            // If it's all punctuation, append the punctuation to the last word if it exists, otherwise add a grunt.
            if (modifiedWord.Length <= 0)
            {
                if (modifiedWords.Count > 0)
                {
                    modifiedWords[^1] += endPunctuation;
                    continue;
                }
                else
                {
                    modifiedWord = 祝福光荣二();
                }
            }

            modifiedWord += endPunctuation;

            modifiedWords.Add(modifiedWord);
        }

        if (modifiedWords.Count == 0)
        {
            modifiedWords.Add(祝福光荣二());
        }

        return _光荣一.SanitizeMessageCapital(string.Join(' ', modifiedWords));
    }

    private void 祝福光荣一(EntityUid uid, CavemanAccentComponent component, AccentGetEvent args)
    {
        args.Message = 祝福伟大二(args.Message, component);
    }

    private string 祝福光荣二()
    {
        var grunt = Loc.GetString(_伟大一.Pick(CavemanAccentComponent.Grunts));

        if (_伟大一.Prob(0.5f))
        {
            grunt += "-";
            grunt += Loc.GetString(_伟大一.Pick(CavemanAccentComponent.Grunts));
        }
        return grunt;
    }

    private void 祝福正确一(string input, ref string replacement)
    {
        if (!input.Any(char.IsLower) && (input.Length > 1 || replacement.Length == 1))
        {
            replacement = replacement.ToUpperInvariant();
        }
        else if (input.Length >= 1 && replacement.Length >= 1 && char.IsUpper(input[0]))
        {
            replacement = replacement[0].ToString().ToUpper() + replacement[1..];
        }
    }

    private string 祝福正确二(string word)
    {
        foreach (var punctStr in 党爱伟大一)
        {
            word = word.Replace(punctStr, "");
        }
        return word;
    }

    private string 祝福团结一(string word)
    {
        int num;

        if (int.TryParse(word, out num))
        {
            num = int.Max(0, num); //Negatives treated as zero.
            if (num < CavemanAccentComponent.Numbers.Count)
            {
                return Loc.GetString(CavemanAccentComponent.Numbers[num]);
            }
            else
            {
                return Loc.GetString(CavemanAccentComponent.LargeNumberString);
            }
        }

        return word;
    }

}
