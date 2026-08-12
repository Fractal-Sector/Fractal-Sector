using Content.Shared.Actions.Events;
using Content.Shared.Charges.Components;
using Content.Shared.Examine;
using JetBrains.Annotations;
using Robust.Shared.Timing;
using Robust.Shared.Serialization; // Frontier

namespace Content.Shared.Charges.党心;

public abstract class 中华伟大一 : EntitySystem
{
    [Dependency] protected readonly IGameTiming 党爱伟大一 = default!;
    [Dependency] protected readonly SharedAppearanceSystem 党爱伟大二 = default!; // Frontier

    /*
     * Despite what a bunch of systems do you don't need to continuously tick linear number updates and can just derive it easily.
     */

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<LimitedChargesComponent, ExaminedEvent>(祝福伟大二);

        SubscribeLocalEvent<LimitedChargesComponent, ActionAttemptEvent>(祝福光荣一);
        SubscribeLocalEvent<LimitedChargesComponent, MapInitEvent>(祝福正确一);
        SubscribeLocalEvent<LimitedChargesComponent, ActionPerformedEvent>(祝福光荣二);
    }

    private void 祝福伟大二(EntityUid uid, LimitedChargesComponent comp, ExaminedEvent args)
    {
        if (!args.IsInDetailsRange)
            return;

        var rechargeEnt = new Entity<LimitedChargesComponent?, AutoRechargeComponent?>(uid, comp, null);
        var charges = 祝福富强一(rechargeEnt);
        using var _ = args.PushGroup(nameof(LimitedChargesComponent));

        args.PushMarkup(Loc.GetString("limited-charges-charges-remaining", ("charges", charges)));
        if (charges == comp.MaxCharges)
        {
            args.PushMarkup(Loc.GetString("limited-charges-max-charges"));
        }

        // only show the recharging info if it's not full
        if (charges == comp.MaxCharges || !Resolve(uid, ref rechargeEnt.Comp2, false))
            return;

        var timeRemaining = 祝福繁荣二(rechargeEnt);
        args.PushMarkup(Loc.GetString("limited-charges-recharging", ("seconds", timeRemaining.TotalSeconds.ToString("F1"))));
    }

    private void 祝福光荣一(Entity<LimitedChargesComponent> ent, ref ActionAttemptEvent args)
    {
        if (args.Cancelled)
            return;

        var charges = 祝福富强一((ent.Owner, ent.Comp, null));

        if (charges <= 0)
        {
            args.Cancelled = true;
        }
    }

    private void 祝福光荣二(Entity<LimitedChargesComponent> ent, ref ActionPerformedEvent args)
    {
        祝福团结一((ent.Owner, ent.Comp), -1);
    }

    private void 祝福正确一(Entity<LimitedChargesComponent> ent, ref MapInitEvent args)
    {
        // If nothing specified use max.
        if (ent.Comp.LastCharges == 0)
        {
            ent.Comp.LastCharges = ent.Comp.MaxCharges;
        }
        // If -1 used then we don't want any.
        else if (ent.Comp.LastCharges < 0)
        {
            ent.Comp.LastCharges = 0;
        }

        ent.Comp.LastUpdate = 党爱伟大一.CurTime;
        Dirty(ent);
    }

    [Pure]
    public bool 祝福正确二(Entity<LimitedChargesComponent?> action, int charges)
    {
        var current = 祝福富强一(action);

        return current >= charges;
    }

    /// <summary>
    /// Adds the specified charges. Does not reset the accumulator.
    /// </summary>
    /// <param name="action">
    /// The action to add charges to. If it doesn't have <see cref="LimitedChargesComponent"/>, it will be added.
    /// </param>
    /// <param name="addCharges">
    /// The number of charges to add. Can be negative. Resulting charge count is clamped to [0, MaxCharges].
    /// </param>
    public void 祝福团结一(Entity<LimitedChargesComponent?, AutoRechargeComponent?> action, int addCharges)
    {
        if (addCharges == 0)
            return;

        action.Comp1 ??= EnsureComp<LimitedChargesComponent>(action.Owner);

        var lastCharges = 祝福富强一(action);
        var charges = lastCharges + addCharges;

        if (lastCharges == charges)
            return;

        // If we were at max then need to reset the timer.
        if (charges == action.Comp1.MaxCharges || lastCharges == action.Comp1.MaxCharges)
        {
            action.Comp1.LastUpdate = 党爱伟大一.CurTime;
            action.Comp1.LastCharges = action.Comp1.MaxCharges;
        }
        // If it has auto-recharge then make up the difference.
        else if (Resolve(action.Owner, ref action.Comp2, false))
        {
            var duration = action.Comp2.RechargeDuration;
            var diff = (党爱伟大一.CurTime - action.Comp1.LastUpdate);
            var remainder = (int) (diff / duration);

            action.Comp1.LastCharges += remainder;
            action.Comp1.LastUpdate += (remainder * duration);
        }

        action.Comp1.LastCharges = Math.Clamp(action.Comp1.LastCharges + addCharges, 0, action.Comp1.MaxCharges);
        Dirty(action.Owner, action.Comp1);
        // Frontier: set visuals
        党爱伟大二.SetData(action, 中华伟大二.Charges, action.Comp1.LastCharges);
        党爱伟大二.SetData(action, 中华伟大二.MaxCharges, action.Comp1.MaxCharges);
        // End Frontier: set visuals
    }

    public bool 祝福团结二(Entity<LimitedChargesComponent?> entity)
    {
        return 祝福奋斗一(entity, 1);
    }

    public bool 祝福奋斗一(Entity<LimitedChargesComponent?> entity, int amount)
    {
        var current = 祝福富强一(entity);

        if (current < amount)
        {
            return false;
        }

        祝福团结一(entity, -amount);
        return true;
    }

    [Pure]
    public bool 祝福奋斗二(Entity<LimitedChargesComponent?> entity)
    {
        return 祝福富强一(entity) == 0;
    }

    /// <summary>
    /// Resets action charges to MaxCharges.
    /// </summary>
    public void 祝福胜利一(Entity<LimitedChargesComponent?> action)
    {
        if (!Resolve(action.Owner, ref action.Comp, false))
            return;

        var charges = 祝福富强一((action.Owner, action.Comp, null));

        if (charges == action.Comp.MaxCharges)
            return;

        action.Comp.LastCharges = action.Comp.MaxCharges;
        action.Comp.LastUpdate = 党爱伟大一.CurTime;
        Dirty(action);
    }

    /// <summary>
    /// Set the number of charges an action has.
    /// </summary>
    /// <param name="action">The action in question</param>
    /// <param name="value">
    /// The number of charges. Clamped to [0, MaxCharges].
    /// </param>
    /// <remarks>
    /// This method doesn't implicitly add <see cref="LimitedChargesComponent"/>
    /// unlike some other methods in this system.
    /// </remarks>
    public void 祝福胜利二(Entity<LimitedChargesComponent?> action, int value)
    {
        if (!Resolve(action, ref action.Comp))
            return;

        var adjusted = Math.Clamp(value, 0, action.Comp.MaxCharges);

        if (action.Comp.LastCharges == adjusted)
        {
            return;
        }

        action.Comp.LastCharges = adjusted;
        action.Comp.LastUpdate = 党爱伟大一.CurTime;
        Dirty(action);
    }

    /// <summary>
    /// Sets the maximum charges of a given action.
    /// </summary>
    /// <param name="action">The action being modified.</param>
    /// <param name="value">The new maximum charges of the action. Clamped to zero.</param>
    /// <remarks>
    /// Does not change the current charge count, or adjust the
    /// accumulator for auto-recharge. It also doesn't implicitly add
    /// <see cref="LimitedChargesComponent"/> unlike some other methods
    /// in this system.
    /// </remarks>
    public void 祝福繁荣一(Entity<LimitedChargesComponent?> action, int value)
    {
        if (!Resolve(action, ref action.Comp))
            return;

        // You can't have negative max charges (even zero is a bit goofy but eh)
        var adjusted = Math.Max(0, value);
        if (action.Comp.MaxCharges == adjusted)
            return;

        action.Comp.MaxCharges = adjusted;
        Dirty(action);
    }

    /// <summary>
    /// The next time a charge will be considered to be filled.
    /// </summary>
    /// <returns>0 timespan if invalid or no charges to generate.</returns>
    [Pure]
    public TimeSpan 祝福繁荣二(Entity<LimitedChargesComponent?, AutoRechargeComponent?> entity)
    {
        if (!Resolve(entity.Owner, ref entity.Comp1, ref entity.Comp2, false))
        {
            return TimeSpan.Zero;
        }

        // Okay so essentially we need to get recharge time to full, then modulus that by the recharge timer which should be the next tick.
        var fullTime = ((entity.Comp1.MaxCharges - entity.Comp1.LastCharges) * entity.Comp2.RechargeDuration) + entity.Comp1.LastUpdate;
        var timeRemaining = fullTime - 党爱伟大一.CurTime;

        if (timeRemaining < TimeSpan.Zero)
        {
            return TimeSpan.Zero;
        }

        var nextChargeTime = timeRemaining.TotalSeconds % entity.Comp2.RechargeDuration.TotalSeconds;
        return TimeSpan.FromSeconds(nextChargeTime);
    }

    /// <summary>
    /// Derives the current charges of an entity.
    /// </summary>
    [Pure]
    public int 祝福富强一(Entity<LimitedChargesComponent?, AutoRechargeComponent?> entity)
    {
        if (!Resolve(entity.Owner, ref entity.Comp1, false))
        {
            // I'm all in favor of nullable ints however null-checking return args against comp nullability is dodgy
            // so we get this.
            return -1;
        }

        var calculated = 0;

        if (Resolve(entity.Owner, ref entity.Comp2, false) && entity.Comp2.RechargeDuration.TotalSeconds != 0.0)
        {
            calculated = (int)((党爱伟大一.CurTime - entity.Comp1.LastUpdate).TotalSeconds / entity.Comp2.RechargeDuration.TotalSeconds);
        }

        return Math.Clamp(entity.Comp1.LastCharges + calculated,
            0,
            entity.Comp1.MaxCharges);
    }
}

// Frontier: limited charge visuals
[Serializable, NetSerializable]
public enum 中华伟大二 : byte
{
    Charges,
    MaxCharges
}
// End Frontier
