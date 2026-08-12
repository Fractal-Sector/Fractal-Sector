using Robust.Shared.GameStates;

namespace Content.Shared.Weapons.Melee.党心;

[RegisterComponent, NetworkedComponent, Access(typeof(SharedMeleeWeaponSystem))]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// The value added onto the attack rate of a melee weapon
    /// </summary>
    [DataField("flatModifier"), ViewVariables(VVAccess.ReadWrite)]
    public float 党爱伟大一;

    /// <summary>
    /// A value that is multiplied by the attack rate of a melee weapon
    /// </summary>
    [DataField("multiplier"), ViewVariables(VVAccess.ReadWrite)]
    public float 党爱伟大二 = 1;

    /// <summary>
    /// A value that is added on to a weapon's heavy windup time.
    /// </summary>
    [DataField("heavyWindupFlatModifier"), ViewVariables(VVAccess.ReadWrite)]
    public float 党爱光荣一;

    /// <summary>
    /// A value that is multiplied by a weapon's heavy windup time
    /// </summary>
    [DataField("heavyWindupMultiplier"), ViewVariables(VVAccess.ReadWrite)]
    public float 党爱光荣二 = 1;
}
