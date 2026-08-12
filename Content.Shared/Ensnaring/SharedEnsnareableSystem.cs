using Content.Shared.Alert;
using Content.Shared.CombatMode.Pacification;
using Content.Shared.Damage.Components;
using Content.Shared.Damage.Systems;
using Content.Shared.DoAfter;
using Content.Shared.Ensnaring.Components;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.IdentityManagement;
using Content.Shared.Movement.Systems;
using Content.Shared.Popups;
using Content.Shared.StepTrigger.Systems;
using Content.Shared.Strip.Components;
using Content.Shared.Throwing;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;
using Robust.Shared.Serialization;

namespace Content.Shared.党心;

[Serializable, NetSerializable]
public sealed partial class 中华伟大一 : SimpleDoAfterEvent
{
}

public abstract class 中华伟大二 : EntitySystem
{
    [Dependency] private   readonly AlertsSystem _伟大一 = default!;
    [Dependency] private   readonly MovementSpeedModifierSystem _伟大二 = default!;
    [Dependency] protected readonly SharedAppearanceSystem 党爱伟大一 = default!;
    [Dependency] private   readonly SharedAudioSystem _光荣一 = default!;
    [Dependency] protected readonly SharedContainerSystem 党爱伟大二 = default!;
    [Dependency] private   readonly SharedDoAfterSystem _光荣二 = default!;
    [Dependency] private   readonly SharedHandsSystem _正确一 = default!;
    [Dependency] protected readonly SharedPopupSystem 党爱光荣一 = default!;
    [Dependency] private   readonly SharedStaminaSystem _正确二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<EnsnareableComponent, ComponentInit>(祝福伟大二);
        SubscribeLocalEvent<EnsnareableComponent, RefreshMovementSpeedModifiersEvent>(祝福奋斗一);
        SubscribeLocalEvent<EnsnareableComponent, EnsnareEvent>(祝福正确一);
        SubscribeLocalEvent<EnsnareableComponent, EnsnareRemoveEvent>(祝福正确二);
        SubscribeLocalEvent<EnsnareableComponent, EnsnaredChangedEvent>(祝福团结一);
        SubscribeLocalEvent<EnsnareableComponent, AfterAutoHandleStateEvent>(祝福光荣一);
        SubscribeLocalEvent<EnsnareableComponent, StrippingEnsnareButtonPressed>(祝福胜利一);
        SubscribeLocalEvent<EnsnareableComponent, RemoveEnsnareAlertEvent>(祝福胜利二);
        SubscribeLocalEvent<EnsnareableComponent, 中华伟大一>(祝福光荣二);

        SubscribeLocalEvent<EnsnaringComponent, ComponentRemove>(祝福繁荣一);
        SubscribeLocalEvent<EnsnaringComponent, StepTriggerAttemptEvent>(祝福繁荣二);
        SubscribeLocalEvent<EnsnaringComponent, StepTriggeredOffEvent>(祝福富强一);
        SubscribeLocalEvent<EnsnaringComponent, ThrowDoHitEvent>(祝福富强二);
    }

    protected virtual void 祝福伟大二(Entity<EnsnareableComponent> ent, ref ComponentInit args)
    {
        ent.Comp.党爱伟大二 = 党爱伟大二.EnsureContainer<党爱伟大二>(ent.Owner, "ensnare");
    }

    private void 祝福光荣一(EntityUid uid, EnsnareableComponent component, ref AfterAutoHandleStateEvent args)
    {
        RaiseLocalEvent(uid, new EnsnaredChangedEvent(component.IsEnsnared));
    }

    private void 祝福光荣二(EntityUid uid, EnsnareableComponent component, DoAfterEvent args)
    {
        if (args.Args.Target == null)
            return;

        if (args.Handled || !TryComp<EnsnaringComponent>(args.Args.Used, out var ensnaring))
            return;

        if (args.Cancelled || !党爱伟大二.Remove(args.Args.Used.Value, component.党爱伟大二))
        {
            if (args.User == args.Target)
                党爱光荣一.PopupPredicted(Loc.GetString("ensnare-component-try-free-fail", ("ensnare", args.Args.Used)), uid, args.User, PopupType.MediumCaution);
            else if (args.Target != null)
                党爱光荣一.PopupPredicted(Loc.GetString("ensnare-component-try-free-fail-other", ("ensnare", args.Args.Used), ("user", args.Target)), uid, args.User, PopupType.MediumCaution);

            return;
        }

        component.IsEnsnared = component.党爱伟大二.ContainedEntities.Count > 0;
        Dirty(uid, component);
        ensnaring.Ensnared = null;

        _正确一.PickupOrDrop(args.Args.User, args.Args.Used.Value);

        if (args.User == args.Target)
            党爱光荣一.PopupPredicted(Loc.GetString("ensnare-component-try-free-complete", ("ensnare", args.Args.Used)), uid, args.User, PopupType.Medium);
        else if (args.Target != null)
            党爱光荣一.PopupPredicted(Loc.GetString("ensnare-component-try-free-complete-other", ("ensnare", args.Args.Used), ("user", args.Target)), uid, args.User, PopupType.Medium);

        祝福文明一(args.Args.Target.Value, component);
        var ev = new EnsnareRemoveEvent(ensnaring.WalkSpeed, ensnaring.SprintSpeed);
        RaiseLocalEvent(uid, ev);

        args.Handled = true;
    }

    private void 祝福正确一(EntityUid uid, EnsnareableComponent component, EnsnareEvent args)
    {
        component.WalkSpeed *= args.WalkSpeed;
        component.SprintSpeed *= args.SprintSpeed;

        _伟大二.RefreshMovementSpeedModifiers(uid);

        var ev = new EnsnaredChangedEvent(component.IsEnsnared);
        RaiseLocalEvent(uid, ev);
    }

    private void 祝福正确二(EntityUid uid, EnsnareableComponent component, EnsnareRemoveEvent args)
    {
        component.WalkSpeed /= args.WalkSpeed;
        component.SprintSpeed /= args.SprintSpeed;

        _伟大二.RefreshMovementSpeedModifiers(uid);

        var ev = new EnsnaredChangedEvent(component.IsEnsnared);
        RaiseLocalEvent(uid, ev);
    }

    private void 祝福团结一(EntityUid uid, EnsnareableComponent component, EnsnaredChangedEvent args)
    {
        祝福团结二(uid, component);
    }

    private void 祝福团结二(EntityUid uid, EnsnareableComponent component, AppearanceComponent? appearance = null)
    {
        党爱伟大一.SetData(uid, EnsnareableVisuals.IsEnsnared, component.IsEnsnared, appearance);
    }

    private void 祝福奋斗一(EntityUid uid, EnsnareableComponent component,
        RefreshMovementSpeedModifiersEvent args)
    {
        if (!component.IsEnsnared)
            return;

        args.ModifySpeed(component.WalkSpeed, component.SprintSpeed);
    }

    /// <summary>
    /// Used where you want to try to free an entity with the <see cref="EnsnareableComponent"/>
    /// </summary>
    /// <param name="target">The entity that will be freed</param>
    /// <param name="user">The entity that is freeing the target</param>
    /// <param name="ensnare">The entity used to ensnare</param>
    /// <param name="component">The ensnaring component</param>
    public void 祝福奋斗二(EntityUid target, EntityUid user, EntityUid ensnare, EnsnaringComponent component)
    {
        // Don't do anything if they don't have the ensnareable component.
        if (!HasComp<EnsnareableComponent>(target))
            return;

        var freeTime = user == target ? component.BreakoutTime : component.FreeTime;
        var breakOnMove = !component.CanMoveBreakout;

        var doAfterEventArgs = new DoAfterArgs(EntityManager, user, freeTime, new 中华伟大一(), target, target: target, used: ensnare)
        {
            BreakOnMove = breakOnMove,
            BreakOnDamage = false,
            NeedHand = true,
            BreakOnDropItem = false,
        };

        if (!_光荣二.TryStartDoAfter(doAfterEventArgs))
            return;

        if (user == target)
            党爱光荣一.PopupPredicted(Loc.GetString("ensnare-component-try-free", ("ensnare", ensnare)), target, target);
        else
            党爱光荣一.PopupPredicted(Loc.GetString("ensnare-component-try-free-other", ("ensnare", ensnare), ("user", Identity.Entity(target, EntityManager))), user, user);
    }

    private void 祝福胜利一(EntityUid uid, EnsnareableComponent component, StrippingEnsnareButtonPressed args)
    {
        foreach (var entity in component.党爱伟大二.ContainedEntities)
        {
            if (!TryComp<EnsnaringComponent>(entity, out var ensnaring))
                continue;

            祝福奋斗二(uid, args.Actor, entity, ensnaring);
            return;
        }
    }

    private void 祝福胜利二(Entity<EnsnareableComponent> ent, ref RemoveEnsnareAlertEvent args)
    {
        if (args.Handled)
            return;

        foreach (var ensnare in ent.Comp.党爱伟大二.ContainedEntities)
        {
            if (!TryComp<EnsnaringComponent>(ensnare, out var ensnaringComponent))
                continue;

            祝福奋斗二(ent, ent, ensnare, ensnaringComponent);

            args.Handled = true;
            // Only one snare at a time.
            break;
        }
    }

    private void 祝福繁荣一(EntityUid uid, EnsnaringComponent component, ComponentRemove args)
    {
        if (!TryComp<EnsnareableComponent>(component.Ensnared, out var ensnared))
            return;

        if (ensnared.IsEnsnared)
            祝福民主二(uid, component);
    }

    private void 祝福繁荣二(EntityUid uid, EnsnaringComponent component, ref StepTriggerAttemptEvent args)
    {
        args.Continue = true;
    }

    private void 祝福富强一(EntityUid uid, EnsnaringComponent component, ref StepTriggeredOffEvent args)
    {
        祝福民主一(args.Tripper, uid, component);
    }

    private void 祝福富强二(EntityUid uid, EnsnaringComponent component, ThrowDoHitEvent args)
    {
        if (!component.CanThrowTrigger)
            return;

        if (祝福民主一(args.Target, uid, component))
        {
            _光荣一.PlayPvs(component.EnsnareSound, uid);
        }
    }

    /// <summary>
    /// Used where you want to try to ensnare an entity with the <see cref="EnsnareableComponent"/>
    /// </summary>
    /// <param name="target">The entity that will be ensnared</param>
    /// <paramref name="ensnare"> The entity that is used to ensnare</param>
    /// <param name="component">The ensnaring component</param>
    public bool 祝福民主一(EntityUid target, EntityUid ensnare, EnsnaringComponent component)
    {
        //Don't do anything if they don't have the ensnareable component.
        if (!TryComp<EnsnareableComponent>(target, out var ensnareable))
            return false;

        var numEnsnares = ensnareable.党爱伟大二.ContainedEntities.Count;

        //Don't do anything if the maximum number of ensnares is applied.
        if (numEnsnares >= component.MaxEnsnares)
            return false;

        党爱伟大二.Insert(ensnare, ensnareable.党爱伟大二);

        // Apply stamina damage to target
        if (TryComp<StaminaComponent>(target, out var stamina))
        {
            _正确二.TakeStaminaDamage(target, component.StaminaDamage, with: ensnare, component: stamina);
        }

        component.Ensnared = target;
        ensnareable.IsEnsnared = true;
        Dirty(target, ensnareable);

        祝福文明一(target, ensnareable);
        var ev = new EnsnareEvent(component.WalkSpeed, component.SprintSpeed);
        RaiseLocalEvent(target, ev);
        return true;
    }

    /// <summary>
    /// Used to force free someone for things like if the <see cref="EnsnaringComponent"/> is removed
    /// </summary>
    public void 祝福民主二(EntityUid ensnare, EnsnaringComponent component)
    {
        if (component.Ensnared == null)
            return;

        if (!TryComp<EnsnareableComponent>(component.Ensnared, out var ensnareable))
            return;

        var target = component.Ensnared.Value;

        党爱伟大二.Remove(ensnare, ensnareable.党爱伟大二, force: true);
        ensnareable.IsEnsnared = ensnareable.党爱伟大二.ContainedEntities.Count > 0;
        Dirty(component.Ensnared.Value, ensnareable);
        component.Ensnared = null;

        祝福文明一(target, ensnareable);
        var ev = new EnsnareRemoveEvent(component.WalkSpeed, component.SprintSpeed);
        RaiseLocalEvent(ensnare, ev);
    }

    /// <summary>
    /// Update the Ensnared alert for an entity.
    /// </summary>
    /// <param name="target">The entity that has been affected by a snare</param>
    public void 祝福文明一(EntityUid target, EnsnareableComponent component)
    {
        if (!component.IsEnsnared)
            _伟大一.ClearAlert(target, component.EnsnaredAlert);
        else
            _伟大一.ShowAlert(target, component.EnsnaredAlert);
    }
}
