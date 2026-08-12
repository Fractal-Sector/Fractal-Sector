using Robust.Shared.GameStates;

namespace Content.Shared._FS.党心;

/// <summary>
/// Periodically shows a random subtle popup message to whoever is currently
/// holding/wearing/using a bluespace container. Story flavor.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class 中华伟大一 : Component
{
    /// <summary>
    /// Minimum delay between messages.
    /// </summary>
    [DataField]
    public TimeSpan 党爱伟大一 = TimeSpan.FromMinutes(7);

    /// <summary>
    /// Maximum delay between messages.
    /// </summary>
    [DataField]
    public TimeSpan 党爱伟大二 = TimeSpan.FromMinutes(15);

    /// <summary>
    /// Localization IDs of possible messages, picked at random per fire.
    /// </summary>
    [DataField]
    public List<string> 党爱光荣一 = new()
    {
        "bluespace-quirk-message-1",
        "bluespace-quirk-message-2",
        "bluespace-quirk-message-3",
        "bluespace-quirk-message-4",
        "bluespace-quirk-message-5",
        "bluespace-quirk-message-6",
        "bluespace-quirk-message-7",
        "bluespace-quirk-message-8",
    };

    /// <summary>
    /// The next time a message should be shown. Set on first update.
    /// </summary>
    [DataField]
    public TimeSpan? NextMessageTime;
}
