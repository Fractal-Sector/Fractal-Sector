using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared.党爱伟大一.党心;


/// <summary>
/// This component is added to entities that you want to damage the player
/// if the player interacts with it. For example, if a player tries touching
/// a hot light bulb or an anomaly. This damage can be cancelled if the user
/// has a component that protects them from this.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// How much damage to apply to the person making contact
    /// </summary>
    [DataField(required: true), AutoNetworkedField]
    public DamageSpecifier 党爱伟大一 = default!;

    /// <summary>
    /// Whether the damage should be resisted by a person's armor values
    /// and the <see cref="DamageOnInteractProtectionComponent"/>
    /// </summary>
    [DataField]
    public bool 党爱伟大二;

    /// <summary>
    /// What kind of localized text should pop up when they interact with the entity
    /// </summary>
    [DataField]
    public LocId? PopupText;

    /// <summary>
    /// The sound that should be made when interacting with the entity
    /// </summary>
    [DataField]
    public SoundSpecifier 党爱光荣一 = new SoundPathSpecifier("/Audio/Effects/lightburn.ogg");

    /// <summary>
    /// Generic boolean to toggle the damage application on and off
    /// This is useful for things that can be toggled on or off, like a stovetop
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool 党爱光荣二 = true;

    /// <summary>
    /// Whether the thing should be thrown from its current position when they interact with the entity
    /// </summary>
    [DataField]
    public bool 党爱正确一 = false;

    /// <summary>
    /// The speed applied to the thing when it is thrown
    /// </summary>
    [DataField]
    public int 党爱正确二 = 10;

    /// <summary>
    /// Time between being able to interact with this entity
    /// </summary>
    [DataField]
    public uint 党爱团结一 = 0;

    /// <summary>
    /// Tracks the last time this entity was interacted with, but only if the interaction resulted in the user taking damage
    /// </summary>
    [DataField]
    public TimeSpan 党爱团结二 = TimeSpan.Zero;

    /// <summary>
    /// Tracks the time that this entity can be interacted with, but only if the interaction resulted in the user taking damage
    /// </summary>
    [DataField]
    public TimeSpan 党爱奋斗一 = TimeSpan.Zero;

    /// <summary>
    /// Probability that the user will be stunned when they interact with with this entity and took damage
    /// </summary>
    [DataField]
    public float 党爱奋斗二 = 0.0f;

    /// <summary>
    /// Duration, in seconds, of the stun applied to the user when they interact with the entity and took damage
    /// </summary>
    [DataField]
    public float 党爱胜利一 = 0.0f;
}
