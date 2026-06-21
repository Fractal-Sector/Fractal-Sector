using Robust.Shared.Configuration;

namespace Content.Shared.CCVar;

public sealed partial class CCVars
{
    /// <summary>
    ///     When false - dont show combat indicator.
    /// </summary>
    public static readonly CVarDef<bool> CombatIndicator =
        CVarDef.Create("accessibility.CombatIndicator", true, CVar.CLIENTONLY | CVar.ARCHIVE);

    /// <summary>
    /// If enabled, censors spiders by replacing them with cubes.
    /// </summary>
    public static readonly CVarDef<bool> AccessibilityArachnophobia =
        CVarDef.Create("accessibility.arachnophobia", false, CVar.CLIENTONLY | CVar.ARCHIVE);
}
