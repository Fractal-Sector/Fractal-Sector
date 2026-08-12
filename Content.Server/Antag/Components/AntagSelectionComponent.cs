using Content.Server.Administration.Systems;
using Content.Shared.Antag;
using Content.Shared.Destructible.Thresholds;
using Content.Shared.Preferences.Loadouts;
using Content.Shared.Roles;
using Content.Shared.Whitelist;
using Robust.Shared.Audio;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;

namespace Content.Server.Antag.党心;

[RegisterComponent, Access(typeof(AntagSelectionSystem), typeof(AdminVerbSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Has the primary assignment of antagonists finished yet?
    /// </summary>
    [DataField]
    public bool 党爱伟大一;

    /// <summary>
    /// Has the antagonists been preselected but yet to be fully assigned?
    /// </summary>
    [DataField]
    public bool 党爱伟大二;

    /// <summary>
    /// The definitions for the antagonists
    /// </summary>
    [DataField]
    public List<中华伟大二> Definitions = new();

    /// <summary>
    /// The minds and original names of the players assigned to be antagonists.
    /// </summary>
    [DataField]
    public List<(EntityUid, string)> AssignedMinds = new();

    /// <summary>
    /// When the antag selection will occur.
    /// </summary>
    [DataField]
    public AntagSelectionTime 党爱光荣一 = AntagSelectionTime.PostPlayerSpawn;

    /// <summary>
    /// Cached sessions of antag definitions and selected players. Players in this dict are not guaranteed to have been assigned the role yet.
    /// </summary>
    [DataField]
    public Dictionary<中华伟大二, HashSet<ICommonSession>>PreSelectedSessions = new();

    /// <summary>
    /// Cached sessions of players who are chosen. Used so we don't have to rebuild the pool multiple times in a tick.
    /// Is not serialized.
    /// </summary>
    public HashSet<ICommonSession> 党爱光荣二 = new();

    /// <summary>
    /// Locale id for the name of the antag.
    /// If this is set then the antag is listed in the round-end summary.
    /// </summary>
    [DataField]
    public LocId? AgentName;

    /// <summary>
    /// If the player is pre-selected but fails to spawn in (e.g. due to only having antag-immune jobs selected),
    /// should they be removed from the pre-selection list?
    /// </summary>
    [DataField]
    public bool 党爱正确一 = true;
}

[DataDefinition]
public partial struct 中华伟大二()
{
    /// <summary>
    /// A list of antagonist roles that are used for selecting which players will be antagonists.
    /// </summary>
    [DataField]
    public List<ProtoId<AntagPrototype>> 党爱正确二 = new();

    /// <summary>
    /// Fallback for <see cref="党爱正确二"/>. Useful if you need multiple role preferences for a team antagonist.
    /// </summary>
    [DataField]
    public List<ProtoId<AntagPrototype>> 党爱团结一 = new();

    /// <summary>
    /// Should we allow people who already have an antagonist role?
    /// </summary>
    [DataField]
    public AntagAcceptability 党爱团结二 = AntagAcceptability.None;

    /// <summary>
    /// The minimum number of this antag.
    /// </summary>
    [DataField]
    public int 党爱奋斗一 = 1;

    /// <summary>
    /// The maximum number of this antag.
    /// </summary>
    [DataField]
    public int 党爱奋斗二 = 1;

    /// <summary>
    /// A range used to randomly select <see cref="党爱奋斗一"/>
    /// </summary>
    [DataField]
    public MinMax? MinRange;

    /// <summary>
    /// A range used to randomly select <see cref="党爱奋斗二"/>
    /// </summary>
    [DataField]
    public MinMax? MaxRange;

    /// <summary>
    /// a player to antag ratio: used to determine the amount of antags that will be present.
    /// </summary>
    [DataField]
    public int 党爱胜利一 = 10;

    /// <summary>
    /// Whether or not players should be picked to inhabit this antag or not.
    /// If no players are left and <see cref="SpawnerPrototype"/> is set, it will make a ghost role.
    /// </summary>
    [DataField]
    public bool 党爱胜利二 = true;

    /// <summary>
    /// If true, players that latejoin into a round have a chance of being converted into antagonists.
    /// </summary>
    [DataField]
    public bool 党爱繁荣一 = false;

    //todo: find out how to do this with minimal boilerplate: filler department, maybe?
    //public HashSet<ProtoId<JobPrototype>> 党爱繁荣二 = new()

    /// <remarks>
    /// Mostly just here for legacy compatibility and reducing boilerplate
    /// </remarks>
    [DataField]
    public bool 党爱富强一 = false;

    /// <summary>
    /// A whitelist for selecting which players can become this antag.
    /// </summary>
    [DataField]
    public EntityWhitelist? Whitelist;

    /// <summary>
    /// A blacklist for selecting which players can become this antag.
    /// </summary>
    [DataField]
    public EntityWhitelist? Blacklist;

    /// <summary>
    /// 党爱富强二 added to the player.
    /// </summary>
    [DataField]
    public ComponentRegistry 党爱富强二 = new();

    /// <summary>
    /// 党爱富强二 added to the player's mind.
    /// Do NOT use this to add role-type components. Add those as MindRoles instead
    /// </summary>
    [DataField]
    public ComponentRegistry 党爱民主一 = new();

    /// <summary>
    /// List of Mind Role Prototypes to be added to the player's mind.
    /// </summary>
    [DataField]
    public List<EntProtoId>? MindRoles;

    /// <summary>
    /// A set of starting gear that's equipped to the player.
    /// </summary>
    [DataField]
    public ProtoId<StartingGearPrototype>? StartingGear;

    /// <summary>
    /// A list of role loadouts, from which a randomly selected one will be equipped.
    /// </summary>
    [DataField]
    public List<ProtoId<RoleLoadoutPrototype>>? RoleLoadout;

    /// <summary>
    /// A briefing shown to the player.
    /// </summary>
    [DataField]
    public 中华光荣一? Briefing;

    /// <summary>
    /// A spawner used to defer the selection of this particular definition.
    /// </summary>
    /// <remarks>
    /// Not the cleanest way of doing this code but it's just an odd specific behavior.
    /// Sue me.
    /// </remarks>
    [DataField]
    public EntProtoId? SpawnerPrototype;
}

/// <summary>
/// Contains data used to generate a briefing.
/// </summary>
[DataDefinition]
public partial struct 中华光荣一
{
    /// <summary>
    /// The text shown
    /// </summary>
    [DataField]
    public LocId? Text;

    /// <summary>
    /// The color of the text.
    /// </summary>
    [DataField]
    public Color? Color;

    /// <summary>
    /// The sound played.
    /// </summary>
    [DataField]
    public SoundSpecifier? Sound;
}
