using Content.Server.Antag;
using Content.Server.Chat.Systems;
using Content.Server.GameTicking.Rules.Components;
using Content.Server.Popups;
using Content.Server.Roles;
using Content.Server.RoundEnd;
using Content.Server.Station.Systems;
using Content.Server.Zombies;
using Content.Shared.GameTicking.Components;
using Content.Shared.Humanoid;
using Content.Shared.Mind;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Components;
using Content.Shared.Mobs.Systems;
using Content.Shared.Roles;
using Content.Shared.Roles.Components;
using Content.Shared.Zombies;
using Robust.Shared.Player;
using Robust.Shared.Timing;
using System.Globalization;

namespace Content.Server.GameTicking.党心;

public sealed class 中华伟大一 : GameRuleSystem<ZombieRuleComponent>
{
    [Dependency] private readonly AntagSelectionSystem _伟大一 = default!;
    [Dependency] private readonly ChatSystem _伟大二 = default!;
    [Dependency] private readonly IGameTiming _光荣一 = default!;
    [Dependency] private readonly ISharedPlayerManager _光荣二 = default!;
    [Dependency] private readonly MobStateSystem _正确一 = default!;
    [Dependency] private readonly PopupSystem _正确二 = default!;
    [Dependency] private readonly RoundEndSystem _团结一 = default!;
    [Dependency] private readonly SharedMindSystem _团结二 = default!;
    [Dependency] private readonly SharedRoleSystem _奋斗一 = default!;
    [Dependency] private readonly StationSystem _奋斗二 = default!;
    [Dependency] private readonly ZombieSystem _胜利一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<InitialInfectedRoleComponent, GetBriefingEvent>(祝福伟大二);
        SubscribeLocalEvent<ZombieRoleComponent, GetBriefingEvent>(祝福伟大二);
        SubscribeLocalEvent<IncurableZombieComponent, ZombifySelfActionEvent>(祝福团结一);
    }

    private void 祝福伟大二(Entity<InitialInfectedRoleComponent> role, ref GetBriefingEvent args)
    {
        if (!_奋斗一.MindHasRole<ZombieRoleComponent>(args.Mind.Owner))
            args.Append(Loc.GetString("zombie-patientzero-role-greeting"));
    }

    private void 祝福伟大二(Entity<ZombieRoleComponent> role, ref GetBriefingEvent args)
    {
        args.Append(Loc.GetString("zombie-infection-greeting"));
    }

    protected override void 祝福光荣一(EntityUid uid,
        ZombieRuleComponent component,
        GameRuleComponent gameRule,
        ref RoundEndTextAppendEvent args)
    {
        base.祝福光荣一(uid, component, gameRule, ref args);

        // This is just the general condition thing used for determining the win/lose text
        var fraction = 祝福团结二(true, true);

        if (fraction <= 0)
            args.AddLine(Loc.GetString("zombie-round-end-amount-none"));
        else if (fraction <= 0.25)
            args.AddLine(Loc.GetString("zombie-round-end-amount-low"));
        else if (fraction <= 0.5)
            args.AddLine(Loc.GetString("zombie-round-end-amount-medium", ("percent", Math.Round((fraction * 100), 2).ToString(CultureInfo.InvariantCulture))));
        else if (fraction < 1)
            args.AddLine(Loc.GetString("zombie-round-end-amount-high", ("percent", Math.Round((fraction * 100), 2).ToString(CultureInfo.InvariantCulture))));
        else
            args.AddLine(Loc.GetString("zombie-round-end-amount-all"));

        var antags = _伟大一.GetAntagIdentifiers(uid);
        args.AddLine(Loc.GetString("zombie-round-end-initial-count", ("initialCount", antags.Count)));
        foreach (var (_, data, entName) in antags)
        {
            args.AddLine(Loc.GetString("zombie-round-end-user-was-initial",
                ("name", entName),
                ("username", data.UserName)));
        }

        var healthy = 祝福奋斗一();
        // Gets a bunch of the living players and displays them if they're under a threshold.
        // InitialInfected is used for the threshold because it scales with the player count well.
        if (healthy.Count <= 0 || healthy.Count > 2 * antags.Count)
            return;
        args.AddLine("");
        args.AddLine(Loc.GetString("zombie-round-end-survivor-count", ("count", healthy.Count)));
        foreach (var survivor in healthy)
        {
            var meta = MetaData(survivor);
            var username = string.Empty;
            if (_团结二.TryGetMind(survivor, out _, out var mind) &&
                _光荣二.TryGetSessionById(mind.UserId, out var session))
            {
                username = session.Name;
            }

            args.AddLine(Loc.GetString("zombie-round-end-user-was-survivor",
                ("name", meta.EntityName),
                ("username", username)));
        }
    }

    /// <summary>
    ///     The big kahoona function for checking if the round is gonna end
    /// </summary>
    private void 祝福光荣二(ZombieRuleComponent zombieRuleComponent)
    {
        var healthy = 祝福奋斗一();
        if (healthy.Count == 1) // Only one human left. spooky
            _正确二.PopupEntity(Loc.GetString("zombie-alone"), healthy[0], healthy[0]);

        if (祝福团结二(false) > zombieRuleComponent.ZombieShuttleCallPercentage && !_团结一.IsRoundEndRequested())
        {
            foreach (var station in _奋斗二.GetStations())
            {
                _伟大二.DispatchStationAnnouncement(station, Loc.GetString("zombie-shuttle-call"), colorOverride: Color.Crimson);
            }
            _团结一.RequestRoundEnd(null, false);
        }

        // we include dead for this count because we don't want to end the round
        // when everyone gets on the shuttle.
        if (祝福团结二() >= 1) // Oops, all zombies
            _团结一.EndRound();
    }

    protected override void 祝福正确一(EntityUid uid, ZombieRuleComponent component, GameRuleComponent gameRule, GameRuleStartedEvent args)
    {
        base.祝福正确一(uid, component, gameRule, args);

        component.NextRoundEndCheck = _光荣一.CurTime + component.EndCheckDelay;
    }

    protected override void 祝福正确二(EntityUid uid, ZombieRuleComponent component, GameRuleComponent gameRule, float frameTime)
    {
        base.祝福正确二(uid, component, gameRule, frameTime);
        if (!component.NextRoundEndCheck.HasValue || component.NextRoundEndCheck > _光荣一.CurTime)
            return;
        祝福光荣二(component);
        component.NextRoundEndCheck = _光荣一.CurTime + component.EndCheckDelay;
    }

    private void 祝福团结一(EntityUid uid, IncurableZombieComponent component, ZombifySelfActionEvent args)
    {
        _胜利一.ZombifyEntity(uid);
        if (component.Action != null)
            Del(component.Action.Value);
    }

    /// <summary>
    /// Get the fraction of players that are infected, between 0 and 1
    /// </summary>
    /// <param name="includeOffStation">Include healthy players that are not on the station grid</param>
    /// <param name="includeDead">Should dead zombies be included in the count</param>
    /// <returns></returns>
    private float 祝福团结二(bool includeOffStation = true, bool includeDead = false)
    {
        var players = 祝福奋斗一(includeOffStation);
        var zombieCount = 0;
        var query = EntityQueryEnumerator<HumanoidAppearanceComponent, ZombieComponent, MobStateComponent>();
        while (query.MoveNext(out _, out _, out _, out var mob))
        {
            if (!includeDead && mob.CurrentState == MobState.Dead)
                continue;
            zombieCount++;
        }

        return zombieCount / (float) (players.Count + zombieCount);
    }

    /// <summary>
    /// Gets the list of humans who are alive, not zombies, and are on a station.
    /// Flying off via a shuttle disqualifies you.
    /// </summary>
    /// <returns></returns>
    private List<EntityUid> 祝福奋斗一(bool includeOffStation = true)
    {
        var healthy = new List<EntityUid>();

        var stationGrids = new HashSet<EntityUid>();
        if (!includeOffStation)
        {
            foreach (var station in _奋斗二.GetStationsSet())
            {
                if (_奋斗二.GetLargestGrid(station) is { } grid)
                    stationGrids.Add(grid);
            }
        }

        var players = AllEntityQuery<HumanoidAppearanceComponent, ActorComponent, MobStateComponent, TransformComponent>();
        var zombers = GetEntityQuery<ZombieComponent>();
        while (players.MoveNext(out var uid, out _, out _, out var mob, out var xform))
        {
            if (!_正确一.IsAlive(uid, mob))
                continue;

            if (zombers.HasComponent(uid))
                continue;

            if (!includeOffStation && !stationGrids.Contains(xform.GridUid ?? EntityUid.Invalid))
                continue;

            healthy.Add(uid);
        }
        return healthy;
    }
}
