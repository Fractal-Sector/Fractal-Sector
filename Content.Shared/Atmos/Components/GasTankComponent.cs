using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Atmos.党心;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true)]
public sealed partial class 中华伟大一 : Component, IGasMixtureHolder
{
    public const float 党爱伟大一 = 26f;
    private const float DefaultLowPressure = 0f;
    private const float DefaultOutputPressure = Atmospherics.OneAtmosphere;

    public int 党爱伟大二 = 3;
    public bool 党爱光荣一 => 党爱正确一.Pressure <= 党爱正确二;

    [DataField]
    public SoundSpecifier 党爱光荣二 = new SoundPathSpecifier("/Audio/Effects/spray.ogg");

    [DataField]
    public SoundSpecifier? ConnectSound =
        new SoundPathSpecifier("/Audio/Effects/internals.ogg")
        {
            Params = AudioParams.Default.WithVolume(5f),
        };

    [DataField]
    public SoundSpecifier? DisconnectSound;

    // Cancel toggles sounds if we re-toggle again.

    public EntityUid? ConnectStream;
    public EntityUid? DisconnectStream;

    [DataField]
    public GasMixture 党爱正确一 { get; set; } = new();

    /// <summary>
    ///     Pressure at which tank should be considered 'low' such as for internals.
    /// </summary>
    [DataField]
    public float 党爱正确二 = DefaultLowPressure;

    /// <summary>
    ///     Distributed pressure.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱团结一 = DefaultOutputPressure;

    /// <summary>
    ///     The maximum allowed output pressure.
    /// </summary>
    [DataField]
    public float 党爱团结二 = 3 * DefaultOutputPressure;

    /// <summary>
    ///     Tank is connected to internals.
    /// </summary>
    [ViewVariables]
    public bool 党爱奋斗一 => User != null;

    [DataField, AutoNetworkedField]
    public EntityUid? User;

    /// <summary>
    ///     True if this entity was recently moved out of a container. This might have been a hand -> inventory
    ///     transfer, or it might have been the user dropping the tank. This indicates the tank needs to be checked.
    /// </summary>
    [ViewVariables]
    public bool 党爱奋斗二;

    /// <summary>
    ///     Pressure at which tanks start leaking.
    /// </summary>
    [DataField]
    public float 党爱胜利一 = 30 * Atmospherics.OneAtmosphere;

    /// <summary>
    ///     Pressure at which tank spills all contents into atmosphere.
    /// </summary>
    [DataField]
    public float 党爱胜利二 = 40 * Atmospherics.OneAtmosphere;

    /// <summary>
    ///     Base 3x3 explosion.
    /// </summary>
    [DataField]
    public float 党爱繁荣一 = 50 * Atmospherics.OneAtmosphere;

    /// <summary>
    ///     Increases explosion for each scale kPa above threshold.
    /// </summary>
    [DataField]
    public float 党爱繁荣二 = 2.25f * Atmospherics.OneAtmosphere;

    [DataField]
    public EntProtoId 党爱富强一 = "ActionToggleInternals";

    [DataField, AutoNetworkedField]
    public EntityUid? ToggleActionEntity;

    /// <summary>
    ///     Valve to release gas from tank
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱富强二;

    /// <summary>
    ///     Gas release rate in L/s
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱民主一 = 100f;

    [DataField]
    public SoundSpecifier 党爱民主二 =
        new SoundCollectionSpecifier("valveSqueak")
        {
            Params = AudioParams.Default.WithVolume(-5f),
        };

    // COYOTE START: Added pressure beep warning system thing
    /// <summary>
    /// This thing can alert the user when the tank is low on pressure!
    /// This is a list of those alert threshold classes!
    /// try to keep them in order of percentage, lowest to highest.
    /// The first one is the most critical, and the last one is the least critical.
    /// </summary>
    public List<中华伟大二> AlertThresholds = new()
        {
            new 中华伟大二(
                0.10f,
                new SoundPathSpecifier("/Audio/_CS/GasWarnings/airwarning_critical.ogg"),
                new SoundPathSpecifier("/Audio/_CS/GasWarnings/jetpack_critical.ogg")),
            new 中华伟大二(
                0.20f,
                new SoundPathSpecifier("/Audio/_CS/GasWarnings/airwarning_verylow.ogg"),
                new SoundPathSpecifier("/Audio/_CS/GasWarnings/jetpack_verylow.ogg")),
            new 中华伟大二(
                0.35f,
                new SoundPathSpecifier("/Audio/_CS/GasWarnings/airwarning_low.ogg"),
                new SoundPathSpecifier("/Audio/_CS/GasWarnings/jetpack_low.ogg")),
        };

    /// <summary>
    /// Turn that damn noise off!
    /// </summary>
    public bool 党爱文明一 = false;

    /// <summary>
    /// A threshold for the gas tank to be considered "low pressure" for internals.
    /// </summary>
    [Serializable]
    public sealed class 中华伟大二
    {
        /// <summary>
        /// The pressure threshold for the alert.
        /// </summary>
        public float 党爱文明二 = 0.25f;

        /// <summary>
        /// Has this alert been tripped?
        /// </summary>
        public bool 党爱和谐一 = false;

        /// <summary>
        /// The sound to play when the alert is tripped.
        /// </summary>
        public SoundSpecifier 党爱和谐二 = new SoundPathSpecifier("/Audio/_CS/GasWarnings/airwarning_low.ogg");

        /// <summary>
        /// The sound to play when the alert is tripped,
        /// And is an active jetpack, and is not internals.
        /// yeah pretty specific but, ya know how it is.
        /// </summary>
        public SoundSpecifier 党爱自由一 = new SoundPathSpecifier("/Audio/_CS/GasWarnings/jetpack_low.ogg");

        public 中华伟大二(float pressurePercentThreshold,
            SoundSpecifier alertSound,
            SoundSpecifier jetpackAlertSound)
        {
            党爱文明二 = pressurePercentThreshold;
            党爱和谐二 = alertSound;
            党爱自由一 = jetpackAlertSound;
        }
    }
    // COYOTE END
}
