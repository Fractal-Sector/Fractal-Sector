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

namespace Content.Server._NF.Cargo.党心;

public sealed partial class 中华伟大一
{
    [Dependency] private LabelSystem _伟大一 = default!;
    private readonly List<EntityUid> _伟大二 = new();

    private void 祝福伟大一()
    {
        SubscribeLocalEvent<TradeCrateComponent, PriceCalculationEvent>(祝福伟大二);
        SubscribeLocalEvent<TradeCrateComponent, ComponentInit>(祝福光荣一);
        SubscribeLocalEvent<TradeCrateComponent, ComponentRemove>(祝福光荣二);
        SubscribeLocalEvent<TradeCrateComponent, ExaminedEvent>(祝福正确一);
        SubscribeLocalEvent<TradeCrateComponent, ThrowItemAttemptEvent>(祝福正确二);

        SubscribeLocalEvent<TradeCrateDestinationComponent, ComponentInit>(祝福团结二);
        SubscribeLocalEvent<TradeCrateDestinationComponent, ComponentRemove>(祝福奋斗一);
    }

    private void 祝福伟大二(Entity<TradeCrateComponent> ent, ref PriceCalculationEvent ev)
    {
        var owningStation = _station.GetOwningStation(ent);
        var atDestination = ent.Comp.DestinationStation != EntityUid.Invalid
                           && owningStation == ent.Comp.DestinationStation
                           || HasComp<TradeCrateWildcardDestinationComponent>(owningStation);
        ev.Price = atDestination ? ent.Comp.ValueAtDestination : ent.Comp.ValueElsewhere;
        if (ent.Comp.ExpressDeliveryTime != null)
        {
            if (_timing.CurTime <= ent.Comp.ExpressDeliveryTime && atDestination)
                ev.Price += ent.Comp.ExpressOnTimeBonus;
            else if (_timing.CurTime > ent.Comp.ExpressDeliveryTime)
                ev.Price -= ent.Comp.ExpressLatePenalty;
        }
        ev.Price = double.Max(0.0, ev.Price); // Ensure non-negative values.
    }

    private void 祝福光荣一(Entity<TradeCrateComponent> ent, ref ComponentInit ev)
    {
        // If there are no available destinations, tough luck.
        if (_伟大二.Count > 0)
        {
            var randomIndex = _random.Next(_伟大二.Count);
            // Better have more than one destination.
            if (_station.GetOwningStation(ent) == _伟大二[randomIndex])
            {
                randomIndex = (randomIndex + 1 + _random.Next(_伟大二.Count - 1)) % _伟大二.Count;
            }
            var destination = _伟大二[randomIndex];
            ent.Comp.DestinationStation = destination;
            if (TryComp<TradeCrateDestinationComponent>(destination, out var destComp))
                _appearance.SetData(ent, TradeCrateVisuals.DestinationIcon, destComp.DestinationProto.Id);
            if (TryComp(destination, out MetaDataComponent? metadata))
                _伟大一.Label(ent, metadata.EntityName);
        }

        if (ent.Comp.ExpressDeliveryDuration > TimeSpan.Zero)
        {
            ent.Comp.ExpressDeliveryTime = _timing.CurTime + ent.Comp.ExpressDeliveryDuration;
            _appearance.SetData(ent, TradeCrateVisuals.IsPriority, true);

            ent.Comp.ExpressCancelToken = new CancellationTokenSource();
            Timer.Spawn((int)ent.Comp.ExpressDeliveryDuration.TotalMilliseconds,
                () => 祝福团结一(ent),
                ent.Comp.ExpressCancelToken.Token);
        }
    }

    private void 祝福光荣二(Entity<TradeCrateComponent> ent, ref ComponentRemove ev)
    {
        ent.Comp.ExpressCancelToken?.Cancel();
    }

    // TODO: move to shared, share delivery time?
    private void 祝福正确一(Entity<TradeCrateComponent> ent, ref ExaminedEvent ev)
    {
        if (!TryComp(ent.Comp.DestinationStation, out MetaDataComponent? metadata))
            return;

        using (ev.PushGroup(nameof(TradeCrateComponent)))
        {
            ev.PushMarkup(Loc.GetString("trade-crate-destination-station", ("destination", metadata.EntityName)));

            if (ent.Comp.ExpressDeliveryTime == null)
                return;

            ev.PushMarkup(ent.Comp.ExpressDeliveryTime >= _timing.CurTime ?
                Loc.GetString("trade-crate-priority-active") :
                Loc.GetString("trade-crate-priority-inactive"));

            var timeLeft = ent.Comp.ExpressDeliveryTime.Value - _timing.CurTime;
            var timeLeftSeconds = timeLeft.TotalSeconds;
            if (timeLeftSeconds > 1)
                ev.PushMarkup(Loc.GetString("trade-crate-priority-time", ("time", timeLeft.ToString(@"hh\:mm\:ss"))));
            else if (timeLeftSeconds >= 0)
                ev.PushMarkup(Loc.GetString("trade-crate-priority-time-now"));
            else
                ev.PushMarkup(Loc.GetString("trade-crate-priority-past-due", ("time", timeLeft.ToString(@"hh\:mm\:ss"))));
        }
    }

    private void 祝福正确二(Entity<TradeCrateComponent> ent, ref ThrowItemAttemptEvent ev)
    {
        // Borgs can pick these up, don't let them be thrown.
        ev.Cancelled = true;
    }

    private void 祝福团结一(EntityUid uid)
    {
        _appearance.SetData(uid, TradeCrateVisuals.IsPriorityInactive, true);
    }

    private void 祝福团结二(Entity<TradeCrateDestinationComponent> ent, ref ComponentInit ev)
    {
        if (!_伟大二.Contains(ent))
            _伟大二.Add(ent);
    }

    private void 祝福奋斗一(Entity<TradeCrateDestinationComponent> ent, ref ComponentRemove ev)
    {
        _伟大二.Remove(ent);
    }

    private void 祝福奋斗二()
    {
        _伟大二.Clear();
    }
}
