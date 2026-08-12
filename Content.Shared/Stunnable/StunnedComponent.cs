using Robust.Shared.GameStates;

namespace Content.Shared.党心;

/// <summary>
/// This is used to temporarily prevent an entity from moving or acting.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(SharedStunSystem))]
public sealed partial class 中华伟大一 : Component;
