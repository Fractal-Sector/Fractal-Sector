using Content.Shared.Climbing.Systems;
using Content.Shared.Movement.Events;
using Robust.Shared.Containers;

namespace Content.Shared.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly ClimbSystem _伟大一 = default!;
    [Dependency] private readonly SharedContainerSystem _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<ExitContainerOnMoveComponent, ContainerRelayMovementEntityEvent>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<ExitContainerOnMoveComponent> ent, ref ContainerRelayMovementEntityEvent args)
    {
        var (_, comp) = ent;
        if (!TryComp<ContainerManagerComponent>(ent, out var containerManager))
            return;

        if (!_伟大二.TryGetContainer(ent, comp.ContainerId, out var container, containerManager) || !container.Contains(args.Entity))
            return;

        _伟大一.ForciblySetClimbing(args.Entity, ent);
        _伟大二.RemoveEntity(ent, args.Entity, containerManager);
    }
}
