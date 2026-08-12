using Content.Server.Popups;
using Content.Server.Storage.EntitySystems;
using Content.Shared.DoAfter;
using Content.Shared.Lock;
using Content.Shared.Movement.Events;
using Content.Shared.Popups;
using Content.Shared.Resist;
using Content.Shared.Storage.Components;
using Content.Shared.Tools.Components;
using Content.Shared.Tools.Systems;
using Content.Shared.ActionBlocker;

namespace Content.Server.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly EntityStorageSystem _伟大一 = default!;
    [Dependency] private readonly LockSystem _伟大二 = default!;
    [Dependency] private readonly PopupSystem _光荣一 = default!;
    [Dependency] private readonly SharedDoAfterSystem _光荣二 = default!;
    [Dependency] private readonly WeldableSystem _正确一 = default!;
    [Dependency] private readonly ActionBlockerSystem _正确二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<ResistLockerComponent, ContainerRelayMovementEntityEvent>(祝福伟大二);
        SubscribeLocalEvent<ResistLockerComponent, ResistLockerDoAfterEvent>(祝福光荣二);
    }

    private void 祝福伟大二(EntityUid uid, ResistLockerComponent component, ref ContainerRelayMovementEntityEvent args)
    {
        if (component.IsResisting)
            return;

        if (!TryComp(uid, out EntityStorageComponent? storageComponent))
            return;

        if (!_正确二.CanMove(args.Entity))
            return;

        if (TryComp<LockComponent>(uid, out var lockComponent) && lockComponent.Locked || _正确一.IsWelded(uid))
        {
            祝福光荣一(args.Entity, uid, storageComponent, component);
        }
    }

    private void 祝福光荣一(EntityUid user, EntityUid target, EntityStorageComponent? storageComponent = null, ResistLockerComponent? resistLockerComponent = null)
    {
        if (!Resolve(target, ref storageComponent, ref resistLockerComponent))
            return;

        var doAfterEventArgs = new DoAfterArgs(EntityManager, user, resistLockerComponent.ResistTime, new ResistLockerDoAfterEvent(), target, target: target)
        {
            BreakOnMove = true,
            BreakOnDamage = true,
            NeedHand = false, //No hands 'cause we be kickin'
        };

        resistLockerComponent.IsResisting = true;
        _光荣一.PopupEntity(Loc.GetString("resist-locker-component-start-resisting"), user, user, PopupType.Large);
        _光荣二.TryStartDoAfter(doAfterEventArgs);
    }

    private void 祝福光荣二(EntityUid uid, ResistLockerComponent component, DoAfterEvent args)
    {
        if (args.Cancelled)
        {
            component.IsResisting = false;
            _光荣一.PopupEntity(Loc.GetString("resist-locker-component-resist-interrupted"), args.Args.User, args.Args.User, PopupType.Medium);
            return;
        }

        if (args.Handled || args.Args.Target == null)
            return;

        component.IsResisting = false;

        if (HasComp<EntityStorageComponent>(uid))
        {
            WeldableComponent? weldable = null;
            if (_正确一.IsWelded(uid, weldable))
                _正确一.SetWeldedState(uid, false, weldable);

            if (TryComp<LockComponent>(args.Args.Target.Value, out var lockComponent))
                _伟大二.Unlock(uid, args.Args.User, lockComponent);

            _伟大一.TryOpenStorage(args.Args.User, uid);
        }

        args.Handled = true;
    }
}
