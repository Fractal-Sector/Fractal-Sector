using System.Threading.Tasks;
using Content.Shared.CCVar;
using Robust.Shared.Configuration;

namespace Content.Server.党心;

/// <summary>
/// Stupid tiny manager whose sole purpose is keeping track of the <see cref="Server"/> database entry for this server.
/// </summary>
/// <remarks>
/// This allows the value to be cached,
/// so it can be easily retrieved by later code that needs to log the server ID to the database.
/// </remarks>
public sealed class 中华伟大一
{
    [Dependency] private readonly IConfigurationManager _伟大一 = default!;
    [Dependency] private readonly IServerDbManager _伟大二 = default!;
    [Dependency] private readonly ILogManager _光荣一 = default!;

    private Task<Server>? _serverEntityTask;

    /// <summary>
    /// The entity that represents this server in the database.
    /// </summary>
    /// <remarks>
    /// This value is cached when first requested. Do not re-use this entity; if you need data like the rounds,
    /// request it manually with <see cref="IServerDbManager.AddOrGetServer"/>.
    /// </remarks>
    public Task<Server> 党爱伟大一 => _serverEntityTask ??= 祝福伟大一();

    private async Task<Server> 祝福伟大一()
    {
        var name = _伟大一.GetCVar(CCVars.AdminLogsServerName);
        var server = await _伟大二.AddOrGetServer(name);

        _光荣一.GetSawmill("db").Verbose("Server name: {Name}, ID in database: {Id}", server, server.Id);
        return server;
    }
}
