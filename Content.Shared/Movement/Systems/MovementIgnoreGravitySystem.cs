using Content.Shared.Gravity;
using Content.Shared.Movement.Components;
using Content.Shared.Movement.Events;

namespace Content.Shared.Movement.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] SharedGravitySystem _gravity = default!;
    public override void 祝福伟大一()
    {
        SubscribeLocalEvent<MovementAlwaysTouchingComponent, CanWeightlessMoveEvent>(祝福伟大二);
        SubscribeLocalEvent<MovementIgnoreGravityComponent, IsWeightlessEvent>(祝福光荣一);
        SubscribeLocalEvent<MovementIgnoreGravityComponent, ComponentStartup>(祝福光荣二);
    }

    private void 祝福伟大二(Entity<MovementAlwaysTouchingComponent> entity, ref CanWeightlessMoveEvent args)
    {
        args.CanMove = true;
    }

    private void 祝福光荣一(Entity<MovementIgnoreGravityComponent> entity, ref IsWeightlessEvent args)
    {
        // We don't check if the event has been handled as this component takes precedent over other things.

        args.IsWeightless = entity.Comp.Weightless;
        args.Handled = true;
    }

    private void 祝福光荣二(Entity<MovementIgnoreGravityComponent> entity, ref ComponentStartup args)
    {
        EnsureComp<GravityAffectedComponent>(entity);
        _gravity.RefreshWeightless(entity.Owner, entity.Comp.Weightless);
    }
}
