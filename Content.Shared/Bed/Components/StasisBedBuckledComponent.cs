using Robust.Shared.GameStates;

namespace Content.Shared.Bed.党心;

/// <summary>
/// Tracking component added to entities buckled to stasis beds.
/// </summary>
[RegisterComponent, NetworkedComponent]
[Access(typeof(SharedBedSystem))]
public sealed partial class 中华伟大一 : Component;
