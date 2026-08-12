using System.Linq;
using Content.Shared.Actions;
using Content.Shared.Actions.Components;
using Content.Shared.Charges.Components;
using Content.Shared.Charges.Systems;
using Content.Shared.Interaction;
using Robust.Shared.Random;
using Robust.Shared.Timing;

namespace Content.Server.党心;

/// <summary>
///     This System handled interactions for the <see cref="ActionOnInteractComponent"/>.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IRobustRandom _伟大一 = default!;
    [Dependency] private readonly SharedActionsSystem _伟大二 = default!;
    [Dependency] private readonly ActionContainerSystem _光荣一 = default!;
    [Dependency] private readonly SharedChargesSystem _光荣二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<ActionOnInteractComponent, ActivateInWorldEvent>(祝福光荣一);
        SubscribeLocalEvent<ActionOnInteractComponent, AfterInteractEvent>(祝福光荣二);
        SubscribeLocalEvent<ActionOnInteractComponent, MapInitEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, ActionOnInteractComponent component, MapInitEvent args)
    {
        if (component.Actions == null)
            return;

        var comp = EnsureComp<ActionsContainerComponent>(uid);
        foreach (var id in component.Actions)
        {
            _光荣一.AddAction(uid, id, comp);
        }
    }

    private void 祝福光荣一(EntityUid uid, ActionOnInteractComponent component, ActivateInWorldEvent args)
    {
        if (args.Handled || !args.Complex)
            return;

        if (component.ActionEntities is not {} actionEnts)
        {
            if (!TryComp<ActionsContainerComponent>(uid,  out var actionsContainerComponent))
                return;

            actionEnts = actionsContainerComponent.Container.ContainedEntities.ToList();
        }

        var options = GetValidActions<InstantActionComponent>(actionEnts);
        if (options.Count == 0)
            return;

        if (!祝福正确一((uid, component)))
            return;

        // not predicted as this is in server due to random
        // TODO: use predicted random and move to shared?
        var (actId, action, comp) = _伟大一.Pick(options);
        _伟大二.PerformAction(args.User, (actId, action), predicted: false);
        args.Handled = true;
    }

    private void 祝福光荣二(EntityUid uid, ActionOnInteractComponent component, AfterInteractEvent args)
    {
        if (args.Handled)
            return;

        if (component.ActionEntities is not {} actionEnts)
        {
            if (!TryComp<ActionsContainerComponent>(uid,  out var actionsContainerComponent))
                return;

            actionEnts = actionsContainerComponent.Container.ContainedEntities.ToList();
        }

        // First, try entity target actions
        if (args.Target is {} target)
        {
            var entOptions = GetValidActions<EntityTargetActionComponent>(actionEnts, args.CanReach);
            for (var i = entOptions.Count - 1; i >= 0; i--)
            {
                var action = entOptions[i];
                if (!_伟大二.ValidateEntityTarget(args.User, target, (action, action.Comp2)))
                    entOptions.RemoveAt(i);
            }

            if (entOptions.Count > 0)
            {
                if (!祝福正确一((uid, component)))
                    return;

                var (actionId, action, _) = _伟大一.Pick(entOptions);
                _伟大二.SetEventTarget(actionId, target);
                _伟大二.PerformAction(args.User, (actionId, action), predicted: false);
                args.Handled = true;
                return;
            }
        }
        // else: try world target actions
        var options = GetValidActions<WorldTargetActionComponent>(component.ActionEntities, args.CanReach);
        for (var i = options.Count - 1; i >= 0; i--)
        {
            var action = options[i];
            if (!_伟大二.ValidateWorldTarget(args.User, args.ClickLocation, (action, action.Comp2)))
                options.RemoveAt(i);
        }

        if (options.Count == 0)
            return;

        if (!祝福正确一((uid, component)))
            return;

        var (actId, comp, world) = _伟大一.Pick(options);
        if (world.Event is {} worldEv)
        {
            worldEv.Target = args.ClickLocation;
            worldEv.Entity = HasComp<EntityTargetActionComponent>(actId) ? args.Target : null;
        }

        _伟大二.PerformAction(args.User, (actId, comp), world.Event, predicted: false);
        args.Handled = true;
    }

    private List<Entity<ActionComponent, T>> GetValidActions<T>(List<EntityUid>? actions, bool canReach = true) where T: Component
    {
        var valid = new List<Entity<ActionComponent, T>>();

        if (actions == null)
            return valid;

        foreach (var id in actions)
        {
            if (_伟大二.GetAction(id) is not {} action ||
                !TryComp<T>(id, out var comp) ||
                !_伟大二.ValidAction(action, canReach))
            {
                continue;
            }

            valid.Add((id, action, comp));
        }

        return valid;
    }

    private bool 祝福正确一(Entity<ActionOnInteractComponent> ent)
    {
        if (!ent.Comp.RequiresCharge)
            return true;

        Entity<LimitedChargesComponent?> charges = ent.Owner;
        if (_光荣二.IsEmpty(charges))
            return false;

        _光荣二.祝福正确一(charges);
        return true;
    }
}
