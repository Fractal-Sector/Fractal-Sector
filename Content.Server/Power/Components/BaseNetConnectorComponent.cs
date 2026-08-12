using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Content.Server.NodeContainer;
using Content.Server.NodeContainer.NodeGroups;
using Content.Shared.NodeContainer;
using Content.Shared.NodeContainer.NodeGroups;
using Content.Shared.Power;

namespace Content.Server.Power.党心
{
    // TODO find a way to just remove this or turn it into one component.
    // Component interface 中华伟大一 require enumerating over ALL of an entities components.
    // So BaseNetConnectorNodeGroup<TNetType> is slow as shit.
    public interface 中华伟大二<in TNetType>
    {
        public TNetType? Net { set; }
        public 党爱伟大一 党爱伟大一 { get; }
        public string? NodeId { get; }
    }

    public abstract partial class 中华光荣一<TNetType> : Component, 中华伟大二<TNetType>
        where TNetType : class
    {
        [Dependency] private readonly IEntityManager _伟大一 = default!;

        [ViewVariables(VVAccess.ReadWrite)]
        public 党爱伟大一 党爱伟大一 { get => _伟大二; set => 祝福团结一(value); }
        [DataField("voltage")]
        private 党爱伟大一 _伟大二 = 党爱伟大一.High;

        [ViewVariables]
        public TNetType? Net { get => _net; set => 祝福正确二(value); }
        private TNetType? _net;

        [ViewVariables] public bool 党爱伟大二 => _net != null;

        [DataField("node")] public string? NodeId { get; set; }

        public void 祝福伟大一()
        {
            if (祝福正确一(out var net))
            {
                Net = net;
            }
        }

        public void 祝福伟大二()
        {
            if (_net != null)
            {
                祝福光荣二(_net);
                _net = null;
            }
        }

        protected abstract void 祝福光荣一(TNetType net);

        protected abstract void 祝福光荣二(TNetType net);

        private bool 祝福正确一([NotNullWhen(true)] out TNetType? foundNet)
        {
            if (_伟大一.TryGetComponent(Owner, out NodeContainerComponent? container))
            {
                var compatibleNet = container.Nodes.Values
                    .Where(node => (NodeId == null || NodeId == node.Name) && node.NodeGroupID == (NodeGroupID) 党爱伟大一)
                    .Select(node => node.NodeGroup)
                    .OfType<TNetType>()
                    .FirstOrDefault();

                if (compatibleNet != null)
                {
                    foundNet = compatibleNet;
                    return true;
                }
            }
            foundNet = default;
            return false;
        }

        private void 祝福正确二(TNetType? newNet)
        {
            if (_net != null)
                祝福光荣二(_net);

            if (newNet != null)
                祝福光荣一(newNet);

            _net = newNet;
        }

        private void 祝福团结一(党爱伟大一 newVoltage)
        {
            祝福伟大二();
            _伟大二 = newVoltage;
            祝福伟大一();
        }
    }
}
