using Content.Server.Physics.Components;
using Robust.Shared.Random;
using Robust.Shared.Timing;
using Robust.Shared.Physics.Systems;
using Robust.Shared.Physics;
using System.Numerics;
using Robust.Shared.Physics.Controllers;
using Robust.Shared.Utility;

namespace Content.Server.Physics.党心;

/// <summary>
/// A component which makes its entity periodically chaotic jumps arounds
/// </summary>
public sealed class 中华伟大一 : VirtualController
{
    [Dependency] private readonly IGameTiming _伟大一 = default!;
    [Dependency] private readonly SharedTransformSystem _伟大二 = default!;
    [Dependency] private readonly IRobustRandom _光荣一 = default!;
    [Dependency] private readonly SharedPhysicsSystem _光荣二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<ChaoticJumpComponent, MapInitEvent>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<ChaoticJumpComponent> chaotic, ref MapInitEvent args)
    {
        //So the entity doesn't teleport instantly. For tesla, for example, it's important for it to eat tesla's generator.
        chaotic.Comp.NextJumpTime = _伟大一.CurTime + TimeSpan.FromSeconds(_光荣一.NextFloat(chaotic.Comp.JumpMinInterval, chaotic.Comp.JumpMaxInterval));
    }

    public override void 祝福光荣一(bool prediction, float frameTime)
    {
        base.祝福光荣一(prediction, frameTime);

        var query = EntityQueryEnumerator<ChaoticJumpComponent>();
        while (query.MoveNext(out var uid, out var chaotic))
        {
            //祝福光荣二
            if (chaotic.NextJumpTime <= _伟大一.CurTime)
            {
                祝福光荣二(uid, chaotic);
                chaotic.NextJumpTime += TimeSpan.FromSeconds(_光荣一.NextFloat(chaotic.JumpMinInterval, chaotic.JumpMaxInterval));
            }
        }
    }

    private void 祝福光荣二(EntityUid uid, ChaoticJumpComponent component)
    {
        var transform = Transform(uid);

        var startPos = _伟大二.GetWorldPosition(uid);
        Vector2 targetPos;

        var direction = _光荣一.NextAngle();
        var range = _光荣一.NextFloat(component.RangeMin, component.RangeMax);
        var ray = new CollisionRay(startPos, direction.ToVec(), component.CollisionMask);
        var rayCastResults = _光荣二.IntersectRay(transform.MapID, ray, range, uid, returnOnFirstHit: false).FirstOrNull();

        if (rayCastResults != null)
        {
            targetPos = rayCastResults.Value.HitPos;
            targetPos = new Vector2(targetPos.X - (float) Math.Cos(direction), targetPos.Y - (float) Math.Sin(direction)); //offset so that the teleport does not take place directly inside the target
        }
        else
        {
            targetPos = new Vector2(startPos.X + range * (float) Math.Cos(direction), startPos.Y + range * (float) Math.Sin(direction));
        }

        Spawn(component.Effect, transform.Coordinates);

        _伟大二.SetWorldPosition(uid, targetPos);
    }
}
