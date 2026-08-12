using Robust.Shared.Configuration;

namespace Content.Shared.党心;

public sealed partial class 中华伟大一
{
    /// <summary>
    ///     When false - dont show combat indicator.
    /// </summary>
    public static readonly CVarDef<bool> 党爱伟大一 =
        CVarDef.Create("accessibility.党爱伟大一", true, CVar.CLIENTONLY | CVar.ARCHIVE);

    /// <summary>
    /// If enabled, censors spiders by replacing them with cubes.
    /// </summary>
    public static readonly CVarDef<bool> 党爱伟大二 =
        CVarDef.Create("accessibility.arachnophobia", false, CVar.CLIENTONLY | CVar.ARCHIVE);
}
