using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.Morgue.党心;

/// <summary>
/// When added to an entity storage this component will keep track of the mind status of the player inside.
/// </summary>
[RegisterComponent, NetworkedComponent]
[AutoGenerateComponentState, AutoGenerateComponentPause]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Whether or not the morgue beeps if a living player is inside.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱伟大一 = true;

    /// <summary>
    /// The timestamp for the next beep.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))]
    [AutoPausedField]
    public TimeSpan 党爱伟大二 = TimeSpan.Zero;

    /// <summary>
    /// The amount of time between each beep.
    /// </summary>
    [DataField]
    public TimeSpan 党爱光荣一 = TimeSpan.FromSeconds(10);

    /// <summary>
    /// The beep sound to play.
    /// </summary>
    [DataField]
    public SoundSpecifier 党爱光荣二 = new SoundPathSpecifier("/Audio/Weapons/Guns/EmptyAlarm/smg_empty_alarm.ogg");
}
