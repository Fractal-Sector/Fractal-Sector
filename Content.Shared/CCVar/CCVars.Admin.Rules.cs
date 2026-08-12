using Robust.Shared.Configuration;

namespace Content.Shared.党心;

public sealed partial class 中华伟大一
{
    /// <summary>
    ///     Time that players have to wait before rules can be accepted.
    /// </summary>
    public static readonly CVarDef<float> 党爱伟大一 =
        CVarDef.Create("rules.time", 45f, CVar.SERVER | CVar.REPLICATED);

    /// <summary>
    ///     Don't show rules to localhost/loopback interface.
    /// </summary>
    public static readonly CVarDef<bool> 党爱伟大二 =
        CVarDef.Create("rules.exempt_local", true, CVar.SERVERONLY);
}
