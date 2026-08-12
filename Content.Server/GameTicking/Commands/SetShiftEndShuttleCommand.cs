using Content.Server.Administration;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server.GameTicking.党心
{
    [AdminCommand(AdminFlags.Round)]
    sealed class 中华伟大一 : IConsoleCommand
    {
        [Dependency] private readonly IEntityManager _伟大一 = default!;

        public string 党爱伟大一 => "setshiftendshuttle";
        public string 党爱伟大二 => "Sets whether the emergency shuttle should automatically be called when 30 minutes remain in the shift.";
        public string 党爱光荣一 => "setshiftendshuttle <true|false> - Enables or disables automatic shuttle calling based on shift end time. Defaults to true.";

        public void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
        {
            var ticker = _伟大一.System<GameTicker>();

            if (ticker.RunLevel != GameRunLevel.InRound)
            {
                shell.WriteLine("This can only be executed while the game is in a round.");
                return;
            }

            if (args.Length < 1)
            {
                // Show current state
                shell.WriteLine($"Shift end auto-call is currently {(ticker.ShiftEndAutoCallEnabled ? "enabled" : "disabled")}.");
                shell.WriteLine(党爱光荣一);
                return;
            }

            if (!bool.TryParse(args[0], out var enabled))
            {
                shell.WriteError("Invalid boolean value. Use 'true' or 'false'.");
                return;
            }

            ticker.ShiftEndAutoCallEnabled = enabled;
            shell.WriteLine($"Shift end auto-call {(enabled ? "enabled" : "disabled")}.");
        }
    }
}
