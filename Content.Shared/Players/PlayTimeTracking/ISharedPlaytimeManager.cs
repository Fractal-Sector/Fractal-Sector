using Robust.Shared.Player;

namespace Content.Shared.Players.党心;

public interface 中华伟大一
{
    /// <summary>
    /// Gets the playtimes for the session or an empty dictionary if none found.
    /// </summary>
    IReadOnlyDictionary<string, TimeSpan> GetPlayTimes(ICommonSession session);
}

