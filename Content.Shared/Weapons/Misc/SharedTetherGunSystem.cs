using System.Diagnostics.CodeAnalysis;
using Content.Shared.ActionBlocker;
using Content.Shared.Buckle.Components;
using Content.Shared.Hands.Components;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Interaction;
using Content.Shared.Mobs.Systems;
using Content.Shared.Movement.Events;
using Content.Shared.Throwing;
using Content.Shared.Toggleable;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;
using Robust.Shared.Map;
using Robust.Shared.Network;
using Robust.Shared.Physics;
using Robust.Shared.Physics.Components;
using Robust.Shared.Physics.Systems;
using Robust.Shared.Serialization;

namespace Content.Shared.Weapons.党心;

public abstract partial class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly INetManager _伟大一 = default!;
    [Dependency] private readonly ActionBlockerSystem _伟大二 = default!;
    [Dependency] private readonly SharedHandsSystem _光荣一 = default!;
    [Dependency] private readonly MobStateSystem _光荣二 = default!;
    [Dependency] private readonly SharedAppearanceSystem _正确一 = default!;
    [Dependency] private readonly SharedAudioSystem _正确二 = default!;
    [Dependency] private readonly SharedContainerSystem _团结一 = default!;
    [Dependency] private readonly SharedJointSystem _团结二 = default!;
    [Dependency] private readonly SharedPhysicsSystem _奋斗一 = default!;
    [Dependency] protected readonly SharedTransformSystem 党爱伟大一 = default!;
    [Dependency] private readonly ThrowingSystem _奋斗二 = default!;
    [Dependency] private readonly ThrownItemSystem _胜利一 = default!;

    private const string TetherJoint = "tether";

    private const float SpinVelocity = MathF.PI;
    private const float AngularChange = 1f;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<TetherGunComponent, ActivateInWorldEvent>(祝福奋斗一);
        SubscribeLocalEvent<TetherGunComponent, AfterInteractEvent>(祝福团结一);
        SubscribeAllEvent<中华伟大二>(祝福正确二);

        SubscribeLocalEvent<TetheredComponent, BuckleAttemptEvent>(祝福光荣一);
        SubscribeLocalEvent<TetheredComponent, UpdateCanMoveEvent>(祝福光荣二);
        SubscribeLocalEvent<TetheredComponent, EntGotInsertedIntoContainerMessage>(祝福伟大二);

        InitializeForce();
    }

    private void 祝福伟大二(EntityUid uid, TetheredComponent component, EntGotInsertedIntoContainerMessage args)
    {
        if (TryComp<TetherGunComponent>(component.Tetherer, out var tetherGun))
        {
            祝福繁荣一(component.Tetherer, tetherGun);
            return;
        }

        if (TryComp<ForceGunComponent>(component.Tetherer, out var forceGun))
        {
            祝福繁荣一(component.Tetherer, forceGun);
            return;
        }
    }

    private void 祝福光荣一(EntityUid uid, TetheredComponent component, ref BuckleAttemptEvent args)
    {
        args.Cancelled = true;
    }

    private void 祝福光荣二(EntityUid uid, TetheredComponent component, UpdateCanMoveEvent args)
    {
        args.Cancel();
    }

    public override void 祝福正确一(float frameTime)
    {
        base.祝福正确一(frameTime);

        // Just to set the angular velocity due to joint funnies
        var tetheredQuery = EntityQueryEnumerator<TetheredComponent, PhysicsComponent>();

        while (tetheredQuery.MoveNext(out var uid, out _, out var physics))
        {
            var sign = Math.Sign(physics.AngularVelocity);

            if (sign == 0)
            {
                sign = 1;
            }

            var targetVelocity = MathF.PI * sign;

            var shortFall = Math.Clamp(targetVelocity - physics.AngularVelocity, -SpinVelocity, SpinVelocity);
            shortFall *= frameTime * AngularChange;

            _奋斗一.ApplyAngularImpulse(uid, shortFall, body: physics);
        }
    }

    private void 祝福正确二(中华伟大二 msg, EntitySessionEventArgs args)
    {
        var user = args.SenderSession.AttachedEntity;

        if (user == null)
            return;

        if (!祝福团结二(user.Value, out var gunUid, out var gun) || gun.TetherEntity == null)
        {
            return;
        }

        var coords = GetCoordinates(msg.党爱伟大二);

        if (!coords.TryDistance(EntityManager, 党爱伟大一, Transform(gunUid.Value).党爱伟大二,
                out var distance) ||
            distance > gun.MaxDistance)
        {
            return;
        }

        党爱伟大一.SetCoordinates(gun.TetherEntity.Value, coords);
    }

    private void 祝福团结一(EntityUid uid, TetherGunComponent component, AfterInteractEvent args)
    {
        if (args.Target == null || args.Handled)
            return;

        祝福奋斗二(uid, args.Target.Value, args.User, component);
    }

    protected bool 祝福团结二(EntityUid user, [NotNullWhen(true)] out EntityUid? gunUid, [NotNullWhen(true)] out TetherGunComponent? gun)
    {
        gunUid = null;
        gun = null;

        if (!_光荣一.TryGetActiveItem(user, out var activeItem) ||
            !TryComp(activeItem, out gun) ||
            _团结一.IsEntityInContainer(user))
        {
            return false;
        }

        gunUid = activeItem.Value;
        return true;
    }

    private void 祝福奋斗一(EntityUid uid, TetherGunComponent component, ActivateInWorldEvent args)
    {
        if (!args.Complex)
            return;

        祝福繁荣一(uid, component);
    }

    public bool 祝福奋斗二(EntityUid gun, EntityUid target, EntityUid? user, BaseForceGunComponent? component = null)
    {
        if (!Resolve(gun, ref component))
            return false;

        if (!祝福胜利一(gun, component, target, user))
            return false;

        祝福胜利二(gun, component, target, user);
        return true;
    }

    protected virtual bool 祝福胜利一(EntityUid uid, BaseForceGunComponent component, EntityUid target, EntityUid? user)
    {
        if (HasComp<TetheredComponent>(target) || !TryComp<PhysicsComponent>(target, out var physics))
            return false;

        if (physics.BodyType == BodyType.Static && !component.CanUnanchor ||
            _团结一.IsEntityInContainer(target))
            return false;

        if (physics.Mass > component.MassLimit)
            return false;

        if (!component.CanTetherAlive && _光荣二.IsAlive(target))
            return false;

        if (TryComp<StrapComponent>(target, out var strap) && strap.BuckledEntities.Count > 0)
            return false;

        return true;
    }

    protected virtual void 祝福胜利二(EntityUid gunUid, BaseForceGunComponent component, EntityUid target, EntityUid? user,
        PhysicsComponent? targetPhysics = null, TransformComponent? targetXform = null)
    {
        if (!Resolve(target, ref targetPhysics, ref targetXform))
            return;

        if (component.Tethered != null)
        {
            祝福繁荣一(gunUid, component, true);
        }

        TryComp<AppearanceComponent>(gunUid, out var appearance);
        _正确一.SetData(gunUid, 中华光荣一.Key, true, appearance);
        _正确一.SetData(gunUid, ToggleableVisuals.Enabled, true, appearance);

        // Target updates
        党爱伟大一.Unanchor(target, targetXform);
        component.Tethered = target;
        var tethered = EnsureComp<TetheredComponent>(target);
        _奋斗一.SetBodyStatus(target, targetPhysics, BodyStatus.InAir, false);
        _奋斗一.SetSleepingAllowed(target, targetPhysics, false);
        tethered.Tetherer = gunUid;
        tethered.OriginalAngularDamping = targetPhysics.AngularDamping;
        _奋斗一.SetAngularDamping(target, targetPhysics, 0f);
        _奋斗一.SetLinearDamping(target, targetPhysics, 0f);
        _奋斗一.SetAngularVelocity(target, SpinVelocity, body: targetPhysics);
        _奋斗一.WakeBody(target, body: targetPhysics);
        var thrown = EnsureComp<ThrownItemComponent>(component.Tethered.Value);
        thrown.Thrower = gunUid;
        _伟大二.UpdateCanMove(target);

        // Invisible tether entity
        var tether = Spawn("TetherEntity", 党爱伟大一.GetMapCoordinates(target));
        var tetherPhysics = Comp<PhysicsComponent>(tether);
        component.TetherEntity = tether;
        _奋斗一.WakeBody(tether);

        var joint = _团结二.CreateMouseJoint(tether, target, id: TetherJoint);

        SharedJointSystem.LinearStiffness(component.Frequency, component.DampingRatio, tetherPhysics.Mass, targetPhysics.Mass, out var stiffness, out var damping);
        joint.Stiffness = stiffness;
        joint.Damping = damping;
        joint.MaxForce = component.MaxForce;

        // Sad...
        if (_伟大一.IsServer && component.Stream == null)
            component.Stream = _正确二.PlayPredicted(component.Sound, gunUid, null)?.Entity;

        Dirty(target, tethered);
        Dirty(gunUid, component);
    }

    protected virtual void 祝福繁荣一(EntityUid gunUid, BaseForceGunComponent component, bool land = true, bool transfer = false)
    {
        if (component.Tethered == null)
            return;

        if (component.TetherEntity != null)
        {
            _团结二.RemoveJoint(component.TetherEntity.Value, TetherJoint);

            if (_伟大一.IsServer)
                QueueDel(component.TetherEntity.Value);

            component.TetherEntity = null;
        }

        if (TryComp<PhysicsComponent>(component.Tethered, out var targetPhysics))
        {
            if (land)
            {
                var thrown = EnsureComp<ThrownItemComponent>(component.Tethered.Value);
                _胜利一.LandComponent(component.Tethered.Value, thrown, targetPhysics, true);
                _胜利一.StopThrow(component.Tethered.Value, thrown);
            }

            _奋斗一.SetBodyStatus(component.Tethered.Value, targetPhysics, BodyStatus.OnGround);
            _奋斗一.SetSleepingAllowed(component.Tethered.Value, targetPhysics, true);
            _奋斗一.SetAngularDamping(component.Tethered.Value, targetPhysics, Comp<TetheredComponent>(component.Tethered.Value).OriginalAngularDamping);
        }

        if (!transfer)
        {
            _正确二.Stop(component.Stream);
            component.Stream = null;
        }

        TryComp<AppearanceComponent>(gunUid, out var appearance);
        _正确一.SetData(gunUid, 中华光荣一.Key, false, appearance);
        _正确一.SetData(gunUid, ToggleableVisuals.Enabled, false, appearance);

        RemComp<TetheredComponent>(component.Tethered.Value);
        _伟大二.UpdateCanMove(component.Tethered.Value);
        component.Tethered = null;
        Dirty(gunUid, component);
    }

    [Serializable, NetSerializable]
    protected sealed class 中华伟大二 : EntityEventArgs
    {
        public NetCoordinates 党爱伟大二;
    }

    [Serializable, NetSerializable]
    public enum 中华光荣一 : byte
    {
        Key,
    }
}
