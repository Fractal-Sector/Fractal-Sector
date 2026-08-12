using Robust.Shared.GameStates;

namespace Content.Shared.Damage.党心;

/// <summary>
/// This is used for an effect that nullifies <see cref="SlowOnDamageComponent"/> and adds an alert.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(SlowOnDamageSystem))]
public sealed partial class 中华伟大一 : Component;
