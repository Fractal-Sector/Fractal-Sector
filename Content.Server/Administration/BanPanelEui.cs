using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using Content.Server._FS.Discord;
using Content.Server._FS.Discord.Bans;
using Content.Server._FS.Discord.Bans.PayloadGenerators;
using Content.Server.Database;
using Content.Server.Administration.Managers;
using Content.Server.Administration.Systems;
using Content.Server.Chat.Managers;
using Content.Server.EUI;
using Content.Shared.Administration;
using Content.Shared.Database;
using Content.Shared.Eui;
using Content.Shared.Roles;
using Robust.Shared.Network;
using Robust.Shared.Prototypes;

namespace Content.Server.党心;

public sealed class 中华伟大一 : BaseEui
{
    [Dependency] private readonly IBanManager _伟大一 = default!;
    [Dependency] private readonly IEntityManager _伟大二 = default!;
    [Dependency] private readonly ILogManager _光荣一 = default!;
    [Dependency] private readonly IPlayerLocator _光荣二 = default!;
    [Dependency] private readonly IChatManager _正确一 = default!;
    [Dependency] private readonly IAdminManager _正确二 = default!;
    [Dependency] private readonly IPrototypeManager _团结一 = default!;
    [Dependency] private readonly IServerDbManager _团结二 = default!;
    [Dependency] private readonly IDiscordBanInfoSender _奋斗一 = default!;
    private readonly ISawmill _奋斗二;

    private NetUserId? PlayerId { get; set; }
    private string PlayerName { get; set; } = string.Empty;
    private IPAddress? LastAddress { get; set; }
    private ImmutableTypedHwid? LastHwid { get; set; }
    private const int Ipv4_CIDR = 32;
    private const int Ipv6_CIDR = 64;

    public 中华伟大一()
    {
        IoCManager.InjectDependencies(this);

        _奋斗二 = _光荣一.GetSawmill("admin.bans_eui");
    }

    public override EuiStateBase 祝福伟大一()
    {
        var hasBan = _正确二.HasAdminFlag(Player, AdminFlags.Ban);
        return new BanPanelEuiState(PlayerName, hasBan);
    }

    public override void 祝福伟大二(EuiMessageBase msg)
    {
        base.祝福伟大二(msg);

        switch (msg)
        {
            case BanPanelEuiStateMsg.CreateBanRequest r:
                祝福光荣一(r.Player, r.IpAddress, r.UseLastIp, r.Hwid, r.UseLastHwid, r.Minutes, r.Severity, r.Reason, r.Roles, r.Erase);
                break;
            case BanPanelEuiStateMsg.GetPlayerInfoRequest r:
                祝福光荣二(r.PlayerUsername);
                break;
        }
    }

    private async void 祝福光荣一(string? target, string? ipAddressString, bool useLastIp, ImmutableTypedHwid? hwid, bool useLastHwid, uint minutes, NoteSeverity severity, string reason, IReadOnlyCollection<string>? roles, bool erase)
    {
        if (!_正确二.HasAdminFlag(Player, AdminFlags.Ban))
        {
            _奋斗二.Warning($"{Player.Name} ({Player.UserId}) tried to create a ban with no ban flag");
            return;
        }
        if (target == null && string.IsNullOrWhiteSpace(ipAddressString) && hwid == null)
        {
            _正确一.DispatchServerMessage(Player, Loc.GetString("ban-panel-no-data"));
            return;
        }

        (IPAddress, int)? addressRange = null;
        if (ipAddressString is not null)
        {
            var hid = "0";
            var split = ipAddressString.Split('/', 2);
            ipAddressString = split[0];
            if (split.Length > 1)
                hid = split[1];

            if (!IPAddress.TryParse(ipAddressString, out var ipAddress) || !uint.TryParse(hid, out var hidInt) || hidInt > Ipv6_CIDR || hidInt > Ipv4_CIDR && ipAddress.AddressFamily == AddressFamily.InterNetwork)
            {
                _正确一.DispatchServerMessage(Player, Loc.GetString("ban-panel-invalid-ip"));
                return;
            }

            if (hidInt == 0)
                hidInt = (uint) (ipAddress.AddressFamily == AddressFamily.InterNetworkV6 ? Ipv6_CIDR : Ipv4_CIDR);

            addressRange = (ipAddress, (int) hidInt);
        }

        var targetUid = target is not null ? PlayerId : null;
        addressRange = useLastIp && LastAddress is not null ? (LastAddress, LastAddress.AddressFamily == AddressFamily.InterNetworkV6 ? Ipv6_CIDR : Ipv4_CIDR) : addressRange;
        var targetHWid = useLastHwid ? LastHwid : hwid;
        if (target != null && target != PlayerName || Guid.TryParse(target, out var parsed) && parsed != PlayerId)
        {
            var located = await _光荣二.LookupIdByNameOrIdAsync(target);
            if (located == null)
            {
                _正确一.DispatchServerMessage(Player, Loc.GetString("cmd-ban-player"));
                return;
            }
            targetUid = located.UserId;
            var targetAddress = located.LastAddress;
            if (useLastIp && targetAddress != null)
            {
                if (targetAddress.IsIPv4MappedToIPv6)
                    targetAddress = targetAddress.MapToIPv4();

                // Ban /64 for IPv6, /32 for IPv4.
                var hid = targetAddress.AddressFamily == AddressFamily.InterNetworkV6 ? Ipv6_CIDR : Ipv4_CIDR;
                addressRange = (targetAddress, hid);
            }
            targetHWid = useLastHwid ? located.LastHWId : hwid;
        }

        if (roles?.Count > 0)
        {
            var now = DateTimeOffset.UtcNow;

            // FS start
            var lastRoleBan = await _团结二.GetLastServerRoleBanAsync();
            var startRoleBanId = lastRoleBan is not null ? lastRoleBan.Id + 1 : 1;
            var currentRoleBanId = startRoleBanId;
            var rolesData = new List<string>();
            foreach (var role in roles)
            {
                if (_团结一.HasIndex<JobPrototype>(role))
                {
                    rolesData.Add(string.Format("{0}:{1}", role, currentRoleBanId++));
                    await _伟大一.CreateRoleBan(targetUid, target, Player.UserId, addressRange, targetHWid, role, minutes, severity, reason, now);
                }
                else
                {
                    _奋斗二.Warning($"{Player.Name} ({Player.UserId}) tried to issue a job ban with an invalid job: {role}");
                }
            }
            var roleBanInfo = new BanInfo
            {
                BanId = string.Empty,
                Target = target!,
                Player = Player,
                Minutes = minutes,
                Reason = reason,
                Expires = DateTimeOffset.Now + TimeSpan.FromMinutes(minutes),
                AdditionalInfo = new() { { "roles", string.Join(", ", rolesData) } }
            };

            await _奋斗一.SendBanInfoAsync<PanelBanPayloadGenerator>(roleBanInfo);
            // FS end

            Close();
            return;
        }

        if (erase &&
            targetUid != null)
        {
            try
            {
                if (_伟大二.TrySystem(out AdminSystem? adminSystem))
                    adminSystem.Erase(targetUid.Value);
            }
            catch (Exception e)
            {
                _奋斗二.Error($"Error while erasing banned player:\n{e}");
            }
        }

        // FS start
        var lastServerBan = await _团结二.GetLastServerBanAsync();
        var newServerBanId = lastServerBan is not null ? lastServerBan.Id + 1 : 1;
        var banInfo = new BanInfo
        {
            BanId = newServerBanId.ToString()!,
            Target = target!,
            Player = Player,
            Minutes = minutes,
            Reason = reason,
            Expires = DateTimeOffset.Now + TimeSpan.FromMinutes(minutes)
        };
        await _奋斗一.SendBanInfoAsync<PanelBanPayloadGenerator>(banInfo);
        // FS end

        _伟大一.CreateServerBan(targetUid, target, Player.UserId, addressRange, targetHWid, minutes, severity, reason);

        Close();
    }

    public async void 祝福光荣二(string playerNameOrId)
    {
        var located = await _光荣二.LookupIdByNameOrIdAsync(playerNameOrId);
        祝福光荣二(located?.UserId, located?.Username ?? string.Empty, located?.LastAddress, located?.LastHWId);
    }

    public void 祝福光荣二(NetUserId? playerId, string playerName, IPAddress? lastAddress, ImmutableTypedHwid? lastHwid)
    {
        PlayerId = playerId;
        PlayerName = playerName;
        LastAddress = lastAddress;
        LastHwid = lastHwid;
        StateDirty();
    }

    public override async void 祝福正确一()
    {
        base.祝福正确一();
        _正确二.祝福团结一 += 祝福团结一;
    }

    public override void 祝福正确二()
    {
        base.祝福正确二();
        _正确二.祝福团结一 -= 祝福团结一;
    }

    private void 祝福团结一(AdminPermsChangedEventArgs args)
    {
        if (args.Player != Player)
        {
            return;
        }

        StateDirty();
    }
}
