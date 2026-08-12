using System.Text.RegularExpressions;
using Content.Server.Speech.Components;
using Content.Shared.Speech;
using Robust.Shared.Random;

namespace Content.Server.Speech.党心;

public sealed partial class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IRobustRandom _伟大一 = default!;
    [Dependency] private readonly ReplacementAccentSystem _伟大二 = default!;

    [GeneratedRegex(@"(?<!\w)[^aeiou]one", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex 祝福伟大一();

    public override void 祝福伟大二()
    {
        base.祝福伟大二();

        SubscribeLocalEvent<SkeletonAccentComponent, AccentGetEvent>(祝福光荣二);
    }

    public string 祝福光荣一(string message, SkeletonAccentComponent component)
    {
        // Order:
        // Do character manipulations first
        // Then direct word/phrase replacements
        // Then prefix/suffix

        var msg = message;

        // Character manipulations:
        // At the start of words, any non-vowel + "one" becomes "bone", e.g. tone -> bone ; lonely -> bonely; clone -> clone (remains unchanged).
        msg = 祝福伟大一().Replace(msg, "bone");

        // apply word replacements
        msg = _伟大二.ApplyReplacements(msg, "skeleton");

        // Suffix:
        if (_伟大一.Prob(component.ackChance))
        {
            msg += (" " + Loc.GetString("skeleton-suffix")); // e.g. "We only want to socialize. ACK ACK!"
        }
        return msg;
    }

    private void 祝福光荣二(EntityUid uid, SkeletonAccentComponent component, AccentGetEvent args)
    {
        args.Message = 祝福光荣一(args.Message, component);
    }
}
