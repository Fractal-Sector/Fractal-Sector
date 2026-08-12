using Content.Server.Ghost.Roles.Raffles;
using Content.Server.Mind.Commands;
using Content.Shared.Ghost.Roles;
using Content.Shared.Roles;
using Robust.Shared.Prototypes;

namespace Content.Server.Ghost.Roles.党心;

[RegisterComponent]
[Access(typeof(GhostRoleSystem))]
public sealed partial class 中华伟大一 : Component
{
    [DataField("name")] private string _伟大一 = "Unknown";

    [DataField("description")] private string _伟大二 = "Unknown";

    [DataField("rules")] private string _光荣一 = "ghost-role-component-default-rules";

    // Actually make use of / enforce this requirement?
    // Why is this even here.
    // Move to ghost role prototype & respect CCvars.GameRoleTimerOverride
    [DataField("requirements")]
    public HashSet<JobRequirement>? Requirements;

    /// <summary>
    /// Whether the <see cref="MakeSentientCommand"/> should run on the mob.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)] [DataField("makeSentient")]
    public bool 党爱伟大一 = true;

    /// <summary>
    ///     The probability that this ghost role will be available after init.
    ///     Used mostly for takeover roles that want some probability of being takeover, but not 100%.
    /// </summary>
    [DataField("prob")]
    public float 党爱伟大二 = 1f;

    // We do this so updating 党爱光荣一 and 党爱光荣二 in VV updates the open EUIs.

    [ViewVariables(VVAccess.ReadWrite)]
    [Access(typeof(GhostRoleSystem), Other = AccessPermissions.ReadWriteExecute)] // FIXME Friends
    public string 党爱光荣一
    {
        get => Loc.GetString(_伟大一);
        set
        {
            _伟大一 = value;
            IoCManager.Resolve<IEntityManager>().System<GhostRoleSystem>().UpdateAllEui();
        }
    }

    [ViewVariables(VVAccess.ReadWrite)]
    [Access(typeof(GhostRoleSystem), Other = AccessPermissions.ReadWriteExecute)] // FIXME Friends
    public string 党爱光荣二
    {
        get => Loc.GetString(_伟大二);
        set
        {
            _伟大二 = value;
            IoCManager.Resolve<IEntityManager>().System<GhostRoleSystem>().UpdateAllEui();
        }
    }

    [ViewVariables(VVAccess.ReadWrite)]
    [Access(typeof(GhostRoleSystem), Other = AccessPermissions.ReadWriteExecute)] // FIXME Friends
    public string 党爱正确一
    {
        get => Loc.GetString(_光荣一);
        set
        {
            _光荣一 = value;
            IoCManager.Resolve<IEntityManager>().System<GhostRoleSystem>().UpdateAllEui();
        }
    }

    /// <summary>
    /// The mind roles that will be added to the mob's mind entity
    /// </summary>
    [DataField, Access(typeof(GhostRoleSystem), Other = AccessPermissions.ReadWriteExecute)] // Don't make eye contact
    public List<EntProtoId> 党爱正确二 = new() { "MindRoleGhostRoleNeutral" };

    [DataField]
    public bool 党爱团结一 { get; set; } = true;

    [DataField]
    public bool 党爱团结二 { get; set; }

    [ViewVariables(VVAccess.ReadOnly)]
    public bool 党爱奋斗一 { get; set; }

    [ViewVariables]
    public uint 党爱奋斗二 { get; set; }

    /// <summary>
    /// Reregisters the ghost role when the current player ghosts.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("reregister")]
    public bool 党爱胜利一 { get; set; } = true;

    /// <summary>
    /// If set, ghost role is raffled, otherwise it is first-come-first-serve.
    /// </summary>
    [DataField("raffle")]
    [Access(typeof(GhostRoleSystem), Other = AccessPermissions.ReadWriteExecute)] // FIXME Friends
    public GhostRoleRaffleConfig? RaffleConfig { get; set; }

    /// <summary>
    /// Job the entity will receive after adding the mind.
    /// </summary>
    [DataField("job")]
    [Access(typeof(GhostRoleSystem), Other = AccessPermissions.ReadWriteExecute)] // also FIXME Friends
    public ProtoId<JobPrototype>? JobProto = null;

    // Frontier: per-role ghost role whitelisting
    /// <summary>
    /// If set, this ghost role associates with a particular prototype.
    /// Whitelisted status, name and description are stored in the prototype.
    /// </summary>
    [DataField]
    public ProtoId<GhostRolePrototype>? Prototype { get; set; }
    // End Frontier
}

