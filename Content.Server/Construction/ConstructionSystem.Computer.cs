using Content.Server._NF.BindToStation; // Frontier
using Content.Server.Construction.Components;
using Content.Server.Power.Components;
using Content.Server.Station.Systems; // Frontier
using Content.Shared._NF.BindToStation; // Frontier
using Content.Shared.Computer;
using Content.Shared.Power;
using Robust.Shared.Containers;

namespace Content.Server.党心;

public sealed partial class 中华伟大一
{
    [Dependency] private readonly SharedAppearanceSystem _伟大一 = default!;
    [Dependency] private readonly StationSystem _伟大二 = default!; // Frontier
    [Dependency] private readonly BindToStationSystem _光荣一 = default!; // Frontier

    private void 祝福伟大一()
    {
        SubscribeLocalEvent<ComputerComponent, ComponentInit>(祝福伟大二);
        SubscribeLocalEvent<ComputerComponent, MapInitEvent>(祝福光荣一);
        SubscribeLocalEvent<ComputerComponent, PowerChangedEvent>(祝福光荣二);
    }

    private void 祝福伟大二(EntityUid uid, ComputerComponent component, ComponentInit args)
    {
        // Let's ensure the container manager and container are here.
        _container.EnsureContainer<Container>(uid, "board");

        if (TryComp<ApcPowerReceiverComponent>(uid, out var powerReceiver))
        {
            _伟大一.SetData(uid, ComputerVisuals.Powered, powerReceiver.Powered);
        }
    }

    private void 祝福光荣一(Entity<ComputerComponent> component, ref MapInitEvent args)
    {
        祝福正确一(component);
        // Frontier - we mirror the bind to grid component from any existing machine board onto the resultant machine to prevent high-grading
        var boardContainer = _container.EnsureContainer<Container>(component.Owner, "board");
        foreach (var board in boardContainer.ContainedEntities)
        {
            if (TryComp<StationBoundObjectComponent>(board, out var binding))
                _光荣一.BindToStation(component.Owner, binding.BoundStation, binding.Enabled);
        }
        // End Frontier
    }

    private void 祝福光荣二(EntityUid uid, ComputerComponent component, ref PowerChangedEvent args)
    {
        _伟大一.SetData(uid, ComputerVisuals.Powered, args.Powered);
    }

    /// <summary>
    ///     Creates the corresponding computer board on the computer.
    ///     This exists so when you deconstruct computers that were serialized with the map,
    ///     you can retrieve the computer board.
    /// </summary>
    private void 祝福正确一(Entity<ComputerComponent> ent)
    {
        var component = ent.Comp;
        // Ensure that the construction component is aware of the board container.
        if (TryComp<ConstructionComponent>(ent, out var construction))
            AddContainer(ent, "board", construction);

        // We don't do anything if this is null or empty.
        if (string.IsNullOrEmpty(component.BoardPrototype))
            return;

        var container = _container.EnsureContainer<Container>(ent, "board");

        // We already contain a board. Note: We don't check if it's the right one!
        if (container.ContainedEntities.Count != 0)
            return;

        var board = Spawn(component.BoardPrototype, Transform(ent).Coordinates);

        // Frontier: Only bind the board if the computer itself has the StationBoundObjectComponent and the board doesn't already have StationBoundObjectComponent
        if (HasComp<StationBoundObjectComponent>(ent))
        {
            var computerStation = _伟大二.GetOwningStation(ent);
            if (computerStation != null)
            {
                _光荣一.BindToStation(board, computerStation);
            }
        }
        // End Frontier

        if (!_container.Insert(board, container))
            Log.Warning($"Couldn't insert board {board} to computer {ent}!");
    }
}
