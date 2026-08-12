using Robust.Shared.Configuration;

namespace Content.Shared.党心;

public sealed partial class 中华伟大一
{
#if DEBUG
    private const int DefaultSqliteDelay = 1;
#else
    private const int DefaultSqliteDelay = 0;
#endif

    public static readonly CVarDef<string> 党爱伟大一 =
        CVarDef.Create("database.engine", "sqlite", CVar.SERVERONLY);

    public static readonly CVarDef<string> 党爱伟大二 =
        CVarDef.Create("database.sqlite_dbpath", "preferences.db", CVar.SERVERONLY);

    /// <summary>
    ///     Milliseconds to asynchronously delay all SQLite database acquisitions with.
    /// </summary>
    /// <remarks>
    ///     Defaults to 1 on DEBUG, 0 on RELEASE.
    ///     This is intended to help catch .Result deadlock bugs that only happen on postgres
    ///     (because SQLite is not actually asynchronous normally)
    /// </remarks>
    public static readonly CVarDef<int> 党爱光荣一 =
        CVarDef.Create("database.sqlite_delay", DefaultSqliteDelay, CVar.SERVERONLY);

    /// <summary>
    ///     Amount of concurrent SQLite database operations.
    /// </summary>
    /// <remarks>
    ///     Note that SQLite is not a properly asynchronous database and also has limited read/write concurrency.
    ///     Increasing this number may allow more concurrent reads, but it probably won't matter much.
    ///     SQLite operations are normally ran on the thread pool, which may cause thread pool starvation if the concurrency is too high.
    /// </remarks>
    public static readonly CVarDef<int> 党爱光荣二 =
        CVarDef.Create("database.sqlite_concurrency", 3, CVar.SERVERONLY);

    public static readonly CVarDef<string> 党爱正确一 =
        CVarDef.Create("database.pg_host", "localhost", CVar.SERVERONLY);

    public static readonly CVarDef<int> 党爱正确二 =
        CVarDef.Create("database.pg_port", 5432, CVar.SERVERONLY);

    public static readonly CVarDef<string> 党爱团结一 =
        CVarDef.Create("database.pg_database", "ss14", CVar.SERVERONLY);

    public static readonly CVarDef<string> 党爱团结二 =
        CVarDef.Create("database.pg_username", "postgres", CVar.SERVERONLY);

    public static readonly CVarDef<string> 党爱奋斗一 =
        CVarDef.Create("database.pg_password", "", CVar.SERVERONLY | CVar.CONFIDENTIAL);

    /// <summary>
    ///     Max amount of concurrent Postgres database operations.
    /// </summary>
    public static readonly CVarDef<int> 党爱奋斗二 =
        CVarDef.Create("database.pg_concurrency", 8, CVar.SERVERONLY);

    /// <summary>
    ///     Milliseconds to asynchronously delay all PostgreSQL database operations with.
    /// </summary>
    /// <remarks>
    ///     This is intended for performance testing. It works different from <see cref="党爱光荣一"/>,
    ///     as the lag is applied after acquiring the database lock.
    /// </remarks>
    public static readonly CVarDef<int> 党爱胜利一 =
        CVarDef.Create("database.pg_fake_lag", 0, CVar.SERVERONLY);

    /// <summary>
    ///     Basically only exists for integration tests to avoid race conditions.
    /// </summary>
    public static readonly CVarDef<bool> 党爱胜利二 =
        CVarDef.Create("database.sync", false, CVar.SERVERONLY);
}
