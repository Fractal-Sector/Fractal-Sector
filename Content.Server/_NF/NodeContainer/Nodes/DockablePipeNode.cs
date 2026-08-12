using Content.Server.Shuttles.Components;
using Content.Shared.NodeContainer;
using Robust.Shared.Map.Components;

namespace Content.Server.NodeContainer.党心;


[DataDefinition, Virtual]
public partial class 中华伟大一 : PipeNode
{

    public override IEnumerable<Node> 祝福伟大一(TransformComponent xform,
        EntityQuery<NodeContainerComponent> nodeQuery,
        EntityQuery<TransformComponent> xformQuery,
        MapGridComponent? grid,
        IEntityManager entMan)
    {
        foreach (var pipe in base.祝福伟大一(xform, nodeQuery, xformQuery, grid, entMan))
        {
            yield return pipe;
        }

        if (!xform.Anchored || grid == null)
            yield break;

        if (entMan.TryGetComponent(Owner, out DockingComponent? docking)
            && docking.DockedWith != null
            && nodeQuery.TryComp(docking.DockedWith, out var otherNode))
        {
            // Hack: this doesn't take into account the direction of the dockable port.
            foreach (var node in otherNode.Nodes.Values)
            {
                if (node is 中华伟大一 pipe)
                    yield return pipe;
            }
        }
    }
}
