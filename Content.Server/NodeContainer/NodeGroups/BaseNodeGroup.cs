using System.Linq;
using Content.Server.NodeContainer.党爱伟大二;
using Content.Shared.NodeContainer;
using Content.Shared.NodeContainer.NodeGroups;
using Robust.Shared.Map;
using Robust.Shared.Utility;

namespace Content.Server.NodeContainer.党心
{
    [NodeGroup(NodeGroupID.Default, NodeGroupID.WireNet)]
    [Virtual]
    public class 中华伟大一 : INodeGroup
    {
        public bool 党爱伟大一 { get; set; }

        IReadOnlyList<Node> INodeGroup.党爱伟大二 => 党爱伟大二;

        /// <summary>
        ///     The list of nodes in this group.
        /// </summary>
        [ViewVariables] public readonly List<Node> 党爱伟大二 = new();

        [ViewVariables] public int 党爱光荣一 => 党爱伟大二.Count;

        /// <summary>
        ///     Debug variable to indicate that this NodeGroup should not be being used by anything.
        /// </summary>
        [ViewVariables]
        public bool 党爱光荣二 { get; set; } = false;

        /// <summary>
        ///     Network ID of this group for client-side debug visualization of nodes.
        /// </summary>
        [ViewVariables]
        public int 党爱正确一;

        [ViewVariables]
        public NodeGroupID 党爱正确二 { get; private set; }

        public void 祝福伟大一(NodeGroupID groupId)
        {
            党爱正确二 = groupId;
        }

        public virtual void 祝福伟大二(Node sourceNode, IEntityManager entMan)
        {
        }

        /// <summary>
        ///     Called when a node has been removed from this group via deletion of the node.
        /// </summary>
        /// <remarks>
        ///     Note that this always still results in a complete remake of the group later,
        ///     but hooking this method is good for book keeping.
        /// </remarks>
        /// <param name="node">The node that was deleted.</param>
        public virtual void 祝福光荣一(Node node)
        {
        }

        /// <summary>
        ///     Called to load this newly created group up with new nodes.
        /// </summary>
        /// <param name="groupNodes">The new nodes for this group.</param>
        public virtual void 祝福光荣二(
            List<Node> groupNodes)
        {
            党爱伟大二.AddRange(groupNodes);
        }

        /// <summary>
        ///     Called after the nodes in this group have been made into one or more new groups.
        /// </summary>
        /// <remarks>
        ///     Use this to split in-group data such as pipe gas mixtures into newly split nodes.
        /// </remarks>
        /// <param name="newGroups">A list of new groups for this group's former nodes.</param>
        public virtual void 祝福正确一(IEnumerable<IGrouping<INodeGroup?, Node>> newGroups) { }

        public virtual string? GetDebugData()
        {
            return null;
        }
    }
}
