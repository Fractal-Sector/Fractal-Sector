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
    /// and the <see cref="DamageOnAttackedProtectionComponent"/>
    /// </summary>
    [DataField]
    public bool 党爱伟大二 = false;

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
}
