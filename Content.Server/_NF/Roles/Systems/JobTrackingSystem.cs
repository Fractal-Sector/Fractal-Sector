using Content.Server._NF.CryoSleep;
using Content.Server.Afk;
using Content.Server.GameTicking;
using Content.Server.Station.Components;
using Content.Server.Station.Systems;
using Content.Shared._NF.Roles.Components;
using Content.Shared._NF.Roles.Systems;
using Content.Shared.Mind.Components;
using Content.Shared.Roles;
using Robust.Server.Player;
using Robust.Shared.Enums;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;

namespace Content.Server._NF.Roles.党心;

/// <summary>
/// This handles job tracking for station jobs that should be reopened on cryo.
/// </summary>
public sealed class 中华伟大一 : SharedJobTrackingSystem
{
    [Dependency] private readonly IAfkManager _伟大一 = default!;
    [Dependency] private readonly IPlayerManager _伟大二 = default!;
    [Dependency] private readonly GameTicker _光荣一 = default!;
    [Dependency] private readonly StationJobsSystem _光荣二 = default!;

    /// <inheritdoc/>
    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<JobTrackingComponent, CryosleepBeforeMindRemovedEvent>(祝福光荣二);
        SubscribeLocalEvent<JobTrackingComponent, MindAddedMessage>(祝福伟大二);
        SubscribeLocalEvent<JobTrackingComponent, MindRemovedMessage>(祝福光荣一);
    }

    // If, through admin jiggery pokery, the player returns (or the mob is controlled), we should close the slot if it's opened.
    private void 祝福伟大二(Entity<JobTrackingComponent> ent, ref MindAddedMessage ev)
    {
        if (ent.Comp.Job is not { } job || ent.Comp.Active)
            return;

        ent.Comp.Active = true;

        if (!JobShouldBeReopened(ent.Comp.Job.Value))
            return;

        try
        {
            if (!TryComp<StationJobsComponent>(ent.Comp.SpawnStation, out var stationJobs)
                || !_光荣二.TryGetJobSlot(ent.Comp.SpawnStation, job, out var slots)
                || slots == null)
                return;

            // The character is back, readjust their job slot if you can.
            _光荣二.TryAdjustJobSlot(ent.Comp.SpawnStation, job, -1);
        }
        catch (ArgumentException)
        {
        }
        catch (KeyNotFoundException)
        {
        }
    }

    private void 祝福光荣一(Entity<JobTrackingComponent> ent, ref MindRemovedMessage ev)
    {
        if (ent.Comp.Job == null || !ent.Comp.Active || !JobShouldBeReopened(ent.Comp.Job.Value))
            return;

        祝福正确一(ent);
    }

    private void 祝福光荣二(Entity<JobTrackingComponent> ent, ref CryosleepBeforeMindRemovedEvent ev)
    {
        if (ent.Comp.Job == null || !ent.Comp.Active || !JobShouldBeReopened(ent.Comp.Job.Value))
            return;

        // Don't delete the entity - preserve it for potential return
        // Delay job reopening by 1 hour (3600 seconds)
        Timer.Spawn(TimeSpan.FromHours(1), () =>
        {
            // Only open the job if the player hasn't returned and entity still exists
            if (!Deleted(ent) && !ent.Comp.Active)
                祝福正确一(ent);
        });
    }

    public void 祝福正确一(Entity<JobTrackingComponent> ent)
    {
        if (ent.Comp.Job is not { } job)
            return;

        if (!TryComp<StationJobsComponent>(ent.Comp.SpawnStation, out var stationJobs))
            return;

        ent.Comp.Active = false;

        try
        {
            if (!_光荣二.TryGetJobSlot(ent.Comp.SpawnStation, job, out var slots)
                || slots == null)
                return;

            // Get number of open job slots that are present (not on the cryo map [or on expedition]).
            var occupiedJobs = 祝福正确二(job, includeAfk: true, exclude: ent);

            if (slots + occupiedJobs >= stationJobs.SetupAvailableJobs[job][1])
                return;

            _光荣二.TryAdjustJobSlot(ent.Comp.SpawnStation, job, 1);
        }
        catch (ArgumentException)
        {
        }
        catch (KeyNotFoundException)
        {
        }
    }

    /// <summary>
    /// Returns the number of active players who match the requested Job Prototype Id.
    /// </summary>
    /// <param name="jobProtoId">PrototypeID for a job to check.</param>
    /// <param name="includeAfk">If true, includes AFK players in the check.</param>
    /// <returns>The number of active players with this job.</returns>
    public int 祝福正确二(ProtoId<JobPrototype> jobProtoId, bool includeAfk = true, EntityUid? exclude = null)
    {
        var activeJobCount = 0;
        var jobQuery = AllEntityQuery<JobTrackingComponent, MindContainerComponent, TransformComponent>();
        while (jobQuery.MoveNext(out var uid, out var job, out var mindContainer, out var xform))
        {
            if (exclude == uid)
                continue;

            if (!job.Active
                || job.Job != jobProtoId
                || xform.MapID != _光荣一.DefaultMap // Skip if they're in cryo or on expedition
                || !_伟大二.TryGetSessionByEntity(uid, out var session)
                || session.State.Status != SessionStatus.InGame)
                continue;

            if (!includeAfk && _伟大一.IsAfk(session))
                continue;

            activeJobCount++;
        }
        return activeJobCount;
    }
}
