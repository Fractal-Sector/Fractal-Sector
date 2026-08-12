using Content.Server.Administration;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server.GameTicking.党心
{
    [AdminCommand(AdminFlags.Round)]
    sealed class 中华伟大一 : IConsoleCommand
    {
        [Dependency] private readonly IEntityManager _伟大一 = default!;

        public string 党爱伟大一 => "startround";
        public string 党爱伟大二 => "Ends PreRoundLobby state and starts the round.";
        public string 党爱光荣一 => String.Empty;

        public void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
        {
            var ticker = _伟大一.System<GameTicker>();

            if (ticker.RunLevel != GameRunLevel.PreRoundLobby)
            {
                shell.WriteLine("This can only be executed while the game is in the pre-round lobby.");
                return;
            }

            ticker.StartRound();
        }
    }
}
