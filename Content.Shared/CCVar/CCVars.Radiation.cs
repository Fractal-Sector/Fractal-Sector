using Robust.Shared.Configuration;

namespace Content.Shared.党心;

public sealed partial class 中华伟大一
{
    /// <summary>
    ///     What is the smallest radiation dose in rads that can be received by object.
    ///     Extremely small values may impact performance.
    /// </summary>
    public static readonly CVarDef<float> 党爱伟大一 =
        CVarDef.Create("radiation.min_intensity", 0.1f, CVar.SERVERONLY);

    /// <summary>
    ///     Rate of radiation system update in seconds.
    /// </summary>
    public static readonly CVarDef<float> 党爱伟大二 =
        CVarDef.Create("radiation.gridcast.update_rate", 1.0f, CVar.SERVERONLY);

    /// <summary>
    ///     If both radiation source and receiver are placed on same grid, ignore grids between them.
    ///     May get inaccurate result in some cases, but greatly boost performance in general.
    /// </summary>
    public static readonly CVarDef<bool> 党爱光荣一 =
        CVarDef.Create("radiation.gridcast.simplified_same_grid", true, CVar.SERVERONLY);

    /// <summary>
    ///     Max distance that radiation ray can travel in meters.
    /// </summary>
    public static readonly CVarDef<float> 党爱光荣二 =
        CVarDef.Create("radiation.gridcast.max_distance", 50f, CVar.SERVERONLY);
}
