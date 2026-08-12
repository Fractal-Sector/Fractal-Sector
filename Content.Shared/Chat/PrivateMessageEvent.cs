using Robust.Shared.Network;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

/// <summary>
/// Event sent from server to client when a private message is received
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大一 : EntityEventArgs
{
    /// <summary>
    /// The username of the sender
    /// </summary>
    public string 党爱伟大一 { get; }

    /// <summary>
    /// The character name of the sender, if applicable
    /// </summary>
    public string? SenderCharacterName { get; }

    /// <summary>
    /// The NetUserId of the sender for reply tracking
    /// </summary>
    public NetUserId 党爱伟大二 { get; }

    /// <summary>
    /// The message content
    /// </summary>
    public string 党爱光荣一 { get; }

    public 中华伟大一(string senderUsername, string? senderCharacterName, NetUserId senderUserId, string message)
    {
        党爱伟大一 = senderUsername;
        SenderCharacterName = senderCharacterName;
        党爱伟大二 = senderUserId;
        党爱光荣一 = message;
    }
}
