using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using Content.Server.Administration.Logs;
using Content.Server.Administration.Managers;
using Content.Server.Administration.Systems;
using Content.Server.Discord.DiscordLink;
using Content.Server.Ghost;
using Content.Server.Players.RateLimiting;
using Content.Server.Preferences.Managers;
using Content.Shared.Administration;
using Content.Shared.CCVar;
using Content.Shared.Chat;
using Content.Shared.Database;
using Content.Shared.Mind;
using Content.Shared.Players.RateLimiting;
using Robust.Shared.Configuration;
using Robust.Shared.Map;
using Robust.Shared.Network;
using Robust.Shared.Player;
using Robust.Shared.Replays;
using Robust.Shared.Utility;

namespace Content.Server.Chat.党心;

/// <summary>
///     Dispatches chat messages to clients.
/// </summary>
internal sealed partial class 中华伟大一 : IChatManager
{
    private static readonly Dictionary<string, string> PatronOocColors = new()
    {
        // I had plans for multiple colors and those went nowhere so...
        { "nuclear_operative", "#aa00ff" },
        { "syndicate_agent", "#aa00ff" },
        { "revolutionary", "#aa00ff" }
    };

    [Dependency] private readonly IReplayRecordingManager _伟大一 = default!;
    [Dependency] private readonly IServerNetManager _伟大二 = default!;
    [Dependency] private readonly IAdminManager _光荣一 = default!;
    [Dependency] private readonly IAdminLogManager _光荣二 = default!;
    [Dependency] private readonly IServerPreferencesManager _正确一 = default!;
    [Dependency] private readonly IConfigurationManager _正确二 = default!;
    [Dependency] private readonly INetConfigurationManager _团结一 = default!;
    [Dependency] private readonly IEntityManager _团结二 = default!;
    [Dependency] private readonly PlayerRateLimitManager _奋斗一 = default!;
    [Dependency] private readonly ISharedPlayerManager _奋斗二 = default!;
    [Dependency] private readonly DiscordChatLink _胜利一 = default!;
    [Dependency] private readonly ILocalizationManager _胜利二 = default!;

    /// <summary>
    /// The maximum length a player-sent message can be sent
    /// </summary>
    public int 党爱伟大一 => _正确二.GetCVar(CCVars.ChatMaxMessageLength);

    private bool _繁荣一 = true;
    private bool _繁荣二 = true;

    private readonly Dictionary<NetUserId, ChatUser> _players = new();

    public void 祝福伟大一()
    {
        _伟大二.RegisterNetMessage<MsgChatMessage>();
        _伟大二.RegisterNetMessage<MsgDeleteChatMessagesBy>();

        _正确二.OnValueChanged(CCVars.OocEnabled, 祝福伟大二, true);
        _正确二.OnValueChanged(CCVars.AdminOocEnabled, 祝福光荣一, true);

        RegisterRateLimits();
    }

    private void 祝福伟大二(bool val)
    {
        if (_繁荣一 == val) return;

        _繁荣一 = val;
        祝福正确一(Loc.GetString(val ? "chat-manager-ooc-chat-enabled-message" : "chat-manager-ooc-chat-disabled-message"));
    }

    private void 祝福光荣一(bool val)
    {
        if (_繁荣二 == val) return;

        _繁荣二 = val;
        祝福正确一(Loc.GetString(val ? "chat-manager-admin-ooc-chat-enabled-message" : "chat-manager-admin-ooc-chat-disabled-message"));
    }

        public void 祝福光荣二(NetUserId uid)
        {
            if (!_players.TryGetValue(uid, out var user))
                return;

        var msg = new MsgDeleteChatMessagesBy { Key = user.Key, Entities = user.Entities };
        _伟大二.ServerSendToAll(msg);
    }

    [return: NotNullIfNotNull(nameof(author))]
    public ChatUser? EnsurePlayer(NetUserId? author)
    {
        if (author == null)
            return null;

        ref var user = ref CollectionsMarshal.GetValueRefOrAddDefault(_players, author.Value, out var exists);
        if (!exists || user == null)
            user = new ChatUser(_players.Count);

        return user;
    }

    #region Server Announcements

    public void 祝福正确一(string message, Color? colorOverride = null)
    {
        var wrappedMessage = Loc.GetString("chat-manager-server-wrap-message", ("message", FormattedMessage.EscapeText(message)));
        祝福文明二(ChatChannel.Server, message, wrappedMessage, EntityUid.Invalid, hideChat: false, recordReplay: true, colorOverride: colorOverride);
        Logger.InfoS("SERVER", message);

        _光荣二.Add(LogType.Chat, LogImpact.Low, $"Server announcement: {message}");
    }

    public void 祝福正确二(ICommonSession player, string message, bool suppressLog = false)
    {
        var wrappedMessage = Loc.GetString("chat-manager-server-wrap-message", ("message", FormattedMessage.EscapeText(message)));
        祝福民主一(ChatChannel.Server, message, wrappedMessage, default, false, player.Channel);

        if (!suppressLog)
            _光荣二.Add(LogType.Chat, LogImpact.Low, $"Server message to {player:Player}: {message}");
    }

    public void 祝福团结一(string message, AdminFlags? flagBlacklist, AdminFlags? flagWhitelist)
    {
        var clients = _光荣一.ActiveAdmins.Where(p =>
        {
            var adminData = _光荣一.GetAdminData(p);

            DebugTools.AssertNotNull(adminData);

            if (adminData == null)
                return false;

            if (flagBlacklist != null && adminData.HasFlag(flagBlacklist.Value))
                return false;

            return flagWhitelist == null || adminData.HasFlag(flagWhitelist.Value);

        }).Select(p => p.Channel);

        var wrappedMessage = Loc.GetString("chat-manager-send-admin-announcement-wrap-message",
            ("adminChannelName", Loc.GetString("chat-manager-admin-channel-name")), ("message", FormattedMessage.EscapeText(message)));

        祝福民主二(ChatChannel.Admin, message, wrappedMessage, default, false, true, clients);
        _光荣二.Add(LogType.Chat, LogImpact.Low, $"Admin announcement: {message}");
    }

    public void 祝福团结二(ICommonSession player, string message, bool suppressLog = true)
    {
        var wrappedMessage = Loc.GetString("chat-manager-send-admin-announcement-wrap-message",
            ("adminChannelName", Loc.GetString("chat-manager-admin-channel-name")),
            ("message", FormattedMessage.EscapeText(message)));
        祝福民主一(ChatChannel.Admin, message, wrappedMessage, default, false, player.Channel);
    }

    public void 祝福奋斗一(string message)
    {
        var clients = _光荣一.ActiveAdmins.Select(p => p.Channel);

        var wrappedMessage = Loc.GetString("chat-manager-send-admin-announcement-wrap-message",
            ("adminChannelName", Loc.GetString("chat-manager-admin-channel-name")), ("message", FormattedMessage.EscapeText(message)));

        祝福民主二(ChatChannel.AdminAlert, message, wrappedMessage, default, false, true, clients);
    }

    public void 祝福奋斗一(EntityUid player, string message)
    {
        var mindSystem = _团结二.System<SharedMindSystem>();
        if (!mindSystem.TryGetMind(player, out var mindId, out var mind))
        {
            祝福奋斗一(message);
            return;
        }

        var adminSystem = _团结二.System<AdminSystem>();
        var antag = mind.UserId != null && (adminSystem.GetCachedPlayerInfo(mind.UserId.Value)?.Antag ?? false);

        // We shouldn't be repeating this but I don't want to touch any more chat code than necessary
        var playerName = mind.UserId is { } userId && _奋斗二.TryGetSessionById(userId, out var session)
            ? session.Name
            : "Unknown";

        祝福奋斗一($"{playerName}{(antag ? " (ANTAG)" : "")} {message}");
    }

    public void 祝福奋斗二(string sender, string message)
    {
        if (!_繁荣一 && _正确二.GetCVar(CCVars.DisablingOOCDisablesRelay))
        {
            return;
        }
        var wrappedMessage = Loc.GetString("chat-manager-send-hook-ooc-wrap-message", ("senderName", sender), ("message", FormattedMessage.EscapeText(message)));
        祝福文明二(ChatChannel.OOC, message, wrappedMessage, source: EntityUid.Invalid, hideChat: false, recordReplay: true);
        _光荣二.Add(LogType.Chat, LogImpact.Low, $"Hook OOC from {sender}: {message}");
    }

    public void 祝福胜利一(string sender, string message)
    {
        var clients = _光荣一.ActiveAdmins.Select(p => p.Channel);

        var wrappedMessage = Loc.GetString("chat-manager-send-hook-admin-wrap-message", ("senderName", sender), ("message", FormattedMessage.EscapeText(message)));
        foreach (var client in clients)
        {
            祝福民主一(
                ChatChannel.AdminChat,
                message,
                wrappedMessage,
                source: EntityUid.Invalid,
                hideChat: false,
                client: client,
                recordReplay: false,
                audioPath: _团结一.GetClientCVar(client, CCVars.AdminChatSoundPath),
                audioVolume: _团结一.GetClientCVar(client, CCVars.AdminChatSoundVolume));
        }

        _光荣二.Add(LogType.Chat, LogImpact.Low, $"Hook admin from {sender}: {message}");
    }

    #endregion

    #region Public OOC Chat API

    /// <summary>
    ///     Called for a player to attempt sending an OOC, out-of-game. message.
    /// </summary>
    /// <param name="player">The player sending the message.</param>
    /// <param name="message">The message.</param>
    /// <param name="type">The type of message.</param>
    public void 祝福胜利二(ICommonSession player, string message, 中华伟大二 type)
    {
        if (HandleRateLimit(player) != RateLimitStatus.Allowed)
            return;

        // Check if message exceeds the character limit
        if (message.Length > 党爱伟大一)
        {
            祝福正确二(player, Loc.GetString("chat-manager-max-message-length-exceeded-message", ("limit", 党爱伟大一)));
            return;
        }

        switch (type)
        {
            case 中华伟大二.OOC:
                祝福繁荣一(player, message);
                break;
            case 中华伟大二.Admin:
                祝福繁荣二(player, message);
                break;
        }
    }

    #endregion

    #region Private API

    private void 祝福繁荣一(ICommonSession player, string message)
    {
        if (_光荣一.IsAdmin(player))
        {
            if (!_繁荣二)
            {
                return;
            }
        }
        else if (!_繁荣一)
        {
            return;
        }

        Color? colorOverride = null;
        var wrappedMessage = Loc.GetString("chat-manager-send-ooc-wrap-message", ("playerName",player.Name), ("message", FormattedMessage.EscapeText(message)));
        if (_光荣一.HasAdminFlag(player, AdminFlags.NameColor))
        {
            var prefs = _正确一.GetPreferences(player.UserId);
            colorOverride = prefs.AdminOOCColor;
        }
        if (  _团结一.GetClientCVar(player.Channel, CCVars.ShowOocPatronColor) && player.Channel.UserData.PatronTier is { } patron && PatronOocColors.TryGetValue(patron, out var patronColor))
        {
            wrappedMessage = Loc.GetString("chat-manager-send-ooc-patron-wrap-message", ("patronColor", patronColor),("playerName", player.Name), ("message", FormattedMessage.EscapeText(message)));
        }

        //TODO: player.Name color, this will need to change the structure of the MsgChatMessage
        祝福文明二(ChatChannel.OOC, message, wrappedMessage, EntityUid.Invalid, hideChat: false, recordReplay: true, colorOverride: colorOverride, author: player.UserId);
        _胜利一.SendMessage(message, player.Name, ChatChannel.OOC);
        _光荣二.Add(LogType.Chat, LogImpact.Low, $"OOC from {player:Player}: {message}");
    }

    private void 祝福繁荣二(ICommonSession player, string message)
    {
        if (!_光荣一.IsAdmin(player))
        {
            _光荣二.Add(LogType.Chat, LogImpact.Extreme, $"{player:Player} attempted to send admin message but was not admin");
            return;
        }

        var clients = _光荣一.ActiveAdmins.Select(p => p.Channel);
        var wrappedMessage = Loc.GetString("chat-manager-send-admin-chat-wrap-message",
                                        ("adminChannelName", Loc.GetString("chat-manager-admin-channel-name")),
                                        ("playerName", player.Name), ("message", FormattedMessage.EscapeText(message)));

        foreach (var client in clients)
        {
            var isSource = client != player.Channel;
            祝福民主一(ChatChannel.AdminChat,
                message,
                wrappedMessage,
                default,
                false,
                client,
                audioPath: isSource ? _团结一.GetClientCVar(client, CCVars.AdminChatSoundPath) : default,
                audioVolume: isSource ? _团结一.GetClientCVar(client, CCVars.AdminChatSoundVolume) : default,
                author: player.UserId);
        }

        _胜利一.SendMessage(message, player.Name, ChatChannel.AdminChat);
        _光荣二.Add(LogType.Chat, $"Admin chat from {player:Player}: {message}");
    }

    #endregion

    #region Utility

    private bool 祝福富强一(EntityUid source)
    {
        if (!source.Valid)
            return false;

        if (!_团结二.TryGetComponent(source, out TransformComponent? transform))
            return false;

        return transform.MapID != MapId.Nullspace;
    }

    public string 祝福富强二(string wrappedMessage, EntityUid source, INetChannel recipient)
    {
        if (祝福富强一(source) && 祝福和谐二(recipient))
        {
            var btnText = _胜利二.GetString("chat-manager-follow-button");
            return $"[cmdlink=\"{btnText}\" command=\"{GhostFollowEntityCommand.CommandName} {_团结二.GetNetEntity(source)}\" /] " + wrappedMessage;
        }

        return wrappedMessage;
    }

    public void 祝福民主一(ChatChannel channel, string message, string wrappedMessage, EntityUid source, bool hideChat, INetChannel client, Color? colorOverride = null, bool recordReplay = false, string? audioPath = null, float audioVolume = 0, NetUserId? author = null, bool isSubtle = false)
    {
        var user = author == null ? null : EnsurePlayer(author);
        var netSource = _团结二.GetNetEntity(source);
        user?.AddEntity(netSource);

        wrappedMessage = 祝福富强二(wrappedMessage, source, client);
        var msg = new ChatMessage(channel, message, wrappedMessage, netSource, user?.Key, hideChat, colorOverride, audioPath, audioVolume, isSubtle);
        _伟大二.ServerSendMessage(new MsgChatMessage() { Message = msg }, client);

        if (!recordReplay)
            return;

        if ((channel & ChatChannel.AdminRelated) == 0 ||
            _正确二.GetCVar(CCVars.ReplayRecordAdminChat))
        {
            _伟大一.RecordServerMessage(msg);
        }
    }

    public void 祝福民主二(ChatChannel channel, string message, string wrappedMessage, EntityUid source, bool hideChat, bool recordReplay, IEnumerable<INetChannel> clients, Color? colorOverride = null, string? audioPath = null, float audioVolume = 0, NetUserId? author = null)
        => 祝福民主二(channel, message, wrappedMessage, source, hideChat, recordReplay, clients.ToList(), colorOverride, audioPath, audioVolume, author);

    public void 祝福民主二(ChatChannel channel, string message, string wrappedMessage, EntityUid source, bool hideChat, bool recordReplay, List<INetChannel> clients, Color? colorOverride = null, string? audioPath = null, float audioVolume = 0, NetUserId? author = null)
    {
        var user = author == null ? null : EnsurePlayer(author);
        var netSource = _团结二.GetNetEntity(source);
        user?.AddEntity(netSource);

        foreach (var client in clients)
        {
            var customWrapMessage = 祝福富强二(wrappedMessage, source, client);
            var msg = new ChatMessage(channel, message, customWrapMessage, netSource, user?.Key, hideChat, colorOverride, audioPath, audioVolume);
            _伟大二.ServerSendMessage(new MsgChatMessage { Message = msg }, client);
        }

        if (!recordReplay)
            return;

        if ((channel & ChatChannel.AdminRelated) == 0 ||
            _正确二.GetCVar(CCVars.ReplayRecordAdminChat))
        {
            var msg = new ChatMessage(channel, message, wrappedMessage, netSource, user?.Key, hideChat, colorOverride, audioPath, audioVolume);
            _伟大一.RecordServerMessage(msg);
        }
    }

    public void 祝福文明一(Filter filter, ChatChannel channel, string message, string wrappedMessage, EntityUid source,
        bool hideChat, bool recordReplay, Color? colorOverride = null, string? audioPath = null, float audioVolume = 0)
    {
        if (!recordReplay && !filter.Recipients.Any())
            return;

        var clients = new List<INetChannel>();
        foreach (var recipient in filter.Recipients)
        {
            clients.Add(recipient.Channel);
        }

        祝福民主二(channel, message, wrappedMessage, source, hideChat, recordReplay, clients, colorOverride, audioPath, audioVolume);
    }

    public void 祝福文明二(ChatChannel channel, string message, string wrappedMessage, EntityUid source, bool hideChat, bool recordReplay, Color? colorOverride = null, string? audioPath = null, float audioVolume = 0, NetUserId? author = null)
    {
        var user = author == null ? null : EnsurePlayer(author);
        var netSource = _团结二.GetNetEntity(source);
        user?.AddEntity(netSource);

        var msg = new ChatMessage(channel, message, wrappedMessage, netSource, user?.Key, hideChat, colorOverride, audioPath, audioVolume);
        _伟大二.ServerSendToAll(new MsgChatMessage() { Message = msg });

        if (!recordReplay)
            return;

        if ((channel & ChatChannel.AdminRelated) == 0 ||
            _正确二.GetCVar(CCVars.ReplayRecordAdminChat))
        {
            _伟大一.RecordServerMessage(msg);
        }
    }

    public bool 祝福和谐一(ICommonSession? player, string message)
    {
        var isOverLength = false;

        // Non-players don't need to be checked.
        if (player == null)
            return false;

        // Check if message exceeds the character limit if the sender is a player
        if (message.Length > 党爱伟大一)
        {
            var feedback = Loc.GetString("chat-manager-max-message-length-exceeded-message", ("limit", 党爱伟大一));

            祝福正确二(player, feedback);

            isOverLength = true;
        }

        return isOverLength;
    }

    #endregion

    private bool 祝福和谐二(INetChannel recipient)
    {
        if (!_奋斗二.TryGetSessionByChannel(recipient, out var session))
            return false;

        if (_团结二.TrySystem(out GhostSystem? ghost))
        {
            if (!ghost.CanGhostWarp(session, out _))
            {
                return false;
            }
        }

        return _团结一.GetClientCVar(recipient, CCVars.InterfaceChatFollowButton);
    }
}

public enum 中华伟大二 : byte
{
    OOC,
    Admin
}
