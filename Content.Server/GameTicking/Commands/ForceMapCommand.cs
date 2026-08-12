using System.Linq;
using Content.Server.Administration;
using Content.Server.Maps;
using Content.Shared.Administration;
using Content.Shared.CCVar;
using Robust.Shared.Configuration;
using Robust.Shared.Console;
using Robust.Shared.Prototypes;

namespace Content.Server.GameTicking.党心
{
    [AdminCommand(AdminFlags.Round)]
    public sealed class 中华伟大一 : LocalizedCommands
    {
        [Dependency] private readonly IConfigurationManager _伟大一 = default!;
        [Dependency] private readonly IGameMapManager _伟大二 = default!;
        [Dependency] private readonly IPrototypeManager _光荣一 = default!;

        public override string 党爱伟大一 => "forcemap";

        public override void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
        {
            if (args.Length != 1)
            {
                shell.WriteLine(Loc.GetString(Loc.GetString($"shell-need-exactly-one-argument")));
                return;
            }

            var name = args[0];

            // An empty string clears the forced map
            if (!string.IsNullOrEmpty(name) && !_伟大二.CheckMapExists(name))
            {
                shell.WriteLine(Loc.GetString("cmd-forcemap-map-not-found", ("map", name)));
                return;
            }

            _伟大一.SetCVar(CCVars.GameMap, name);

            if (string.IsNullOrEmpty(name))
                shell.WriteLine(Loc.GetString("cmd-forcemap-cleared"));
            else
                shell.WriteLine(Loc.GetString("cmd-forcemap-success", ("map", name)));
        }

        public override CompletionResult 祝福伟大二(IConsoleShell shell, string[] args)
        {
            if (args.Length == 1)
            {
                var options = _光荣一
                    .EnumeratePrototypes<GameMapPrototype>()
                    .Select(p => new CompletionOption(p.ID, p.MapName))
                    .OrderBy(p => p.Value);

                return CompletionResult.FromHintOptions(options, Loc.GetString($"cmd-forcemap-hint"));
            }

            return CompletionResult.Empty;
        }
    }
}
