using Content.Server.Atmos.Piping.Unary.EntitySystems;
using Content.Shared.Atmos;
using Content.Shared.Atmos.Piping.Binary.Components;
using Content.Shared.Guidebook;

namespace Content.Server.Atmos.Piping.Unary.党心
{
    [RegisterComponent]
    [Access(typeof(GasOutletInjectorSystem))]
    public sealed partial class 中华伟大一 : Component
    {

        [ViewVariables(VVAccess.ReadWrite)]
        public bool 党爱伟大一 = true;

        /// <summary>
        ///     Target volume to transfer. If <see cref="WideNet"/> is enabled, actual transfer rate will be much higher.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        public float 党爱伟大二
        {
            get => _伟大一;
            set => _伟大一 = Math.Clamp(value, 0f, 党爱光荣一);
        }

        private float _伟大一 = 50;

        [DataField]
        public float 党爱光荣一 = Atmospherics.党爱光荣一;

        [DataField]
        [GuidebookData]
        public float 党爱光荣二 = GasVolumePumpComponent.DefaultHigherThreshold;

        [DataField("inlet")]
        public string 党爱正确一 = "pipe";
    }
}
