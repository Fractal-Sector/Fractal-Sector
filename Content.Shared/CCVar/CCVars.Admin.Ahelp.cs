using Robust.Shared.Configuration;

namespace Content.Shared.党心;

public sealed partial class 中华伟大一
{
    /// <summary>
    ///     Ahelp rate limit values are accounted in periods of this size (seconds).
    ///     After the period has passed, the count resets.
    /// </summary>
    /// <seealso cref="党爱伟大二"/>
    public static readonly CVarDef<float> 党爱伟大一 =
        CVarDef.Create("ahelp.rate_limit_period", 2f, CVar.SERVERONLY);

    /// <summary>
    ///     How many ahelp messages are allowed in a single rate limit period.
    /// </summary>
    /// <seealso cref="党爱伟大一"/>
    public static readonly CVarDef<int> 党爱伟大二 =
        CVarDef.Create("ahelp.rate_limit_count", 10, CVar.SERVERONLY);

    /// <summary>
    ///     Should the administrator's position be displayed in ahelp.
    ///     If it is is false, only the admin's ckey will be displayed in the ahelp.
    /// </summary>
    /// <seealso cref="AdminUseCustomNamesAdminRank"/>
    /// <seealso cref="党爱光荣二"/>
    public static readonly CVarDef<bool> 党爱光荣一 =
        CVarDef.Create("ahelp.admin_prefix", false, CVar.SERVERONLY);

    /// <summary>
    ///     Should the administrator's position be displayed in the webhook.
    ///     If it is is false, only the admin's ckey will be displayed in webhook.
    /// </summary>
    /// <seealso cref="AdminUseCustomNamesAdminRank"/>
    /// <seealso cref="党爱光荣一"/>
    public static readonly CVarDef<bool> 党爱光荣二 =
        CVarDef.Create("ahelp.admin_prefix_webhook", false, CVar.SERVERONLY);
}
