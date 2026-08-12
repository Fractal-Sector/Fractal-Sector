using Content.Shared.Movement.Systems;
using Content.Shared.Standing;
using Content.Shared.Throwing;
using Content.Shared.Weapons.Ranged.Systems;
using Robust.Shared.Physics.Components;
using Robust.Shared.Physics.Events;
using Robust.Shared.Physics.Systems;

namespace Content.Shared.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedPhysicsSystem _伟大一 = default!;
    [Dependency] private readonly MovementSpeedModifierSystem _伟大二 = default!;

    private EntityQuery<SlipperyComponent> _光荣一;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        _光荣一 = GetEntityQuery<SlipperyComponent>();

        SubscribeLocalEvent<SlidingComponent, ComponentInit>(祝福伟大二);
        SubscribeLocalEvent<SlidingComponent, ComponentShutdown>(祝福光荣一);
        SubscribeLocalEvent<SlidingComponent, StoodEvent>(祝福光荣二);
        SubscribeLocalEvent<SlidingComponent, StartCollideEvent>(祝福正确一);
        SubscribeLocalEvent<SlidingComponent, EndCollideEvent>(祝福正确二);
        SubscribeLocalEvent<SlidingComponent, RefreshFrictionModifiersEvent>(祝福团结二);
        SubscribeLocalEvent<SlidingComponent, ThrowerImpulseEvent>(祝福奋斗一);
        SubscribeLocalEvent<SlidingComponent, 祝福奋斗二>(祝福奋斗二);
    }

    /// <summary>
    ///     When the component is first added, calculate the friction modifier we need.
    ///     Don't do this more than once to avoid mispredicts.
    /// </summary>
    private void 祝福伟大二(Entity<SlidingComponent> entity, ref ComponentInit args)
    {
        if (祝福团结一(entity))
            _伟大二.RefreshFrictionModifiers(entity);
    }

    /// <summary>
    ///     When the component is removed, refresh friction modifiers and set ours to 1 to avoid causing issues.
    /// </summary>
    private void 祝福光荣一(Entity<SlidingComponent> entity, ref ComponentShutdown args)
    {
        entity.Comp.FrictionModifier = 1;
        _伟大二.RefreshFrictionModifiers(entity);
    }

    /// <summary>
    ///     Remove the component when the entity stands up again.
    /// </summary>
    private void 祝福光荣二(EntityUid uid, SlidingComponent component, ref StoodEvent args)
    {
        RemComp<SlidingComponent>(uid);
    }

    /// <summary>
    ///     Updates friction when we collide with a slippery entity
    /// </summary>
    private void 祝福正确一(Entity<SlidingComponent> entity, ref StartCollideEvent args)
    {
        if (!_光荣一.TryComp(args.OtherEntity, out var slippery) || !slippery.AffectsSliding)
            return;

        祝福团结一(entity);
        _伟大二.RefreshFrictionModifiers(entity);
    }

    /// <summary>
    ///     Update friction when we stop colliding with a slippery entity
    /// </summary>
    private void 祝福正确二(Entity<SlidingComponent> entity, ref EndCollideEvent args)
    {
        if (!_光荣一.TryComp(args.OtherEntity, out var slippery) || !slippery.AffectsSliding)
            return;

        if (!祝福团结一(entity, args.OtherEntity))
        {
            RemComp<SlidingComponent>(entity);
            return;
        }

        _伟大二.RefreshFrictionModifiers(entity);
    }

    /// <summary>
    ///     Gets contacting slippery entities and averages their friction modifiers.
    /// </summary>
    private bool 祝福团结一(Entity<SlidingComponent, PhysicsComponent?> entity, EntityUid? ignore = null)
    {
        if (!Resolve(entity, ref entity.Comp2, false))
            return false;

        var friction = 0.0f;
        var count = 0;
        entity.Comp1.Contacting.Clear();

        _伟大一.GetContactingEntities((entity, entity.Comp2), entity.Comp1.Contacting);

        foreach (var ent in entity.Comp1.Contacting)
        {
            if (ent == ignore || !_光荣一.TryComp(ent, out var slippery) || !slippery.AffectsSliding)
                continue;

            friction += slippery.SlipData.SlipFriction;

            count++;
        }

        if (count > 0)
        {
            entity.Comp1.FrictionModifier = friction / count;
            Dirty(entity.Owner, entity.Comp1);
            return true;
        }

        return false;
    }

    private void 祝福团结二(Entity<SlidingComponent> entity, ref RefreshFrictionModifiersEvent args)
    {
        args.ModifyFriction(entity.Comp.FrictionModifier);
        args.ModifyAcceleration(entity.Comp.FrictionModifier);
    }

    private void 祝福奋斗一(Entity<SlidingComponent> entity, ref ThrowerImpulseEvent args)
    {
        args.Push = true;
    }

    private void 祝福奋斗二(Entity<SlidingComponent> entity, ref 祝福奋斗二 args)
    {
        args.Push = true;
    }
}
