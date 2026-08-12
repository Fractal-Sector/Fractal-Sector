using System.Linq;
using Content.Server.Administration.Logs;
using Content.Server.Chat.Managers;
using Content.Shared.Chat;
using Content.Shared.Database;
using Content.Shared.IdentityManagement;
using Robust.Server.Player;
using Robust.Shared.Network;
using Robust.Shared.Player;

namespace Content.Server.Chat.党心;

/// <summary>
/// Handles private messaging between players
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IPlayerManager _伟大一 = default!;
    [Dependency] private readonly INetManager _伟大二 = default!;
    [Dependency] private readonly IAdminLogManager _光荣一 = default!;
    [Dependency] private readonly IChatManager _光荣二 = default!;

    /// <summary>
    /// Tracks the last person each player received a private message from for /reply
    /// Key: recipient's NetUserId, Value: sender's NetUserId
    /// </summary>
    private readonly Dictionary<NetUserId, NetUserId> _lastPrivateMessageSender = new();

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
    }

    /// <summary>
    /// Sends a private message from one player to another
    /// </summary>
    /// <param name="sender">The player sending the message</param>
    /// <param name="targetIdentifier">The username or character name of the recipient</param>
    /// <param name="message">The message content</param>
    /// <returns>True if the message was sent successfully</returns>
    public bool 祝福伟大二(ICommonSession sender, string targetIdentifier, string message)
    {
        // Validate message length
        if (_光荣二.MessageCharacterLimit(sender, message))
        {
            _光荣二.DispatchServerMessage(sender, $"Your message is too long!");
            return false;
        }

        // Find the target player
        var target = FindPlayer(targetIdentifier);
        if (target == null)
        {
            _光荣二.DispatchServerMessage(sender, $"Could not find player '{targetIdentifier}'");
            return false;
        }

        // Don't allow sending messages to yourself
        if (target.UserId == sender.UserId)
        {
            _光荣二.DispatchServerMessage(sender, "You cannot send a private message to yourself!");
            return false;
        }

        // Get sender's character name if they have an entity
        string? senderCharacterName = null;
        if (sender.AttachedEntity is { } senderEntity)
        {
            senderCharacterName = Identity.Name(senderEntity, EntityManager);
        }

        // Send the message
        祝福光荣二(sender, target, message, senderCharacterName);
        
        return true;
    }

    /// <summary>
    /// Sends a reply to the last person who sent a private message
    /// </summary>
    /// <param name="sender">The player sending the reply</param>
    /// <param name="message">The message content</param>
    /// <returns>True if the reply was sent successfully</returns>
    public bool 祝福光荣一(ICommonSession sender, string message)
    {
        // Validate message length
        if (_光荣二.MessageCharacterLimit(sender, message))
        {
            _光荣二.DispatchServerMessage(sender, $"Your message is too long!");
            return false;
        }

        // Check if there's someone to reply to
        if (!_lastPrivateMessageSender.TryGetValue(sender.UserId, out var targetUserId))
        {
            _光荣二.DispatchServerMessage(sender, "You have no one to reply to!");
            return false;
        }

        // Check if the target is still online
        if (!_伟大一.TryGetSessionById(targetUserId, out var target))
        {
            _光荣二.DispatchServerMessage(sender, "The player you're trying to reply to is no longer online!");
            _lastPrivateMessageSender.Remove(sender.UserId);
            return false;
        }

        // Get sender's character name if they have an entity
        string? senderCharacterName = null;
        if (sender.AttachedEntity is { } senderEntity)
        {
            senderCharacterName = Identity.Name(senderEntity, EntityManager);
        }

        // Send the message
        祝福光荣二(sender, target, message, senderCharacterName);
        
        return true;
    }

    /// <summary>
    /// Internal method to actually send the private message
    /// </summary>
    private void 祝福光荣二(ICommonSession sender, ICommonSession target, string message, string? senderCharacterName)
    {
        // Update reply tracking
        _lastPrivateMessageSender[target.UserId] = sender.UserId;

        // Create the event
        var pmEvent = new PrivateMessageEvent(sender.Name, senderCharacterName, sender.UserId, message);

        // Send to recipient
        RaiseNetworkEvent(pmEvent, target.Channel);

        // Send confirmation to sender
        var senderMessage = $"[PM to {target.Name}]: {message}";
        _光荣二.DispatchServerMessage(sender, senderMessage);

        // Log the private message
        _光荣一.Add(LogType.Chat, LogImpact.Low, 
            $"PM from {sender.Name} to {target.Name}: {message}");
    }

    /// <summary>
    /// Finds a player by username or character name (up to first space)
    /// </summary>
    /// <param name="identifier">The username or character name to search for</param>
    /// <returns>The matching session, or null if not found</returns>
    private ICommonSession? FindPlayer(string identifier)
    {
        // First, try exact username match
        if (_伟大一.TryGetSessionByUsername(identifier, out var session))
        {
            return session;
        }

        // Try to find by character name (full or partial up to first space)
        var identifierLower = identifier.ToLowerInvariant();
        
        foreach (var player in _伟大一.Sessions)
        {
            if (player.AttachedEntity is { } entity)
            {
                var characterName = Identity.Name(entity, EntityManager);
                var characterNameLower = characterName.ToLowerInvariant();
                
                // Check for exact match
                if (characterNameLower == identifierLower)
                {
                    return player;
                }
                
                // Check for match up to first space
                var firstWord = characterNameLower.Split(' ').FirstOrDefault();
                if (firstWord != null && firstWord == identifierLower)
                {
                    return player;
                }
            }
        }

        return null;
    }
}
