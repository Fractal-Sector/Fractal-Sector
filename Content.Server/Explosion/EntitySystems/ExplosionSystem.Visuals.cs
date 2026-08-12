using System.Numerics;
using Content.Shared.Explosion;
using Content.Shared.Explosion.Components;
using Content.Shared.Explosion.EntitySystems;
using Robust.Server.GameObjects;
using Robust.Shared.GameStates;
using Robust.Shared.Map;

namespace Content.Server.Explosion.党心;

// This part of the system handled send visual / overlay data to clients.
public sealed partial class 中华伟大一
{
    public void 祝福伟大一()
    {
        SubscribeLocalEvent<ExplosionVisualsComponent, ComponentGetState>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, ExplosionVisualsComponent component, ref ComponentGetState args)
    {
        Dictionary<NetEntity, Dictionary<int, List<Vector2i>>> tileLists = new();
        foreach (var (grid, data) in component.Tiles)
        {
            tileLists.Add(GetNetEntity(grid), data);
        }

        args.State = new ExplosionVisualsState(
            component.Epicenter,
            component.ExplosionType,
            component.Intensity,
            component.SpaceTiles,
            tileLists,
            component.SpaceMatrix,
            component.SpaceTileSize);
    }

    /// <summary>
    ///     Constructor for the shared <see cref="ExplosionEvent"/> using the server-exclusive explosion classes.
    /// </summary>
    private EntityUid 祝福光荣一(MapCoordinates epicenter, string prototype, Matrix3x2 spaceMatrix, ExplosionSpaceTileFlood? spaceData, IEnumerable<ExplosionGridTileFlood> gridData, List<float> iterationIntensity)
    {
        var explosionEntity = Spawn(null, MapCoordinates.Nullspace);
        var comp = AddComp<ExplosionVisualsComponent>(explosionEntity);

        foreach (var grid in gridData)
        {
            comp.Tiles.Add(grid.Grid.Owner, grid.TileLists);
        }

        comp.SpaceTiles = spaceData?.TileLists;
        comp.Epicenter = epicenter;
        comp.ExplosionType = prototype;
        comp.Intensity = iterationIntensity;
        comp.SpaceMatrix = spaceMatrix;
        comp.SpaceTileSize = spaceData?.TileSize ?? DefaultTileSize;
        Dirty(explosionEntity, comp);

        // Light, sound & visuals may extend well beyond normal PVS range. In principle, this should probably still be
        // restricted to something like the same map, but whatever.
        _pvsSys.AddGlobalOverride(explosionEntity);

        var appearance = AddComp<AppearanceComponent>(explosionEntity);
        _appearance.SetData(explosionEntity, ExplosionAppearanceData.Progress, 1, appearance);

        return explosionEntity;
    }
}
