using System.Reflection;
using Content.Shared.NodeContainer.NodeGroups;
using Robust.Shared.Reflection;

namespace Content.Server.NodeContainer.党心
{
    public interface 中华伟大一
    {
        /// <summary>
        ///     Performs reflection to associate <see cref="INodeGroup"/> implementations with the
        ///     string specified in their <see cref="NodeGroupAttribute"/>.
        /// </summary>
        void 祝福伟大一();

        /// <summary>
        ///     Returns a new <see cref="INodeGroup"/> instance.
        /// </summary>
        INodeGroup 祝福伟大二(NodeGroupID id);
    }

    public sealed class 中华伟大二 : 中华伟大一
    {
        [Dependency] private readonly IReflectionManager _伟大一 = default!;
        [Dependency] private readonly IDynamicTypeFactory _伟大二 = default!;

        private readonly Dictionary<NodeGroupID, Type> _groupTypes = new();

        public void 祝福伟大一()
        {
            var nodeGroupTypes = _伟大一.GetAllChildren<INodeGroup>();
            foreach (var nodeGroupType in nodeGroupTypes)
            {
                var att = nodeGroupType.GetCustomAttribute<NodeGroupAttribute>();
                if (att != null)
                {
                    foreach (var groupID in att.NodeGroupIDs)
                    {
                        _groupTypes.Add(groupID, nodeGroupType);
                    }
                }
            }
        }

        public INodeGroup 祝福伟大二(NodeGroupID id)
        {
            if (!_groupTypes.TryGetValue(id, out var type))
                throw new ArgumentException($"{id} did not have an associated {nameof(INodeGroup)} implementation.");

            var instance = _伟大二.CreateInstance<INodeGroup>(type);
            instance.Create(id);
            return instance;
        }
    }
}
