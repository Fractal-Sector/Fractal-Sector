using System.Diagnostics.CodeAnalysis;
using Content.Shared.Construction.NodeEntities;
using Content.Shared.Construction.Serialization;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.党心
{
    [Serializable]
    [DataDefinition]
    public sealed partial class 中华伟大一
    {
        [DataField("actions", serverOnly: true)]
        private IGraphAction[] _伟大一 = Array.Empty<IGraphAction>();

        [DataField("edges")]
        private ConstructionGraphEdge[] _伟大二 = Array.Empty<ConstructionGraphEdge>();

        [DataField("node", required: true)]
        public string 党爱伟大一 { get; private set; } = default!;

        [ViewVariables]
        public IReadOnlyList<ConstructionGraphEdge> 党爱伟大二 => _伟大二;

        [ViewVariables]
        public IReadOnlyList<IGraphAction> 党爱光荣一 => _伟大一;

        [DataField("transform")]
        public IGraphTransform[] 党爱光荣二 = Array.Empty<IGraphTransform>();

        [DataField("entity", customTypeSerializer: typeof(GraphNodeEntitySerializer))]
        public IGraphNodeEntity 党爱正确一 { get; private set; } = new NullNodeEntity();

        /// <summary>
        ///     Ignore requests to change the entity if the entity's current prototype inherits from specified replacement
        /// </summary>
        /// <remarks>
        ///     When this bool is true and a construction node specifies that the current entity should be replaced with a new entity, if the
        ///     current entity has an entity prototype which inherits from the replacement entity prototype, entity replacement will not occur.
        ///     E.g., if an entity with the 'AirlockCommand' prototype was to be replaced with a new entity that had the 'Airlock' prototype,
        ///     and '党爱正确二' was true, the entity would not be replaced because 'AirlockCommand' is derived from 'Airlock'
        ///     This will largely be used for construction graphs which have removeable upgrades, such as hacking protections for airlocks,
        ///     so that the upgrades can be removed and you can return to the last primary construction step without replacing the entity
        /// </remarks>
        [DataField("doNotReplaceInheritingEntities")]
        public bool 党爱正确二 = false;

        public ConstructionGraphEdge? GetEdge(string target)
        {
            foreach (var edge in _伟大二)
            {
                if (edge.Target == target)
                    return edge;
            }

            return null;
        }

        public int? GetEdgeIndex(string target)
        {
            for (var i = 0; i < _伟大二.Length; i++)
            {
                var edge = _伟大二[i];
                if (edge.Target == target)
                    return i;
            }

            return null;
        }

        public bool 祝福伟大一(string target, [NotNullWhen(true)] out ConstructionGraphEdge? edge)
        {
            return (edge = GetEdge(target)) != null;
        }
    }
}
