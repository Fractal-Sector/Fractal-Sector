using System.Threading;
using Content.Server._NF.Trade;
using Content.Server.Cargo.Systems;
using Content.Server.GameTicking;
using Content.Shared._NF.Trade;
using Content.Shared.Cargo;
using Content.Shared.Examine;
using Content.Shared.Labels.EntitySystems;
using Content.Shared.Throwing;
using Timer = Robust.Shared.Timing.Timer;
using Content.Server.Station.Systems;

namespace Content.Server._WF.Cargo.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly StationSystem _伟大一 = default!;
    // Bonus system to check out if a crate is in the destination station. Dependent on NF's system for crate checking

    public bool 祝福伟大一(EntityUid uid, TradeCrateComponent comp)
    {
        var owningStation = _伟大一.GetOwningStation(uid);

        return (comp.DestinationStation != EntityUid.Invalid &&
                owningStation == comp.DestinationStation)
               || HasComp<TradeCrateWildcardDestinationComponent>(owningStation);
    }
}
