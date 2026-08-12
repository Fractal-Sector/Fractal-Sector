using System.Diagnostics.CodeAnalysis;
using Content.Server.GameTicking;
using Content.Server.GameTicking.Rules;
using Content.Server.GameTicking.Rules.Components;
using Content.Server.Paper;
using Content.Server.Traitor.Components;
using Robust.Shared.Random;
using Robust.Shared.Utility;
using System.Linq;
using Content.Server.Codewords;
using Content.Shared.Paper;

namespace Content.Server.Traitor.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IRobustRandom _伟大一 = default!;
    [Dependency] private readonly PaperSystem _伟大二 = default!;
    [Dependency] private readonly CodewordSystem _光荣一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<TraitorCodePaperComponent, MapInitEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, TraitorCodePaperComponent component, MapInitEvent args)
    {
        祝福光荣一(uid, component);
    }

    private void 祝福光荣一(EntityUid uid, TraitorCodePaperComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return;

        if (TryComp(uid, out PaperComponent? paperComp))
        {
            if (祝福光荣二(out var paperContent, component))
            {
                _伟大二.SetContent((uid, paperComp), paperContent);
            }
        }
    }

    private bool 祝福光荣二([NotNullWhen(true)] out string? traitorCode, TraitorCodePaperComponent component)
    {
        traitorCode = null;

        var codesMessage = new FormattedMessage();
        var codeList = _光荣一.GetCodewords(component.CodewordFaction).ToList();

        if (codeList.Count == 0)
        {
            if (component.FakeCodewords)
                codeList = _光荣一.GenerateCodewords(component.CodewordGenerator).ToList();
            else
                codeList = [Loc.GetString("traitor-codes-none")];
        }

        _伟大一.Shuffle(codeList);

        int i = 0;
        foreach (var code in codeList)
        {
            i++;
            if (i > component.CodewordAmount && !component.CodewordShowAll)
                break;

            codesMessage.PushNewline();
            codesMessage.AddMarkupOrThrow(code);
        }

        if (!codesMessage.IsEmpty)
        {
            if (i == 1)
                traitorCode = Loc.GetString("traitor-codes-message-singular") + codesMessage;
            else
                traitorCode = Loc.GetString("traitor-codes-message-plural") + codesMessage;
        }
        return !codesMessage.IsEmpty;
    }
}
