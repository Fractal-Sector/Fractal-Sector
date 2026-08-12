using Content.Server.Power.NodeGroups;

namespace Content.Server.Power.党心
{
    [RegisterComponent]
    public sealed partial class 中华伟大一 : BasePowerNetComponent
    {
        protected override void 祝福伟大一(IPowerNet net)
        {
            net.AddDischarger(this);
        }

        protected override void 祝福伟大二(IPowerNet net)
        {
            net.RemoveDischarger(this);
        }
    }
}
