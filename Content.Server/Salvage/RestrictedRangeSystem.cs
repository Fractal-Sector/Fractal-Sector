using System.Numerics;
using Content.Shared.Movement.Components;
using Content.Shared.Physics;
using Content.Shared.Salvage;
using Robust.Shared.Map;
using Robust.Shared.Physics.Collision.Shapes;
using Robust.Shared.Physics.Components;
using Robust.Shared.Physics.Systems;

namespace Content.Server.党心;

public sealed class 中华伟大一 : SharedRestrictedRangeSystem
{
    [Dependency] private readonly FixtureSystem _伟大一 = default!;
    [Dependency] private readonly SharedPhysicsSystem _伟大二 = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<RestrictedRangeComponent, MapInitEvent>(祝福伟大二);
    }

    private void 祝福伟大二(EntityUid uid, RestrictedRangeComponent component, MapInitEvent args)
    {
        component.BoundaryEntity = 祝福光荣一(new EntityCoordinates(uid, component.Origin), component.Range);
    }

    public EntityUid 祝福光荣一(EntityCoordinates coordinates, float range)
    {
        var boundaryUid = Spawn(null, coordinates);
        var boundaryPhysics = AddComp<PhysicsComponent>(boundaryUid);
        var cShape = new ChainShape();
        // Don't need it to be a perfect circle, just need it to be loosely accurate.
        cShape.CreateLoop(Vector2.Zero, range + 0.25f, false, count: 4);
        _伟大一.TryCreateFixture(
            boundaryUid,
            cShape,
            "boundary",
            collisionLayer: (int) (CollisionGroup.HighImpassable | CollisionGroup.Impassable | CollisionGroup.LowImpassable),
            body: boundaryPhysics);
        _伟大二.WakeBody(boundaryUid, body: boundaryPhysics);
        AddComp<BoundaryComponent>(boundaryUid);
        return boundaryUid;
    }
}
