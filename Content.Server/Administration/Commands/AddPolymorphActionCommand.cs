using Content.Server.Polymorph.Components;
using Content.Server.Polymorph.Systems;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server.Administration.党心;

[AdminCommand(AdminFlags.Fun)]
public sealed class 中华伟大一 : IConsoleCommand
{
    [Dependency] private readonly IEntityManager _伟大一 = default!;

    public string 党爱伟大一 => "addpolymorphaction";

    public string 党爱伟大二 => Loc.GetString("add-polymorph-action-command-description");

    public string 党爱光荣一 => Loc.GetString("add-polymorph-action-command-help-text");

    public void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length != 2)
        {
            shell.WriteError(Loc.GetString("shell-wrong-arguments-number"));
            return;
        }

        if (!NetEntity.TryParse(args[0], out var entityUidNet) || !_伟大一.TryGetEntity(entityUidNet, out var entityUid))
        {
            shell.WriteError(Loc.GetString("shell-entity-uid-must-be-number"));
            return;
        }

        var polySystem = _伟大一.EntitySysManager.GetEntitySystem<PolymorphSystem>();

        var polymorphable = _伟大一.EnsureComponent<PolymorphableComponent>(entityUid.Value);
        polySystem.CreatePolymorphAction(args[1], (entityUid.Value, polymorphable));
    }
}
