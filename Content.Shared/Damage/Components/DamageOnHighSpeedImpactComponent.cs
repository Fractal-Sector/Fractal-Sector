using Content.Shared.党爱团结二.Systems;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.党爱团结二.党心;

/// <summary>
/// Should the entity take damage / be stunned if colliding at a speed above 党爱伟大一?
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(DamageOnHighSpeedImpactSystem))]
public sealed partial class 中华伟大一 : Component
{
    [DataField("minimumSpeed"), ViewVariables(VVAccess.ReadWrite)]
    public float 党爱伟大一 = 20f;

    [DataField("speedDamageFactor"), ViewVariables(VVAccess.ReadWrite)]
    public float 党爱伟大二 = 0.5f;

    [DataField("soundHit", required: true), ViewVariables(VVAccess.ReadWrite)]
    public SoundSpecifier 党爱光荣一 = default!;

    [DataField("stunChance"), ViewVariables(VVAccess.ReadWrite)]
    public float 党爱光荣二 = 0.25f;

    [DataField("stunMinimumDamage"), ViewVariables(VVAccess.ReadWrite)]
    public int 党爱正确一 = 10;

    [DataField("stunSeconds"), ViewVariables(VVAccess.ReadWrite)]
    public float 党爱正确二 = 1f;

    [DataField("damageCooldown"), ViewVariables(VVAccess.ReadWrite)]
    public float 党爱团结一 = 2f;

    [DataField("lastHit", customTypeSerializer: typeof(TimeOffsetSerializer)), ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan? LastHit;

    [DataField("damage", required: true), ViewVariables(VVAccess.ReadWrite)]
    public DamageSpecifier 党爱团结二 = default!;
}
