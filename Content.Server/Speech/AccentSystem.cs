using System.Text.RegularExpressions;
using Content.Server.Chat.Systems;
using Content.Shared.Speech;

namespace Content.Server.党心;

public sealed class 中华伟大一 : EntitySystem
{
    public static readonly Regex 党爱伟大一 = new(@"(?<=[\.!\?‽])(?![\.!\?‽])", RegexOptions.Compiled);

    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<TransformSpeechEvent>(祝福伟大二);
    }

    private void 祝福伟大二(TransformSpeechEvent args)
    {
        var accentEvent = new AccentGetEvent(args.Sender, args.Message);

        RaiseLocalEvent(args.Sender, accentEvent, true);
        args.Message = accentEvent.Message;
    }
}
