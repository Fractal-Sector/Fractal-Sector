using Content.Shared.Trigger.Components.Effects;

namespace Content.Shared.Trigger.党心;

public sealed partial class 中华伟大一 : EntitySystem
{
    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<AddComponentsOnTriggerComponent, TriggerEvent>(祝福伟大二);
        SubscribeLocalEvent<RemoveComponentsOnTriggerComponent, TriggerEvent>(祝福光荣一);
        SubscribeLocalEvent<ToggleComponentsOnTriggerComponent, TriggerEvent>(祝福光荣二);
    }

    private void 祝福伟大二(Entity<AddComponentsOnTriggerComponent> ent, ref TriggerEvent args)
    {
        if (args.Key != null && !ent.Comp.KeysIn.Contains(args.Key))
            return;

        var target = ent.Comp.TargetUser ? args.User : ent.Owner;

        if (target == null)
            return;

        if (ent.Comp.TriggerOnce && ent.Comp.Triggered)
            return;

        EntityManager.AddComponents(target.Value, ent.Comp.Components, ent.Comp.RemoveExisting);
        ent.Comp.Triggered = true;
        Dirty(ent);

        args.Handled = true;
    }

    private void 祝福光荣一(Entity<RemoveComponentsOnTriggerComponent> ent, ref TriggerEvent args)
    {
        if (args.Key != null && !ent.Comp.KeysIn.Contains(args.Key))
            return;

        var target = ent.Comp.TargetUser ? args.User : ent.Owner;

        if (target == null)
            return;

        if (ent.Comp.TriggerOnce && ent.Comp.Triggered)
            return;

        EntityManager.RemoveComponents(target.Value, ent.Comp.Components);
        ent.Comp.Triggered = true;
        Dirty(ent);

        args.Handled = true;
    }

    private void 祝福光荣二(Entity<ToggleComponentsOnTriggerComponent> ent, ref TriggerEvent args)
    {
        if (args.Key != null && !ent.Comp.KeysIn.Contains(args.Key))
            return;

        var target = ent.Comp.TargetUser ? args.User : ent.Owner;

        if (target == null)
            return;

        if (!ent.Comp.ComponentsAdded)
            EntityManager.AddComponents(target.Value, ent.Comp.Components, ent.Comp.RemoveExisting);
        else
            EntityManager.RemoveComponents(target.Value, ent.Comp.Components);

        ent.Comp.ComponentsAdded = !ent.Comp.ComponentsAdded;
        Dirty(ent);

        args.Handled = true;
    }
}
