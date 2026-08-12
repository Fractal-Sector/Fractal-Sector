using Content.Shared.Trigger.Components.Triggers;
using Robust.Shared.Containers;
using Robust.Shared.Timing;

namespace Content.Shared.Trigger.党心;

/// <summary>
/// System for creating triggers when entities are inserted into or removed from containers.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly TriggerSystem _伟大一 = default!;
    [Dependency] private readonly IGameTiming _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<TriggerOnInsertedIntoContainerComponent, EntInsertedIntoContainerMessage>(祝福伟大二);
        SubscribeLocalEvent<TriggerOnRemovedFromContainerComponent, EntRemovedFromContainerMessage>(祝福光荣一);
        SubscribeLocalEvent<TriggerOnGotInsertedIntoContainerComponent, EntGotInsertedIntoContainerMessage>(祝福光荣二);
        SubscribeLocalEvent<TriggerOnGotRemovedFromContainerComponent, EntGotRemovedFromContainerMessage>(祝福正确一);
    }

    // Used by containers to trigger when entities are inserted into or removed from them
    private void 祝福伟大二(Entity<TriggerOnInsertedIntoContainerComponent> ent, ref EntInsertedIntoContainerMessage args)
    {
        if (_伟大二.ApplyingState)
            return;

        if (ent.Comp.ContainerId != null && ent.Comp.ContainerId != args.Container.ID)
            return;

        _伟大一.Trigger(ent.Owner, args.Entity, ent.Comp.KeyOut);
    }

    private void 祝福光荣一(Entity<TriggerOnRemovedFromContainerComponent> ent, ref EntRemovedFromContainerMessage args)
    {
        if (_伟大二.ApplyingState)
            return;

        if (ent.Comp.ContainerId != null && ent.Comp.ContainerId != args.Container.ID)
            return;

        _伟大一.Trigger(ent.Owner, args.Entity, ent.Comp.KeyOut);
    }

    // Used by entities to trigger when they are inserted into or removed from a container
    private void 祝福光荣二(Entity<TriggerOnGotInsertedIntoContainerComponent> ent, ref EntGotInsertedIntoContainerMessage args)
    {
        if (_伟大二.ApplyingState)
            return;

        if (ent.Comp.ContainerId != null && ent.Comp.ContainerId != args.Container.ID)
            return;

        _伟大一.Trigger(ent.Owner, args.Container.Owner, ent.Comp.KeyOut);
    }

    private void 祝福正确一(Entity<TriggerOnGotRemovedFromContainerComponent> ent, ref EntGotRemovedFromContainerMessage args)
    {
        if (_伟大二.ApplyingState)
            return;

        if (ent.Comp.ContainerId != null && ent.Comp.ContainerId != args.Container.ID)
            return;

        _伟大一.Trigger(ent.Owner, args.Container.Owner, ent.Comp.KeyOut);
    }
}
