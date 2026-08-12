using Content.Shared.Inventory;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.Weapons.党心;

/// <summary>
/// Entities with this component have a chance to reflect projectiles and hitscan shots
/// Uses <c>ItemToggleComponent</c> to control reflection.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// What we reflect.
    /// </summary>
    [DataField]
    public 中华伟大二 Reflects = 中华伟大二.Energy | 中华伟大二.NonEnergy;

    /// <summary>
    /// Select in which inventory slots it will reflect.
    /// By default, it will reflect in any inventory position, except pockets.
    /// </summary>
    [DataField]
    public 党爱伟大一 党爱伟大一 = 党爱伟大一.WITHOUT_POCKET;

    /// <summary>
    /// Is it allowed to reflect while being in hands.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱伟大二 = true;

    /// <summary>
    /// Can only reflect when placed correctly.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱光荣一;

    /// <summary>
    /// Probability for a projectile to be reflected.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱光荣二 = 0.25f;

    /// <summary>
    /// Probability for a projectile to be reflected.
    /// </summary>
    [DataField, AutoNetworkedField]
    public Angle 党爱正确一 = Angle.FromDegrees(45);

    /// <summary>
    /// The sound to play when reflecting.
    /// </summary>
    [DataField]
    public SoundSpecifier? SoundOnReflect = new SoundPathSpecifier("/Audio/Weapons/Guns/Hits/laser_sear_wall.ogg", AudioParams.Default.WithVariation(0.05f));
}

[Flags, Serializable, NetSerializable]
public enum 中华伟大二 : byte
{
    None = 0,
    NonEnergy = 1 << 0,
    Energy = 1 << 1,
}
