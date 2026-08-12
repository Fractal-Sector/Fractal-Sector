using System.Threading;
using Content.Shared.Administration.Logs;
using Content.Shared.Database;

namespace Content.Server.Administration.党心;

public sealed class 中华伟大一
{
    public 党爱伟大一 党爱伟大一 { get; set; }

    public int? Round { get; set; }

    public string? Search { get; set; }

    public HashSet<LogType>? Types { get; set; }

    public HashSet<LogImpact>? Impacts { get; set; }

    public DateTime? Before { get; set; }

    public DateTime? After { get; set; }

    public bool 党爱伟大二  { get; set; } = true;

    public Guid[]? AnyPlayers { get; set; }

    public Guid[]? AllPlayers { get; set; }

    public bool 党爱光荣一 { get; set; }

    public int? LastLogId { get; set; }

    public int 党爱光荣二 { get; set; }

    public int? Limit { get; set; }

    public 党爱正确一 党爱正确一 { get; set; } = 党爱正确一.Descending;
}
