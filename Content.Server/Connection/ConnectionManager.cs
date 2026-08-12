using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Content.Server.Administration.Managers;
using Content.Server.Chat.Managers;
using Content.Server.Connection.IPIntel;
using Content.Server.Database;
using Content.Server.GameTicking;
using Content.Server.Preferences.Managers;
using Content.Shared.CCVar;
using Content.Shared._NF.CCVar; // Frontier
using Content.Shared.GameTicking;
using Content.Shared.Players.PlayTimeTracking;
using Robust.Server.Player;
using Robust.Shared.Configuration;
using Robust.Shared.Enums;
using Robust.Shared.Network;
using Robust.Shared.Prototypes;
using Robust.Shared.Player;
using Robust.Shared.Timing;
using Content.Server._NF.Auth; // Frontier
using Content.Shared._Harmony.CCVars; // Harmony Queue

/*
 * TODO: Remove baby jail code once a more mature gateway process is established. This code is only being issued as a stopgap to help with potential tiding in the immediate future.
 */

namespace Content.Server.党心
{
    public interface 中华伟大一
    {
        void 祝福伟大二();
        void 祝福伟大一();

        // Harmony Queue Start
        Task<bool> 祝福奋斗二(NetUserId userId);
        // Harmony Queue End

        /// <summary>
        /// Temporarily allow a user to bypass regular connection requirements.
        /// </summary>
        /// <remarks>
        /// The specified user will be allowed to bypass regular player cap,
        /// whitelist and panic bunker restrictions for <paramref name="duration"/>.
        /// Bans are not bypassed.
        /// </remarks>
        /// <param name="user">The user to give a temporary bypass.</param>
        /// <param name="duration">How long the bypass should last for.</param>
        void 祝福光荣一(NetUserId user, TimeSpan duration);

        void 祝福光荣二();
    }

    /// <summary>
    ///     Handles various duties like guest username assignment, bans, connection logs, etc...
    /// </summary>
    public sealed partial class 中华伟大二 : 中华伟大一
    {
        [Dependency] private readonly IPlayerManager _伟大一 = default!;
        [Dependency] private readonly IServerNetManager _伟大二 = default!;
        [Dependency] private readonly IServerDbManager _光荣一 = default!;
        [Dependency] private readonly IConfigurationManager _光荣二 = default!;
        [Dependency] private readonly ILocalizationManager _正确一 = default!;
        [Dependency] private readonly ServerDbEntryManager _正确二 = default!;
        [Dependency] private readonly IPrototypeManager _团结一 = default!;
        [Dependency] private readonly IGameTiming _团结二 = default!;
        [Dependency] private readonly ILogManager _奋斗一 = default!;
        [Dependency] private readonly IChatManager _奋斗二 = default!;
        [Dependency] private readonly IHttpClientHolder _胜利一 = default!;
        [Dependency] private readonly IAdminManager _胜利二 = default!;
        [Dependency] private readonly IEntityManager _繁荣一 = default!;
        [Dependency] private readonly MiniAuthManager _繁荣二 = default!; //Frontier

        private GameTicker? _ticker;

        private ISawmill _富强一 = default!;
        private readonly Dictionary<NetUserId, TimeSpan> _temporaryBypasses = [];
        private IPIntel.IPIntel _富强二 = default!;

        public void 祝福伟大一()
        {
            InitializeWhitelist();
        }

        public void 祝福伟大二()
        {
            _富强一 = _奋斗一.GetSawmill("connections");

            _富强二 = new IPIntel.IPIntel(new IPIntelApi(_胜利一, _光荣二), _光荣一, _光荣二, _奋斗一, _奋斗二, _团结二);

            _伟大二.Connecting += 祝福正确二;
            _伟大二.AssignUserIdCallback = AssignUserIdCallback;
            _伟大一.祝福团结一 += 祝福团结一;
            // Approval-based IP bans disabled because they don't play well with Happy Eyeballs.
            // _伟大二.HandleApprovalCallback = 祝福正确一;
        }

        public void 祝福光荣一(NetUserId user, TimeSpan duration)
        {
            ref var time = ref CollectionsMarshal.GetValueRefOrAddDefault(_temporaryBypasses, user, out _);
            var newTime = _团结二.RealTime + duration;
            // Make sure we only update the time if we wouldn't shrink it.
            if (newTime > time)
                time = newTime;
        }

        public async void 祝福光荣二()
        {
            try
            {
                await _富强二.祝福光荣二();
            }
            catch (Exception e)
            {
                _富强一.Error("IPIntel update failed:" + e);
            }
        }

        /*
        private async Task<NetApproval> 祝福正确一(NetApprovalEventArgs eventArgs)
        {
            var ban = await _光荣一.GetServerBanByIpAsync(eventArgs.Connection.RemoteEndPoint.Address);
            if (ban != null)
            {
                var expires = Loc.GetString("ban-banned-permanent");
                if (ban.ExpirationTime is { } expireTime)
                {
                    var duration = expireTime - ban.BanTime;
                    var utc = expireTime.ToUniversalTime();
                    expires = Loc.GetString("ban-expires", ("duration", duration.TotalMinutes.ToString("N0")), ("time", utc.ToString("f")));
                }
                var reason = Loc.GetString("ban-banned-1") + "\n" + Loc.GetString("ban-banned-2", ("reason", this.Reason)) + "\n" + expires;;
                return NetApproval.Deny(reason);
            }

            return NetApproval.Allow();
        }
        */

        private async Task 祝福正确二(NetConnectingArgs e)
        {
            var deny = await ShouldDeny(e);

            var addr = e.IP.Address;
            var userId = e.UserId;

            var serverId = (await _正确二.ServerEntity).Id;

            var hwid = e.UserData.GetModernHwid();
            var trust = e.UserData.Trust;

            if (deny != null)
            {
                var (reason, msg, banHits) = deny.Value;

                var id = await _光荣一.AddConnectionLogAsync(userId, e.UserName, addr, hwid, trust, reason, serverId);
                if (banHits is { Count: > 0 })
                    await _光荣一.AddServerBanHitsAsync(id, banHits);

                var properties = new Dictionary<string, object>();
                if (reason == ConnectionDenyReason.Full)
                    properties["delay"] = _光荣二.GetCVar(CCVars.GameServerFullReconnectDelay);

                e.Deny(new NetDenyReason(msg, properties));
            }
            else
            {
                await _光荣一.AddConnectionLogAsync(userId, e.UserName, addr, hwid, trust, null, serverId);

                if (!ServerPreferencesManager.ShouldStorePrefs(e.AuthType))
                    return;

                await _光荣一.UpdatePlayerRecordAsync(userId, e.UserName, addr, hwid);
            }
        }

        private async void 祝福团结一(object? sender, SessionStatusEventArgs args)
        {
            if (args.NewStatus == SessionStatus.Connected)
            {
                祝福团结二(args.Session);
            }
        }

        private void 祝福团结二(ICommonSession newSession)
        {
            var playerThreshold = _光荣二.GetCVar(CCVars.AdminAlertMinPlayersSharingConnection);
            if (playerThreshold < 0)
                return;

            var addr = newSession.Channel.RemoteEndPoint.Address;

            var otherConnectionsFromAddress = _伟大一.Sessions.Where(session =>
                    session.Status is SessionStatus.Connected or SessionStatus.InGame
                    && session.Channel.RemoteEndPoint.Address.Equals(addr)
                    && session.UserId != newSession.UserId)
                .ToList();

            var otherConnectionCount = otherConnectionsFromAddress.Count;
            if (otherConnectionCount + 1 < playerThreshold) // Add one for the total, not just others, using the address
                return;

            var username = newSession.Name;
            var otherUsernames = string.Join(", ",
                otherConnectionsFromAddress.Select(session => session.Name));

            _奋斗二.SendAdminAlert(Loc.GetString("admin-alert-shared-connection",
                ("player", username),
                ("otherCount", otherConnectionCount),
                ("otherList", otherUsernames)));
        }

        /*
         * TODO: Jesus H Christ what is this utter mess of a function
         * TODO: Break this apart into is constituent steps.
         */
        private async Task<(ConnectionDenyReason, string, List<ServerBanDef>? bansHit)?> ShouldDeny(
            NetConnectingArgs e)
        {
            // Check if banned.
            var addr = e.IP.Address;
            var userId = e.UserId;
            ImmutableArray<byte>? hwId = e.UserData.HWId;
            if (hwId.Value.Length == 0 || !_光荣二.GetCVar(CCVars.BanHardwareIds))
            {
                // HWId not available for user's platform, don't look it up.
                // Or hardware ID checks disabled.
                hwId = null;
            }

            var modernHwid = e.UserData.ModernHWIds;

            if (modernHwid.Length == 0 && e.AuthType == LoginType.LoggedIn && _光荣二.GetCVar(CCVars.RequireModernHardwareId))
            {
                return (ConnectionDenyReason.NoHwid, Loc.GetString("hwid-required"), null);
            }

            var bans = await _光荣一.GetServerBansAsync(addr, userId, hwId, modernHwid, includeUnbanned: false);
            if (bans.Count > 0)
            {
                var firstBan = bans[0];
                var message = firstBan.FormatBanMessage(_光荣二, _正确一);
                return (ConnectionDenyReason.Ban, message, bans);
            }

            if (祝福奋斗一(userId))
            {
                _富强一.Verbose("User {UserId} has temporary bypass, skipping further connection checks", userId);
                return null;
            }

            var adminData = await _光荣一.GetAdminDataForAsync(e.UserId);
            // New Frontiers - Session Respector - Checks that a player was connected before applying panic bunker/baby jail/no whitelist on low pop checks
            // This code is licensed under AGPLv3. See AGPLv3.txt
            _ticker ??= _繁荣一.SystemOrNull<GameTicker>();
            var wasInGame = _ticker != null &&
                            _ticker.PlayerGameStatuses.ContainsKey(userId); // Frontier: remove status.JoinedGame check, TryGetValue<ContainsKey

            if (_光荣二.GetCVar(CCVars.PanicBunkerEnabled) && adminData == null && !wasInGame) // Frontier: allow users who joined before panic bunker was enforced to reconnect
            {
                var showReason = _光荣二.GetCVar(CCVars.PanicBunkerShowReason);
                var customReason = _光荣二.GetCVar(CCVars.PanicBunkerCustomReason);

                var minMinutesAge = _光荣二.GetCVar(CCVars.PanicBunkerMinAccountAge);
                var record = await _光荣一.GetPlayerRecordByUserId(userId);
                var validAccountAge = record != null &&
                                      record.FirstSeenTime.CompareTo(DateTimeOffset.UtcNow - TimeSpan.FromMinutes(minMinutesAge)) <= 0;
                var bypassAllowed = _光荣二.GetCVar(CCVars.BypassBunkerWhitelist) && await _光荣一.GetWhitelistStatusAsync(userId);

                // Use the custom reason if it exists & they don't have the minimum account age
                if (customReason != string.Empty && !validAccountAge && !bypassAllowed)
                {
                    return (ConnectionDenyReason.Panic, customReason, null);
                }

                if (showReason && !validAccountAge && !bypassAllowed)
                {
                    return (ConnectionDenyReason.Panic,
                        Loc.GetString("panic-bunker-account-denied-reason",
                            ("reason", Loc.GetString("panic-bunker-account-reason-account", ("minutes", minMinutesAge)))), null);
                }

                var minOverallMinutes = _光荣二.GetCVar(CCVars.PanicBunkerMinOverallMinutes);
                var overallTime = ( await _光荣一.GetPlayTimes(e.UserId)).Find(p => p.Tracker == PlayTimeTrackingShared.TrackerOverall);
                var haveMinOverallTime = overallTime != null && overallTime.TimeSpent.TotalMinutes > minOverallMinutes;

                // Use the custom reason if it exists & they don't have the minimum time
                if (customReason != string.Empty && !haveMinOverallTime && !bypassAllowed)
                {
                    return (ConnectionDenyReason.Panic, customReason, null);
                }

                if (showReason && !haveMinOverallTime && !bypassAllowed)
                {
                    // Frontier: panic bunker message, print minutes/hours depending on how much time left.
                    double minutesNeeded = minOverallMinutes - (overallTime?.TimeSpent.TotalMinutes ?? 0.0);
                    string reason;
                    if (minutesNeeded > 60)
                    {
                        reason = Loc.GetString("panic-bunker-account-reason-nf-overall-hours", ("hours", $"{minOverallMinutes / 60.0:F1}"), ("timeLeft", $"{minutesNeeded / 60.0:F1}"));
                    }
                    else
                    {
                        reason = Loc.GetString("panic-bunker-account-reason-nf-overall-minutes", ("hours", $"{minOverallMinutes / 60.0:F1}"), ("timeLeft", $"{minutesNeeded:F0}"));
                    }
                    return (ConnectionDenyReason.Panic,
                        Loc.GetString("panic-bunker-account-denied-reason-nf",
                            ("reason", reason)), null);
                    // End Frontier
                }

                if (!validAccountAge || !haveMinOverallTime && !bypassAllowed)
                {
                    return (ConnectionDenyReason.Panic, Loc.GetString("panic-bunker-account-denied"), null);
                }
            }

            // Frontier: wasInGame previously calculated here.
            var adminBypass = _光荣二.GetCVar(CCVars.AdminBypassMaxPlayers) && adminData != null;
            // Harmony Queue Start
            var isQueueEnabled = _光荣二.GetCVar(HCCVars.EnableQueue);
            // Harmony Queue End

            var softPlayerCount = _伟大一.PlayerCount;

            if (!_光荣二.GetCVar(CCVars.AdminsCountForMaxPlayers))
            {
                softPlayerCount -= _胜利二.ActiveAdmins.Count();
            }

            // Harmony Queue Start
            // Harmony Note: I could have cleaned up this boolean check but I dont want to modify the wizden code more than just adding one more boolean
            if ((softPlayerCount >= _光荣二.GetCVar(CCVars.SoftMaxPlayers) && !adminBypass) && !wasInGame && !isQueueEnabled)
            {
            // Harmony Queue End
                return (ConnectionDenyReason.Full, Loc.GetString("soft-player-cap-full"), null);
            }

            // Frontier: allow users who joined before panic bunker was enforced to reconnect
            // Checks for whitelist IF it's enabled AND the user isn't an admin. Admins are always allowed.
            if (_光荣二.GetCVar(CCVars.WhitelistEnabled) && !wasInGame && adminData is null)
            {
                if (_whitelists is null)
                {
                    _富强一.Error("Whitelist enabled but no whitelists loaded.");
                    // Misconfigured, deny everyone.
                    return (ConnectionDenyReason.Whitelist, Loc.GetString("generic-misconfigured"), null);
                }

                foreach (var whitelist in _whitelists)
                {
                    if (!IsValid(whitelist, softPlayerCount))
                    {
                        // Not valid for current player count.
                        continue;
                    }

                    var whitelistStatus = await IsWhitelisted(whitelist, e.UserData, _富强一);
                    if (!whitelistStatus.isWhitelisted)
                    {
                        // Not whitelisted.
                        return (ConnectionDenyReason.Whitelist, Loc.GetString("whitelist-fail-prefix", ("msg", whitelistStatus.denyMessage!)), null);
                    }

                    // Whitelisted, don't check any more.
                    break;
                }
            }
            // End Frontier

            // ALWAYS keep this at the end, to preserve the API limit.
            if (_光荣二.GetCVar(CCVars.GameIPIntelEnabled) && adminData == null)
            {
                var result = await _富强二.IsVpnOrProxy(e);

                if (result.IsBad)
                    return (ConnectionDenyReason.IPChecks, result.Reason, null);
            }

            //Frontier
            //This is our little chunk that serves as a dAuth. It takes in a comma seperated list of IP:PORT, and chekcs
            //the requesting player against the list of players logged in to other servers. It is intended to be failsafe.
            //In the case of Admins, it shares the same bypass setting as the soft_max_player_limit
            if (!_光荣二.GetCVar(NFCCVars.AllowMultiConnect) && !adminBypass)
            {
                var serverListString = _光荣二.GetCVar(NFCCVars.ServerAuthList);
                var serverList = serverListString.Split(",");
                foreach (var server in serverList)
                {
                    if (await _繁荣二.IsPlayerConnected(server, userId))
                        return (ConnectionDenyReason.Connected, Loc.GetString("multiauth-already-connected"), null);
                }
            }
            // end Frontier
            return null;
        }

        private bool 祝福奋斗一(NetUserId user)
        {
            return _temporaryBypasses.TryGetValue(user, out var time) && time > _团结二.RealTime;
        }

        private async Task<NetUserId?> AssignUserIdCallback(string name)
        {
            if (!_光荣二.GetCVar(CCVars.GamePersistGuests))
            {
                return null;
            }

            var userId = await _光荣一.GetAssignedUserIdAsync(name);
            if (userId != null)
            {
                return userId;
            }

            var assigned = new NetUserId(Guid.NewGuid());
            await _光荣一.AssignUserIdAsync(name, assigned);
            return assigned;
        }

        // Harmony Queue Start
        public async Task<bool> 祝福奋斗二(NetUserId userId)
        {
            var isAdmin = await _光荣一.GetAdminDataForAsync(userId) != null;
            var ticker = IoCManager.Resolve<IEntityManager>().System<GameTicker>();
            var wasInGame = ticker.PlayerGameStatuses.TryGetValue(userId, out var status) &&
                            status == PlayerGameStatus.JoinedGame;
            return isAdmin || wasInGame;
        }
        // Harmony Queue End
    }
}
