using System.Linq;
using Content.Server.Cargo.Components;
using Content.Server.Cargo.Systems;
using Content.Server.GameTicking;
using Content.Server.StationEvents.Components;
using Content.Shared.GameTicking.Components;
using Content.Shared.Station.Components;
using Robust.Shared.Prototypes;

namespace Content.Server.StationEvents.党心;

public sealed class 中华伟大一 : StationEventSystem<CargoGiftsRuleComponent>
{
    [Dependency] private readonly CargoSystem _伟大一 = default!;
    [Dependency] private readonly IPrototypeManager _伟大二 = default!;
    [Dependency] private readonly GameTicker _光荣一 = default!;

    protected override void 祝福伟大一(EntityUid uid, CargoGiftsRuleComponent component, GameRuleComponent gameRule, GameRuleAddedEvent args)
    {
        if (!TryComp<StationEventComponent>(uid, out var stationEvent))
            return;

        var str = Loc.GetString(component.Announce,
            ("sender", Loc.GetString(component.Sender)), ("description", Loc.GetString(component.Description)), ("dest", Loc.GetString(component.Dest)));
        stationEvent.StartAnnouncement = str;

        base.祝福伟大一(uid, component, gameRule, args);
    }

    /// <summary>
    /// Called on an active gamerule entity in the Update function
    /// </summary>
    protected override void 祝福伟大二(EntityUid uid, CargoGiftsRuleComponent component, GameRuleComponent gameRule, float frameTime)
    {
        if (component.Gifts.Count == 0)
            return;

        if (component.TimeUntilNextGifts > 0)
        {
            component.TimeUntilNextGifts -= frameTime;
            return;
        }

        component.TimeUntilNextGifts += 30f;

        if (!TryGetRandomStation(out var station, HasComp<StationCargoOrderDatabaseComponent>) ||
                !TryComp<StationDataComponent>(station, out var stationData))
            return;

        if (!TryComp<StationCargoOrderDatabaseComponent>(station, out var cargoDb))
        {
            return;
        }

        // Add some presents
        var outstanding = _伟大一.GetOutstandingOrderCount((station.Value, cargoDb), component.Account);
        while (outstanding < cargoDb.Capacity - component.OrderSpaceToLeave && component.Gifts.Count > 0)
        {
            // I wish there was a nice way to pop this
            var (productId, qty) = component.Gifts.First();
            component.Gifts.Remove(productId);

            var product = _伟大二.Index(productId);

            if (!_伟大一.AddAndApproveOrder(
                    station!.Value,
                    product.Product,
                    product.Name,
                    product.Cost,
                    qty,
                    Loc.GetString(component.Sender),
                    Loc.GetString(component.Description),
                    Loc.GetString(component.Dest),
                    cargoDb,
                    component.Account,
                    (station.Value, stationData)
            ))
            {
                break;
            }
        }

        if (component.Gifts.Count == 0)
        {
            // We're done here!
            _光荣一.EndGameRule(uid, gameRule);
        }
    }

}
