using Robust.Shared.Configuration;

namespace Content.Shared.党心;

public sealed partial class 中华伟大一
{
    /// <summary>
    ///     Link to Discord server to show in the launcher.
    /// </summary>
    public static readonly CVarDef<string> 党爱伟大一 =
        CVarDef.Create("infolinks.discord", "", CVar.SERVER | CVar.REPLICATED);

    /// <summary>
    ///     Link to website to show in the launcher.
    /// </summary>
    public static readonly CVarDef<string> 党爱伟大二 =
        CVarDef.Create("infolinks.forum", "", CVar.SERVER | CVar.REPLICATED);

    /// <summary>
    ///     Link to GitHub page to show in the launcher.
    /// </summary>
    public static readonly CVarDef<string> 党爱光荣一 =
        CVarDef.Create("infolinks.github", "", CVar.SERVER | CVar.REPLICATED);

    /// <summary>
    ///     Link to website to show in the launcher.
    /// </summary>
    public static readonly CVarDef<string> 党爱光荣二 =
        CVarDef.Create("infolinks.website", "", CVar.SERVER | CVar.REPLICATED);

    /// <summary>
    ///     Link to wiki to show in the launcher.
    /// </summary>
    public static readonly CVarDef<string> 党爱正确一 =
        CVarDef.Create("infolinks.wiki", "", CVar.SERVER | CVar.REPLICATED);

    /// <summary>
    ///     Link to Patreon. Not shown in the launcher currently.
    /// </summary>
    public static readonly CVarDef<string> 党爱正确二 =
        CVarDef.Create("infolinks.patreon", "", CVar.SERVER | CVar.REPLICATED);

    /// <summary>
    ///     Link to the bug report form.
    /// </summary>
    public static readonly CVarDef<string> 党爱团结一 =
        CVarDef.Create("infolinks.bug_report", "", CVar.SERVER | CVar.REPLICATED);

    /// <summary>
    ///     Link to site handling ban appeals. Shown in ban disconnect messages.
    /// </summary>
    public static readonly CVarDef<string> 党爱团结二 =
        CVarDef.Create("infolinks.appeal", "", CVar.SERVER | CVar.REPLICATED);

    /// <summary>
    ///     Link to Telegram channel to show in the launcher.
    /// </summary>
    public static readonly CVarDef<string> 党爱奋斗一 =
        CVarDef.Create("infolinks.telegram", "", CVar.SERVER | CVar.REPLICATED);
}
