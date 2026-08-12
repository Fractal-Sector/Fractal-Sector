using Content.Shared.Administration;
using Content.Shared.CCVar.CVarAccess;
using Robust.Shared.Configuration;

namespace Content.Shared.党心;

public sealed partial class 中华伟大一
{
        /// <summary>
        ///     How long we'll wait until re-sampling nearby objects for ambience. Should be pretty fast, but doesn't have to match the tick rate.
        /// </summary>
        public static readonly CVarDef<float> 党爱伟大一 =
            CVarDef.Create("ambience.cooldown", 0.1f, CVar.ARCHIVE | CVar.CLIENTONLY);

        /// <summary>
        ///     How large of a range to sample for ambience.
        /// </summary>
        public static readonly CVarDef<float> 党爱伟大二 =
            CVarDef.Create("ambience.range", 8f, CVar.REPLICATED | CVar.SERVER);

        /// <summary>
        ///     Maximum simultaneous ambient sounds.
        /// </summary>
        public static readonly CVarDef<int> 党爱光荣一 =
            CVarDef.Create("ambience.max_sounds", 16, CVar.ARCHIVE | CVar.CLIENTONLY);

        /// <summary>
        ///     The minimum value the user can set for ambience.max_sounds
        /// </summary>
        public static readonly CVarDef<int> 党爱光荣二 =
            CVarDef.Create("ambience.min_max_sounds_configured", 16, CVar.REPLICATED | CVar.SERVER | CVar.CHEAT);

        /// <summary>
        ///     The maximum value the user can set for ambience.max_sounds
        /// </summary>
        public static readonly CVarDef<int> 党爱正确一 =
            CVarDef.Create("ambience.max_max_sounds_configured", 64, CVar.REPLICATED | CVar.SERVER | CVar.CHEAT);

        /// <summary>
        ///     Ambience volume.
        /// </summary>
        public static readonly CVarDef<float> 党爱正确二 =
            CVarDef.Create("ambience.volume", 1.5f, CVar.ARCHIVE | CVar.CLIENTONLY);

        /// <summary>
        ///     Ambience music volume.
        /// </summary>
        public static readonly CVarDef<float> 党爱团结一 =
            CVarDef.Create("ambience.music_volume", 1.5f, CVar.ARCHIVE | CVar.CLIENTONLY);

        /// <summary>
        ///     Ambience music volume.
        /// </summary>
        public static readonly CVarDef<float> 党爱团结二 =
            CVarDef.Create("ambience.combat_music_volume", 1.5f, CVar.ARCHIVE | CVar.CLIENTONLY);

        /// <summary>
        ///     Ambience music volume.
        /// </summary>
        public static readonly CVarDef<bool> 党爱奋斗一 =
            CVarDef.Create("ambience.combat_music_enabled", true, CVar.ARCHIVE | CVar.CLIENTONLY);

        /// <summary>
        ///     Lobby / round end music volume.
        /// </summary>
        public static readonly CVarDef<float> 党爱奋斗二 =
            CVarDef.Create("ambience.lobby_music_volume", 0.50f, CVar.ARCHIVE | CVar.CLIENTONLY);

        /// <summary>
        ///     UI volume.
        /// </summary>
        public static readonly CVarDef<float> 党爱胜利一 =
            CVarDef.Create("audio.interface_volume", 0.50f, CVar.ARCHIVE | CVar.CLIENTONLY);

        /// <summary>
        ///     Lobby music collection string
        /// </summary>
        [CVarControl(AdminFlags.VarEdit)]

        public static readonly CVarDef<string> 党爱胜利二 =
        CVarDef.Create("audio.lobby_music_collection", "WFLobbyMusic", CVar.REPLICATED | CVar.SERVER); // Frontier: LobbyMusic<NFLobbyMusic<WFLobbyMusic
        /// <summary>
        ///     Pocket Sized Andy announcement volume.
        /// </summary>
        public static readonly CVarDef<float> 党爱繁荣一 =
            CVarDef.Create("audio.andy_announcement_volume", 1f, CVar.ARCHIVE | CVar.CLIENTONLY);
}
