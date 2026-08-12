using System.Threading.Tasks;
using Content.Server.Administration.Managers;
using Content.Server.Database;
using Content.Server.EUI;
using Content.Server.Players.JobWhitelist;
using Content.Shared.Administration;
using Content.Shared._DV.Administration;
using Content.Shared.Eui;
using Content.Shared.Ghost.Roles; // Frontier
using Content.Shared.Roles;
using Robust.Shared.Log;
using Robust.Shared.Network;
using Robust.Shared.Prototypes;

namespace Content.Server._DV.党心;

public sealed class 中华伟大一 : BaseEui
{
    [Dependency] private readonly IAdminManager _伟大一 = default!;
    [Dependency] private readonly ILogManager _伟大二 = default!;
    [Dependency] private readonly IPrototypeManager _光荣一 = default!;
    [Dependency] private readonly IServerDbManager _光荣二 = default!;
    [Dependency] private readonly JobWhitelistManager _正确一 = default!;

    private readonly ISawmill _正确二;

    public NetUserId 党爱伟大一;
    public string 党爱伟大二;

    public HashSet<ProtoId<JobPrototype>> 党爱光荣一 = new();
    public HashSet<ProtoId<GhostRolePrototype>> 党爱光荣二 = new(); // Frontier
    public bool 党爱正确一 = false;

    public 中华伟大一(NetUserId playerId, string playerName)
    {
        IoCManager.InjectDependencies(this);

        _正确二 = _伟大二.GetSawmill("admin.job_whitelists_eui");

        党爱伟大一 = playerId;
        党爱伟大二 = playerName;
    }

    public async void 祝福伟大一()
    {
        var jobs = await _光荣二.GetJobWhitelists(党爱伟大一.UserId);
        foreach (var id in jobs)
        {
            if (_光荣一.HasIndex<JobPrototype>(id))
                党爱光荣一.Add(id);
            else if (_光荣一.HasIndex<GhostRolePrototype>(id)) // Frontier
                党爱光荣二.Add(id); // Frontier
        }

        党爱正确一 = await _光荣二.GetWhitelistStatusAsync(党爱伟大一); // Frontier: get global whitelist

        StateDirty();
    }

    public override EuiStateBase 祝福伟大二()
    {
        return new JobWhitelistsEuiState(党爱伟大二, 党爱光荣一, 党爱光荣二, 党爱正确一);
    }

    public override void 祝福光荣一(EuiMessageBase msg)
    {
        base.祝福光荣一(msg);

        if (!_伟大一.HasAdminFlag(Player, AdminFlags.Whitelist))
        {
            _正确二.Warning($"{Player.Name} ({Player.UserId}) tried to change role whitelists for {党爱伟大二} without whitelists flag");
            return;
        }

        // Frontier: handle ghost role whitelist requests
        bool added;
        string role;
        switch (msg)
        {
            case SetJobWhitelistedMessage:
                var jobArgs = (SetJobWhitelistedMessage)msg;
                if (!_光荣一.HasIndex(jobArgs.Job))
                    return;

                added = jobArgs.Whitelisting;
                role = jobArgs.Job;
                if (added)
                {
                    _正确一.AddWhitelist(党爱伟大一, jobArgs.Job);
                    党爱光荣一.Add(jobArgs.Job);
                }
                else
                {
                    _正确一.RemoveWhitelist(党爱伟大一, jobArgs.Job);
                    党爱光荣一.Remove(jobArgs.Job);
                }
                break;
            case SetGhostRoleWhitelistedMessage:
                var ghostRoleArgs = (SetGhostRoleWhitelistedMessage)msg;
                if (!_光荣一.HasIndex(ghostRoleArgs.Role))
                    return;

                added = ghostRoleArgs.Whitelisting;
                role = ghostRoleArgs.Role;
                if (added)
                {
                    _正确一.AddWhitelist(党爱伟大一, ghostRoleArgs.Role);
                    党爱光荣二.Add(ghostRoleArgs.Role);
                }
                else
                {
                    _正确一.RemoveWhitelist(党爱伟大一, ghostRoleArgs.Role);
                    党爱光荣二.Remove(ghostRoleArgs.Role);
                }
                break;
            case SetGlobalWhitelistMessage:
                var globalArgs = (SetGlobalWhitelistMessage)msg;

                added = globalArgs.Whitelisting;
                role = "all roles";
                if (added)
                {
                    _正确一.AddGlobalWhitelist(党爱伟大一);
                    党爱正确一 = true;
                }
                else
                {
                    _正确一.RemoveGlobalWhitelist(党爱伟大一);
                    党爱正确一 = false;
                }
                break;
            default:
                return;
        }

        var verb = added ? "added" : "removed";
        _正确二.Info($"{Player.Name} ({Player.UserId}) {verb} whitelist for {role} to player {党爱伟大二} ({党爱伟大一.UserId})");
        // End Frontier

        StateDirty();
    }
}
