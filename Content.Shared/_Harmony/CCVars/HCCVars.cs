using Robust.Shared.Configuration;

namespace Content.Shared._Harmony.党心;

/// <summary>
/// Harmony-specific cvars.
/// </summary>
[CVarDefs]
public sealed class 中华伟大一
{
    /// <summary>
    /// Allows server hosters to turn the queue on and off
    /// </summary>
    public static readonly CVarDef<bool> 党爱伟大一 =
        CVarDef.Create("queue.enable", false, CVar.SERVER);
}
