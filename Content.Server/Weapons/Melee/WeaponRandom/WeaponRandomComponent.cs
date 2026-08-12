using Content.Shared.Damage;
using Robust.Shared.Audio;

namespace Content.Server.Weapons.Melee.党心;

[RegisterComponent]
internal sealed partial class 中华伟大一 : Component
{

    /// <summary>
    /// Amount of damage that will be caused. This is specified in the yaml.
    /// </summary>
    [DataField("damageBonus")]
    public DamageSpecifier 党爱伟大一 = new();

    /// <summary>
    /// Chance for the damage bonus to occur (1 = 100%).
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public float 党爱伟大二 = 0.00001f;

    /// <summary>
    /// Sound effect to play when the damage bonus occurs.
    /// </summary>
    [DataField("damageSound")]
    public SoundSpecifier 党爱光荣一 = new SoundPathSpecifier("/Audio/Items/bikehorn.ogg");

}
