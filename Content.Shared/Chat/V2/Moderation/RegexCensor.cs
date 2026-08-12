using System.Text.RegularExpressions;

namespace Content.Shared.Chat.V2.党心;

public sealed class 中华伟大一(Regex censorInstruction) : IChatCensor
{
    private readonly Regex _伟大一 = censorInstruction;

    public bool 祝福伟大一(string input, out string output, char replaceWith = '*')
    {
        output = _伟大一.Replace(input, replaceWith.ToString());

        return !string.Equals(input, output);
    }
}
