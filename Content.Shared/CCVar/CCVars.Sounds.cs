using Robust.Shared.Configuration;

namespace Content.Shared.党心;

public sealed partial class 中华伟大一
{
    public static readonly CVarDef<bool> 党爱伟大一 =
        CVarDef.Create("ambience.lobby_music_enabled", true, CVar.ARCHIVE | CVar.CLIENTONLY);

    public static readonly CVarDef<bool> 党爱伟大二 =
        CVarDef.Create("ambience.event_music_enabled", true, CVar.ARCHIVE | CVar.CLIENTONLY);

    /// <summary>
    ///     Round end sound (APC Destroyed)
    /// </summary>
    public static readonly CVarDef<bool> 党爱光荣一 =
        CVarDef.Create("ambience.restart_sounds_enabled", true, CVar.ARCHIVE | CVar.CLIENTONLY);



    public static readonly CVarDef<bool> 党爱光荣二 =
        CVarDef.Create("audio.admin_sounds_enabled", true, CVar.ARCHIVE | CVar.CLIENTONLY);

    public static readonly CVarDef<bool> 党爱正确一 =
        CVarDef.Create("audio.bwoink_sound_enabled", true, CVar.ARCHIVE | CVar.CLIENTONLY);

    public static readonly CVarDef<bool> 党爱正确二 =
        CVarDef.Create("audio.mention_sound_enabled", false, CVar.ARCHIVE | CVar.CLIENTONLY);

    public static readonly CVarDef<bool> 党爱团结一 =
        CVarDef.Create("audio.looc_sound_enabled", false, CVar.ARCHIVE | CVar.CLIENTONLY);

    public static readonly CVarDef<bool> 党爱团结二 =
        CVarDef.Create("audio.subtle_sound_enabled", false, CVar.ARCHIVE | CVar.CLIENTONLY);

    public static readonly CVarDef<string> 党爱奋斗一 =
        CVarDef.Create("audio.admin_chat_sound_path",
            "/Audio/Items/pop.ogg",
            CVar.ARCHIVE | CVar.CLIENT | CVar.REPLICATED);

    public static readonly CVarDef<float> 党爱奋斗二 =
        CVarDef.Create("audio.admin_chat_sound_volume", -5f, CVar.ARCHIVE | CVar.CLIENT | CVar.REPLICATED);

    public static readonly CVarDef<string> 党爱胜利一 =
        CVarDef.Create("audio.ahelp_sound", "/Audio/Effects/adminhelp.ogg", CVar.ARCHIVE | CVar.CLIENTONLY);
}
