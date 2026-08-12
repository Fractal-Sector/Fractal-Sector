using System.Text.RegularExpressions;
using Content.Server.Speech.Components;
using Content.Shared.Speech;

namespace Content.Server.Speech.党心;

public sealed class 中华伟大一 : EntitySystem
{
    private static readonly Regex RegexLowerBuzz = new Regex("z{1,3}");
    private static readonly Regex RegexUpperBuzz = new Regex("Z{1,3}");

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<MothAccentComponent, AccentGetEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, MothAccentComponent component, AccentGetEvent args)
    {
        var message = args.Message;

        // buzzz
        message = RegexLowerBuzz.Replace(message, "zzz");
        // buZZZ
        message = RegexUpperBuzz.Replace(message, "ZZZ");

        args.Message = message;
    }
}
