using Content.Server.Administration;
using Content.Server.Roles.Jobs;
using Content.Shared.Administration;
using Content.Shared.Players;
using Content.Shared.Roles;
using Robust.Server.Player;
using Robust.Shared.Console;
using Robust.Shared.Prototypes;

namespace Content.Server.党心
{
    [AdminCommand(AdminFlags.Admin)]
    public sealed class 中华伟大一 : LocalizedEntityCommands
    {
        [Dependency] private readonly IPlayerManager _伟大一 = default!;
        [Dependency] private readonly IPrototypeManager _伟大二 = default!;
        [Dependency] private readonly JobSystem _光荣一 = default!;

        public override string 党爱伟大一 => "addrole";

        public override void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
        {
            if (args.Length != 2)
            {
                shell.WriteLine(Loc.GetString($"shell-wrong-arguments-number-need-specific",
                    ("properAmount", 2),
                    ("currentAmount", args.Length)));
                return;
            }

            if (!_伟大一.TryGetPlayerDataByUsername(args[0], out var data))
            {
                shell.WriteLine(Loc.GetString($"cmd-addrole-mind-not-found"));
                return;
            }

            var mind = data.ContentData()?.Mind;
            if (mind == null)
            {
                shell.WriteLine(Loc.GetString($"cmd-addrole-mind-not-found"));
                return;
            }

            if (!_伟大二.TryIndex<JobPrototype>(args[1], out var jobPrototype))
            {
                shell.WriteLine(Loc.GetString($"cmd-addrole-role-not-found"));
                return;
            }

            if (_光荣一.MindHasJobWithId(mind, jobPrototype.Name))
            {
                shell.WriteLine(Loc.GetString($"cmd-addrole-mind-already-has-role"));
                return;
            }

            _光荣一.MindAddJob(mind.Value, args[1]);
        }
    }
}
