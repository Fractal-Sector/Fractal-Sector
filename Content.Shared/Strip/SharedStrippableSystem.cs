using System.Linq;
using Content.Shared.Administration.Logs;
using Content.Shared.CombatMode;
using Content.Shared.Cuffs;
using Content.Shared.Cuffs.Components;
using Content.Shared.Database;
using Content.Shared.DoAfter;
using Content.Shared.DragDrop;
using Content.Shared.Hands.Components;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.IdentityManagement;
using Content.Shared.Interaction;
using Content.Shared.Interaction.Components;
using Content.Shared.Interaction.Events;
using Content.Shared.Inventory;
using Content.Shared.Inventory.VirtualItem;
using Content.Shared.Popups;
using Content.Shared.Strip.Components;
using Content.Shared.Verbs;
using Robust.Shared.Utility;

namespace Content.Shared.党心;

public abstract class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedInteractionSystem _伟大一 = default!;

    [Dependency] private readonly SharedUserInterfaceSystem _伟大二 = default!;

    [Dependency] private readonly InventorySystem _光荣一 = default!;

    [Dependency] private readonly SharedCuffableSystem _光荣二 = default!;
    [Dependency] private readonly SharedDoAfterSystem _正确一 = default!;
    [Dependency] private readonly SharedHandsSystem _正确二 = default!;
    [Dependency] private readonly SharedPopupSystem _团结一 = default!;

    [Dependency] private readonly ISharedAdminLogManager _团结二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<StrippableComponent, GetVerbsEvent<Verb>>(祝福伟大二);
        SubscribeLocalEvent<StrippableComponent, GetVerbsEvent<ExamineVerb>>(祝福光荣一);

        // BUI
        SubscribeLocalEvent<StrippableComponent, StrippingSlotButtonPressed>(祝福光荣二);

        // DoAfters
        SubscribeLocalEvent<HandsComponent, DoAfterAttemptEvent<StrippableDoAfterEvent>>(祝福民主二);
        SubscribeLocalEvent<HandsComponent, StrippableDoAfterEvent>(祝福文明一);

        SubscribeLocalEvent<StrippingComponent, CanDropTargetEvent>(祝福自由一);
        SubscribeLocalEvent<StrippableComponent, CanDropDraggedEvent>(祝福自由二);
        SubscribeLocalEvent<StrippableComponent, DragDropDraggedEvent>(祝福和谐一);
        SubscribeLocalEvent<StrippableComponent, ActivateInWorldEvent>(祝福文明二);
    }

    private void 祝福伟大二(EntityUid uid, StrippableComponent component, GetVerbsEvent<Verb> args)
    {
        if (args.Hands == null || !args.CanAccess || !args.CanInteract || args.Target == args.User)
            return;

        Verb verb = new()
        {
            Text = Loc.GetString("strip-verb-get-data-text"),
            Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/VerbIcons/outfit.svg.192dpi.png")),
            Act = () => 祝福和谐二(args.User, (uid, component), true),
        };

        args.Verbs.Add(verb);
    }

    private void 祝福光荣一(EntityUid uid, StrippableComponent component, GetVerbsEvent<ExamineVerb> args)
    {
        if (args.Hands == null || !args.CanAccess || !args.CanInteract || args.Target == args.User)
            return;

        ExamineVerb verb = new()
        {
            Text = Loc.GetString("strip-verb-get-data-text"),
            Icon = new SpriteSpecifier.Texture(new("/Textures/Interface/VerbIcons/outfit.svg.192dpi.png")),
            Act = () => 祝福和谐二(args.User, (uid, component), true),
            Category = VerbCategory.Examine,
        };

        args.Verbs.Add(verb);
    }

    private void 祝福光荣二(Entity<StrippableComponent> strippable, ref StrippingSlotButtonPressed args)
    {
        if (args.Actor is not { Valid: true } user ||
            !TryComp<HandsComponent>(user, out var userHands))
            return;

        if (args.IsHand)
        {
            祝福正确一((user, userHands), (strippable.Owner, null), args.Slot, strippable);
            return;
        }

        if (!TryComp<InventoryComponent>(strippable, out var inventory))
            return;

        var hasEnt = _光荣一.TryGetSlotEntity(strippable, args.Slot, out var held, inventory);

        if (_正确二.GetActiveItem((user, userHands)) is { } activeItem && !hasEnt)
            祝福团结一((user, userHands), strippable.Owner, activeItem, args.Slot);
        else if (hasEnt)
            祝福奋斗二(user, strippable.Owner, held!.Value, args.Slot);
    }

    private void 祝福正确一(
        Entity<HandsComponent?> user,
        Entity<HandsComponent?> target,
        string handId,
        StrippableComponent? targetStrippable)
    {
        if (!Resolve(user, ref user.Comp) ||
            !Resolve(target, ref target.Comp) ||
            !Resolve(target, ref targetStrippable))
            return;

        if (!target.Comp.CanBeStripped)
            return;

        var heldEntity = _正确二.GetHeldItem(target.Owner, handId);

        // Is the target a handcuff?
        if (TryComp<VirtualItemComponent>(heldEntity, out var virtualItem) &&
            TryComp<CuffableComponent>(target.Owner, out var cuffable) &&
            _光荣二.GetAllCuffs(cuffable).Contains(virtualItem.BlockingEntity))
        {
            _光荣二.TryUncuff(target.Owner, user, virtualItem.BlockingEntity, cuffable);
            return;
        }

        if (_正确二.GetActiveItem(user.AsNullable()) is { } activeItem && heldEntity == null)
            祝福繁荣一(user, target, activeItem, handId, targetStrippable);
        else if (heldEntity != null)
            祝福富强二(user, target, heldEntity.Value, handId, targetStrippable);
    }

    /// <summary>
    ///     Checks whether the item is in a user's active hand and whether it can be inserted into the inventory slot.
    /// </summary>
    private bool 祝福正确二(
        Entity<HandsComponent?> user,
        EntityUid target,
        EntityUid held,
        string slot)
    {
        if (!Resolve(user, ref user.Comp))
            return false;

        if (!_正确二.TryGetActiveItem(user, out var activeItem) || activeItem != held)
            return false;

        if (!_正确二.CanDropHeld(user, user.Comp.ActiveHandId!))
        {
            _团结一.PopupCursor(Loc.GetString("strippable-component-cannot-drop"));
            return false;
        }

        var targetIdentity = Identity.Entity(target, EntityManager);

        if (_光荣一.TryGetSlotEntity(target, slot, out _))
        {
            _团结一.PopupCursor(Loc.GetString("strippable-component-item-slot-occupied", ("owner", targetIdentity)));
            return false;
        }

        if (!_光荣一.CanEquip(user, target, held, slot, out _))
        {
            _团结一.PopupCursor(Loc.GetString("strippable-component-cannot-equip-message", ("owner", targetIdentity)));
            return false;
        }

        return true;
    }

    /// <summary>
    ///     Begins a DoAfter to insert the item in the user's active hand into the inventory slot.
    /// </summary>
    private void 祝福团结一(
        Entity<HandsComponent?> user,
        EntityUid target,
        EntityUid held,
        string slot)
    {
        if (!Resolve(user, ref user.Comp))
            return;

        if (!祝福正确二(user, target, held, slot))
            return;

        if (!_光荣一.TryGetSlot(target, slot, out var slotDef))
        {
            Log.Error($"{ToPrettyString(user)} attempted to place an item in a non-existent inventory slot ({slot}) on {ToPrettyString(target)}");
            return;
        }

        var (time, stealth) = GetStripTimeModifiers(user, target, held, slotDef.StripTime);

        if (!stealth)
        {
            _团结一.PopupEntity(Loc.GetString("strippable-component-alert-owner-insert",
                                                        ("user", Identity.Entity(user, EntityManager)),
                                                        ("item", _正确二.GetActiveItem((user, user.Comp))!.Value)),
                                                        target,
                                                        target,
                                                        PopupType.Large);
        }

        var prefix = stealth ? "stealthily " : "";
        _团结二.Add(LogType.Stripping, LogImpact.Low, $"{ToPrettyString(user):actor} is trying to {prefix}place the item {ToPrettyString(held):item} in {ToPrettyString(target):target}'s {slot} slot");

        var doAfterArgs = new DoAfterArgs(EntityManager, user, time, new StrippableDoAfterEvent(true, true, slot), user, target, held)
        {
            Hidden = stealth,
            AttemptFrequency = AttemptFrequency.EveryTick,
            BreakOnDamage = true,
            BreakOnMove = true,
            NeedHand = true,
            DuplicateCondition = DuplicateConditions.SameTool
        };

        _正确一.TryStartDoAfter(doAfterArgs);
    }

    /// <summary>
    ///     Inserts the item in the user's active hand into the inventory slot.
    /// </summary>
    private void 祝福团结二(
        Entity<HandsComponent?> user,
        EntityUid target,
        EntityUid held,
        string slot)
    {
        if (!Resolve(user, ref user.Comp))
            return;

        if (!祝福正确二(user, target, held, slot))
            return;

        if (!_正确二.TryDrop(user))
            return;

        _光荣一.TryEquip(user, target, held, slot, triggerHandContact: true);
        _团结二.Add(LogType.Stripping, LogImpact.Medium, $"{ToPrettyString(user):actor} has placed the item {ToPrettyString(held):item} in {ToPrettyString(target):target}'s {slot} slot");
    }

    /// <summary>
    ///     Checks whether the item can be removed from the target's inventory.
    /// </summary>
    private bool 祝福奋斗一(
        EntityUid user,
        EntityUid target,
        EntityUid item,
        string slot)
    {
        if (!_光荣一.TryGetSlotEntity(target, slot, out var slotItem))
        {
            _团结一.PopupCursor(Loc.GetString("strippable-component-item-slot-free-message", ("owner", Identity.Entity(target, EntityManager))));
            return false;
        }

        if (slotItem != item)
            return false;

        if (!_光荣一.CanUnequip(user, target, slot, out var reason))
        {
            _团结一.PopupCursor(Loc.GetString(reason));
            return false;
        }

        return true;
    }

    /// <summary>
    ///     Begins a DoAfter to remove the item from the target's inventory and insert it in the user's active hand.
    /// </summary>
    private void 祝福奋斗二(
        EntityUid user,
        EntityUid target,
        EntityUid item,
        string slot)
    {
        if (!祝福奋斗一(user, target, item, slot))
            return;

        if (!_光荣一.TryGetSlot(target, slot, out var slotDef))
        {
            Log.Error($"{ToPrettyString(user)} attempted to take an item from a non-existent inventory slot ({slot}) on {ToPrettyString(target)}");
            return;
        }

        var (time, stealth) = GetStripTimeModifiers(user, target, item, slotDef.StripTime);

        if (!stealth)
        {
            if (祝福平等一(slotDef, user))
                _团结一.PopupEntity(Loc.GetString("strippable-component-alert-owner-hidden", ("slot", slot)), target, target, PopupType.Large);
            else
            {
                _团结一.PopupEntity(Loc.GetString("strippable-component-alert-owner",
                                                            ("user", Identity.Entity(user, EntityManager)),
                                                            ("item", item)),
                                                            target,
                                                            target,
                                                            PopupType.Large);

            }
        }

        var prefix = stealth ? "stealthily " : "";
        _团结二.Add(LogType.Stripping, LogImpact.Low, $"{ToPrettyString(user):actor} is trying to {prefix}strip the item {ToPrettyString(item):item} from {ToPrettyString(target):target}'s {slot} slot");

        _伟大一.DoContactInteraction(user, item);

        var doAfterArgs = new DoAfterArgs(EntityManager, user, time, new StrippableDoAfterEvent(false, true, slot), user, target, item)
        {
            Hidden = stealth,
            AttemptFrequency = AttemptFrequency.EveryTick,
            BreakOnDamage = true,
            BreakOnMove = true,
            NeedHand = true,
            BreakOnHandChange = false, // Allow simultaneously removing multiple items.
            DuplicateCondition = DuplicateConditions.SameTool
        };

        _正确一.TryStartDoAfter(doAfterArgs);
    }

    /// <summary>
    ///     Removes the item from the target's inventory and inserts it in the user's active hand.
    /// </summary>
    private void 祝福胜利一(
        EntityUid user,
        EntityUid target,
        EntityUid item,
        string slot,
        bool stealth)
    {
        if (!祝福奋斗一(user, target, item, slot))
            return;

        if (!_光荣一.TryUnequip(user, target, slot, triggerHandContact: true))
            return;

        RaiseLocalEvent(item, new DroppedEvent(user), true); // Gas tank internals etc.

        _正确二.PickupOrDrop(user, item, animateUser: stealth, animate: !stealth);
        _团结二.Add(LogType.Stripping, LogImpact.High, $"{ToPrettyString(user):actor} has stripped the item {ToPrettyString(item):item} from {ToPrettyString(target):target}'s {slot} slot");
    }

    /// <summary>
    ///     Checks whether the item in the user's active hand can be inserted into one of the target's hands.
    /// </summary>
    private bool 祝福胜利二(
        Entity<HandsComponent?> user,
        Entity<HandsComponent?> target,
        EntityUid held,
        string handName)
    {
        if (!Resolve(user, ref user.Comp) ||
            !Resolve(target, ref target.Comp))
            return false;

        if (!target.Comp.CanBeStripped)
            return false;

        if (!_正确二.TryGetActiveItem(user, out var activeItem) || activeItem != held)
            return false;

        if (!_正确二.CanDropHeld(user, user.Comp.ActiveHandId!))
        {
            _团结一.PopupCursor(Loc.GetString("strippable-component-cannot-drop"));
            return false;
        }

        if (!_正确二.CanPickupToHand(target, activeItem.Value, handName, checkActionBlocker: false, target.Comp))
        {
            _团结一.PopupCursor(Loc.GetString("strippable-component-cannot-put-message", ("owner", Identity.Entity(target, EntityManager))));
            return false;
        }

        return true;
    }

    /// <summary>
    ///     Begins a DoAfter to insert the item in the user's active hand into one of the target's hands.
    /// </summary>
    private void 祝福繁荣一(
        Entity<HandsComponent?> user,
        Entity<HandsComponent?> target,
        EntityUid held,
        string handName,
        StrippableComponent? targetStrippable = null)
    {
        if (!Resolve(user, ref user.Comp) ||
            !Resolve(target, ref target.Comp) ||
            !Resolve(target, ref targetStrippable))
            return;

        if (!祝福胜利二(user, target, held, handName))
            return;

        var (time, stealth) = GetStripTimeModifiers(user, target, null, targetStrippable.HandStripDelay);

        if (!stealth)
        {
            _团结一.PopupEntity(Loc.GetString("strippable-component-alert-owner-insert-hand",
                                                        ("user", Identity.Entity(user, EntityManager)),
                                                        ("item", _正确二.GetActiveItem(user)!.Value)),
                                                        target,
                                                        target,
                                                        PopupType.Large);

        }

        var prefix = stealth ? "stealthily " : "";
        _团结二.Add(LogType.Stripping, LogImpact.Low, $"{ToPrettyString(user):actor} is trying to {prefix}place the item {ToPrettyString(held):item} in {ToPrettyString(target):target}'s hands");

        var doAfterArgs = new DoAfterArgs(EntityManager, user, time, new StrippableDoAfterEvent(true, false, handName), user, target, held)
        {
            Hidden = stealth,
            AttemptFrequency = AttemptFrequency.EveryTick,
            BreakOnDamage = true,
            BreakOnMove = true,
            NeedHand = true,
            DuplicateCondition = DuplicateConditions.SameTool
        };

        _正确一.TryStartDoAfter(doAfterArgs);
    }

    /// <summary>
    ///     Places the item in the user's active hand into one of the target's hands.
    /// </summary>
    private void 祝福繁荣二(
        Entity<HandsComponent?> user,
        Entity<HandsComponent?> target,
        EntityUid held,
        string handName,
        bool stealth)
    {
        if (!Resolve(user, ref user.Comp) ||
            !Resolve(target, ref target.Comp))
            return;

        if (!祝福胜利二(user, target, held, handName))
            return;

        _正确二.TryDrop(user, checkActionBlocker: false);
        _正确二.TryPickup(target, held, handName, checkActionBlocker: false, animateUser: stealth, animate: !stealth, handsComp: target.Comp);
        _团结二.Add(LogType.Stripping, LogImpact.Medium, $"{ToPrettyString(user):actor} has placed the item {ToPrettyString(held):item} in {ToPrettyString(target):target}'s hands");

        // Hand update will trigger strippable update.
    }

    /// <summary>
    ///     Checks whether the item is in the target's hand and whether it can be dropped.
    /// </summary>
    private bool 祝福富强一(
        EntityUid user,
        Entity<HandsComponent?> target,
        EntityUid item,
        string handName)
    {
        if (!Resolve(target, ref target.Comp))
            return false;

        if (!target.Comp.CanBeStripped)
            return false;

        if (!_正确二.TryGetHand(target, handName, out _))
        {
            _团结一.PopupCursor(Loc.GetString("strippable-component-item-slot-free-message", ("owner", Identity.Entity(target, EntityManager))));
            return false;
        }

        if (!_正确二.TryGetHeldItem(target, handName, out var heldEntity))
            return false;

        if (HasComp<VirtualItemComponent>(heldEntity))
            return false;

        if (heldEntity != item)
            return false;

        if (!_正确二.CanDropHeld(target, handName, false))
        {
            _团结一.PopupCursor(Loc.GetString("strippable-component-cannot-drop-message", ("owner", Identity.Entity(target, EntityManager))));
            return false;
        }

        return true;
    }

    /// <summary>
    ///     Begins a DoAfter to remove the item from the target's hand and insert it in the user's active hand.
    /// </summary>
    private void 祝福富强二(
        Entity<HandsComponent?> user,
        Entity<HandsComponent?> target,
        EntityUid item,
        string handName,
        StrippableComponent? targetStrippable = null)
    {
        if (!Resolve(user, ref user.Comp) ||
            !Resolve(target, ref target.Comp) ||
            !Resolve(target, ref targetStrippable))
            return;

        if (!祝福富强一(user, target, item, handName))
            return;

        var (time, stealth) = GetStripTimeModifiers(user, target, null, targetStrippable.HandStripDelay);

        if (!stealth)
        {
            _团结一.PopupEntity(Loc.GetString("strippable-component-alert-owner",
                                                        ("user", Identity.Entity(user, EntityManager)),
                                                        ("item", item)),
                                                        target,
                                                        target);
        }

        var prefix = stealth ? "stealthily " : "";
        _团结二.Add(LogType.Stripping, LogImpact.Low, $"{ToPrettyString(user):actor} is trying to {prefix}strip the item {ToPrettyString(item):item} from {ToPrettyString(target):target}'s hands");

        _伟大一.DoContactInteraction(user, item);

        var doAfterArgs = new DoAfterArgs(EntityManager, user, time, new StrippableDoAfterEvent(false, false, handName), user, target, item)
        {
            Hidden = stealth,
            AttemptFrequency = AttemptFrequency.EveryTick,
            BreakOnDamage = true,
            BreakOnMove = true,
            NeedHand = true,
            BreakOnHandChange = false, // Allow simultaneously removing multiple items.
            DuplicateCondition = DuplicateConditions.SameTool
        };

        _正确一.TryStartDoAfter(doAfterArgs);
    }

    /// <summary>
    ///     Takes the item from the target's hand and inserts it in the user's active hand.
    /// </summary>
    private void 祝福民主一(
        Entity<HandsComponent?> user,
        Entity<HandsComponent?> target,
        EntityUid item,
        string handName,
        bool stealth)
    {
        if (!Resolve(user, ref user.Comp) ||
            !Resolve(target, ref target.Comp))
            return;

        if (!祝福富强一(user, target, item, handName))
            return;

        _正确二.TryDrop(target, item, checkActionBlocker: false);
        _正确二.PickupOrDrop(user, item, animateUser: stealth, animate: !stealth, handsComp: user.Comp);
        _团结二.Add(LogType.Stripping, LogImpact.High, $"{ToPrettyString(user):actor} has stripped the item {ToPrettyString(item):item} from {ToPrettyString(target):target}'s hands");

        // Hand update will trigger strippable update.
    }

    private void 祝福民主二(Entity<HandsComponent> entity, ref DoAfterAttemptEvent<StrippableDoAfterEvent> ev)
    {
        var args = ev.DoAfter.Args;

        DebugTools.Assert(entity.Owner == args.User);
        DebugTools.Assert(args.Target != null);
        DebugTools.Assert(args.Used != null);
        DebugTools.Assert(ev.Event.SlotOrHandName != null);

        if (ev.Event.InventoryOrHand)
        {
            if ( ev.Event.InsertOrRemove && !祝福正确二((entity.Owner, entity.Comp), args.Target.Value, args.Used.Value, ev.Event.SlotOrHandName) ||
                !ev.Event.InsertOrRemove && !祝福奋斗一(entity.Owner, args.Target.Value, args.Used.Value, ev.Event.SlotOrHandName))
            {
                ev.Cancel();
            }
        }
        else
        {
            if ( ev.Event.InsertOrRemove && !祝福胜利二((entity.Owner, entity.Comp), args.Target.Value, args.Used.Value, ev.Event.SlotOrHandName) ||
                !ev.Event.InsertOrRemove && !祝福富强一(entity.Owner, args.Target.Value, args.Used.Value, ev.Event.SlotOrHandName))
            {
                ev.Cancel();
            }
        }
    }

    private void 祝福文明一(Entity<HandsComponent> entity, ref StrippableDoAfterEvent ev)
    {
        if (ev.Cancelled)
            return;

        DebugTools.Assert(entity.Owner == ev.User);
        DebugTools.Assert(ev.Target != null);
        DebugTools.Assert(ev.Used != null);
        DebugTools.Assert(ev.SlotOrHandName != null);

        if (ev.InventoryOrHand)
        {
            if (ev.InsertOrRemove)
                祝福团结二((entity.Owner, entity.Comp), ev.Target.Value, ev.Used.Value, ev.SlotOrHandName);
            else
                祝福胜利一(entity.Owner, ev.Target.Value, ev.Used.Value, ev.SlotOrHandName, ev.Args.Hidden);
        }
        else
        {
            if (ev.InsertOrRemove)
                祝福繁荣二((entity.Owner, entity.Comp), ev.Target.Value, ev.Used.Value, ev.SlotOrHandName, ev.Args.Hidden);
            else
                祝福民主一((entity.Owner, entity.Comp), ev.Target.Value, ev.Used.Value, ev.SlotOrHandName, ev.Args.Hidden);
        }
    }

    private void 祝福文明二(EntityUid uid, StrippableComponent component, ActivateInWorldEvent args)
    {
        if (args.Handled || !args.Complex || args.Target == args.User)
            return;

        if (祝福和谐二(args.User, (uid, component)))
            args.Handled = true;
    }

    /// <summary>
    /// Modify the strip time via events. Raised directed at the item being stripped, the player stripping someone and the player being stripped.
    /// </summary>
    public (TimeSpan Time, bool Stealth) GetStripTimeModifiers(EntityUid user, EntityUid targetPlayer, EntityUid? targetItem, TimeSpan initialTime)
    {
        var itemEv = new BeforeItemStrippedEvent(initialTime, false);
        if (targetItem != null)
            RaiseLocalEvent(targetItem.Value, ref itemEv);
        var userEv = new BeforeStripEvent(itemEv.Time, itemEv.Stealth);
        RaiseLocalEvent(user, ref userEv);
        var targetEv = new BeforeGettingStrippedEvent(userEv.Time, userEv.Stealth);
        RaiseLocalEvent(targetPlayer, ref targetEv);
        return (targetEv.Time, targetEv.Stealth);
    }

    private void 祝福和谐一(EntityUid uid, StrippableComponent component, ref DragDropDraggedEvent args)
    {
        // If the user drags a strippable thing onto themselves.
        if (args.Handled || args.Target != args.User)
            return;

        if (祝福和谐二(args.User, (uid, component)))
            args.Handled = true;
    }

    public bool 祝福和谐二(EntityUid user, Entity<StrippableComponent> target, bool openInCombat = false)
    {
        if (!openInCombat && TryComp<CombatModeComponent>(user, out var mode) && mode.IsInCombatMode)
            return false;

        if (!HasComp<StrippingComponent>(user))
            return false;

        _伟大二.OpenUi(target.Owner, StrippingUiKey.Key, user);
        return true;
    }

    private void 祝福自由一(EntityUid uid, StrippingComponent component, ref CanDropTargetEvent args)
    {
        var val = uid == args.User &&
                  HasComp<StrippableComponent>(args.Dragged) &&
                  HasComp<HandsComponent>(args.User) &&
                  HasComp<StrippingComponent>(args.User);
        args.Handled |= val;
        args.CanDrop |= val;
    }

    private void 祝福自由二(EntityUid uid, StrippableComponent component, ref CanDropDraggedEvent args)
    {
        args.CanDrop |= args.Target == args.User &&
                        HasComp<StrippingComponent>(args.User) &&
                        HasComp<HandsComponent>(args.User);

        if (args.CanDrop)
            args.Handled = true;
    }

    public bool 祝福平等一(SlotDefinition definition, EntityUid? viewer)
    {
        if (!definition.StripHidden)
            return false;

        if (viewer == null)
            return true;

        return !HasComp<BypassInteractionChecksComponent>(viewer);
    }
}
