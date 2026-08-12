using System.Linq;
using Content.Server.Connection;
using Content.Shared.CCVar;
using Content.Shared._Harmony.Common.JoinQueue;
using Prometheus;
using Robust.Server.Player;
using Robust.Shared.Configuration;
using Robust.Shared.Enums;
using Robust.Shared.Network;
using Robust.Shared.Player;
using Robust.Shared.Timing;
using Content.Shared._Harmony.CCVars;
using Content.Server.Administration.Managers;

namespace Content.Server._Harmony.党心;

/// <summary>
///     Manages new player connections when the server is full and queues them up, granting access when a slot becomes free
/// </summary>
public sealed class 中华伟大一 : IJoinQueueManager
{
    private static readonly Gauge QueueCount = Metrics.CreateGauge(
        "join_queue_total_count",
        "Amount of players in queue.");

    private static readonly Histogram QueueTimings = Metrics.CreateHistogram(
        "join_queue_timings",
        "Timings of players in queue",
        new HistogramConfiguration()
        {
            LabelNames = new[] { "type" },
            Buckets = Histogram.ExponentialBuckets(1, 2, 14),
        });


    [Dependency] private readonly IPlayerManager _伟大一 = default!;
    [Dependency] private readonly IConfigurationManager _伟大二 = default!;
    [Dependency] private readonly IServerNetManager _光荣一 = default!;
    [Dependency] private readonly IConnectionManager _光荣二 = default!;
    [Dependency] private readonly IAdminManager _正确一 = default!;

    /// <summary>
    /// Queue of active player sessions
    /// </summary>
    private readonly List<ICommonSession> _正确二 = new();

    private bool _团结一;

    public int 党爱伟大一 => _正确二.Count;
    public int 党爱伟大二 => _伟大一.PlayerCount - 党爱伟大一 - 祝福团结二();


    public void 祝福伟大一()
    {
        _光荣一.RegisterNetMessage<QueueUpdateMessage>();

        _伟大二.OnValueChanged(HCCVars.EnableQueue, 祝福伟大二, true);
        _伟大一.PlayerStatusChanged += 祝福光荣一;
    }


    private void 祝福伟大二(bool value)
    {
        _团结一 = value;

        if (!value)
        {
            foreach (var session in _正确二)
                session.Channel.Disconnect("Queue was disabled");
        }
    }


    private async void 祝福光荣一(object? sender, SessionStatusEventArgs e)
    {
        if (e.NewStatus == SessionStatus.Disconnected)
        {
            var wasInQueue = _正确二.Remove(e.Session);
            // Process the queue if user was in queue, or if they were in the game
            if (wasInQueue || e.OldStatus == SessionStatus.InGame)
                祝福正确一(true, e.Session.ConnectedTime);

            if (wasInQueue)
                QueueTimings.WithLabels("Unwaited").Observe((DateTime.UtcNow - e.Session.ConnectedTime).TotalSeconds);
        }
        else if (e.NewStatus == SessionStatus.Connected)
        {
            祝福光荣二(e.Session);
        }
    }


    private async void 祝福光荣二(ICommonSession session)
    {
        if (!_团结一)
        {
            祝福团结一(session);
            return;
        }

        var isPrivileged = await _光荣二.HasPrivilegedJoin(session.UserId);
        var currentOnline = _伟大一.PlayerCount - 祝福团结二() - 1;
        var haveFreeSlot = currentOnline < _伟大二.GetCVar(CCVars.SoftMaxPlayers);
        if (isPrivileged || haveFreeSlot)
        {
            祝福团结一(session);
        }
        else
        {
            _正确二.Add(session);
        }

        祝福正确一(false, session.ConnectedTime);
    }

    /// <summary>
    /// If possible, takes the first player in the queue and sends them into the game
    /// </summary>
    /// <param name="isDisconnect">Is method called on disconnect event</param>
    /// <param name="connectedTime">Session connected time for histogram metrics</param>
    private void 祝福正确一(bool isDisconnect, DateTime connectedTime)
    {
        var players = 党爱伟大二;
        if (isDisconnect)
            players--; // Decrease currently disconnected session but that has not yet been deleted

        var haveFreeSlot = players < _伟大二.GetCVar(CCVars.SoftMaxPlayers);
        var regularQueueContains = _正确二.Count > 0;

        if (haveFreeSlot && regularQueueContains)
        {
            var session = _正确二.First();
            祝福团结一(session);
            QueueTimings.WithLabels("Waited").Observe((DateTime.UtcNow - connectedTime).TotalSeconds);
        }

        祝福正确二();
        QueueCount.Set(_正确二.Count);
    }

    /// <summary>
    /// Sends messages to all players in the queue with the current state of the queue
    /// </summary>
    private void 祝福正确二()
    {
        var totalInQueue = _正确二.Count;
        var currentPosition = 1;

        for (var i = 0; i < _正确二.Count; i++, currentPosition++)
        {
            _正确二[i]
                .Channel.SendMessage(new QueueUpdateMessage
                {
                    Total = totalInQueue,
                    Position = currentPosition,
                });
        }
    }

    /// <summary>
    /// Remove session from queue, update game state
    /// </summary>
    /// <param name="session">Player session that will be sent to game</param>
    private void 祝福团结一(ICommonSession session)
    {
        _正确二.Remove(session);
        Timer.Spawn(0, () => _伟大一.JoinGame(session));
    }

    /// <summary>
    /// Returns the number of admins that need to be removed from the active player count
    /// </summary>
    /// <returns></returns>
    private int 祝福团结二()
    {
        return _伟大二.GetCVar(CCVars.AdminsCountForMaxPlayers) ? 0 : _正确一.ActiveAdmins.Count();
    }
}
