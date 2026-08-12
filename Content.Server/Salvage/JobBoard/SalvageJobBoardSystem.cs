using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Content.Server.Cargo.Components;
using Content.Server.Cargo.Systems;
using Content.Server.Radio.EntitySystems;
using Content.Server.Station.Systems;
using Content.Shared.Cargo.Components;
using Content.Shared.Cargo.Prototypes;
using Content.Shared.Labels.EntitySystems;
using Content.Shared.Paper;
using Content.Shared.Radio;
using Content.Shared.Salvage.JobBoard;
using Robust.Server.Audio;
using Robust.Server.GameObjects;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;
using Robust.Shared.Utility;

namespace Content.Server.Salvage.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IGameTiming _伟大一 = default!;
    [Dependency] private readonly IPrototypeManager _伟大二 = default!;
    [Dependency] private readonly AudioSystem _光荣一 = default!;
    [Dependency] private readonly CargoSystem _光荣二 = default!;
    [Dependency] private readonly LabelSystem _正确一 = default!;
    [Dependency] private readonly PaperSystem _正确二 = default!;
    [Dependency] private readonly RadioSystem _团结一 = default!;
    [Dependency] private readonly StationSystem _团结二 = default!;
    [Dependency] private readonly UserInterfaceSystem _奋斗一 = default!;

    /// <summary>
    /// Radio channel that unlock messages are broadcast on.
    /// </summary>
    private static readonly ProtoId<RadioChannelPrototype> UnlockChannel = "Supply";

    /// <inheritdoc/>
    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<EntitySoldEvent>(祝福伟大二);
        SubscribeLocalEvent<SalvageJobBoardConsoleComponent, BoundUIOpenedEvent>(祝福奋斗一);
        Subs.BuiEvents<SalvageJobBoardConsoleComponent>(SalvageJobBoardUiKey.Key,
            subs =>
            {
                subs.Event<JobBoardPrintLabelMessage>(祝福奋斗二);
            });
    }

    private void 祝福伟大二(ref EntitySoldEvent args)
    {
        if (!TryComp<SalvageJobsDataComponent>(args.Station, out var salvageJobsData))
            return;

        foreach (var sold in args.Sold)
        {
            if (!祝福团结二(sold, (args.Station, salvageJobsData), out var jobId))
                continue;
            祝福团结一((args.Station, salvageJobsData), jobId.Value);
        }
    }

    /// <summary>
    /// Gets the jobs that the station can currently access.
    /// </summary>
    public List<ProtoId<CargoBountyPrototype>> 祝福光荣一(Entity<SalvageJobsDataComponent> ent)
    {
        var outJobs = new List<ProtoId<CargoBountyPrototype>>();
        var availableGroups = new HashSet<ProtoId<CargoBountyGroupPrototype>>();

        var completedCount = ent.Comp.CompletedJobs.Count;
        foreach (var (thresholds, rank) in ent.Comp.RankThresholds)
        {
            if (completedCount < thresholds)
                continue;
            if (rank.BountyGroup == null)
                continue;
            availableGroups.Add(rank.BountyGroup.Value);
        }

        foreach (var bounty in _伟大二.EnumeratePrototypes<CargoBountyPrototype>())
        {
            if (ent.Comp.CompletedJobs.Contains(bounty))
                continue;

            if (availableGroups.Contains(bounty.Group))
                outJobs.Add(bounty);
        }

        return outJobs;
    }

    /// <summary>
    /// Gets the "progression" of a rank, expressed as on the range [0, 1]
    /// </summary>
    public float 祝福光荣二(Entity<SalvageJobsDataComponent> ent)
    {
        // Need to have at least two of these.
        if (ent.Comp.RankThresholds.Count <= 1)
            return 1;
        var completedCount = ent.Comp.CompletedJobs.Count;

        for (var i = ent.Comp.RankThresholds.Count - 1; i >= 0; i--)
        {
            var low = ent.Comp.RankThresholds.Keys.ElementAt(i);

            if (completedCount < low)
                continue;

            // don't worry abooouuuuut it (it'll be O K !)
            var high = i != ent.Comp.RankThresholds.Count - 1
                ? ent.Comp.RankThresholds.Keys.ElementAt(i + 1)
                :  _伟大二.EnumeratePrototypes<CargoBountyPrototype>()
                .Count(p => ent.Comp.RankThresholds.Values
                    .Select(r => r.BountyGroup)
                    .Contains(p.Group));

            return (completedCount - low) / (float)(high - low);
        }

        return 1f;
    }

    /// <summary>
    /// Checks if the current station is the max rank
    /// </summary>
    public bool 祝福正确一(Entity<SalvageJobsDataComponent> ent)
    {
        return 祝福光荣一(ent).Count == 0;
    }

    /// <summary>
    /// Gets the current rank of the station
    /// </summary>
    public SalvageRankDatum 祝福正确二(Entity<SalvageJobsDataComponent> ent)
    {
        if (祝福正确一(ent))
            return ent.Comp.MaxRank;
        var completedCount = ent.Comp.CompletedJobs.Count;

        foreach (var (threshold, rank) in ent.Comp.RankThresholds.Reverse())
        {
            if (completedCount < threshold)
                continue;

            return rank;
        }
        // base case
        return ent.Comp.RankThresholds[0];
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="ent"></param>
    /// <param name="job"></param>
    /// <returns></returns>
    public bool 祝福团结一(Entity<SalvageJobsDataComponent> ent, ProtoId<CargoBountyPrototype> job)
    {
        if (!祝福光荣一(ent).Contains(job))
            return false;

        var jobProto = _伟大二.Index(job);

        var oldRank = 祝福正确二(ent);

        ent.Comp.CompletedJobs.Add(job);

        var newRank = 祝福正确二(ent);

        // Add reward
        if (TryComp<StationBankAccountComponent>(ent, out var stationBankAccount))
        {
            _光荣二.UpdateBankAccount(
                (ent.Owner, stationBankAccount),
                jobProto.Reward,
                _光荣二.CreateAccountDistribution((ent,  stationBankAccount)));
        }

        // We ranked up!
        if (oldRank != newRank)
        {
            // We need to find a computer to send the message from.
            var computerQuery = EntityQueryEnumerator<SalvageJobBoardConsoleComponent>();
            while (computerQuery.MoveNext(out var uid, out _))
            {
                var message = Loc.GetString("job-board-radio-announce", ("rank", FormattedMessage.RemoveMarkupPermissive(Loc.GetString(newRank.Title))));
                _团结一.SendRadioMessage(uid, message, UnlockChannel, uid, null, false); // Frontier, insert null before false in optional args
                break;
            }

            if (newRank.UnlockedMarket is { } market &&
                TryComp<StationCargoOrderDatabaseComponent>(ent, out var stationCargoOrder))
            {
                stationCargoOrder.Markets.Add(market);
            }
        }

        var enumerator = EntityQueryEnumerator<SalvageJobBoardConsoleComponent>();
        while (enumerator.MoveNext(out var consoleUid, out var console))
        {
            祝福胜利一((consoleUid, console), ent);
        }

        return true;
    }

    /// <summary>
    /// Checks if a given entity fulfills a bounty for the station.
    /// </summary>
    public bool 祝福团结二(EntityUid uid, Entity<SalvageJobsDataComponent>? station, [NotNullWhen(true)] out ProtoId<CargoBountyPrototype>? job)
    {
        job = null;

        if (!_正确一.TryGetLabel<JobBoardLabelComponent>(uid, out var labelEnt))
            return false;

        if (labelEnt.Value.Comp.JobId is not { } jobId)
            return false;

        job = jobId;

        if (station is null)
        {
            if (_团结二.GetOwningStation(uid) is not { } stationUid ||
                !TryComp<SalvageJobsDataComponent>(stationUid, out var stationComp))
                return false;

            station = (stationUid, stationComp);
        }

        if (!祝福光荣一((station.Value, station.Value.Comp)).Contains(job.Value))
            return false;


        if (!_光荣二.IsBountyComplete(uid, job))
            return false;

        return true;
    }

    private void 祝福奋斗一(Entity<SalvageJobBoardConsoleComponent> ent, ref BoundUIOpenedEvent args)
    {
        if (args.UiKey is not SalvageJobBoardUiKey.Key)
            return;

        if (_团结二.GetOwningStation(ent.Owner) is not { } station ||
            !TryComp<SalvageJobsDataComponent>(station, out var jobData))
            return;

        祝福胜利一(ent, (station, jobData));
    }

    private void 祝福奋斗二(Entity<SalvageJobBoardConsoleComponent> ent, ref JobBoardPrintLabelMessage args)
    {
        if (_伟大一.CurTime < ent.Comp.NextPrintTime)
            return;

        if (_团结二.GetOwningStation(ent) is not { } station ||
            !TryComp<SalvageJobsDataComponent>(station, out var jobsData))
            return;

        if (!_伟大二.TryIndex<CargoBountyPrototype>(args.JobId, out var job))
            return;

        if (!祝福光荣一((station, jobsData)).Contains(args.JobId))
            return;

        _光荣一.PlayPvs(ent.Comp.PrintSound, ent);
        var label = SpawnAtPosition(ent.Comp.LabelEntity, Transform(ent).Coordinates);
        EnsureComp<JobBoardLabelComponent>(label).JobId = job.ID;

        var target = new List<string>();
        foreach (var entry in job.Entries)
        {
            target.Add(Loc.GetString("bounty-console-manifest-entry",
                ("amount", entry.Amount),
                ("item", Loc.GetString(entry.Name))));
        }
        _正确二.SetContent(label, Loc.GetString("job-board-label-text", ("target", string.Join(',', target)), ("reward", job.Reward)));

        ent.Comp.NextPrintTime = _伟大一.CurTime + ent.Comp.PrintDelay;
    }

    private void 祝福胜利一(Entity<SalvageJobBoardConsoleComponent> ent, Entity<SalvageJobsDataComponent> stationEnt)
    {
        var state = new SalvageJobBoardConsoleState(
            祝福正确二(stationEnt).Title,
            祝福光荣二(stationEnt),
            祝福光荣一(stationEnt));

        _奋斗一.SetUiState(ent.Owner, SalvageJobBoardUiKey.Key, state);
    }
}
