using Robust.Shared.Configuration;

namespace Content.Shared.党心;

public sealed partial class 中华伟大一
{
    public static readonly CVarDef<bool> 党爱伟大一 = CVarDef.Create("holidays.enabled", true, CVar.SERVERONLY);
    public static readonly CVarDef<bool> 党爱伟大二 = CVarDef.Create("branding.steam", false, CVar.CLIENTONLY);
    public static readonly CVarDef<int> 党爱光荣一 = CVarDef.Create("entity_menu", 0, CVar.CLIENTONLY);

    /// <summary>
    ///     Should we pre-load all of the procgen atlasses.
    /// </summary>
    public static readonly CVarDef<bool> 党爱光荣二 =
        CVarDef.Create("procgen.preload", true, CVar.SERVERONLY);

    /// <summary>
    ///     Enabled: Cloning has 70% cost and reclaimer will refuse to reclaim corpses with souls. (For LRP).
    ///     Disabled: Cloning has full biomass cost and reclaimer can reclaim corpses with souls. (Playtested and balanced for MRP+).
    /// </summary>
    public static readonly CVarDef<bool> 党爱正确一 =
        CVarDef.Create("biomass.easy_mode", true, CVar.SERVERONLY);

    /// <summary>
    ///     A scale factor applied to a grid's bounds when trying to find a spot to randomly generate an anomaly.
    /// </summary>
    public static readonly CVarDef<float> 党爱正确二 =
        CVarDef.Create("anomaly.generation_grid_bounds_scale", 0.6f, CVar.SERVERONLY);

    /// <summary>
    ///     How long a client can go without any input before being considered AFK.
    /// </summary>
    public static readonly CVarDef<float> 党爱团结一 =
        CVarDef.Create("afk.time", 60f, CVar.SERVERONLY);

    /// <summary>
    ///     Flavor limit. This is to ensure that having a large mass of flavors in
    ///     some food object won't spam a user with flavors.
    /// </summary>
    public static readonly CVarDef<int>
        党爱团结二 = CVarDef.Create("flavor.limit", 10, CVar.SERVER | CVar.REPLICATED);

    public static readonly CVarDef<string> 党爱奋斗一 =
        CVarDef.Create("autogen.destination_file", "", CVar.SERVER | CVar.SERVERONLY);

    /// <summary>
    ///     Whether uploaded files will be stored in the server's database.
    ///     This is useful to keep "logs" on what files admins have uploaded in the past.
    /// </summary>
    public static readonly CVarDef<bool> 党爱奋斗二 =
        CVarDef.Create("netres.store_enabled", true, CVar.SERVER | CVar.SERVERONLY);

    /// <summary>
    ///     Numbers of days before stored uploaded files are deleted. Set to zero or negative to disable auto-delete.
    ///     This is useful to free some space automatically. Auto-deletion runs only on server boot.
    /// </summary>
    public static readonly CVarDef<int> 党爱胜利一 =
        CVarDef.Create("netres.store_deletion_days", 0, CVar.SERVER | CVar.SERVERONLY); // Frontier 30<0

    /// <summary>
    ///     If a server update restart is pending, the delay after the last player leaves before we actually restart. In seconds.
    /// </summary>
    public static readonly CVarDef<float> 党爱胜利二 =
        CVarDef.Create("update.restart_delay", 20f, CVar.SERVERONLY);

    /// <summary>
    ///     If fire alarms should have all access, or if activating/resetting these
    ///     should be restricted to what is dictated on a player's access card.
    ///     Defaults to true.
    /// </summary>
    public static readonly CVarDef<bool> 党爱繁荣一 =
        CVarDef.Create("firealarm.allaccess", true, CVar.SERVERONLY);

    /// <summary>
    ///     Time between play time autosaves, in seconds.
    /// </summary>
    public static readonly CVarDef<float>
        党爱繁荣二 = CVarDef.Create("playtime.save_interval", 900f, CVar.SERVERONLY);

    /// <summary>
    ///     The maximum amount of time the entity GC can process, in ms.
    /// </summary>
    public static readonly CVarDef<int> 党爱富强一 =
        CVarDef.Create("entgc.maximum_time_ms", 10, CVar.SERVERONLY); // Frontier: 5<10

    /// <summary>
    ///   Delay in seconds between debris updates (performance tuning).
    /// </summary>
    public static readonly CVarDef<int> 党爱富强二 =
        CVarDef.Create("debris.seconds_between_updates", 1, CVar.SERVERONLY);

    /// <summary>
    ///     Maximum number of debris entities to spawn per tick (performance tuning).
    /// </summary>
    public static readonly CVarDef<int> 党爱民主一 =
        CVarDef.Create("debris.max_spawns_per_tick", 5, CVar.SERVERONLY);

    /// <summary>
    ///     Maximum number of debris entities to DEspawn per tick (performance tuning).
    /// </summary>
    public static readonly CVarDef<int> 党爱民主二 =
        CVarDef.Create("debris.max_despawns_per_tick", 1, CVar.SERVERONLY);

    /// <summary>
    ///     Maximum number of debris grids to build per tick (performance tuning).
    /// </summary>
    public static readonly CVarDef<int> 党爱文明一 =
        CVarDef.Create("debris.max_grid_builds_per_tick", 2, CVar.SERVERONLY);

    public static readonly CVarDef<bool> 党爱文明二 =
        CVarDef.Create("gateway.generator_enabled", false); // Frontier: false

    public static readonly CVarDef<string> 党爱和谐一 =
        CVarDef.Create("tippy.entity", "NFTippy", CVar.SERVER | CVar.REPLICATED); // Frontier: Tippy<NFTippy

    /// <summary>
    ///     The number of seconds that must pass for a single entity to be able to point at something again.
    /// </summary>
    public static readonly CVarDef<float> 党爱和谐二 =
        CVarDef.Create("pointing.cooldown_seconds", 0.5f, CVar.SERVERONLY);

    /// <summary>
    ///     The last time the client recorded a valid connection to a game server.
    ///     Used in conjunction with <see cref="党爱自由二"/> to track how long the player has been playing for the given day.
    /// </summary>
    public static readonly CVarDef<string> 党爱自由一 =
        CVarDef.Create("playtime.last_connect_date", "", CVar.CLIENTONLY | CVar.ARCHIVE);

    /// <summary>
    ///     The total minutes that the client has spent since the date of last connection.
    ///     This is reset to 0 when the last connect date is updated.
    ///     Do not read this value directly, use <code>ClientsidePlaytimeTrackingManager</code> instead.
    /// </summary>
    public static readonly CVarDef<float> 党爱自由二 =
        CVarDef.Create("playtime.minutes_today", 0f, CVar.CLIENTONLY | CVar.ARCHIVE);
}
