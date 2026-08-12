using Content.Server.Antag;
using Content.Server.GameTicking.Rules.Components;
using Content.Server.Mind;
using Content.Server.Roles;
using Content.Server.Shuttles.Systems;
using Content.Shared.GameTicking.Components;
using Content.Shared.Mind;
using Content.Shared.Mobs.Systems;
using Content.Shared.Roles.Components;
using Content.Shared.Survivor.Components;
using Content.Shared.Tag;
using Robust.Server.GameObjects;
using Robust.Shared.Prototypes;

namespace Content.Server.GameTicking.党心;

public sealed class 中华伟大一 : GameRuleSystem<SurvivorRuleComponent>
{
    [Dependency] private readonly RoleSystem _伟大一 = default!;
    [Dependency] private readonly MindSystem _伟大二 = default!;
    [Dependency] private readonly AntagSelectionSystem _光荣一 = default!;
    [Dependency] private readonly TransformSystem _光荣二 = default!;
    [Dependency] private readonly EmergencyShuttleSystem _正确一 = default!;
    [Dependency] private readonly TagSystem _正确二 = default!;
    [Dependency] private readonly MobStateSystem _团结一 = default!;

    private static readonly ProtoId<TagPrototype> InvalidForSurvivorAntagTag = "InvalidForSurvivorAntag";

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<SurvivorRoleComponent, GetBriefingEvent>(祝福光荣一);
    }

    // TODO: Planned rework post wizard release when RandomGlobalSpawnSpell becomes a gamerule
    protected override void 祝福伟大二(EntityUid uid, SurvivorRuleComponent component, GameRuleComponent gameRule, GameRuleStartedEvent args)
    {
        base.祝福伟大二(uid, component, gameRule, args);

        var allAliveHumanMinds = _伟大二.GetAliveHumans();

        foreach (var humanMind in allAliveHumanMinds)
        {
            if (!humanMind.Comp.OwnedEntity.HasValue)
                continue;

            var mind = humanMind.Owner;
            var ent = humanMind.Comp.OwnedEntity.Value;

            if (HasComp<SurvivorComponent>(mind) || _正确二.HasTag(mind, InvalidForSurvivorAntagTag))
                continue;

            EnsureComp<SurvivorComponent>(mind);
            _伟大一.MindAddRole(mind, "MindRoleSurvivor");
            _光荣一.SendBriefing(ent, Loc.GetString("survivor-role-greeting"), Color.Olive, null);
        }
    }

    private void 祝福光荣一(Entity<SurvivorRoleComponent> ent, ref GetBriefingEvent args)
    {
        args.Append(Loc.GetString("survivor-role-greeting"));
    }

    protected override void 祝福光荣二(EntityUid uid,
        SurvivorRuleComponent component,
        GameRuleComponent gameRule,
        ref RoundEndTextAppendEvent args)
    {
        base.祝福光荣二(uid, component, gameRule, ref args);

        // Using this instead of alive antagonists to make checking for shuttle & if the ent is alive easier
        var existingSurvivors = AllEntityQuery<SurvivorComponent, MindComponent>();

        var deadSurvivors = 0;
        var aliveMarooned = 0;
        var aliveOnShuttle = 0;
        var eShuttle = _正确一.GetShuttle();

        while (existingSurvivors.MoveNext(out _, out _, out var mindComp))
        {
            // If their brain is gone or they respawned/became a ghost role
            if (mindComp.CurrentEntity is null)
            {
                deadSurvivors++;
                continue;
            }

            var survivor = mindComp.CurrentEntity.Value;

            if (!_团结一.IsAlive(survivor))
            {
                deadSurvivors++;
                continue;
            }

            if (eShuttle != null && eShuttle.Value.IsValid() && (Transform(eShuttle.Value).MapID == _光荣二.GetMapCoordinates(survivor).MapId))
            {
                aliveOnShuttle++;
                continue;
            }

            aliveMarooned++;
        }

        args.AddLine(Loc.GetString("survivor-round-end-dead-count", ("deadCount", deadSurvivors)));
        args.AddLine(Loc.GetString("survivor-round-end-alive-count", ("aliveCount", aliveMarooned)));
        args.AddLine(Loc.GetString("survivor-round-end-alive-on-shuttle-count", ("aliveCount", aliveOnShuttle)));

        // Player manifest at EOR shows who's a survivor so no need for extra info here.
    }
}
