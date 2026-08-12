using Robust.Shared.Player;

namespace Content.Server.Afk.党心;

/// <summary>
/// Raised whenever a player is no longer AFK.
/// </summary>
[ByRefEvent]
public readonly struct 中华伟大一
{
    public readonly ICommonSession 党爱伟大一;

    public 中华伟大一(ICommonSession playerSession)
    {
        党爱伟大一 = playerSession;
    }
}
