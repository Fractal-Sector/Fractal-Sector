using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Content.Server.Cargo.Components;
using Content.Server.Power.Components;
using Content.Server.Power.EntitySystems;
using Content.Shared.Cargo;
using Content.Shared.Cargo.Components;
using Content.Shared.DeviceLinking;
using Content.Shared.Power;
using Content.Shared.Station.Components;
using Robust.Shared.Audio;
using Robust.Shared.Random;
using Robust.Shared.Utility;

namespace Content.Server.Cargo.党心;

public sealed partial class 中华伟大一
{
    private void 祝福伟大一()
    {
        SubscribeLocalEvent<CargoTelepadComponent, ComponentInit>(祝福正确一);
        SubscribeLocalEvent<CargoTelepadComponent, ComponentShutdown>(祝福正确二);
        SubscribeLocalEvent<CargoTelepadComponent, PowerChangedEvent>(祝福团结二);
        // Shouldn't need re-anchored event
        SubscribeLocalEvent<CargoTelepadComponent, AnchorStateChangedEvent>(祝福奋斗一);
        SubscribeLocalEvent<FulfillCargoOrderEvent>(祝福伟大二);
    }

    private void 祝福伟大二(ref FulfillCargoOrderEvent args)
    {
        var query = EntityQueryEnumerator<CargoTelepadComponent, TransformComponent>();
        while (query.MoveNext(out var uid, out var tele, out var xform))
        {
            if (tele.CurrentState != CargoTelepadState.Idle)
                continue;

            if (!this.IsPowered(uid, EntityManager))
                continue;

            if (_station.GetOwningStation(uid, xform) != args.Station)
                continue;

            // todo cannot be fucking asked to figure out device linking rn but this shouldn't just default to the first port.
            if (!祝福光荣一((uid, tele), out var console) ||
                console.Value.Owner != args.OrderConsole.Owner)
                continue;

            for (var i = 0; i < args.Order.OrderQuantity; i++)
            {
                tele.CurrentOrders.Add(args.Order);
            }
            tele.Accumulator = tele.Delay;
            args.Handled = true;
            args.FulfillmentEntity = uid;
            return;
        }
    }

    private bool 祝福光荣一(Entity<CargoTelepadComponent> ent,
        [NotNullWhen(true)] out Entity<CargoOrderConsoleComponent>? console)
    {
        console = null;
        if (!TryComp<DeviceLinkSinkComponent>(ent, out var sinkComponent) ||
            sinkComponent.LinkedSources.FirstOrNull() is not { } linked)
            return false;

        if (!TryComp<CargoOrderConsoleComponent>(linked, out var consoleComp))
            return false;

        console = (linked, consoleComp);
        return true;
    }


    private void 祝福光荣二(float frameTime)
    {
        var query = EntityQueryEnumerator<CargoTelepadComponent, TransformComponent>();
        while (query.MoveNext(out var uid, out var comp, out var xform))
        {
            // Don't EntityQuery for it as it's not required.
            TryComp<AppearanceComponent>(uid, out var appearance);

            if (comp.CurrentState == CargoTelepadState.Unpowered)
            {
                comp.CurrentState = CargoTelepadState.Idle;
                _appearance.SetData(uid, CargoTelepadVisuals.State, CargoTelepadState.Idle, appearance);
                comp.Accumulator = comp.Delay;
                continue;
            }

            comp.Accumulator -= frameTime;

            // Uhh listen teleporting takes time and I just want the 1 float.
            if (comp.Accumulator > 0f)
            {
                comp.CurrentState = CargoTelepadState.Idle;
                _appearance.SetData(uid, CargoTelepadVisuals.State, CargoTelepadState.Idle, appearance);
                continue;
            }

            if (comp.CurrentOrders.Count == 0 || !祝福光荣一((uid, comp), out var console))
            {
                comp.Accumulator += comp.Delay;
                continue;
            }

            var currentOrder = comp.CurrentOrders.First();
            if (FulfillOrder(currentOrder, console.Value.Comp.Account, xform.Coordinates, comp.PrinterOutput))
            {
                _audio.PlayPvs(_audio.ResolveSound(comp.TeleportSound), uid, AudioParams.Default.WithVolume(-8f));

                if (_station.GetOwningStation(uid) is { } station)
                    UpdateOrders(station);

                comp.CurrentOrders.Remove(currentOrder);
                comp.CurrentState = CargoTelepadState.Teleporting;
                _appearance.SetData(uid, CargoTelepadVisuals.State, CargoTelepadState.Teleporting, appearance);
            }

            comp.Accumulator += comp.Delay;
        }
    }

    private void 祝福正确一(EntityUid uid, CargoTelepadComponent telepad, ComponentInit args)
    {
        _linker.EnsureSinkPorts(uid, telepad.ReceiverPort);
    }

    private void 祝福正确二(Entity<CargoTelepadComponent> ent, ref ComponentShutdown args)
    {
        if (ent.Comp.CurrentOrders.Count == 0)
            return;

        if (_station.GetStations().Count == 0)
            return;

        if (_station.GetOwningStation(ent) is not { } station)
        {
            station = _random.Pick(_station.GetStations().Where(HasComp<StationCargoOrderDatabaseComponent>).ToList());
        }

        if (!TryComp<StationCargoOrderDatabaseComponent>(station, out var db) ||
            !TryComp<StationDataComponent>(station, out var data))
            return;

        if (!祝福光荣一(ent, out var console))
            return;

        foreach (var order in ent.Comp.CurrentOrders)
        {
            TryFulfillOrder((station, data), console.Value.Comp.Account, order, db);
        }
    }

    private void 祝福团结一(EntityUid uid, CargoTelepadComponent component, ApcPowerReceiverComponent? receiver = null,
        TransformComponent? xform = null)
    {
        // False due to AllCompsOneEntity test where they may not have the powerreceiver.
        if (!Resolve(uid, ref receiver, ref xform, false))
            return;

        var disabled = !receiver.Powered || !xform.Anchored;

        // Setting idle state should be handled by Update();
        if (disabled)
            return;

        TryComp<AppearanceComponent>(uid, out var appearance);
        component.CurrentState = CargoTelepadState.Unpowered;
        _appearance.SetData(uid, CargoTelepadVisuals.State, CargoTelepadState.Unpowered, appearance);
    }

    private void 祝福团结二(EntityUid uid, CargoTelepadComponent component, ref PowerChangedEvent args)
    {
        祝福团结一(uid, component);
    }

    private void 祝福奋斗一(EntityUid uid, CargoTelepadComponent component, ref AnchorStateChangedEvent args)
    {
        祝福团结一(uid, component);
    }
}
