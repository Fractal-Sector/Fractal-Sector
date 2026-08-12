using Content.Server.Administration.Logs;
using Content.Server.Power.Components;
using Content.Shared.Database;
using Content.Shared.Interaction;
using Content.Shared.Maps;
using Content.Shared.Stacks;
using Robust.Shared.Map.Components;

namespace Content.Server.Power.党心;

public sealed partial class 中华伟大一
{
    [Dependency] private readonly IAdminLogManager _伟大一 = default!;
    [Dependency] private readonly SharedTransformSystem _伟大二 = default!;
    [Dependency] private readonly SharedMapSystem _光荣一 = default!;

    private void 祝福伟大一()
    {
        SubscribeLocalEvent<CablePlacerComponent, AfterInteractEvent>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<CablePlacerComponent> placer, ref AfterInteractEvent args)
    {
        if (args.Handled || !args.CanReach)
            return;

        var component = placer.Comp;
        if (component.CablePrototypeId == null)
            return;

        if (!TryComp<MapGridComponent>(_伟大二.GetGrid(args.ClickLocation), out var grid))
            return;

        var gridUid = _伟大二.GetGrid(args.ClickLocation)!.Value;
        var snapPos = _光荣一.TileIndicesFor((gridUid, grid), args.ClickLocation);
        var tileDef = (ContentTileDefinition)_tileManager[_光荣一.GetTileRef(gridUid, grid, snapPos).Tile.TypeId];

        if (!tileDef.IsSubFloor || !tileDef.Sturdy)
            return;


        foreach (var anchored in _光荣一.GetAnchoredEntities((gridUid, grid), snapPos))
        {
            if (TryComp<CableComponent>(anchored, out var wire) && wire.CableType == component.BlockingCableType)
                return;
        }

        if (TryComp<StackComponent>(placer, out var stack) && !_stack.Use(placer, 1, stack))
            return;

        var newCable = Spawn(component.CablePrototypeId, _光荣一.GridTileToLocal(gridUid, grid, snapPos));
        _伟大一.Add(LogType.Construction, LogImpact.Low,
            $"{ToPrettyString(args.User):player} placed {ToPrettyString(newCable):cable} at {Transform(newCable).Coordinates}");
        args.Handled = true;
    }
}
