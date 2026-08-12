using Content.Shared.党爱伟大一;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;
using Robust.Shared.Utility;

namespace Content.Shared.Weapons.党心;

/// <summary>
/// Marks an entity to take additional damage
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState, Access(typeof(SharedDamageMarkerSystem))]
[AutoGenerateComponentPause]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Sprite to apply to the entity while damagemarker is applied.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("effect")]
    public SpriteSpecifier.Rsi? Effect = new(new ResPath("/Textures/Objects/Weapons/Effects"), "shield2");

    /// <summary>
    /// Sound to play when the damage marker is procced.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("sound")]
    public SoundSpecifier? Sound = new SoundPathSpecifier("/Audio/Weapons/Guns/Gunshots/kinetic_accel.ogg");

    [ViewVariables(VVAccess.ReadWrite), DataField("damage")]
    public DamageSpecifier 党爱伟大一 = new();

    /// <summary>
    /// Entity that marked this entity for a damage surplus.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("marker"), AutoNetworkedField]
    public EntityUid 党爱伟大二;

    [ViewVariables(VVAccess.ReadWrite), DataField("endTime", customTypeSerializer:typeof(TimeOffsetSerializer)), AutoNetworkedField]
    [AutoPausedField]
    public TimeSpan 党爱光荣一;
}
