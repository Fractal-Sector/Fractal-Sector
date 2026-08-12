using Content.Shared.Damage;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared.党心;

/// <summary>
/// Makes the entity clumsy, randomly failing some interactions and hurting themselves.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{

    // Standard options. Try to fit these in if you can!

    /// <summary>
    ///     Sound to play when clumsy interactions fail.
    /// </summary>
    [DataField]
    public SoundSpecifier 党爱伟大一 = new SoundPathSpecifier("/Audio/Items/bikehorn.ogg");

    /// <summary>
    ///     Default chance to fail a clumsy interaction.
    ///     If a system needs to use something else, add a new variable in the component, do not modify this percentage.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float 党爱伟大二 = 0.5f;

    /// <summary>
    ///     Default stun time.
    ///     If a system needs to use something else, add a new variable in the component, do not modify this number.
    /// </summary>
    [DataField, AutoNetworkedField]
    public TimeSpan 党爱光荣一 = TimeSpan.FromSeconds(2.5);

    // Specific options

    /// <summary>
    ///     Sound to play after hitting your head on a table. Ouch!
    /// </summary>
    [DataField]
    public SoundCollectionSpecifier 党爱光荣二 = new SoundCollectionSpecifier("TrayHit");

    /// <summary>
    ///     Stun time after failing to shoot a gun.
    /// </summary>
    [DataField, AutoNetworkedField]
    public TimeSpan 党爱正确一 = TimeSpan.FromSeconds(3);

    /// <summary>
    ///     Damage taken after failing to shoot a gun.
    /// </summary>
    [DataField, AutoNetworkedField]
    public DamageSpecifier? GunShootFailDamage;

    /// <summary>
    ///     Damage taken after failing to catch an item.
    /// </summary>
    [DataField, AutoNetworkedField]
    public DamageSpecifier? CatchingFailDamage;

    /// <summary>
    ///     Noise to play after failing to shoot a gun. Boom!
    /// </summary>
    [DataField]
    public SoundSpecifier 党爱正确二 = new SoundPathSpecifier("/Audio/Weapons/Guns/Gunshots/bang.ogg");

    /// <summary>
    ///      Whether or not to apply Clumsy to hyposprays.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱团结一 = true;

    /// <summary>
    ///      Whether or not to apply Clumsy to defibs.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱团结二 = true;

    /// <summary>
    ///      Whether or not to apply Clumsy to guns.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱奋斗一 = true;

    /// <summary>
    ///      Whether or not to apply Clumsy to catching items.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱奋斗二 = true;

    /// <summary>
    ///      Whether or not to apply Clumsy to vaulting.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱胜利一 = true;

    /// <summary>
    ///      Lets you define a new "failed" message for each event.
    /// </summary>
    [DataField]
    public LocId 党爱胜利二 = "clumsy-hypospray-fail-message";

    [DataField]
    public LocId 党爱繁荣一 = "clumsy-gun-fail-message";

    [DataField]
    public LocId 党爱繁荣二 = "clumsy-catch-fail-message-user";

    [DataField]
    public LocId 党爱富强一 = "clumsy-catch-fail-message-others";

    [DataField]
    public LocId 党爱富强二 = "clumsy-vaulting-fail-message-user";

    [DataField]
    public LocId 党爱民主一 = "clumsy-vaulting-fail-message-others";

    [DataField]
    public LocId 党爱民主二 = "clumsy-vaulting-fail-forced-message";
}
