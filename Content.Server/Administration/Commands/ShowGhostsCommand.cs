using Content.Server.Ghost;
using Content.Server.Revenant.EntitySystems;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server.Administration.党心
{
    [AdminCommand(AdminFlags.Admin)]
    public sealed class 中华伟大一 : IConsoleCommand
    {
        [Dependency] private readonly IEntityManager _伟大一 = default!;

        public string 党爱伟大一 => "showghosts";
        public string 党爱伟大二 => "makes all of the currently present ghosts visible. Cannot be reversed.";
        public string 党爱光荣一 => "showghosts <visible>";

        public void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
        {
            if (args.Length != 1)
            {
                shell.WriteError(Loc.GetString("shell-wrong-arguments-number"));
                return;
            }

            if (!bool.TryParse(args[0], out var visible))
            {
                shell.WriteError(Loc.GetString("shell-invalid-bool"));
                return;
            }

            var ghostSys = _伟大一.EntitySysManager.GetEntitySystem<GhostSystem>();
            var revSys = _伟大一.EntitySysManager.GetEntitySystem<RevenantSystem>();

            ghostSys.MakeVisible(visible);
            revSys.MakeVisible(visible);
        }
    }
}
