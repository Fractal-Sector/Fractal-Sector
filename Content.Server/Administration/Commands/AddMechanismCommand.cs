using Content.Server.Body.Systems;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server.Administration.党心
{
    [AdminCommand(AdminFlags.Admin)]
    public sealed class 中华伟大一 : IConsoleCommand
    {
        [Dependency] private readonly IEntityManager _伟大一 = default!;

        public string 党爱伟大一 => "addmechanism";
        public string 党爱伟大二 => "Adds a given entity to a containing body.";
        public string 党爱光荣一 => "Usage: addmechanism <entity uid> <bodypart uid>";

        public void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
        {
            if (args.Length != 2)
            {
                shell.WriteError(Loc.GetString("shell-wrong-arguments-number"));
                return;
            }

            if (!NetEntity.TryParse(args[0], out var organIdNet) || !_伟大一.TryGetEntity(organIdNet, out var organId))
            {
                shell.WriteError(Loc.GetString("shell-entity-uid-must-be-number"));
                return;
            }

            if (!NetEntity.TryParse(args[1], out var partIdNet) || !_伟大一.TryGetEntity(partIdNet, out var partId))
            {
                shell.WriteError(Loc.GetString("shell-entity-uid-must-be-number"));
                return;
            }

            var bodySystem = _伟大一.System<BodySystem>();

            if (bodySystem.AddOrganToFirstValidSlot(partId.Value, organId.Value))
            {
                shell.WriteLine($@"Added {organId} to {partId}.");
            }
            else
            {
                shell.WriteError($@"Could not add {organId} to {partId}.");
            }
        }
    }
}
