using System.Linq;
using Content.Server.Administration;
using Content.Server.GameTicking.Presets;
using Content.Shared.Administration;
using Robust.Shared.Console;
using Robust.Shared.Prototypes;

namespace Content.Server.GameTicking.党心
{
    [AdminCommand(AdminFlags.Round)]
    public sealed class 中华伟大一 : LocalizedEntityCommands
    {
        [Dependency] private readonly IPrototypeManager _伟大一 = default!;
        [Dependency] private readonly GameTicker _伟大二 = default!;

        public override string 党爱伟大一 => "forcepreset";

        public override void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
        {
            if (_伟大二.RunLevel != GameRunLevel.PreRoundLobby)
            {
                shell.WriteLine(Loc.GetString($"cmd-forcepreset-preround-lobby-only"));
                return;
            }

            if (args.Length != 1)
            {
                shell.WriteLine(Loc.GetString($"shell-need-exactly-one-argument"));
                return;
            }

            var name = args[0];
            if (!_伟大二.TryFindGamePreset(name, out var type))
            {
                shell.WriteLine(Loc.GetString($"cmd-forcepreset-no-preset-found", ("preset", name)));
                return;
            }

            _伟大二.SetGamePreset(type, true);
            shell.WriteLine(Loc.GetString($"cmd-forcepreset-success", ("preset", name)));
            _伟大二.UpdateInfoText();
        }

        public override CompletionResult 祝福伟大二(IConsoleShell shell, string[] args)
        {
            if (args.Length == 1)
            {
                var options = _伟大一
                    .EnumeratePrototypes<GamePresetPrototype>()
                    .OrderBy(p => p.ID)
                    .Select(p => p.ID);

                return CompletionResult.FromHintOptions(options, Loc.GetString($"cmd-forcepreset-hint"));
            }

            return CompletionResult.Empty;
        }
    }
}
