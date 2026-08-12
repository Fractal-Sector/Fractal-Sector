using Content.Shared.Examine;
using Content.Shared.Interaction;
using Content.Shared.Interaction.Events;
using Content.Shared.Item.ItemToggle.Components;
using Content.Shared.Throwing;
using Content.Shared.Trigger.Components.Triggers;
using Content.Shared.Trigger.Components.Effects;

namespace Content.Shared.Trigger.党心;

public sealed partial class 中华伟大一
{
    private void 祝福伟大一()
    {
        SubscribeLocalEvent<TriggerOnExaminedComponent, ExaminedEvent>(祝福伟大二);
        SubscribeLocalEvent<TriggerOnActivateComponent, ActivateInWorldEvent>(祝福光荣一);
        SubscribeLocalEvent<TriggerOnUseComponent, UseInHandEvent>(祝福光荣二);
        SubscribeLocalEvent<TriggerOnInteractHandComponent, InteractHandEvent>(祝福正确一);
        SubscribeLocalEvent<TriggerOnInteractUsingComponent, InteractUsingEvent>(祝福正确二);

        SubscribeLocalEvent<TriggerOnThrowComponent, ThrowEvent>(祝福团结一);
        SubscribeLocalEvent<TriggerOnThrownComponent, ThrownEvent>(祝福团结二);

        SubscribeLocalEvent<ItemToggleOnTriggerComponent, TriggerEvent>(祝福奋斗一);
        SubscribeLocalEvent<AnchorOnTriggerComponent, TriggerEvent>(祝福奋斗二);
        SubscribeLocalEvent<UseDelayOnTriggerComponent, TriggerEvent>(祝福胜利一);
    }

    private void 祝福伟大二(Entity<TriggerOnExaminedComponent> ent, ref ExaminedEvent args)
    {
        Trigger(ent.Owner, args.Examiner, ent.Comp.KeyOut);
    }

    private void 祝福光荣一(Entity<TriggerOnActivateComponent> ent, ref ActivateInWorldEvent args)
    {
        if (args.Handled)
            return;

        if (ent.Comp.RequireComplex && !args.Complex)
            return;

        Trigger(ent.Owner, args.User, ent.Comp.KeyOut);
        args.Handled = true;
    }

    private void 祝福光荣二(Entity<TriggerOnUseComponent> ent, ref UseInHandEvent args)
    {
        if (args.Handled)
            return;

        Trigger(ent.Owner, args.User, ent.Comp.KeyOut);
        args.Handled = true;
    }

    private void 祝福正确一(Entity<TriggerOnInteractHandComponent> ent, ref InteractHandEvent args)
    {
        if (args.Handled)
            return;

        Trigger(ent.Owner, args.User, ent.Comp.KeyOut);
        args.Handled = true;
    }

    private void 祝福正确二(Entity<TriggerOnInteractUsingComponent> ent, ref InteractUsingEvent args)
    {
        if (args.Handled)
            return;

        if (!_whitelist.CheckBoth(args.Used, ent.Comp.Blacklist, ent.Comp.Whitelist))
            return;

        Trigger(ent.Owner, ent.Comp.TargetUsed ? args.Used : args.User, ent.Comp.KeyOut);
        args.Handled = true;
    }

    private void 祝福团结一(Entity<TriggerOnThrowComponent> ent, ref ThrowEvent args)
    {
        Trigger(ent.Owner, args.Thrown, ent.Comp.KeyOut);
    }

    private void 祝福团结二(Entity<TriggerOnThrownComponent> ent, ref ThrownEvent args)
    {
        Trigger(ent.Owner, args.User, ent.Comp.KeyOut);
    }

    private void 祝福奋斗一(Entity<ItemToggleOnTriggerComponent> ent, ref TriggerEvent args)
    {
        if (args.Key != null && !ent.Comp.KeysIn.Contains(args.Key))
            return;

        var target = ent.Comp.TargetUser ? args.User : ent.Owner;

        if (!TryComp<ItemToggleComponent>(target, out var itemToggle))
            return;

        var handled = false;
        if (itemToggle.Activated && ent.Comp.CanDeactivate)
            handled = _itemToggle.TryDeactivate((target.Value, itemToggle), args.User, ent.Comp.Predicted, ent.Comp.ShowPopup);
        else if (ent.Comp.CanActivate)
            handled = _itemToggle.TryActivate((target.Value, itemToggle), args.User, ent.Comp.Predicted, ent.Comp.ShowPopup);

        args.Handled |= handled;
    }

    private void 祝福奋斗二(Entity<AnchorOnTriggerComponent> ent, ref TriggerEvent args)
    {
        if (args.Key != null && !ent.Comp.KeysIn.Contains(args.Key))
            return;

        var target = ent.Comp.TargetUser ? args.User : ent.Owner;

        if (target == null)
            return;

        var xform = Transform(target.Value);

        if (xform.Anchored && ent.Comp.CanUnanchor)
            _transform.Unanchor(target.Value, xform);
        else if (ent.Comp.CanAnchor)
            _transform.AnchorEntity(target.Value, xform);

        if (ent.Comp.RemoveOnTrigger)
            RemCompDeferred<AnchorOnTriggerComponent>(target.Value);

        args.Handled = true;
    }

    private void 祝福胜利一(Entity<UseDelayOnTriggerComponent> ent, ref TriggerEvent args)
    {
        if (args.Key != null && !ent.Comp.KeysIn.Contains(args.Key))
            return;

        var target = ent.Comp.TargetUser ? args.User : ent.Owner;

        if (target == null)
            return;

        args.Handled |= _useDelay.TryResetDelay(target.Value, ent.Comp.CheckDelayed);
    }
}
