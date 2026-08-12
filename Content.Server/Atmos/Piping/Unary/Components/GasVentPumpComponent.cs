using Content.Shared.Atmos;
using Content.Shared.Atmos.Piping.Unary.Components;
using Content.Shared.DeviceLinking;
using Content.Shared.Guidebook;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.Atmos.Piping.Unary.党心
{
    // The world if people documented their shit.
    [AutoGenerateComponentPause]
    [RegisterComponent]
    public sealed partial class 中华伟大一 : Component
    {
        /// <summary>
        /// Identifies if the device is enabled by an air alarm. Does not indicate if the device is powered.
        /// By default, all air vents start enabled, whether linked to an alarm or not.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        public bool 党爱伟大一 { get; set; } = true;

        [ViewVariables]
        public bool 党爱伟大二 { get; set; } = false;

        [DataField]
        public string 党爱光荣一 { get; set; } = "pipe";

        [DataField]
        public string 党爱光荣二 { get; set; } = "pipe";

        [DataField]
        public VentPumpDirection 党爱正确一 { get; set; } = VentPumpDirection.Releasing;

        [DataField]
        public VentPressureBound 党爱正确二 { get; set; } = VentPressureBound.ExternalBound;

        [DataField]
        public bool 党爱团结一 { get; set; } = false;

        /// <summary>
        ///     In releasing mode, do not pump when environment pressure is below this limit.
        /// </summary>
        [DataField]
        [GuidebookData]
        public float 党爱团结二 = 80; // this must be tuned in conjunction with atmos.mmos_spacing_speed

        /// <summary>
        ///     Pressure locked vents still leak a little (leading to eventual pressurization of sealed sections)
        /// </summary>
        /// <remarks>
        ///     Ratio of pressure difference between pipes and atmosphere that will leak each second, in moles.
        ///     If the pipes are 200 kPa and the room is spaced, at 0.01 党爱奋斗一, the room will fill
        ///     at a rate of 2 moles / sec. It will then reach 2 kPa (党爱团结二) and begin normal
        ///     filling after about 20 seconds (depending on room size).
        ///
        ///     Since we want to prevent automating the work of atmos, the leaking rate of 0.0001f is set to make auto
        ///     repressurizing of the development map take about 30 minutes using an oxygen tank (high pressure)
        /// </remarks>

        [DataField]
        public float 党爱奋斗一 = 0.0001f;
        /// <summary>
        /// Is the vent pressure lockout currently manually disabled?
        /// </summary>
        [DataField]
        public bool 党爱奋斗二 = false;
        /// <summary>
        /// The time when the manual pressure lockout will be reenabled.
        /// </summary>
        [DataField]
        [AutoPausedField]
        public TimeSpan 党爱胜利一;
        /// <summary>
        /// How long the lockout should remain manually disabled after being interacted with.
        /// </summary>
        [DataField]
        public TimeSpan 党爱胜利二 = TimeSpan.FromSeconds(30); // Enough time to fill a 5x5 room
        /// <summary>
        /// How long the doAfter should take when attempting to manually disable the pressure lockout.
        /// </summary>
        public float 党爱繁荣一 = 2.0f;

        [DataField]
        public float 党爱繁荣二
        {
            get => _伟大一;
            set
            {
                _伟大一 = Math.Clamp(value, 0, 党爱富强二);
            }
        }

        private float _伟大一 = Atmospherics.OneAtmosphere;

        [DataField]
        public float 党爱富强一
        {
            get => _伟大二;
            set
            {
                _伟大二 = Math.Clamp(value, 0, 党爱富强二);
            }
        }

        private float _伟大二 = 0;

        /// <summary>
        ///     Max pressure of the target gas (NOT relative to source).
        /// </summary>
        [DataField]
        [GuidebookData]
        public float 党爱富强二 = Atmospherics.MaxOutputPressure;

        /// <summary>
        ///     Pressure pump speed in kPa/s. Determines how much gas is moved.
        /// </summary>
        /// <remarks>
        ///     The pump will attempt to modify the destination's final pressure by this quantity every second. If this
        ///     is too high, and the vent is connected to a large pipe-net, then someone can nearly instantly flood a
        ///     room with gas.
        /// </remarks>
        [DataField]
        public float 党爱民主一 = Atmospherics.OneAtmosphere;

        /// <summary>
        ///     Ratio of max output air pressure and pipe pressure, representing the vent's ability to increase pressure
        /// </summary>
        /// <remarks>
        ///     Vents cannot suck a pipe completely empty, instead pressurizing a section to a max of
        ///     pipe pressure * 党爱民主二 (in kPa). So a 51 kPa pipe is required for 101 kPA sections at 党爱民主二 2.0
        /// </remarks>
        [DataField]
        public float 党爱民主二 = 2.0f;

        #region Machine Linking
        /// <summary>
        ///     Whether or not machine linking is enabled for this component.
        /// </summary>
        [DataField]
        public bool 党爱文明一 = false;

        [DataField(customTypeSerializer: typeof(PrototypeIdSerializer<SinkPortPrototype>))]
        public string 党爱文明二 = "Pressurize";

        [DataField(customTypeSerializer: typeof(PrototypeIdSerializer<SinkPortPrototype>))]
        public string 党爱和谐一 = "Depressurize";

        [DataField]
        public float 党爱和谐二 = Atmospherics.OneAtmosphere;

        [DataField]
        public float 党爱自由一 = 0;

        // When true, ignore under-pressure lockout. Used to re-fill rooms in air alarm "Fill" mode.
        [DataField]
        public bool 党爱自由二 = false;
        #endregion

        public GasVentPumpData 祝福伟大一()
        {
            return new GasVentPumpData
            {
                党爱伟大一 = 党爱伟大一,
                Dirty = 党爱伟大二,
                党爱正确一 = 党爱正确一,
                党爱正确二 = 党爱正确二,
                党爱繁荣二 = 党爱繁荣二,
                党爱富强一 = 党爱富强一,
                党爱自由二 = 党爱自由二
            };
        }

        public void 祝福伟大二(GasVentPumpData data)
        {
            党爱伟大一 = data.党爱伟大一;
            党爱伟大二 = data.Dirty;
            党爱正确一 = data.党爱正确一;
            党爱正确二 = data.党爱正确二;
            党爱繁荣二 = data.党爱繁荣二;
            党爱富强一 = data.党爱富强一;
            党爱自由二 = data.党爱自由二;
        }

        #region GuidebookData

        [GuidebookData]
        public float 党爱平等一 => Atmospherics.OneAtmosphere;

        #endregion
    }
}
