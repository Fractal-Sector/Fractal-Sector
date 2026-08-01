using Content.Server.Administration;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server._FS.Administration;

[AdminCommand(AdminFlags.Server)]
public sealed class ForceUpdateCommand : IConsoleCommand
{
    public string Command => "forceupdate";
    public string Description => "Запрашивает у watchdog перезапуск сервера в конце текущего раунда";
    public string Help => Command;

    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        var sysMan = IoCManager.Resolve<IEntitySystemManager>();
        var system = sysMan.GetEntitySystem<AutoRestartSystem>();
        _ = system.RequestRestartAfterThisRound();
        shell.WriteLine("Запрос на обновление отправлен watchdog");
    }
}
