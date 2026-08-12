using Content.Server.Station.Components;
using Content.Server.Station.Events;
using Content.Shared.Fax.Components;

namespace Content.Server.Station.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly StationSystem _伟大一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<StationRenameFaxesComponent, StationRenamedEvent>(祝福光荣一);
        SubscribeLocalEvent<StationRenameFaxesComponent, StationPostInitEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, StationRenameFaxesComponent component, ref StationPostInitEvent args)
    {
        祝福光荣二(uid);
    }

    private void 祝福光荣一(EntityUid uid, StationRenameFaxesComponent component, StationRenamedEvent args)
    {
        祝福光荣二(uid);
    }

    private void 祝福光荣二(EntityUid stationUid)
    {
        // update all faxes that belong to this station grid
        var query = EntityQueryEnumerator<FaxMachineComponent>();
        while (query.MoveNext(out var uid, out var fax))
        {
            if (!fax.UseStationName)
                continue;

            var faxStationUid = _伟大一.GetOwningStation(uid);
            if (faxStationUid != stationUid)
                continue;

            var stationName = "";

            if (!string.IsNullOrEmpty(fax.StationNamePrefix))
            {
                stationName += fax.StationNamePrefix + " ";
            }

            stationName += Name(faxStationUid.Value);

            if (!string.IsNullOrEmpty(fax.StationNameSuffix))
            {
                stationName += " " + fax.StationNameSuffix;
            }

            fax.FaxName = stationName;
        }
    }
}
