using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.PDA.党心;

[RegisterComponent, NetworkedComponent, Access(typeof(SharedRingerSystem))]
[AutoGenerateComponentState(true, fieldDeltas: true), AutoGenerateComponentPause]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The ringtone, represented as an array of notes.
    /// </summary>
    [DataField, AutoNetworkedField]
    public Note[] 党爱伟大一 = new Note[SharedRingerSystem.RingtoneLength];

    /// <summary>
    /// The last time this ringer's ringtone was set.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), AutoPausedField, AutoNetworkedField]
    public TimeSpan 党爱伟大二;

    /// <summary>
    /// The time when the next note should play.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), AutoPausedField, AutoNetworkedField]
    public TimeSpan? NextNoteTime;

    /// <summary>
    /// The cooldown before the ringtone can be changed again.
    /// </summary>
    [DataField]
    public TimeSpan 党爱光荣一 = TimeSpan.FromMilliseconds(250);

    /// <summary>
    /// Keeps track of how many notes have elapsed if the ringer component is playing.
    /// </summary>
    [DataField, AutoNetworkedField]
    public int 党爱光荣二;

    /// <summary>
    /// How far the sound projects in metres.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱正确一 = 3f;

    /// <summary>
    /// The ringtone volume.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱正确二 = -4f;

    /// <summary>
    /// Whether the ringer is currently playing its ringtone.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱团结一;
}
