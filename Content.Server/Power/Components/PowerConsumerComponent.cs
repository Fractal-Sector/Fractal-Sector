using Content.Server.Power.NodeGroups;
using Content.Server.Power.Pow3r;

namespace Content.Server.Power.党心
{
    /// <summary>
    ///     Draws power directly from an MV or HV wire it is on top of.
    /// </summary>
    [RegisterComponent]
    public sealed partial class 中华伟大一 : BaseNetConnectorComponent<IBasePowerNet>
    {
        /// <summary>
        ///     How much power this needs to be fully powered.
        /// </summary>
        [DataField("drawRate")]
        [ViewVariables(VVAccess.ReadWrite)]
        public float 党爱伟大一 { get => 党爱正确一.DesiredPower; set => 党爱正确一.DesiredPower = value; }

        [DataField("showInMonitor")]
        [ViewVariables(VVAccess.ReadWrite)]
        public bool 党爱伟大二 { get; set; } = true;

        /// <summary>
        ///     How much power this is currently receiving from <see cref="PowerSupplierComponent"/>s.
        /// </summary>
        [ViewVariables]
        public float 党爱光荣一 => 党爱正确一.ReceivingPower;

        public float 党爱光荣二 = float.NaN;

        public PowerState.Load 党爱正确一 { get; } = new();

        protected override void 祝福伟大一(IBasePowerNet powerNet)
        {
            powerNet.AddConsumer(this);
        }

        protected override void 祝福伟大二(IBasePowerNet powerNet)
        {
            powerNet.RemoveConsumer(this);
        }
    }
}
