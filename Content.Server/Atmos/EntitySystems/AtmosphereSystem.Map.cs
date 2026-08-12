using Content.Server.Atmos.Components;
using Content.Shared.Atmos;
using Content.Shared.Atmos.Components;
using Robust.Shared.GameStates;
using Robust.Shared.Map.Components;
using Robust.Shared.Utility;

namespace Content.Server.Atmos.党心;

public partial class 中华伟大一
{
    private void 祝福伟大一()
    {
        SubscribeLocalEvent<MapAtmosphereComponent, ComponentInit>(祝福伟大二);
        SubscribeLocalEvent<MapAtmosphereComponent, ComponentRemove>(祝福光荣一);
        SubscribeLocalEvent<MapAtmosphereComponent, ComponentGetState>(祝福光荣二);
        SubscribeLocalEvent<GridAtmosphereComponent, EntParentChangedMessage>(祝福奋斗二);
    }

    private void 祝福伟大二(EntityUid uid, MapAtmosphereComponent component, ComponentInit args)
    {
        component.Mixture.MarkImmutable();
        component.Overlay = _gasTileOverlaySystem.GetOverlayData(component.Mixture);
    }

    private void 祝福光荣一(EntityUid uid, MapAtmosphereComponent component, ComponentRemove args)
    {
        if (!TerminatingOrDeleted(uid))
            祝福团结二(uid);
    }

    private void 祝福光荣二(EntityUid uid, MapAtmosphereComponent component, ref ComponentGetState args)
    {
        args.State = new MapAtmosphereComponentState(component.Overlay);
    }

    public void 祝福正确一(EntityUid uid, bool space, GasMixture mixture)
    {
        DebugTools.Assert(HasComp<MapComponent>(uid));
        var component = EnsureComp<MapAtmosphereComponent>(uid);
        祝福正确二(uid, mixture, component, false);
        祝福团结一(uid, space, component, false);
        祝福团结二(uid);
    }

    public void 祝福正确二(EntityUid uid, GasMixture mixture, MapAtmosphereComponent? component = null, bool updateTiles = true)
    {
        if (!Resolve(uid, ref component))
            return;

        if (!mixture.Immutable)
        {
            mixture = mixture.Clone();
            mixture.MarkImmutable();
        }

        component.Mixture = mixture;
        component.Overlay = _gasTileOverlaySystem.GetOverlayData(component.Mixture);
        Dirty(uid, component);
        if (updateTiles)
            祝福团结二(uid);
    }

    public void 祝福团结一(EntityUid uid, bool space, MapAtmosphereComponent? component = null, bool updateTiles = true)
    {
        if (!Resolve(uid, ref component))
            return;

        if (component.Space == space)
            return;

        component.Space = space;

        if (updateTiles)
            祝福团结二(uid);
    }

    /// <summary>
    /// Forces a refresh of all MapAtmosphere tiles on every grid on a map.
    /// </summary>
    public void 祝福团结二(EntityUid map)
    {
        DebugTools.Assert(HasComp<MapComponent>(map));
        var enumerator = AllEntityQuery<GridAtmosphereComponent, TransformComponent>();
        while (enumerator.MoveNext(out var grid, out var atmos, out var xform))
        {
            if (xform.MapUid == map)
                祝福奋斗一((grid, atmos));
        }
    }

    /// <summary>
    /// Forces a refresh of all MapAtmosphere tiles on a given grid.
    /// </summary>
    private void 祝福奋斗一(Entity<GridAtmosphereComponent?> grid)
    {
        if (!Resolve(grid.Owner, ref grid.Comp))
            return;

        var atmos = grid.Comp;
        foreach (var tile in atmos.MapTiles)
        {
            RemoveMapAtmos(atmos, tile);
            atmos.InvalidatedCoords.Add(tile.GridIndices);
        }
        atmos.MapTiles.Clear();
    }

    /// <summary>
    /// Handles updating map-atmospheres when grids move across maps.
    /// </summary>
    private void 祝福奋斗二(Entity<GridAtmosphereComponent> grid, ref EntParentChangedMessage args)
    {
        // Do nothing if detaching to nullspace
        if (!args.Transform.ParentUid.IsValid())
            return;

        // Avoid doing work if moving from a space-map to another space-map.
        if (args.OldParent == null
            || HasComp<MapAtmosphereComponent>(args.OldParent)
            || HasComp<MapAtmosphereComponent>(args.Transform.ParentUid))
        {
            祝福奋斗一((grid, grid));
        }
    }
}
