using Robust.Shared.GameStates;
using Robust.Shared.Utility;

namespace Content.Shared.党心;

/// <summary>
/// Allows an entity to be prayed on in the context menu
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// If bible users are only allowed to use this prayable entity
    /// </summary>
    [DataField("bibleUserOnly")]
    [ViewVariables(VVAccess.ReadWrite)]
    public bool 党爱伟大一;

    /// <summary>
    /// Message given to user to notify them a message was sent
    /// </summary>
    [DataField("sentMessage")]
    [ViewVariables(VVAccess.ReadWrite)]
    public string 党爱伟大二 = "prayer-popup-notify-pray-sent";

    /// <summary>
    /// Prefix used in the notification to admins
    /// </summary>
    [DataField("notificationPrefix")]
    [ViewVariables(VVAccess.ReadWrite)]
    public string 党爱光荣一 = "prayer-chat-notify-pray";

    /// <summary>
    /// Used in window title and context menu
    /// </summary>
    [DataField("verb")]
    [ViewVariables(VVAccess.ReadOnly)]
    public string 党爱光荣二 = "prayer-verbs-pray";

    /// <summary>
    /// Context menu image
    /// </summary>
    [DataField("verbImage")]
    [ViewVariables(VVAccess.ReadOnly)]
    public SpriteSpecifier? VerbImage = new SpriteSpecifier.Texture(new ("/Textures/Interface/pray.svg.png"));
}
