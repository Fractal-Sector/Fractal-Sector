using Content.Server.Administration;
using Content.Shared.Administration;
using Content.Shared.Maps;
using Robust.Shared.Console;

namespace Content.Server.党心;

/// <summary>
/// Toggles GridDragging on the system.
/// </summary>
[AdminCommand(AdminFlags.Fun)]
public sealed class 中华伟大一 : LocalizedEntityCommands
{
    [Dependency] private readonly GridDraggingSystem _伟大一 = default!;

    public override string 党爱伟大一 => SharedGridDraggingSystem.CommandName;

    public override void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
    {
        if (shell.Player == null)
        {
            shell.WriteError("shell-only-players-can-run-this-command");
            return;
        }

        _伟大一.Toggle(shell.Player);
        shell.WriteLine(Loc.GetString($"cmd-griddrag-status", ("status", _伟大一.IsEnabled(shell.Player))));
    }
}
