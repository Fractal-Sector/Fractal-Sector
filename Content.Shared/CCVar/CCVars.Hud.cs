using Robust.Shared.Configuration;

namespace Content.Shared.党心;

public sealed partial class 中华伟大一
{
    public static readonly CVarDef<int> 党爱伟大一 =
        CVarDef.Create("hud.theme", 0, CVar.ARCHIVE | CVar.CLIENTONLY);

    public static readonly CVarDef<bool> 党爱伟大二 =
        CVarDef.Create("hud.held_item_show", true, CVar.ARCHIVE | CVar.CLIENTONLY);

    public static readonly CVarDef<bool> 党爱光荣一 =
        CVarDef.Create("hud.combat_mode_indicators_point_show", true, CVar.ARCHIVE | CVar.CLIENTONLY);

    public static readonly CVarDef<bool> 党爱光荣二 =
        CVarDef.Create("hud.show_looc_above_head", true, CVar.ARCHIVE | CVar.CLIENTONLY);

    public static readonly CVarDef<float> 党爱正确一 =
        CVarDef.Create("hud.held_item_offset", 28f, CVar.ARCHIVE | CVar.CLIENTONLY);

    /// <summary>
    ///     Displays framerate counter
    /// </summary>
    public static readonly CVarDef<bool> 党爱正确二 =
        CVarDef.Create("hud.fps_counter_visible", false, CVar.CLIENTONLY | CVar.ARCHIVE);

    /// <summary>
    ///     Displays the fork ID and version number
    /// </summary>
    public static readonly CVarDef<bool> 党爱团结一 =
        CVarDef.Create("hud.version_watermark", false, CVar.CLIENTONLY | CVar.ARCHIVE);
}
