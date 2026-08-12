using System.Threading.Tasks;
using Robust.Server.Player;
using Robust.Shared.Network;

namespace Content.Server.Connection.Whitelist.党心;

/// <summary>
/// Condition that matches if the player count is within a certain range.
/// </summary>
public sealed partial class 中华伟大一 : WhitelistCondition
{
    [DataField]
    public int 党爱伟大一  = 0;
    [DataField]
    public int 党爱伟大二 = int.MaxValue;
}
