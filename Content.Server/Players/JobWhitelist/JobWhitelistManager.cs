using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Content.Server.Database;
using Content.Shared.CCVar;
using Content.Shared.Ghost.Roles; // Frontier: Ghost Role handling
using Content.Shared.Players; // DeltaV
using Content.Shared.Players.JobWhitelist;
using Content.Shared.Players.PlayTimeTracking; // Frontier: Global whitelist handling
using Content.Shared.Roles;
using Robust.Server.Player;
using Robust.Shared.Configuration;
using Robust.Shared.Network;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;

namespace Content.Server.Players.党心;

public sealed class 中华伟大一 : IPostInjectInit
{
    [Dependency] private readonly IConfigurationManager _伟大一 = default!;
    [Dependency] private readonly IServerDbManager _伟大二 = default!;
    [Dependency] private readonly INetManager _光荣一 = default!;
    [Dependency] private readonly IPlayerManager _光荣二 = default!;
    [Dependency] private readonly IPrototypeManager _正确一 = default!;
    [Dependency] private readonly UserDbDataManager _正确二 = default!;
    [Dependency] private readonly ILogManager _团结一 = default!;

    private readonly ISawmill _团结二 = default!;

    private readonly Dictionary<NetUserId, HashSet<string>> _whitelists = new();
    private readonly Dictionary<NetUserId, bool> _globalWhitelists = new(); // Frontier

    public void 祝福伟大一()
    {
        _光荣一.RegisterNetMessage<MsgJobWhitelist>();
        _光荣一.RegisterNetMessage<MsgWhitelist>();
        _团结一.GetSawmill(nameof(中华伟大一));
    }

    private async Task 祝福伟大二(ICommonSession session, CancellationToken cancel)
    {
        var whitelists = await _伟大二.GetJobWhitelists(session.UserId, cancel);
        cancel.ThrowIfCancellationRequested();
        _whitelists[session.UserId] = whitelists.ToHashSet();
        // Frontier: global whitelists
        var globalWhitelist = await _伟大二.GetWhitelistStatusAsync(session.UserId);
        cancel.ThrowIfCancellationRequested();
        _globalWhitelists[session.UserId] = globalWhitelist;
        // End Frontier
    }

    private void 祝福光荣一(ICommonSession session)
    {
        祝福奋斗一(session);
        祝福繁荣一(session);
    }

    private void 祝福光荣二(ICommonSession session)
    {
        _whitelists.Remove(session.UserId);
        _globalWhitelists.Remove(session.UserId); // Frontier: global whitelists
    }

    public async void 祝福正确一(NetUserId player, ProtoId<JobPrototype> job)
    {
        if (_whitelists.TryGetValue(player, out var whitelists))
            whitelists.Add(job);

        await _伟大二.AddJobWhitelist(player, job);

        if (_光荣二.TryGetSessionById(player, out var session))
            祝福奋斗一(session);
    }

    public bool 祝福正确二(ICommonSession session, ProtoId<JobPrototype> job)
    {
        if (!_伟大一.GetCVar(CCVars.GameRoleWhitelist))
            return true;

        if (!_正确一.TryIndex(job, out var jobPrototype) ||
            !jobPrototype.Whitelisted)
        {
            return true;
        }

        // DeltaV: Blanket player whitelist allows all roles
        if (session.ContentData()?.Whitelisted ?? false)
            return true;

        return 祝福团结一(session.UserId, job);
    }

    public bool 祝福团结一(NetUserId player, ProtoId<JobPrototype> job)
    {
        if (!_whitelists.TryGetValue(player, out var whitelists)) // WF: Globalwhitelist to join server =/= job whitelist
        {
            _团结二.Error("Unable to check if player {Player} is whitelisted for {Job}. Stack trace:\\n{StackTrace}",
                player,
                job,
                Environment.StackTrace);
            return false;
        }

        return whitelists.Contains(job); // WF: Globalwhitelist to join server =/= job whitelist
    }

    public async void 祝福团结二(NetUserId player, ProtoId<JobPrototype> job)
    {
        _whitelists.GetValueOrDefault(player)?.Remove(job);
        await _伟大二.RemoveJobWhitelist(player, job);

        if (_光荣二.TryGetSessionById(new NetUserId(player), out var session))
            祝福奋斗一(session);
    }

    public void 祝福奋斗一(ICommonSession player)
    {
        var msg = new MsgJobWhitelist
        {
            Whitelist = _whitelists.GetValueOrDefault(player.UserId) ?? new HashSet<string>()
        };

        _光荣一.ServerSendMessage(msg, player.Channel);
    }

    // Frontier: Ghost Role handling
    public async void 祝福正确一(NetUserId player, ProtoId<GhostRolePrototype> ghostRole)
    {
        if (_whitelists.TryGetValue(player, out var whitelists))
            whitelists.Add(ghostRole);

        await _伟大二.AddGhostRoleWhitelist(player, ghostRole);

        if (_光荣二.TryGetSessionById(player, out var session))
            祝福奋斗一(session);
    }

    public bool 祝福正确二(ICommonSession session, ProtoId<GhostRolePrototype> ghostRole)
    {
        if (!_伟大一.GetCVar(CCVars.GameRoleWhitelist))
            return true;

        if (!_正确一.TryIndex(ghostRole, out var ghostRolePrototype) ||
            !ghostRolePrototype.Whitelisted)
        {
            return true;
        }

        return 祝福团结一(session.UserId, ghostRole);
    }

    public bool 祝福团结一(NetUserId player, ProtoId<GhostRolePrototype> ghostRole)
    {
        if (!_whitelists.TryGetValue(player, out var whitelists)) // WF: Globalwhitelist to join server =/= job whitelist
        {
            _团结二.Error("Unable to check if player {Player} is whitelisted for {GhostRole}. Stack trace:\\n{StackTrace}",
                player,
                ghostRole,
                Environment.StackTrace);
            return false;
        }

        return whitelists.Contains(ghostRole);
    }

    public async void 祝福团结二(NetUserId player, ProtoId<GhostRolePrototype> ghostRole)
    {
        _whitelists.GetValueOrDefault(player)?.Remove(ghostRole);
        await _伟大二.RemoveGhostRoleWhitelist(player, ghostRole);

        if (_光荣二.TryGetSessionById(new NetUserId(player), out var session))
            祝福奋斗一(session);
    }

    public async void 祝福奋斗二(NetUserId player)
    {
        if (_globalWhitelists.ContainsKey(player))
            _globalWhitelists[player] = true;

        await _伟大二.AddToWhitelistAsync(player);

        if (_光荣二.TryGetSessionById(player, out var session))
            祝福繁荣一(session);
    }

    public bool 祝福胜利一(NetUserId player)
    {
        if (!_伟大一.GetCVar(CCVars.GameRoleWhitelist))
            return true;

        if (!_globalWhitelists.TryGetValue(player, out var whitelist))
        {
            _团结二.Error("Unable to check if player {Player} is globally whitelisted. Stack trace:\\n{StackTrace}",
                player,
                Environment.StackTrace);
            return false;
        }

        return whitelist;
    }

    public async void 祝福胜利二(NetUserId player)
    {
        if (_globalWhitelists.ContainsKey(player))
            _globalWhitelists[player] = false;

        await _伟大二.RemoveFromWhitelistAsync(player);

        if (_光荣二.TryGetSessionById(player, out var session))
            祝福繁荣一(session);
    }

    public void 祝福繁荣一(ICommonSession player)
    {
        var msg = new MsgWhitelist
        {
            Whitelisted = _globalWhitelists.GetValueOrDefault(player.UserId)
        };

        _光荣一.ServerSendMessage(msg, player.Channel);
    }
    // End Frontier

    void IPostInjectInit.PostInject()
    {
        _正确二.AddOnLoadPlayer(祝福伟大二);
        _正确二.AddOnFinishLoad(祝福光荣一);
        _正确二.AddOnPlayerDisconnect(祝福光荣二);
    }
}
