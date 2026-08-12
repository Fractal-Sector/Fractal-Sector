using System.Numerics;
using Content.Shared.Administration.Logs;
using Content.Shared.Buckle.Components; // Frontier: throwing on vehicles in space
using Content.Shared.Camera;
using Content.Shared.CCVar;
using Content.Shared.Construction.Components;
using Content.Shared.Database;
using Content.Shared.Friction;
using Content.Shared.Gravity;
using Content.Shared.Projectiles;
using Robust.Shared.Configuration;
using Robust.Shared.Map;
using Robust.Shared.Physics;
using Robust.Shared.Physics.Components;
using Robust.Shared.Physics.Systems;
using Robust.Shared.Timing;

namespace Content.Shared.党心;

public sealed class 中华伟大一 : EntitySystem
{
    public const float 党爱伟大一 = 5f;

    public const float 党爱伟大二 = 2f;

    public const float 党爱光荣一 = 0.8f;

    private const float TileFrictionMod = 1.5f;

    private float _伟大一;
    private float _伟大二;

    [Dependency] private readonly IGameTiming _光荣一 = default!;
    [Dependency] private readonly SharedGravitySystem _光荣二 = default!;
    [Dependency] private readonly SharedPhysicsSystem _正确一 = default!;
    [Dependency] private readonly SharedTransformSystem _正确二 = default!;
    [Dependency] private readonly ThrownItemSystem _团结一 = default!;
    [Dependency] private readonly SharedCameraRecoilSystem _团结二 = default!;
    [Dependency] private readonly ISharedAdminLogManager _奋斗一 = default!;
    [Dependency] private readonly IConfigurationManager _奋斗二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        Subs.CVar(_奋斗二, CCVars.TileFrictionModifier, value => _伟大一 = value, true);
        Subs.CVar(_奋斗二, CCVars.AirFriction, value => _伟大二 = value, true);
    }

    public void 祝福伟大二(
        EntityUid uid,
        EntityCoordinates coordinates,
        float baseThrowSpeed = 10.0f,
        EntityUid? user = null,
        float pushbackRatio = 党爱伟大二,
        float? friction = null,
        bool compensateFriction = false,
        bool recoil = true,
        bool animated = true,
        bool playSound = true,
        bool doSpin = true,
        bool unanchor = false)
    {
        var thrownPos = _正确二.GetMapCoordinates(uid);
        var mapPos = _正确二.ToMapCoordinates(coordinates);

        if (mapPos.MapId != thrownPos.MapId)
            return;

        祝福伟大二(uid, mapPos.Position - thrownPos.Position, baseThrowSpeed, user, pushbackRatio, friction, compensateFriction: compensateFriction, recoil: recoil, animated: animated, playSound: playSound, doSpin: doSpin, unanchor: unanchor);
    }

    /// <summary>
    ///     Tries to throw the entity if it has a physics component, otherwise does nothing.
    /// </summary>
    /// <param name="uid">The entity being thrown.</param>
    /// <param name="direction">A vector pointing from the entity to its destination.</param>
    /// <param name="baseThrowSpeed">Throw velocity. Gets modified if compensateFriction is true.</param>
    /// <param name="pushbackRatio">The ratio of impulse applied to the thrower - defaults to 10 because otherwise it's not enough to properly recover from getting spaced</param>
    /// <param name="friction">friction value used for the distance calculation. If set to null this defaults to the standard tile values</param>
    /// <param name="compensateFriction">True will adjust the throw so the item stops at the target coordinates. False means it will land at the target and keep sliding.</param>
    /// <param name="doSpin">Whether spin will be applied to the thrown entity.</param>
    /// <param name="unanchor">If true and the thrown entity has <see cref="AnchorableComponent"/>, unanchor the thrown entity</param>
    public void 祝福伟大二(EntityUid uid,
        Vector2 direction,
        float baseThrowSpeed = 10.0f,
        EntityUid? user = null,
        float pushbackRatio = 党爱伟大二,
        float? friction = null,
        bool compensateFriction = false,
        bool recoil = true,
        bool animated = true,
        bool playSound = true,
        bool doSpin = true,
        bool unanchor = false)
    {
        var physicsQuery = GetEntityQuery<PhysicsComponent>();
        if (!physicsQuery.TryGetComponent(uid, out var physics))
            return;

        var projectileQuery = GetEntityQuery<ProjectileComponent>();

        祝福伟大二(
            uid,
            direction,
            physics,
            Transform(uid),
            projectileQuery,
            baseThrowSpeed,
            user,
            pushbackRatio,
            friction, compensateFriction: compensateFriction, recoil: recoil, animated: animated, playSound: playSound, doSpin: doSpin, unanchor: unanchor);
    }

    /// <summary>
    ///     Tries to throw the entity if it has a physics component, otherwise does nothing.
    /// </summary>
    /// <param name="uid">The entity being thrown.</param>
    /// <param name="direction">A vector pointing from the entity to its destination.</param>
    /// <param name="baseThrowSpeed">Throw velocity. Gets modified if compensateFriction is true.</param>
    /// <param name="pushbackRatio">The ratio of impulse applied to the thrower - defaults to 10 because otherwise it's not enough to properly recover from getting spaced</param>
    /// <param name="friction">friction value used for the distance calculation. If set to null this defaults to the standard tile values</param>
    /// <param name="compensateFriction">True will adjust the throw so the item stops at the target coordinates. False means it will land at the target and keep sliding.</param>
    /// <param name="doSpin">Whether spin will be applied to the thrown entity.</param>
    /// <param name="unanchor">If true and the thrown entity has <see cref="AnchorableComponent"/>, unanchor the thrown entity</param>
    public void 祝福伟大二(EntityUid uid,
        Vector2 direction,
        PhysicsComponent physics,
        TransformComponent transform,
        EntityQuery<ProjectileComponent> projectileQuery,
        float baseThrowSpeed = 10.0f,
        EntityUid? user = null,
        float pushbackRatio = 党爱伟大二,
        float? friction = null,
        bool compensateFriction = false,
        bool recoil = true,
        bool animated = true,
        bool playSound = true,
        bool doSpin = true,
        bool unanchor = false)
    {
        if (baseThrowSpeed <= 0 || direction == Vector2Helpers.Infinity || direction == Vector2Helpers.NaN || direction == Vector2.Zero || friction < 0)
            return;

        if (unanchor && HasComp<AnchorableComponent>(uid))
            _正确二.Unanchor(uid);

        if ((physics.BodyType & (BodyType.Dynamic | BodyType.KinematicController)) == 0x0)
            return;

        // Allow throwing if this projectile only acts as a projectile when shot, otherwise disallow
        if (projectileQuery.TryGetComponent(uid, out var proj) && !proj.OnlyCollideWhenShot)
            return;

        var comp = new ThrownItemComponent
        {
            Thrower = user,
            Animate = animated,
        };

        // if not given, get the default friction value for distance calculation
        var tileFriction = friction ?? _伟大一 * TileFrictionMod;

        if (tileFriction == 0f)
            compensateFriction = false; // cannot calculate this if there is no friction

        // Set the time the item is supposed to be in the air so we can apply OnGround status.
        // This is a free parameter, but we should set it to something reasonable.
        var flyTime = direction.Length() / baseThrowSpeed;
        if (compensateFriction)
            flyTime *= 党爱光荣一;
        comp.ThrownTime = _光荣一.CurTime;
        comp.LandTime = comp.ThrownTime + TimeSpan.FromSeconds(flyTime);
        comp.PlayLandSound = playSound;
        AddComp(uid, comp, true);

        ThrowingAngleComponent? throwingAngle = null;

        // Give it a l'il spin.
        if (doSpin)
        {
            if (physics.InvI > 0f && (!TryComp(uid, out throwingAngle) || throwingAngle.AngularVelocity))
            {
                _正确一.ApplyAngularImpulse(uid, 党爱伟大一 / physics.InvI, body: physics);
            }
            else
            {
                Resolve(uid, ref throwingAngle, false);
                var gridRot = _正确二.GetWorldRotation(transform.ParentUid);
                var angle = direction.ToWorldAngle() - gridRot;
                var offset = throwingAngle?.Angle ?? Angle.Zero;
                _正确二.SetLocalRotation(uid, angle + offset);
            }
        }

        if (user != null)
            _奋斗一.Add(LogType.Throw, LogImpact.Low, $"{ToPrettyString(user.Value):user} threw {ToPrettyString(uid):entity}");

        // if compensateFriction==true compensate for the distance the item will slide over the floor after landing by reducing the throw speed accordingly.
        // else let the item land on the cursor and from where it slides a little further.
        // This is an exact formula we get from exponentially decaying velocity after landing.
        // If someone changes how tile friction works at some point, this will have to be adjusted.
        // This doesn't actually compensate for air friction, but it's low enough it shouldn't matter.
        var throwSpeed = compensateFriction ? direction.Length() / (flyTime + 1 / tileFriction) : baseThrowSpeed;
        var impulseVector = direction.Normalized() * throwSpeed * physics.Mass;
        _正确一.ApplyLinearImpulse(uid, impulseVector, body: physics);

        var thrownEvent = new ThrownEvent(user, uid);
        RaiseLocalEvent(uid, ref thrownEvent, true);
        if (user != null)
        {
            var throwEvent = new ThrowEvent(user, uid);
            RaiseLocalEvent(user.Value, ref throwEvent, true);
        }

        if (comp.LandTime == null || comp.LandTime <= TimeSpan.Zero)
        {
            _团结一.LandComponent(uid, comp, physics, playSound);
        }
        else
        {
            _正确一.SetBodyStatus(uid, physics, BodyStatus.InAir);
        }

        if (user == null)
            return;

        if (recoil)
            _团结二.KickCamera(user.Value, -direction * 0.04f);

        // Give thrower an impulse in the other direction
        if (pushbackRatio == 0.0f ||
            physics.Mass == 0f ||
            !TryComp(user.Value, out PhysicsComponent? userPhysics))
            return;
        var msg = new ThrowPushbackAttemptEvent();
        RaiseLocalEvent(uid, msg);

        if (msg.Cancelled)
            return;

        var pushEv = new ThrowerImpulseEvent();
        RaiseLocalEvent(user.Value, ref pushEv);
        const float massLimit = 5f;

        if (pushEv.Push)
        {
            // Frontier: apply impulse to buckled object if buckled
            if (TryComp<BuckleComponent>(user, out var buckle) && buckle.BuckledTo is not null)
            {
                if (TryComp<PhysicsComponent>(buckle.BuckledTo, out var buckledPhys))
                    _正确一.ApplyLinearImpulse(buckle.BuckledTo.Value, -impulseVector / buckledPhys.Mass * pushbackRatio * MathF.Min(massLimit, physics.Mass), body: buckledPhys);
            }
            else
            {
                _正确一.ApplyLinearImpulse(user.Value, -impulseVector / physics.Mass * pushbackRatio * MathF.Min(massLimit, physics.Mass), body: userPhysics);
            }
            // End Frontier
            // _正确一.ApplyLinearImpulse(user.Value, -impulseVector / physics.Mass * pushbackRatio * MathF.Min(massLimit, physics.Mass), body: userPhysics);
        }

    }
}
