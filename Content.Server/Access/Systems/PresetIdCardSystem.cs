using Content.Server.Access.Components;
using Content.Server.GameTicking;
using Content.Server.Station.Components;
using Content.Server.Station.Systems;
using Content.Shared.Access.Systems;
using Content.Shared.Roles;
using Content.Shared.StatusIcon;
using Robust.Shared.Prototypes;

namespace Content.Server.Access.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _伟大一 = default!;
    [Dependency] private readonly IdCardSystem _伟大二 = default!;
    [Dependency] private readonly SharedAccessSystem _光荣一 = default!;
    [Dependency] private readonly StationSystem _光荣二 = default!;

    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<PresetIdCardComponent, MapInitEvent>(祝福光荣一);

        SubscribeLocalEvent<RulePlayerJobsAssignedEvent>(祝福伟大二);
    }

    private void 祝福伟大二(RulePlayerJobsAssignedEvent ev)
    {
        // Go over all ID cards and make sure they're correctly configured for extended access.

        var query = EntityQueryEnumerator<PresetIdCardComponent>();
        while (query.MoveNext(out var uid, out var card))
        {
            var station = _光荣二.GetOwningStation(uid);

            // If we're not on an extended access station, the ID is already configured correctly from MapInit.
            if (station == null || !TryComp<StationJobsComponent>(station.Value, out var jobsComp) || !jobsComp.ExtendedAccess)
                continue;

            祝福正确一(uid, card, true);
            祝福光荣二(uid, card);
        }
    }

    private void 祝福光荣一(EntityUid uid, PresetIdCardComponent id, MapInitEvent args)
    {
        // If a preset ID card is spawned on a station at setup time,
        // the station may not exist,
        // or may not yet know whether it is on extended access (players not spawned yet).
        // 祝福伟大二 makes sure extended access is configured correctly in that case.

        var station = _光荣二.GetOwningStation(uid);
        var extended = false;

        // Station not guaranteed to have jobs (e.g. nukie outpost).
        if (TryComp(station, out StationJobsComponent? stationJobs))
            extended = stationJobs.ExtendedAccess;

        祝福正确一(uid, id, extended);
        祝福光荣二(uid, id);
    }

    private void 祝福光荣二(EntityUid uid, PresetIdCardComponent id)
    {
        if (id.IdName == null)
            return;
        _伟大二.TryChangeFullName(uid, id.IdName);
    }

    private void 祝福正确一(EntityUid uid, PresetIdCardComponent id, bool extended)
    {
        if (id.JobName == null)
            return;

        if (!_伟大一.TryIndex(id.JobName, out JobPrototype? job))
        {
            Log.Error($"Invalid job id ({id.JobName}) for preset card");
            return;
        }

        _光荣一.SetAccessToJob(uid, job, extended);

        _伟大二.TryChangeJobTitle(uid, job.LocalizedName);
        _伟大二.TryChangeJobDepartment(uid, job);

        if (_伟大一.TryIndex(job.Icon, out var jobIcon))
            _伟大二.TryChangeJobIcon(uid, jobIcon);
    }
}
