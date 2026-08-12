using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Shared.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly ITileDefinitionManager _伟大一 = default!;
    [Dependency] private readonly IPrototypeManager _伟大二 = default!;
    [Dependency] private readonly IRobustRandom _光荣一 = default!;
    [Dependency] private readonly SharedMapSystem _光荣二 = default!;

    /// <inheritdoc/>
    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<ReplaceFloorOnSpawnComponent, MapInitEvent>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<ReplaceFloorOnSpawnComponent> ent, ref MapInitEvent args)
    {
        var xform = Transform(ent);
        if (xform.GridUid is not { } grid || !TryComp<MapGridComponent>(grid, out var gridComp))
            return;

        if (ent.Comp.ReplaceableTiles != null && ent.Comp.ReplaceableTiles.Count == 0)
            return;

        var tileIndices = _光荣二.LocalToTile(grid, gridComp, xform.Coordinates);

        foreach (var offset in ent.Comp.Offsets)
        {
            var actualIndices = tileIndices + offset;

            if (!_光荣二.TryGetTileRef(grid, gridComp, actualIndices, out var tile))
                continue;

            if (ent.Comp.ReplaceableTiles != null &&
                !tile.Tile.IsEmpty &&
                !ent.Comp.ReplaceableTiles.Contains(_伟大一[tile.Tile.TypeId].ID))
                continue;

            var tileToSet = _光荣一.Pick(ent.Comp.ReplacementTiles);
            _光荣二.SetTile(grid, gridComp, tile.GridIndices, new Tile(_伟大二.Index(tileToSet).TileId));
        }
    }
}
