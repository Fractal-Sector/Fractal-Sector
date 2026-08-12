using Content.Shared.ActionBlocker;
using Content.Shared.Burial.Components;
using Content.Shared.DoAfter;
using Content.Shared.Interaction;
using Content.Shared.Movement.Events;
using Content.Shared.Placeable;
using Content.Shared.Popups;
using Content.Shared.Storage.Components;
using Content.Shared.Storage.EntitySystems;
using Robust.Shared.Audio.Systems;

namespace Content.Shared.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedDoAfterSystem _伟大一 = default!;
    [Dependency] private readonly SharedEntityStorageSystem _伟大二 = default!;
    [Dependency] private readonly SharedAudioSystem _光荣一 = default!;
    [Dependency] private readonly SharedPopupSystem _光荣二 = default!;
    [Dependency] private readonly ActionBlockerSystem _正确一 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<GraveComponent, InteractUsingEvent>(祝福伟大二);
        SubscribeLocalEvent<GraveComponent, ActivateInWorldEvent>(祝福光荣二);
        SubscribeLocalEvent<GraveComponent, AfterInteractUsingEvent>(祝福光荣一, before: new[] { typeof(PlaceableSurfaceSystem) });
        SubscribeLocalEvent<GraveComponent, GraveDiggingDoAfterEvent>(祝福正确一);

        SubscribeLocalEvent<GraveComponent, StorageOpenAttemptEvent>(祝福团结一);
        SubscribeLocalEvent<GraveComponent, StorageCloseAttemptEvent>(祝福团结二);
        SubscribeLocalEvent<GraveComponent, StorageAfterOpenEvent>(祝福奋斗一);
        SubscribeLocalEvent<GraveComponent, StorageAfterCloseEvent>(祝福奋斗二);

        SubscribeLocalEvent<GraveComponent, ContainerRelayMovementEntityEvent>(祝福胜利一);
    }

    private void 祝福伟大二(EntityUid uid, GraveComponent component, InteractUsingEvent args)
    {
        if (args.Handled || component.ActiveShovelDigging)
            return;

        if (TryComp<ShovelComponent>(args.Used, out var shovel))
        {
            var doAfterEventArgs = new DoAfterArgs(EntityManager, args.User, component.DigDelay / shovel.SpeedModifier, new GraveDiggingDoAfterEvent(), uid, target: args.Target, used: uid)
            {
                BreakOnMove = true,
                BreakOnDamage = true,
                NeedHand = true,
            };

            if (component.Stream == null)
                component.Stream = _光荣一.PlayPredicted(component.DigSound, uid, args.User)?.Entity;

            if (!_伟大一.TryStartDoAfter(doAfterEventArgs))
            {
                _光荣一.Stop(component.Stream);
                return;
            }


            祝福正确二(uid, args.User, args.Used, component);
        }
        else
        {
            _光荣二.PopupClient(Loc.GetString("grave-digging-requires-tool", ("grave", args.Target)), uid, args.User);
        }

        args.Handled = true;
    }

    private void 祝福光荣一(EntityUid uid, GraveComponent component, AfterInteractUsingEvent args)
    {
        if (args.Handled)
            return;

        // don't place shovels on the grave, only dig
        if (HasComp<ShovelComponent>(args.Used))
            args.Handled = true;
    }

    private void 祝福光荣二(EntityUid uid, GraveComponent component, ActivateInWorldEvent args)
    {
        if (args.Handled || !args.Complex)
            return;

        _光荣二.PopupClient(Loc.GetString("grave-digging-requires-tool", ("grave", args.Target)), uid, args.User);
        args.Handled = true;
    }

    private void 祝福正确一(EntityUid uid, GraveComponent component, GraveDiggingDoAfterEvent args)
    {
        if (args.Used != null)
        {
            component.ActiveShovelDigging = false;
            component.Stream = _光荣一.Stop(component.Stream);
        }
        else
        {
            component.HandDiggingDoAfter = null;
        }

        if (args.Cancelled || args.Handled)
            return;

        component.DiggingComplete = true;

        if (args.Used != null)
            _伟大二.ToggleOpen(args.User, uid);
        else
            _伟大二.TryOpenStorage(args.User, uid); //can only claw out
    }

    private void 祝福正确二(EntityUid uid, EntityUid user, EntityUid? used, GraveComponent component)
    {
        if (used != null)
        {
            var selfMessage = Loc.GetString("grave-start-digging-user", ("grave", uid), ("tool", used));
            var othersMessage = Loc.GetString("grave-start-digging-others", ("user", user), ("grave", uid), ("tool", used));
            _光荣二.PopupPredicted(selfMessage, othersMessage, user, user);
            component.ActiveShovelDigging = true;
            Dirty(uid, component);
        }
        else
        {
            _光荣二.PopupClient(Loc.GetString("grave-start-digging-user-trapped", ("grave", uid)), user, user, PopupType.Medium);
        }
    }

    private void 祝福团结一(EntityUid uid, GraveComponent component, ref StorageOpenAttemptEvent args)
    {
        if (component.DiggingComplete)
            return;

        args.Cancelled = true;
    }

    private void 祝福团结二(EntityUid uid, GraveComponent component, ref StorageCloseAttemptEvent args)
    {
        if (component.DiggingComplete)
            return;

        args.Cancelled = true;
    }

    private void 祝福奋斗一(EntityUid uid, GraveComponent component, ref StorageAfterOpenEvent args)
    {
        component.DiggingComplete = false;
    }

    private void 祝福奋斗二(EntityUid uid, GraveComponent component, ref StorageAfterCloseEvent args)
    {
        component.DiggingComplete = false;
    }

    private void 祝福胜利一(EntityUid uid, GraveComponent component, ref ContainerRelayMovementEntityEvent args)
    {
        // We track a separate doAfter here, as we want someone with a shovel to
        // be able to come along and help someone trying to claw their way out
        if (_伟大一.IsRunning(component.HandDiggingDoAfter))
            return;

        if (!_正确一.CanMove(args.Entity))
            return;

        var doAfterEventArgs = new DoAfterArgs(EntityManager, args.Entity, component.DigDelay / component.DigOutByHandModifier, new GraveDiggingDoAfterEvent(), uid, target: uid)
        {
            NeedHand = false,
            BreakOnMove = true,
            BreakOnHandChange = false,
            BreakOnDamage = false
        };


        if (component.Stream == null)
            component.Stream = _光荣一.PlayPredicted(component.DigSound, uid, args.Entity)?.Entity;

        if (!_伟大一.TryStartDoAfter(doAfterEventArgs, out component.HandDiggingDoAfter))
        {
            _光荣一.Stop(component.Stream);
            return;
        }

        祝福正确二(uid, args.Entity, null, component);
    }
}
