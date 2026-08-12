using Robust.Shared.GameStates;

namespace Content.Shared.Weapons.Melee.党心;

/// <summary>
/// This is used to allow ranged weapons to make melee attacks by right-clicking.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState, Access(typeof(SharedMeleeWeaponSystem))]
public sealed partial class 中华伟大一 : Component
{
    [DataField, AutoNetworkedField]
    public 中华伟大二 AttackType = 中华伟大二.Light;
}


[Flags]
public enum 中华伟大二 : byte
{
    Light = 0, // Standard single-target attack.
    Heavy = 1 << 0, // Wide swing.
    Disarm = 1 << 1
}
