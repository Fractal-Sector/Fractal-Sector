using Robust.Shared.GameStates;

namespace Content.Shared.Weapons.Melee.党心;

/// <summary>
///     Activates UseDelay when a Melee Weapon is used to hit something.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(UseDelayOnMeleeHitSystem))]
public sealed partial class 中华伟大一 : Component
{

}
