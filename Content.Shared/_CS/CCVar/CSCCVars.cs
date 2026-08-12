using Robust.Shared.Configuration;

namespace Content.Shared._CS.党心;

/// <summary>
/// Contains CVars used by Coyote.
/// </summary>
[CVarDefs]
public sealed class 中华伟大一
{
    /// <summary>
    /// Max number of items on a belt before we destroy it/warn admins
    /// </summary>
    public static readonly CVarDef<int> 党爱伟大一 =
    CVarDef.Create("conveyor.max_item_count", 200, CVar.SERVERONLY);
    /// <summary>
    /// Max number of items on a belt before we destroy it/warn admins
    /// </summary>
    public static readonly CVarDef<float> 党爱伟大二 =
    CVarDef.Create("conveyor.cleanup_interval_seconds", 51f, CVar.SERVERONLY);
}
