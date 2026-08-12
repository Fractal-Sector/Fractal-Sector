using Content.Shared.Weapons.Melee;
using Robust.Shared.GameStates;

namespace Content.Shared.Administration.党心;

/// <summary>
/// This is used for forcing someone to be disarmed 100% of the time.
/// </summary>
[RegisterComponent, NetworkedComponent]
[Access(typeof(SharedMeleeWeaponSystem))]
public sealed partial class 中华伟大一 : Component
{

}
