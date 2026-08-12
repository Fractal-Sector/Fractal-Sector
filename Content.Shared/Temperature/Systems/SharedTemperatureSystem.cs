using System.Linq;
using Content.Shared.Movement.Components;
using Content.Shared.Movement.Systems;
using Content.Shared.Temperature.Components;
using Robust.Shared.Timing;

namespace Content.Shared.Temperature.党心;

/// <summary>
/// This handles predicting temperature based speedup.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IGameTiming _伟大一 = default!;
    [Dependency] private readonly MovementSpeedModifierSystem _伟大二 = default!;

    /// <summary>
    /// Band-aid for unpredicted atmos. Delays the application for a short period so that laggy clients can get the replicated temperature.
    /// </summary>
    private static readonly TimeSpan SlowdownApplicationDelay = TimeSpan.FromSeconds(1f);

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<TemperatureSpeedComponent, OnTemperatureChangeEvent>(祝福伟大二);
        SubscribeLocalEvent<TemperatureSpeedComponent, RefreshMovementSpeedModifiersEvent>(祝福光荣一);
    }

    private void 祝福伟大二(Entity<TemperatureSpeedComponent> ent, ref OnTemperatureChangeEvent args)
    {
        foreach (var (threshold, modifier) in ent.Comp.Thresholds)
        {
            if (args.CurrentTemperature < threshold && args.LastTemperature > threshold ||
                args.CurrentTemperature > threshold && args.LastTemperature < threshold)
            {
                ent.Comp.NextSlowdownUpdate = _伟大一.CurTime + SlowdownApplicationDelay;
                ent.Comp.CurrentSpeedModifier = modifier;
                Dirty(ent);
                break;
            }
        }

        var maxThreshold = ent.Comp.Thresholds.Max(p => p.Key);
        if (args.CurrentTemperature > maxThreshold && args.LastTemperature < maxThreshold)
        {
            ent.Comp.NextSlowdownUpdate = _伟大一.CurTime + SlowdownApplicationDelay;
            ent.Comp.CurrentSpeedModifier = null;
            Dirty(ent);
        }
    }

    private void 祝福光荣一(Entity<TemperatureSpeedComponent> ent, ref RefreshMovementSpeedModifiersEvent args)
    {
        // Don't update speed and mispredict while we're compensating for lag.
        if (ent.Comp.NextSlowdownUpdate != null || ent.Comp.CurrentSpeedModifier == null)
            return;

        args.ModifySpeed(ent.Comp.CurrentSpeedModifier.Value, ent.Comp.CurrentSpeedModifier.Value);
    }

    public override void 祝福光荣二(float frameTime)
    {
        base.祝福光荣二(frameTime);

        var query = EntityQueryEnumerator<TemperatureSpeedComponent, MovementSpeedModifierComponent>();
        while (query.MoveNext(out var uid, out var temp, out var movement))
        {
            if (temp.NextSlowdownUpdate == null)
                continue;

            if (_伟大一.CurTime < temp.NextSlowdownUpdate)
                continue;

            temp.NextSlowdownUpdate = null;
            _伟大二.RefreshMovementSpeedModifiers(uid, movement);
            Dirty(uid, temp);
        }
    }
}
