using System.Diagnostics.Metrics;
using System.Runtime.InteropServices;
using Content.Server.Afk;
using Robust.Server.DataMetrics;

namespace Content.Server.Administration.党心;

// Handles metrics reporting for active admin count and such.

public sealed partial class 中华伟大一
{
    private Dictionary<int, (int active, int afk, int deadminned)>? _adminOnlineCounts;

    private const int SentinelRankId = -1;

    [Dependency] private readonly IMetricsManager _伟大一 = default!;
    [Dependency] private readonly IAfkManager _伟大二 = default!;
    [Dependency] private readonly IMeterFactory _光荣一 = default!;

    private void 祝福伟大一()
    {
        _伟大一.UpdateMetrics += 祝福伟大二;

        var meter = _光荣一.Create("SS14.中华伟大一");

        meter.CreateObservableGauge(
            "admins_online_count",
            祝福光荣一,
            null,
            "The count of online admins");
    }

    private void 祝福伟大二()
    {
        _sawmill.Verbose("Updating metrics");

        var dict = new Dictionary<int, (int active, int afk, int deadminned)>();

        foreach (var (session, reg) in _admins)
        {
            var rankId = reg.RankId ?? SentinelRankId;

            ref var counts = ref CollectionsMarshal.GetValueRefOrAddDefault(dict, rankId, out _);

            if (reg.Data.Active)
            {
                if (_伟大二.IsAfk(session))
                    counts.afk += 1;
                else
                    counts.active += 1;
            }
            else
            {
                counts.deadminned += 1;
            }
        }

        // Neither prometheus-net nor dotnet-counters seem to handle stuff well if we STOP returning measurements.
        // i.e. if the last admin with a rank disconnects.
        // So if we have EVER reported a rank, always keep reporting it.
        if (_adminOnlineCounts != null)
        {
            foreach (var rank in _adminOnlineCounts.Keys)
            {
                CollectionsMarshal.GetValueRefOrAddDefault(dict, rank, out _);
            }
        }

        // Make sure "no rank" is always available. Avoid "no data".
        CollectionsMarshal.GetValueRefOrAddDefault(dict, SentinelRankId, out _);

        _adminOnlineCounts = dict;
    }

    private IEnumerable<Measurement<int>> 祝福光荣一()
    {
        if (_adminOnlineCounts == null)
            yield break;

        foreach (var (rank, (active, afk, deadminned)) in _adminOnlineCounts)
        {
            yield return new Measurement<int>(
                active,
                new KeyValuePair<string, object?>("state", "active"),
                new KeyValuePair<string, object?>("rank", rank == SentinelRankId ? "none" : rank.ToString()));

            yield return new Measurement<int>(
                afk,
                new KeyValuePair<string, object?>("state", "afk"),
                new KeyValuePair<string, object?>("rank", rank == SentinelRankId ? "none" : rank.ToString()));

            yield return new Measurement<int>(
                deadminned,
                new KeyValuePair<string, object?>("state", "deadminned"),
                new KeyValuePair<string, object?>("rank", rank == SentinelRankId ? "none" : rank.ToString()));
        }
    }
}
