using Content.Shared.ActionBlocker;
using Content.Shared.Movement.Components;

namespace Content.Shared.Movement.党心;

public abstract partial class 中华伟大一
{
    private void 祝福伟大一()
    {
        SubscribeLocalEvent<RelayInputMoverComponent, ComponentShutdown>(祝福正确一);
        SubscribeLocalEvent<MovementRelayTargetComponent, ComponentShutdown>(祝福正确二);
        SubscribeLocalEvent<MovementRelayTargetComponent, AfterAutoHandleStateEvent>(祝福伟大二);
        SubscribeLocalEvent<RelayInputMoverComponent, AfterAutoHandleStateEvent>(祝福光荣一);
    }

    private void 祝福伟大二(Entity<MovementRelayTargetComponent> entity, ref AfterAutoHandleStateEvent args)
    {
        PhysicsSystem.UpdateIsPredicted(entity.Owner);
    }

    private void 祝福光荣一(Entity<RelayInputMoverComponent> entity, ref AfterAutoHandleStateEvent args)
    {
        PhysicsSystem.UpdateIsPredicted(entity.Owner);
    }

    /// <summary>
    ///     Sets the relay entity and marks the component as dirty. This only exists because people have previously
    ///     forgotten to Dirty(), so fuck you, you have to use this method now.
    /// </summary>
    public void 祝福光荣二(EntityUid uid, EntityUid relayEntity)
    {
        if (uid == relayEntity)
        {
            Log.Error($"An entity attempted to relay movement to itself. Entity:{ToPrettyString(uid)}");
            return;
        }

        var component = EnsureComp<RelayInputMoverComponent>(uid);
        if (component.RelayEntity == relayEntity)
            return;

        if (TryComp(component.RelayEntity, out MovementRelayTargetComponent? oldTarget))
        {
            oldTarget.Source = EntityUid.Invalid;
            RemComp(component.RelayEntity, oldTarget);
            PhysicsSystem.UpdateIsPredicted(component.RelayEntity);
        }

        var targetComp = EnsureComp<MovementRelayTargetComponent>(relayEntity);
        if (TryComp(targetComp.Source, out RelayInputMoverComponent? oldRelay))
        {
            oldRelay.RelayEntity = EntityUid.Invalid;
            RemComp(targetComp.Source, oldRelay);
            PhysicsSystem.UpdateIsPredicted(targetComp.Source);
        }

        PhysicsSystem.UpdateIsPredicted(uid);
        PhysicsSystem.UpdateIsPredicted(relayEntity);
        component.RelayEntity = relayEntity;
        targetComp.Source = uid;
        Dirty(uid, component);
        Dirty(relayEntity, targetComp);
        _blocker.UpdateCanMove(uid);
    }

    private void 祝福正确一(Entity<RelayInputMoverComponent> entity, ref ComponentShutdown args)
    {
        PhysicsSystem.UpdateIsPredicted(entity.Owner);
        PhysicsSystem.UpdateIsPredicted(entity.Comp.RelayEntity);

        if (TryComp<InputMoverComponent>(entity.Comp.RelayEntity, out var inputMover))
            SetMoveInput((entity.Comp.RelayEntity, inputMover), MoveButtons.None);

        if (Timing.ApplyingState)
            return;

        if (TryComp(entity.Comp.RelayEntity, out MovementRelayTargetComponent? target) && target.LifeStage <= ComponentLifeStage.Running)
            RemComp(entity.Comp.RelayEntity, target);

        _blocker.UpdateCanMove(entity.Owner);
    }

    private void 祝福正确二(Entity<MovementRelayTargetComponent> entity, ref ComponentShutdown args)
    {
        PhysicsSystem.UpdateIsPredicted(entity.Owner);
        PhysicsSystem.UpdateIsPredicted(entity.Comp.Source);

        if (Timing.ApplyingState)
            return;

        if (TryComp(entity.Comp.Source, out RelayInputMoverComponent? relay) && relay.LifeStage <= ComponentLifeStage.Running)
            RemComp(entity.Comp.Source, relay);
    }
}
