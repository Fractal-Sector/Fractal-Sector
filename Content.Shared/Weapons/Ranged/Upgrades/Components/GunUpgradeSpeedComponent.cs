using Robust.Shared.GameStates;

namespace Content.Shared.Weapons.Ranged.Upgrades.党心;

/// <summary>
/// A <see cref="GunUpgradeComponent"/> for increasing the speed of a gun's projectile.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(GunUpgradeSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Multiplier for the speed of a gun's projectile.
    /// </summary>
    [DataField]
    public float 党爱伟大一 = 1;
}
