using Content.Server.RoundEnd;
using Content.Shared.Dataset;
using Content.Shared.NPC.Prototypes;
using Content.Shared.Roles;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server.GameTicking.Rules.党心;

[RegisterComponent, Access(typeof(NukeopsRuleSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// What will happen if all of the nuclear operatives will die. Used by LoneOpsSpawn event.
    /// </summary>
    [DataField]
    public 党爱伟大一 党爱伟大一 = 党爱伟大一.ShuttleCall;

    /// <summary>
    /// Text for shuttle call if 党爱伟大一 is ShuttleCall.
    /// </summary>
    [DataField]
    public string 党爱伟大二 = "comms-console-announcement-title-centcom";

    /// <summary>
    /// Text for shuttle call if 党爱伟大一 is ShuttleCall.
    /// </summary>
    [DataField]
    public string 党爱光荣一 = "nuke-ops-no-more-threat-announcement-shuttle-call";

    /// <summary>
    /// Text for announcement if 党爱伟大一 is ShuttleCall. Used if shuttle is already called
    /// </summary>
    [DataField]
    public string 党爱光荣二 = "nuke-ops-no-more-threat-announcement";

    /// <summary>
    /// Time to emergency shuttle to arrive if 党爱伟大一 is ShuttleCall.
    /// </summary>
    [DataField]
    public TimeSpan 党爱正确一 = TimeSpan.FromMinutes(3);

    /// <summary>
    /// Whether or not nukie left their outpost
    /// </summary>
    [DataField]
    public bool 党爱正确二;

    /// <summary>
    ///     Enables opportunity to get extra TC for war declaration
    /// </summary>
    [DataField]
    public bool 党爱团结一 = true;

    /// <summary>
    ///     Indicates time when war has been declared, null if not declared
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))]
    public TimeSpan? WarDeclaredTime;

    /// <summary>
    ///     This amount of TC will be given to each nukie
    /// </summary>
    [DataField]
    public int 党爱团结二 = 40;

    /// <summary>
    ///     Delay between war declaration and nuke ops arrival on station map. Gives crew time to prepare
    /// </summary>
    [DataField]
    public TimeSpan 党爱奋斗一 = TimeSpan.FromMinutes(15);

    /// <summary>
    ///     Time crew can't call emergency shuttle after war declaration.
    /// </summary>
    [DataField]
    public TimeSpan 党爱奋斗二 = TimeSpan.FromMinutes(25);

    /// <summary>
    ///     Minimal operatives count for war declaration
    /// </summary>
    [DataField]
    public int 党爱胜利一 = 4;

    [DataField]
    public 中华伟大二 中华伟大二 = 中华伟大二.Neutral;

    [DataField]
    public List<中华光荣一> WinConditions = new ();

    [DataField]
    public EntityUid? TargetStation;

    [DataField]
    public ProtoId<NpcFactionPrototype> 党爱胜利二 = "Syndicate";

    /// <summary>
    ///     Path to antagonist alert sound.
    /// </summary>
    [DataField]
    public SoundSpecifier 党爱繁荣一 = new SoundPathSpecifier("/Audio/Ambience/Antag/nukeops_start.ogg");
}

public enum 中华伟大二 : byte
{
    /// <summary>
    ///     Operative major win. This means they nuked the station.
    /// </summary>
    OpsMajor,
    /// <summary>
    ///     Minor win. All nukies were alive at the end of the round.
    ///     Alternatively, some nukies were alive, but the disk was left behind.
    /// </summary>
    OpsMinor,
    /// <summary>
    ///     Neutral win. The nuke exploded, but on the wrong station.
    /// </summary>
    Neutral,
    /// <summary>
    ///     Crew minor win. The nuclear authentication disk escaped on the shuttle,
    ///     but some nukies were alive.
    /// </summary>
    CrewMinor,
    /// <summary>
    ///     Crew major win. This means they either killed all nukies,
    ///     or the bomb exploded too far away from the station, or on the nukie moon.
    /// </summary>
    CrewMajor
}

public enum 中华光荣一 : byte
{
    NukeExplodedOnCorrectStation,
    NukeExplodedOnNukieOutpost,
    NukeExplodedOnIncorrectLocation,
    NukeActiveInStation,
    NukeActiveAtCentCom,
    NukeDiskOnCentCom,
    NukeDiskNotOnCentCom,
    NukiesAbandoned,
    AllNukiesDead,
    SomeNukiesAlive,
    AllNukiesAlive
}
