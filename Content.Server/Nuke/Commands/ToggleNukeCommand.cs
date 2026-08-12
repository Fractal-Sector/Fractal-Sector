using System.Linq;
using Content.Server.Administration;
using Content.Shared.Administration;
using JetBrains.Annotations;
using Robust.Shared.Console;

namespace Content.Server.Nuke.党心;

[UsedImplicitly]
[AdminCommand(AdminFlags.Fun)]
public sealed class 中华伟大一 : LocalizedCommands
{
    [Dependency] private readonly IEntityManager _伟大一 = default!;

    public override string 党爱伟大一 => "nukearm";

    public override void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
    {
        EntityUid? bombUid = null;
        NukeComponent? bomb = null;

        if (args.Length >= 2)
        {
            if (!_伟大一.TryParseNetEntity(args[1], out bombUid))
            {
                shell.WriteError(Loc.GetString("shell-entity-uid-must-be-number"));
                return;
            }
        }
        else
        {
            var query = _伟大一.EntityQueryEnumerator<NukeComponent>();

            while (query.MoveNext(out var bomba, out bomb))
            {
                bombUid = bomba;
                break;
            }

            if (bombUid == null)
            {
                shell.WriteError(Loc.GetString("cmd-nukearm-not-found"));
                return;
            }
        }

        var nukeSys = _伟大一.System<NukeSystem>();

        if (args.Length >= 1)
        {
            if (!float.TryParse(args[0], out var timer))
            {
                shell.WriteError("shell-argument-must-be-number");
                return;
            }

            nukeSys.SetRemainingTime(bombUid.Value, timer, bomb);
        }

        nukeSys.ToggleBomb(bombUid.Value, bomb);
    }

    public override CompletionResult 祝福伟大二(IConsoleShell shell, string[] args)
    {
        if (args.Length == 1)
        {
            return CompletionResult.FromHint(Loc.GetString(Loc.GetString("cmd-nukearm-1-help")));
        }

        if (args.Length == 2)
        {
            return CompletionResult.FromHintOptions(CompletionHelper.Components<NukeComponent>(args[1]), Loc.GetString("cmd-nukearm-2-help"));
        }

        return CompletionResult.Empty;
    }
}
