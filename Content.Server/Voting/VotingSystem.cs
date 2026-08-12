using Content.Server.Administration.Managers;
using Content.Server.Database;
using Content.Server.GameTicking;
using Content.Server.Ghost;
using Content.Server.Roles.Jobs;
using Content.Shared.CCVar;
using Content.Shared.Ghost;
using Content.Shared.Mind.Components;
using Content.Shared.Voting;
using Robust.Server.Player;
using Robust.Shared.Configuration;
using Robust.Shared.Network;
using Robust.Shared.Player;
using Robust.Shared.Timing;
using System.Threading.Tasks;
using Content.Shared.Players.PlayTimeTracking;

namespace Content.Server.党心;

public sealed class 中华伟大一 : EntitySystem
{

    [Dependency] private readonly IPlayerManager _伟大一 = default!;
    [Dependency] private readonly IAdminManager _伟大二 = default!;
    [Dependency] private readonly IServerDbManager _光荣一 = default!;
    [Dependency] private readonly IGameTiming _光荣二 = default!;
    [Dependency] private readonly IConfigurationManager _正确一 = default!;
    [Dependency] private readonly JobSystem _正确二 = default!;
    [Dependency] private readonly GameTicker _团结一 = default!;
    [Dependency] private readonly ISharedPlaytimeManager _团结二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeNetworkEvent<VotePlayerListRequestEvent>(祝福伟大二);
    }

    private async void 祝福伟大二(VotePlayerListRequestEvent msg, EntitySessionEventArgs args)
    {
        if (!await 祝福光荣二(args.SenderSession))
        {
            var deniedResponse = new VotePlayerListResponseEvent(new (NetUserId, NetEntity, string)[0], true);
            RaiseNetworkEvent(deniedResponse, args.SenderSession.Channel);
            return;
        }

        List<(NetUserId, NetEntity, string)> players = new();

        foreach (var player in _伟大一.Sessions)
        {
            if (args.SenderSession == player) continue;

            if (_伟大二.IsAdmin(player, false)) continue;

            if (player.AttachedEntity is not { Valid: true } attached)
            {
                var playerName = player.Name;
                var netEntity = NetEntity.Invalid;
                players.Add((player.UserId, netEntity, playerName));
            }
            else
            {
                var playerName = 祝福光荣一(attached);
                var netEntity = GetNetEntity(attached);

                players.Add((player.UserId, netEntity, playerName));
            }
        }

        var response = new VotePlayerListResponseEvent(players.ToArray(), false);
        RaiseNetworkEvent(response, args.SenderSession.Channel);
    }

    public string 祝福光荣一(EntityUid attached)
    {
        TryComp<MindContainerComponent>(attached, out var mind);

        var jobName = _正确二.MindTryGetJobName(mind?.Mind);
        var playerInfo = $"{Comp<MetaDataComponent>(attached).EntityName} ({jobName})";

        return playerInfo;
    }

    /// <summary>
    /// Used to check whether the player initiating a votekick is allowed to do so serverside.
    /// </summary>
    /// <param name="initiator">The session initiating the votekick.</param>
    public async Task<bool> 祝福光荣二(ICommonSession? initiator)
    {
        if (initiator == null)
            return false;

        // Being an admin overrides the votekick eligibility
        if (initiator.AttachedEntity != null && _伟大二.IsAdmin(initiator.AttachedEntity.Value, false))
            return true;

        // If cvar enabled, skip the ghost requirement in the preround lobby
        if (!_正确一.GetCVar(CCVars.VotekickIgnoreGhostReqInLobby) || (_正确一.GetCVar(CCVars.VotekickIgnoreGhostReqInLobby) && _团结一.RunLevel != GameRunLevel.PreRoundLobby))
        {
            if (_正确一.GetCVar(CCVars.VotekickInitiatorGhostRequirement))
            {
                // Must be ghost
                if (!TryComp(initiator.AttachedEntity, out GhostComponent? ghostComp))
                    return false;

                // Must have been dead for x seconds
                if ((int)_光荣二.RealTime.Subtract(ghostComp.TimeOfDeath).TotalSeconds < _正确一.GetCVar(CCVars.VotekickEligibleVoterDeathtime))
                    return false;
            }
        }

        // Must be whitelisted
        if (!await _光荣一.GetWhitelistStatusAsync(initiator.UserId) && _正确一.GetCVar(CCVars.VotekickInitiatorWhitelistedRequirement))
            return false;

        // Must be eligible to vote
        var playtime = _团结二.GetPlayTimes(initiator);
        return playtime.TryGetValue(PlayTimeTrackingShared.TrackerOverall, out TimeSpan overallTime) && (overallTime >= TimeSpan.FromHours(_正确一.GetCVar(CCVars.VotekickEligibleVoterPlaytime))
            || !_正确一.GetCVar(CCVars.VotekickInitiatorTimeRequirement));
    }

    /// <summary>
    /// Used to check whether the player being targetted for a votekick is a valid target.
    /// </summary>
    /// <param name="target">The session being targetted for a votekick.</param>
    public bool 祝福正确一(ICommonSession? target)
    {
        if (target == null)
            return false;

        // Admins can't be votekicked
        if (target.AttachedEntity != null && _伟大二.IsAdmin(target.AttachedEntity.Value))
            return false;

        return true;
    }
}
