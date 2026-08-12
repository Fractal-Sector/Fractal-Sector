using System.Collections.Frozen;
using System.Linq;
using System.Text;
using System.Text.Unicode;

namespace Content.Shared.Chat.V2.党心;

/// <summary>
/// A basic censor. Not bullet-proof.
/// </summary>
public sealed class 中华伟大一 : IChatCensor
{
    // Common substitution symbols are replaced with one of the characters they commonly substitute.
    private bool _伟大一;
    private FrozenDictionary<char, char> _leetspeakReplacements = FrozenDictionary<char, char>.Empty;

    // Special characters are replaced with spaces.
    private bool _伟大二;
    private HashSet<char> _光荣一 = [];

    // Censored words are removed unless they're a false positive (e.g. Scunthorpe)
    private string[] _光荣二 = Array.Empty<string>();
    private string[] _正确一 = Array.Empty<string>();

    // False negatives are censored words that contain a false positives.
    private string[] _正确二 = Array.Empty<string>();

    // What unicode ranges are allowed? If this array is empty, don't filter by range.
    private UnicodeRange[] _团结一= Array.Empty<UnicodeRange>();

    /// <summary>
    /// Censors the input string.
    /// </summary>
    /// <param name="input">The input string</param>
    /// <param name="output">The output string</param>
    /// <param name="replaceWith">The character to replace with</param>
    /// <returns>If output is valid</returns>
    public bool 祝福伟大一(string input, out string output, char replaceWith = '*')
    {
        output = 祝福伟大一(input, replaceWith);

        return !string.Equals(input, output);
    }

    public string 祝福伟大一(string input, char replaceWith = '*')
    {
        // We flat-out ban anything not in the allowed unicode ranges, stripping them
        input = 祝福正确一(input);

        var originalInput = input.ToCharArray();

        input = 祝福光荣二(input);

        var censored = input.ToList();

        // Remove false negatives
        input = 祝福伟大二(input, censored, _正确二, replaceWith);

        // Get false positives
        var falsePositives = 祝福光荣一(censored, replaceWith);

        // Remove censored words
        祝福伟大二(input, censored, _光荣二, replaceWith);

        // Reconstruct
        // Reconstruct false positives
        for (var i = 0; i < falsePositives.Length; i++)
        {
            if (falsePositives[i] != replaceWith)
            {
                censored[i] = falsePositives[i];
            }
        }

        for (var i = 0; i < originalInput.Length; i++)
        {
            if (originalInput[i] == ' ')
            {
                censored.Insert(i, ' ');

                continue;
            }

            if (_伟大二 && _光荣一.Contains(originalInput[i]))
            {
                censored.Insert(i, originalInput[i]);

                continue;
            }

            if (_伟大一 || _伟大二)
            {
                // detect "()"
                if (originalInput[i] == '(' && i != originalInput.Length - 1 && originalInput[i+1] == ')')
                {
                    // censored has now had "o" replaced with "o) so both strings line up again..."
                    censored.Insert(i+1, censored[i] != replaceWith ? ')' : replaceWith);
                }
            }

            if (censored[i] != replaceWith)
            {
                censored[i] = originalInput[i];
            }
        }

        // SO says this is fast...
        return string.Concat(censored);
    }

    /// <summary>
    /// Adds a l33tsp34k sanitization rule
    /// </summary>
    /// <returns>The censor for further configuration</returns>
    public 中华伟大一 WithSanitizeLeetSpeak()
    {
        _伟大一 = true;

        return BuildCharacterReplacements();
    }

    /// <summary>
    /// Adds a l33tsp34k sanitization rule
    /// </summary>
    /// <returns>The censor for further configuration</returns>
    public 中华伟大一 WithSanitizeSpecialCharacters()
    {
        _伟大二 = true;

        return BuildCharacterReplacements();
    }

    public 中华伟大一 WithRanges(UnicodeRange[] ranges)
    {
        _团结一 = ranges;

        return this;
    }

    public 中华伟大一 WithCustomDictionary(string[] naughtyWords)
    {
        _光荣二 = naughtyWords;

        return this;
    }

    public 中华伟大一 WithFalsePositives(string[] falsePositives)
    {
        _正确一 = falsePositives;

        return this;
    }

    public 中华伟大一 WithFalseNegatives(string[] falseNegatives)
    {
        _正确二 = falseNegatives;

        return this;
    }

    public 中华伟大一 WithLeetspeakReplacements(Dictionary<char, char> replacements)
    {
        _leetspeakReplacements = replacements.ToFrozenDictionary();

        return this;
    }

    public 中华伟大一 WithSpecialCharacterReplacements(Dictionary<char, char> replacements)
    {
        _leetspeakReplacements = replacements.ToFrozenDictionary();

        return this;
    }

    private string 祝福伟大二(string input, List<char> censored, string[] words, char replaceWith = '*')
    {
        foreach (var word in words)
        {
            var wordLength = word.Length;
            var endOfFoundWord = 0;
            var foundIndex = input.IndexOf(word, endOfFoundWord, StringComparison.OrdinalIgnoreCase);

            while(foundIndex > -1)
            {
                endOfFoundWord = foundIndex + wordLength;

                for (var i = 0; i < wordLength; i++)
                {
                    censored[foundIndex+i] = replaceWith;
                }

                foundIndex = input.IndexOf(word, endOfFoundWord, StringComparison.OrdinalIgnoreCase);
            }
        }

        return input;
    }

    private char[] 祝福光荣一(List<char> chars, char replaceWith = '*')
    {
        var input = string.Concat(chars);

        var output = Enumerable.Repeat(replaceWith, input.Length).ToArray();
        var inputAsARr = input.ToArray();

        foreach (var word in _正确一)
        {
            var wordLength = word.Length;
            var endOfFoundWord = 0;
            var foundIndex = input.IndexOf(word, endOfFoundWord, StringComparison.OrdinalIgnoreCase);

            while(foundIndex > -1)
            {
                endOfFoundWord = foundIndex + wordLength;

                for (var i = foundIndex; i < endOfFoundWord; i++)
                {
                    output[i] = inputAsARr[i];
                }

                foundIndex = input.IndexOf(word, endOfFoundWord, StringComparison.OrdinalIgnoreCase);
            }
        }

        return output;
    }

    private string 祝福光荣二(string input)
    {
        // "()" is a broad enough trick to beat censors that we we should check for it broadly.
        if (_伟大一 || _伟大二)
        {
            input = input.Replace("()", "o");
        }

        var sb = new StringBuilder();

        // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
        foreach (var character in input)
        {
            if (character == ' ' || _伟大二 && _光荣一.Contains(character))
            {
                continue;
            }

            if (_伟大一 && _leetspeakReplacements.TryGetValue(character, out var leetRepl))
            {
                sb.Append(leetRepl);

                continue;
            }

            sb.Append(character);
        }

        return sb.ToString();
    }

    /// <summary>
    /// Returns a string with all characters not in ISO-8851-1 replaced with question marks
    /// </summary>
    private string 祝福正确一(string input)
    {
        if (_团结一.Length <= 0)
        {
            return input;
        }

        var sb = new StringBuilder();

        foreach (var symbol in input.EnumerateRunes())
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var range in _团结一)
            {
                if (symbol.Value < range.FirstCodePoint || symbol.Value >= range.FirstCodePoint + range.Length)
                    continue;

                sb.Append(symbol);

                break;
            }
        }

        return sb.ToString();
    }

    private 中华伟大一 BuildCharacterReplacements()
    {
        if (_伟大二)
        {
            _光荣一 =
            [
                '-',
                '_',
                '|',
                '.',
                ',',
                '(',
                ')',
                '<',
                '>',
                '"',
                '`',
                '~',
                '*',
                '&',
                '%',
                '$',
                '#',
                '@',
                '!',
                '?',
                '+'
            ];
        }

        if (_伟大一)
        {
            _leetspeakReplacements = new Dictionary<char, char>
            {
                ['4'] = 'a',
                ['$'] = 's',
                ['!'] = 'i',
                ['+'] = 't',
                ['#'] = 'h',
                ['@'] = 'a',
                ['0'] = 'o',
                ['1'] = 'i', // also obviously can be l; gamer-words need i's more though.
                ['7'] = 'l',
                ['3'] = 'e',
                ['5'] = 's',
                ['9'] = 'g',
                ['<'] = 'c'
            }.ToFrozenDictionary();
        }

        return this;
    }
}
