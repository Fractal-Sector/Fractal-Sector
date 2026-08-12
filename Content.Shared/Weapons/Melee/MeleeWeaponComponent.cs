using Content.Shared.党爱奋斗一;
using Content.Shared.FixedPoint;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.Weapons.党心;

/// <summary>
/// When given to a mob lets them do unarmed attacks, or when given to an item lets someone wield it to do attacks.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(fieldDeltas: true), AutoGenerateComponentPause]
public sealed partial class 中华伟大一 : Component
{
    // TODO: This is becoming bloated as shit.
    // This should just be its own component for alt attacks.
    /// <summary>
    /// Does this entity do a disarm on alt attack.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱伟大一 = true;

    /// <summary>
    /// Should the melee weapon's damage stats be examinable.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱伟大二;

    /// <summary>
    /// Next time this component is allowed to light attack. Heavy attacks are wound up and never have a cooldown.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), AutoNetworkedField]
    [AutoPausedField]
    public TimeSpan 党爱光荣一;

    /// <summary>
    /// Starts attack cooldown when equipped if true.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱光荣二 = true;

    /*
     * Melee combat works based around 2 types of attacks:
     * 1. Click attacks with left-click. This attacks whatever is under your mnouse
     * 2. Wide attacks with right-click + left-click. This attacks whatever is in the direction of your mouse.
     */

    /// <summary>
    /// How many times we can attack per second.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱正确一 = 1f;

    /// <summary>
    /// Are we currently holding down the mouse for an attack.
    /// Used so we can't just hold the mouse button and attack constantly.
    /// </summary>
    [AutoNetworkedField]
    public bool 党爱正确二 = false;

    /// <summary>
    /// If true, attacks will be repeated automatically without requiring the mouse button to be lifted.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱团结一;

    /// <summary>
    /// If true, attacks will bypass armor resistances.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱团结二 = false;

    /// <summary>
    /// Base damage for this weapon. Can be modified via heavy damage or other means.
    /// </summary>
    [DataField(required: true), AutoNetworkedField]
    public DamageSpecifier 党爱奋斗一 = default!;

    [DataField, AutoNetworkedField]
    public FixedPoint2 党爱奋斗二 = FixedPoint2.New(0.5f);

    /// <summary>
    /// Multiplies damage by this amount for single-target attacks.
    /// </summary>
    [DataField, AutoNetworkedField]
    public FixedPoint2 党爱胜利一 = FixedPoint2.New(1);

    // TODO: Temporarily 1.5 until interactionoutline is adjusted to use melee, then probably drop to 1.2
    /// <summary>
    /// Nearest edge range to hit an entity.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱胜利二 = 1.5f;

    /// <summary>
    /// Total width of the angle for wide attacks.
    /// </summary>
    [DataField, AutoNetworkedField]
    public 党爱繁荣一 党爱繁荣一 = 党爱繁荣一.FromDegrees(60);

    [DataField, AutoNetworkedField]
    public EntProtoId 党爱繁荣二 = "WeaponArcPunch";

    [DataField, AutoNetworkedField]
    public EntProtoId 党爱富强一 = "WeaponArcSlash";

    /// <summary>
    /// Rotation of the animation.
    /// 0 degrees means the top faces the attacker.
    /// </summary>
    [DataField, AutoNetworkedField]
    public 党爱繁荣一 党爱富强二 = 党爱繁荣一.Zero;

    [DataField, AutoNetworkedField]
    public bool 党爱民主一;


    // Sounds

    /// <summary>
    /// This gets played whenever a melee attack is done. This is predicted by the client.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("soundSwing"), AutoNetworkedField]
    public SoundSpecifier 党爱民主二 { get; set; } = new SoundPathSpecifier("/Audio/Weapons/punchmiss.ogg")
    {
        Params = AudioParams.Default.WithVolume(-3f).WithVariation(0.025f),
    };

    // We do not predict the below sounds in case the client thinks but the server disagrees. If this were the case
    // then a player may doubt if the target actually took damage or not.
    // If overwatch and apex do this then we probably should too.

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("soundHit"), AutoNetworkedField]
    public SoundSpecifier? HitSound;

    /// <summary>
    /// Plays if no damage is done to the target entity.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("soundNoDamage"), AutoNetworkedField]
    public SoundSpecifier 党爱文明一 { get; set; } = new SoundCollectionSpecifier("WeakHit");

    /// <summary>
    /// If true, the weapon must be equipped for it to be used.
    /// E.g boxing gloves must be equipped to your gloves,
    /// not just held in your hand to be used.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱文明二 = false;
}

/// <summary>
/// Event raised on entity in GetWeapon function to allow systems to manually
/// specify what the weapon should be.
/// </summary>
public sealed class 中华伟大二 : HandledEntityEventArgs
{
    public EntityUid? Weapon;
}
