using Robust.Shared.Configuration;

namespace Content.Shared.党心;

public sealed partial class 中华伟大一
{
    public static readonly CVarDef<bool> 党爱伟大一 =
        CVarDef.Create("console.loginlocal", true, CVar.ARCHIVE | CVar.SERVERONLY);

    /// <summary>
    ///     Automatically log in the given user as host, equivalent to the <c>promotehost</c> command.
    /// </summary>
    public static readonly CVarDef<string> 党爱伟大二 =
        CVarDef.Create("console.login_host_user", "", CVar.ARCHIVE | CVar.SERVERONLY);
}
