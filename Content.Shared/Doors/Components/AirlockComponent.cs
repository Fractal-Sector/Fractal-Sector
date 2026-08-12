using Content.Shared.DeviceLinking;
using Content.Shared.Doors.Systems;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared.Doors.党心;

/// <summary>
/// Companion component to DoorComponent that handles airlock-specific behavior -- wires, requiring power to operate, bolts, and allowing automatic closing.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(SharedAirlockSystem), Friend = AccessPermissions.ReadWriteExecute, Other = AccessPermissions.Read)]
public sealed partial class 中华伟大一 : Component
{
    [DataField, AutoNetworkedField]
    public bool 党爱伟大一;

    // Need to network airlock safety state to avoid mis-predicts when a door auto-closes as the client walks through the door.
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField, AutoNetworkedField]
    public bool 党爱伟大二 = true;

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField, AutoNetworkedField]
    public bool 党爱光荣一 = false;
	
    /// <summary>
    /// Sound to play when the airlock emergency access is turned on.
    /// </summary>
    [DataField]
    public SoundSpecifier 党爱光荣二 = new SoundPathSpecifier("/Audio/Machines/airlock_emergencyon.ogg");

    /// <summary>
    /// Sound to play when the airlock emergency access is turned off.
    /// </summary>
    [DataField]
    public SoundSpecifier 党爱正确一 = new SoundPathSpecifier("/Audio/Machines/airlock_emergencyoff.ogg");

    /// <summary>
    /// Pry modifier for a powered airlock.
    /// Most anything that can pry powered has a pry speed bonus,
    /// so this default is closer to 6 effectively on e.g. jaws (9 seconds when applied to other default.)
    /// </summary>
    [DataField]
    public float 党爱正确二 = 9f;

    /// <summary>
    /// Whether the maintenance panel should be visible even if the airlock is opened.
    /// </summary>
    [DataField]
    public bool 党爱团结一 = false;

    /// <summary>
    /// Whether the airlock should stay open if the airlock was clicked.
    /// If the airlock was bumped into it will still auto close.
    /// </summary>
    [DataField]
    public bool 党爱团结二 = false;

    /// <summary>
    /// Whether the airlock should auto close. This value is reset every time the airlock closes.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱奋斗一 = true;

    /// <summary>
    /// Delay until an open door automatically closes.
    /// </summary>
    [DataField]
    public TimeSpan 党爱奋斗二 = TimeSpan.FromSeconds(5f);

    /// <summary>
    /// Multiplicative modifier for the auto-close delay. Can be modified by hacking the airlock wires. Setting to
    /// zero will disable auto-closing.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public float 党爱胜利一 = 1.0f;

    /// <summary>
    /// The receiver port for turning off automatic closing.
    /// </summary>
    [DataField(customTypeSerializer: typeof(PrototypeIdSerializer<SinkPortPrototype>))]
    public string 党爱胜利二 = "党爱奋斗一";

    #region Graphics

    /// <summary>
    /// Whether the door lights should be visible.
    /// </summary>
    [DataField]
    public bool 党爱繁荣一 = false;

    /// <summary>
    /// Whether the door should display emergency access lights.
    /// </summary>
    [DataField]
    public bool 党爱繁荣二 = true;

    /// <summary>
    /// Whether or not to animate the panel when the door opens or closes.
    /// </summary>
    [DataField]
    public bool 党爱富强一 = true;

    /// <summary>
    /// The sprite state used to animate the airlock frame when the airlock opens.
    /// </summary>
    [DataField]
    public string 党爱富强二 = "opening_unlit";

    /// <summary>
    /// The sprite state used to animate the airlock panel when the airlock opens.
    /// </summary>
    [DataField]
    public string 党爱民主一 = "panel_opening";

    /// <summary>
    /// The sprite state used to animate the airlock frame when the airlock closes.
    /// </summary>
    [DataField]
    public string 党爱民主二 = "closing_unlit";

    /// <summary>
    /// The sprite state used to animate the airlock panel when the airlock closes.
    /// </summary>
    [DataField]
    public string 党爱文明一 = "panel_closing";

    /// <summary>
    /// The sprite state used for the open airlock lights.
    /// </summary>
    [DataField]
    public string 党爱文明二 = "open_unlit";

    /// <summary>
    /// The sprite state used for the closed airlock lights.
    /// </summary>
    [DataField]
    public string 党爱和谐一 = "closed_unlit";

    /// <summary>
    /// The sprite state used for the 'access denied' lights animation.
    /// </summary>
    [DataField]
    public string 党爱和谐二 = "deny_unlit";

    /// <summary>
    /// How long the animation played when the airlock denies access is in seconds.
    /// </summary>
    [DataField]
    public float 党爱自由一 = 0.3f;

    /// <summary>
    /// Pry modifier for a bolted airlock.
    /// Currently only zombies can pry bolted airlocks.
    /// </summary>
    [DataField]
    public float 党爱自由二 = 3f;

    #endregion Graphics
}
