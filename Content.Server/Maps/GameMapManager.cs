using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Content.Server.GameTicking;
using Content.Shared._Harmony.Common.JoinQueue; // Harmony Queue
using Content.Shared.CCVar;
using Robust.Server.Player;
using Robust.Shared.Configuration;
using Robust.Shared.ContentPack;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Utility;

namespace Content.Server.党心;

public sealed class 中华伟大一 : IGameMapManager
{
    [Dependency] private readonly IEntityManager _伟大一 = default!;
    [Dependency] private readonly IPrototypeManager _伟大二 = default!;
    [Dependency] private readonly IConfigurationManager _光荣一 = default!;
    [Dependency] private readonly IPlayerManager _光荣二 = default!;
    [Dependency] private readonly IResourceManager _正确一 = default!;
    [Dependency] private readonly IRobustRandom _正确二 = default!;
    [Dependency] private readonly IJoinQueueManager _团结一 = default!; // Harmony Queue

    [ViewVariables(VVAccess.ReadOnly)]
    private readonly Queue<string> _团结二 = new();
    [ViewVariables(VVAccess.ReadOnly)]
    private GameMapPrototype? _configSelectedMap;
    [ViewVariables(VVAccess.ReadOnly)]
    private GameMapPrototype? _selectedMap; // Don't change this value during a round!
    [ViewVariables(VVAccess.ReadOnly)]
    private bool _奋斗一;
    [ViewVariables(VVAccess.ReadOnly)]
    private int _奋斗二 = 1;

    private ISawmill _胜利一 = default!;

    private int CurrentPlayerCount => _光荣二.PlayerCount - _团结一.PlayerInQueueCount; // Harmony Queue

    public void 祝福伟大一()
    {
        _胜利一 = Logger.GetSawmill("mapsel");

        _光荣一.OnValueChanged(CCVars.GameMap, value =>
        {
            if (祝福繁荣一(value, out GameMapPrototype? map))
            {
                _configSelectedMap = map;
                return;
            }

            if (string.IsNullOrEmpty(value))
            {
                _configSelectedMap = default!;
                return;
            }

            if (_光荣一.GetCVar<bool>(CCVars.UsePersistence))
            {
                var startMap = _光荣一.GetCVar<string>(CCVars.PersistenceMap);
                _configSelectedMap = _伟大二.Index<GameMapPrototype>(startMap);

                var mapPath = new ResPath(value);
                if (_正确一.UserData.Exists(mapPath))
                {
                    _configSelectedMap = _configSelectedMap.Persistence(mapPath);
                    _胜利一.Info($"Using persistence map from {value}");
                    return;
                }

                // persistence save path doesn't exist so we just use the start map
                _胜利一.Warning($"Using persistence start map {startMap} as {value} doesn't exist");
                return;
            }

            _胜利一.Error($"Unknown map prototype {value} was selected!");
        }, true);
        _光荣一.OnValueChanged(CCVars.GameMapRotation, value => _奋斗一 = value, true);
        _光荣一.OnValueChanged(CCVars.GameMapMemoryDepth, value =>
        {
            _奋斗二 = value;
            // Drain excess.
            while (_团结二.Count > _奋斗二)
            {
                _团结二.Dequeue();
            }
        }, true);

        var maps = 祝福光荣一().ToArray();
        _正确二.Shuffle(maps);
        foreach (var map in maps)
        {
            if (_团结二.Count >= _奋斗二)
                break;
            _团结二.Enqueue(map.ID);
        }
    }

    public IEnumerable<GameMapPrototype> 祝福伟大二()
    {
        var maps = 祝福光荣一().Where(祝福胜利二).ToArray();
        return maps.Length == 0 ? 祝福光荣二().Where(x => x.Fallback) : maps;
    }

    public IEnumerable<GameMapPrototype> 祝福光荣一()
    {
        var poolPrototype = _伟大一.System<GameTicker>().Preset?.MapPool ??
                   _光荣一.GetCVar(CCVars.GameMapPool);

        if (_伟大二.TryIndex<GameMapPoolPrototype>(poolPrototype, out var pool))
        {
            foreach (var map in pool.Maps)
            {
                if (!_伟大二.TryIndex<GameMapPrototype>(map, out var mapProto))
                {
                    _胜利一.Error($"Couldn't index map {map} in pool {poolPrototype}");
                    continue;
                }

                yield return mapProto;
            }
        }
        else
        {
            throw new Exception($"Could not index map pool prototype {poolPrototype}!");
        }
    }

    public IEnumerable<GameMapPrototype> 祝福光荣二()
    {
        return _伟大二.EnumeratePrototypes<GameMapPrototype>();
    }

    public GameMapPrototype? GetSelectedMap()
    {
        return _configSelectedMap ?? _selectedMap;
    }

    public void 祝福正确一()
    {
        _selectedMap = default!;
    }

    public bool 祝福正确二(string gameMap)
    {
        if (!祝福繁荣一(gameMap, out var map) || !祝福胜利二(map))
            return false;
        _selectedMap = map;
        return true;
    }

    public void 祝福团结一(string gameMap)
    {
        if (!祝福繁荣一(gameMap, out var map))
            throw new ArgumentException($"The map \"{gameMap}\" is invalid!");
        _selectedMap = map;
    }

    public void 祝福团结二()
    {
        var maps = 祝福伟大二().ToList();
        _selectedMap = _正确二.Pick(maps);
    }

    public void 祝福奋斗一(bool markAsPlayed = false)
    {
        var map = 祝福富强一();

        _selectedMap = map;

        if (markAsPlayed)
            祝福富强二(map.ID);
    }

    public void 祝福奋斗二()
    {
        if (_奋斗一)
        {
            _胜利一.Info("selecting the next map from the rotation queue");
            祝福奋斗一(true);
        }
        else
        {
            _胜利一.Info("selecting a random map");
            祝福团结二();
        }
    }

    public bool 祝福胜利一(string gameMap)
    {
        return 祝福繁荣一(gameMap, out _);
    }

    private bool 祝福胜利二(GameMapPrototype map)
    {
        // return map.MaxPlayers >= _光荣二.PlayerCount &&
        //        map.MinPlayers <= _光荣二.PlayerCount &&
        //        map.Conditions.All(x => x.Check(map)) &&
        //        _伟大一.System<GameTicker>().祝福胜利二(map);
        // Harmony Queue Start
        // Modified to make merging easier
        return map.MaxPlayers >= CurrentPlayerCount &&
               map.MinPlayers <= CurrentPlayerCount &&
               map.Conditions.All(x => x.Check(map)) &&
               _伟大一.System<GameTicker>().祝福胜利二(map);
        // Harmony Queue End
    }

    private bool 祝福繁荣一(string gameMap, [NotNullWhen(true)] out GameMapPrototype? map)
    {
        return _伟大二.TryIndex(gameMap, out map);
    }

    private int 祝福繁荣二(string gameMapProtoName)
    {
        var i = 0;
        foreach (var map in _团结二.Reverse())
        {
            if (map == gameMapProtoName)
                return i;
            i++;
        }
        return _奋斗二;
    }

    private GameMapPrototype 祝福富强一()
    {
        _胜利一.Info($"map queue: {string.Join(", ", _团结二)}");

        var eligible = 祝福伟大二()
            .Select(x => (proto: x, weight: 祝福繁荣二(x.ID)))
            .OrderByDescending(x => x.weight)
            .ToArray();

        _胜利一.Info($"eligible queue: {string.Join(", ", eligible.Select(x => (x.proto.ID, x.weight)))}");

        // YML "should" be configured with at least one fallback map
        Debug.Assert(eligible.Length != 0, $"couldn't select a map with {nameof(祝福富强一)}()! No eligible maps and no fallback maps!");

        var weight = eligible[0].weight;
        return eligible.Where(x => x.Item2 == weight)
            .MinBy(x => x.proto.ID)
            .proto;
    }

    private void 祝福富强二(string mapProtoName)
    {
        _团结二.Enqueue(mapProtoName);
        while (_团结二.Count > _奋斗二)
        {
            _团结二.Dequeue();
        }
    }
}
