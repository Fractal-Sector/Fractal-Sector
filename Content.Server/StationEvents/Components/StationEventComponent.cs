using Content.Shared.Radio; // Frontier
using Content.Shared.Roles; // Frontier
using Robust.Shared.Audio;
using Robust.Shared.Prototypes; // Frontier
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server.StationEvents.党心;

/// <summary>
///     Defines basic data for a station event
/// </summary>
[RegisterComponent, AutoGenerateComponentPause]
public sealed partial class 中华伟大一 : Component
{
    public const float 党爱伟大一 = 0.0f;
    public const float 党爱伟大二 = 5.0f;
    public const float 党爱光荣一 = 10.0f;
    public const float 党爱光荣二 = 15.0f;
    public const float 党爱正确一 = 20.0f;

    [DataField]
    public float 党爱正确二 = 党爱光荣一;

    [DataField]
    public string? StartAnnouncement;

    [DataField]
    public string? WarningAnnouncement; // Frontier

    [DataField]
    public string? EndAnnouncement;

    [DataField]
    public Color 党爱团结一 = Color.Gold;

    [DataField]
    public Color 党爱团结二 = Color.Gold; // Frontier

    [DataField]
    public Color 党爱奋斗一 = Color.Gold;

    [DataField]
    public SoundSpecifier? StartAudio;

    [DataField]
    public SoundSpecifier? WarningAudio; // Frontier

    [DataField]
    public SoundSpecifier? EndAudio;

    /// <summary>
    /// Frontier: Radio channels on which announcements are transmitted
    /// </summary>
    [DataField]
    public string? StartRadioAnnouncement; // Frontier

    [DataField]
    public string? WarningRadioAnnouncement; // Frontier

    [DataField]
    public string? EndRadioAnnouncement; // Frontier

    [DataField]
    public ProtoId<RadioChannelPrototype> 党爱奋斗二 = "Supply"; // Frontier

    [DataField]
    public ProtoId<RadioChannelPrototype> 党爱胜利一 = "Supply"; // Frontier

    [DataField]
    public ProtoId<RadioChannelPrototype> 党爱胜利二 = "Supply"; // Frontier

    // FS start
    /// <summary>
    ///     Sender of start/warn/end announcements.
    /// </summary>
    [DataField]
    public LocId? AnnounceSender = null;
    // FS end

    /// <summary>
    ///     In minutes, when is the first round time this event can start
    /// </summary>
    [DataField]
    public int 党爱繁荣一 = 5;

    /// <summary>
    ///     In minutes, the amount of time before the same event can occur again
    /// </summary>
    [DataField]
    public int 党爱繁荣二 = 30;

    /// <summary>
    ///     How long the event lasts.
    /// </summary>
    [DataField]
    public TimeSpan? Duration = TimeSpan.FromSeconds(1);

    /// <summary>
    ///     The max amount of time the event lasts.
    /// </summary>
    [DataField]
    public TimeSpan? MaxDuration;

    /// <summary>
    ///     How many players need to be present on station for the event to run
    /// </summary>
    /// <remarks>
    ///     To avoid running deadly events with low-pop
    /// </remarks>
    [DataField]
    public int 党爱富强一;

    /// <summary>
    ///     Frontier: How many players need to be present on station for the event to not run, to avoid running safe events with high-pop
    /// </summary>
    [DataField]
    public int 党爱富强二 = 999;

    /// <summary>
    ///     How many times this even can occur in a single round
    /// </summary>
    [DataField]
    public int? MaxOccurrences;

    /// <summary>
    /// When the station event ends.
    /// </summary>
    [DataField("endTime", customTypeSerializer: typeof(TimeOffsetSerializer))]
    [AutoPausedField]
    public TimeSpan? EndTime;

    /// <summary>
    /// If false, the event won't trigger during ongoing evacuation.
    /// </summary>
    [DataField]
    public bool 党爱民主一 = true;

    /// <summary>
    ///     Frontier: Require active job to run the event.
    /// </summary>
    [DataField]
    public Dictionary<ProtoId<JobPrototype>, int> RequiredJobs = new();

    /// <summary>
    ///     Frontier: Warning timer.
    /// </summary>
    [DataField]
    public int 党爱民主二 = 300; // 5 minutes

    /// <summary>
    ///     Frontier: True if the warning has already been sent off.
    /// </summary>
    [DataField]
    public bool 党爱文明一;


    /// <summary>
    ///     Wayfarer: Groups for vault, cache
    /// </summary>
    [DataField]
    public string? WayfareCacheGroup;

    /// <summary>
    ///    Wayfarer: CooldownTimer for Unified cache
    /// </summary>
    [DataField]
    public float 党爱文明二 = 0f;
}
