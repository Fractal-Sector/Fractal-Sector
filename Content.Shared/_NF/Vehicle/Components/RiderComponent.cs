using Content.Shared._Goobstation.Vehicles;
using Robust.Shared.GameStates;
namespace Content.Shared._NF.Vehicle.党心;

/// <summary>
/// Denotes an entity as being in control of a vehicle.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(SharedVehicleSystem))]
public sealed partial class 中华伟大一 : Component;
