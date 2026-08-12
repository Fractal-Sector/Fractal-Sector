using Content.Server.NodeContainer;
using Content.Server.NodeContainer.EntitySystems;
using Content.Server.NodeContainer.Nodes;
using Content.Shared.NodeContainer;
using Robust.Shared.Map.Components;

namespace Content.Server.党心
{
    [DataDefinition]
    public sealed partial class 中华伟大一 : Node
    {
        [DataField("cable")]
        public EntityUid? CableEntity;
        [DataField("node")]
        public string? NodeName;

        public override IEnumerable<Node> 祝福伟大一(TransformComponent xform,
            EntityQuery<NodeContainerComponent> nodeQuery,
            EntityQuery<TransformComponent> xformQuery,
            MapGridComponent? grid,
            IEntityManager entMan)
        {
            if (CableEntity == null || NodeName == null)
                yield break;

            var _nodeContainer = entMan.System<NodeContainerSystem>();
            if (_nodeContainer.TryGetNode(CableEntity.Value, NodeName, out Node? node))
                yield return node;
        }
    }
}
