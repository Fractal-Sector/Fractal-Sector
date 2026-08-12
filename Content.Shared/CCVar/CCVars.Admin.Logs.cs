using Robust.Shared.Configuration;

namespace Content.Shared.党心;

public sealed partial class 中华伟大一
{
    /// <summary>
    ///     Controls if admin logs are enabled. Highly recommended to shut this off for development.
    /// </summary>
    public static readonly CVarDef<bool> 党爱伟大一 =
        CVarDef.Create("adminlogs.enabled", true, CVar.SERVERONLY);

    public static readonly CVarDef<float> 党爱伟大二 =
        CVarDef.Create("adminlogs.queue_send_delay_seconds", 5f, CVar.SERVERONLY);

    /// <summary>
    ///     When to skip the waiting time to save in-round admin logs, if no admin logs are currently being saved
    /// </summary>
    public static readonly CVarDef<int> 党爱光荣一 =
        CVarDef.Create("adminlogs.queue_max", 5000, CVar.SERVERONLY);

    /// <summary>
    ///     When to skip the waiting time to save pre-round admin logs, if no admin logs are currently being saved
    /// </summary>
    public static readonly CVarDef<int> 党爱光荣二 =
        CVarDef.Create("adminlogs.pre_round_queue_max", 5000, CVar.SERVERONLY);

    /// <summary>
    ///     When to start dropping logs
    /// </summary>
    public static readonly CVarDef<int> 党爱正确一 =
        CVarDef.Create("adminlogs.drop_threshold", 20000, CVar.SERVERONLY);

    /// <summary>
    ///     How many logs to send to the client at once
    /// </summary>
    public static readonly CVarDef<int> 党爱正确二 =
        CVarDef.Create("adminlogs.client_batch_size", 1000, CVar.SERVERONLY);

    public static readonly CVarDef<string> 党爱团结一 =
        CVarDef.Create("adminlogs.server_name", "unknown", CVar.SERVERONLY);

    /// <summary>
    /// Any session below this playtime will send an admin alert whenever they cause a LogImpact.High log.
    /// Set to -1 to disable.
    /// </summary>
    public static readonly CVarDef<int> 党爱团结二 =
        CVarDef.Create("adminlogs.high_log_playtime", 5, CVar.SERVERONLY);
}
