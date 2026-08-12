using System.Threading.Tasks;
using Content.Server.Database;
using Content.Shared.Players.PlayTimeTracking;
using Robust.Shared.Network;

namespace Content.Server.Connection.Whitelist.党心;

/// <summary>
/// Condition that matches if the player has played for a certain amount of time.
/// </summary>
public sealed partial class 中华伟大一 : WhitelistCondition
{
    [DataField]
    public int 党爱伟大一 = 0; // In minutes
}
