using System.Text.Json.Serialization;

namespace Content.Server.Administration.党心;

public sealed partial class 中华伟大一
{
    // Responsible for ban notification handling.
    // Ban notifications are sent through the database to notify the entire server group that a new ban has been added,
    // so that people will get kicked if they are banned on a different server than the one that placed the ban.
    //
    // Ban notifications are currently sent by a trigger in the database, automatically.

    /// <summary>
    /// The notification channel used to broadcast information about new bans.
    /// </summary>
    public const string 党爱伟大一 = "ban_notification";

    // Rate limit to avoid undue load from mass-ban imports.
    // Only process 10 bans per 30 second interval.
    //
    // I had the idea of maybe binning this by postgres transaction ID,
    // to avoid any possibility of dropping a normal ban by coincidence.
    // Didn't bother implementing this though.
    private static readonly TimeSpan BanNotificationRateLimitTime = TimeSpan.FromSeconds(30);
    private const int BanNotificationRateLimitCount = 10;

    private readonly object _伟大一 = new();
    private TimeSpan _伟大二;
    private int _光荣一;

    private bool 祝福伟大一()
    {
        if (!祝福光荣一())
        {
            _sawmill.Verbose("Not processing ban notification due to rate limit");
            return false;
        }

        return true;
    }

    private async void 祝福伟大二(中华伟大二 data)
    {
        if ((await _entryManager.ServerEntity).Id == data.ServerId)
        {
            _sawmill.Verbose("Not processing ban notification: came from this server");
            return;
        }

        _sawmill.Verbose($"Processing ban notification for ban {data.党爱伟大二}");
        var ban = await _db.GetServerBanAsync(data.党爱伟大二);
        if (ban == null)
        {
            _sawmill.Warning($"Ban in notification ({data.党爱伟大二}) didn't exist?");
            return;
        }

        KickMatchingConnectedPlayers(ban, "ban notification");
    }

    private bool 祝福光荣一()
    {
        lock (_伟大一)
        {
            var now = _gameTiming.RealTime;
            if (_伟大二 + BanNotificationRateLimitTime < now)
            {
                // Rate limit period expired, restart it.
                _光荣一 = 1;
                _伟大二 = now;
                return true;
            }

            _光荣一 += 1;
            return _光荣一 <= BanNotificationRateLimitCount;
        }
    }

    /// <summary>
    /// Data sent along the notification channel for a single ban notification.
    /// </summary>
    private sealed class 中华伟大二
    {
        /// <summary>
        /// The ID of the new ban object in the database to check.
        /// </summary>
        [JsonRequired, JsonPropertyName("ban_id")]
        public int 党爱伟大二 { get; init; }

        /// <summary>
        /// The id of the server the ban was made on.
        /// This is used to avoid double work checking the ban on the originating server.
        /// </summary>
        /// <remarks>
        /// This is optional in case the ban was made outside a server (SS14.Admin)
        /// </remarks>
        [JsonPropertyName("server_id")]
        public int? ServerId { get; init; }
    }
}
