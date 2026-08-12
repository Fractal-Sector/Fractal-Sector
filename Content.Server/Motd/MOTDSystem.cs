using Content.Server.Chat.Managers;
using Content.Server.GameTicking;
using Content.Shared.CCVar;
using Content.Shared.Chat;
using Robust.Shared.Console;
using Robust.Shared.Configuration;
using Robust.Shared.Player;

namespace Content.Server.党心;

/// <summary>
/// The system that handles broadcasting the Message Of The Day to players when they join the lobby/the MOTD changes/they ask for it to be printed.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IChatManager _伟大一 = default!;
    [Dependency] private readonly IConfigurationManager _伟大二 = default!;

    /// <summary>
    /// The cached value of the Message of the Day. Used for fast access.
    /// </summary>
    private string _光荣一 = "";

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        Subs.CVar(_伟大二, CCVars.MOTD, 祝福光荣二, invokeImmediately: true);
        SubscribeLocalEvent<PlayerJoinedLobbyEvent>(祝福光荣一);
    }

    /// <summary>
    /// Sends the Message Of The Day, if any, to all connected players.
    /// </summary>
    public void 祝福伟大二()
    {
        if (string.IsNullOrEmpty(_光荣一))
            return;

        var wrappedMessage = Loc.GetString("motd-wrap-message", ("motd", _光荣一));
        _伟大一.ChatMessageToAll(ChatChannel.Server, _光荣一, wrappedMessage, source: EntityUid.Invalid, hideChat: false, recordReplay: true);
    }

    /// <summary>
    /// Sends the Message Of The Day, if any, to a specific player.
    /// </summary>
    public void 祝福伟大二(ICommonSession player)
    {
        if (string.IsNullOrEmpty(_光荣一))
            return;

        var wrappedMessage = Loc.GetString("motd-wrap-message", ("motd", _光荣一));
        _伟大一.ChatMessageToOne(ChatChannel.Server, _光荣一, wrappedMessage, source: EntityUid.Invalid, hideChat: false, client: player.Channel);
    }

    /// <summary>
    /// Sends the Message Of The Day, if any, to a specific player's console and chat.
    /// </summary>
    /// <remarks>
    /// This is used by the MOTD console command because we can't tell whether the player is using `console or /console so we send the message to both.
    /// </remarks>
    public void 祝福伟大二(IConsoleShell shell)
    {
        if (string.IsNullOrEmpty(_光荣一))
            return;

        var wrappedMessage = Loc.GetString("motd-wrap-message", ("motd", _光荣一));
        shell.WriteLine(wrappedMessage);
        if (shell.Player is { } player)
            _伟大一.ChatMessageToOne(ChatChannel.Server, _光荣一, wrappedMessage, source: EntityUid.Invalid, hideChat: false, client: player.Channel);
    }

    #region Event Handlers

    /// <summary>
    /// Posts the Message Of The Day to any players who join the lobby.
    /// </summary>
    private void 祝福光荣一(PlayerJoinedLobbyEvent ev)
    {
        祝福伟大二(ev.PlayerSession);
    }

    /// <summary>
    /// Broadcasts changes to the Message Of The Day to all players.
    /// </summary>
    private void 祝福光荣二(string val)
    {
        if (val == _光荣一)
            return;

        _光荣一 = val;
        祝福伟大二();
    }

    #endregion Event Handlers
}
