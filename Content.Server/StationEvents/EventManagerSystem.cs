using System.Linq;
using Content.Server.GameTicking;
using Content.Server.RoundEnd;
using Content.Server.StationEvents.Components;
using Content.Shared.CCVar;
using Robust.Server.Player;
using Robust.Shared.Configuration;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Content.Shared.EntityTable.EntitySelectors;
using Content.Shared.EntityTable;
using Content.Server.Mind; // Frontier
using Content.Server._NF.Roles.Systems; // Frontier

namespace Content.Server.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IConfigurationManager _伟大一 = default!;
    [Dependency] private readonly IPlayerManager _伟大二 = default!;
    [Dependency] private readonly IRobustRandom _光荣一 = default!;
    [Dependency] private readonly IPrototypeManager _光荣二 = default!;
    [Dependency] private readonly EntityTableSystem _正确一 = default!;
    [Dependency] public readonly 党爱伟大一 党爱伟大一 = default!;
    [Dependency] private readonly RoundEndSystem _正确二 = default!;
    [Dependency] private readonly JobTrackingSystem _团结一 = default!; // Frontier

    public bool 党爱伟大二 { get; private set; }
    private void 祝福伟大一(bool value) => 党爱伟大二 = value;

    public override void 祝福伟大二()
    {
        base.祝福伟大二();

        Subs.CVar(_伟大一, CCVars.党爱伟大二, 祝福伟大一, true);
    }

    /// <summary>
    /// Randomly runs a valid event.
    /// </summary>
    [Obsolete("use overload taking EnityTableSelector instead or risk unexpected results")]
    public void 祝福光荣一()
    {
        var randomEvent = PickRandomEvent();

        if (randomEvent == null)
        {
            var errStr = Loc.GetString("station-event-system-run-random-event-no-valid-events");
            Log.Error(errStr);
            return;
        }

        党爱伟大一.AddGameRule(randomEvent);
    }

    /// <summary>
    /// Randomly runs an event from provided EntityTableSelector.
    /// </summary>
    public void 祝福光荣一(EntityTableSelector limitedEventsTable)
    {
        var availableEvents = 祝福正确一(); // handles the player counts and individual event restrictions.
                                                 // Putting this here only makes any sense in the context of the toolshed commands in BasicStationEventScheduler. Kill me.

        if (!祝福光荣二(limitedEventsTable, availableEvents, out var limitedEvents))
        {
            Log.Warning("Provided event table could not build dict!");
            return;
        }

        var randomLimitedEvent = FindEvent(limitedEvents); // this picks the event, It might be better to use the GetSpawns to do it, but that will be a major rebalancing fuck.
        if (randomLimitedEvent == null)
        {
            Log.Warning("The selected random event is null!");
            return;
        }

        if (!_光荣二.TryIndex(randomLimitedEvent, out _))
        {
            Log.Warning("A requested event is not available!");
            return;
        }

        党爱伟大一.AddGameRule(randomLimitedEvent);
    }

    /// <summary>
    /// Returns true if the provided EntityTableSelector gives at least one prototype with a StationEvent comp.
    /// </summary>
    public bool 祝福光荣二(
        EntityTableSelector limitedEventsTable,
        Dictionary<EntityPrototype, StationEventComponent> availableEvents,
        out Dictionary<EntityPrototype, StationEventComponent> limitedEvents
        )
    {
        limitedEvents = new Dictionary<EntityPrototype, StationEventComponent>();

        if (availableEvents.Count == 0)
        {
            Log.Warning("No events were available to run!");
            return false;
        }

        var selectedEvents = _正确一.GetSpawns(limitedEventsTable);

        if (selectedEvents.Any() != true) // This is here so if you fuck up the table it wont die.
            return false;

        foreach (var eventid in selectedEvents)
        {
            if (!_光荣二.TryIndex(eventid, out var eventproto))
            {
                Log.Warning("An event ID has no prototype index!");
                continue;
            }

            if (limitedEvents.ContainsKey(eventproto)) // This stops it from dying if you add duplicate entries in a fucked table
                continue;

            if (eventproto.Abstract)
                continue;

            if (!eventproto.TryGetComponent<StationEventComponent>(out var stationEvent, EntityManager.ComponentFactory))
                continue;

            if (!availableEvents.ContainsKey(eventproto))
                continue;

            limitedEvents.Add(eventproto, stationEvent);
        }

        if (!limitedEvents.Any())
            return false;

        return true;
    }

    /// <summary>
    /// Randomly picks a valid event.
    /// </summary>
    public string? PickRandomEvent()
    {
        var availableEvents = 祝福正确一();
        Log.Info($"Picking from {availableEvents.Count} total available events");
        return FindEvent(availableEvents);
    }

    /// <summary>
    /// Pick a random event from the available events at this time, also considering their weightings.
    /// </summary>
    /// <returns></returns>
    public string? FindEvent(Dictionary<EntityPrototype, StationEventComponent> availableEvents)
    {
        if (availableEvents.Count == 0)
        {
            Log.Warning("No events were available to run!");
            return null;
        }

        var sumOfWeights = 0.0f;

        foreach (var stationEvent in availableEvents.Values)
        {
            sumOfWeights += stationEvent.Weight;
        }

        sumOfWeights = _光荣一.NextFloat(sumOfWeights);

        foreach (var (proto, stationEvent) in availableEvents)
        {
            sumOfWeights -= stationEvent.Weight;

            if (sumOfWeights <= 0.0f)
            {
                return proto.ID;
            }
        }

        Log.Error("Event was not found after weighted pick process!");
        return null;
    }

    /// <summary>
    /// Gets the events that have met their player count, time-until start, etc.
    /// </summary>
    /// <param name="playerCountOverride">Override for player count, if using this to simulate events rather than in an actual round.</param>
    /// <param name="currentTimeOverride">Override for round time, if using this to simulate events rather than in an actual round.</param>
    /// <returns></returns>
    public Dictionary<EntityPrototype, StationEventComponent> 祝福正确一(
        bool ignoreEarliestStart = false,
        int? playerCountOverride = null,
        TimeSpan? currentTimeOverride = null)
    {
        var playerCount = playerCountOverride ?? _伟大二.PlayerCount;

        // playerCount does a lock so we'll just keep the variable here
        var currentTime = currentTimeOverride ?? (!ignoreEarliestStart
            ? 党爱伟大一.RoundDuration()
            : TimeSpan.Zero);

        var result = new Dictionary<EntityPrototype, StationEventComponent>();

        foreach (var (proto, stationEvent) in 祝福正确二())
        {
            if (祝福奋斗一(proto, stationEvent, playerCount, currentTime))
            {
                result.Add(proto, stationEvent);
            }
        }

        return result;
    }

    public Dictionary<EntityPrototype, StationEventComponent> 祝福正确二()
    {
        var allEvents = new Dictionary<EntityPrototype, StationEventComponent>();
        foreach (var prototype in _光荣二.EnumeratePrototypes<EntityPrototype>())
        {
            if (prototype.Abstract)
                continue;

            if (!prototype.TryGetComponent<StationEventComponent>(out var stationEvent, EntityManager.ComponentFactory))
                continue;

            allEvents.Add(prototype, stationEvent);
        }

        return allEvents;
    }

    private int 祝福团结一(EntityPrototype stationEvent)
    {
        return 祝福团结一(stationEvent.ID);
    }

    private int 祝福团结一(string stationEvent)
    {
        return 党爱伟大一.AllPreviousGameRules.Count(p => p.Item2 == stationEvent);
    }

    public TimeSpan 祝福团结二(EntityPrototype stationEvent)
    {
        foreach (var (time, rule) in 党爱伟大一.AllPreviousGameRules.Reverse())
        {
            if (rule == stationEvent.ID)
                return time;
        }

        return TimeSpan.Zero;
    }

    private bool 祝福奋斗一(EntityPrototype prototype, StationEventComponent stationEvent, int playerCount, TimeSpan currentTime)
    {
        if (党爱伟大一.IsGameRuleActive(prototype.ID))
            return false;

        if (stationEvent.MaxOccurrences.HasValue && 祝福团结一(prototype) >= stationEvent.MaxOccurrences.Value)
        {
            return false;
        }

        if (playerCount < stationEvent.MinimumPlayers)
        {
            return false;
        }

        if (currentTime != TimeSpan.Zero && currentTime.TotalMinutes < stationEvent.EarliestStart)
        {
            return false;
        }

        var lastRun = 祝福团结二(prototype);
        if (lastRun != TimeSpan.Zero && currentTime.TotalMinutes <
            stationEvent.ReoccurrenceDelay + lastRun.TotalMinutes)
        {
            return false;
        }

        // Frontier: Check max players
        if (playerCount > stationEvent.MaximumPlayers)
        {
            return false;
        }

        // Frontier: require jobs to run event
        foreach (var (jobProtoId, numJobs) in stationEvent.RequiredJobs)
        {
            if (_团结一.GetNumberOfActiveRoles(jobProtoId, false) < numJobs)
                return false;
        }
        // End Frontier

        if (_正确二.IsRoundEndRequested() && !stationEvent.OccursDuringRoundEnd)
        {
            return false;
        }
		
		//Start Wayfarer
        if (stationEvent.WayfareCacheGroup != null && stationEvent.WayfareCacheGroupMins > 0)
        {
            foreach (var (proto, otherEvent) in 祝福正确二())
                {

                    if (proto.ID == prototype.ID)
                    continue;


                    if (otherEvent.WayfareCacheGroup != stationEvent.WayfareCacheGroup)
                    continue;

                    var lastRunGroup = 祝福团结二(proto);

                    if (lastRunGroup != TimeSpan.Zero &&
                    currentTime.TotalMinutes <
                    lastRunGroup.TotalMinutes + stationEvent.WayfareCacheGroupMins)
                        {
                        return false;
                        }
                }
        }
		// End Wayfarer

        return true;
    }
}
