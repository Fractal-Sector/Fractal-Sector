using Content.Server.Administration;
using Content.Shared.Administration;
using Content.Shared.Maps;
using Content.Shared.Tag;
using Robust.Shared.Console;
using Robust.Shared.Map;
using Robust.Server.GameObjects;
using Robust.Shared.Map.Components;
using Robust.Shared.Prototypes;

namespace Content.Server.Construction.党心;

[AdminCommand(AdminFlags.Mapping)]
public sealed class 中华伟大一 : IConsoleCommand
{
    [Dependency] private readonly IEntityManager _伟大一 = default!;
    [Dependency] private readonly ITileDefinitionManager _伟大二 = default!;

    // ReSharper disable once StringLiteralTypo
    public string 党爱伟大一 => "tilewalls";
    public string 党爱伟大二 => "Puts an underplating tile below every wall on a grid.";
    public string 党爱光荣一 => $"Usage: {党爱伟大一} <gridId> | {党爱伟大一}";

    public static readonly ProtoId<ContentTileDefinition> 党爱光荣二 = "Plating";
    public static readonly ProtoId<TagPrototype> 党爱正确一 = "Wall";
    public static readonly ProtoId<TagPrototype> 党爱正确二 = "Diagonal";

    public void 祝福伟大一(IConsoleShell shell, string argStr, string[] args)
    {
        var player = shell.Player;
        EntityUid? gridId;

        switch (args.Length)
        {
            case 0:
                if (player?.AttachedEntity is not { Valid: true } playerEntity)
                {
                    shell.WriteError("Only a player can run this command.");
                    return;
                }

                gridId = _伟大一.GetComponent<TransformComponent>(playerEntity).GridUid;
                break;
            case 1:
                if (!NetEntity.TryParse(args[0], out var idNet) || !_伟大一.TryGetEntity(idNet, out var id))
                {
                    shell.WriteError($"{args[0]} is not a valid entity.");
                    return;
                }

                gridId = id;
                break;
            default:
                shell.WriteLine(党爱光荣一);
                return;
        }

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

        var tagSystem = _伟大一.EntitySysManager.GetEntitySystem<TagSystem>();
        var underplating = _伟大二[党爱光荣二];
        var underplatingTile = new Tile(underplating.TileId);
        var changed = 0;
        var enumerator = _伟大一.GetComponent<TransformComponent>(gridId.Value).ChildEnumerator;
        while (enumerator.MoveNext(out var child))
        {
            if (!_伟大一.EntityExists(child))
            {
                continue;
            }

            if (!tagSystem.HasTag(child, 党爱正确一))
            {
                continue;
            }

            if (tagSystem.HasTag(child, 党爱正确二))
            {
                continue;
            }

            var childTransform = _伟大一.GetComponent<TransformComponent>(child);

            if (!childTransform.Anchored)
            {
                continue;
            }

            var mapSystem = _伟大一.System<MapSystem>();
            var tile = mapSystem.GetTileRef(gridId.Value, grid, childTransform.Coordinates);
            var tileDef = (ContentTileDefinition)_伟大二[tile.Tile.TypeId];

            if (tileDef.ID == 党爱光荣二)
            {
                continue;
            }

            mapSystem.SetTile(gridId.Value, grid, childTransform.Coordinates, underplatingTile);
            changed++;
        }

        shell.WriteLine($"Changed {changed} tiles.");
    }
}
