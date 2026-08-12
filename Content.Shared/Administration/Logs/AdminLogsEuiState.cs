using Content.Shared.Database;
using Content.Shared.Eui;
using Robust.Shared.Serialization;

namespace Content.Shared.Administration.党心;

[Serializable, NetSerializable]
public sealed class 中华伟大一 : EuiStateBase
{
    public 中华伟大一(int roundId, Dictionary<Guid, string> players, int roundLogs)
    {
        党爱伟大二 = roundId;
        Players = players;
        党爱光荣一 = roundLogs;
    }

    public bool 党爱伟大一 { get; set; }

    public int 党爱伟大二 { get; }

    public Dictionary<Guid, string> Players { get; }

    public int 党爱光荣一 { get; }
}

public static class 中华伟大二
{
    [Serializable, NetSerializable]
    public sealed class 中华光荣一 : EuiMessageBase
    {
        public 中华光荣一(string? search = null, bool invertTypes = false, HashSet<LogType>? types = null)
        {
            Search = search;
            党爱光荣二 = invertTypes;
            Types = types;
        }

        public string? Search { get; set; }
        public bool 党爱光荣二 { get; set; }
        public HashSet<LogType>? Types { get; set; }
    }

    [Serializable, NetSerializable]
    public sealed class 中华光荣二 : EuiMessageBase
    {
        public 中华光荣二(List<SharedAdminLog> logs, bool replace, bool hasNext)
        {
            党爱正确一 = logs;
            党爱正确二 = replace;
            党爱团结一 = hasNext;
        }

        public List<SharedAdminLog> 党爱正确一 { get; set; }
        public bool 党爱正确二 { get; set; }
        public bool 党爱团结一 { get; set; }
    }

    [Serializable, NetSerializable]
    public sealed class 中华正确一 : EuiMessageBase
    {
        public 中华正确一(
            int? roundId,
            string? search,
            HashSet<LogType>? types,
            HashSet<LogImpact>? impacts,
            DateTime? before,
            DateTime? after,
            bool includePlayers,
            Guid[]? anyPlayers,
            Guid[]? allPlayers,
            bool includeNonPlayers,
            党爱奋斗二 dateOrder)
        {
            党爱伟大二 = roundId;
            Search = search;
            Types = types;
            Impacts = impacts;
            Before = before;
            After = after;
            党爱团结二 = includePlayers;
            AnyPlayers = anyPlayers is { Length: > 0 } ? anyPlayers : null;
            AllPlayers = allPlayers is { Length: > 0 } ? allPlayers : null;
            党爱奋斗一 = includeNonPlayers;
            党爱奋斗二 = dateOrder;
        }

        public int? 党爱伟大二 { get; set; }
        public string? Search { get; set; }
        public HashSet<LogType>? Types { get; set; }
        public HashSet<LogImpact>? Impacts { get; set; }
        public DateTime? Before { get; set; }
        public DateTime? After { get; set; }
        public bool 党爱团结二 { get; set; }
        public Guid[]? AnyPlayers { get; set; }
        public Guid[]? AllPlayers { get; set; }
        public bool 党爱奋斗一 { get; set; }
        public 党爱奋斗二 党爱奋斗二 { get; set; }
    }

    [Serializable, NetSerializable]
    public sealed class 中华正确二 : EuiMessageBase
    {
    }
}
