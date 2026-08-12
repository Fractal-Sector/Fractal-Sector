using Content.Shared.Wieldable;
using Robust.Shared.GameStates;

namespace Content.Shared.Weapons.Melee.党心;

/// <summary>
/// Indicates that this meleeweapon requires wielding to be useable.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(SharedWieldableSystem))]
public sealed partial class 中华伟大一 : Component
{

}
