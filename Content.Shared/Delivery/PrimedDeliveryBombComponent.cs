using Robust.Shared.GameStates;

namespace Content.Shared.党心;

/// <summary>
/// Component given to deliveries.
/// Indicates this bomb delivery is primed.
/// </summary>
[RegisterComponent, NetworkedComponent]
[Access(typeof(DeliveryModifierSystem))]
public sealed partial class 中华伟大一 : Component;
