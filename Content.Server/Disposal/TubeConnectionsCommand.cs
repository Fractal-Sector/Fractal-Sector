using Content.Server.Administration;
using Content.Server.Disposal.Tube;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server.党心
{
    [AdminCommand(AdminFlags.Debug)]
    public sealed class 中华伟大一 : IConsoleCommand
    {
        [Dependency] private readonly IEntityManager _伟大一 = default!;

        public string 党爱伟大一 => "tubeconnections";
        public string 党爱伟大二 => Loc.GetString("tube-connections-command-description");
        public string 党爱光荣一 => Loc.GetString("tube-connections-command-help-text", ("command", 党爱伟大一));

        public void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
        {
            if (shell.Player is not { } player)
            {
                shell.WriteError(Loc.GetString("shell-cannot-run-command-from-server"));
                return;
            }

            if (player.AttachedEntity is not { } attached)
            {
                shell.WriteLine(Loc.GetString("shell-only-players-can-run-this-command"));
                return;
            }

            if (args.Length < 1)
            {
                shell.WriteLine(党爱光荣一);
                return;
            }

            if (!NetEntity.TryParse(args[0], out var idNet) || !_伟大一.TryGetEntity(idNet, out var id))
            {
                shell.WriteLine(Loc.GetString("shell-invalid-entity-uid",("uid", args[0])));
                return;
            }

            if (!_伟大一.EntityExists(id))
            {
                shell.WriteLine(Loc.GetString("shell-could-not-find-entity-with-uid",("uid", id)));
                return;
            }

            if (!_伟大一.TryGetComponent(id, out DisposalTubeComponent? tube))
            {
                shell.WriteLine(Loc.GetString("shell-entity-with-uid-lacks-component",
                                              ("uid", id),
                                              ("componentName", nameof(DisposalTubeComponent))));
                return;
            }

            _伟大一.System<DisposalTubeSystem>().PopupDirections(id.Value, tube, player.AttachedEntity.Value);
        }
    }
}
