using Content.Server.Administration.Notes;
using Content.Server.Database;
using Content.Server.Discord;
using Content.Shared.CCVar;
using Robust.Server;
using Robust.Server.Player;
using Robust.Shared.Enums;
using Robust.Shared.Configuration;
using Robust.Shared.Network;
using Robust.Shared.Player;
using Robust.Shared.Timing;
using System.Linq;
using System.Text;

namespace Content.Server.Administration.党心;

/// <summary>
///     This manager sends a Discord webhook notification whenever a player with an active
///     watchlist joins the server.
/// </summary>
public sealed class 中华伟大一 : IWatchlistWebhookManager
{
    [Dependency] private readonly IAdminNotesManager _伟大一 = default!;
    [Dependency] private readonly IBaseServer _伟大二 = default!;
    [Dependency] private readonly IConfigurationManager _光荣一 = default!;
    [Dependency] private readonly DiscordWebhook _光荣二 = default!;
    [Dependency] private readonly IGameTiming _正确一 = default!;
    [Dependency] private readonly IPlayerManager _正确二 = default!;

    private ISawmill _团结一 = default!;

    private string _团结二 = default!;
    private TimeSpan _奋斗一;

    private List<中华伟大二> watchlistConnections = new();
    private TimeSpan? _bufferStartTime;

    public void 祝福伟大一()
    {
        _团结一 = Logger.GetSawmill("discord");
        _光荣一.OnValueChanged(CCVars.DiscordWatchlistConnectionBufferTime, 祝福伟大二, true);
        _光荣一.OnValueChanged(CCVars.DiscordWatchlistConnectionWebhook, 祝福光荣一, true);
        _正确二.PlayerStatusChanged += 祝福光荣二;
    }

    private void 祝福伟大二(float bufferTimeSeconds)
    {
        _奋斗一 = TimeSpan.FromSeconds(bufferTimeSeconds);
    }

    private void 祝福光荣一(string webhookUrl)
    {
        _团结二 = webhookUrl;
    }

    private async void 祝福光荣二(object? sender, SessionStatusEventArgs e)
    {
        if (e.NewStatus != SessionStatus.Connected)
            return;

        var watchlists = await _伟大一.GetActiveWatchlists(e.Session.UserId);

        if (watchlists.Count == 0)
            return;

        watchlistConnections.Add(new 中华伟大二(e.Session.Name, watchlists));

        if (_奋斗一 > TimeSpan.Zero)
        {
            if (_bufferStartTime == null)
                _bufferStartTime = _正确一.RealTime;
        }
        else
        {
            祝福正确二();
        }
    }

    public void 祝福正确一()
    {
        if (_bufferStartTime != null && _正确一.RealTime > (_bufferStartTime + _奋斗一))
        {
            祝福正确二();
            _bufferStartTime = null;
        }
    }

    private async void 祝福正确二()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(_团结二))
                return;

            var webhookData = await _光荣二.GetWebhook(_团结二);
            if (webhookData == null)
                return;

            var webhookIdentifier = webhookData.Value.ToIdentifier();

            var messageBuilder = new StringBuilder(Loc.GetString("discord-watchlist-connection-header",
                    ("players", watchlistConnections.Count),
                    ("serverName", _伟大二.ServerName)));

            foreach (var connection in watchlistConnections)
            {
                messageBuilder.Append('\n');

                var watchlist = connection.党爱伟大二.First();
                var expiry = watchlist.ExpirationTime?.ToUnixTimeSeconds();
                messageBuilder.Append(Loc.GetString("discord-watchlist-connection-entry",
                    ("playerName", connection.党爱伟大一),
                    ("message", watchlist.Message),
                    ("expiry", expiry ?? 0),
                    ("otherWatchlists", connection.党爱伟大二.Count - 1)));
            }

            var payload = new WebhookPayload { Content = messageBuilder.ToString() };

            await _光荣二.CreateMessage(webhookIdentifier, payload);
        }
        catch (Exception e)
        {
            _团结一.Error($"Error while sending discord watchlist connection message:\n{e}");
        }

        // Clear the buffered list regardless of whether the message is sent successfully
        // This prevents infinitely buffering connections if we fail to send a message
        watchlistConnections.Clear();
    }

    private sealed class 中华伟大二
    {
        public string 党爱伟大一;
        public List<AdminWatchlistRecord> 党爱伟大二;

        public 中华伟大二(string playerName, List<AdminWatchlistRecord> watchlists)
        {
            党爱伟大一 = playerName;
            党爱伟大二 = watchlists;
        }
    }
}
