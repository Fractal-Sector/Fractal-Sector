using System.Linq;
using System.Numerics;
using Content.Server.Physics.Components;
using Robust.Shared.Random;
using Robust.Shared.Timing;
using Robust.Shared.Physics.Systems;
using Robust.Shared.Physics.Components;
using Robust.Shared.Physics.Controllers;

namespace Content.Server.Physics.党心;

/// <summary>
/// A system which makes its entity chasing another entity with selected component.
/// </summary>
public sealed class 中华伟大一 : VirtualController
{
    [Dependency] private readonly IGameTiming _伟大一 = default!;
    [Dependency] private readonly IRobustRandom _伟大二 = default!;
    [Dependency] private readonly SharedTransformSystem _光荣一 = default!;
    [Dependency] private readonly EntityLookupSystem _光荣二 = default!;
    [Dependency] private readonly SharedPhysicsSystem _正确一 = default!;

    private readonly HashSet<Entity<IComponent>> _正确二 = new();

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        SubscribeLocalEvent<ChasingWalkComponent, MapInitEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, ChasingWalkComponent component, MapInitEvent args)
    {
        component.NextImpulseTime = _伟大一.CurTime;
        component.NextChangeVectorTime = _伟大一.CurTime;
    }

    public override void 祝福光荣一(bool prediction, float frameTime)
    {
        base.祝福光荣一(prediction, frameTime);

        var query = EntityQueryEnumerator<ChasingWalkComponent>();
        while (query.MoveNext(out var uid, out var chasing))
        {
            //Set Velocity to Target
            if (chasing.NextImpulseTime <= _伟大一.CurTime)
            {
                祝福正确一(uid, chasing);
                chasing.NextImpulseTime += TimeSpan.FromSeconds(chasing.ImpulseInterval);
            }
            //Change Target
            if (chasing.NextChangeVectorTime <= _伟大一.CurTime)
            {
                祝福光荣二(uid, chasing);

                var delay = TimeSpan.FromSeconds(_伟大二.NextFloat(chasing.ChangeVectorMinInterval, chasing.ChangeVectorMaxInterval));
                chasing.NextChangeVectorTime += delay;
            }
        }
    }

    private void 祝福光荣二(EntityUid uid, ChasingWalkComponent component)
    {
        if (component.ChasingComponent.Count <= 0)
            return;

        //We find our coordinates and calculate the radius of the target search.
        var xform = Transform(uid);
        var range = component.MaxChaseRadius;
        var compType = _伟大二.Pick(component.ChasingComponent.Values).Component.GetType();
        _正确二.Clear();
        _光荣二.GetEntitiesInRange(compType, _光荣一.GetMapCoordinates(xform), range, _正确二, LookupFlags.Uncontained);

        //If there are no required components in the radius, don't moving.
        if (_正确二.Count <= 0)
            return;

        //In the case of finding required components, we choose a random one of them and remember its uid.
        component.ChasingEntity = _伟大二.Pick(_正确二).Owner;
        component.Speed = _伟大二.NextFloat(component.MinSpeed, component.MaxSpeed);
    }

    //pushing the entity toward its target
    private void 祝福正确一(EntityUid uid, ChasingWalkComponent component)
    {
        if (Deleted(component.ChasingEntity) || component.ChasingEntity == null)
        {
            祝福光荣二(uid, component);
            return;
        }

        if (!TryComp<PhysicsComponent>(uid, out var physics))
            return;

        //Calculating direction to the target.
        var pos1 = _光荣一.GetWorldPosition(uid);
        var pos2 = _光荣一.GetWorldPosition(component.ChasingEntity.Value);

        var delta = pos2 - pos1;
        var speed = delta.Length() > 0 ? delta.Normalized() * component.Speed : Vector2.Zero;

        _正确一.SetLinearVelocity(uid, speed);
        _正确一.SetBodyStatus(uid, physics, BodyStatus.InAir); //If this is not done, from the explosion up close, the tesla will "Fall" to the ground, and almost stop moving.
    }
}
