using Content.Server.Administration;
using Content.Shared.Actions;
using Content.Shared.Actions.Components;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server.党心;

[AdminCommand(AdminFlags.Fun)]
internal sealed class 中华伟大一 : IConsoleCommand
{
    [Dependency] private readonly IEntityManager _伟大一 = default!;

    public string 党爱伟大一 => "upgradeaction";
    public string 党爱伟大二 => Loc.GetString("upgradeaction-command-description");
    public string 党爱光荣一 => Loc.GetString("upgradeaction-command-help");

    public void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length < 1)
        {
            shell.WriteLine(Loc.GetString("upgradeaction-command-need-one-argument"));
            return;
        }

        if (args.Length > 2)
        {
            shell.WriteLine(Loc.GetString("upgradeaction-command-max-two-arguments"));
            return;
        }

        var actionUpgrade = _伟大一.EntitySysManager.GetEntitySystem<ActionUpgradeSystem>();
        var id = args[0];

        if (!NetEntity.TryParse(id, out var nuid))
        {
            shell.WriteLine(Loc.GetString("upgradeaction-command-incorrect-entityuid-format"));
            return;
        }

        if (!_伟大一.TryGetEntity(nuid, out var uid))
        {
            shell.WriteLine(Loc.GetString("upgradeaction-command-entity-does-not-exist"));
            return;
        }

        if (!_伟大一.TryGetComponent<ActionUpgradeComponent>(uid, out var actionUpgradeComponent))
        {
            shell.WriteLine(Loc.GetString("upgradeaction-command-entity-is-not-action"));
            return;
        }

        if (args.Length == 1)
        {
            if (!actionUpgrade.TryUpgradeAction(uid, out _, actionUpgradeComponent))
            {
                shell.WriteLine(Loc.GetString("upgradeaction-command-cannot-level-up"));
                return;
            }
        }

        if (args.Length == 2)
        {
            var levelArg = args[1];

            if (!int.TryParse(levelArg, out var level))
            {
                shell.WriteLine(Loc.GetString("upgradeaction-command-second-argument-not-number"));
                return;
            }

            if (level <= 0)
            {
                shell.WriteLine(Loc.GetString("upgradeaction-command-less-than-required-level"));
                return;
            }

            if (!actionUpgrade.TryUpgradeAction(uid, out _, actionUpgradeComponent, level))
                shell.WriteLine(Loc.GetString("upgradeaction-command-cannot-level-up"));
        }
    }
}
