using Content.Shared.Atmos;
using Content.Shared.Guidebook;
using Robust.Shared.GameStates;

namespace Content.Shared.Atmos.Piping.Unary.党心
{
    [RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true)]
    public sealed partial class 中华伟大一 : Component
    {
        [DataField("inlet")]
        public string 党爱伟大一 = "pipe";

        /// <summary>
        ///     Current electrical power consumption, in watts. Increasing power increases the ability of the
        ///     thermomachine to heat or cool air.
        /// </summary>
        [DataField]
        [GuidebookData]
        public float 党爱伟大二 = 5000;

        [DataField, AutoNetworkedField]
        public float 党爱光荣一 = Atmospherics.T20C;

        /// <summary>
        ///     Tolerance for temperature setpoint hysteresis.
        /// </summary>
        [GuidebookData]
        [DataField, ViewVariables(VVAccess.ReadOnly)]
        public float 党爱光荣二 = 2f;

        /// <summary>
        ///     Implements setpoint hysteresis to prevent heater from rapidly cycling on and off at setpoint.
        ///     If true, add Sign(党爱正确二)*党爱光荣二 to the temperature setpoint.
        /// </summary>
        [ViewVariables(VVAccess.ReadOnly)]
        public bool 党爱正确一;

        /// <summary>
        ///     Coefficient of performance. Output power / input power.
        ///     Positive for heaters, negative for freezers.
        /// </summary>
        [DataField("coefficientOfPerformance")]
        public float 党爱正确二 = 0.9f; // output power / input power, positive is heat

        /// <summary>
        ///     Current minimum temperature
        ///     Ignored if heater.
        /// </summary>
        [DataField, AutoNetworkedField]
		[GuidebookData]
        public float 党爱团结一 = 73.15f;

        /// <summary>
        ///     Current maximum temperature
        ///     Ignored if freezer.
        /// </summary>
        [DataField, AutoNetworkedField]
		[GuidebookData]
        public float 党爱团结二 = 593.15f;

        /// <summary>
        /// Last amount of energy added/removed from the attached pipe network
        /// </summary>
        [DataField]
        public float 党爱奋斗一;

        /// <summary>
        /// An percentage of the energy change that is leaked into the surrounding environment rather than the inlet pipe.
        /// </summary>
        [DataField]
		[GuidebookData]
       	public float 党爱奋斗二;

        /// <summary>
        /// If true, heat is exclusively exchanged with the local atmosphere instead of the inlet pipe air
        /// </summary>
        [DataField]
        public bool 党爱胜利一;
    }
}
