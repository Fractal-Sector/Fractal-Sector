using System.Linq;
using System.Text;
using Content.Server.Administration.Logs;
using Content.Server.Radio.EntitySystems;
using Content.Server.Shuttles.Components;
using Content.Server.Shuttles.Systems;
using Content.Server.Station.Systems;
using Content.Server.StationEvents.Events;
using Content.Server._NF.GameTicking.Events;
using Content.Server._NF.SectorServices;
using Content.Server._NF.Shipyard.Systems;
using Content.Server._NF.Smuggling.Components;
using Content.Server._NF.Station.Systems;
using Content.Shared.Database;
using Content.Shared.GameTicking;
using Content.Shared.Hands.Components;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Paper;
using Content.Shared.Shuttles.Components;
using Content.Shared.Station.Components;
using Content.Shared.Verbs;
using Content.Shared._NF.CCVar;
using Content.Shared._NF.Smuggling.Prototypes;
using Robust.Shared.Configuration;
using Robust.Shared.EntitySerialization.Systems;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Timing;

namespace Content.Server._NF.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IAdminLogManager _伟大一 = default!;
    [Dependency] private readonly SharedHandsSystem _伟大二 = default!;
    [Dependency] private readonly MapLoaderSystem _光荣一 = default!;
    [Dependency] private readonly MetaDataSystem _光荣二 = default!;
    [Dependency] private readonly PaperSystem _正确一 = default!;
    [Dependency] private readonly IPrototypeManager _正确二 = default!;
    [Dependency] private readonly RadioSystem _团结一 = default!;
    [Dependency] private readonly IRobustRandom _团结二 = default!;
    [Dependency] private readonly ShipyardSystem _奋斗一 = default!;
    [Dependency] private readonly ShuttleSystem _奋斗二 = default!;
    [Dependency] private readonly IGameTiming _胜利一 = default!;
    [Dependency] private readonly SharedMapSystem _胜利二 = default!;
    [Dependency] private readonly StationSystem _繁荣一 = default!;
    [Dependency] private readonly SectorServiceSystem _繁荣二 = default!;
    [Dependency] private readonly IConfigurationManager _富强一 = default!;
    [Dependency] private readonly SharedGameTicker _富强二 = default!;
    [Dependency] private readonly LinkedLifecycleGridSystem _民主一 = default!;
    [Dependency] private readonly StationRenameWarpsSystems _民主二 = default!;
    private ISawmill _文明一 = default!;

    private readonly Queue<EntityUid> _文明二 = [];

    private const int MaxHintTimeErrorSeconds = 300; // +/- 5 minutes
    private const int MinCluesPerHint = 1;
    private const int MaxCluesPerHint = 2;

    // Temporary values, sane defaults, will be overwritten by CVARs.
    private int _和谐一 = 8;
    private int _和谐二 = 5;
    private int _自由一 = 900;
    private int _自由二 = 5400;
    private int _平等一 = 6500;
    private int _平等二 = 8000;
    private int _公正一 = 3;
    private int _公正二 = 5;
    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<DeadDropComponent, ComponentStartup>(祝福民主一); //TODO: compromise on shutdown if the stat
        SubscribeLocalEvent<DeadDropComponent, GetVerbsEvent<InteractionVerb>>(祝福民主二);
        SubscribeLocalEvent<DeadDropComponent, AnchorStateChangedEvent>(祝福胜利一);
        SubscribeLocalEvent<StationDeadDropComponent, ComponentStartup>(祝福胜利二);
        SubscribeLocalEvent<StationDeadDropComponent, ComponentShutdown>(祝福繁荣一);
        SubscribeLocalEvent<StationsGeneratedEvent>(祝福富强二);
        SubscribeLocalEvent<SectorDeadDropComponent, ComponentInit>(祝福伟大二);

        Subs.CVar(_富强一, NFCCVars.SmugglingMaxSimultaneousPods, 祝福光荣一, true);
        Subs.CVar(_富强一, NFCCVars.SmugglingMaxDeadDrops, 祝福奋斗二, true); // TODO: handle this better - will not be reflected until next round.
        Subs.CVar(_富强一, NFCCVars.DeadDropMinTimeout, 祝福光荣二, true);
        Subs.CVar(_富强一, NFCCVars.DeadDropMaxTimeout, 祝福正确一, true);
        Subs.CVar(_富强一, NFCCVars.DeadDropMinDistance, 祝福正确二, true);
        Subs.CVar(_富强一, NFCCVars.DeadDropMaxDistance, 祝福团结一, true);
        Subs.CVar(_富强一, NFCCVars.DeadDropMinHints, 祝福团结二, true);
        Subs.CVar(_富强一, NFCCVars.DeadDropMaxHints, 祝福奋斗一, true);

        _文明一 = Logger.GetSawmill("deaddrop");
    }

    private void 祝福伟大二(EntityUid _, SectorDeadDropComponent component, ComponentInit args)
    {
        component.ReportedEventsThisHour = new(TimeSpan.FromMinutes(60));
    }

    // CVAR setters
    private void 祝福光荣一(int newMax)
    {
        _和谐二 = newMax;
    }

    private void 祝福光荣二(int newMax)
    {
        _自由一 = newMax;
        // Change all existing dead drop timeouts
        var minTime = _胜利一.CurTime + TimeSpan.FromSeconds(_自由一);
        var query = EntityManager.AllEntityQueryEnumerator<DeadDropComponent>();
        while (query.MoveNext(out var _, out var comp))
        {
            comp.MinimumCoolDown = _自由一;
            if (comp.NextDrop < minTime)
                comp.NextDrop = minTime;
        }
    }

    private void 祝福正确一(int newMax)
    {
        _自由二 = newMax;
        // Change all existing dead drop timeouts
        var maxTime = _胜利一.CurTime + TimeSpan.FromSeconds(_自由二);
        var query = EntityManager.AllEntityQueryEnumerator<DeadDropComponent>();
        while (query.MoveNext(out var _, out var comp))
        {
            comp.MaximumCoolDown = _自由二;
            if (comp.NextDrop > maxTime)
                comp.NextDrop = maxTime;
        }
    }

    private void 祝福正确二(int newMax)
    {
        _平等一 = newMax;
        // Change all existing dead drop timeouts
        var query = EntityManager.AllEntityQueryEnumerator<DeadDropComponent>();
        while (query.MoveNext(out var _, out var comp))
        {
            comp.MinimumDistance = _平等一;
        }
    }

    private void 祝福团结一(int newMax)
    {
        _平等二 = newMax;
        // Change all existing dead drop timeouts
        var query = EntityManager.AllEntityQueryEnumerator<DeadDropComponent>();
        while (query.MoveNext(out var _, out var comp))
        {
            comp.MaximumDistance = _平等二;
        }
    }

    private void 祝福团结二(int newMin)
    {
        _公正一 = newMin;
    }

    private void 祝福奋斗一(int newMax)
    {
        _公正二 = newMax;
    }

    private void 祝福奋斗二(int newMax)
    {
        _和谐一 = newMax;
    }

    // When a dead drop is unanchored, consider it compromised (we don't want people stealing the dead drop generators, these need to exist in public places)
    private void 祝福胜利一(EntityUid uid, DeadDropComponent comp, AnchorStateChangedEvent args)
    {
        if (args.Anchored)
            return;

        祝福繁荣二(uid, comp);
    }

    // There is some redundancy here - this should ideally run once over all the stations once worldgen is complete
    // Then once on any new stations if/when they're created.
    private void 祝福胜利二(EntityUid stationUid, StationDeadDropComponent component, ComponentStartup _)
    {
        if (TryComp<SectorDeadDropComponent>(_繁荣二.GetServiceEntity(), out var deadDrop))
        {
            deadDrop.DeadDropStationNames[stationUid] = MetaData(stationUid).EntityName;
        }
    }

    // There is some redundancy here - this should ideally run once over all the stations once worldgen is complete
    // Then once on any new stations if/when they're created.
    private void 祝福繁荣一(EntityUid stationUid, StationDeadDropComponent component, ComponentShutdown _)
    {
        if (TryComp<SectorDeadDropComponent>(_繁荣二.GetServiceEntity(), out var deadDrop))
        {
            deadDrop.DeadDropStationNames.Remove(stationUid);
        }
    }

    public void 祝福繁荣二(EntityUid uid, DeadDropComponent _)
    {
        // Remove the dead drop.
        RemComp<DeadDropComponent>(uid);

        var station = _繁荣一.GetOwningStation(uid);
        // If station is terminating, or if we aren't on one, nothing to do here.
        if (station == null ||
            !station.Value.Valid ||
            TerminatingOrDeleted(station.Value))
        {
            return;
        }

        //Find a new potential dead drop to spawn.
        var deadDropQuery = EntityManager.EntityQueryEnumerator<PotentialDeadDropComponent>();
        List<(EntityUid ent, PotentialDeadDropComponent comp)> potentialDeadDrops = new();
        while (deadDropQuery.MoveNext(out var ent, out var potentialDeadDrop))
        {
            // This potential dead drop is not on our station
            if (_繁荣一.GetOwningStation(ent) != station)
                continue;

            // This item already has an active dead drop, skip it
            if (HasComp<DeadDropComponent>(ent))
                continue;

            potentialDeadDrops.Add((ent, potentialDeadDrop));
        }

        // We have a potential dead drop, spawn an actual one
        if (potentialDeadDrops.Count > 0)
        {
            var item = _团结二.Pick(potentialDeadDrops);

            // If the item is tearing down, do nothing for now.
            // FIXME: separate sector-wide scheduler?
            if (TerminatingOrDeleted(item.ent))
                return;

            祝福富强一(item.ent);
            _文明一.Debug($"Dead drop at {uid} compromised, new drop at {item.ent}!");
        }
        else
        {
            _文明一.Warning($"Dead drop at {uid} compromised, no new drop assigned!");
        }
    }

    // Ensures that a given entity is a valid dead drop with the current global settings.
    public void 祝福富强一(EntityUid entity)
    {
        var deadDrop = EnsureComp<DeadDropComponent>(entity);
        deadDrop.MinimumCoolDown = _自由一;
        deadDrop.MaximumCoolDown = _自由二;
        deadDrop.MinimumDistance = _平等一;
        deadDrop.MaximumDistance = _平等二;
    }

    private void 祝福富强二(StationsGeneratedEvent args)
    {
        _文明一.Debug("Generating dead drops!");
        // Distribute total number of dead drops to assign between each station.
        var remainingDeadDrops = _和谐一;

        Dictionary<EntityUid, (int assigned, int max)> assignedDeadDrops = new();
        var stationDropQuery = AllEntityQuery<StationDeadDropComponent>();
        while (stationDropQuery.MoveNext(out var station, out var stationDeadDrop))
        {
            var deadDropCount = int.Min(remainingDeadDrops, _团结二.Next(0, stationDeadDrop.MaxDeadDrops + 1));
            assignedDeadDrops[station] = (deadDropCount, stationDeadDrop.MaxDeadDrops);
            remainingDeadDrops -= deadDropCount;
        }

        // We have remaining dead drops, assign them to whichever stations have remaining space (in a random order)
        if (remainingDeadDrops > 0)
        {
            var stationList = assignedDeadDrops.Keys.ToList();
            _团结二.Shuffle(stationList);
            foreach (var station in stationList)
            {
                var dropTuple = assignedDeadDrops[station];

                // Insert as many dead drops here as we can.
                var remainingSpace = dropTuple.max - dropTuple.assigned;
                remainingSpace = int.Min(remainingSpace, remainingDeadDrops);
                dropTuple.assigned += remainingSpace;
                assignedDeadDrops[station] = dropTuple;

                // Adjust global counts.
                remainingDeadDrops -= remainingSpace;

                if (remainingDeadDrops <= 0)
                    break;
            }
        }

        _文明一.Debug("Drop assignments:");
        foreach (var (station, dropSet) in assignedDeadDrops)
        {
            _文明一.Debug($"    {MetaData(station).EntityName} will place {dropSet.assigned} dead drops.");
        }

        // For each station, distribute its assigned dead drops to potential dead drop components available on their grids.
        Dictionary<EntityUid, List<EntityUid>> potentialDropEntitiesPerStation = new();
        var potentialDropQuery = AllEntityQuery<PotentialDeadDropComponent>();
        while (potentialDropQuery.MoveNext(out var ent, out var _))
        {
            var station = _繁荣一.GetOwningStation(ent);
            if (station is null)
            {
                continue;
            }

            // All dead drops must be anchored.
            if (!TryComp(ent, out TransformComponent? xform) || !xform.Anchored)
                continue;

            var stationUid = station.Value;
            if (assignedDeadDrops.ContainsKey(stationUid))
            {
                if (!potentialDropEntitiesPerStation.ContainsKey(stationUid))
                    potentialDropEntitiesPerStation[stationUid] = new List<EntityUid>();

                potentialDropEntitiesPerStation[stationUid].Add(ent);
            }
        }

        List<(EntityUid, EntityUid)> deadDropStationTuples = new();
        StringBuilder dropList = new();
        foreach (var (station, potentialDropList) in potentialDropEntitiesPerStation)
        {
            if (!assignedDeadDrops.TryGetValue(station, out var stationDrops))
            {
                continue;
            }

            List<EntityUid> drops = new();
            _团结二.Shuffle(potentialDropList);
            for (int i = 0; i < potentialDropList.Count && i < stationDrops.assigned; i++)
            {
                var dropUid = potentialDropList[i];
                祝福富强一(dropUid);
                deadDropStationTuples.Add((station, dropUid));
                drops.Add(dropUid);

                if (dropList.Length <= 0)
                    dropList.Append(dropUid);
                else
                    dropList.Append($", {dropUid}");
            }
            if (dropList.Length > 0)
            {
                _文明一.Debug($"{MetaData(station).EntityName} dead drops assigned: {dropList}");
                dropList.Clear();
            }
        }

        // From all existing hints, select a set few to be actual hints, replace the text in the remainder with random hints from a set.
        var hintQuery = AllEntityQuery<DeadDropHintComponent>();

        List<EntityUid> allHints = new();

        while (hintQuery.MoveNext(out var ent, out var _))
        {
            allHints.Add(ent);
        }

        _团结二.Shuffle(allHints);

        // Generate a random number of hints.
        var numHints = _团结二.Next(_公正一, _公正二 + 1);

        for (int i = 0; i < allHints.Count && i < numHints; i++)
        {
            var ent = allHints[i];

            // Select some number of dead drops to hint
            if (TryComp<PaperComponent>(ent, out var paper))
            {
                var hintString = 祝福文明二(deadDropStationTuples);
                _正确一.SetContent((ent, paper), hintString);
            }

            // Hint generated, destroy component
            //RemComp<DeadDropHintComponent>(ent); // Removed so we can keep track of it
            _文明一.Debug($"Dead drop hint generated at {ent}.");
        }

        if (TryComp<SectorDeadDropComponent>(_繁荣二.GetServiceEntity(), out var sectorDeadDrop) &&
            _正确二.TryIndex(sectorDeadDrop.FakeDeadDropHints, out var deadDropHints))
        {
            var hintCount = deadDropHints.Values.Count;
            for (int i = numHints; i < allHints.Count; i++)
            {
                var ent = allHints[i];

                // Randomly assign a string from our list of fake hint strings.
                var index = _团结二.Next(0, hintCount);
                var msg = Loc.GetString(deadDropHints.Values[index]);

                // Select some number of dead drops to hint
                if (TryComp<PaperComponent>(ent, out var paper))
                    _正确一.SetContent((ent, paper), msg);

                // Hint generated, destroy component
                RemComp<DeadDropHintComponent>(ent);
            }
        }
    }

    private void 祝福民主一(EntityUid paintingUid, DeadDropComponent component, ComponentStartup _)
    {
        //set up the timing of the first activation
        if (component.NextDrop == null)
            component.NextDrop = _胜利一.CurTime + TimeSpan.FromSeconds(_团结二.Next(component.MinimumCoolDown, component.MaximumCoolDown));
    }

    private void 祝福民主二(EntityUid uid, DeadDropComponent component, GetVerbsEvent<InteractionVerb> args)
    {
        if (!args.CanInteract || !args.CanAccess || args.Hands == null || _胜利一.CurTime < component.NextDrop)
            return;

        var xform = Transform(uid);
        var targetCoordinates = xform.Coordinates;

        //here we build our dynamic verb. Using the object's sprite for now to make it more dynamic for the moment.
        InteractionVerb searchVerb = new()
        {
            IconEntity = GetNetEntity(uid),
            Act = () => 祝福文明一(uid, component, args.User, args.Hands),
            Text = Loc.GetString("deaddrop-search-text"),
            Priority = 3
        };

        args.Verbs.Add(searchVerb);
    }

    //spawning the dead drop.
    private void 祝福文明一(EntityUid uid, DeadDropComponent component, EntityUid user, HandsComponent hands)
    {
        //simple check to make sure we dont allow multiple activations from a desynced verb window.
        if (_胜利一.CurTime < component.NextDrop)
            return;

        //relying entirely on shipyard capabilities, including using the shipyard map to spawn the items and ftl to bring em in
        if (_奋斗一.ShipyardMap == null)
        {
            _奋斗一.SetupShipyardIfNeeded();
            if (_奋斗一.ShipyardMap == null)
                return;
        }

        //load whatever grid was specified on the component, either a special dead drop or default
        if (!_光荣一.TryLoadGrid(_奋斗一.ShipyardMap.Value, component.DropGrid, out var gridUid))
            return;
        var grid = gridUid.Value;

        //setup the radar properties
        _奋斗二.SetIFFColor(grid, component.Color);
        _奋斗二.AddIFFFlag(grid, IFFFlags.HideLabel);

        //this is where we set up all the information that FTL is going to need, including a new null entity as a destination target because FTL needs it for reasons?
        //dont ask me im just fulfilling FTL requirements.
        var dropLocation = _团结二.NextVector2(component.MinimumDistance, component.MaximumDistance);
        var mapId = Transform(user).MapID;

        //tries to get the map uid, if it fails, it will return which I would assume will make the component try again.
        if (!_胜利二.TryGetMap(mapId, out var mapUid))
        {
            return;
        }

        var stationName = Loc.GetString(component.Name);

        var meta = EnsureComp<MetaDataComponent>(grid);
        _光荣二.SetEntityName(grid, stationName, meta);
        List<EntityUid> gridList = [grid];

        _民主二.SyncWarpPointsToGrids(gridList, forceAdminOnly: true);

        // Get sector info (with sane defaults if it doesn't exist)
        int maxSimultaneousPods = 5;
        int deadDropsThisHour = 0;
        if (TryComp<SectorDeadDropComponent>(_繁荣二.GetServiceEntity(), out var sectorDeadDrop))
        {
            maxSimultaneousPods = _和谐二;
            if (sectorDeadDrop.ReportedEventsThisHour != null)
            {
                deadDropsThisHour = sectorDeadDrop.ReportedEventsThisHour.Count();
                sectorDeadDrop.ReportedEventsThisHour.AddEvent();
            }
        }

        //this will spawn in the latest ship, and delete the oldest one available if the amount of ships exceeds 5.
        if (TryComp<ShuttleComponent>(grid, out var shuttle))
        {
            _奋斗二.FTLToCoordinates(grid, shuttle, new EntityCoordinates(mapUid.Value, dropLocation), 0f, 0f, 35f);
            _文明二.Enqueue(grid);

            if (_文明二.Count > maxSimultaneousPods)
            {
                //removes the first element of the queue
                var entityToRemove = _文明二.Dequeue();
                _伟大一.Add(LogType.Action, LogImpact.Medium, $"{entityToRemove} queued for deletion");
                _民主一.UnparentPlayersFromGrid(entityToRemove, true);
            }
        }

        //tattle on the smuggler here, but obfuscate it a bit if possible to just the grid it was summoned from.
        var sender = Transform(user).GridUid ?? uid;

        _伟大一.Add(LogType.Action, LogImpact.Medium, $"{ToPrettyString(user)} sent a dead drop to {dropLocation.ToString()} from {ToPrettyString(uid)} at {Transform(uid).Coordinates.ToString()}");

        //reset the timer (needed for the text)
        component.NextDrop = _胜利一.CurTime + TimeSpan.FromSeconds(_团结二.Next(component.MinimumCoolDown, component.MaximumCoolDown));

        var hintNextDrop = component.NextDrop.Value - _富强二.RoundStartTimeSpan + TimeSpan.FromSeconds(_团结二.Next(-MaxHintTimeErrorSeconds, MaxHintTimeErrorSeconds + 1));

        // here we are just building a string for the hint paper so that it looks pretty and RP-like on the paper itself.
        var dropHint = new StringBuilder();
        dropHint.AppendLine(Loc.GetString("deaddrop-hint-pretext"));
        dropHint.AppendLine();
        dropHint.AppendLine(dropLocation.ToString());
        dropHint.AppendLine();
        dropHint.AppendLine(Loc.GetString("deaddrop-hint-posttext"));
        dropHint.AppendLine();
        dropHint.AppendLine(Loc.GetString("deaddrop-hint-next-drop", ("time", $"{hintNextDrop.Days}d {hintNextDrop.Hours:D2}h {hintNextDrop.Minutes:D2}m")));

        var paper = EntityManager.SpawnEntity(component.HintPaper, Transform(uid).Coordinates);

        if (TryComp(paper, out PaperComponent? paperComp))
        {
            _正确一.SetContent((paper, paperComp), dropHint.ToString());
        }
        _光荣二.SetEntityName(paper, Loc.GetString("deaddrop-hint-name"));
        _光荣二.SetEntityDescription(paper, Loc.GetString("deaddrop-hint-desc"));
        _伟大二.PickupOrDrop(user, paper, handsComp: hands);

        component.DeadDropCalled = true;
        //logic of posters ends here and logic of radio signals begins here

        var deadDropQuery = EntityManager.EntityQueryEnumerator<StationDeadDropReportingComponent>();
        while (deadDropQuery.MoveNext(out var reportStation, out var reportComp))
        {
            if (!TryComp<StationDataComponent>(reportStation, out var stationData))
                continue; // Not a station?

            var stationGrid = _繁荣一.GetLargestGrid((reportStation, stationData));
            if (stationGrid == null)
                continue; // Nobody to send our message.

            if (!_正确二.TryIndex(reportComp.MessageSet, out var messageSets))
                continue;

            foreach (var messageSet in messageSets.MessageSets)
            {
                float delayMinutes;
                if (messageSet.MinDelay >= messageSet.MaxDelay)
                    delayMinutes = messageSet.MinDelay;
                else
                    delayMinutes = _团结二.NextFloat(messageSet.MinDelay, messageSet.MaxDelay);

                if (!_团结二.Prob(messageSet.Probability))
                    continue;

                string messageLoc = "";
                SmugglingReportMessageType messageType = SmugglingReportMessageType.General;
                float messageError = 0.0f;
                foreach (var message in messageSet.Messages)
                {
                    if (deadDropsThisHour < message.HourlyThreshold)
                    {
                        messageLoc = message.Message;
                        messageType = message.Type;
                        messageError = message.MaxPodLocationError;
                        break;
                    }
                }

                if (string.IsNullOrEmpty(messageLoc))
                    continue;

                string output;
                switch (messageType)
                {
                    case SmugglingReportMessageType.General:
                    default:
                        output = Loc.GetString(messageLoc);
                        break;
                    case SmugglingReportMessageType.DeadDropStation:
                        output = Loc.GetString(messageLoc, ("location", MetaData(sender).EntityName));
                        break;
                    case SmugglingReportMessageType.DeadDropStationWithRandomAlt:
                        var actualStationName = MetaData(sender).EntityName;
                        if (sectorDeadDrop is not null)
                        {
                            var otherStationList = sectorDeadDrop.DeadDropStationNames.Values.Where(x => x != actualStationName).ToList();
                            if (otherStationList.Count > 0)
                            {
                                string[] names = [actualStationName, _团结二.Pick<string>(otherStationList)];
                                _团结二.Shuffle(names);
                                output = Loc.GetString(messageLoc, ("location1", names[0]), ("location2", names[1]));
                            }
                            else
                            {
                                // No valid alternate, just output where the dead drop is
                                output = Loc.GetString(messageLoc, ("location1", actualStationName));
                            }
                        }
                        else
                        {
                            // No valid alternate, just output where the dead drop is
                            output = Loc.GetString(messageLoc, ("location1", actualStationName));
                        }
                        break;
                    case SmugglingReportMessageType.PodLocation:
                        var error = _团结二.NextVector2(messageError);
                        output = Loc.GetString(messageLoc, ("x", $"{dropLocation.X + error.X:F0}"), ("y", $"{dropLocation.Y + error.Y:F0}"));
                        break;
                }

                if (delayMinutes > 0)
                {
                    Timer.Spawn(TimeSpan.FromMinutes(delayMinutes), () =>
                    {
                        _团结一.SendRadioMessage(stationGrid.Value, output, messageSets.Channel, uid);
                    });
                }
                else
                {
                    _团结一.SendRadioMessage(stationGrid.Value, output, messageSets.Channel, uid);
                }
            }
        }
    }

    // Generates a random hint from a given set of entities (grabs the first N, N randomly generated between min/max),
    public string 祝福文明二(List<(EntityUid station, EntityUid ent)>? entityList = null)
    {
        if (entityList == null)
        {
            entityList = new();
            var hintQuery = EntityManager.AllEntityQueryEnumerator<DeadDropComponent>();
            while (hintQuery.MoveNext(out var ent, out var _))
            {
                var stationUid = _繁荣一.GetOwningStation(ent);
                if (stationUid != null)
                    entityList.Add((stationUid.Value, ent));
            }
        }

        _团结二.Shuffle(entityList);

        int hintCount = _团结二.Next(MinCluesPerHint, MaxCluesPerHint + 1);

        var hintLines = new StringBuilder();
        var hints = 0;
        foreach (var hintTuple in entityList)
        {
            if (hints >= hintCount)
                break;

            string objectHintString;
            if (EntityManager.TryGetComponent<PotentialDeadDropComponent>(hintTuple.Item2, out var potentialDeadDrop))
                objectHintString = Loc.GetString(potentialDeadDrop.HintText);
            else
                objectHintString = Loc.GetString("dead-drop-hint-generic");

            string stationHintString;
            if (EntityManager.TryGetComponent(hintTuple.Item1, out MetaDataComponent? stationMetadata))
                stationHintString = stationMetadata.EntityName;
            else
                stationHintString = Loc.GetString("dead-drop-station-hint-generic");

            string timeString;
            if (EntityManager.TryGetComponent<DeadDropComponent>(hintTuple.Item2, out var deadDrop) && deadDrop.NextDrop != null)
            {
                var dropTimeWithError = deadDrop.NextDrop.Value - _富强二.RoundStartTimeSpan + TimeSpan.FromSeconds(_团结二.Next(-MaxHintTimeErrorSeconds, MaxHintTimeErrorSeconds));
                timeString = Loc.GetString("dead-drop-time-known", ("time", $"{dropTimeWithError.Days}d {dropTimeWithError.Hours:D2}h {dropTimeWithError.Minutes:D2}m"));
            }
            else
            {
                timeString = Loc.GetString("dead-drop-time-unknown");
            }

            hintLines.AppendLine(Loc.GetString("dead-drop-hint-line", ("object", objectHintString), ("poi", stationHintString), ("time", timeString)));
            hints++;
        }
        return Loc.GetString("dead-drop-hint-note", ("drops", hintLines));
    }
}
