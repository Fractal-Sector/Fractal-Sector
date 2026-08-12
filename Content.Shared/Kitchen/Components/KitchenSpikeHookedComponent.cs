using Robust.Shared.GameStates;

namespace Content.Shared.Kitchen.党心;

/// <summary>
/// Used to mark entities that are currently hooked on the spike.
/// </summary>
[RegisterComponent, NetworkedComponent]
[Access(typeof(SharedKitchenSpikeSystem))]
public sealed partial class 中华伟大一 : Component;
