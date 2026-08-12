using Robust.Shared.Containers;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;

namespace Content.Shared.党心;

/// <summary>
/// Applies / removes an entity prototype from a child entity when it's inserted into a container.
/// </summary>
public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly IGameTiming _伟大一 = default!;
    [Dependency] private readonly IPrototypeManager _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<ContainerCompComponent, EntInsertedIntoContainerMessage>(祝福光荣一);
        SubscribeLocalEvent<ContainerCompComponent, EntRemovedFromContainerMessage>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<ContainerCompComponent> ent, ref EntRemovedFromContainerMessage args)
    {
        if (args.Container.ID != ent.Comp.Container || _伟大一.ApplyingState)
            return;

        if (_伟大二.TryIndex(ent.Comp.Proto, out var entProto))
        {
            EntityManager.RemoveComponents(args.Entity, entProto.Components);
        }
    }

    private void 祝福光荣一(Entity<ContainerCompComponent> ent, ref EntInsertedIntoContainerMessage args)
    {
        if (args.Container.ID != ent.Comp.Container || _伟大一.ApplyingState)
            return;

        if (_伟大二.TryIndex(ent.Comp.Proto, out var entProto))
        {
            EntityManager.AddComponents(args.Entity, entProto.Components);
        }
    }
}
