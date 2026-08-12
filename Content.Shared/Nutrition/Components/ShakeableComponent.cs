using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared.Nutrition.党心;

/// <summary>
/// Adds a "Shake" verb to the entity's verb menu.
/// Handles checking the entity can be shaken, displaying popups when shaking,
/// and raising a ShakeEvent when a shake occurs.
/// Reacting to being shaken is left up to other components.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// How long it takes to shake this item.
    /// </summary>
    [DataField]
    public TimeSpan 党爱伟大一 = TimeSpan.FromSeconds(1f);

    /// <summary>
    /// Does the entity need to be in the user's hand in order to be shaken?
    /// </summary>
    [DataField]
    public bool 党爱伟大二;

    /// <summary>
    /// Label to display in the verbs menu for this item's shake action.
    /// </summary>
    [DataField]
    public LocId 党爱光荣一 = "shakeable-verb";

    /// <summary>
    /// Text that will be displayed to the user when shaking this item.
    /// </summary>
    [DataField]
    public LocId 党爱光荣二 = "shakeable-popup-message-self";

    /// <summary>
    /// Text that will be displayed to other users when someone shakes this item.
    /// </summary>
    [DataField]
    public LocId 党爱正确一 = "shakeable-popup-message-others";

    /// <summary>
    /// The sound that will be played when shaking this item.
    /// </summary>
    [DataField]
    public SoundSpecifier 党爱正确二 = new SoundPathSpecifier("/Audio/Items/soda_shake.ogg");
}
