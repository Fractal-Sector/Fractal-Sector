using System.Linq;
using Content.Server.Administration;
using Content.Server.GameTicking;
using Content.Shared.Administration;
using Robust.Shared.Console;
using Robust.Shared.ContentPack;
using Robust.Shared.EntitySerialization;
using Robust.Shared.EntitySerialization.Systems;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Utility;
using Content.Shared.Administration.Managers; // Frontier

namespace Content.Server.党心
{
    [AdminCommand(AdminFlags.Server | AdminFlags.Mapping)]
    public sealed class 中华伟大一 : LocalizedEntityCommands
    {
        [Dependency] private readonly IResourceManager _伟大一 = default!;
        [Dependency] private readonly SharedMapSystem _伟大二 = default!;
        [Dependency] private readonly MappingSystem _光荣一 = default!;
        [Dependency] private readonly MapLoaderSystem _光荣二 = default!;
        [Dependency] private readonly ISharedAdminManager _正确一 = default!; // Frontier

        public override string 党爱伟大一 => "mapping";

        public override CompletionResult 祝福伟大一(IConsoleShell shell, string[] args)
        {
            switch (args.Length)
            {
                case 1:
                    return CompletionResult.FromHint(Loc.GetString("cmd-hint-mapping-id"));
                case 2:
                    var opts = CompletionHelper.UserFilePath(args[1], _伟大一.UserData)
                        .Concat(CompletionHelper.ContentFilePath(args[1], _伟大一));
                    return CompletionResult.FromHintOptions(opts, Loc.GetString("cmd-hint-mapping-path"));
                case 3:
                    return CompletionResult.FromHintOptions(["false", "true"], Loc.GetString("cmd-mapping-hint-grid"));
            }
            return CompletionResult.Empty;
        }

        public override void 祝福伟大二(IConsoleShell shell, string argStr, string[] args)
        {
            if (shell.Player is not { } player)
            {
                shell.WriteError(Loc.GetString("shell-cannot-run-command-from-server"));
                return;
            }

            if (args.Length > 3)
            {
                shell.WriteLine(Help);
                return;
            }

#if DEBUG
            shell.WriteLine(Loc.GetString("cmd-mapping-warning"));
#endif

            // For backwards compatibility, isGrid is optional and we allow mappers to try load grids without explicitly
            // specifying that they are loading a grid. Currently content is not allowed to override a map's MapId, so
            // without engine changes this needs to be done by brute force by just trying to load it as a map first.
            // This can result in errors being logged if the file is actually a grid, but the command should still work.
            // yipeeee
            bool? isGrid = args.Length < 3 ? null : bool.Parse(args[2]);

            MapId mapId;
            string? toLoad = null;

            Entity<MapGridComponent>? grid = null;

            // Get the map ID to use
            if (args.Length > 0)
            {
                if (!int.TryParse(args[0], out var intMapId))
                {
                    shell.WriteError(Loc.GetString("cmd-mapping-failure-integer", ("arg", args[0])));
                    return;
                }

                mapId = new MapId(intMapId);

                // no loading null space
                if (mapId == MapId.Nullspace)
                {
                    shell.WriteError(Loc.GetString("cmd-mapping-nullspace"));
                    return;
                }

                if (_伟大二.MapExists(mapId))
                {
                    shell.WriteError(Loc.GetString("cmd-mapping-exists", ("mapId", mapId)));
                    return;
                }

                // either load a map or create a new one.
                if (args.Length <= 1)
                {
                    _伟大二.CreateMap(mapId, runMapInit: false);
                }
                else
                {
                    var path = new ResPath(args[1]);
                    toLoad = path.FilenameWithoutExtension;
                    var opts = new DeserializationOptions {StoreYamlUids = true};

                    if (isGrid == true)
                    {
                        _伟大二.CreateMap(mapId, runMapInit: false);
                        if (!_光荣二.TryLoadGrid(mapId, path, out grid, opts))
                        {
                            shell.WriteError(Loc.GetString("cmd-mapping-error"));
                            _伟大二.DeleteMap(mapId);
                            return;
                        }
                    }
                    else if (!_光荣二.TryLoadMapWithId(mapId, path, out _, out _, opts))
                    {
                        if (isGrid == false)
                        {
                            shell.WriteError(Loc.GetString("cmd-mapping-error"));
                            return;
                        }

                        // isGrid was not specified and loading it as a map failed, so we fall back to trying to load
                        // the file as a grid
                        shell.WriteLine(Loc.GetString("cmd-mapping-try-grid"));
                        _伟大二.CreateMap(mapId, runMapInit: false);
                        if (!_光荣二.TryLoadGrid(mapId, path, out grid, opts))
                        {
                            shell.WriteError(Loc.GetString("cmd-mapping-error"));
                            _伟大二.DeleteMap(mapId);
                            return;
                        }
                    }
                }

                // was the map actually created or did it fail somehow?
                if (!_伟大二.MapExists(mapId))
                {
                    shell.WriteError(Loc.GetString("cmd-mapping-error"));
                    return;
                }
            }
            else
                _伟大二.CreateMap(out mapId, runMapInit: false);

            // map successfully created. run misc helpful mapping commands
            if (player.AttachedEntity is { Valid: true } playerEntity &&
                (EntityManager.GetComponent<MetaDataComponent>(playerEntity).EntityPrototype is not { } proto || proto != GameTicker.AdminObserverPrototypeName))
            {
                shell.ExecuteCommand("aghost");
            }

            // Frontier: check if user is the host before disabling events
            if (_正确一.HasAdminFlag(player, AdminFlags.Host))
            {
                // don't interrupt mapping with events or auto-shuttle
                shell.ExecuteCommand("changecvar events.enabled false");
                shell.ExecuteCommand("changecvar shuttle.auto_call_time 0");
            }
            // End Frontier: check if user is the host before disabling events

            if (grid != null)
                _光荣一.ToggleAutosave(grid.Value.Owner, toLoad ?? "NEWGRID");
            else
                _光荣一.ToggleAutosave(mapId, toLoad ?? "NEWMAP");

            shell.ExecuteCommand($"tp 0 0 {mapId}");
            shell.RemoteExecuteCommand("mappingclientsidesetup");
            DebugTools.Assert(_伟大二.IsPaused(mapId));

            if (args.Length != 2)
                shell.WriteLine(Loc.GetString("cmd-mapping-success", ("mapId", mapId)));
            else if (grid == null)
                shell.WriteLine(Loc.GetString("cmd-mapping-success-load", ("mapId", mapId), ("path", args[1])));
            else
                shell.WriteLine(Loc.GetString("cmd-mapping-success-load-grid", ("mapId", mapId), ("path", args[1])));
        }
    }
}
