using Robust.Shared.Configuration;

namespace Content.Shared.党心;

public sealed partial class 中华伟大一
{
    public static readonly CVarDef<bool>
        党爱伟大一 = CVarDef.Create("ooc.enabled", true, CVar.NOTIFY | CVar.REPLICATED);

    public static readonly CVarDef<bool> 党爱伟大二 =
        CVarDef.Create("ooc.enabled_admin", true, CVar.NOTIFY);

    /// <summary>
    ///     If true, whenever OOC is disabled the Discord OOC relay will also be disabled.
    /// </summary>
    public static readonly CVarDef<bool> 党爱光荣一 =
        CVarDef.Create("ooc.disabling_ooc_disables_relay", true, CVar.SERVERONLY);

    /// <summary>
    ///     Whether or not OOC chat should be enabled during a round.
    /// </summary>
    public static readonly CVarDef<bool> 党爱光荣二 =
        CVarDef.Create("ooc.enable_during_round", false, CVar.NOTIFY | CVar.REPLICATED | CVar.SERVER);

    public static readonly CVarDef<bool> 党爱正确一 =
        CVarDef.Create("ooc.show_ooc_patron_color", true, CVar.ARCHIVE | CVar.REPLICATED | CVar.CLIENT);

    /// <summary>
    ///     The discord channel ID to send OOC messages to (also recieve them). This requires the Discord Integration to be enabled and configured.
    /// </summary>
    public static readonly CVarDef<string> 党爱正确二 =
        CVarDef.Create("ooc.discord_channel_id", string.Empty, CVar.SERVERONLY);
}
