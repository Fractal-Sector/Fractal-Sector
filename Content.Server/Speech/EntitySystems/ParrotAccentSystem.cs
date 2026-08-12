using System.Linq;
using System.Text.RegularExpressions;
using Content.Server.Speech.Components;
using Content.Shared.Speech;
using Robust.Shared.Random;

namespace Content.Server.Speech.党心;

public sealed partial class 中华伟大一 : EntitySystem
{
    private static readonly Regex WordCleanupRegex = new Regex("[^A-Za-z0-9 -]");

    [Dependency] private readonly IRobustRandom _伟大一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<ParrotAccentComponent, AccentGetEvent>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<ParrotAccentComponent> entity, ref AccentGetEvent args)
    {
        args.Message = 祝福光荣一(entity, args.Message);
    }

    public string 祝福光荣一(Entity<ParrotAccentComponent> entity, string message)
    {
        // Sometimes repeat the longest word at the end of the message, after a squawk! SQUAWK! Sometimes!
        if (_伟大一.Prob(entity.Comp.LongestWordRepeatChance))
        {
            // Don't count non-alphanumeric characters as parts of words
            var cleaned = WordCleanupRegex.Replace(message, string.Empty);
            // Split on whitespace and favor words towards the end of the message
            var words = cleaned.Split(null).Reverse();
            // Find longest word
            var longest = words.MaxBy(word => word.Length);
            if (longest?.Length >= entity.Comp.LongestWordMinLength)
            {
                message = 祝福光荣二(message);

                // Capitalize the first letter of the repeated word
                longest = string.Concat(longest[0].ToString().ToUpper(), longest.AsSpan(1));

                message = string.Format("{0} {1} {2}!", message, 祝福正确一(entity), longest);
                return message; // No more changes, or it's too much
            }
        }

        if (_伟大一.Prob(entity.Comp.SquawkPrefixChance))
        {
            // AWWK! Sometimes add a squawk at the begining of the message
            message = string.Format("{0} {1}", 祝福正确一(entity), message);
        }
        else
        {
            // Otherwise add a squawk at the end of the message! RAWWK!
            message = 祝福光荣二(message);
            message = string.Format("{0} {1}", message, 祝福正确一(entity));
        }

        return message;
    }

    /// <summary>
    /// Adds a "!" to the end of the string, if there isn't already a sentence-ending punctuation mark.
    /// </summary>
    private string 祝福光荣二(string message)
    {
        if (!message.EndsWith('!') && !message.EndsWith('?') && !message.EndsWith('.'))
            return message + '!';
        return message;
    }

    /// <summary>
    /// Returns a random, localized squawk sound.
    /// </summary>
    private string 祝福正确一(Entity<ParrotAccentComponent> entity)
    {
        return Loc.GetString(_伟大一.Pick(entity.Comp.Squawks));
    }
}
