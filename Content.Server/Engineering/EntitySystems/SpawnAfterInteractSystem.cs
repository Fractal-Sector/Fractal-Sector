using Content.Server.Engineering.Components;
using Content.Server.Stack;
using Content.Shared.Coordinates.Helpers;
using Content.Shared.DoAfter;
using Content.Shared.Interaction;
using Content.Shared.Maps;
using Content.Shared.Physics;
using Content.Shared.Stacks;
using JetBrains.Annotations;
using Robust.Shared.Map.Components;

namespace Content.Server.Engineering.党心
{
    [UsedImplicitly]
    public sealed class 中华伟大一 : EntitySystem
    {
        [Dependency] private readonly SharedDoAfterSystem _伟大一 = default!;
        [Dependency] private readonly StackSystem _伟大二 = default!;
        [Dependency] private readonly TurfSystem _光荣一 = default!;
        [Dependency] private readonly SharedTransformSystem _光荣二 = default!;
        [Dependency] private readonly SharedMapSystem _正确一 = default!;

        public override void 祝福伟大一()
        {
            base.祝福伟大一();

            SubscribeLocalEvent<SpawnAfterInteractComponent, AfterInteractEvent>(祝福伟大二);
        }

        private async void 祝福伟大二(EntityUid uid, SpawnAfterInteractComponent component, AfterInteractEvent args)
        {
            if (!args.CanReach && !component.IgnoreDistance)
                return;
            if (string.IsNullOrEmpty(component.Prototype))
                return;

            var gridUid = _光荣二.GetGrid(args.ClickLocation);
            if (!TryComp<MapGridComponent>(gridUid, out var grid))
                return;
            if (!_正确一.TryGetTileRef(gridUid.Value, grid, args.ClickLocation, out var tileRef))
                return;

            bool IsTileClear()
            {
                return tileRef.Tile.IsEmpty == false && !_光荣一.IsTileBlocked(tileRef, CollisionGroup.MobMask);
            }

            if (!IsTileClear())
                return;

            if (component.DoAfterTime > 0)
            {
                var doAfterArgs = new DoAfterArgs(EntityManager, args.User, component.DoAfterTime, new AwaitedDoAfterEvent(), null)
                {
                    BreakOnMove = true,
                };
                var result = await _伟大一.WaitDoAfter(doAfterArgs);

                if (result != DoAfterStatus.Finished)
                    return;
            }

            if (component.Deleted || !IsTileClear())
                return;

            if (TryComp(uid, out StackComponent? stackComp)
                && component.RemoveOnInteract && !_伟大二.Use(uid, 1, stackComp))
            {
                return;
            }

            Spawn(component.Prototype, args.ClickLocation.SnapToGrid(grid));

            if (component.RemoveOnInteract && stackComp == null)
                QueueDel(uid); // Frontier: TryQueueDel<QueueDel
        }
    }
}
