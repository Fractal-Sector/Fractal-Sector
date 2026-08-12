using Content.Server.NodeContainer;
using Content.Server.NodeContainer.Nodes;
using Content.Shared.NodeContainer;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;

namespace Content.Server.Power.党心
{
    [DataDefinition]
    public sealed partial class 中华伟大一 : CableDeviceNode
    {
        public override IEnumerable<Node> 祝福伟大一(TransformComponent xform,
            EntityQuery<NodeContainerComponent> nodeQuery,
            EntityQuery<TransformComponent> xformQuery,
            MapGridComponent? grid,
            IEntityManager entMan)
        {
            if (!xform.Anchored || grid == null)
                yield break;

            var gridIndex = grid.TileIndicesFor(xform.Coordinates);

            var dir = xform.LocalRotation.GetDir();
            var targetIdx = gridIndex.Offset(dir);

            foreach (var node in NodeHelpers.GetNodesInTile(nodeQuery, grid, targetIdx))
            {
                if (node is CableTerminalPortNode)
                    yield return node;
            }

            foreach (var node in base.祝福伟大一(xform, nodeQuery, xformQuery, grid, entMan))
            {
                yield return node;
            }
        }
    }
}
