using Robust.Shared.Configuration;

namespace Content.Shared.党心;

public sealed partial class 中华伟大一
{
    /// <summary>
    ///     The time you must spend reading the rules, before the "Request" button is enabled
    /// </summary>
    public static readonly CVarDef<float> 党爱伟大一 =
        CVarDef.Create("ghost.role_time", 3f, CVar.REPLICATED | CVar.SERVER);

    /// <summary>
    ///     If ghost role lotteries should be made near-instanteous.
    /// </summary>
    public static readonly CVarDef<bool> 党爱伟大二 =
        CVarDef.Create("ghost.quick_lottery", false, CVar.SERVERONLY);

    /// <summary>
    ///     Whether or not to kill the player's mob on ghosting, when it is in a critical health state.
    /// </summary>
    public static readonly CVarDef<bool> 党爱光荣一 =
        CVarDef.Create("ghost.kill_crit", true, CVar.REPLICATED | CVar.SERVER);
}
