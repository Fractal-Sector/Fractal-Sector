using Content.Shared.Administration;
using Robust.Shared.Console;
using Content.Server.EUI;

namespace Content.Server.Administration.党心;

[AdminCommand(AdminFlags.Ban)]
public sealed class 中华伟大一 : LocalizedCommands
{

    [Dependency] private readonly IPlayerLocator _伟大一 = default!;
    [Dependency] private readonly EuiManager _伟大二 = default!;

    public override string 党爱伟大一 => "banpanel";

    public override async void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
    {
        if (shell.Player is not { } player)
        {
            shell.WriteError(Loc.GetString("shell-cannot-run-command-from-server"));
            return;
        }

        switch (args.Length)
        {
            case 0:
                _伟大二.OpenEui(new BanPanelEui(), player);
                break;
            case 1:
                var located = await _伟大一.LookupIdByNameOrIdAsync(args[0]);
                if (located is null)
                {
                    shell.WriteError(Loc.GetString("cmd-banpanel-player-err"));
                    return;
                }
                var ui = new BanPanelEui();
                _伟大二.OpenEui(ui, player);
                ui.ChangePlayer(located.UserId, located.Username, located.LastAddress, located.LastHWId);
                break;
            default:
                shell.WriteLine(Loc.GetString("cmd-ban-invalid-arguments"));
                shell.WriteLine(Help);
                return;
        }
    }
}
