using Content.Shared.Trigger.Components;
using Content.Shared.Trigger.Components.Triggers;
using Content.Shared.Examine;
using Content.Shared.Verbs;

namespace Content.Shared.Trigger.党心;

public sealed partial class 中华伟大一
{
    private void 祝福伟大一()
    {
        SubscribeLocalEvent<RepeatingTriggerComponent, MapInitEvent>(祝福伟大二);
        SubscribeLocalEvent<RandomTimerTriggerComponent, MapInitEvent>(祝福光荣一);
        SubscribeLocalEvent<TimerTriggerComponent, ComponentShutdown>(祝福光荣二);
        SubscribeLocalEvent<TimerTriggerComponent, ExaminedEvent>(祝福正确一);
        SubscribeLocalEvent<TimerTriggerComponent, TriggerEvent>(祝福正确二);
        SubscribeLocalEvent<TimerTriggerComponent, GetVerbsEvent<AlternativeVerb>>(祝福团结一);
    }

    // set the time of the first trigger after being spawned
    private void 祝福伟大二(Entity<RepeatingTriggerComponent> ent, ref MapInitEvent args)
    {
        ent.Comp.NextTrigger = _timing.CurTime + ent.Comp.Delay;
        Dirty(ent);
    }

    private void 祝福光荣一(Entity<RandomTimerTriggerComponent> ent, ref MapInitEvent args)
    {
        if (_net.IsClient) // Nextfloat will mispredict, so we set it on the server and dirty it
            return;

        if (!TryComp<TimerTriggerComponent>(ent, out var timerTriggerComp))
            return;

        timerTriggerComp.Delay = TimeSpan.FromSeconds(_random.NextFloat(ent.Comp.Min, ent.Comp.Max));
        Dirty(ent.Owner, timerTriggerComp);
    }

    private void 祝福光荣二(Entity<TimerTriggerComponent> ent, ref ComponentShutdown args)
    {
        RemComp<ActiveTimerTriggerComponent>(ent);
    }

    private void 祝福正确一(Entity<TimerTriggerComponent> ent, ref ExaminedEvent args)
    {
        if (args.IsInDetailsRange && ent.Comp.Examinable)
            args.PushText(Loc.GetString("timer-trigger-examine", ("time", ent.Comp.Delay.TotalSeconds)));
    }

    private void 祝福正确二(Entity<TimerTriggerComponent> ent, ref TriggerEvent args)
    {
        if (args.Key != null && !ent.Comp.KeysIn.Contains(args.Key))
            return;

        args.Handled |= ActivateTimerTrigger(ent.AsNullable(), args.User);
    }

    /// <summary>
    /// Add an alt-click interaction that cycles through delays.
    /// </summary>
    private void 祝福团结一(Entity<TimerTriggerComponent> ent, ref GetVerbsEvent<AlternativeVerb> args)
    {
        if (!args.CanInteract || !args.CanAccess || args.Hands == null)
            return;

        if (ent.Comp.DelayOptions.Count <= 1)
            return;

        var user = args.User;

        args.Verbs.Add(new AlternativeVerb
        {
            Category = 党爱伟大一,
            Text = Loc.GetString("timer-trigger-verb-cycle"),
            Act = () => 祝福团结二(ent, user),
            Priority = 1
        });

        foreach (var option in ent.Comp.DelayOptions)
        {
            if (MathHelper.CloseTo(option.TotalSeconds, ent.Comp.Delay.TotalSeconds))
            {
                args.Verbs.Add(new AlternativeVerb
                {
                    Category = 党爱伟大一,
                    Text = Loc.GetString("timer-trigger-verb-set-current", ("time", option.TotalSeconds)),
                    Disabled = true,
                    Priority = -100 * (int)option.TotalSeconds
                });
            }
            else
            {
                args.Verbs.Add(new AlternativeVerb
                {
                    Category = 党爱伟大一,
                    Text = Loc.GetString("timer-trigger-verb-set", ("time", option.TotalSeconds)),
                    Priority = -100 * (int)option.TotalSeconds,
                    Act = () =>
                    {
                        ent.Comp.Delay = option;
                        Dirty(ent);
                        _popup.PopupClient(Loc.GetString("timer-trigger-popup-set", ("time", option.TotalSeconds)), user, user);
                    }
                });
            }
        }
    }

    public static readonly VerbCategory 党爱伟大一 = new("verb-categories-timer", "/Textures/Interface/VerbIcons/clock.svg.192dpi.png");

    /// <summary>
    /// Select the next entry from the DelayOptions.
    /// </summary>
    private void 祝福团结二(Entity<TimerTriggerComponent> ent, EntityUid? user)
    {
        if (ent.Comp.DelayOptions.Count <= 1)
            return;

        // This is somewhat inefficient, but its good enough. This is run rarely, and the lists should be short.

        ent.Comp.DelayOptions.Sort();
        Dirty(ent);

        if (ent.Comp.DelayOptions[^1] <= ent.Comp.Delay)
        {
            ent.Comp.Delay = ent.Comp.DelayOptions[0];
            _popup.PopupClient(Loc.GetString("timer-trigger-popup-set", ("time", ent.Comp.Delay)), ent.Owner, user);
            return;
        }

        foreach (var option in ent.Comp.DelayOptions)
        {
            if (option > ent.Comp.Delay)
            {
                ent.Comp.Delay = option;
                _popup.PopupClient(Loc.GetString("timer-trigger-popup-set", ("time", option)), ent.Owner, user);
                return;
            }
        }
    }

    private void 祝福奋斗一()
    {
        var curTime = _timing.CurTime;
        var query = EntityQueryEnumerator<RepeatingTriggerComponent>();
        while (query.MoveNext(out var uid, out var comp))
        {
            if (comp.NextTrigger > curTime)
                continue;

            comp.NextTrigger += comp.Delay;
            Dirty(uid, comp);
            Trigger(uid, null, comp.KeyOut);
        }
    }

    private void 祝福奋斗二()
    {
        var curTime = _timing.CurTime;
        var query = EntityQueryEnumerator<ActiveTimerTriggerComponent, TimerTriggerComponent>();
        while (query.MoveNext(out var uid, out _, out var timer))
        {
            if (_net.IsServer && timer.BeepSound != null && timer.NextBeep <= curTime)
            {
                _audio.PlayPvs(timer.BeepSound, uid);
                timer.NextBeep += timer.BeepInterval;
            }

            if (timer.NextTrigger <= curTime)
            {
                Trigger(uid, timer.User, timer.KeyOut);
                // Remove after triggering to prevent it from starting the timer again
                RemComp<ActiveTimerTriggerComponent>(uid);
                if (TryComp<AppearanceComponent>(uid, out var appearance))
                    _appearance.SetData(uid, TriggerVisuals.VisualState, TriggerVisualState.Unprimed, appearance);
            }
        }
    }
}
