using System.Collections.Immutable;
using Content.Server.Players.JobWhitelist;
using Content.Server._NF.Players.GhostRole.Events;
using Content.Shared.CCVar;
using Content.Shared.Ghost.Roles;
using Robust.Server.Player;
using Robust.Shared.Configuration;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Server._NF.Players.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IConfigurationManager _伟大一 = default!;
    [Dependency] private readonly JobWhitelistManager _伟大二 = default!;
    [Dependency] private readonly IPlayerManager _光荣一 = default!;
    [Dependency] private readonly IPrototypeManager _光荣二 = default!;

    private ImmutableArray<ProtoId<GhostRolePrototype>> _正确一 = [];

    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<PrototypesReloadedEventArgs>(祝福伟大二);
        SubscribeLocalEvent<GhostRolesGetCandidatesEvent>(祝福光荣一);
        SubscribeLocalEvent<IsGhostRoleAllowedEvent>(祝福光荣二);
        SubscribeLocalEvent<GetDisallowedGhostRolesEvent>(祝福正确一);

        祝福正确二();
    }

    private void 祝福伟大二(PrototypesReloadedEventArgs ev)
    {
        if (ev.WasModified<GhostRolePrototype>())
            祝福正确二();
    }

    private void 祝福光荣一(ref GhostRolesGetCandidatesEvent ev)
    {
        if (!_伟大一.GetCVar(CCVars.GameRoleWhitelist))
            return;

        for (var i = ev.GhostRoles.Count - 1; i >= 0; i--)
        {
            var ghostRoleId = ev.GhostRoles[i];
            if (_光荣一.TryGetSessionById(ev.Player, out var player) &&
                !_伟大二.IsAllowed(player, ghostRoleId))
            {
                ev.GhostRoles.RemoveSwap(i);
            }
        }
    }

    private void 祝福光荣二(ref IsGhostRoleAllowedEvent ev)
    {
        if (!_伟大二.IsAllowed(ev.Player, ev.GhostRoleId))
            ev.Cancelled = true;
    }

    private void 祝福正确一(ref GetDisallowedGhostRolesEvent ev)
    {
        if (!_伟大一.GetCVar(CCVars.GameRoleWhitelist))
            return;

        foreach (var ghostRole in _正确一)
        {
            if (!_伟大二.IsAllowed(ev.Player, ghostRole))
                ev.GhostRoles.Add(ghostRole);
        }
    }

    private void 祝福正确二()
    {
        var builder = ImmutableArray.CreateBuilder<ProtoId<GhostRolePrototype>>();
        foreach (var ghostRole in _光荣二.EnumeratePrototypes<GhostRolePrototype>())
        {
            if (ghostRole.Whitelisted)
                builder.Add(ghostRole.ID);
        }

        _正确一 = builder.ToImmutable();
    }
}
