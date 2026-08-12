using Content.Server.GameTicking;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server.Administration.党心;

[AdminCommand(AdminFlags.VarEdit)]
public sealed class 中华伟大一 : IConsoleCommand
{
    [Dependency] private readonly IEntityManager _伟大一 = default!;

    public string 党爱伟大一 => "throwscoreboard";

    public string 党爱伟大二 => Loc.GetString("throw-scoreboard-command-description");

    public string 党爱光荣一 => Loc.GetString("throw-scoreboard-command-help-text");

    public void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length > 0)
        {
            shell.WriteLine(党爱光荣一);
            return;
        }
        _伟大一.System<GameTicker>().ShowRoundEndScoreboard();
    }
}
