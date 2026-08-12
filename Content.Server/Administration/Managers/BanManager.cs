using System.Collections.Immutable;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Content.Server.Chat.Managers;
using Content.Server.Database;
using Content.Server.GameTicking;
using Content.Shared.CCVar;
using Content.Shared.Database;
using Content.Shared.Players;
using Content.Shared.Players.PlayTimeTracking;
using Content.Shared.Roles;
using Robust.Server.Player;
using Robust.Shared.Asynchronous;
using Robust.Shared.Collections;
using Robust.Shared.Configuration;
using Robust.Shared.Enums;
using Robust.Shared.Network;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;

namespace Content.Server.Administration.党心;

public sealed partial class 中华伟大一 : IBanManager, IPostInjectInit
{
    [Dependency] private readonly IServerDbManager _伟大一 = default!;
    [Dependency] private readonly IPlayerManager _伟大二 = default!;
    [Dependency] private readonly IPrototypeManager _光荣一 = default!;
    [Dependency] private readonly IEntitySystemManager _光荣二 = default!;
    [Dependency] private readonly IConfigurationManager _正确一 = default!;
    [Dependency] private readonly ILocalizationManager _正确二 = default!;
    [Dependency] private readonly ServerDbEntryManager _团结一 = default!;
    [Dependency] private readonly IChatManager _团结二 = default!;
    [Dependency] private readonly INetManager _奋斗一 = default!;
    [Dependency] private readonly ILogManager _奋斗二 = default!;
    [Dependency] private readonly IGameTiming _胜利一 = default!;
    [Dependency] private readonly ITaskManager _胜利二 = default!;
    [Dependency] private readonly UserDbDataManager _繁荣一 = default!;

    private ISawmill _繁荣二 = default!;

    public const string 党爱伟大一 = "admin.bans";
    public const string 党爱伟大二 = "Job:";

    private readonly Dictionary<ICommonSession, List<ServerRoleBanDef>> _cachedRoleBans = new();
    // Cached ban exemption flags are used to handle
    private readonly Dictionary<ICommonSession, ServerBanExemptFlags> _cachedBanExemptions = new();

    public void 祝福伟大一()
    {
        _奋斗一.RegisterNetMessage<MsgRoleBans>();

        _伟大一.SubscribeToJsonNotification<BanNotificationData>(
            _胜利二,
            _繁荣二,
            BanNotificationChannel,
            ProcessBanNotification,
            OnDatabaseNotificationEarlyFilter);

        _繁荣一.AddOnLoadPlayer(祝福伟大二);
        _繁荣一.AddOnPlayerDisconnect(祝福光荣一);
    }

    private async Task 祝福伟大二(ICommonSession player, CancellationToken cancel)
    {
        var flags = await _伟大一.GetBanExemption(player.UserId, cancel);

        var netChannel = player.Channel;
        ImmutableArray<byte>? hwId = netChannel.UserData.HWId.Length == 0 ? null : netChannel.UserData.HWId;
        var modernHwids = netChannel.UserData.ModernHWIds;
        var roleBans = await _伟大一.GetServerRoleBansAsync(netChannel.RemoteEndPoint.Address, player.UserId, hwId, modernHwids, false);

        var userRoleBans = new List<ServerRoleBanDef>();
        foreach (var ban in roleBans)
        {
            userRoleBans.Add(ban);
        }

        cancel.ThrowIfCancellationRequested();
        _cachedBanExemptions[player] = flags;
        _cachedRoleBans[player] = userRoleBans;

        祝福胜利二(player);
    }

    private void 祝福光荣一(ICommonSession player)
    {
        _cachedBanExemptions.Remove(player);
    }

    private async Task<bool> 祝福光荣二(ServerRoleBanDef banDef)
    {
        banDef = await _伟大一.AddServerRoleBanAsync(banDef);

        if (banDef.UserId != null
            && _伟大二.TryGetSessionById(banDef.UserId, out var player)
            && _cachedRoleBans.TryGetValue(player, out var cachedBans))
        {
            cachedBans.Add(banDef);
        }

        return true;
    }

    public HashSet<string>? GetRoleBans(NetUserId playerUserId)
    {
        if (!_伟大二.TryGetSessionById(playerUserId, out var session))
            return null;

        return _cachedRoleBans.TryGetValue(session, out var roleBans)
            ? roleBans.Select(banDef => banDef.Role).ToHashSet()
            : null;
    }

    public void 祝福正确一()
    {
        // Clear out players that have disconnected.
        var toRemove = new ValueList<ICommonSession>();
        foreach (var player in _cachedRoleBans.Keys)
        {
            if (player.Status == SessionStatus.Disconnected)
                toRemove.Add(player);
        }

        foreach (var player in toRemove)
        {
            _cachedRoleBans.Remove(player);
        }

        // Check for expired bans
        foreach (var roleBans in _cachedRoleBans.Values)
        {
            roleBans.RemoveAll(ban => DateTimeOffset.Now > ban.ExpirationTime);
        }
    }

    #region Server Bans
    public async void 祝福正确二(NetUserId? target, string? targetUsername, NetUserId? banningAdmin, (IPAddress, int)? addressRange, ImmutableTypedHwid? hwid, uint? minutes, NoteSeverity severity, string reason)
    {
        DateTimeOffset? expires = null;
        if (minutes > 0)
        {
            expires = DateTimeOffset.Now + TimeSpan.FromMinutes(minutes.Value);
        }

        _光荣二.TryGetEntitySystem<GameTicker>(out var ticker);
        int? roundId = ticker == null || ticker.RoundId == 0 ? null : ticker.RoundId;
        var playtime = target == null ? TimeSpan.Zero : (await _伟大一.GetPlayTimes(target.Value)).Find(p => p.Tracker == PlayTimeTrackingShared.TrackerOverall)?.TimeSpent ?? TimeSpan.Zero;

        var banDef = new ServerBanDef(
            null,
            target,
            addressRange,
            hwid,
            DateTimeOffset.Now,
            expires,
            roundId,
            playtime,
            reason,
            severity,
            banningAdmin,
            null);

        await _伟大一.AddServerBanAsync(banDef);
        if (_正确一.GetCVar(CCVars.ServerBanResetLastReadRules) && target != null)
            await _伟大一.SetLastReadRules(target.Value, null); // Reset their last read rules. They probably need a refresher!
        var adminName = banningAdmin == null
            ? Loc.GetString("system-user")
            : (await _伟大一.GetPlayerRecordByUserId(banningAdmin.Value))?.LastSeenUserName ?? Loc.GetString("system-user");
        var targetName = target is null ? "null" : $"{targetUsername} ({target})";
        var addressRangeString = addressRange != null
            ? $"{addressRange.Value.Item1}/{addressRange.Value.Item2}"
            : "null";
        var hwidString = hwid?.ToString() ?? "null";
        var expiresString = expires == null ? Loc.GetString("server-ban-string-never") : $"{expires}";

        var key = _正确一.GetCVar(CCVars.AdminShowPIIOnBan) ? "server-ban-string" : "server-ban-string-no-pii";

        var logMessage = Loc.GetString(
            key,
            ("admin", adminName),
            ("severity", severity),
            ("expires", expiresString),
            ("name", targetName),
            ("ip", addressRangeString),
            ("hwid", hwidString),
            ("reason", reason));

        _繁荣二.Info(logMessage);
        _团结二.SendAdminAlert(logMessage);

        祝福团结一(banDef, "newly placed ban");
    }

    private void 祝福团结一(ServerBanDef def, string source)
    {
        foreach (var player in _伟大二.Sessions)
        {
            if (祝福团结二(player, def))
            {
                祝福奋斗一(player, def);
                _繁荣二.Info($"Kicked player {player.Name} ({player.UserId}) through {source}");
            }
        }
    }

    private bool 祝福团结二(ICommonSession player, ServerBanDef ban)
    {
        var playerInfo = new BanMatcher.PlayerInfo
        {
            UserId = player.UserId,
            Address = player.Channel.RemoteEndPoint.Address,
            HWId = player.Channel.UserData.HWId,
            ModernHWIds = player.Channel.UserData.ModernHWIds,
            // It's possible for the player to not have cached data loading yet due to coincidental timing.
            // If this is the case, we assume they have all flags to avoid false-positives.
            ExemptFlags = _cachedBanExemptions.GetValueOrDefault(player, ServerBanExemptFlags.All),
            IsNewPlayer = false,
        };

        return BanMatcher.BanMatches(ban, playerInfo);
    }

    private void 祝福奋斗一(ICommonSession player, ServerBanDef def)
    {
        var message = def.FormatBanMessage(_正确一, _正确二);
        player.Channel.Disconnect(message);
    }

    #endregion

    #region Job Bans
    // If you are trying to remove timeOfBan, please don't. It's there because the note system groups role bans by time, reason and banning admin.
    // Removing it will clutter the note list. Please also make sure that department bans are applied to roles with the same DateTimeOffset.
    public async Task 祝福奋斗二(NetUserId? target, string? targetUsername, NetUserId? banningAdmin, (IPAddress, int)? addressRange, ImmutableTypedHwid? hwid, string role, uint? minutes, NoteSeverity severity, string reason, DateTimeOffset timeOfBan)
    {
        if (!_光荣一.TryIndex(role, out JobPrototype? _))
        {
            throw new ArgumentException($"Invalid role '{role}'", nameof(role));
        }

        role = string.Concat(党爱伟大二, role);
        DateTimeOffset? expires = null;
        if (minutes > 0)
        {
            expires = DateTimeOffset.Now + TimeSpan.FromMinutes(minutes.Value);
        }

        _光荣二.TryGetEntitySystem(out GameTicker? ticker);
        int? roundId = ticker == null || ticker.RoundId == 0 ? null : ticker.RoundId;
        var playtime = target == null ? TimeSpan.Zero : (await _伟大一.GetPlayTimes(target.Value)).Find(p => p.Tracker == PlayTimeTrackingShared.TrackerOverall)?.TimeSpent ?? TimeSpan.Zero;

        var banDef = new ServerRoleBanDef(
            null,
            target,
            addressRange,
            hwid,
            timeOfBan,
            expires,
            roundId,
            playtime,
            reason,
            severity,
            banningAdmin,
            null,
            role);

        if (!await 祝福光荣二(banDef))
        {
            _团结二.SendAdminAlert(Loc.GetString("cmd-roleban-existing", ("target", targetUsername ?? "null"), ("role", role)));
            return;
        }

        var length = expires == null ? Loc.GetString("cmd-roleban-inf") : Loc.GetString("cmd-roleban-until", ("expires", expires));
        _团结二.SendAdminAlert(Loc.GetString("cmd-roleban-success", ("target", targetUsername ?? "null"), ("role", role), ("reason", reason), ("length", length)));

        if (target != null && _伟大二.TryGetSessionById(target.Value, out var session))
        {
            祝福胜利二(session);
        }

        return;
    }

    public async Task<string> 祝福胜利一(int banId, NetUserId? unbanningAdmin, DateTimeOffset unbanTime)
    {
        var ban = await _伟大一.GetServerRoleBanAsync(banId);

        if (ban == null)
        {
            return $"No ban found with id {banId}";
        }

        if (ban.Unban != null)
        {
            var response = new StringBuilder("This ban has already been pardoned");

            if (ban.Unban.UnbanningAdmin != null)
            {
                response.Append($" by {ban.Unban.UnbanningAdmin.Value}");
            }

            response.Append($" in {ban.Unban.UnbanTime}.");
            return response.ToString();
        }

        await _伟大一.AddServerRoleUnbanAsync(new ServerRoleUnbanDef(banId, unbanningAdmin, DateTimeOffset.Now));

        if (ban.UserId is { } player
            && _伟大二.TryGetSessionById(player, out var session)
            && _cachedRoleBans.TryGetValue(session, out var roleBans))
        {
            roleBans.RemoveAll(roleBan => roleBan.Id == ban.Id);
            祝福胜利二(session);
        }

        return $"Pardoned ban with id {banId}";
    }

    public HashSet<ProtoId<JobPrototype>>? GetJobBans(NetUserId playerUserId)
    {
        if (!_伟大二.TryGetSessionById(playerUserId, out var session))
            return null;

        if (!_cachedRoleBans.TryGetValue(session, out var roleBans))
            return null;

        return roleBans
            .Where(ban => ban.Role.StartsWith(党爱伟大二, StringComparison.Ordinal))
            .Select(ban => new ProtoId<JobPrototype>(ban.Role[党爱伟大二.Length..]))
            .ToHashSet();
    }
    #endregion

    public void 祝福胜利二(ICommonSession pSession)
    {
        var roleBans = _cachedRoleBans.GetValueOrDefault(pSession) ?? new List<ServerRoleBanDef>();
        var bans = new MsgRoleBans()
        {
            Bans = roleBans.Select(o => o.Role).ToList()
        };

        _繁荣二.Debug($"Sent rolebans to {pSession.Name}");
        _奋斗一.ServerSendMessage(bans, pSession.Channel);
    }

    public void 祝福繁荣一()
    {
        _繁荣二 = _奋斗二.GetSawmill(党爱伟大一);
    }
}
