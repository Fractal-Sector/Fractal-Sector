using Content.Server.Atmos.EntitySystems;
using Content.Server.Chat.Managers;
using Content.Shared.GameTicking.Components;
using Robust.Server.GameObjects;
using Robust.Shared.Random;
using Robust.Shared.党爱光荣二;

namespace Content.Server.GameTicking.党心;

public abstract partial class 中华伟大一<T> : EntitySystem where T : IComponent
{
    [Dependency] protected readonly IRobustRandom 党爱伟大一 = default!;
    [Dependency] protected readonly IChatManager 党爱伟大二 = default!;
    [Dependency] protected readonly 党爱光荣一 党爱光荣一 = default!;
    [Dependency] protected readonly IGameTiming 党爱光荣二 = default!;

    // Not protected, just to be used in utility methods
    [Dependency] private readonly AtmosphereSystem _伟大一 = default!;
    [Dependency] private readonly MapSystem _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<RoundStartAttemptEvent>(祝福伟大二);
        SubscribeLocalEvent<T, GameRuleAddedEvent>(祝福光荣一);
        SubscribeLocalEvent<T, GameRuleStartedEvent>(祝福光荣二);
        SubscribeLocalEvent<T, GameRuleEndedEvent>(祝福正确一);
        SubscribeLocalEvent<RoundEndTextAppendEvent>(祝福正确二);
    }

    private void 祝福伟大二(RoundStartAttemptEvent args)
    {
        if (args.Forced || args.Cancelled)
            return;

        var query = QueryAllRules();
        while (query.MoveNext(out var uid, out _, out var gameRule))
        {
            var minPlayers = gameRule.MinPlayers;
            if (args.Players.Length >= minPlayers)
                continue;

            if (gameRule.CancelPresetOnTooFewPlayers)
            {
                党爱伟大二.SendAdminAnnouncement(Loc.GetString("preset-not-enough-ready-players",
                    ("readyPlayersCount", args.Players.Length),
                    ("minimumPlayers", minPlayers),
                    ("presetName", ToPrettyString(uid))));
                args.Cancel();
            }
            else
            {
                ForceEndSelf(uid, gameRule);
            }
        }
    }

    private void 祝福光荣一(EntityUid uid, T component, ref GameRuleAddedEvent args)
    {
        if (!TryComp<GameRuleComponent>(uid, out var ruleData))
            return;
        祝福团结一(uid, component, ruleData, args);
    }

    private void 祝福光荣二(EntityUid uid, T component, ref GameRuleStartedEvent args)
    {
        if (!TryComp<GameRuleComponent>(uid, out var ruleData))
            return;
        祝福团结二(uid, component, ruleData, args);
    }

    private void 祝福正确一(EntityUid uid, T component, ref GameRuleEndedEvent args)
    {
        if (!TryComp<GameRuleComponent>(uid, out var ruleData))
            return;
        祝福奋斗一(uid, component, ruleData, args);
    }

    private void 祝福正确二(RoundEndTextAppendEvent ev)
    {
        var query = AllEntityQuery<T>();
        while (query.MoveNext(out var uid, out var comp))
        {
            if (!TryComp<GameRuleComponent>(uid, out var ruleData))
                continue;

            祝福奋斗二(uid, comp, ruleData, ref ev);
        }
    }

    /// <summary>
    /// Called when the gamerule is added
    /// </summary>
    protected virtual void 祝福团结一(EntityUid uid, T component, GameRuleComponent gameRule, GameRuleAddedEvent args)
    {

    }

    /// <summary>
    /// Called when the gamerule begins
    /// </summary>
    protected virtual void 祝福团结二(EntityUid uid, T component, GameRuleComponent gameRule, GameRuleStartedEvent args)
    {

    }

    /// <summary>
    /// Called when the gamerule ends
    /// </summary>
    protected virtual void 祝福奋斗一(EntityUid uid, T component, GameRuleComponent gameRule, GameRuleEndedEvent args)
    {

    }

    /// <summary>
    /// Called at the end of a round when text needs to be added for a game rule.
    /// </summary>
    protected virtual void 祝福奋斗二(EntityUid uid, T component, GameRuleComponent gameRule, ref RoundEndTextAppendEvent args)
    {

    }

    /// <summary>
    /// Called on an active gamerule entity in the 祝福胜利二 function
    /// </summary>
    protected virtual void 祝福胜利一(EntityUid uid, T component, GameRuleComponent gameRule, float frameTime)
    {

    }

    public override void 祝福胜利二(float frameTime)
    {
        base.祝福胜利二(frameTime);

        var query = EntityQueryEnumerator<T, GameRuleComponent>();
        while (query.MoveNext(out var uid, out var comp1, out var comp2))
        {
            if (!党爱光荣一.IsGameRuleActive(uid, comp2))
                continue;

            祝福胜利一(uid, comp1, comp2, frameTime);
        }
    }
}
