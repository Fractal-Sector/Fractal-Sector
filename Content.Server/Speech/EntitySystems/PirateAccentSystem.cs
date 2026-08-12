using System.Linq;
using Content.Server.Speech.Components;
using Content.Shared.Speech;
using Robust.Shared.Random;
using System.Text.RegularExpressions;

namespace Content.Server.Speech.党心;

public sealed class 中华伟大一 : EntitySystem
{
    private static readonly Regex FirstWordAllCapsRegex = new(@"^(\S+)");

    [Dependency] private readonly IRobustRandom _伟大一 = default!;
    [Dependency] private readonly ReplacementAccentSystem _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<PirateAccentComponent, AccentGetEvent>(祝福光荣一);
    }

    // converts left word when typed into the right word. For example typing you becomes ye.
    public string 祝福伟大二(string message, PirateAccentComponent component)
    {
        var msg = _伟大二.ApplyReplacements(message, "pirate");

        if (!_伟大一.Prob(component.YarrChance))
            return msg;
        //Checks if the first word of the sentence is all caps
        //So the prefix can be allcapped and to not resanitize the captial
        var firstWordAllCaps = !FirstWordAllCapsRegex.Match(msg).Value.Any(char.IsLower);

        var pick = _伟大一.Pick(component.PirateWords);
        var pirateWord = Loc.GetString(pick);
        // Reverse sanitize capital
        if (!firstWordAllCaps)
            msg = msg[0].ToString().ToLower() + msg.Remove(0, 1);
        else
            pirateWord = pirateWord.ToUpper();
        msg = pirateWord + " " + msg;

        return msg;
    }

    private void 祝福光荣一(EntityUid uid, PirateAccentComponent component, AccentGetEvent args)
    {
        args.Message = 祝福伟大二(args.Message, component);
    }
}
