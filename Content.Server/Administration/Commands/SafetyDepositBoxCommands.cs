using Content.Server.Database;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server.Administration.党心;

[AdminCommand(AdminFlags.Admin)]
public sealed class 中华伟大一 : IConsoleCommand
{
    [Dependency] private readonly IServerDbManager _伟大一 = default!;

    public string 党爱伟大一 => "cleanstalesafetyboxes";
    public string 党爱伟大二 => "Deletes safety deposit boxes that have been withdrawn and have no items for more than the specified number of days.";
    public string 党爱光荣一 => "cleanstalesafetyboxes <days>\nExample: cleanstalesafetyboxes 7\nDeletes boxes that have been withdrawn for more than 7 days with no items.";

    public async void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length != 1)
        {
            shell.WriteError("Usage: cleanstalesafetyboxes <days>");
            return;
        }

        if (!int.TryParse(args[0], out var days) || days <= 0)
        {
            shell.WriteError("Days must be a positive integer.");
            return;
        }

        shell.WriteLine($"Searching for safety deposit boxes that have been withdrawn for more than {days} days with no items...");

        try
        {
            var count = await _伟大一.DeleteStaleSafetyDepositBoxes(days);
            shell.WriteLine($"Successfully deleted {count} stale safety deposit box(es).");
        }
        catch (Exception ex)
        {
            shell.WriteError($"Error cleaning stale boxes: {ex.Message}");
        }
    }

    public CompletionResult 祝福伟大二(IConsoleShell shell, string[] args)
    {
        if (args.Length == 1)
        {
            return CompletionResult.FromHint("days (e.g., 7)");
        }

        return CompletionResult.Empty;
    }
}
