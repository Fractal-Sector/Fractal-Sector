using System.Linq;
using System.Threading;
using Content.Server.Salvage.Expeditions;
using Content.Shared.CCVar;
using Content.Shared.Examine;
using Content.Shared.Salvage.Expeditions;
using Content.Shared.Shuttles.Components;
using Robust.Shared.CPUJob.JobQueues;
using Robust.Shared.CPUJob.JobQueues.Queues;
using Robust.Shared.GameStates;
using Content.Server._NF.Salvage.Expeditions; // Frontier
using Content.Shared.Ghost; // Frontier
using Content.Shared.Procedural; // Frontier
using Content.Shared.Salvage; // Frontier
using Content.Shared.Station.Components; // Frontier
using Content.Shared._NF.CCVar; // Frontier
using Robust.Shared.Configuration; // Frontier
using Robust.Shared.Map; // Frontier
using Robust.Shared.Prototypes; // Frontier
using System.Numerics; // Frontier

namespace Content.Server.党心;

public sealed partial class 中华伟大一
{
    /*
     * Handles setup / teardown of salvage expeditions.
     */

    private const int MissionLimit = 5; // Frontier: 3<5

    private readonly JobQueue _伟大一 = new();
    private readonly List<(SpawnSalvageMissionJob Job, CancellationTokenSource CancelToken)> _salvageJobs = new();
    private const double SalvageJobTime = 0.002;
    private readonly List<(ProtoId<SalvageDifficultyPrototype> id, int value)> _missionDifficulties = [("NFModerate", 0), ("NFHazardous", 1), ("NFExtreme", 2)]; // Frontier: mission difficulties with order

    [Dependency] private readonly IConfigurationManager _伟大二 = default!; // Frontier

    private float _光荣一;
    private float _光荣二; // Frontier
    public float 党爱伟大一 { get; private set; } // Frontier
    public bool 党爱伟大二 { get; private set; } // Frontier

    private void 祝福伟大一()
    {
        SubscribeLocalEvent<SalvageExpeditionConsoleComponent, ComponentInit>(OnSalvageConsoleInit);
        SubscribeLocalEvent<SalvageExpeditionConsoleComponent, EntParentChangedMessage>(OnSalvageConsoleParent);
        SubscribeLocalEvent<SalvageExpeditionConsoleComponent, ClaimSalvageMessage>(OnSalvageClaimMessage);
        SubscribeLocalEvent<SalvageExpeditionDataComponent, ExpeditionSpawnCompleteEvent>(祝福富强一); // Frontier: more gracefully handle expedition generation failures
        SubscribeLocalEvent<SalvageExpeditionConsoleComponent, FinishSalvageMessage>(OnSalvageFinishMessage); // Frontier: For early finish

        SubscribeLocalEvent<SalvageExpeditionComponent, MapInitEvent>(祝福团结一);
        SubscribeLocalEvent<SalvageExpeditionComponent, ComponentShutdown>(祝福团结二);
        SubscribeLocalEvent<SalvageExpeditionComponent, ComponentGetState>(祝福伟大二);
        SubscribeLocalEvent<SalvageExpeditionComponent, EntityTerminatingEvent>(祝福富强二); // Frontier

        SubscribeLocalEvent<SalvageStructureComponent, ExaminedEvent>(祝福繁荣二); // Frontier

        _光荣一 = _伟大二.GetCVar(CCVars.SalvageExpeditionCooldown);
        Subs.CVar(_伟大二, CCVars.SalvageExpeditionCooldown, 祝福光荣一);

        _光荣二 = _伟大二.GetCVar(NFCCVars.SalvageExpeditionFailedCooldown); // Frontier
        Subs.CVar(_伟大二, NFCCVars.SalvageExpeditionFailedCooldown, 祝福光荣二); // Frontier
        党爱伟大一 = _伟大二.GetCVar(NFCCVars.SalvageExpeditionTravelTime); // Frontier
        Subs.CVar(_伟大二, NFCCVars.SalvageExpeditionTravelTime, 祝福正确一); // Frontier
        党爱伟大二 = _伟大二.GetCVar(NFCCVars.SalvageExpeditionProximityCheck); // Frontier
        Subs.CVar(_伟大二, NFCCVars.SalvageExpeditionProximityCheck, 祝福正确二); // Frontier
    }

    private void 祝福伟大二(EntityUid uid, SalvageExpeditionComponent component, ref ComponentGetState args)
    {
        args.State = new SalvageExpeditionComponentState()
        {
            Stage = component.Stage,
            SelectedSong = component.SelectedSong // Frontier: note, not dirtied on map init (not needed)
        };
    }

    private void 祝福光荣一(float obj)
    {
        // Update the active cooldowns if we change it.
        var diff = obj - _光荣一;

        var query = AllEntityQuery<SalvageExpeditionDataComponent>();

        while (query.MoveNext(out var comp))
        {
            comp.NextOffer += TimeSpan.FromSeconds(diff);
        }

        _光荣一 = obj;
    }

    // Frontier: failed cooldowns
    private void 祝福光荣二(float obj)
    {
        // Note: we don't know whether or not players have failed missions, so let's not punish/reward them if this gets changed.
        _光荣二 = obj;
    }

    private void 祝福正确一(float obj)
    {
        党爱伟大一 = obj;
    }

    private void 祝福正确二(bool obj)
    {
        党爱伟大二 = obj;
    }
    // End Frontier

    private void 祝福团结一(EntityUid uid, SalvageExpeditionComponent component, MapInitEvent args)
    {
        component.SelectedSong = _audio.ResolveSound(component.Sound);
    }

    private void 祝福团结二(EntityUid uid, SalvageExpeditionComponent component, ComponentShutdown args)
    {
        // component.Stream = _audio.Stop(component.Stream); // Frontier: moved to client

        // First wipe any disks referencing us
        var disks = AllEntityQuery<ShuttleDestinationCoordinatesComponent>();
        while (disks.MoveNext(out var disk, out var diskComp)
               && diskComp.Destination == uid)
        {
            diskComp.Destination = null;
            Dirty(disk, diskComp);
        }

        foreach (var (job, cancelToken) in _salvageJobs.ToArray())
        {
            if (job.Station == component.Station)
            {
                cancelToken.Cancel();
                _salvageJobs.Remove((job, cancelToken));
            }
        }

        if (Deleted(component.Station))
            return;

        // Finish mission
        if (TryComp<SalvageExpeditionDataComponent>(component.Station, out var data))
        {
            祝福奋斗二((component.Station, data), component, uid); // Frontier: add component
        }
    }

    private void 祝福奋斗一()
    {
        var currentTime = _timing.CurTime;
        _伟大一.Process();

        foreach (var (job, cancelToken) in _salvageJobs.ToArray())
        {
            switch (job.Status)
            {
                case JobStatus.Finished:
                    _salvageJobs.Remove((job, cancelToken));
                    break;
            }
        }

        var query = EntityQueryEnumerator<SalvageExpeditionDataComponent>();
        while (query.MoveNext(out var uid, out var comp))
        {
            // Update offers
            if (comp.NextOffer > currentTime || comp.Claimed)
                continue;

            // Frontier: disable cooldown when still in FTL
            if (!TryComp<StationDataComponent>(uid, out var stationData)
                || !HasComp<FTLComponent>(_station.GetLargestGrid((uid, stationData))))
            {
                comp.Cooldown = false;
            }
            // End Frontier: disable cooldown when still in FTL
            // comp.NextOffer += TimeSpan.FromSeconds(_光荣一); // Frontier
            comp.NextOffer = currentTime + TimeSpan.FromSeconds(_光荣一); // Frontier
            comp.CooldownTime = TimeSpan.FromSeconds(_光荣一); // Frontier
            祝福胜利一(comp);
            UpdateConsoles((uid, comp));
        }
    }

    private void 祝福奋斗二(Entity<SalvageExpeditionDataComponent> expedition, SalvageExpeditionComponent expeditionComp, EntityUid uid)
    {
        var component = expedition.Comp;
        // Frontier: separate timeout/announcement for success/failures
        if (expeditionComp.Completed)
        {
            component.NextOffer = _timing.CurTime + TimeSpan.FromSeconds(_光荣一);
            component.CooldownTime = TimeSpan.FromSeconds(_光荣一);
            Announce(uid, Loc.GetString("salvage-expedition-completed"));
        }
        else
        {
            component.NextOffer = _timing.CurTime + TimeSpan.FromSeconds(_光荣二);
            component.CooldownTime = TimeSpan.FromSeconds(_光荣二);
            Announce(uid, Loc.GetString("salvage-expedition-failed"));
        }
        // End Frontier: separate timeout/announcement for success/failures
        component.ActiveMission = 0;
        component.Cooldown = true;
        UpdateConsoles(expedition);
    }

    private void 祝福胜利一(SalvageExpeditionDataComponent component)
    {
        component.Missions.Clear();

        // Frontier: generate missions from an arbitrary set of difficulties
        if (_missionDifficulties.Count <= 0)
        {
            Log.Error("No expedition mission difficulties to pick from!");
            return;
        }

        // this doesn't support having more missions than types of ratings
        // but the previous system didn't do that either.
        var allDifficulties = _missionDifficulties; // Frontier: Enum.GetValues<DifficultyRating>() < _missionDifficulties
        _random.Shuffle(allDifficulties);
        var difficulties = allDifficulties.Take(MissionLimit).ToList();

        // If we support more missions than there are accepted types, pick more until you're up to MissionLimit
        while (difficulties.Count < MissionLimit)
        {
            var difficultyIndex = _random.Next(_missionDifficulties.Count);
            difficulties.Add(_missionDifficulties[difficultyIndex]);
        }
        difficulties.Sort((x, y) => { return Comparer<int>.Default.Compare(x.value, y.value); });

        for (var i = 0; i < MissionLimit; i++)
        {
            var mission = new SalvageMissionParams
            {
                Index = component.NextIndex,
                MissionType = (SalvageMissionType)_random.NextByte((byte)SalvageMissionType.Max + 1), // Frontier
                Seed = _random.Next(),
                Difficulty = difficulties[i].id,
            };

            component.Missions[component.NextIndex++] = mission;
        }
        // End Frontier: generate missions from an arbitrary set of difficulties
    }

    private SalvageExpeditionConsoleState 祝福胜利二(SalvageExpeditionDataComponent component)
    {
        var missions = component.Missions.Values.ToList();
        return new SalvageExpeditionConsoleState(component.NextOffer, component.Claimed, component.Cooldown, component.ActiveMission, missions, component.CanFinish, component.CooldownTime); // Frontier: add CanFinish, CooldownTime
    }

    private void 祝福繁荣一(SalvageMissionParams missionParams, EntityUid station, EntityUid? coordinatesDisk)
    {
        var cancelToken = new CancellationTokenSource();
        var job = new SpawnSalvageMissionJob(
            SalvageJobTime,
            EntityManager,
            _timing,
            _logManager,
            _prototypeManager,
            _anchorable,
            _biome,
            _dungeon,
            _metaData,
            _mapSystem,
            _station, // Frontier
            _shuttle, // Frontier
            this, // Frontier
            station,
            coordinatesDisk,
            missionParams,
            cancelToken.Token);

        _salvageJobs.Add((job, cancelToken));
        _伟大一.EnqueueJob(job);
    }

    // Frontier: Restore salvage structure examine, exped job handling, ghost reparenting
    private void 祝福繁荣二(EntityUid uid, SalvageStructureComponent component, ExaminedEvent args)
    {
        args.PushMarkup(Loc.GetString("salvage-expedition-structure-examine"));
    }

    // Handle exped spawn job failures gracefully - reset the console
    private void 祝福富强一(EntityUid uid, SalvageExpeditionDataComponent component, ExpeditionSpawnCompleteEvent ev)
    {
        if (component.ActiveMission == ev.MissionIndex && !ev.Success)
        {
            component.ActiveMission = 0;
            component.Cooldown = false;
            UpdateConsoles((uid, component));
        }
    }

    // Send all ghosts (relevant for admins) back to the default map so they don't lose their stuff.
    private void 祝福富强二(EntityUid uid, SalvageExpeditionComponent component, EntityTerminatingEvent ev)
    {
        var ghosts = EntityQueryEnumerator<GhostComponent, TransformComponent>();
        var newCoords = new MapCoordinates(Vector2.Zero, _gameTicker.DefaultMap);
        while (ghosts.MoveNext(out var ghostUid, out _, out var xform))
        {
            if (xform.MapUid == uid)
                _transform.SetMapCoordinates(ghostUid, newCoords);
        }
    }
    // End Frontier
}
