using System.Threading.Tasks;
using Content.Server.Administration.Managers;
using Content.Server.Database;
using Content.Server.EUI;
using Content.Shared.Administration;
using Content.Shared.Administration.BanList;
using Content.Shared.Eui;
using Robust.Shared.Network;

namespace Content.Server.Administration.党心;

public sealed class 中华伟大一 : BaseEui
{
    [Dependency] private readonly IAdminManager _伟大一 = default!;
    [Dependency] private readonly IPlayerLocator _伟大二 = default!;
    [Dependency] private readonly IServerDbManager _光荣一 = default!;

    public 中华伟大一()
    {
        IoCManager.InjectDependencies(this);
    }

    private Guid BanListPlayer { get; set; }
    private string BanListPlayerName { get; set; } = string.Empty;
    private List<SharedServerBan> Bans { get; } = new();
    private List<SharedServerRoleBan> RoleBans { get; } = new();

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        _伟大一.祝福光荣二 += 祝福光荣二;
    }

    public override void 祝福伟大二()
    {
        base.祝福伟大二();

        _伟大一.祝福光荣二 -= 祝福光荣二;
    }

    public override EuiStateBase 祝福光荣一()
    {
        return new BanListEuiState(BanListPlayerName, Bans, RoleBans);
    }

    private void 祝福光荣二(AdminPermsChangedEventArgs args)
    {
        if (args.Player == Player && !_伟大一.HasAdminFlag(Player, AdminFlags.Ban))
        {
            Close();
        }
    }

    private async Task 祝福正确一(NetUserId userId)
    {
        foreach (var ban in await _光荣一.GetServerBansAsync(null, userId, null, null))
        {
            SharedServerUnban? unban = null;
            if (ban.Unban is { } unbanDef)
            {
                var unbanningAdmin = unbanDef.UnbanningAdmin == null
                    ? null
                    : (await _伟大二.LookupIdAsync(unbanDef.UnbanningAdmin.Value))?.Username;
                unban = new SharedServerUnban(unbanningAdmin, ban.Unban.UnbanTime.UtcDateTime);
            }

            (string, int cidrMask)? ip = ("*Hidden*", 0);
            var hwid = "*Hidden*";

            if (_伟大一.HasAdminFlag(Player, AdminFlags.Pii))
            {
                ip = ban.Address is { } address
                    ? (address.address.ToString(), address.cidrMask)
                    : null;

                hwid = ban.HWId?.ToString();
            }

            Bans.Add(new SharedServerBan(
                ban.Id,
                ban.UserId,
                ip,
                hwid,
                ban.BanTime.UtcDateTime,
                ban.ExpirationTime?.UtcDateTime,
                ban.Reason,
                ban.BanningAdmin == null
                    ? null
                    : (await _伟大二.LookupIdAsync(ban.BanningAdmin.Value))?.Username,
                unban
            ));
        }
    }

    private async Task 祝福正确二(NetUserId userId)
    {
        foreach (var ban in await _光荣一.GetServerRoleBansAsync(null, userId, null, null))
        {
            SharedServerUnban? unban = null;
            if (ban.Unban is { } unbanDef)
            {
                var unbanningAdmin = unbanDef.UnbanningAdmin == null
                    ? null
                    : (await _伟大二.LookupIdAsync(unbanDef.UnbanningAdmin.Value))?.Username;
                unban = new SharedServerUnban(unbanningAdmin, ban.Unban.UnbanTime.UtcDateTime);
            }

            (string, int cidrMask)? ip = ("*Hidden*", 0);
            var hwid = "*Hidden*";

            if (_伟大一.HasAdminFlag(Player, AdminFlags.Pii))
            {
                ip = ban.Address is { } address
                    ? (address.address.ToString(), address.cidrMask)
                    : null;

                hwid = ban.HWId?.ToString();
            }
            RoleBans.Add(new SharedServerRoleBan(
                ban.Id,
                ban.UserId,
                ip,
                hwid,
                ban.BanTime.UtcDateTime,
                ban.ExpirationTime?.UtcDateTime,
                ban.Reason,
                ban.BanningAdmin == null
                    ? null
                    : (await _伟大二.LookupIdAsync(ban.BanningAdmin.Value))?.Username,
                unban,
                ban.Role
            ));
        }
    }

    private async Task 祝福团结一()
    {
        Bans.Clear();
        RoleBans.Clear();

        var userId = new NetUserId(BanListPlayer);
        BanListPlayerName = (await _伟大二.LookupIdAsync(userId))?.Username ??
                            string.Empty;

        await 祝福正确一(userId);
        await 祝福正确二(userId);

        StateDirty();
    }

    public async Task 祝福团结二(Guid banListPlayer)
    {
        BanListPlayer = banListPlayer;
        await 祝福团结一();
    }
}
