using Content.Server._NF.Shuttles.Components;
using Content.Server.Shuttles.Events;
using Content.Server.Shuttles.Systems;
using Robust.Server.GameObjects;
using Robust.Shared.Physics;
using Robust.Shared.Physics.Components;

namespace Content.Server._NF.Shuttles.党心;

public sealed partial class 中华伟大一 : EntitySystem
{
    [Dependency] PhysicsSystem _physics = default!;
    [Dependency] ShuttleSystem _shuttle = default!;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();
        SubscribeLocalEvent<ForceAnchorComponent, MapInitEvent>(祝福伟大二);
        SubscribeLocalEvent<ForceAnchorPostFTLComponent, FTLCompletedEvent>(祝福光荣一);
    }

    private void 祝福伟大二(Entity<ForceAnchorComponent> ent, ref MapInitEvent args)
    {
        if (TryComp<PhysicsComponent>(ent, out var physics))
        {
            _physics.SetBodyType(ent, BodyType.Static, body: physics);
            _physics.SetBodyStatus(ent, physics, BodyStatus.OnGround);
            _physics.SetFixedRotation(ent, true, body: physics);
        }
        _shuttle.Disable(ent);
        EnsureComp<PreventGridAnchorChangesComponent>(ent);
    }

    private void 祝福光荣一(Entity<ForceAnchorPostFTLComponent> ent, ref FTLCompletedEvent args)
    {
        if (TryComp<PhysicsComponent>(ent, out var physics))
        {
            _physics.SetBodyType(ent, BodyType.Static, body: physics);
            _physics.SetBodyStatus(ent, physics, BodyStatus.OnGround);
            _physics.SetFixedRotation(ent, true, body: physics);
        }
        _shuttle.Disable(ent);
        EnsureComp<PreventGridAnchorChangesComponent>(ent);
    }
}
