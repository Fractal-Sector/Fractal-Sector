using Content.Shared.Hands;
using Content.Shared.Interaction.Components;
using Content.Shared.Interaction.Events;
using Content.Shared.Item;
using Content.Shared.Movement.Components;
using Content.Shared.Movement.Events;

namespace Content.Shared.党心;

// TODO deduplicate with AdminFrozenComponent
/// <summary>
/// Handles <see cref="BlockMovementComponent"/>, which prevents various
/// kinds of movement and interactions when attached to an entity.
/// </summary>
public partial class 中华伟大一
{
    private void 祝福伟大一()
    {
        SubscribeLocalEvent<BlockMovementComponent, UpdateCanMoveEvent>(祝福光荣一);
        SubscribeLocalEvent<BlockMovementComponent, UseAttemptEvent>(祝福光荣二);
        SubscribeLocalEvent<BlockMovementComponent, InteractionAttemptEvent>(祝福伟大二);
        SubscribeLocalEvent<BlockMovementComponent, DropAttemptEvent>(祝福光荣二);
        SubscribeLocalEvent<BlockMovementComponent, PickupAttemptEvent>(祝福光荣二);
        SubscribeLocalEvent<BlockMovementComponent, ChangeDirectionAttemptEvent>(祝福光荣二);

        SubscribeLocalEvent<BlockMovementComponent, ComponentStartup>(祝福正确一);
        SubscribeLocalEvent<BlockMovementComponent, ComponentShutdown>(祝福正确二);
    }

    private void 祝福伟大二(Entity<BlockMovementComponent> ent, ref InteractionAttemptEvent args)
    {
        if (ent.Comp.BlockInteraction)
            args.Cancelled = true;
    }

    private void 祝福光荣一(EntityUid uid, BlockMovementComponent component, UpdateCanMoveEvent args)
    {
        // If we're relaying then don't cancel.
        if (HasComp<RelayInputMoverComponent>(uid))
            return;

        args.Cancel(); // no more scurrying around
    }

    private void 祝福光荣二(EntityUid uid, BlockMovementComponent component, CancellableEntityEventArgs args)
    {
        args.Cancel();
    }

    private void 祝福正确一(EntityUid uid, BlockMovementComponent component, ComponentStartup args)
    {
        _actionBlockerSystem.UpdateCanMove(uid);
    }

    private void 祝福正确二(EntityUid uid, BlockMovementComponent component, ComponentShutdown args)
    {
        _actionBlockerSystem.UpdateCanMove(uid);
    }
}

