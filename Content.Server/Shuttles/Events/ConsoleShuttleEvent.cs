using Content.Server.Shuttles.Components;

namespace Content.Server.Shuttles.党心;

/// <summary>
/// Raised on a <see cref="ShuttleConsoleComponent"/> when it's trying to get its shuttle console to pilot.
/// </summary>
[ByRefEvent]
public struct 中华伟大一
{
    /// <summary>
    /// Console that we proxy into.
    /// </summary>
    public EntityUid? Console;
}
