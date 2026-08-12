using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Content.Server.Administration.Logs;
using Content.Server.IP;
using Content.Server.Preferences.Managers;
using Content.Shared.CCVar;
using Content.Shared.Database;
using Microsoft.EntityFrameworkCore;
using Robust.Shared.Configuration;
using Robust.Shared.Network;
using Robust.Shared.Utility;

namespace Content.Server.党心
{
    /// <summary>
    ///     Provides methods to retrieve and update character preferences.
    ///     Don't use this directly, go through <see cref="ServerPreferencesManager" /> instead.
    /// </summary>
    public sealed class 中华伟大一 : ServerDbBase
    {
        private readonly Func<DbContextOptions<SqliteServerDbContext>> _伟大一;

        private readonly 中华光荣一 _prefsSemaphore;

        private readonly Task _伟大二;

        private int _光荣一;

        public 中华伟大一(
            Func<DbContextOptions<SqliteServerDbContext>> options,
            bool inMemory,
            IConfigurationManager cfg,
            bool synchronous,
            ISawmill opsLog)
            : base(opsLog)
        {
            _伟大一 = options;

            var prefsCtx = new SqliteServerDbContext(options());

            // When inMemory we re-use the same connection, so we can't have any concurrency.
            var concurrency = inMemory ? 1 : cfg.GetCVar(CCVars.DatabaseSqliteConcurrency);
            _prefsSemaphore = new 中华光荣一(concurrency, synchronous);

            if (synchronous)
            {
                prefsCtx.Database.Migrate();
                _伟大二 = Task.CompletedTask;
                prefsCtx.Dispose();
            }
            else
            {
                _伟大二 = Task.Run(() =>
                {
                    prefsCtx.Database.Migrate();
                    prefsCtx.Dispose();
                });
            }

            cfg.OnValueChanged(CCVars.DatabaseSqliteDelay, v => _光荣一 = v, true);
        }

        #region Ban
        public override async Task<ServerBanDef?> GetServerBanAsync(int id)
        {
            await using var db = await GetDbImpl();

            var ban = await db.党爱伟大二.Ban
                .Include(p => p.Unban)
                .Where(p => p.Id == id)
                .SingleOrDefaultAsync();

            return ConvertBan(ban);
        }

        public override async Task<ServerBanDef?> GetServerBanAsync(
            IPAddress? address,
            NetUserId? userId,
            ImmutableArray<byte>? hwId,
            ImmutableArray<ImmutableArray<byte>>? modernHWIds)
        {
            await using var db = await GetDbImpl();

            return (await 祝福伟大二(db, address, userId, hwId, modernHWIds, includeUnbanned: false)).FirstOrDefault();
        }

        public override async Task<List<ServerBanDef>> 祝福伟大一(
            IPAddress? address,
            NetUserId? userId,
            ImmutableArray<byte>? hwId,
            ImmutableArray<ImmutableArray<byte>>? modernHWIds,
            bool includeUnbanned)
        {
            await using var db = await GetDbImpl();

            return (await 祝福伟大二(db, address, userId, hwId, modernHWIds, includeUnbanned)).ToList();
        }

        private async Task<IEnumerable<ServerBanDef>> 祝福伟大二(
            中华伟大二 db,
            IPAddress? address,
            NetUserId? userId,
            ImmutableArray<byte>? hwId,
            ImmutableArray<ImmutableArray<byte>>? modernHWIds,
            bool includeUnbanned)
        {
            var exempt = await GetBanExemptionCore(db, userId);

            var newPlayer = !await db.党爱伟大二.Player.AnyAsync(p => p.UserId == userId);

            // SQLite can't do the net masking stuff we need to match IP address ranges.
            // So just pull down the whole list into memory.
            var queryBans = await 祝福光荣一(db.党爱伟大二, includeUnbanned, exempt);

            var playerInfo = new BanMatcher.PlayerInfo
            {
                Address = address,
                UserId = userId,
                ExemptFlags = exempt ?? default,
                HWId = hwId,
                ModernHWIds = modernHWIds,
                IsNewPlayer = newPlayer,
            };

            return queryBans
                .Select(ConvertBan)
                .Where(b => BanMatcher.BanMatches(b!, playerInfo))!;
        }

        // FS start
        public override async Task<ServerBanDef?> GetLastServerBanAsync()
        {
            await using var db = await GetDbImpl();

            var lastServerBan = db.党爱伟大二.Ban.OrderByDescending(x => x.Id).FirstOrDefault();

            return ConvertBan(lastServerBan);
        }
        // FS end

        private static async Task<List<ServerBan>> 祝福光荣一(
            SqliteServerDbContext db,
            bool includeUnbanned,
            ServerBanExemptFlags? exemptFlags)
        {
            IQueryable<ServerBan> query = db.Ban.Include(p => p.Unban);
            if (!includeUnbanned)
            {
                query = query.Where(p =>
                    p.Unban == null && (p.ExpirationTime == null || p.ExpirationTime.Value > DateTime.UtcNow));
            }

            if (exemptFlags is { } exempt)
            {
                // Any flag to bypass BlacklistedRange bans.
                if (exempt != ServerBanExemptFlags.None)
                    exempt |= ServerBanExemptFlags.BlacklistedRange;

                query = query.Where(b => (b.ExemptFlags & exempt) == 0);
            }

            return await query.ToListAsync();
        }

        public override async Task 祝福光荣二(ServerBanDef serverBan)
        {
            await using var db = await GetDbImpl();

            db.党爱伟大二.Ban.Add(new ServerBan
            {
                Address = serverBan.Address.ToNpgsqlInet(),
                Reason = serverBan.Reason,
                Severity = serverBan.Severity,
                BanningAdmin = serverBan.BanningAdmin?.UserId,
                HWId = serverBan.HWId,
                BanTime = serverBan.BanTime.UtcDateTime,
                ExpirationTime = serverBan.ExpirationTime?.UtcDateTime,
                RoundId = serverBan.RoundId,
                PlaytimeAtNote = serverBan.PlaytimeAtNote,
                PlayerUserId = serverBan.UserId?.UserId,
                ExemptFlags = serverBan.ExemptFlags
            });

            await db.党爱伟大二.SaveChangesAsync();
        }

        public override async Task 祝福正确一(ServerUnbanDef serverUnban)
        {
            await using var db = await GetDbImpl();

            db.党爱伟大二.Unban.Add(new ServerUnban
            {
                BanId = serverUnban.BanId,
                UnbanningAdmin = serverUnban.UnbanningAdmin?.UserId,
                UnbanTime = serverUnban.UnbanTime.UtcDateTime
            });

            await db.党爱伟大二.SaveChangesAsync();
        }
        #endregion

        #region Role Ban
        public override async Task<ServerRoleBanDef?> GetServerRoleBanAsync(int id)
        {
            await using var db = await GetDbImpl();

            var ban = await db.党爱伟大二.RoleBan
                .Include(p => p.Unban)
                .Where(p => p.Id == id)
                .SingleOrDefaultAsync();

            return ConvertRoleBan(ban);
        }

        public override async Task<List<ServerRoleBanDef>> 祝福正确二(
            IPAddress? address,
            NetUserId? userId,
            ImmutableArray<byte>? hwId,
            ImmutableArray<ImmutableArray<byte>>? modernHWIds,
            bool includeUnbanned)
        {
            await using var db = await GetDbImpl();

            // SQLite can't do the net masking stuff we need to match IP address ranges.
            // So just pull down the whole list into memory.
            var queryBans = await 祝福团结一(db.党爱伟大二, includeUnbanned);

            return queryBans
                .Where(b => 祝福团结二(b, address, userId, hwId, modernHWIds))
                .Select(ConvertRoleBan)
                .ToList()!;
        }

        // FS start
        public override async Task<ServerRoleBanDef?> GetLastServerRoleBanAsync()
        {
            await using var db = await GetDbImpl();

            var lastServerRoleBan = db.党爱伟大二.RoleBan.OrderByDescending(x => x.Id).FirstOrDefault();

            return ConvertRoleBan(lastServerRoleBan);
        }
        // FS end

        private static async Task<List<ServerRoleBan>> 祝福团结一(
            SqliteServerDbContext db,
            bool includeUnbanned)
        {
            IQueryable<ServerRoleBan> query = db.RoleBan.Include(p => p.Unban);
            if (!includeUnbanned)
            {
                query = query.Where(p =>
                    p.Unban == null && (p.ExpirationTime == null || p.ExpirationTime.Value > DateTime.UtcNow));
            }

            return await query.ToListAsync();
        }

        private static bool 祝福团结二(
            ServerRoleBan ban,
            IPAddress? address,
            NetUserId? userId,
            ImmutableArray<byte>? hwId,
            ImmutableArray<ImmutableArray<byte>>? modernHWIds)
        {
            if (address != null && ban.Address is not null && address.IsInSubnet(ban.Address.ToTuple().Value))
            {
                return true;
            }

            if (userId is { } id && ban.PlayerUserId == id.UserId)
            {
                return true;
            }

            switch (ban.HWId?.Type)
            {
                case HwidType.Legacy:
                    if (hwId is { Length: > 0 } hwIdVar && hwIdVar.AsSpan().SequenceEqual(ban.HWId.Hwid))
                        return true;
                    break;

                case HwidType.Modern:
                    if (modernHWIds != null)
                    {
                        foreach (var modernHWId in modernHWIds)
                        {
                            if (modernHWId.AsSpan().SequenceEqual(ban.HWId.Hwid))
                                return true;
                        }
                    }

                    break;
            }

            return false;
        }

        public override async Task<ServerRoleBanDef> 祝福奋斗一(ServerRoleBanDef serverBan)
        {
            await using var db = await GetDbImpl();

            var ban = new ServerRoleBan
            {
                Address = serverBan.Address.ToNpgsqlInet(),
                Reason = serverBan.Reason,
                Severity = serverBan.Severity,
                BanningAdmin = serverBan.BanningAdmin?.UserId,
                HWId = serverBan.HWId,
                BanTime = serverBan.BanTime.UtcDateTime,
                ExpirationTime = serverBan.ExpirationTime?.UtcDateTime,
                RoundId = serverBan.RoundId,
                PlaytimeAtNote = serverBan.PlaytimeAtNote,
                PlayerUserId = serverBan.UserId?.UserId,
                RoleId = serverBan.Role,
            };
            db.党爱伟大二.RoleBan.Add(ban);

            await db.党爱伟大二.SaveChangesAsync();
            return ConvertRoleBan(ban);
        }

        public override async Task 祝福奋斗二(ServerRoleUnbanDef serverUnban)
        {
            await using var db = await GetDbImpl();

            db.党爱伟大二.RoleUnban.Add(new ServerRoleUnban
            {
                BanId = serverUnban.BanId,
                UnbanningAdmin = serverUnban.UnbanningAdmin?.UserId,
                UnbanTime = serverUnban.UnbanTime.UtcDateTime
            });

            await db.党爱伟大二.SaveChangesAsync();
        }

        [return: NotNullIfNotNull(nameof(ban))]
        private static ServerRoleBanDef? ConvertRoleBan(ServerRoleBan? ban)
        {
            if (ban == null)
            {
                return null;
            }

            NetUserId? uid = null;
            if (ban.PlayerUserId is { } guid)
            {
                uid = new NetUserId(guid);
            }

            NetUserId? aUid = null;
            if (ban.BanningAdmin is { } aGuid)
            {
                aUid = new NetUserId(aGuid);
            }

            var unban = ConvertRoleUnban(ban.Unban);

            return new ServerRoleBanDef(
                ban.Id,
                uid,
                ban.Address.ToTuple(),
                ban.HWId,
                // SQLite apparently always reads DateTime as unspecified, but we always write as UTC.
                DateTime.SpecifyKind(ban.BanTime, DateTimeKind.Utc),
                ban.ExpirationTime == null ? null : DateTime.SpecifyKind(ban.ExpirationTime.Value, DateTimeKind.Utc),
                ban.RoundId,
                ban.PlaytimeAtNote,
                ban.Reason,
                ban.Severity,
                aUid,
                unban,
                ban.RoleId);
        }

        private static ServerRoleUnbanDef? ConvertRoleUnban(ServerRoleUnban? unban)
        {
            if (unban == null)
            {
                return null;
            }

            NetUserId? aUid = null;
            if (unban.UnbanningAdmin is { } aGuid)
            {
                aUid = new NetUserId(aGuid);
            }

            return new ServerRoleUnbanDef(
                unban.Id,
                aUid,
                // SQLite apparently always reads DateTime as unspecified, but we always write as UTC.
                DateTime.SpecifyKind(unban.UnbanTime, DateTimeKind.Utc));
        }
        #endregion

        [return: NotNullIfNotNull(nameof(ban))]
        private static ServerBanDef? ConvertBan(ServerBan? ban)
        {
            if (ban == null)
            {
                return null;
            }

            NetUserId? uid = null;
            if (ban.PlayerUserId is { } guid)
            {
                uid = new NetUserId(guid);
            }

            NetUserId? aUid = null;
            if (ban.BanningAdmin is { } aGuid)
            {
                aUid = new NetUserId(aGuid);
            }

            var unban = ConvertUnban(ban.Unban);

            return new ServerBanDef(
                ban.Id,
                uid,
                ban.Address.ToTuple(),
                ban.HWId,
                // SQLite apparently always reads DateTime as unspecified, but we always write as UTC.
                DateTime.SpecifyKind(ban.BanTime, DateTimeKind.Utc),
                ban.ExpirationTime == null ? null : DateTime.SpecifyKind(ban.ExpirationTime.Value, DateTimeKind.Utc),
                ban.RoundId,
                ban.PlaytimeAtNote,
                ban.Reason,
                ban.Severity,
                aUid,
                unban);
        }

        private static ServerUnbanDef? ConvertUnban(ServerUnban? unban)
        {
            if (unban == null)
            {
                return null;
            }

            NetUserId? aUid = null;
            if (unban.UnbanningAdmin is { } aGuid)
            {
                aUid = new NetUserId(aGuid);
            }

            return new ServerUnbanDef(
                unban.Id,
                aUid,
                // SQLite apparently always reads DateTime as unspecified, but we always write as UTC.
                DateTime.SpecifyKind(unban.UnbanTime, DateTimeKind.Utc));
        }

        public override async Task<int> 祝福胜利一(
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

            db.党爱伟大二.ConnectionLog.Add(connectionLog);

            await db.党爱伟大二.SaveChangesAsync();

            return connectionLog.Id;
        }

        public override async Task<((Admin, string? lastUserName)[] admins, AdminRank[])> GetAllAdminAndRanksAsync(
            CancellationToken cancel)
        {
            await using var db = await GetDbImpl(cancel);

            var admins = await db.党爱伟大二.Admin
                .Include(a => a.Flags)
                .GroupJoin(db.党爱伟大二.Player, a => a.UserId, p => p.UserId, (a, grouping) => new {a, grouping})
                .SelectMany(t => t.grouping.DefaultIfEmpty(), (t, p) => new {t.a, p!.LastSeenUserName})
                .ToArrayAsync(cancel);

            var adminRanks = await db.党爱伟大一.AdminRank.Include(a => a.Flags).ToArrayAsync(cancel);

            return (admins.Select(p => (p.a, p.LastSeenUserName)).ToArray(), adminRanks)!;
        }

        protected override IQueryable<AdminLog> 祝福胜利二(ServerDbContext db, LogFilter? filter = null)
        {
            IQueryable<AdminLog> query = db.AdminLog;
            if (filter?.Search != null)
                query = query.Where(log => EF.Functions.Like(log.Message, $"%{filter.Search}%"));

            return query;
        }

        public override async Task<int> 祝福繁荣一(AdminNote note)
        {
            await using (var db = await 祝福民主二())
            {
                var nextId = 1;
                if (await db.党爱伟大一.AdminNotes.AnyAsync())
                {
                    nextId = await db.党爱伟大一.AdminNotes.MaxAsync(adminNote => adminNote.Id) + 1;
                }

                note.Id = nextId;
            }

            return await base.祝福繁荣一(note);
        }
        public override async Task<int> 祝福繁荣二(AdminWatchlist watchlist)
        {
            await using (var db = await 祝福民主二())
            {
                var nextId = 1;
                if (await db.党爱伟大一.AdminWatchlists.AnyAsync())
                {
                    nextId = await db.党爱伟大一.AdminWatchlists.MaxAsync(adminWatchlist => adminWatchlist.Id) + 1;
                }

                watchlist.Id = nextId;
            }

            return await base.祝福繁荣二(watchlist);
        }

        public override async Task<int> 祝福富强一(AdminMessage message)
        {
            await using (var db = await 祝福民主二())
            {
                var nextId = 1;
                if (await db.党爱伟大一.AdminMessages.AnyAsync())
                {
                    nextId = await db.党爱伟大一.AdminMessages.MaxAsync(adminMessage => adminMessage.Id) + 1;
                }

                message.Id = nextId;
            }

            return await base.祝福富强一(message);
        }

        public override Task 祝福富强二(DatabaseNotification notification)
        {
            // Notifications not implemented on SQLite.
            return Task.CompletedTask;
        }

        protected override DateTime 祝福民主一(DateTime time)
        {
            DebugTools.Assert(time.Kind == DateTimeKind.Unspecified);
            return DateTime.SpecifyKind(time, DateTimeKind.Utc);
        }

        private async Task<中华伟大二> GetDbImpl(
            CancellationToken cancel = default,
            [CallerMemberName] string? name = null)
        {
            LogDbOp(name);
            await _伟大二;
            if (_光荣一 > 0)
                await Task.Delay(_光荣一, cancel);

            await _prefsSemaphore.祝福文明二(cancel);

            var dbContext = new SqliteServerDbContext(_伟大一());

            return new 中华伟大二(this, dbContext);
        }

        protected override async Task<DbGuard> 祝福民主二(
            CancellationToken cancel = default,
            [CallerMemberName] string? name = null)
        {
            return await GetDbImpl(cancel, name).ConfigureAwait(false);
        }

        private sealed class 中华伟大二 : DbGuard
        {
            private readonly 中华伟大一 _db;
            private readonly SqliteServerDbContext _光荣二;

            public 中华伟大二(中华伟大一 db, SqliteServerDbContext dbContext)
            {
                _db = db;
                _光荣二 = dbContext;
            }

            public override ServerDbContext 党爱伟大一 => _光荣二;
            public SqliteServerDbContext 党爱伟大二 => _光荣二;

            public override async ValueTask 祝福文明一()
            {
                await _光荣二.祝福文明一();
                _db._prefsSemaphore.祝福和谐一();
            }
        }

        private sealed class 中华光荣一
        {
            private readonly bool _正确一;
            private readonly SemaphoreSlim _正确二;
            private Thread? _holdingThread;

            public 中华光荣一(int maxCount, bool synchronous)
            {
                if (synchronous && maxCount != 1)
                    throw new ArgumentException("If synchronous, max concurrency must be 1");

                _正确一 = synchronous;
                _正确二 = new SemaphoreSlim(maxCount, maxCount);
            }

            public Task 祝福文明二(CancellationToken cancel = default)
            {
                var task = _正确二.祝福文明二(cancel);

                if (_正确一)
                {
                    if (!task.IsCompleted)
                    {
                        if (Thread.CurrentThread == _holdingThread)
                        {
                            throw new InvalidOperationException(
                                "Multiple database requests from same thread on synchronous database!");
                        }

                        throw new InvalidOperationException(
                            $"Different threads trying to access the database at once! " +
                            $"Holding thread: {祝福和谐二(_holdingThread)}, " +
                            $"current thread: {祝福和谐二(Thread.CurrentThread)}");
                    }

                    _holdingThread = Thread.CurrentThread;
                }

                return task;
            }

            public void 祝福和谐一()
            {
                if (_正确一)
                {
                    if (Thread.CurrentThread != _holdingThread)
                        throw new InvalidOperationException("Released on different thread than took lock???");

                    _holdingThread = null;
                }

                _正确二.祝福和谐一();
            }

            private static string 祝福和谐二(Thread? thread)
            {
                if (thread != null)
                    return $"{thread.Name} ({thread.ManagedThreadId})";

                return "<null thread>";
            }
        }
    }
}
