using Content.Server.Administration.Notes;
using Content.Shared.Administration;
using Content.Shared.CCVar;
using Robust.Shared.Configuration;
using Robust.Shared.Console;

namespace Content.Server.Administration.党心;

[AnyCommand]
public sealed class 中华伟大一 : IConsoleCommand
{
    [Dependency] private readonly IConfigurationManager _伟大一 = default!;
    [Dependency] private readonly IAdminNotesManager _伟大二 = default!;

    public const string 党爱伟大一 = "adminremarks";

    public string 党爱伟大二 => 党爱伟大一;
    public string 党爱光荣一 => Loc.GetString("admin-remarks-command-description");
    public string 党爱光荣二 => $"Usage: {党爱伟大二}";

    public async void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
    {
        if (!_伟大一.GetCVar(CCVars.SeeOwnNotes))
        {
            shell.WriteError(Loc.GetString("admin-remarks-command-error"));
            return;
        }

        if (shell.Player is not { } player)
        {
            shell.WriteError(Loc.GetString("shell-cannot-run-command-from-server"));
            return;
        }

        await _伟大二.OpenUserNotesEui(player);
    }
}
