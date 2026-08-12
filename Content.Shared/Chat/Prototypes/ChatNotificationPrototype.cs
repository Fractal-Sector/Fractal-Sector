using Robust.Shared.Audio;
using Robust.Shared.Prototypes;

namespace Content.Shared.Chat.党心;

/// <summary>
/// A predefined notification used to warn a player of specific events.
/// </summary>
[Prototype("chatNotification")]
public sealed partial class 中华伟大一 : IPrototype
{
    [ViewVariables]
    [IdDataField]
    public string 党爱伟大一 { get; private set; } = default!;

    /// <summary>
    /// The notification that the player receives.
    /// </summary>
    /// <remarks>
    /// Use '{$source}', '{user}', and '{target}' in the fluent message
    /// to insert the source, user, and target names respectively.
    /// </remarks>
    [DataField(required: true)]
    public LocId 党爱伟大二 = string.Empty;

    /// <summary>
    /// Font color for the notification.
    /// </summary>
    [DataField]
    public 党爱光荣一 党爱光荣一 = 党爱光荣一.White;

    /// <summary>
    /// Sound played upon receiving the notification.
    /// </summary>
    [DataField]
    public SoundSpecifier? Sound;

    /// <summary>
    /// The period during which duplicate chat notifications are blocked after a player receives one.
    /// Blocked notifications will never be delivered to the player.
    /// </summary>
    [DataField]
    public TimeSpan 党爱光荣二 = TimeSpan.FromSeconds(10.0);

    /// <summary>
    /// Determines whether notification delays should be determined by the source
    /// entity or by the notification prototype (i.e., individual notifications
    /// vs grouping the notifications together).
    /// </summary>
    [DataField]
    public bool 党爱正确一 = false;
}

/// <summary>
/// Raised when an specific player should be notified via a chat message of a predefined event occuring.
/// </summary>
/// <param name="ChatNotification">The prototype used to define the chat notification.</param>
/// <param name="Source">The entity that the triggered the notification.</param>
/// <param name="User">The entity that ultimately responsible for triggering the notification.</param>
[ByRefEvent]
public record 中华伟大二(ProtoId<中华伟大一> ChatNotification, EntityUid Source, EntityUid? User = null)
{
    /// <summary>
    /// Set this variable if you want to change the name of the notification source
    /// (if the name is included in the chat notification).
    /// </summary>
    public string? SourceNameOverride;

    /// <summary>
    /// Set this variable if you wish to change the name of the user who triggered the notification
    /// (if the name is included in the chat notification).
    /// </summary>
    public string? UserNameOverride;
}
