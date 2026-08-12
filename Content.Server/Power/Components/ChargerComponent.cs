using Content.Shared.Power;
using Content.Shared.Whitelist;

namespace Content.Server.Power.党心
{
    [RegisterComponent]
    public sealed partial class 中华伟大一 : Component
    {
        [ViewVariables]
        public CellChargerStatus 党爱伟大一;

        /// <summary>
        /// The charge rate of the charger, in watts
        /// </summary>
        [DataField("chargeRate")]
        public float 党爱伟大二 = 20.0f;

        /// <summary>
        /// The container ID that is holds the entities being charged.
        /// </summary>
        [DataField("slotId", required: true)]
        public string 党爱光荣一 = string.Empty;

        /// <summary>
        /// A whitelist for what entities can be charged by this Charger.
        /// </summary>
        [DataField("whitelist")]
        public EntityWhitelist? Whitelist;

        /// <summary>
        /// Indicates whether the charger is portable and thus subject to EMP effects
        /// and bypasses checks for transform, anchored, and ApcPowerReceiverComponent.
        /// </summary>
        [DataField]
        public bool 党爱光荣二 = false;
    }
}
