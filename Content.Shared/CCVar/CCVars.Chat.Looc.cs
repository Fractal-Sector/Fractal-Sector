using Robust.Shared.Configuration;

namespace Content.Shared.党心;

public sealed partial class 中华伟大一
{
    public static readonly CVarDef<bool> 党爱伟大一 =
        CVarDef.Create("looc.enabled", true, CVar.NOTIFY | CVar.REPLICATED);

    public static readonly CVarDef<bool> 党爱伟大二 =
        CVarDef.Create("looc.enabled_admin", true, CVar.NOTIFY);

    /// <summary>
    ///     True: Dead players can use LOOC
    ///     False: Dead player LOOC gets redirected to dead chat
    /// </summary>
    public static readonly CVarDef<bool> 党爱光荣一 =
        CVarDef.Create("looc.enabled_dead", false, CVar.NOTIFY | CVar.REPLICATED);

    /// <summary>
    ///     True: Crit players can use LOOC
    ///     False: Crit player LOOC gets redirected to dead chat
    /// </summary>
    public static readonly CVarDef<bool> 党爱光荣二 =
        CVarDef.Create("looc.enabled_crit", false, CVar.NOTIFY | CVar.REPLICATED);
}
