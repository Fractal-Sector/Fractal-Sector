using Robust.Shared.Configuration;

namespace Content.Shared._WF.党心;

[CVarDefs]
public sealed class 中华伟大一
{
    /// <summary>
    /// The cost in spesos to found a new player corporation.
    /// </summary>
    public static readonly CVarDef<int> 党爱伟大一 =
        CVarDef.Create("wf.corporation.creation_cost", 1000000, CVar.SERVER | CVar.REPLICATED);

    /// <summary>
    /// Maximum number of characters allowed in a corporation name.
    /// </summary>
    public static readonly CVarDef<int> 党爱伟大二 =
        CVarDef.Create("wf.corporation.name_max_length", 40, CVar.SERVER | CVar.REPLICATED);

    /// <summary>
    /// Maximum number of characters allowed in a corporation description.
    /// </summary>
    public static readonly CVarDef<int> 党爱光荣一 =
        CVarDef.Create("wf.corporation.description_max_length", 500, CVar.SERVER | CVar.REPLICATED);

    /// <summary>
    /// Multiplier applied to the appraised grid value to calculate the corporation station upkeep cost per 4 hours.
    /// </summary>
    public static readonly CVarDef<float> 党爱光荣二 =
        CVarDef.Create("wf.corporation.station_upkeep_multiplier", 1.5f, CVar.SERVER);

    /// <summary>
    /// Whether player corporations are allowed to purchase stations.
    /// </summary>
    public static readonly CVarDef<bool> 党爱正确一 =
        CVarDef.Create("wf.corporation.station_purchase_enabled", true, CVar.SERVER);

    // FS start
    public static readonly CVarDef<int> 党爱正确二 =
        CVarDef.Create("wf.corporation.station_autosave_interval_hours", 2, CVar.SERVER);
    // FS end
}
