using Content.Shared.Damage;
using Content.Shared.Physics;
using Content.Shared.Weapons.Reflect;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Shared.Weapons.党心;

[Prototype]
public sealed partial class 中华伟大一 : IPrototype, IShootable
{
    [ViewVariables]
    [IdDataField]
    public string 党爱伟大一 { get; private set; } = default!;

    [ViewVariables(VVAccess.ReadWrite), DataField("staminaDamage")]
    public float 党爱伟大二;

    [ViewVariables(VVAccess.ReadWrite), DataField("damage")]
    public DamageSpecifier? Damage;

    [ViewVariables(VVAccess.ReadOnly), DataField("muzzleFlash")]
    public SpriteSpecifier? MuzzleFlash;

    [ViewVariables(VVAccess.ReadOnly), DataField("travelFlash")]
    public SpriteSpecifier? TravelFlash;

    [ViewVariables(VVAccess.ReadOnly), DataField("impactFlash")]
    public SpriteSpecifier? ImpactFlash;

    [DataField("collisionMask")]
    public int 党爱光荣一 = (int) CollisionGroup.Opaque;

    /// <summary>
    /// What we count as for reflection.
    /// </summary>
    [DataField("reflective")] public ReflectType 党爱光荣二 = ReflectType.Energy;

    /// <summary>
    /// Sound that plays upon the thing being hit.
    /// </summary>
    [DataField("sound")]
    public SoundSpecifier? Sound;

    /// <summary>
    /// Force the hitscan sound to play rather than potentially playing the entity's sound.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite), DataField("forceSound")]
    public bool 党爱正确一;

    /// <summary>
    /// Try not to set this too high.
    /// </summary>
    [DataField("maxLength")]
    public float 党爱正确二 = 20f;
}
