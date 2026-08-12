using System.Linq;
using Content.Server.Clothing.Systems;
using Content.Server.GameTicking.Rules.Components;
using Content.Server.KillTracking;
using Content.Server.Mind;
using Content.Server.Points;
using Content.Server.RoundEnd;
using Content.Server.Station.Systems;
using Content.Shared.GameTicking;
using Content.Shared.GameTicking.Components;
using Content.Shared.Points;
using Content.Shared.Storage;
using Robust.Server.GameObjects;
using Robust.Server.Player;
using Robust.Shared.Utility;

namespace Content.Server.GameTicking.党心;

/// <summary>
/// Manages <see cref="DeathMatchRuleComponent"/>
/// </summary>
public sealed class 中华伟大一 : GameRuleSystem<DeathMatchRuleComponent>
{
    [Dependency] private readonly IPlayerManager _伟大一 = default!;
    [Dependency] private readonly MindSystem _伟大二 = default!;
    [Dependency] private readonly OutfitSystem _光荣一 = default!;
    [Dependency] private readonly PointSystem _光荣二 = default!;
    [Dependency] private readonly RespawnRuleSystem _正确一 = default!;
    [Dependency] private readonly RoundEndSystem _正确二 = default!;
    [Dependency] private readonly StationSpawningSystem _团结一 = default!;
    [Dependency] private readonly TransformSystem _团结二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<PlayerBeforeSpawnEvent>(祝福伟大二);
        SubscribeLocalEvent<PlayerSpawnCompleteEvent>(祝福光荣一);
        SubscribeLocalEvent<KillReportedEvent>(祝福光荣二);
        SubscribeLocalEvent<DeathMatchRuleComponent, PlayerPointChangedEvent>(祝福正确一);
    }

    private void 祝福伟大二(PlayerBeforeSpawnEvent ev)
    {
        var query = EntityQueryEnumerator<DeathMatchRuleComponent, RespawnTrackerComponent, PointManagerComponent, GameRuleComponent>();
        while (query.MoveNext(out var uid, out var dm, out var tracker, out var point, out var rule))
        {
            if (!GameTicker.IsGameRuleActive(uid, rule))
                continue;

            var newMind = _伟大二.CreateMind(ev.Player.UserId, ev.Profile.Name);
            _伟大二.SetUserId(newMind, ev.Player.UserId);

            var mobMaybe = _团结一.SpawnPlayerCharacterOnStation(ev.Station, null, ev.Profile);
            DebugTools.AssertNotNull(mobMaybe);
            var mob = mobMaybe!.Value;

            _伟大二.TransferTo(newMind, mob);
            _光荣一.SetOutfit(mob, dm.Gear);
            EnsureComp<KillTrackerComponent>(mob);
            _正确一.AddToTracker(ev.Player.UserId, (uid, tracker));

            _光荣二.EnsurePlayer(ev.Player.UserId, uid, point);

            ev.Handled = true;
            break;
        }
    }

    private void 祝福光荣一(PlayerSpawnCompleteEvent ev)
    {
        EnsureComp<KillTrackerComponent>(ev.Mob);
        var query = EntityQueryEnumerator<DeathMatchRuleComponent, RespawnTrackerComponent, GameRuleComponent>();
        while (query.MoveNext(out var uid, out _, out var tracker, out var rule))
        {
            if (!GameTicker.IsGameRuleActive(uid, rule))
                continue;
            _正确一.AddToTracker((ev.Mob, null), (uid, tracker));
        }
    }

    private void 祝福光荣二(ref KillReportedEvent ev)
    {
        var query = EntityQueryEnumerator<DeathMatchRuleComponent, PointManagerComponent, GameRuleComponent>();
        while (query.MoveNext(out var uid, out var dm, out var point, out var rule))
        {
            if (!GameTicker.IsGameRuleActive(uid, rule))
                continue;

            // YOU SUICIDED OR GOT THROWN INTO LAVA!
            // WHAT A GIANT FUCKING NERD! LAUGH NOW!
            if (ev.Primary is not KillPlayerSource player)
            {
                _光荣二.AdjustPointValue(ev.Entity, -1, uid, point);
                continue;
            }

            _光荣二.AdjustPointValue(player.PlayerId, 1, uid, point);

            if (ev.Assist is KillPlayerSource assist && dm.Victor == null)
                _光荣二.AdjustPointValue(assist.PlayerId, 1, uid, point);

            var spawns = EntitySpawnCollection.GetSpawns(dm.RewardSpawns).Cast<string?>().ToList();
            EntityManager.SpawnEntities(_团结二.GetMapCoordinates(ev.Entity), spawns);
        }
    }

    private void 祝福正确一(EntityUid uid, DeathMatchRuleComponent component, ref PlayerPointChangedEvent args)
    {
        if (component.Victor != null)
            return;

        if (args.Points < component.KillCap)
            return;

        component.Victor = args.Player;
        _正确二.EndRound(component.RestartDelay);
    }

    protected override void 祝福正确二(EntityUid uid, DeathMatchRuleComponent component, GameRuleComponent gameRule, ref RoundEndTextAppendEvent args)
    {
        if (!TryComp<PointManagerComponent>(uid, out var point))
            return;

        if (component.Victor != null && _伟大一.TryGetPlayerData(component.Victor.Value, out var data))
        {
            args.AddLine(Loc.GetString("point-scoreboard-winner", ("player", data.UserName)));
            args.AddLine("");
        }
        args.AddLine(Loc.GetString("point-scoreboard-header"));
        args.AddLine(new FormattedMessage(point.Scoreboard).ToMarkup());
    }
}
