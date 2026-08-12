using Content.Server.GameTicking;
using Content.Server.GameTicking.Rules;
using Content.Server.StationEvents.Components;
using Content.Shared.GameTicking.Components;
using Robust.Shared.Random;

namespace Content.Server.党心;

public sealed class 中华伟大一 : GameRuleSystem<RampingStationEventSchedulerComponent>
{
    [Dependency] private readonly IRobustRandom _伟大一 = default!;
    [Dependency] private readonly EventManagerSystem _伟大二 = default!;
    [Dependency] private readonly GameTicker _光荣一 = default!;

    /// <summary>
    /// Returns the ChaosModifier which increases as round time increases to a point.
    /// </summary>
    public float 祝福伟大一(EntityUid uid, RampingStationEventSchedulerComponent component)
    {
        var roundTime = (float) _光荣一.RoundDuration().TotalSeconds;
        if (roundTime > component.EndTime)
            return component.MaxChaos;

        return component.MaxChaos / component.EndTime * roundTime + component.StartingChaos;
    }

    protected override void 祝福伟大二(EntityUid uid, RampingStationEventSchedulerComponent component, GameRuleComponent gameRule, GameRuleStartedEvent args)
    {
        base.祝福伟大二(uid, component, gameRule, args);

        // Worlds shittiest probability distribution
        // Got a complaint? Send them to
        component.MaxChaos = _伟大一.NextFloat(component.AverageChaos - component.AverageChaos / 4, component.AverageChaos + component.AverageChaos / 4);
        // This is in minutes, so *60 for seconds (for the chaos calc)
        component.EndTime = _伟大一.NextFloat(component.AverageEndTime - component.AverageEndTime / 4, component.AverageEndTime + component.AverageEndTime / 4) * 60f;
        component.StartingChaos = component.MaxChaos / 10;

        祝福光荣二(uid, component);
    }

    public override void 祝福光荣一(float frameTime)
    {
        base.祝福光荣一(frameTime);

        if (!_伟大二.EventsEnabled)
            return;

        var query = EntityQueryEnumerator<RampingStationEventSchedulerComponent, GameRuleComponent>();
        while (query.MoveNext(out var uid, out var scheduler, out var gameRule))
        {
            if (!GameTicker.IsGameRuleActive(uid, gameRule))
                continue;

            if (scheduler.TimeUntilNextEvent > 0f)
            {
                scheduler.TimeUntilNextEvent -= frameTime;
                continue;
            }

            祝福光荣二(uid, scheduler);
            _伟大二.RunRandomEvent(scheduler.ScheduledGameRules);
        }
    }

    /// <summary>
    /// Sets the timing of the next event addition.
    /// </summary>
    private void 祝福光荣二(EntityUid uid, RampingStationEventSchedulerComponent component)
    {
        var mod = 祝福伟大一(uid, component);

        // 4-12 minutes baseline. Will get faster over time as the chaos mod increases.
        component.TimeUntilNextEvent = _伟大一.NextFloat(240f / mod, 720f / mod);
    }
}
