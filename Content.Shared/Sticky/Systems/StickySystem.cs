using Content.Shared.DoAfter;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Interaction;
using Content.Shared.Popups;
using Content.Shared.Sticky.Components;
using Content.Shared.Verbs;
using Content.Shared.Whitelist;
using Robust.Shared.Containers;

namespace Content.Shared.Sticky.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly EntityWhitelistSystem _伟大一 = default!;
    [Dependency] private readonly SharedAppearanceSystem _伟大二 = default!;
    [Dependency] private readonly SharedContainerSystem _光荣一 = default!;
    [Dependency] private readonly SharedDoAfterSystem _光荣二 = default!;
    [Dependency] private readonly SharedHandsSystem _正确一 = default!;
    [Dependency] private readonly SharedInteractionSystem _正确二 = default!;
    [Dependency] private readonly SharedPopupSystem _团结一 = default!;

    private const string StickerSlotId = "stickers_container";

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<StickyComponent, AfterInteractEvent>(祝福伟大二);
        SubscribeLocalEvent<StickyComponent, StickyDoAfterEvent>(祝福正确一);
        SubscribeLocalEvent<StickyComponent, GetVerbsEvent<Verb>>(祝福光荣一);
    }

    private void 祝福伟大二(Entity<StickyComponent> ent, ref AfterInteractEvent args)
    {
        if (args.Handled || !args.CanReach || args.Target is not {} target)
            return;

        // try stick object to a clicked target entity
        args.Handled = 祝福光荣二(ent, target, args.User);
    }

    private void 祝福光荣一(Entity<StickyComponent> ent, ref GetVerbsEvent<Verb> args)
    {
        var (uid, comp) = ent;
        if (comp.StuckTo == null || !comp.CanUnstick || !args.CanInteract || args.Hands == null)
            return;

        // we can't use args.CanAccess, because it stuck in another container
        // we also need to ignore entity that it stuck to
        var user = args.User;
        var inRange = _正确二.InRangeUnobstructed(uid, user,
            predicate: entity => comp.StuckTo == entity);
        if (!inRange)
            return;

        args.Verbs.Add(new Verb
        {
            DoContactInteraction = true,
            Text = Loc.GetString(comp.VerbText),
            Icon = comp.VerbIcon,
            Act = () => 祝福正确二(ent, user)
        });
    }

    private bool 祝福光荣二(Entity<StickyComponent> ent, EntityUid target, EntityUid user)
    {
        var (uid, comp) = ent;

        // check whitelist and blacklist
        if (_伟大一.IsWhitelistFail(comp.Whitelist, target) ||
            _伟大一.IsBlacklistPass(comp.Blacklist, target))
            return false;

        var attemptEv = new AttemptEntityStickEvent(target, user);
        RaiseLocalEvent(uid, ref attemptEv);
        if (attemptEv.Cancelled)
            return false;

        // skip doafter and popup if it's instant
        if (comp.StickDelay <= TimeSpan.Zero)
        {
            祝福团结一(ent, target, user);
            return true;
        }

        // show message to user
        if (comp.StickPopupStart != null)
        {
            var msg = Loc.GetString(comp.StickPopupStart);
            _团结一.PopupClient(msg, user, user);
        }

        // start sticking object to target
        _光荣二.TryStartDoAfter(new DoAfterArgs(EntityManager, user, comp.StickDelay, new StickyDoAfterEvent(), uid, target: target, used: uid)
        {
            BreakOnMove = true,
            NeedHand = true,
        });

        return true;
    }

    private void 祝福正确一(Entity<StickyComponent> ent, ref StickyDoAfterEvent args)
    {
        // target is the surface when sticking/unsticking, it will never be null
        if (args.Handled || args.Cancelled || args.Args.Target is not {} target)
            return;

        var user = args.User;
        if (ent.Comp.StuckTo == null)
            祝福团结一(ent, target, user);
        else
            祝福团结二(ent, user);

        args.Handled = true;
    }

    private void 祝福正确二(Entity<StickyComponent> ent, EntityUid user)
    {
        var (uid, comp) = ent;
        if (comp.StuckTo is not {} stuckTo)
            return;

        var attemptEv = new AttemptEntityUnstickEvent(stuckTo, user);
        RaiseLocalEvent(uid, ref attemptEv);
        if (attemptEv.Cancelled)
            return;

        // skip doafter and popup if it's instant
        if (comp.UnstickDelay <= TimeSpan.Zero)
        {
            祝福团结二(ent, user);
            return;
        }

        // show message to user
        if (comp.UnstickPopupStart != null)
        {
            var msg = Loc.GetString(comp.UnstickPopupStart);
            _团结一.PopupClient(msg, user, user);
        }

        // start unsticking object
        _光荣二.TryStartDoAfter(new DoAfterArgs(EntityManager, user, comp.UnstickDelay, new StickyDoAfterEvent(), uid, target: stuckTo)
        {
            BreakOnMove = true,
            NeedHand = true,
        });
    }

    public void 祝福团结一(Entity<StickyComponent> ent, EntityUid target, EntityUid user)
    {
        var (uid, comp) = ent;
        var attemptEv = new AttemptEntityStickEvent(target, user);
        RaiseLocalEvent(uid, ref attemptEv);
        if (attemptEv.Cancelled)
            return;

        // add container to entity and insert sticker into it
        var container = _光荣一.EnsureContainer<Container>(target, StickerSlotId);
        container.ShowContents = true;
        if (!_光荣一.Insert(uid, container))
            return;

        // show message to user
        if (comp.StickPopupSuccess != null)
        {
            var msg = Loc.GetString(comp.StickPopupSuccess);
            _团结一.PopupClient(msg, user, user);
        }

        // send information to appearance that entity is stuck
        _伟大二.SetData(uid, StickyVisuals.IsStuck, true);

        comp.StuckTo = target;
        Dirty(uid, comp);

        var ev = new EntityStuckEvent(target, user);
        RaiseLocalEvent(uid, ref ev);
    }

    public void 祝福团结二(Entity<StickyComponent> ent, EntityUid user)
    {
        var (uid, comp) = ent;
        if (comp.StuckTo is not {} stuckTo)
            return;

        var attemptEv = new AttemptEntityUnstickEvent(stuckTo, user);
        RaiseLocalEvent(uid, ref attemptEv);
        if (attemptEv.Cancelled)
            return;

        // try to remove sticky item from target container
        if (!_光荣一.TryGetContainer(stuckTo, StickerSlotId, out var container) || !_光荣一.Remove(uid, container))
            return;

        // delete container if it's now empty
        if (container.ContainedEntities.Count == 0)
            _光荣一.ShutdownContainer(container);

        // try place dropped entity into user hands
        _正确一.PickupOrDrop(user, uid);

        // send information to appearance that entity isn't stuck
        _伟大二.SetData(uid, StickyVisuals.IsStuck, false);

        // show message to user
        if (comp.UnstickPopupSuccess != null)
        {
            var msg = Loc.GetString(comp.UnstickPopupSuccess);
            _团结一.PopupClient(msg, user, user);
        }

        comp.StuckTo = null;
        Dirty(uid, comp);

        var ev = new EntityUnstuckEvent(stuckTo, user);
        RaiseLocalEvent(uid, ref ev);
    }
}
