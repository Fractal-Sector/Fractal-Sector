using System.Numerics;
using Content.Server.Shuttles.Components;
using Content.Shared._FS.Shuttles.Components;
using Robust.Shared.Physics.Components;
using Robust.Shared.Physics.Events;
using Robust.Shared.Physics.Systems;
using Robust.Shared.Timing;

namespace Content.Server._FS.Shuttles.System;

public sealed class BeaconBoostSystem : EntitySystem
{
    [Dependency] private readonly SharedPhysicsSystem _physics = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] private readonly IGameTiming _timing = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<BeaconBoostComponent, StartCollideEvent>(OnStartCollide);
        SubscribeLocalEvent<BeaconBoostComponent, EntParentChangedMessage>(OnParentChanged);
    }

    private void OnParentChanged(EntityUid uid, BeaconBoostComponent comp, ref EntParentChangedMessage args)
    {
        if (args.Transform.MapUid != null
            && args.Transform.ParentUid != args.Transform.MapUid)
        {
            _transform.SetParent(uid, args.Transform, args.Transform.MapUid.Value);
        }
    }

    private void OnStartCollide(EntityUid uid, BeaconBoostComponent comp, ref StartCollideEvent args)
    {
        var targetUid = args.OtherEntity;
        var gridUid = Transform(targetUid).GridUid;

        if (gridUid == null)
            return;

        if (!HasComp<ShuttleComponent>(gridUid.Value) || !TryComp<PhysicsComponent>(gridUid.Value, out var physics))
            return;

        var currentTime = _timing.CurTime;
        if (comp.LastBoostTimes.TryGetValue(gridUid.Value, out var lastTime))
        {
            if (currentTime - lastTime < TimeSpan.FromSeconds(comp.Cooldown))
                return;
        }
        comp.LastBoostTimes[gridUid.Value] = currentTime;

        var currentVelocity = physics.LinearVelocity;
        var currentSpeed = currentVelocity.Length();

        Vector2 direction;
        if (currentVelocity.Length() > 0.05f)
        {
            direction = Vector2.Normalize(currentVelocity);
        }
        else
        {
            var rotation = Transform(gridUid.Value).WorldRotation;
            direction = new Vector2(MathF.Cos((float)rotation), MathF.Sin((float)rotation));
        }

        var newVelocity = direction * (currentSpeed + comp.Boost);
        _physics.SetLinearVelocity(gridUid.Value, newVelocity, body: physics);
    }
}
