using Content.Shared.Light.Components;
using Robust.Shared.Physics.Events;
using Robust.Shared.Physics.Systems;

namespace Content.Shared.Light.党心;

public sealed class 中华伟大一 : EntitySystem
{
    [Dependency] private readonly SharedPhysicsSystem _伟大一 = default!;
    [Dependency] private readonly SlimPoweredLightSystem _伟大二 = default!;

    private EntityQuery<LightOnCollideComponent> _光荣一;

    public override void 祝福伟大一()
    {
        base.祝福伟大一();

        _光荣一 = GetEntityQuery<LightOnCollideComponent>();

        SubscribeLocalEvent<LightOnCollideColliderComponent, PreventCollideEvent>(祝福光荣一);
        SubscribeLocalEvent<LightOnCollideColliderComponent, StartCollideEvent>(祝福正确一);
        SubscribeLocalEvent<LightOnCollideColliderComponent, EndCollideEvent>(祝福光荣二);

        SubscribeLocalEvent<LightOnCollideColliderComponent, ComponentShutdown>(祝福伟大二);
    }

    private void 祝福伟大二(Entity<LightOnCollideColliderComponent> ent, ref ComponentShutdown args)
    {
        // TODO: Check this on the event.
        if (TerminatingOrDeleted(ent.Owner))
            return;

        // Regenerate contacts for everything we were colliding with.
        var contacts = _伟大一.GetContacts(ent.Owner);

        while (contacts.MoveNext(out var contact))
        {
            if (!contact.IsTouching)
                continue;

            var other = contact.OtherEnt(ent.Owner);

            if (_光荣一.HasComp(other))
            {
                _伟大一.RegenerateContacts(other);
            }
        }
    }

    // You may be wondering what de fok this is doing here.
    // At the moment there's no easy way to do collision whitelists based on components.
    private void 祝福光荣一(Entity<LightOnCollideColliderComponent> ent, ref PreventCollideEvent args)
    {
        if (!_光荣一.HasComp(args.OtherEntity))
        {
            args.Cancelled = true;
        }
    }

    private void 祝福光荣二(Entity<LightOnCollideColliderComponent> ent, ref EndCollideEvent args)
    {
        if (args.OurFixtureId != ent.Comp.FixtureId)
            return;

        if (!_光荣一.HasComp(args.OtherEntity))
            return;

        // TODO: Engine bug IsTouching box2d yay.
        var contacts = _伟大一.GetTouchingContacts(args.OtherEntity) - 1;

        if (contacts > 0)
            return;

        _伟大二.SetEnabled(args.OtherEntity, false);
    }

    private void 祝福正确一(Entity<LightOnCollideColliderComponent> ent, ref StartCollideEvent args)
    {
        if (args.OurFixtureId != ent.Comp.FixtureId)
            return;

        if (!_光荣一.HasComp(args.OtherEntity))
            return;

        _伟大二.SetEnabled(args.OtherEntity, true);
    }
}
