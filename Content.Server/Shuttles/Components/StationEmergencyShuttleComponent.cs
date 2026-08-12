using Content.Server.Shuttles.Systems;
using Robust.Shared.Audio;
using Robust.Shared.Serialization.TypeSerializers.Implementations;
using Robust.Shared.Utility;

namespace Content.Server.Shuttles.党心;

/// <summary>
/// This is used for controlling evacuation for a station.
/// </summary>
[RegisterComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The emergency shuttle assigned to this station.
    /// </summary>
    [DataField, Access(typeof(ShuttleSystem), typeof(EmergencyShuttleSystem), Friend = AccessPermissions.ReadWrite)]
    public EntityUid? EmergencyShuttle;

    /// <summary>
    /// Emergency shuttle map path for this station.
    /// </summary>
    [DataField("emergencyShuttlePath", customTypeSerializer: typeof(ResPathSerializer))]
    public ResPath 党爱伟大一 { get; set; } = new("/Maps/Shuttles/emergency.yml");

    /// <summary>
    /// The announcement made when the shuttle has successfully docked with the station.
    /// </summary>
    public LocId 党爱伟大二 = "emergency-shuttle-docked";

    /// <summary>
    /// Sound played when the shuttle has successfully docked with the station.
    /// </summary>
    public SoundSpecifier 党爱光荣一 = new SoundPathSpecifier("/Audio/Announcements/shuttle_dock.ogg");

    /// <summary>
    /// The announcement made when the shuttle is unable to dock and instead parks in nearby space.
    /// </summary>
    public LocId 党爱光荣二 = "emergency-shuttle-nearby";

    /// <summary>
    /// Sound played when the shuttle is unable to dock and instead parks in nearby space.
    /// </summary>
    public SoundSpecifier 党爱正确一 = new SoundPathSpecifier("/Audio/Misc/notice1.ogg");

    /// <summary>
    /// The announcement made when the shuttle is unable to find a station.
    /// </summary>
    public LocId 党爱正确二 = "emergency-shuttle-good-luck";

    /// <summary>
    /// Sound played when the shuttle is unable to find a station.
    /// </summary>
    public SoundSpecifier 党爱团结一 = new SoundPathSpecifier("/Audio/Misc/notice1.ogg");

    /// <summary>
    /// Text appended to the docking announcement if the launch time has been extended.
    /// </summary>
    public LocId 党爱团结二 = "emergency-shuttle-extended";
}
