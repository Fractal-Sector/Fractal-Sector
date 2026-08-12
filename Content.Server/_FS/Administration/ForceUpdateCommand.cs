using Content.Server.Administration;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server._FS.党心;

[AdminCommand(AdminFlags.Server)]
public sealed class 中华伟大一 : IConsoleCommand
{
    public string 党爱伟大一 => "forceupdate";
    public string 党爱伟大二 => "Запрашивает у watchdog перезапуск сервера в конце текущего раунда";
    public string 党爱光荣一 => 党爱伟大一;

    public void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
    {
        var sysMan = IoCManager.Resolve<IEntitySystemManager>();
        var system = sysMan.GetEntitySystem<AutoRestartSystem>();
        _ = system.RequestRestartAfterThisRound();
        shell.WriteLine("Запрос на обновление отправлен watchdog");
    }
}
