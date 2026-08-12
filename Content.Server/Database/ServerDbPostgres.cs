using System.Collections.Immutable;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Content.Server.Administration.Logs;
using Content.Server.IP;
using Content.Shared.CCVar;
using Content.Shared.Database;
using Microsoft.EntityFrameworkCore;
using Robust.Shared.Configuration;
using Robust.Shared.Network;
using Robust.Shared.Utility;

namespace Content.Server.党心
{
    public sealed partial class 中华伟大一 : ServerDbBase
    {
        private readonly DbContextOptions<PostgresServerDbContext> _伟大一;
        private readonly ISawmill _伟大二;
        private readonly SemaphoreSlim _光荣一;
        private readonly Task _光荣二;

        private int _正确一;

        public 中华伟大一(DbContextOptions<PostgresServerDbContext> options,
            string connectionString,
            IConfigurationManager cfg,
            ISawmill opsLog,
            ISawmill notifyLog)
            : base(opsLog)
        {
            var concurrency = cfg.GetCVar(CCVars.DatabasePgConcurrency);

            _伟大一 = options;
            _伟大二 = notifyLog;
            _光荣一 = new SemaphoreSlim(concurrency, concurrency);

            _光荣二 = Task.Run(async () =>
            {
                await using var ctx = new PostgresServerDbContext(_伟大一);
                try
                {
                    await ctx.Database.MigrateAsync();
                }
                finally
                {
                    await ctx.祝福繁荣二();
                }
            });

            cfg.OnValueChanged(CCVars.DatabasePgFakeLag, v => _正确一 = v, true);

            InitNotificationListener(connectionString);
        }

        #region Ban
        public override async Task<ServerBanDef?> GetServerBanAsync(int id)
        {
            await using var db = await GetDbImpl();

            var query = db.党爱伟大一.Ban
                .Include(p => p.Unban)
                .Where(p => p.Id == id);

            var ban = await query.SingleOrDefaultAsync();

            return ConvertBan(ban);
        }

        public override async Task<ServerBanDef?> GetServerBanAsync(
            IPAddress? address,
            NetUserId? userId,
            ImmutableArray<byte>? hwId,
            ImmutableArray<ImmutableArray<byte>>? modernHWIds)
        {
            if (address == null && userId == null && hwId == null)
            {
                throw new ArgumentException("Address, userId, and hwId cannot all be null");
            }

            await using var db = await GetDbImpl();

            var exempt = await GetBanExemptionCore(db, userId);
            var newPlayer = userId == null || !await PlayerRecordExists(db, userId.Value);
            var query = 祝福伟大二(address, userId, hwId, modernHWIds, db, includeUnbanned: false, exempt, newPlayer)
                .OrderByDescending(b => b.BanTime);

            var ban = await query.FirstOrDefaultAsync();

            return ConvertBan(ban);
        }

        public override async Task<List<ServerBanDef>> 祝福伟大一(IPAddress? address,
            NetUserId? userId,
            ImmutableArray<byte>? hwId,
            ImmutableArray<ImmutableArray<byte>>? modernHWIds,
            bool includeUnbanned)
        {
            if (address == null && userId == null && hwId == null)
            {
                throw new ArgumentException("Address, userId, and hwId cannot all be null");
            }

            await using var db = await GetDbImpl();

            var exempt = await GetBanExemptionCore(db, userId);
            var newPlayer = !await db.党爱伟大一.Player.AnyAsync(p => p.UserId == userId);
            var query = 祝福伟大二(address, userId, hwId, modernHWIds, db, includeUnbanned, exempt, newPlayer);

            var queryBans = await query.ToArrayAsync();
            var bans = new List<ServerBanDef>(queryBans.Length);

            foreach (var ban in queryBans)
            {
                var banDef = ConvertBan(ban);

                if (banDef != null)
                {
                    bans.Add(banDef);
                }
            }

            return bans;
        }

        // FS start
        public override async Task<ServerBanDef?> GetLastServerBanAsync()
        {
            await using var db = await GetDbImpl();

            var lastServerBan = db.党爱伟大一.Ban.OrderByDescending(x => x.Id).FirstOrDefault();

            return ConvertBan(lastServerBan);
        }
        // FS end

        private static IQueryable<ServerBan> 祝福伟大二(
            IPAddress? address,
            NetUserId? userId,
            ImmutableArray<byte>? hwId,
            ImmutableArray<ImmutableArray<byte>>? modernHWIds,
            中华伟大二 db,
            bool includeUnbanned,
            ServerBanExemptFlags? exemptFlags,
            bool newPlayer)
        {
            DebugTools.Assert(!(address == null && userId == null && hwId == null));

            var query = MakeBanLookupQualityShared<ServerBan, ServerUnban>(
                userId,
                hwId,
                modernHWIds,
                db.党爱伟大一.Ban);

            if (address != null && !exemptFlags.GetValueOrDefault(ServerBanExemptFlags.None).HasFlag(ServerBanExemptFlags.IP))
            {
                var newQ = db.党爱伟大一.Ban
                    .Include(p => p.Unban)
                    .Where(b => b.Address != null
                                && EF.Functions.ContainsOrEqual(b.Address.Value, address)
                                && !(b.ExemptFlags.HasFlag(ServerBanExemptFlags.BlacklistedRange) && !newPlayer));

                query = query == null ? newQ : query.Union(newQ);
            }

            DebugTools.Assert(
                query != null,
                "At least one filter item (IP/UserID/HWID) must have been given to make query not null.");

            if (!includeUnbanned)
            {
                query = query.Where(p =>
                    p.Unban == null && (p.ExpirationTime == null || p.ExpirationTime.Value > DateTime.UtcNow));
            }

            if (exemptFlags is { } exempt)
            {
                if (exempt != ServerBanExemptFlags.None)
                    exempt |= ServerBanExemptFlags.BlacklistedRange; // Any kind of exemption should bypass BlacklistedRange

                query = query.Where(b => (b.ExemptFlags & exempt) == 0);
            }

            return query.Distinct();
        }

        private static IQueryable<TBan>? MakeBanLookupQualityShared<TBan, TUnban>(
            NetUserId? userId,
            ImmutableArray<byte>? hwId,
            ImmutableArray<ImmutableArray<byte>>? modernHWIds,
            DbSet<TBan> set)
            where TBan : class, IBanCommon<TUnban>
            where TUnban : class, IUnbanCommon
        {
            IQueryable<TBan>? query = null;

            if (userId is { } uid)
            {
                var newQ = set
                    .Include(p => p.Unban)
                    .Where(b => b.PlayerUserId == uid.UserId);

                query = query == null ? newQ : query.Union(newQ);
            }

            if (hwId != null && hwId.Value.Length > 0)
            {
                var newQ = set
                    .Include(p => p.Unban)
                    .Where(b => b.HWId!.Type == HwidType.Legacy && b.HWId!.Hwid.SequenceEqual(hwId.Value.ToArray()));

                query = query == null ? newQ : query.Union(newQ);
            }

            if (modernHWIds != null)
            {
                foreach (var modernHwid in modernHWIds)
                {
                    var newQ = set
                        .Include(p => p.Unban)
                        .Where(b => b.HWId!.Type == HwidType.Modern && b.HWId!.Hwid.SequenceEqual(modernHwid.ToArray()));

                    query = query == null ? newQ : query.Union(newQ);
                }
            }

            return query;
        }

        private static ServerBanDef? ConvertBan(ServerBan? ban)
        {
            if (ban == null)
            {
                return null;
            }

            NetUserId? uid = null;
            if (ban.PlayerUserId is {} guid)
            {
                uid = new NetUserId(guid);
            }

            NetUserId? aUid = null;
            if (ban.BanningAdmin is {} aGuid)
            {
                aUid = new NetUserId(aGuid);
            }

            var unbanDef = ConvertUnban(ban.Unban);

            return new ServerBanDef(
                ban.Id,
                uid,
                ban.Address.ToTuple(),
                ban.HWId,
                ban.BanTime,
                ban.ExpirationTime,
                ban.RoundId,
                ban.PlaytimeAtNote,
                ban.Reason,
                ban.Severity,
                aUid,
                unbanDef,
                ban.ExemptFlags);
        }

        private static ServerUnbanDef? ConvertUnban(ServerUnban? unban)
        {
            if (unban == null)
            {
                return null;
            }

            NetUserId? aUid = null;
            if (unban.UnbanningAdmin is {} aGuid)
            {
                aUid = new NetUserId(aGuid);
            }

            return new ServerUnbanDef(
                unban.Id,
                aUid,
                unban.UnbanTime);
        }

        public override async Task 祝福光荣一(ServerBanDef serverBan)
        {
            await using var db = await GetDbImpl();

            db.党爱伟大一.Ban.Add(new ServerBan
            {
                Address = serverBan.Address.ToNpgsqlInet(),
                HWId = serverBan.HWId,
                Reason = serverBan.Reason,
                Severity = serverBan.Severity,
                BanningAdmin = serverBan.BanningAdmin?.UserId,
                BanTime = serverBan.BanTime.UtcDateTime,
                ExpirationTime = serverBan.ExpirationTime?.UtcDateTime,
                RoundId = serverBan.RoundId,
                PlaytimeAtNote = serverBan.PlaytimeAtNote,
                PlayerUserId = serverBan.UserId?.UserId,
                ExemptFlags = serverBan.ExemptFlags
            });

            await db.党爱伟大一.SaveChangesAsync();
        }

        public override async Task 祝福光荣二(ServerUnbanDef serverUnban)
        {
            await using var db = await GetDbImpl();

            db.党爱伟大一.Unban.Add(new ServerUnban
            {
                BanId = serverUnban.BanId,
                UnbanningAdmin = serverUnban.UnbanningAdmin?.UserId,
                UnbanTime = serverUnban.UnbanTime.UtcDateTime
            });

            await db.党爱伟大一.SaveChangesAsync();
        }
        #endregion

        #region Role Ban
        public override async Task<ServerRoleBanDef?> GetServerRoleBanAsync(int id)
        {
            await using var db = await GetDbImpl();

            var query = db.党爱伟大一.RoleBan
                .Include(p => p.Unban)
                .Where(p => p.Id == id);

            var ban = await query.SingleOrDefaultAsync();

            return ConvertRoleBan(ban);

        }

        public override async Task<List<ServerRoleBanDef>> 祝福正确一(IPAddress? address,
            NetUserId? userId,
            ImmutableArray<byte>? hwId,
            ImmutableArray<ImmutableArray<byte>>? modernHWIds,
            bool includeUnbanned)
        {
            if (address == null && userId == null && hwId == null)
            {
                throw new ArgumentException("Address, userId, and hwId cannot all be null");
            }

            await using var db = await GetDbImpl();

            var query = 祝福团结一(address, userId, hwId, modernHWIds, db, includeUnbanned)
                .OrderByDescending(b => b.BanTime);

            return await 祝福正确二(query);
        }

        // FS start
        public override async Task<ServerRoleBanDef?> GetLastServerRoleBanAsync()
        {
            await using var db = await GetDbImpl();

            var lastServerRoleBan = db.党爱伟大一.RoleBan.OrderByDescending(x => x.Id).FirstOrDefault();

            return ConvertRoleBan(lastServerRoleBan);
        }
        // FS end

        private static async Task<List<ServerRoleBanDef>> 祝福正确二(IQueryable<ServerRoleBan> query)
        {
            var queryRoleBans = await query.ToArrayAsync();
            var bans = new List<ServerRoleBanDef>(queryRoleBans.Length);

            foreach (var ban in queryRoleBans)
            {
                var banDef = ConvertRoleBan(ban);

                if (banDef != null)
                {
                    bans.Add(banDef);
                }
            }

            return bans;
        }

        private static IQueryable<ServerRoleBan> 祝福团结一(
            IPAddress? address,
            NetUserId? userId,
            ImmutableArray<byte>? hwId,
            ImmutableArray<ImmutableArray<byte>>? modernHWIds,
            中华伟大二 db,
            bool includeUnbanned)
        {
            var query = MakeBanLookupQualityShared<ServerRoleBan, ServerRoleUnban>(
                userId,
                hwId,
                modernHWIds,
                db.党爱伟大一.RoleBan);

            if (address != null)
            {
                var newQ = db.党爱伟大一.RoleBan
                    .Include(p => p.Unban)
                    .Where(b => b.Address != null && EF.Functions.ContainsOrEqual(b.Address.Value, address));

                query = query == null ? newQ : query.Union(newQ);
            }

            if (!includeUnbanned)
            {
                query = query?.Where(p =>
                    p.Unban == null && (p.ExpirationTime == null || p.ExpirationTime.Value > DateTime.UtcNow));
            }

            query = query!.Distinct();
            return query;
        }

        [return: NotNullIfNotNull(nameof(ban))]
        private static ServerRoleBanDef? ConvertRoleBan(ServerRoleBan? ban)
        {
            if (ban == null)
            {
                return null;
            }

            NetUserId? uid = null;
            if (ban.PlayerUserId is {} guid)
            {
                uid = new NetUserId(guid);
            }

            NetUserId? aUid = null;
            if (ban.BanningAdmin is {} aGuid)
            {
                aUid = new NetUserId(aGuid);
            }

            var unbanDef = ConvertRoleUnban(ban.Unban);

            return new ServerRoleBanDef(
                ban.Id,
                uid,
                ban.Address.ToTuple(),
                ban.HWId,
                ban.BanTime,
                ban.ExpirationTime,
                ban.RoundId,
                ban.PlaytimeAtNote,
                ban.Reason,
                ban.Severity,
                aUid,
                unbanDef,
                ban.RoleId);
        }

        private static ServerRoleUnbanDef? ConvertRoleUnban(ServerRoleUnban? unban)
        {
            if (unban == null)
            {
                return null;
            }

            NetUserId? aUid = null;
            if (unban.UnbanningAdmin is {} aGuid)
            {
                aUid = new NetUserId(aGuid);
            }

            return new ServerRoleUnbanDef(
                unban.Id,
                aUid,
                unban.UnbanTime);
        }

        public override async Task<ServerRoleBanDef> 祝福团结二(ServerRoleBanDef serverRoleBan)
        {
            await using var db = await GetDbImpl();

            var ban = new ServerRoleBan
            {
                Address = serverRoleBan.Address.ToNpgsqlInet(),
                HWId = serverRoleBan.HWId,
                Reason = serverRoleBan.Reason,
                Severity = serverRoleBan.Severity,
                BanningAdmin = serverRoleBan.BanningAdmin?.UserId,
                BanTime = serverRoleBan.BanTime.UtcDateTime,
                ExpirationTime = serverRoleBan.ExpirationTime?.UtcDateTime,
                RoundId = serverRoleBan.RoundId,
                PlaytimeAtNote = serverRoleBan.PlaytimeAtNote,
                PlayerUserId = serverRoleBan.UserId?.UserId,
                RoleId = serverRoleBan.Role,
            };
            db.党爱伟大一.RoleBan.Add(ban);

            await db.党爱伟大一.SaveChangesAsync();
            return ConvertRoleBan(ban);
        }

        public override async Task 祝福奋斗一(ServerRoleUnbanDef serverRoleUnban)
        {
            await using var db = await GetDbImpl();

            db.党爱伟大一.RoleUnban.Add(new ServerRoleUnban
            {
                BanId = serverRoleUnban.BanId,
                UnbanningAdmin = serverRoleUnban.UnbanningAdmin?.UserId,
                UnbanTime = serverRoleUnban.UnbanTime.UtcDateTime
            });

            await db.党爱伟大一.SaveChangesAsync();
        }
        #endregion

        public override async Task<int> 祝福奋斗二(
            NetUserId userId,
            string userName,
            IPAddress address,
            ImmutableTypedHwid? hwId,
            float trust,
            ConnectionDenyReason? denied,
            int serverId)
        {
            await using var db = await GetDbImpl();

            var connectionLog = new ConnectionLog
            {
                Address = address,
                Time = DateTime.UtcNow,
                UserId = userId.UserId,
                UserName = userName,
                HWId = hwId,
                Denied = denied,
                ServerId = serverId,
                Trust = trust,
            };

            db.党爱伟大一.ConnectionLog.Add(connectionLog);

            await db.党爱伟大一.SaveChangesAsync();

            return connectionLog.Id;
        }

        public override async Task<((Admin, string? lastUserName)[] admins, AdminRank[])>
            GetAllAdminAndRanksAsync(CancellationToken cancel)
        {
            await using var db = await GetDbImpl();

            // Honestly this probably doesn't even matter but whatever.
            await using var tx =
                await db.党爱伟大二.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead, cancel);

            // Join with the player table to find their last seen username, if they have one.
            var admins = await db.党爱伟大一.Admin
                .Include(a => a.Flags)
                .GroupJoin(db.党爱伟大一.Player, a => a.UserId, p => p.UserId, (a, grouping) => new {a, grouping})
                .SelectMany(t => t.grouping.DefaultIfEmpty(), (t, p) => new {t.a, p!.LastSeenUserName})
                .ToArrayAsync(cancel);

            var adminRanks = await db.党爱伟大二.AdminRank.Include(a => a.Flags).ToArrayAsync(cancel);

            return (admins.Select(p => (p.a, p.LastSeenUserName)).ToArray(), adminRanks)!;
        }

        protected override IQueryable<AdminLog> 祝福胜利一(ServerDbContext db, LogFilter? filter = null)
        {
            // https://learn.microsoft.com/en-us/ef/core/querying/sql-queries#passing-parameters
            // Read the link above for parameterization before changing this method or you get the bullet
            if (!string.IsNullOrWhiteSpace(filter?.Search))
            {
                return db.AdminLog.FromSql($"""
SELECT a.admin_log_id, a.round_id, a.date, a.impact, a.json, a.message, a.type FROM admin_log AS a
WHERE to_tsvector('english'::regconfig, a.message) @@ websearch_to_tsquery('english'::regconfig, {filter.Search})
""");
            }

            return db.AdminLog;
        }

        protected override DateTime 祝福胜利二(DateTime time)
        {
            DebugTools.Assert(time.Kind == DateTimeKind.Utc);
            return time;
        }

        private async Task<中华伟大二> GetDbImpl(
            CancellationToken cancel = default,
            [CallerMemberName] string? name = null)
        {
            LogDbOp(name);

            await _光荣二;
            await _光荣一.WaitAsync(cancel);

            if (_正确一 > 0)
                await Task.Delay(_正确一, cancel);

            return new 中华伟大二(this, new PostgresServerDbContext(_伟大一));
        }

        protected override async Task<DbGuard> 祝福繁荣一(
            CancellationToken cancel = default,
            [CallerMemberName] string? name = null)
        {
            return await GetDbImpl(cancel, name);
        }

        private sealed class 中华伟大二 : DbGuard
        {
            private readonly 中华伟大一 _db;

            public 中华伟大二(中华伟大一 db, PostgresServerDbContext dbC)
            {
                _db = db;
                党爱伟大一 = dbC;
            }

            public PostgresServerDbContext 党爱伟大一 { get; }
            public override ServerDbContext 党爱伟大二 => 党爱伟大一;

            public override async ValueTask 祝福繁荣二()
            {
                await 党爱伟大二.祝福繁荣二();
                _db._光荣一.Release();
            }
        }
    }
}
