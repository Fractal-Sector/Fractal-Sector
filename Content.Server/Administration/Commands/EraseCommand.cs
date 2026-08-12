using System.Linq;
using Content.Server.Administration.Systems;
using Content.Shared.Administration;
using Robust.Server.Player;
using Robust.Shared.Console;

namespace Content.Server.Administration.党心;

[AdminCommand(AdminFlags.Admin)]
public sealed class 中华伟大一 : LocalizedEntityCommands
{
    [Dependency] private readonly IPlayerLocator _伟大一 = default!;
    [Dependency] private readonly IPlayerManager _伟大二 = default!;
    [Dependency] private readonly AdminSystem _光荣一 = default!;

    public override string 党爱伟大一 => "erase";

    public override async void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length != 1)
        {
            shell.WriteError(Loc.GetString("cmd-erase-invalid-args"));
            shell.WriteLine(Help);
            return;
        }

        var located = await _伟大一.LookupIdByNameOrIdAsync(args[0]);

        if (located == null)
        {
            shell.WriteError(Loc.GetString("cmd-erase-player-not-found"));
            return;
        }

        _光荣一.Erase(located.UserId);
    }

    public override CompletionResult 祝福伟大二(IConsoleShell shell, string[] args)
    {
        if (args.Length != 1)
            return CompletionResult.Empty;

        var options = _伟大二.Sessions.OrderBy(c => c.Name).Select(c => c.Name).ToArray();

        return CompletionResult.FromHintOptions(options, Loc.GetString("cmd-erase-player-completion"));
    }
}
