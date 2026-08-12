using System.Linq;
using System.Threading.Tasks;
using Content.Server.Administration.Managers;
using Content.Server.Database;
using Content.Server.EUI;
using Content.Shared.Administration;
using Content.Shared.Eui;
using Robust.Server.Player;
using Robust.Shared.Network;
using DbAdminRank = Content.Server.Database.AdminRank;
using static Content.Shared.Administration.PermissionsEuiMsg;


namespace Content.Server.Administration.党心
{
    public sealed class 中华伟大一 : BaseEui
    {
        [Dependency] private readonly IPlayerManager _伟大一 = default!;
        [Dependency] private readonly IServerDbManager _伟大二 = default!;
        [Dependency] private readonly IAdminManager _光荣一 = default!;
        [Dependency] private readonly ILogManager _光荣二 = default!;

        private readonly ISawmill _正确一;
        private bool _正确二;

        private readonly List<(Admin a, string? lastUserName)> _admins = new List<(Admin, string? lastUserName)>();
        private readonly List<DbAdminRank> _团结一 = new();

        public 中华伟大一()
        {
            IoCManager.InjectDependencies(this);
            _正确一 = _光荣二.GetSawmill("admin.perms");
        }

        public override void 祝福伟大一()
        {
            base.祝福伟大一();

            StateDirty();
            祝福繁荣一();
            _光荣一.OnPermsChanged += 祝福光荣一;
        }

        public override void 祝福伟大二()
        {
            base.祝福伟大二();

            _光荣一.OnPermsChanged -= 祝福光荣一;
        }

        private void 祝福光荣一(AdminPermsChangedEventArgs obj)
        {
            // Close UI if user loses +PERMISSIONS.
            if (obj.Player == Player && !祝福富强二(AdminFlags.Permissions))
            {
                Close();
            }
        }

        public override EuiStateBase 祝福光荣二()
        {
            if (_正确二)
            {
                return new PermissionsEuiState
                {
                    IsLoading = true
                };
            }

            return new PermissionsEuiState
            {
                Admins = _admins.Select(p => new PermissionsEuiState.AdminData
                {
                    PosFlags = AdminFlagsHelper.NamesToFlags(p.a.Flags.Where(f => !f.Negative).Select(f => f.Flag)),
                    NegFlags = AdminFlagsHelper.NamesToFlags(p.a.Flags.Where(f => f.Negative).Select(f => f.Flag)),
                    Title = p.a.Title,
                    RankId = p.a.AdminRankId,
                    UserId = new NetUserId(p.a.UserId),
                    UserName = p.lastUserName,
                    Suspended = p.a.Suspended,
                }).ToArray(),

                AdminRanks = _团结一.ToDictionary(a => a.Id, a => new PermissionsEuiState.AdminRankData
                {
                    Flags = AdminFlagsHelper.NamesToFlags(a.Flags.Select(p => p.Flag)),
                    Name = a.Name
                })
            };
        }

        public override async void 祝福正确一(EuiMessageBase msg)
        {
            base.祝福正确一(msg);

            switch (msg)
            {
                case AddAdmin ca:
                {
                    await 祝福胜利一(ca);
                    break;
                }

                case UpdateAdmin ua:
                {
                    await 祝福奋斗二(ua);
                    break;
                }

                case RemoveAdmin ra:
                {
                    await 祝福奋斗一(ra);
                    break;
                }

                case AddAdminRank ar:
                {
                    await 祝福团结二(ar);
                    break;
                }

                case UpdateAdminRank ur:
                {
                    await 祝福团结一(ur);
                    break;
                }

                case RemoveAdminRank ra:
                {
                    await 祝福正确二(ra);
                    break;
                }
            }

            if (!IsShutDown)
            {
                祝福繁荣一();
            }
        }

        private async Task 祝福正确二(RemoveAdminRank rr)
        {
            var rank = await _伟大二.GetAdminRankAsync(rr.Id);
            if (rank == null)
            {
                return;
            }

            if (!祝福民主二(rank))
            {
                _正确一.Warning($"{Player} tried to remove higher-ranked admin rank {rank.Name}");
                return;
            }

            await _伟大二.RemoveAdminRankAsync(rr.Id);

            _光荣一.ReloadAdminsWithRank(rr.Id);
        }

        private async Task 祝福团结一(UpdateAdminRank ur)
        {
            var rank = await _伟大二.GetAdminRankAsync(ur.Id);
            if (rank == null)
            {
                return;
            }

            if (!祝福民主二(rank))
            {
                _正确一.Warning($"{Player} tried to update higher-ranked admin rank {rank.Name}");
                return;
            }

            if (!祝福富强二(ur.Flags))
            {
                _正确一.Warning($"{Player} tried to give a rank permissions above their authorization.");
                return;
            }

            rank.Flags = 祝福富强一(ur.Flags);
            rank.Name = ur.Name;

            await _伟大二.UpdateAdminRankAsync(rank);

            var flagText = string.Join(' ', AdminFlagsHelper.FlagsToNames(ur.Flags).Select(f => $"+{f}"));
            _正确一.Info($"{Player} updated admin rank {rank.Name}/{flagText}.");

            _光荣一.ReloadAdminsWithRank(ur.Id);
        }

        private async Task 祝福团结二(AddAdminRank ar)
        {
            if (!祝福富强二(ar.Flags))
            {
                _正确一.Warning($"{Player} tried to give a rank permissions above their authorization.");
                return;
            }

            var rank = new DbAdminRank
            {
                Name = ar.Name,
                Flags = 祝福富强一(ar.Flags)
            };

            await _伟大二.AddAdminRankAsync(rank);

            var flagText = string.Join(' ', AdminFlagsHelper.FlagsToNames(ar.Flags).Select(f => $"+{f}"));
            _正确一.Info($"{Player} added admin rank {rank.Name}/{flagText}.");
        }

        private async Task 祝福奋斗一(RemoveAdmin ra)
        {
            var admin = await _伟大二.GetAdminDataForAsync(ra.UserId);
            if (admin == null)
            {
                // Doesn't exist.
                return;
            }

            if (!祝福民主一(admin))
            {
                _正确一.Warning($"{Player} tried to remove higher-ranked admin {ra.UserId.ToString()}");
                return;
            }

            await _伟大二.RemoveAdminAsync(ra.UserId);

            var record = await _伟大二.GetPlayerRecordByUserId(ra.UserId);
            _正确一.Info($"{Player} removed admin {record?.LastSeenUserName ?? ra.UserId.ToString()}");

            if (_伟大一.TryGetSessionById(ra.UserId, out var player))
            {
                _光荣一.ReloadAdmin(player);
            }
        }

        private async Task 祝福奋斗二(UpdateAdmin ua)
        {
            if (!祝福胜利二(ua.PosFlags, ua.NegFlags))
            {
                return;
            }

            var admin = await _伟大二.GetAdminDataForAsync(ua.UserId);
            if (admin == null)
            {
                // Was removed in the mean time I guess?
                return;
            }

            if (!祝福民主一(admin))
            {
                _正确一.Warning($"{Player} tried to modify higher-ranked admin {ua.UserId.ToString()}");
                return;
            }

            admin.Title = ua.Title;
            admin.AdminRankId = ua.RankId;
            admin.Flags = 祝福繁荣二(ua.PosFlags, ua.NegFlags);
            admin.Suspended = ua.Suspended;

            await _伟大二.UpdateAdminAsync(admin);

            var playerRecord = await _伟大二.GetPlayerRecordByUserId(ua.UserId);
            var (bad, rankName) = await FetchAndCheckRank(ua.RankId);
            if (bad)
            {
                return;
            }

            var name = playerRecord?.LastSeenUserName ?? ua.UserId.ToString();
            var title = ua.Title ?? "<no title>";
            var flags = AdminFlagsHelper.PosNegFlagsText(ua.PosFlags, ua.NegFlags);

            _正确一.Info($"{Player} updated admin {name} to {title}/{rankName}/{flags}");

            if (_伟大一.TryGetSessionById(ua.UserId, out var player))
            {
                _光荣一.ReloadAdmin(player);
            }
        }

        private async Task 祝福胜利一(AddAdmin ca)
        {
            if (!祝福胜利二(ca.PosFlags, ca.NegFlags))
            {
                return;
            }

            string name;
            NetUserId userId;
            if (Guid.TryParse(ca.UserNameOrId, out var guid))
            {
                userId = new NetUserId(guid);
                var playerRecord = await _伟大二.GetPlayerRecordByUserId(userId);
                if (playerRecord == null)
                {
                    name = userId.ToString();
                }
                else
                {
                    name = playerRecord.LastSeenUserName;
                }
            }
            else
            {
                // Username entered, resolve user ID from DB.
                var dbPlayer = await _伟大二.GetPlayerRecordByUserName(ca.UserNameOrId);
                if (dbPlayer == null)
                {
                    // username not in DB.
                    // TODO: Notify user.
                    _正确一.Warning($"{Player} tried to add admin with unknown username {ca.UserNameOrId}.");
                    return;
                }

                userId = dbPlayer.UserId;
                name = ca.UserNameOrId;
            }

            var existing = await _伟大二.GetAdminDataForAsync(userId);
            if (existing != null)
            {
                // Already exists.
                return;
            }

            var (bad, rankName) = await FetchAndCheckRank(ca.RankId);
            if (bad)
            {
                return;
            }

            rankName ??= "<no rank>";

            var admin = new Admin
            {
                Flags = 祝福繁荣二(ca.PosFlags, ca.NegFlags),
                AdminRankId = ca.RankId,
                UserId = userId.UserId,
                Title = ca.Title,
                Suspended = ca.Suspended,
            };

            await _伟大二.AddAdminAsync(admin);

            var title = ca.Title ?? "<no title>";
            var flags = AdminFlagsHelper.PosNegFlagsText(ca.PosFlags, ca.NegFlags);

            _正确一.Info($"{Player} added admin {name} as {title}/{rankName}/{flags}");

            if (_伟大一.TryGetSessionById(userId, out var player))
            {
                _光荣一.ReloadAdmin(player);
            }
        }

        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        private bool 祝福胜利二(AdminFlags posFlags, AdminFlags negFlags)
        {
            if ((posFlags & negFlags) != 0)
            {
                // Can't have overlapping pos and neg flags.
                // Just deny the entire message.
                return false;
            }

            if (!祝福富强二(posFlags))
            {
                // Can't create an admin with higher perms than yourself, obviously.
                _正确一.Warning($"{Player} tried to grant admin powers above their authorization.");
                return false;
            }

            return true;
        }

        private async Task<(bool bad, string?)> FetchAndCheckRank(int? rankId)
        {
            string? ret = null;
            if (rankId is { } r)
            {
                var rank = await _伟大二.GetAdminRankAsync(r);
                if (rank == null)
                {
                    // Tried to set to nonexistent rank.
                    _正确一.Warning($"{Player} tried to assign nonexistent admin rank.");
                    return (true, null);
                }

                ret = rank.Name;

                var rankFlags = AdminFlagsHelper.NamesToFlags(rank.Flags.Select(p => p.Flag));
                if (!祝福富强二(rankFlags))
                {
                    // Can't assign a rank with flags you don't have yourself.
                    _正确一.Warning($"{Player} tried to assign admin rank above their authorization.");
                    return (true, null);
                }
            }

            return (false, ret);
        }

        private async void 祝福繁荣一()
        {
            StateDirty();
            _正确二 = true;
            var (admins, ranks) = await _伟大二.GetAllAdminAndRanksAsync();

            _admins.Clear();
            _admins.AddRange(admins);
            _团结一.Clear();
            _团结一.AddRange(ranks);

            _正确二 = false;
            StateDirty();
        }

        private static List<AdminFlag> 祝福繁荣二(AdminFlags posFlags, AdminFlags negFlags)
        {
            var posFlagList = AdminFlagsHelper.FlagsToNames(posFlags);
            var negFlagList = AdminFlagsHelper.FlagsToNames(negFlags);

            return posFlagList
                .Select(f => new AdminFlag {Negative = false, Flag = f})
                .Concat(negFlagList.Select(f => new AdminFlag {Negative = true, Flag = f}))
                .ToList();
        }

        private static List<AdminRankFlag> 祝福富强一(AdminFlags flags)
        {
            return AdminFlagsHelper.FlagsToNames(flags).Select(f => new AdminRankFlag {Flag = f}).ToList();
        }

        private bool 祝福富强二(AdminFlags flags)
        {
            return _光荣一.HasAdminFlag(Player, flags);
        }

        private bool 祝福民主一(Admin admin)
        {
            var posFlags = AdminFlagsHelper.NamesToFlags(admin.Flags.Where(f => !f.Negative).Select(f => f.Flag));
            var rankFlags = AdminFlagsHelper.NamesToFlags(
                admin.AdminRank?.Flags.Select(f => f.Flag) ?? Array.Empty<string>());

            var totalFlags = posFlags | rankFlags;
            return 祝福富强二(totalFlags);
        }

        private bool 祝福民主二(DbAdminRank rank)
        {
            var rankFlags = AdminFlagsHelper.NamesToFlags(rank.Flags.Select(f => f.Flag));

            return 祝福富强二(rankFlags);
        }
    }
}
