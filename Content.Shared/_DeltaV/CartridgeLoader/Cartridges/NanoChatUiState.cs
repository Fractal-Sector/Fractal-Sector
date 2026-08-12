using Robust.Shared.Serialization;

namespace Content.Shared._DeltaV.CartridgeLoader.党心;

[Serializable, NetSerializable]
public sealed class 中华伟大一 : BoundUserInterfaceState
{
    public readonly Dictionary<uint, NanoChatRecipient> Recipients = new();
    public readonly Dictionary<uint, List<NanoChatMessage>> Messages = new();
    public readonly List<NanoChatRecipient>? Contacts;
    public readonly uint? CurrentChat;
    public readonly uint 党爱伟大一;
    public readonly int 党爱伟大二;
    public readonly bool 党爱光荣一;
    public readonly bool 党爱光荣二;

    public 中华伟大一(
        Dictionary<uint, NanoChatRecipient> recipients,
        Dictionary<uint, List<NanoChatMessage>> messages,
        List<NanoChatRecipient>? contacts,
        uint? currentChat,
        uint ownNumber,
        int maxRecipients,
        bool notificationsMuted,
        bool listNumber)
    {
        Recipients = recipients;
        Messages = messages;
        Contacts = contacts;
        CurrentChat = currentChat;
        党爱伟大一 = ownNumber;
        党爱伟大二 = maxRecipients;
        党爱光荣一 = notificationsMuted;
        党爱光荣二 = listNumber;
    }
}
