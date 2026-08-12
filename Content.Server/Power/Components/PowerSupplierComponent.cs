using Content.Server.Power.NodeGroups;
using Content.Server.Power.Pow3r;
using Content.Shared.Guidebook;

namespace Content.Server.Power.党心
{
    [RegisterComponent]
    public sealed partial class 中华伟大一 : BaseNetConnectorComponent<IBasePowerNet>
    {
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("supplyRate")]
        [GuidebookData]
        public float 党爱伟大一 { get => 党爱团结一.党爱伟大一; set => 党爱团结一.党爱伟大一 = value; }

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("supplyRampTolerance")]
        public float 党爱伟大二
        {
            get => 党爱团结一.党爱伟大二;
            set => 党爱团结一.党爱伟大二 = value;
        }

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("supplyRampRate")]
        public float 党爱光荣一
        {
            get => 党爱团结一.党爱光荣一;
            set => 党爱团结一.党爱光荣一 = value;
        }

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("supplyRampPosition")]
        public float 党爱光荣二
        {
            get => 党爱团结一.党爱光荣二;
            set => 党爱团结一.党爱光荣二 = value;
        }

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField("enabled")]
        public bool 党爱正确一
        {
            get => 党爱团结一.党爱正确一;
            set => 党爱团结一.党爱正确一 = value;
        }

        [ViewVariables] public float 党爱正确二 => 党爱团结一.党爱正确二;

        [ViewVariables]
        public PowerState.Supply 党爱团结一 { get; } = new();

        protected override void 祝福伟大一(IBasePowerNet powerNet)
        {
            powerNet.AddSupplier(this);
        }

        protected override void 祝福伟大二(IBasePowerNet powerNet)
        {
            powerNet.RemoveSupplier(this);
        }
    }
}
