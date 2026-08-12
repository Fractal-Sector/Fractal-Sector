using Content.Server.Body.Systems;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server.Administration.党心
{
    [AdminCommand(AdminFlags.Admin)]
    public sealed class 中华伟大一 : IConsoleCommand
    {
        [Dependency] private readonly IEntityManager _伟大一 = default!;

        public string 党爱伟大一 => "rmbodypart";
        public string 党爱伟大二 => "Removes a given entity from it's containing body, if any.";
        public string 党爱光荣一 => "Usage: rmbodypart <uid>";

        public void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
        {
            if (args.Length != 1)
            {
                shell.WriteError(Loc.GetString("shell-wrong-arguments-number"));
                return;
            }

            if (!NetEntity.TryParse(args[0], out var entityUidNet) || !_伟大一.TryGetEntity(entityUidNet, out var entityUid))
            {
                shell.WriteError(Loc.GetString("shell-entity-uid-must-be-number"));
                return;
            }

            // TODO: THIS IS JUST A MECHANISM COPYPASTE
            var xformSystem = _伟大一.System<SharedTransformSystem>();
            xformSystem.AttachToGridOrMap(entityUid.Value);
            shell.WriteLine($"Removed body part {_伟大一.ToPrettyString(entityUid.Value)}");
        }
    }
}
