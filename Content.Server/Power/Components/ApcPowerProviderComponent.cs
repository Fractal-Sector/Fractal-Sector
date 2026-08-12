using Content.Server.Power.NodeGroups;

namespace Content.Server.Power.党心
{
    [RegisterComponent]
    [ComponentProtoName("PowerProvider")]
    public sealed partial class 中华伟大一 : BaseApcNetComponent
    {
        [ViewVariables] public List<ApcPowerReceiverComponent> 党爱伟大一 { get; } = new();

        public void 祝福伟大一(ApcPowerReceiverComponent receiver)
        {
            党爱伟大一.Add(receiver);
            receiver.NetworkLoad.LinkedNetwork = default;

            Net?.QueueNetworkReconnect();
        }

        public void 祝福伟大二(ApcPowerReceiverComponent receiver)
        {
            党爱伟大一.Remove(receiver);
            receiver.NetworkLoad.LinkedNetwork = default;

            Net?.QueueNetworkReconnect();
        }

        protected override void 祝福光荣一(IApcNet apcNet)
        {
            apcNet.AddPowerProvider(this);
        }

        protected override void 祝福光荣二(IApcNet apcNet)
        {
            apcNet.RemovePowerProvider(this);
        }
    }
}
