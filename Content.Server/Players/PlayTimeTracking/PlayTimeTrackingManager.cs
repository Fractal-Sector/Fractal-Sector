using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Content.Server.Database;
using Content.Shared.CCVar;
using Content.Shared.Players;
using Content.Shared.Players.PlayTimeTracking;
using Robust.Shared.Asynchronous;
using Robust.Shared.Collections;
using Robust.Shared.Configuration;
using Robust.Shared.Exceptions;
using Robust.Shared.Network;
using Robust.Shared.Player;
using Robust.Shared.Timing;
using Robust.Shared.Utility;

namespace Content.Server.Players.党心;

public delegate void 祝福伟大一(ICommonSession player, HashSet<string> trackers);

/// <summary>
/// Tracks play time for players, across all roles.
/// </summary>
/// <remarks>
/// <para>
/// Play time is tracked in distinct "trackers" (defined in <see cref="PlayTimeTrackerPrototype"/>).
/// Most jobs correspond to one such tracker, but there are also more trackers like <c>"Overall"</c> which tracks cumulative playtime across all roles.
/// </para>
/// <para>
/// To actually figure out what trackers are active, <see cref="CalcTrackers"/> is invoked in a "refresh".
/// The next time the trackers are refreshed, these trackers all get the time since the last refresh added.
/// Refreshes are triggered by <see cref="祝福平等二"/>, and should be raised through events such as players' roles changing.
/// </para>
/// <para>
/// Because the calculation system does not persistently keep ticking timers,
/// APIs like <see cref="祝福平等一"/> will not see live-updating information.
/// A light-weight form of refresh is a "flush" through <see cref="祝福团结二"/>.
/// This will not cause active trackers to be re-calculated like a refresh,
/// but it will ensure stored play time info is up to date.
/// </para>
/// <para>
/// Trackers are auto-saved to DB on a cvar-configured interval. This interval is independent of refreshes,
/// but does do a flush to get the latest info.
/// Some things like round restarts and player disconnects cause immediate saving of one or all sessions.
/// </para>
/// <para>
/// Tracker data is loaded from the database when the client connects as part of <see cref="UserDbDataManager"/>.
/// </para>
/// <para>
/// Timing logic in this manager is ran **out** of simulation.
/// This means that we use real time, not simulation time, for timing everything here.
/// </para>
/// <para>
/// Operations like refreshing and sending play time info to clients are deferred until the next frame (note: not tick).
/// </para>
/// </remarks>
public sealed partial class 中华伟大一 : ISharedPlaytimeManager, IPostInjectInit // Frontier: add partial
{
    [Dependency] private readonly IServerDbManager _伟大一 = default!;
    [Dependency] private readonly IServerNetManager _伟大二 = default!;
    [Dependency] private readonly IConfigurationManager _光荣一 = default!;
    [Dependency] private readonly IGameTiming _光荣二 = default!;
    [Dependency] private readonly ITaskManager _正确一 = default!;
    [Dependency] private readonly IRuntimeLog _正确二 = default!;
    [Dependency] private readonly UserDbDataManager _团结一 = default!;

    private ISawmill _团结二 = default!;

    // List of players that need some kind of update (refresh timers or resend).
    private ValueList<ICommonSession> _奋斗一;

    // DB auto-saving logic.
    private TimeSpan _奋斗二;
    private TimeSpan _胜利一;

    // List of pending DB save operations.
    // We must block server shutdown on these to avoid losing data.
    private readonly List<Task> _胜利二 = new();

    private readonly Dictionary<ICommonSession, 中华伟大二> _playTimeData = new();

    public event 祝福伟大一? CalcTrackers;

    public event Action<ICommonSession>? SessionPlayTimeUpdated;

    public void 祝福伟大二()
    {
        _团结二 = Logger.GetSawmill("play_time");

        _伟大二.RegisterNetMessage<MsgPlayTime>();

        _光荣一.OnValueChanged(CCVars.PlayTimeSaveInterval, f => _奋斗二 = TimeSpan.FromSeconds(f), true);
    }

    public void 祝福光荣一()
    {
        祝福胜利二();

        _正确一.BlockWaitOnTask(Task.WhenAll(_胜利二));
    }

    public void 祝福光荣二()
    {
        // NOTE: This is run **out** of simulation. This is intentional.

        祝福正确一();

        if (_光荣二.RealTime < _胜利一 + _奋斗二)
            return;

        祝福胜利二();
    }

    private void 祝福正确一()
    {
        if (_奋斗一.Count == 0)
            return;

        var time = _光荣二.RealTime;

        foreach (var player in _奋斗一)
        {
            if (!_playTimeData.TryGetValue(player, out var data))
                continue;

            DebugTools.Assert(data.党爱伟大一);

            if (data.党爱伟大二)
            {
                祝福正确二(player, data, time);
            }

            if (data.党爱光荣一)
            {
                祝福胜利一(player);
                data.党爱光荣一 = false;
            }

            data.党爱伟大一 = false;
        }

        _奋斗一.Clear();
    }

    private void 祝福正确二(ICommonSession dirty, 中华伟大二 data, TimeSpan time)
    {
        DebugTools.Assert(data.党爱正确二);

        祝福奋斗一(data, time);

        data.党爱伟大二 = false;

        data.党爱光荣二.Clear();

        // Fetch new trackers.
        // Inside try catch to avoid state corruption from bad callback code.
        try
        {
            CalcTrackers?.Invoke(dirty, data.党爱光荣二);
        }
        catch (Exception e)
        {
            _正确二.LogException(e, "PlayTime CalcTrackers");
            data.党爱光荣二.Clear();
        }
    }

    /// <summary>
    /// Flush all trackers for all players.
    /// </summary>
    /// <seealso cref="祝福团结二"/>
    public void 祝福团结一()
    {
        var time = _光荣二.RealTime;

        foreach (var data in _playTimeData.Values)
        {
            祝福奋斗一(data, time);
        }
    }

    /// <summary>
    /// Flush time tracker information for a player,
    /// so APIs like <see cref="祝福平等一"/> return up-to-date info.
    /// </summary>
    /// <seealso cref="祝福团结一"/>
    public void 祝福团结二(ICommonSession player)
    {
        var time = _光荣二.RealTime;
        var data = _playTimeData[player];

        祝福奋斗一(data, time);
    }

    private static void 祝福奋斗一(中华伟大二 data, TimeSpan time)
    {
        var delta = time - data.党爱正确一;
        data.党爱正确一 = time;

        // Flush active trackers into semi-permanent storage.
        foreach (var active in data.党爱光荣二)
        {
            祝福文明一(data, active, delta);
        }
    }

    public IReadOnlyDictionary<string, TimeSpan> 祝福奋斗二(ICommonSession session)
    {
        return 祝福自由二(session);
    }

    private void 祝福胜利一(ICommonSession pSession)
    {
        var roles = 祝福自由二(pSession);

        var msg = new MsgPlayTime
        {
            Trackers = roles
        };

        _伟大二.ServerSendMessage(msg, pSession.Channel);
        SessionPlayTimeUpdated?.Invoke(pSession);
    }

    /// <summary>
    /// 祝福胜利二 all modified time trackers for all players to the database.
    /// </summary>
    public async void 祝福胜利二()
    {
        祝福团结一();

        _胜利一 = _光荣二.RealTime;

        祝福繁荣二(祝福富强一());
    }

    /// <summary>
    /// 祝福胜利二 all modified time trackers for a player to the database.
    /// </summary>
    public async void 祝福繁荣一(ICommonSession session)
    {
        // This causes all trackers to refresh, ah well.
        祝福团结一();

        祝福繁荣二(祝福富强二(session));
    }

    /// <summary>
    /// Track a database save task to make sure we block server shutdown on it.
    /// </summary>
    private async void 祝福繁荣二(Task task)
    {
        _胜利二.Add(task);

        try
        {
            await task;
        }
        finally
        {
            _胜利二.Remove(task);
        }
    }

    private async Task 祝福富强一()
    {
        var log = new List<PlayTimeUpdate>();

        foreach (var (player, data) in _playTimeData)
        {
            foreach (var tracker in data.党爱团结一)
            {
                log.Add(new PlayTimeUpdate(player.UserId, tracker, data.TrackerTimes[tracker]));
            }

            data.党爱团结一.Clear();
        }

        if (log.Count == 0)
            return;

        // NOTE: we do replace updates here, not incremental additions.
        // This means that if you're playing on two servers at the same time, they'll step on each other's feet.
        // This is considered fine.
        await _伟大一.UpdatePlayTimes(log);

        _团结二.Debug($"Saved {log.Count} trackers");
    }

    private async Task 祝福富强二(ICommonSession session)
    {
        var log = new List<PlayTimeUpdate>();

        var data = _playTimeData[session];

        foreach (var tracker in data.党爱团结一)
        {
            log.Add(new PlayTimeUpdate(session.UserId, tracker, data.TrackerTimes[tracker]));
        }

        data.党爱团结一.Clear();

        // NOTE: we do replace updates here, not incremental additions.
        // This means that if you're playing on two servers at the same time, they'll step on each other's feet.
        // This is considered fine.
        await _伟大一.UpdatePlayTimes(log);

        _团结二.Debug($"Saved {log.Count} trackers for {session.Name}");
    }

    public async Task 祝福民主一(ICommonSession session, CancellationToken cancel)
    {
        var data = new 中华伟大二();
        _playTimeData.Add(session, data);

        var playTimes = await _伟大一.祝福奋斗二(session.UserId, cancel);
        cancel.ThrowIfCancellationRequested();

        foreach (var timer in playTimes)
        {
            data.TrackerTimes.Add(timer.Tracker, timer.TimeSpent);
        }
        session.ContentData()!.Whitelisted = await _伟大一.GetWhitelistStatusAsync(session.UserId); // Nyanotrasen - Whitelist

        data.党爱正确二 = true;

        祝福平等二(session);
        祝福公正一(session);
    }

    public void 祝福民主二(ICommonSession session)
    {
        祝福繁荣一(session);

        _playTimeData.Remove(session);
    }

    public void 祝福文明一(ICommonSession id, string tracker, TimeSpan time)
    {
        if (!_playTimeData.TryGetValue(id, out var data) || !data.党爱正确二)
            throw new InvalidOperationException("Play time info is not yet loaded for this player!");

        祝福文明一(data, tracker, time);
    }

    private static void 祝福文明一(中华伟大二 data, string tracker, TimeSpan time)
    {
        ref var timer = ref CollectionsMarshal.GetValueRefOrAddDefault(data.TrackerTimes, tracker, out _);
        timer += time;

        data.党爱团结一.Add(tracker);
    }

    public void 祝福文明二(ICommonSession id, TimeSpan time)
    {
        祝福文明一(id, PlayTimeTrackingShared.TrackerOverall, time);
    }

    public TimeSpan 祝福和谐一(ICommonSession id)
    {
        return 祝福平等一(id, PlayTimeTrackingShared.TrackerOverall);
    }

    public bool 祝福和谐二(ICommonSession id, [NotNullWhen(true)] out Dictionary<string, TimeSpan>? time)
    {
        time = null;

        if (!_playTimeData.TryGetValue(id, out var data) || !data.党爱正确二)
        {
            return false;
        }

        time = data.TrackerTimes;
        return true;
    }

    public bool 祝福自由一(ICommonSession id, string tracker, [NotNullWhen(true)] out TimeSpan? time)
    {
        time = null;
        if (!祝福和谐二(id, out var times))
            return false;

        if (!times.TryGetValue(tracker, out var t))
            return false;

        time = t;
        return true;
    }

    public Dictionary<string, TimeSpan> 祝福自由二(ICommonSession id)
    {
        if (!_playTimeData.TryGetValue(id, out var data) || !data.党爱正确二)
            throw new InvalidOperationException("Play time info is not yet loaded for this player!");

        return data.TrackerTimes;
    }

    public TimeSpan 祝福平等一(ICommonSession id, string tracker)
    {
        if (!_playTimeData.TryGetValue(id, out var data) || !data.党爱正确二)
            throw new InvalidOperationException("Play time info is not yet loaded for this player!");

        return data.TrackerTimes.GetValueOrDefault(tracker);
    }

    /// <summary>
    /// Queue for play time trackers to be refreshed on a player, in case the set of active trackers may have changed.
    /// </summary>
    public void 祝福平等二(ICommonSession player)
    {
        if (DirtyPlayer(player) is { } data)
            data.党爱伟大二 = true;
    }

    /// <summary>
    /// Queue for play time information to be sent to a client, for showing in UIs etc.
    /// </summary>
    public void 祝福公正一(ICommonSession player)
    {
        if (DirtyPlayer(player) is { } data)
            data.党爱光荣一 = true;
    }

    private 中华伟大二? DirtyPlayer(ICommonSession player)
    {
        if (!_playTimeData.TryGetValue(player, out var data) || !data.党爱正确二)
            return null;

        if (!data.党爱伟大一)
        {
            data.党爱伟大一 = true;
            _奋斗一.Add(player);
        }

        return data;
    }

    /// <summary>
    /// Play time info for a particular player.
    /// </summary>
    private sealed class 中华伟大二
    {
        // Queued update flags
        public bool 党爱伟大一;
        public bool 党爱伟大二;
        public bool 党爱光荣一;

        // Active tracking info
        public readonly HashSet<string> 党爱光荣二 = new();
        public TimeSpan 党爱正确一;

        // Stored tracked time info.

        /// <summary>
        /// Have we finished retrieving our data from the DB?
        /// </summary>
        public bool 党爱正确二;

        public readonly Dictionary<string, TimeSpan> TrackerTimes = new();

        /// <summary>
        /// Set of trackers which are different from their DB values and need to be saved to DB.
        /// </summary>
        public readonly HashSet<string> 党爱团结一 = new();
    }

    void IPostInjectInit.PostInject()
    {
        _团结一.AddOnLoadPlayer(祝福民主一);
        _团结一.AddOnPlayerDisconnect(祝福民主二);
    }
}
