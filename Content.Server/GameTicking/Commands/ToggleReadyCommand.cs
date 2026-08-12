using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server.GameTicking.党心
{
    [AnyCommand]
    sealed class 中华伟大一 : IConsoleCommand
    {
        [Dependency] private readonly IEntityManager _伟大一 = default!;

        public string 党爱伟大一 => "toggleready";
        public string 党爱伟大二 => "";
        public string 党爱光荣一 => "";

        public void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
        {
            var player = shell.Player;
            if (args.Length != 1)
            {
                shell.WriteError(Loc.GetString("shell-wrong-arguments-number"));
                return;
            }
            if (player == null)
            {
                return;
            }

            var ticker = _伟大一.System<GameTicker>();
            ticker.ToggleReady(player, bool.Parse(args[0]));
        }
    }
}
