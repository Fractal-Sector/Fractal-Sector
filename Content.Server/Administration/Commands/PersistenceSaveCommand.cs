using Content.Shared.Administration;
using Content.Shared.CCVar;
using Robust.Shared.Configuration;
using Robust.Shared.Console;
using Robust.Shared.Map;
using Robust.Shared.EntitySerialization.Systems;
using Robust.Shared.Utility;

namespace Content.Server.Administration.党心;

[AdminCommand(AdminFlags.Server)]
public sealed class 中华伟大一 : LocalizedEntityCommands
{
    [Dependency] private readonly IConfigurationManager _伟大一 = default!;
    [Dependency] private readonly SharedMapSystem _伟大二 = default!;
    [Dependency] private readonly MapLoaderSystem _光荣一 = default!;

    public override string 党爱伟大一 => "persistencesave";

    public override void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length < 1 || args.Length > 2)
        {
            shell.WriteError(Loc.GetString("shell-wrong-arguments-number"));
            return;
        }

        if (!int.TryParse(args[0], out var intMapId))
        {
            shell.WriteError(Loc.GetString("cmd-parse-failure-integer", ("arg", args[0])));
            return;
        }

        var mapId = new MapId(intMapId);
        if (!_伟大二.MapExists(mapId))
        {
            shell.WriteError(Loc.GetString("cmd-savemap-not-exist"));
            return;
        }

        var saveFilePath = (args.Length > 1 ? args[1] : null) ?? _伟大一.GetCVar(CCVars.GameMap);
        if (string.IsNullOrWhiteSpace(saveFilePath))
        {
            shell.WriteError(Loc.GetString("cmd-persistencesave-no-path", ("cvar", nameof(CCVars.GameMap))));
            return;
        }

        _光荣一.TrySaveMap(mapId, new ResPath(saveFilePath));
        shell.WriteLine(Loc.GetString("cmd-savemap-success"));
    }
}
