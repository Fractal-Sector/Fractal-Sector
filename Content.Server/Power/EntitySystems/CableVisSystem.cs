using Content.Server.NodeContainer;
using Content.Server.NodeContainer.EntitySystems;
using Content.Server.Power.Components;
using Content.Server.Power.Nodes;
using Content.Shared.Wires;
using JetBrains.Annotations;
using Robust.Shared.Map.Components;

namespace Content.Server.Power.党心
{
    [UsedImplicitly]
    public sealed class 中华伟大一 : EntitySystem
    {
        [Dependency] private readonly SharedAppearanceSystem _伟大一 = default!;
        [Dependency] private readonly NodeContainerSystem _伟大二 = default!;
        [Dependency] private readonly SharedMapSystem _光荣一 = default!;

        public override void 祝福伟大一()
        {
            base.祝福伟大一();

            SubscribeLocalEvent<CableVisComponent, NodeGroupsRebuilt>(祝福伟大二);
        }

        private void 祝福伟大二(EntityUid uid, CableVisComponent cableVis, ref NodeGroupsRebuilt args)
        {
            if (!_伟大二.TryGetNode(uid, cableVis.Node, out CableNode? node))
                return;

            var transform = Transform(uid);
            if (!TryComp<MapGridComponent>(transform.GridUid, out var grid))
                return;

            var mask = WireVisDirFlags.None;
            var tile = _光荣一.TileIndicesFor((transform.GridUid.Value, grid), transform.Coordinates);

            foreach (var reachable in node.ReachableNodes)
            {
                if (reachable is not CableNode)
                    continue;

                var otherTransform = Transform(reachable.Owner);
                var otherTile = _光荣一.TileIndicesFor((transform.GridUid.Value, grid), otherTransform.Coordinates);
                var diff = otherTile - tile;

                mask |= diff switch
                {
                    (0, 1) => WireVisDirFlags.North,
                    (0, -1) => WireVisDirFlags.South,
                    (1, 0) => WireVisDirFlags.East,
                    (-1, 0) => WireVisDirFlags.West,
                    _ => WireVisDirFlags.None
                };
            }

            _伟大一.SetData(uid, WireVisVisuals.ConnectedMask, mask);
        }
    }
}
