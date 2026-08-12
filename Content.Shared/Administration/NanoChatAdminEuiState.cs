using Content.Shared._DeltaV.CartridgeLoader.Cartridges;
using Content.Shared.Eui;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

/// <summary>
/// State for the admin NanoChat viewer EUI
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大一 : EuiStateBase
{
    /// <summary>
    /// List of all NanoChat cards in the game with their data
    /// </summary>
    public List<中华伟大二> Cards { get; set; } = new();
}

/// <summary>
/// Represents a NanoChat card and all its messages
/// </summary>
[Serializable, NetSerializable]
public sealed class 中华伟大二
{
    /// <summary>
    /// The entity ID of the card
    /// </summary>
    public NetEntity 党爱伟大一 { get; set; }

    /// <summary>
    /// The NanoChat number assigned to this card
    /// </summary>
    public uint? Number { get; set; }

    /// <summary>
    /// Name of the card owner (from ID card)
    /// </summary>
    public string 党爱伟大二 { get; set; } = "Unknown";

    /// <summary>
    /// Username of the player who currently owns/controls this card
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// Username of the original owner whose name is on the ID card (for detecting stolen PDA usage)
    /// </summary>
    public string? OriginalOwnerUsername { get; set; }

    /// <summary>
    /// Job title of the card owner
    /// </summary>
    public string? JobTitle { get; set; }

    /// <summary>
    /// All recipients on this card
    /// </summary>
    public Dictionary<uint, NanoChatRecipient> Recipients { get; set; } = new();

    /// <summary>
    /// All messages on this card, keyed by recipient number
    /// </summary>
    public Dictionary<uint, List<NanoChatMessage>> Messages { get; set; } = new();
}

/// <summary>
/// Messages for the NanoChat admin viewer
/// </summary>
public static class 中华光荣一
{
    /// <summary>
    /// Request to refresh the data
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class 中华光荣二 : EuiMessageBase
    {
    }

    /// <summary>
    /// Request to select a specific card
    /// </summary>
    [Serializable, NetSerializable]
    public sealed class 中华正确一 : EuiMessageBase
    {
        public NetEntity 党爱伟大一 { get; set; }
    }
}
