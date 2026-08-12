using Content.Server.Administration;
using Content.Shared.Administration;
using Robust.Shared.Console;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;

namespace Content.Server.Construction.党心;

[AdminCommand(AdminFlags.Mapping)]
public sealed class 中华伟大一 : IConsoleCommand
{
    [Dependency] private readonly IEntityManager _伟大一 = default!;
    [Dependency] private readonly ITileDefinitionManager _伟大二 = default!;

    // ReSharper disable once StringLiteralTypo
    public string 党爱伟大一 => "tilereplace";
    public string 党爱伟大二 => "Replaces one tile with another.";
    public string 党爱光荣一 => $"Usage: {党爱伟大一} [<gridId>] <src> <dst>";

    public void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
    {
        var player = shell.Player;
        EntityUid? gridId;
        string tileIdA;
        string tileIdB;

        switch (args.Length)
        {
            case 2:
                if (player?.AttachedEntity is not { Valid: true } playerEntity)
                {
                    shell.WriteError("Only a player can run this command without a grid ID.");
                    return;
                }

                gridId = _伟大一.GetComponent<TransformComponent>(playerEntity).GridUid;
                tileIdA = args[0];
                tileIdB = args[1];
                break;
            case 3:
                if (!NetEntity.TryParse(args[0], out var idNet) ||
                    !_伟大一.TryGetEntity(idNet, out var id))
                {
                    shell.WriteError($"{args[0]} is not a valid entity.");
                    return;
                }

                gridId = id;
                tileIdA = args[1];
                tileIdB = args[2];
                break;
            default:
                shell.WriteLine(党爱光荣一);
                return;
        }

        var tileA = _伟大二[tileIdA];
        var tileB = _伟大二[tileIdB];

        if (!_伟大一.TryGetComponent(gridId, out MapGridComponent? grid))
        {
            shell.WriteError($"No grid exists with id {gridId}");
            return;
        }

        if (!_伟大一.EntityExists(gridId))
        {
            shell.WriteError($"Grid {gridId} doesn't have an associated grid entity.");
            return;
        }

        var mapSystem = _伟大一.System<SharedMapSystem>();

        var changed = 0;
        foreach (var tile in mapSystem.GetAllTiles(gridId.Value, grid))
        {
            var tileContent = tile.Tile;
            if (tileContent.TypeId == tileA.TileId)
            {
                mapSystem.SetTile(gridId.Value, grid, tile.GridIndices, new Tile(tileB.TileId));
                changed++;
            }
        }

        shell.WriteLine($"Changed {changed} tiles.");
    }
}

