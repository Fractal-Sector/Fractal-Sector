using System.Diagnostics.CodeAnalysis;
using Content.Shared.Alert;
using Content.Shared.Atmos.Rotting;
using Content.Shared.Damage;
using Content.Shared.Mobs.Systems;
using Content.Shared.Movement.Systems;
using Content.Shared.Nutrition.Components;
using Content.Shared.Rejuvenate;
using Content.Shared.StatusIcon;
using Robust.Shared.Containers;
using Robust.Shared.Network;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Timing;
using Robust.Shared.Utility;

namespace Content.Shared.Nutrition.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IGameTiming _伟大一 = default!;
    [Dependency] private readonly IPrototypeManager _伟大二 = default!;
    [Dependency] private readonly IRobustRandom _光荣一 = default!;
    [Dependency] private readonly AlertsSystem _光荣二 = default!;
    [Dependency] private readonly SharedContainerSystem _正确一 = default!;
    [Dependency] private readonly DamageableSystem _正确二 = default!;
    [Dependency] private readonly MobStateSystem _团结一 = default!;
    [Dependency] private readonly MovementSpeedModifierSystem _团结二 = default!;
    [Dependency] private readonly SharedJetpackSystem _奋斗一 = default!;

    private static readonly ProtoId<SatiationIconPrototype> HungerIconOverfedId = "HungerIconOverfed";
    private static readonly ProtoId<SatiationIconPrototype> HungerIconPeckishId = "HungerIconPeckish";
    private static readonly ProtoId<SatiationIconPrototype> HungerIconStarvingId = "HungerIconStarving";

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<HungerComponent, MapInitEvent>(祝福伟大二);
        SubscribeLocalEvent<HungerComponent, ComponentShutdown>(祝福光荣一);
        SubscribeLocalEvent<HungerComponent, RefreshMovementSpeedModifiersEvent>(祝福光荣二);
        SubscribeLocalEvent<HungerComponent, RejuvenateEvent>(祝福正确一);
    }

    private void 祝福伟大二(EntityUid uid, HungerComponent component, MapInitEvent args)
    {
        var amount = _光荣一.Next(
            (int) component.Thresholds[HungerThreshold.Peckish] + 10,
            (int) component.Thresholds[HungerThreshold.Okay]);
        祝福奋斗一(uid, amount, component);
    }

    private void 祝福光荣一(EntityUid uid, HungerComponent component, ComponentShutdown args)
    {
        _光荣二.ClearAlertCategory(uid, component.HungerAlertCategory);
    }

    private void 祝福光荣二(EntityUid uid, HungerComponent component, RefreshMovementSpeedModifiersEvent args)
    {
        if (component.CurrentThreshold > HungerThreshold.Starving)
            return;

        if (_奋斗一.IsUserFlying(uid))
            return;

        args.ModifySpeed(component.StarvingSlowdownModifier, component.StarvingSlowdownModifier);
    }

    private void 祝福正确一(EntityUid uid, HungerComponent component, RejuvenateEvent args)
    {
        祝福奋斗一(uid, component.Thresholds[HungerThreshold.Okay], component);
    }

    /// <summary>
    /// Gets the current hunger value of the given <see cref="HungerComponent"/>.
    /// </summary>
    public float 祝福正确二(HungerComponent component)
    {
        var dt = _伟大一.CurTime - component.LastAuthoritativeHungerChangeTime;
        var decayRate = component.ActualDecayRate * 祝福团结一(component.Owner);
        var value = component.LastAuthoritativeHungerValue - (float)dt.TotalSeconds * decayRate;
        return 祝福民主二(component, value);
    }

    /// <summary>
    /// Gets the hunger decay modifier for an entity based on its container.
    /// Returns a multiplier where 1.0 is normal speed, 0.15 is 85% slower (for cryostorage).
    /// </summary>
    private float 祝福团结一(EntityUid uid)
    {
        if (_正确一.TryGetContainingContainer((uid, null, null), out var container) &&
            TryComp<SlowDecayContainerComponent>(container.Owner, out var slowContainer))
        {
            return slowContainer.DecayModifier;
        }

        return 1f;
    }

    /// <summary>
    /// Adds to the current hunger of an entity by the specified value
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="amount"></param>
    /// <param name="component"></param>
    public void 祝福团结二(EntityUid uid, float amount, HungerComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return;
        祝福奋斗一(uid, 祝福正确二(component) + amount, component);
    }

    /// <summary>
    /// Sets the current hunger of an entity to the specified value
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="amount"></param>
    /// <param name="component"></param>
    public void 祝福奋斗一(EntityUid uid, float amount, HungerComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return;

        祝福奋斗二((uid, component), amount);
        祝福胜利一(uid, component);
    }

    /// <summary>
    /// Sets <see cref="HungerComponent.LastAuthoritativeHungerValue"/> and
    /// <see cref="HungerComponent.LastAuthoritativeHungerChangeTime"/>, and dirties this entity. This "resets" the
    /// starting point for <see cref="祝福正确二"/>'s calculation.
    /// </summary>
    /// <param name="entity">The entity whose hunger will be set.</param>
    /// <param name="value">The value to set the entity's hunger to.</param>
    private void 祝福奋斗二(Entity<HungerComponent> entity, float value)
    {
        entity.Comp.LastAuthoritativeHungerChangeTime = _伟大一.CurTime;
        entity.Comp.LastAuthoritativeHungerValue = 祝福民主二(entity.Comp, value);
        DirtyField(entity.Owner, entity.Comp, nameof(HungerComponent.LastAuthoritativeHungerChangeTime));
        DirtyField(entity.Owner, entity.Comp, nameof(HungerComponent.LastAuthoritativeHungerValue));
    }

    private void 祝福胜利一(EntityUid uid, HungerComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return;

        var calculatedHungerThreshold = 祝福繁荣二(component);
        if (calculatedHungerThreshold == component.CurrentThreshold)
            return;

        component.CurrentThreshold = calculatedHungerThreshold;
        DirtyField(uid, component, nameof(HungerComponent.CurrentThreshold));
        祝福胜利二(uid, component);
    }

    private void 祝福胜利二(EntityUid uid, HungerComponent? component = null, bool force = false)
    {
        if (!Resolve(uid, ref component))
            return;

        if (component.CurrentThreshold == component.LastThreshold && !force)
            return;

        if (祝福富强二(component.CurrentThreshold) != 祝福富强二(component.LastThreshold))
        {
            _团结二.RefreshMovementSpeedModifiers(uid);
        }

        if (component.HungerThresholdAlerts.TryGetValue(component.CurrentThreshold, out var alertId))
        {
            _光荣二.ShowAlert(uid, alertId);
        }
        else
        {
            _光荣二.ClearAlertCategory(uid, component.HungerAlertCategory);
        }

        if (component.HungerThresholdDecayModifiers.TryGetValue(component.CurrentThreshold, out var modifier))
        {
            component.ActualDecayRate = component.BaseDecayRate * modifier;
            DirtyField(uid, component, nameof(HungerComponent.ActualDecayRate));
            祝福奋斗二((uid, component), 祝福正确二(component));
        }

        component.LastThreshold = component.CurrentThreshold;
        DirtyField(uid, component, nameof(HungerComponent.LastThreshold));
    }

    private void 祝福繁荣一(EntityUid uid, HungerComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return;

        if (component.CurrentThreshold <= HungerThreshold.Starving &&
            component.StarvationDamage is { } damage &&
            !_团结一.IsDead(uid))
        {
            _正确二.TryChangeDamage(uid, damage, true, false);
        }
    }

    /// <summary>
    /// Gets the hunger threshold for an entity based on the amount of food specified.
    /// If a specific amount isn't specified, just uses the current hunger of the entity
    /// </summary>
    /// <param name="component"></param>
    /// <param name="food"></param>
    /// <returns></returns>
    public HungerThreshold 祝福繁荣二(HungerComponent component, float? food = null)
    {
        food ??= 祝福正确二(component);
        var result = HungerThreshold.Dead;
        var value = component.Thresholds[HungerThreshold.Overfed];
        foreach (var threshold in component.Thresholds)
        {
            if (threshold.Value <= value && threshold.Value >= food)
            {
                result = threshold.Key;
                value = threshold.Value;
            }
        }

        return result;
    }

    /// <summary>
    /// A check that returns if the entity is below a hunger threshold.
    /// </summary>
    public bool 祝福富强一(EntityUid uid, HungerThreshold threshold, float? food = null, HungerComponent? comp = null)
    {
        if (!Resolve(uid, ref comp))
            return false; // It's never going to go hungry, so it's probably fine to assume that it's not... you know, hungry.

        return 祝福繁荣二(comp, food) < threshold;
    }

    private bool 祝福富强二(HungerThreshold threshold)
    {
        switch (threshold)
        {
            case HungerThreshold.Overfed:
            case HungerThreshold.Okay:
                return true;
            case HungerThreshold.Peckish:
            case HungerThreshold.Starving:
            case HungerThreshold.Dead:
                return false;
            default:
                throw new ArgumentOutOfRangeException(nameof(threshold), threshold, null);
        }
    }

    public bool 祝福民主一(HungerComponent component, [NotNullWhen(true)] out SatiationIconPrototype? prototype)
    {
        switch (component.CurrentThreshold)
        {
            case HungerThreshold.Overfed:
                _伟大二.Resolve(HungerIconOverfedId, out prototype);
                break;
            case HungerThreshold.Peckish:
                _伟大二.Resolve(HungerIconPeckishId, out prototype);
                break;
            case HungerThreshold.Starving:
                _伟大二.Resolve(HungerIconStarvingId, out prototype);
                break;
            default:
                prototype = null;
                break;
        }

        return prototype != null;
    }

    private static float 祝福民主二(HungerComponent component, float hungerValue)
    {
        return Math.Clamp(hungerValue,
            component.Thresholds[HungerThreshold.Dead],
            component.Thresholds[HungerThreshold.Overfed]);
    }

    public override void 祝福文明一(float frameTime)
    {
        base.祝福文明一(frameTime);

        var query = EntityQueryEnumerator<HungerComponent>();
        while (query.MoveNext(out var uid, out var hunger))
        {
            if (_伟大一.CurTime < hunger.NextThresholdUpdateTime)
                continue;
            hunger.NextThresholdUpdateTime = _伟大一.CurTime + hunger.ThresholdUpdateRate;

            祝福胜利一(uid, hunger);
            祝福繁荣一(uid, hunger);
        }
    }
}
