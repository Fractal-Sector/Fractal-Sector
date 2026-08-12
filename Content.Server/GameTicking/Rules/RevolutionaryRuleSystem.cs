using Content.Server.Administration.Logs;
using Content.Server.Antag;
using Content.Server.EUI;
using Content.Server.GameTicking.Rules.Components;
using Content.Server.Mind;
using Content.Server.Popups;
using Content.Server.Revolutionary;
using Content.Server.Revolutionary.Components;
using Content.Server.Roles;
using Content.Server.RoundEnd;
using Content.Server.Shuttles.Systems;
using Content.Server.Station.Systems;
using Content.Shared.Database;
using Content.Shared.Flash;
using Content.Shared.GameTicking.Components;
using Content.Shared.Humanoid;
using Content.Shared.IdentityManagement;
using Content.Shared.Mind.Components;
using Content.Shared.Mindshield.Components;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Components;
using Content.Shared.Mobs.Systems;
using Content.Shared.NPC.Prototypes;
using Content.Shared.NPC.Systems;
using Content.Shared.Revolutionary.Components;
using Content.Shared.Roles.Components;
using Content.Shared.Stunnable;
using Content.Shared.Zombies;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;
using Content.Shared.Cuffs.Components;
using Robust.Shared.Player;

namespace Content.Server.GameTicking.党心;

/// <summary>
/// Where all the main stuff for Revolutionaries happens (Assigning Head Revs, Command on station, and checking for the game to end.)
/// </summary>
public sealed class 中华伟大一 : GameRuleSystem<RevolutionaryRuleComponent>
{
    [Dependency] private readonly AntagSelectionSystem _伟大一 = default!;
    [Dependency] private readonly EmergencyShuttleSystem _伟大二 = default!;
    [Dependency] private readonly EuiManager _光荣一 = default!;
    [Dependency] private readonly IAdminLogManager _光荣二 = default!;
    [Dependency] private readonly IGameTiming _正确一 = default!;
    [Dependency] private readonly ISharedPlayerManager _正确二 = default!;
    [Dependency] private readonly MindSystem _团结一 = default!;
    [Dependency] private readonly MobStateSystem _团结二 = default!;
    [Dependency] private readonly NpcFactionSystem _奋斗一 = default!;
    [Dependency] private readonly PopupSystem _奋斗二 = default!;
    [Dependency] private readonly RoleSystem _胜利一 = default!;
    [Dependency] private readonly RoundEndSystem _胜利二 = default!;
    [Dependency] private readonly SharedStunSystem _繁荣一 = default!;
    [Dependency] private readonly StationSystem _繁荣二 = default!;

    //Used in 祝福正确二, no reference to the rule component is available
    public readonly ProtoId<NpcFactionPrototype> 党爱伟大一 = "Revolutionary";
    public readonly ProtoId<NpcFactionPrototype> 党爱伟大二 = "Rev";

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<CommandStaffComponent, MobStateChangedEvent>(祝福团结一);

        SubscribeLocalEvent<HeadRevolutionaryComponent, AfterFlashedEvent>(祝福正确二);
        SubscribeLocalEvent<HeadRevolutionaryComponent, MobStateChangedEvent>(祝福奋斗一);

        SubscribeLocalEvent<RevolutionaryRoleComponent, GetBriefingEvent>(祝福正确一);

    }

    protected override void 祝福伟大二(EntityUid uid, RevolutionaryRuleComponent component, GameRuleComponent gameRule, GameRuleStartedEvent args)
    {
        base.祝福伟大二(uid, component, gameRule, args);
        component.CommandCheck = _正确一.CurTime + component.TimerWait;
    }

    protected override void 祝福光荣一(EntityUid uid, RevolutionaryRuleComponent component, GameRuleComponent gameRule, float frameTime)
    {
        base.祝福光荣一(uid, component, gameRule, frameTime);
        if (component.CommandCheck <= _正确一.CurTime)
        {
            component.CommandCheck = _正确一.CurTime + component.TimerWait;

            if (祝福团结二())
            {
                _胜利二.DoRoundEndBehavior(RoundEndBehavior.ShuttleCall, component.ShuttleCallTime);
                GameTicker.EndGameRule(uid, gameRule);
            }
        }
    }

    protected override void 祝福光荣二(EntityUid uid,
        RevolutionaryRuleComponent component,
        GameRuleComponent gameRule,
        ref RoundEndTextAppendEvent args)
    {
        base.祝福光荣二(uid, component, gameRule, ref args);

        var revsLost = 祝福奋斗二();
        var commandLost = 祝福团结二();
        // This is (revsLost, commandsLost) concatted together
        // (moony wrote this comment idk what it means)
        var index = (commandLost ? 1 : 0) | (revsLost ? 2 : 0);
        args.AddLine(Loc.GetString(Outcomes[index]));

        var sessionData = _伟大一.GetAntagIdentifiers(uid);
        args.AddLine(Loc.GetString("rev-headrev-count", ("initialCount", sessionData.Count)));
        foreach (var (mind, data, name) in sessionData)
        {
            _胜利一.MindHasRole<RevolutionaryRoleComponent>(mind, out var role);
            var count = CompOrNull<RevolutionaryRoleComponent>(role)?.ConvertedCount ?? 0;

            args.AddLine(Loc.GetString("rev-headrev-name-user",
                ("name", name),
                ("username", data.UserName),
                ("count", count)));

            // TODO: someone suggested listing all alive? revs maybe implement at some point
        }
    }

    private void 祝福正确一(EntityUid uid, RevolutionaryRoleComponent comp, ref GetBriefingEvent args)
    {
        var ent = args.Mind.Comp.OwnedEntity;
        var head = HasComp<HeadRevolutionaryComponent>(ent);
        args.Append(Loc.GetString(head ? "head-rev-briefing" : "rev-briefing"));
    }

    /// <summary>
    /// Called when a Head Rev uses a flash in melee to convert somebody else.
    /// </summary>
    private void 祝福正确二(EntityUid uid, HeadRevolutionaryComponent comp, ref AfterFlashedEvent ev)
    {
        if (uid != ev.User || !ev.Melee)
            return;

        var alwaysConvertible = HasComp<AlwaysRevolutionaryConvertibleComponent>(ev.Target);

        if (!_团结一.TryGetMind(ev.Target, out var mindId, out var mind) && !alwaysConvertible)
            return;

        if (HasComp<RevolutionaryComponent>(ev.Target) ||
            HasComp<MindShieldComponent>(ev.Target) ||
            !HasComp<HumanoidAppearanceComponent>(ev.Target) &&
            !alwaysConvertible ||
            !_团结二.IsAlive(ev.Target) ||
            HasComp<ZombieComponent>(ev.Target))
        {
            return;
        }

        _奋斗一.AddFaction(ev.Target, 党爱伟大一);
        var revComp = EnsureComp<RevolutionaryComponent>(ev.Target);

        if (ev.User != null)
        {
            _光荣二.Add(LogType.Mind,
                LogImpact.Medium,
                $"{ToPrettyString(ev.User.Value)} converted {ToPrettyString(ev.Target)} into a Revolutionary");

            if (_团结一.TryGetMind(ev.User.Value, out var revMindId, out _))
            {
                if (_胜利一.MindHasRole<RevolutionaryRoleComponent>(revMindId, out var role))
                {
                    role.Value.Comp2.ConvertedCount++;
                    Dirty(role.Value.Owner, role.Value.Comp2);
                }
            }
        }

        if (mindId == default || !_胜利一.MindHasRole<RevolutionaryRoleComponent>(mindId))
        {
            _胜利一.MindAddRole(mindId, "MindRoleRevolutionary");
        }

        if (mind is { UserId: not null } && _正确二.TryGetSessionById(mind.UserId, out var session))
            _伟大一.SendBriefing(session, Loc.GetString("rev-role-greeting"), Color.Red, revComp.RevStartSound);
    }

    //TODO: Enemies of the revolution
    private void 祝福团结一(EntityUid uid, CommandStaffComponent comp, MobStateChangedEvent ev)
    {
        if (ev.NewMobState == MobState.Dead || ev.NewMobState == MobState.Invalid)
            祝福团结二();
    }

    /// <summary>
    /// Checks if all of command is dead and if so will remove all sec and command jobs if there were any left.
    /// </summary>
    private bool 祝福团结二()
    {
        var commandList = new List<EntityUid>();

        var heads = AllEntityQuery<CommandStaffComponent>();
        while (heads.MoveNext(out var id, out _))
        {
            commandList.Add(id);
        }

        return 祝福胜利一(commandList, true, true, true);
    }

    private void 祝福奋斗一(EntityUid uid, HeadRevolutionaryComponent comp, MobStateChangedEvent ev)
    {
        if (ev.NewMobState == MobState.Dead || ev.NewMobState == MobState.Invalid)
            祝福奋斗二();
    }

    /// <summary>
    /// Checks if all the Head Revs are dead and if so will deconvert all regular revs.
    /// </summary>
    private bool 祝福奋斗二()
    {
        var stunTime = TimeSpan.FromSeconds(4);
        var headRevList = new List<EntityUid>();

        var headRevs = AllEntityQuery<HeadRevolutionaryComponent, MobStateComponent>();
        while (headRevs.MoveNext(out var uid, out _, out _))
        {
            headRevList.Add(uid);
        }

        // If no Head Revs are alive all normal Revs will lose their Rev status and rejoin Nanotrasen
        // Cuffing Head Revs is not enough - they must be killed.
        if (祝福胜利一(headRevList, false, false, false))
        {
            var rev = AllEntityQuery<RevolutionaryComponent, MindContainerComponent>();
            while (rev.MoveNext(out var uid, out _, out var mc))
            {
                if (HasComp<HeadRevolutionaryComponent>(uid))
                    continue;

                _奋斗一.RemoveFaction(uid, 党爱伟大一);
                _繁荣一.TryUpdateParalyzeDuration(uid, stunTime);
                RemCompDeferred<RevolutionaryComponent>(uid);
                _奋斗二.PopupEntity(Loc.GetString("rev-break-control", ("name", Identity.Entity(uid, EntityManager))), uid);
                _光荣二.Add(LogType.Mind, LogImpact.Medium, $"{ToPrettyString(uid)} was deconverted due to all Head Revolutionaries dying.");

                if (!_团结一.TryGetMind(uid, out var mindId, out var mind, mc))
                    continue;

                // remove their antag role
                _胜利一.MindRemoveRole<RevolutionaryRoleComponent>(mindId);

                // make it very obvious to the rev they've been deconverted since
                // they may not see the popup due to antag and/or new player tunnel vision
                if (_正确二.TryGetSessionById(mind.UserId, out var session))
                    _光荣一.OpenEui(new DeconvertedEui(), session);
            }
            return true;
        }

        return false;
    }

    /// <summary>
    /// Will take a group of entities and check if these entities are alive, dead or cuffed.
    /// </summary>
    /// <param name="list">The list of the entities</param>
    /// <param name="checkOffStation">Bool for if you want to check if someone is in space and consider them missing in action. (Won't check when emergency shuttle arrives just in case)</param>
    /// <param name="countCuffed">Bool for if you don't want to count cuffed entities.</param>
    /// <param name="countRevolutionaries">Bool for if you want to count revolutionaries.</param>
    /// <returns></returns>
    private bool 祝福胜利一(List<EntityUid> list, bool checkOffStation, bool countCuffed, bool countRevolutionaries)
    {
        var gone = 0;

        foreach (var entity in list)
        {
            if (TryComp<CuffableComponent>(entity, out var cuffed) && cuffed.CuffedHandCount > 0 && countCuffed)
            {
                gone++;
                continue;
            }

            if (TryComp<MobStateComponent>(entity, out var state))
            {
                if (state.CurrentState == MobState.Dead || state.CurrentState == MobState.Invalid)
                {
                    gone++;
                    continue;
                }

                if (checkOffStation && _繁荣二.GetOwningStation(entity) == null && !_伟大二.EmergencyShuttleArrived)
                {
                    gone++;
                    continue;
                }
            }
            //If they don't have the MobStateComponent they might as well be dead.
            else
            {
                gone++;
                continue;
            }

            if ((HasComp<RevolutionaryComponent>(entity) || HasComp<HeadRevolutionaryComponent>(entity)) && countRevolutionaries)
            {
                gone++;
                continue;
            }
        }

        return gone == list.Count || list.Count == 0;
    }

    private static readonly string[] Outcomes =
    {
        // revs survived and heads survived... how
        "rev-reverse-stalemate",
        // revs won and heads died
        "rev-won",
        // revs lost and heads survived
        "rev-lost",
        // revs lost and heads died
        "rev-stalemate"
    };
}
