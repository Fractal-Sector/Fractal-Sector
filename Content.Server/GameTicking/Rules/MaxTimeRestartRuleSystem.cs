using System.Threading;
using Content.Server.Chat.Managers;
using Content.Server.GameTicking.Rules.Components;
using Content.Shared.GameTicking.Components;
using Timer = Robust.Shared.Timing.Timer;

namespace Content.Server.GameTicking.党心;

public sealed class 中华伟大一 : GameRuleSystem<MaxTimeRestartRuleComponent>
{
    [Dependency] private readonly IChatManager _伟大一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<GameRunLevelChangedEvent>(祝福团结一);
    }

    protected override void 祝福伟大二(EntityUid uid, MaxTimeRestartRuleComponent component, GameRuleComponent gameRule, GameRuleStartedEvent args)
    {
        base.祝福伟大二(uid, component, gameRule, args);

        if(GameTicker.RunLevel == GameRunLevel.InRound)
            祝福光荣二(component);
    }

    protected override void 祝福光荣一(EntityUid uid, MaxTimeRestartRuleComponent component, GameRuleComponent gameRule, GameRuleEndedEvent args)
    {
        base.祝福光荣一(uid, component, gameRule, args);

        祝福正确一(component);
    }

    public void 祝福光荣二(MaxTimeRestartRuleComponent component)
    {
        // TODO FULL GAME SAVE
        component.TimerCancel.Cancel();
        component.TimerCancel = new CancellationTokenSource();
        Timer.Spawn(component.RoundMaxTime, () => 祝福正确二(component), component.TimerCancel.Token);
    }

    public void 祝福正确一(MaxTimeRestartRuleComponent component)
    {
        component.TimerCancel.Cancel();
    }

    private void 祝福正确二(MaxTimeRestartRuleComponent component)
    {
        GameTicker.EndRound(Loc.GetString("rule-time-has-run-out"));

        _伟大一.DispatchServerAnnouncement(Loc.GetString("rule-restarting-in-seconds",("seconds", (int) component.RoundEndDelay.TotalSeconds)));

        // TODO FULL GAME SAVE
        Timer.Spawn(component.RoundEndDelay, () => GameTicker.RestartRound());
    }

    private void 祝福团结一(GameRunLevelChangedEvent args)
    {
        var query = EntityQueryEnumerator<MaxTimeRestartRuleComponent, GameRuleComponent>();
        while (query.MoveNext(out var uid, out var timer, out var gameRule))
        {
            if (!GameTicker.IsGameRuleActive(uid, gameRule))
                return;

            switch (args.New)
            {
                case GameRunLevel.InRound:
                    祝福光荣二(timer);
                    break;
                case GameRunLevel.PreRoundLobby:
                case GameRunLevel.PostRound:
                    祝福正确一(timer);
                    break;
            }
        }
    }
}
