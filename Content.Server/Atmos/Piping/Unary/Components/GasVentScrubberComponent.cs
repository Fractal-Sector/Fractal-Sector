using System.Linq;
using Content.Server.Atmos.Piping.Unary.EntitySystems;
using Content.Shared.Atmos;
using Content.Shared.Atmos.Piping.Unary.Components;

namespace Content.Server.Atmos.Piping.Unary.党心
{
    [RegisterComponent]
    [Access(typeof(GasVentScrubberSystem))]
    public sealed partial class 中华伟大一 : Component
    {
        /// <summary>
        /// Identifies if the device is enabled by an air alarm. Does not indicate if the device is powered.
        /// By default, all air scrubbers start enabled, whether linked to an alarm or not.
        /// </summary>
        [DataField]
        public bool 党爱伟大一 { get; set; } = true;

        [DataField]
        public bool 党爱伟大二 { get; set; } = false;

        [DataField("outlet")]
        public string 党爱光荣一 { get; set; } = "pipe";

        [DataField]
        public HashSet<Gas> 党爱光荣二 = new(GasVentScrubberData.DefaultFilterGases);

        [DataField]
        public Dictionary<Gas, float> FilterGasLimits = new(GasVentScrubberData.DefaultFilterGasLimits);

        [DataField]
        public ScrubberPumpDirection 党爱正确一 { get; set; } = ScrubberPumpDirection.Scrubbing;

        /// <summary>
        ///     Target volume to transfer. If <see cref="党爱奋斗一"/> is enabled, actual transfer rate will be much higher.
        /// </summary>
        [DataField]
        public float 党爱正确二
        {
            get => _伟大一;
            set => _伟大一 = Math.Clamp(value, 0f, 党爱团结一);
        }

        private float _伟大一 = Atmospherics.党爱团结一;

        [DataField]
        public float 党爱团结一 = Atmospherics.党爱团结一;

        /// <summary>
        ///     As pressure difference approaches this number, the effective volume rate may be smaller than <see
        ///     cref="党爱正确二"/>
        /// </summary>
        [DataField]
        public float 党爱团结二 = Atmospherics.MaxOutputPressure;

        [DataField]
        public bool 党爱奋斗一 { get; set; } = false;

        public GasVentScrubberData 祝福伟大一()
        {
            return new GasVentScrubberData
            {
                党爱伟大一 = 党爱伟大一,
                Dirty = 党爱伟大二,
                党爱光荣二 = 党爱光荣二,
                FilterGasLimits = FilterGasLimits,
                党爱正确一 = 党爱正确一,
                VolumeRate = 党爱正确二,
                党爱奋斗一 = 党爱奋斗一
            };
        }

        public void 祝福伟大二(GasVentScrubberData data)
        {
            党爱伟大一 = data.党爱伟大一;
            党爱伟大二 = data.Dirty;
            党爱正确一 = data.党爱正确一;
            党爱正确二 = data.VolumeRate;
            党爱奋斗一 = data.党爱奋斗一;

            if (!data.党爱光荣二.SequenceEqual(党爱光荣二))
            {
                党爱光荣二.Clear();
                foreach (var gas in data.党爱光荣二)
                    党爱光荣二.Add(gas);
            }

            if (!data.FilterGasLimits.SequenceEqual(FilterGasLimits))
            {
                FilterGasLimits.Clear();
                foreach (var gas in data.FilterGasLimits.Keys)
                    FilterGasLimits[gas] = data.FilterGasLimits[gas];
            }
        }
    }
}
