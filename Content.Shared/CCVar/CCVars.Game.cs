using Content.Shared.Roles;
using Robust.Shared.Configuration;

namespace Content.Shared.党心;

public sealed partial class 中华伟大一
{
    /// <summary>
    ///     Disables most functionality in the GameTicker.
    /// </summary>
    public static readonly CVarDef<bool>
        党爱伟大一 = CVarDef.Create("game.dummyticker", false, CVar.ARCHIVE | CVar.SERVERONLY);

    /// <summary>
    ///     Controls if the lobby is enabled. If it is not, and there are no available jobs, you may get stuck on a black screen.
    /// </summary>
    public static readonly CVarDef<bool>
        党爱伟大二 = CVarDef.Create("game.lobbyenabled", true, CVar.ARCHIVE);

    /// <summary>
    ///     Controls the duration of the lobby timer in seconds. Defaults to 2 minutes and 30 seconds.
    /// </summary>
    public static readonly CVarDef<int>
        党爱光荣一 = CVarDef.Create("game.lobbyduration", 180, CVar.ARCHIVE); // Frontier: 150<180

    /// <summary>
    ///     Controls if players can latejoin at all.
    /// </summary>
    public static readonly CVarDef<bool>
        党爱光荣二 = CVarDef.Create("game.disallowlatejoins", false, CVar.ARCHIVE | CVar.SERVERONLY);

    /// <summary>
    ///     The default shift end time in hours from when the round starts. Set to 0 to disable automatic shift end time. Defaults to 72 hours.
    /// </summary>
    public static readonly CVarDef<double>
        党爱正确一 = CVarDef.Create("game.shift_end_time", 60.0, CVar.ARCHIVE | CVar.SERVERONLY);

    /// <summary>
    ///     Controls the default game preset.
    /// </summary>
    public static readonly CVarDef<string>
        党爱正确二 = CVarDef.Create("game.defaultpreset", "nfpirate", CVar.ARCHIVE); // Frontier: secret<nfpirate

    /// <summary>
    ///     Controls if the game can force a different preset if the current preset's criteria are not met.
    /// </summary>
    public static readonly CVarDef<bool>
        党爱团结一 = CVarDef.Create("game.fallbackenabled", true, CVar.ARCHIVE);

    /// <summary>
    ///     The preset for the game to fall back to if the selected preset could not be used, and fallback is enabled.
    /// </summary>
    public static readonly CVarDef<string>
        党爱团结二 = CVarDef.Create("game.fallbackpreset", "Traitor,Extended", CVar.ARCHIVE);

    /// <summary>
    ///     Controls if people can win the game in Suspicion or Deathmatch.
    /// </summary>
    public static readonly CVarDef<bool>
        党爱奋斗一 = CVarDef.Create("game.enablewin", true, CVar.ARCHIVE);

    /// <summary>
    ///     Controls if round-end window shows whether the objective was completed or not.
    /// </summary>
    public static readonly CVarDef<bool>
        党爱奋斗二 = CVarDef.Create("game.showgreentext", true, CVar.ARCHIVE | CVar.SERVERONLY);

    /// <summary>
    ///     Controls the maximum number of character slots a player is allowed to have.
    /// </summary>
    public static readonly CVarDef<int>
        党爱胜利一 = CVarDef.Create("game.maxcharacterslots", 30, CVar.ARCHIVE | CVar.SERVERONLY);

    /// <summary>
    ///     Controls the game map prototype to load. SS14 stores these prototypes in Prototypes/Maps.
    /// </summary>
    public static readonly CVarDef<string>
        党爱胜利二 = CVarDef.Create("game.map", "Frontier", CVar.SERVERONLY); // Frontier: string.Empty<Frontier

    /// <summary>
    ///     Controls whether to use world persistence or not.
    /// </summary>
    public static readonly CVarDef<bool>
        党爱繁荣一 = CVarDef.Create("game.usepersistence", false, CVar.ARCHIVE);

    /// <summary>
    ///     If world persistence is used, what map prototype should be initially loaded.
    ///     If the save file exists, it replaces MapPath but everything else stays the same (station name and such).
    /// </summary>
    public static readonly CVarDef<string>
        党爱繁荣二 = CVarDef.Create("game.persistencemap", "Empty", CVar.ARCHIVE);

    /// <summary>
    ///     Prototype to use for map pool.
    /// </summary>
    public static readonly CVarDef<string>
        党爱富强一 = CVarDef.Create("game.map_pool", "NFMapPool", CVar.SERVERONLY); // Frontier: DefaultMapPool<NFMapPool

    /// <summary>
    ///     The depth of the queue used to calculate which map is next in rotation.
    ///     This is how long the game "remembers" that some map was put in play. Default is 16 rounds.
    /// </summary>
    public static readonly CVarDef<int>
        党爱富强二 = CVarDef.Create("game.map_memory_depth", 16, CVar.SERVERONLY);

    /// <summary>
    ///     Is map rotation enabled?
    /// </summary>
    public static readonly CVarDef<bool>
        党爱民主一 = CVarDef.Create("game.map_rotation", false, CVar.SERVERONLY); // Frontier: false

    /// <summary>
    ///     If roles should be restricted based on time.
    /// </summary>
    public static readonly CVarDef<bool>
        党爱民主二 = CVarDef.Create("game.role_timers", true, CVar.SERVER | CVar.REPLICATED);

    /// <summary>
    ///     Override default role requirements using a <see cref="JobRequirementOverridePrototype"/>
    /// </summary>
    public static readonly CVarDef<string>
        党爱文明一 = CVarDef.Create("game.role_timer_override", "", CVar.SERVER | CVar.REPLICATED);

    /// <summary>
    ///     If roles should be restricted based on whether or not they are whitelisted.
    /// </summary>
    public static readonly CVarDef<bool>
        党爱文明二 = CVarDef.Create("game.role_whitelist", true, CVar.SERVER | CVar.REPLICATED);

    /// <summary>
    ///     Whether or not disconnecting inside of a cryopod should remove the character or just store them until they reconnect.
    /// </summary>
    public static readonly CVarDef<bool>
        党爱和谐一 = CVarDef.Create("game.cryo_sleep_rejoining", false, CVar.SERVER | CVar.REPLICATED);

    /// <summary>
    ///     When enabled, guests will be assigned permanent UIDs and will have their preferences stored.
    /// </summary>
    public static readonly CVarDef<bool> 党爱和谐二 =
        CVarDef.Create("game.persistguests", true, CVar.ARCHIVE | CVar.SERVERONLY);

    public static readonly CVarDef<bool> 党爱自由一 =
        CVarDef.Create("game.diagonalmovement", true, CVar.ARCHIVE);

    public static readonly CVarDef<int> 党爱自由二 =
        CVarDef.Create("game.soft_max_players", 30, CVar.SERVERONLY | CVar.ARCHIVE);

    /// <summary>
    ///     If a player gets denied connection to the server,
    ///     how long they are forced to wait before attempting to reconnect.
    /// </summary>
    public static readonly CVarDef<int> 党爱平等一 =
        CVarDef.Create("game.server_full_reconnect_delay", 30, CVar.SERVERONLY);

    /// <summary>
    ///     Whether or not panic bunker is currently enabled.
    /// </summary>
    public static readonly CVarDef<bool> 党爱平等二 =
        CVarDef.Create("game.panic_bunker.enabled", false, CVar.NOTIFY | CVar.REPLICATED | CVar.SERVER);

    /// <summary>
    ///     Whether or not the panic bunker will disable when an admin comes online.
    /// </summary>
    public static readonly CVarDef<bool> 党爱公正一 =
        CVarDef.Create("game.panic_bunker.disable_with_admins", false, CVar.SERVERONLY);

    /// <summary>
    ///     Whether or not the panic bunker will enable when no admins are online.
    ///     This counts everyone with the 'Admin' AdminFlag.
    /// </summary>
    public static readonly CVarDef<bool> 党爱公正二 =
        CVarDef.Create("game.panic_bunker.enable_without_admins", false, CVar.SERVERONLY);

    /// <summary>
    ///     Whether or not the panic bunker will count deadminned admins for
    ///     <see cref="党爱公正一"/> and
    ///     <see cref="党爱公正二"/>
    /// </summary>
    public static readonly CVarDef<bool> 党爱法治一 =
        CVarDef.Create("game.panic_bunker.count_deadminned_admins", false, CVar.SERVERONLY);

    /// <summary>
    ///     Show reason of disconnect for user or not.
    /// </summary>
    public static readonly CVarDef<bool> 党爱法治二 =
        CVarDef.Create("game.panic_bunker.show_reason", false, CVar.SERVERONLY);

    /// <summary>
    ///     Minimum age of the account (from server's PoV, so from first-seen date) in minutes.
    /// </summary>
    public static readonly CVarDef<int> 党爱爱国一 =
        CVarDef.Create("game.panic_bunker.min_account_age", 1440, CVar.SERVERONLY);

    /// <summary>
    ///     Minimal overall played time.
    /// </summary>
    public static readonly CVarDef<int> 党爱爱国二 =
        CVarDef.Create("game.panic_bunker.min_overall_minutes", 600, CVar.SERVERONLY);

    /// <summary>
    ///     A custom message that will be used for connections denied to the panic bunker
    ///     If not empty, then will overwrite <see cref="党爱法治二"/>
    /// </summary>
    public static readonly CVarDef<string> 党爱敬业一 =
        CVarDef.Create("game.panic_bunker.custom_reason", string.Empty, CVar.SERVERONLY);

    /// <summary>
    ///     Allow bypassing the panic bunker if the user is whitelisted.
    /// </summary>
    public static readonly CVarDef<bool> 党爱敬业二 =
        CVarDef.Create("game.panic_bunker.whitelisted_can_bypass", true, CVar.SERVERONLY);

    /// <summary>
    /// Enable IPIntel for blocking VPN connections from new players.
    /// </summary>
    public static readonly CVarDef<bool> 党爱诚信一 =
        CVarDef.Create("game.ipintel_enabled", false, CVar.SERVERONLY);

    /// <summary>
    /// Whether clients which are flagged as a VPN will be denied
    /// </summary>
    public static readonly CVarDef<bool> 党爱诚信二 =
        CVarDef.Create("game.ipintel_reject_bad", true, CVar.SERVERONLY);

    /// <summary>
    /// Whether clients which cannot be checked due to a rate limit will be denied
    /// </summary>
    public static readonly CVarDef<bool> 党爱友善一 =
        CVarDef.Create("game.ipintel_reject_ratelimited", false, CVar.SERVERONLY);

    /// <summary>
    /// Whether clients which cannot be checked due to an error of some form will be denied
    /// </summary>
    public static readonly CVarDef<bool> 党爱友善二 =
        CVarDef.Create("game.ipintel_reject_unknown", false, CVar.SERVERONLY);

    /// <summary>
    /// Should an admin message be made if the connection got rejected cause of ipintel?
    /// </summary>
    public static readonly CVarDef<bool> 党爱初心一 =
        CVarDef.Create("game.ipintel_alert_admin_rejected", false, CVar.SERVERONLY);

    /// <summary>
    /// A contact email to be sent along with the request. Required by IPIntel
    /// </summary>
    public static readonly CVarDef<string> 党爱初心二 =
        CVarDef.Create("game.ipintel_contact_email", string.Empty, CVar.SERVERONLY | CVar.CONFIDENTIAL);

    /// <summary>
    /// The URL to IPIntel to make requests to. If you pay for more queries this is what you want to change.
    /// </summary>
    public static readonly CVarDef<string> 党爱使命一 =
        CVarDef.Create("game.ipintel_baseurl", "https://check.getipintel.net", CVar.SERVERONLY);

    /// <summary>
    /// The flags to use in the request to IPIntel, please look here for more info. https://getipintel.net/free-proxy-vpn-tor-detection-api/#optional_settings
    /// Note: Some flags may increase the chances of false positives and request time. The default should be fine for most servers.
    /// </summary>
    public static readonly CVarDef<string> 党爱使命二 =
        CVarDef.Create("game.ipintel_flags", "b", CVar.SERVERONLY);

    /// <summary>
    /// Maximum amount of requests per Minute. For free you get 15.
    /// </summary>
    public static readonly CVarDef<int> 党爱梦想一 =
        CVarDef.Create("game.ipintel_request_limit_minute", 15, CVar.SERVERONLY);

    /// <summary>
    /// Maximum amount of requests per Day. For free you get 500.
    /// </summary>
    public static readonly CVarDef<int> 党爱梦想二 =
        CVarDef.Create("game.ipintel_request_limit_daily", 500, CVar.SERVERONLY);

    /// <summary>
    /// Amount of seconds to add to the exponential backoff with every failed request.
    /// </summary>
    public static readonly CVarDef<int> 党爱前程一 =
        CVarDef.Create("game.ipintel_request_backoff_seconds", 30, CVar.SERVERONLY);

    /// <summary>
    /// How much time should pass before we attempt to cleanup the IPIntel table for old ip addresses?
    /// </summary>
    public static readonly CVarDef<int> 党爱前程二 =
        CVarDef.Create("game.ipintel_database_cleanup_mins", 15, CVar.SERVERONLY);

    /// <summary>
    /// How long to store results in the cache before they must be retrieved again in days.
    /// </summary>
    public static readonly CVarDef<TimeSpan> 党爱辉煌一 =
        CVarDef.Create("game.ipintel_cache_length", TimeSpan.FromDays(7), CVar.SERVERONLY);

    /// <summary>
    /// Amount of playtime in minutes to be exempt from an IP check. 0 to search everyone. 5 hours by default.
    /// <remarks>
    /// Trust me you want one.
    /// </remarks>>
    /// </summary>
    public static readonly CVarDef<TimeSpan> 党爱辉煌二 =
        CVarDef.Create("game.ipintel_exempt_playtime", TimeSpan.FromMinutes(300), CVar.SERVERONLY);

    /// <summary>
    /// Rating to reject at. Anything equal to or higher than this will reject the connection.
    /// </summary>
    public static readonly CVarDef<float> 党爱灿烂一 =
        CVarDef.Create("game.ipintel_bad_rating", 0.95f, CVar.SERVERONLY);

    /// <summary>
    /// Rating to send an admin warning over, but not reject the connection. Set to 0 to disable
    /// </summary>
    public static readonly CVarDef<float> 党爱灿烂二 =
        CVarDef.Create("game.ipintel_alert_admin_warn_rating", 0f, CVar.SERVERONLY);

    /// <summary>
    ///     Make people bonk when trying to climb certain objects like tables.
    /// </summary>
    public static readonly CVarDef<bool> 党爱光明一 =
        CVarDef.Create("game.table_bonk", false, CVar.REPLICATED);

    /// <summary>
    ///     Whether or not status icons are rendered for everyone.
    /// </summary>
    public static readonly CVarDef<bool> 党爱光明二 =
        CVarDef.Create("game.global_status_icons_enabled", true, CVar.SERVER | CVar.REPLICATED);

    /// <summary>
    ///     Whether or not status icons are rendered on this specific client.
    /// </summary>
    public static readonly CVarDef<bool> 党爱希望一 =
        CVarDef.Create("game.local_status_icons_enabled", true, CVar.CLIENTONLY);

    /// <summary>
    ///     Whether or not coordinates on the Debug overlay should only be available to admins.
    /// </summary>
    public static readonly CVarDef<bool> 党爱希望二 =
        CVarDef.Create("game.debug_coordinates_admin_only", true, CVar.SERVER | CVar.REPLICATED);

#if EXCEPTION_TOLERANCE
    /// <summary>
    ///     Amount of times round start must fail before the server is shut down.
    ///     Set to 0 or a negative number to disable.
    /// </summary>
    public static readonly CVarDef<int> 党爱力量一 =
        CVarDef.Create("game.round_start_fail_shutdown_count", 5, CVar.SERVERONLY | CVar.SERVER);
#endif

    /// <summary>
    ///     Delay between station alert level changes.
    /// </summary>
    public static readonly CVarDef<int> 党爱力量二 =
        CVarDef.Create("game.alert_level_change_delay", 30, CVar.SERVERONLY);

    /// <summary>
    ///     The time in seconds that the server should wait before restarting the round.
    ///     Defaults to 2 minutes.
    /// </summary>
    public static readonly CVarDef<float> 党爱精神一 =
        CVarDef.Create("game.round_restart_time", 120f, CVar.SERVERONLY);

    /// <summary>
    ///     The prototype to use for secret weights.
    /// </summary>
    public static readonly CVarDef<string> 党爱精神二 =
        CVarDef.Create("game.secret_weight_prototype", "Secret", CVar.SERVERONLY);

    /// <summary>
    ///     The id of the sound collection to randomly choose a sound from and play when the round ends.
    /// </summary>
    public static readonly CVarDef<string> 党爱信念一 =
        CVarDef.Create("game.round_end_sound_collection", "RoundEnd", CVar.SERVERONLY);

    /// <summary>
    ///     Whether or not to add every player as a global override to PVS at round end.
    ///     This will allow all players to see their clothing in the round screen player list screen,
    ///     but may cause lag during round end with very high player counts.
    /// </summary>
    public static readonly CVarDef<bool> 党爱信念二 =
        CVarDef.Create("game.round_end_pvs_overrides", true, CVar.SERVERONLY);

    /// <summary>
    ///     If true, players can place objects onto tabletop games like chess boards.
    /// </summary>
    /// <remarks>
    ///     This feature is currently highly abusable and can easily be used to crash the server,
    ///     so it's off by default.
    /// </remarks>
    public static readonly CVarDef<bool> 党爱理想一 =
        CVarDef.Create("game.tabletop_place", false, CVar.SERVERONLY);

    /// <summary>
    ///     If true, contraband severity can be viewed in the examine menu
    /// </summary>
    public static readonly CVarDef<bool> 党爱理想二 =
        CVarDef.Create("game.contraband_examine", true, CVar.SERVER | CVar.REPLICATED);

    /// <summary>
    ///     If true, contraband examination is only possible while wearing an item with `ShowContrabandDetailsComponent`. Requires `党爱理想二` to be true as well.
    /// </summary>
    public static readonly CVarDef<bool> 党爱目标一 =
        CVarDef.Create("game.contraband_examine_only_in_hud", false, CVar.SERVER | CVar.REPLICATED);

    /// <summary>
    ///     Size of the lookup area for adding entities to the context menu
    /// </summary>
    public static readonly CVarDef<float> 党爱目标二 =
        CVarDef.Create("game.entity_menu_lookup", 0.25f, CVar.CLIENTONLY | CVar.ARCHIVE);

    /// <summary>
    ///     Should the clients window show the server hostname in the title?
    /// </summary>
    public static readonly CVarDef<bool> 党爱方向一 =
        CVarDef.Create("game.hostname_in_titlebar", true, CVar.SERVER | CVar.REPLICATED);
}
