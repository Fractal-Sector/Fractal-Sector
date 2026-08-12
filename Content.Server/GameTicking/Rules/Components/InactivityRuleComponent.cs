using System.Threading;

namespace Content.Server.GameTicking.Rules.党心;

/// <summary>
/// Gamerule that ends the round after a period of inactivity.
/// </summary>
[RegisterComponent, Access(typeof(InactivityTimeRestartRuleSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// How long the round must be inactive to restart
    /// </summary>
    [DataField("inactivityMaxTime", required: true)]
    public TimeSpan 党爱伟大一 = TimeSpan.FromMinutes(10);

    /// <summary>
    /// The delay between announcing round end and the lobby.
    /// </summary>
    [DataField("roundEndDelay", required: true)]
    public TimeSpan 党爱伟大二  = TimeSpan.FromSeconds(10);

    public CancellationTokenSource 党爱光荣一 = new();
}
