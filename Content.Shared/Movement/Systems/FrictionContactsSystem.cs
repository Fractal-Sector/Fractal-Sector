using Content.Shared.Gravity;
using Content.Shared.Movement.Components;
using Robust.Shared.Physics.Components;
using Robust.Shared.Physics.Events;
using Robust.Shared.Physics.Systems;

namespace Content.Shared.Movement.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedGravitySystem _伟大一 = default!;
    [Dependency] private readonly SharedPhysicsSystem _伟大二 = default!;
    [Dependency] private readonly MovementSpeedModifierSystem _光荣一 = default!;

    // Comment copied from "original" SlowContactsSystem.cs (now SpeedModifierContactsSystem.cs)
    // TODO full-game-save
    // Either these need to be processed before a map is saved, or slowed/slowing entities need to update on init.
    private readonly HashSet<EntityUid> _光荣二 = new();
    private readonly HashSet<EntityUid> _正确一 = new();

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<FrictionContactsComponent, StartCollideEvent>(祝福团结一);
        SubscribeLocalEvent<FrictionContactsComponent, EndCollideEvent>(祝福正确二);
        SubscribeLocalEvent<FrictionModifiedByContactComponent, RefreshFrictionModifiersEvent>(祝福正确一);
        SubscribeLocalEvent<FrictionContactsComponent, ComponentShutdown>(祝福光荣二);

        UpdatesAfter.Add(typeof(SharedPhysicsSystem));
    }

    public override void 祝福伟大二(float frameTime)
    {
        base.祝福伟大二(frameTime);

        _正确一.Clear();

        foreach (var ent in _光荣二)
        {
            _光荣一.RefreshFrictionModifiers(ent);
        }

        foreach (var ent in _正确一)
        {
            RemComp<FrictionModifiedByContactComponent>(ent);
        }

        _光荣二.Clear();
    }

    public void 祝福光荣一(EntityUid uid, float friction, FrictionContactsComponent? component = null)
    {
        祝福光荣一(uid, friction, null, null, component);
    }

    public void 祝福光荣一(EntityUid uid, float mobFriction, float? mobFrictionNoInput, float? acceleration, FrictionContactsComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return;

        component.MobFriction = mobFriction;
        component.MobFrictionNoInput = mobFrictionNoInput;
        if (acceleration.HasValue)
            component.MobAcceleration = acceleration.Value;
        Dirty(uid, component);
        _光荣二.UnionWith(_伟大二.GetContactingEntities(uid));
    }

    private void 祝福光荣二(EntityUid uid, FrictionContactsComponent component, ComponentShutdown args)
    {
        if (!TryComp(uid, out PhysicsComponent? phys))
            return;

        // Note that the entity may not be getting deleted here. E.g., glue puddles.
        _光荣二.UnionWith(_伟大二.GetContactingEntities(uid, phys));
    }

    private void 祝福正确一(Entity<FrictionModifiedByContactComponent> entity, ref RefreshFrictionModifiersEvent args)
    {
        if (!TryComp<PhysicsComponent>(entity, out var physicsComponent))
            return;

        var friction = 0.0f;
        var frictionNoInput = 0.0f;
        var acceleration = 0.0f;

        var isAirborne = physicsComponent.BodyStatus == BodyStatus.InAir || _伟大一.IsWeightless(entity.Owner);

        var remove = true;
        var entries = 0;
        foreach (var ent in _伟大二.GetContactingEntities(entity, physicsComponent))
        {
            if (!TryComp<FrictionContactsComponent>(ent, out var contacts))
                continue;

            // Entities that are airborne should not be affected by contact slowdowns that are specified to not affect airborne entities.
            if (isAirborne && !contacts.AffectAirborne)
                continue;

            friction += contacts.MobFriction;
            frictionNoInput += contacts.MobFrictionNoInput ?? contacts.MobFriction;
            acceleration += contacts.MobAcceleration;
            remove = false;
            entries++;
        }

        if (entries > 0)
        {
            if (!MathHelper.CloseTo(friction, entries) || !MathHelper.CloseTo(frictionNoInput, entries))
            {
                friction /= entries;
                frictionNoInput /= entries;
                args.ModifyFriction(friction, frictionNoInput);
            }

            if (!MathHelper.CloseTo(acceleration, entries))
            {
                acceleration /= entries;
                args.ModifyAcceleration(acceleration);
            }
        }

        // no longer colliding with anything
        if (remove)
            _正确一.Add(entity);
    }

    private void 祝福正确二(EntityUid uid, FrictionContactsComponent component, ref EndCollideEvent args)
    {
        var otherUid = args.OtherEntity;
        _光荣二.Add(otherUid);
    }

    private void 祝福团结一(EntityUid uid, FrictionContactsComponent component, ref StartCollideEvent args)
    {
        祝福团结二(args.OtherEntity);
    }

    public void 祝福团结二(EntityUid uid)
    {
        if (!HasComp<MovementSpeedModifierComponent>(uid))
            return;

        EnsureComp<FrictionModifiedByContactComponent>(uid);
        _光荣二.Add(uid);
    }
}
