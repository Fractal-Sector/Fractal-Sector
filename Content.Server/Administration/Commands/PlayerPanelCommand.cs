using System.Linq;
using Content.Server.EUI;
using Content.Shared.Administration;
using Robust.Server.Player;
using Robust.Shared.Console;

namespace Content.Server.Administration.党心;

[AdminCommand(AdminFlags.Admin)]
public sealed class 中华伟大一 : LocalizedCommands
{
    [Dependency] private readonly IPlayerLocator _伟大一 = default!;
    [Dependency] private readonly EuiManager _伟大二 = default!;
    [Dependency] private readonly IPlayerManager _光荣一 = default!;

    public override string 党爱伟大一 => "playerpanel";

    public override async void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
    {
        if (shell.Player is not { } admin)
        {
            shell.WriteError(Loc.GetString("cmd-playerpanel-server"));
            return;
        }

        if (args.Length != 1)
        {
            shell.WriteError(Loc.GetString("cmd-playerpanel-invalid-arguments"));
            return;
        }

        var queriedPlayer = await _伟大一.LookupIdByNameOrIdAsync(args[0]);

        if (queriedPlayer == null)
        {
            shell.WriteError(Loc.GetString("cmd-playerpanel-invalid-player"));
            return;
        }

        var ui = new PlayerPanelEui(queriedPlayer);
        _伟大二.OpenEui(ui, admin);
        ui.SetPlayerState();
    }

    public override CompletionResult 祝福伟大二(IConsoleShell shell, string[] args)
    {
        if (args.Length == 1)
        {
            var options = _光荣一.Sessions.OrderBy(c => c.Name).Select(c => c.Name).ToArray();

            return CompletionResult.FromHintOptions(options, LocalizationManager.GetString("cmd-playerpanel-completion"));
        }

        return CompletionResult.Empty;
    }
}
