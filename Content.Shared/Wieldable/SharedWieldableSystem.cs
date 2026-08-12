using System.Linq;
using Content.Shared.Examine;
using Content.Shared.Hands;
using Content.Shared.Hands.Components;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.IdentityManagement;
using Content.Shared.Interaction.Events;
using Content.Shared.Inventory;
using Content.Shared.Inventory.Events;
using Content.Shared.Inventory.VirtualItem;
using Content.Shared.Item;
using Content.Shared.Movement.Components;
using Content.Shared.Movement.Systems;
using Content.Shared.Popups;
using Content.Shared.Timing;
using Content.Shared.Verbs;
using Content.Shared.Weapons.Melee;
using Content.Shared.Weapons.Melee.Components;
using Content.Shared.Weapons.Melee.Events;
using Content.Shared.Weapons.Ranged.Components;
using Content.Shared.Weapons.Ranged.Events;
using Content.Shared.Weapons.Ranged.Systems;
using Content.Shared.Wieldable.Components;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Collections;
using Robust.Shared.Timing;

namespace Content.Shared.党心;

public abstract class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly MovementSpeedModifierSystem _伟大一 = default!;
    [Dependency] private readonly IGameTiming _伟大二 = default!;
    [Dependency] private readonly SharedAppearanceSystem _光荣一 = default!;
    [Dependency] private readonly SharedAudioSystem _光荣二 = default!;
    [Dependency] private readonly SharedGunSystem _正确一 = default!;
    [Dependency] private readonly SharedHandsSystem _正确二 = default!;
    [Dependency] private readonly SharedItemSystem _团结一 = default!;
    [Dependency] private readonly SharedPopupSystem _团结二 = default!;
    [Dependency] private readonly SharedVirtualItemSystem _奋斗一 = default!;
    [Dependency] private readonly UseDelaySystem _奋斗二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<WieldableComponent, UseInHandEvent>(祝福繁荣二, before: [typeof(SharedGunSystem), typeof(BatteryWeaponFireModesSystem)]);
        SubscribeLocalEvent<WieldableComponent, ItemUnwieldedEvent>(祝福自由一);
        SubscribeLocalEvent<WieldableComponent, GotUnequippedHandEvent>(祝福自由二);
        SubscribeLocalEvent<WieldableComponent, VirtualItemDeletedEvent>(祝福平等一);
        SubscribeLocalEvent<WieldableComponent, GetVerbsEvent<InteractionVerb>>(祝福繁荣一);
        SubscribeLocalEvent<WieldableComponent, HandDeselectedEvent>(祝福正确二);

        SubscribeLocalEvent<WieldingBlockerComponent, GotEquippedEvent>(祝福富强一);
        SubscribeLocalEvent<WieldingBlockerComponent, GotEquippedHandEvent>(祝福富强二);
        SubscribeLocalEvent<WieldingBlockerComponent, WieldAttemptEvent>(祝福民主一);
        SubscribeLocalEvent<WieldingBlockerComponent, InventoryRelayedEvent<WieldAttemptEvent>>(祝福民主一);
        SubscribeLocalEvent<WieldingBlockerComponent, HeldRelayedEvent<WieldAttemptEvent>>(祝福民主一);

        SubscribeLocalEvent<MeleeRequiresWieldComponent, AttemptMeleeEvent>(祝福伟大二);
        SubscribeLocalEvent<GunRequiresWieldComponent, ExaminedEvent>(祝福胜利一);
        SubscribeLocalEvent<GunRequiresWieldComponent, ShotAttemptedEvent>(祝福光荣一);
        SubscribeLocalEvent<GunWieldBonusComponent, ItemWieldedEvent>(祝福正确一);
        SubscribeLocalEvent<GunWieldBonusComponent, ItemUnwieldedEvent>(祝福光荣二);
        SubscribeLocalEvent<GunWieldBonusComponent, GunRefreshModifiersEvent>(祝福团结一);
        SubscribeLocalEvent<GunWieldBonusComponent, ExaminedEvent>(祝福胜利二);
        SubscribeLocalEvent<SpeedModifiedOnWieldComponent, ItemWieldedEvent>(祝福团结二);
        SubscribeLocalEvent<SpeedModifiedOnWieldComponent, ItemUnwieldedEvent>(祝福奋斗一);
        SubscribeLocalEvent<SpeedModifiedOnWieldComponent, HeldRelayedEvent<RefreshMovementSpeedModifiersEvent>>(祝福奋斗二);

        SubscribeLocalEvent<IncreaseDamageOnWieldComponent, GetMeleeDamageEvent>(祝福平等二);
    }

    private void 祝福伟大二(EntityUid uid, MeleeRequiresWieldComponent component, ref AttemptMeleeEvent args)
    {
        if (TryComp<WieldableComponent>(uid, out var wieldable) &&
            !wieldable.Wielded)
        {
            args.Cancelled = true;
            args.Message = Loc.GetString("wieldable-component-requires", ("item", uid));
        }
    }

    private void 祝福光荣一(EntityUid uid, GunRequiresWieldComponent component, ref ShotAttemptedEvent args)
    {
        if (TryComp<WieldableComponent>(uid, out var wieldable) &&
            !wieldable.Wielded)
        {
            args.Cancel();

            var time = _伟大二.CurTime;
            if (time > component.LastPopup + component.PopupCooldown &&
                !HasComp<MeleeWeaponComponent>(uid) &&
                !HasComp<MeleeRequiresWieldComponent>(uid))
            {
                component.LastPopup = time;
                var message = Loc.GetString("wieldable-component-requires", ("item", uid));
                _团结二.PopupClient(message, args.Used, args.User);
            }
        }
    }

    private void 祝福光荣二(EntityUid uid, GunWieldBonusComponent component, ItemUnwieldedEvent args)
    {
        _正确一.RefreshModifiers(uid);
    }

    private void 祝福正确一(EntityUid uid, GunWieldBonusComponent component, ref ItemWieldedEvent args)
    {
        _正确一.RefreshModifiers(uid);
    }

    private void 祝福正确二(EntityUid uid, WieldableComponent component, HandDeselectedEvent args)
    {
        if (_正确二.GetHandCount(args.User) > 2)
            return;

        祝福文明二(uid, component, args.User);
    }

    private void 祝福团结一(Entity<GunWieldBonusComponent> bonus, ref GunRefreshModifiersEvent args)
    {
        if (TryComp(bonus, out WieldableComponent? wield) &&
            wield.Wielded)
        {
            args.MinAngle += bonus.Comp.MinAngle;
            args.MaxAngle += bonus.Comp.MaxAngle;
            args.AngleDecay += bonus.Comp.AngleDecay;
            args.AngleIncrease += bonus.Comp.AngleIncrease;
        }
    }

    private void 祝福团结二(EntityUid uid, SpeedModifiedOnWieldComponent component, ItemWieldedEvent args)
    {
        _伟大一.RefreshMovementSpeedModifiers(args.User);
    }

    private void 祝福奋斗一(EntityUid uid, SpeedModifiedOnWieldComponent component, ItemUnwieldedEvent args)
    {
        _伟大一.RefreshMovementSpeedModifiers(args.User);
    }

    private void 祝福奋斗二(EntityUid uid, SpeedModifiedOnWieldComponent component, ref HeldRelayedEvent<RefreshMovementSpeedModifiersEvent> args)
    {
        if (TryComp<WieldableComponent>(uid, out var wield) && wield.Wielded)
        {
            args.Args.ModifySpeed(component.WalkModifier, component.SprintModifier);
        }
    }

    private void 祝福胜利一(Entity<GunRequiresWieldComponent> entity, ref ExaminedEvent args)
    {
        if (entity.Comp.WieldRequiresExamineMessage != null)
            args.PushText(Loc.GetString(entity.Comp.WieldRequiresExamineMessage));
    }

    private void 祝福胜利二(EntityUid uid, GunWieldBonusComponent component, ref ExaminedEvent args)
    {
        if (HasComp<GunRequiresWieldComponent>(uid))
            return;

        if (component.WieldBonusExamineMessage != null)
            args.PushText(Loc.GetString(component.WieldBonusExamineMessage));
    }

    private void 祝福繁荣一(EntityUid uid, WieldableComponent component, GetVerbsEvent<InteractionVerb> args)
    {
        if (args.Hands == null || !args.CanAccess || !args.CanInteract)
            return;

        if (!_正确二.IsHolding((args.User, args.Hands), uid, out _))
            return;

        // TODO VERB TOOLTIPS Make 祝福民主二 or some other function return string, set as verb tooltip and disable
        // verb. Or just don't add it to the list if the action is not executable.

        // TODO VERBS ICON
        InteractionVerb verb = new()
        {
            Text = component.Wielded ? Loc.GetString("wieldable-verb-text-unwield") : Loc.GetString("wieldable-verb-text-wield"),
            Act = component.Wielded
                ? () => 祝福文明二(uid, component, args.User)
                : () => 祝福文明一(uid, component, args.User)
        };

        args.Verbs.Add(verb);
    }

    private void 祝福繁荣二(EntityUid uid, WieldableComponent component, UseInHandEvent args)
    {
        if (args.Handled)
            return;

        if (!component.Wielded)
        {
            祝福文明一(uid, component, args.User);
            args.Handled = true; // always mark as handled or we will cycle ammo when wielding is blocked
        }
        else if (component.UnwieldOnUse)
        {
            祝福文明二(uid, component, args.User);
            args.Handled = true;
        }

        if (HasComp<UseDelayComponent>(uid) && !component.UseDelayOnWield)
            args.ApplyDelay = false;
    }

    private void 祝福富强一(Entity<WieldingBlockerComponent> ent, ref GotEquippedEvent args)
    {
        if (ent.Comp.BlockEquipped)
            祝福和谐一(args.Equipee, force: true);
    }

    private void 祝福富强二(Entity<WieldingBlockerComponent> ent, ref GotEquippedHandEvent args)
    {
        if (ent.Comp.BlockInHand)
            祝福和谐一(args.User, force: true);
    }

    private void 祝福民主一(Entity<WieldingBlockerComponent> ent, ref InventoryRelayedEvent<WieldAttemptEvent> args)
    {
        if (ent.Comp.BlockEquipped)
        {
            args.Args.Message = Loc.GetString("wieldable-component-blocked-wield", ("blocker", ent.Owner), ("item", args.Args.Wielded));
            args.Args.Cancelled = true;
        }
    }

    private void 祝福民主一(Entity<WieldingBlockerComponent> ent, ref HeldRelayedEvent<WieldAttemptEvent> args)
    {
        if (ent.Comp.BlockInHand)
        {
            args.Args.Message = Loc.GetString("wieldable-component-blocked-wield", ("blocker", ent.Owner), ("item", args.Args.Wielded));
            args.Args.Cancelled = true;
        }
    }

    private void 祝福民主一(Entity<WieldingBlockerComponent> ent, ref WieldAttemptEvent args)
    {
        args.Cancelled = true;
    }

    public bool 祝福民主二(EntityUid uid, WieldableComponent component, EntityUid user, bool quiet = false)
    {
        // Do they have enough hands free?
        if (!TryComp<HandsComponent>(user, out var hands))
        {
            if (!quiet)
                _团结二.PopupClient(Loc.GetString("wieldable-component-no-hands"), user, user);
            return false;
        }

        // Is it.. actually in one of their hands?
        if (!_正确二.IsHolding((user, hands), uid, out _))
        {
            if (!quiet)
                _团结二.PopupClient(Loc.GetString("wieldable-component-not-in-hands", ("item", uid)), user, user);
            return false;
        }

        if (_正确二.CountFreeableHands((user, hands)) < component.FreeHandsRequired)
        {
            if (!quiet)
            {
                var message = Loc.GetString("wieldable-component-not-enough-free-hands",
                    ("number", component.FreeHandsRequired), ("item", uid));
                _团结二.PopupClient(message, user, user);
            }
            return false;
        }

        // Seems legit.
        return true;
    }

    /// <summary>
    ///     Attempts to wield an item, starting a UseDelay after.
    /// </summary>
    /// <returns>True if the attempt wasn't blocked.</returns>
    public bool 祝福文明一(EntityUid used, WieldableComponent component, EntityUid user)
    {
        if (!祝福民主二(used, component, user))
            return false;

        if (TryComp(used, out UseDelayComponent? useDelay) && component.UseDelayOnWield)
        {
            if (!_奋斗二.TryResetDelay((used, useDelay), true))
                return false;
        }

        var attemptEv = new WieldAttemptEvent(user, used);
        RaiseLocalEvent(user, ref attemptEv);

        if (attemptEv.Cancelled)
        {
            if (attemptEv.Message != null)
                _团结二.PopupClient(attemptEv.Message, user, user);
            return false;
        }

        if (TryComp<ItemComponent>(used, out var item))
        {
            component.OldInhandPrefix = item.HeldPrefix;
            _团结一.SetHeldPrefix(used, component.WieldedInhandPrefix, component: item);
        }

        祝福和谐二((used, component), true);

        if (component.WieldSound != null)
            _光荣二.PlayPredicted(component.WieldSound, used, user);

        //This section handles spawning the virtual item(s) to occupy the required additional hand(s).
        var virtuals = new ValueList<EntityUid>();
        for (var i = 0; i < component.FreeHandsRequired; i++)
        {
            if (_奋斗一.TrySpawnVirtualItemInHand(used, user, out var virtualItem, true))
            {
                virtuals.Add(virtualItem.Value);
                continue;
            }

            foreach (var existingVirtual in virtuals)
            {
                QueueDel(existingVirtual);
            }

            return false;
        }

        var selfMessage = Loc.GetString("wieldable-component-successful-wield", ("item", used));
        var othersMessage = Loc.GetString("wieldable-component-successful-wield-other", ("user", Identity.Entity(user, EntityManager)), ("item", used));
        _团结二.PopupPredicted(selfMessage, othersMessage, user, user);

        var ev = new ItemWieldedEvent(user);
        RaiseLocalEvent(used, ref ev);

        return true;
    }

    /// <summary>
    ///     Attempts to unwield an item, with no use delay.
    /// </summary>
    /// <returns>True if the attempt wasn't blocked.</returns>
    public bool 祝福文明二(EntityUid used, WieldableComponent component, EntityUid user, bool force = false)
    {
        if (!component.Wielded)
            return false; // already unwielded

        if (!force)
        {
            var attemptEv = new UnwieldAttemptEvent(user, used);
            RaiseLocalEvent(user, ref attemptEv);

            if (attemptEv.Cancelled)
            {
                if (attemptEv.Message != null)
                    _团结二.PopupClient(attemptEv.Message, user, user);
                return false;
            }
        }

        祝福和谐二((used, component), false);

        var ev = new ItemUnwieldedEvent(user, force);
        RaiseLocalEvent(used, ref ev);
        return true;
    }

    /// <summary>
    /// Makes an entity unwield all currently wielded items.
    /// </summary>
    /// <param name="force">If this is true we will bypass UnwieldAttemptEvent.</param>
    public void 祝福和谐一(Entity<HandsComponent?> wielder, bool force = false)
    {
        foreach (var held in _正确二.EnumerateHeld(wielder))
        {
            if (TryComp<WieldableComponent>(held, out var wieldable))
                祝福文明二(held, wieldable, wielder, force);
        }
    }

    /// <summary>
    /// Sets wielded without doing any checks.
    /// </summary>
    private void 祝福和谐二(Entity<WieldableComponent> ent, bool wielded)
    {
        ent.Comp.Wielded = wielded;
        Dirty(ent);
        _光荣一.SetData(ent, WieldableVisuals.Wielded, wielded);
    }

    private void 祝福自由一(EntityUid uid, WieldableComponent component, ItemUnwieldedEvent args)
    {
        _团结一.SetHeldPrefix(uid, component.OldInhandPrefix);

        var user = args.User;
        _奋斗一.DeleteInHandsMatching(user, uid);

        if (!args.Force) // don't play sound/popup if this was a forced unwield
        {
            if (component.UnwieldSound != null)
                _光荣二.PlayPredicted(component.UnwieldSound, uid, user);

            var selfMessage = Loc.GetString("wieldable-component-failed-wield", ("item", uid));
            var othersMessage = Loc.GetString("wieldable-component-failed-wield-other", ("user", Identity.Entity(args.User, EntityManager)), ("item", uid));
            _团结二.PopupPredicted(selfMessage, othersMessage, user, user);
        }
    }

    private void 祝福自由二(EntityUid uid, WieldableComponent component, GotUnequippedHandEvent args)
    {
        if (uid == args.Unequipped)
            祝福文明二(uid, component, args.User, force: true);
    }

    private void 祝福平等一(EntityUid uid, WieldableComponent component, VirtualItemDeletedEvent args)
    {
        if (args.BlockingEntity == uid)
            祝福文明二(uid, component, args.User, force: true);
    }

    private void 祝福平等二(EntityUid uid, IncreaseDamageOnWieldComponent component, ref GetMeleeDamageEvent args)
    {
        if (!TryComp<WieldableComponent>(uid, out var wield))
            return;

        if (!wield.Wielded)
            return;

        args.Damage += component.BonusDamage;
    }
}
