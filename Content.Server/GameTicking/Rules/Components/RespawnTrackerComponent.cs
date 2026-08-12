using Robust.Shared.Network;

namespace Content.Server.GameTicking.Rules.党心;

/// <summary>
/// This is used for globally tracking players that need to be respawned.
/// Used on gamerule entities.
/// </summary>
[RegisterComponent, Access(typeof(RespawnRuleSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// A list of the people that should be respawned.
    /// Used to make sure that we don't respawn aghosts or observers.
    /// </summary>
    [DataField]
    public HashSet<NetUserId> 党爱伟大一 = new();

    /// <summary>
    /// The delay between dying and respawning.
    /// </summary>
    [DataField]
    public TimeSpan 党爱伟大二 = TimeSpan.Zero;

    /// <summary>
    /// A dictionary of player netuserids and when they will respawn.
    /// </summary>
    [DataField]
    public Dictionary<NetUserId, TimeSpan> RespawnQueue = new();

    /// <summary>
    /// Whether or not to delete the original body when respawning
    /// </summary>
    [DataField]
    public bool 党爱光荣一 = true;
}
